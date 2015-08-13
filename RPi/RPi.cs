using System;
using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.Components.Sensors.Temperature.Dht;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace RPi
{
    public class FlowMeter : IDisposable
    {
        InputPinConfiguration flowSensorPin = ConnectorPin.P1Pin03.Input();
        public GpioConnection cn;

        public delegate void FlowChangedEventHandler(object sender, EventArgs e);
        public event FlowChangedEventHandler FlowChanged;

        //Flow Sensor variables
        private const double PINTS_IN_A_LITRE = 2.11338;
        private const int SECONDS_IN_A_MINUTE = 60;
        private const double MS_IN_SECOND = 1000.0;
        private const double MIN_HZ = 0.25;
        private const double MAX_HZ = 80;
        private bool lastFlowPinState = false;
        // Start low
        private bool pouring = false;
        private int lastPinChange = (int)(DateTime.Now.Ticks * 1000);
        private int pinChange;
        private int pinDelta = 0;
        private int hertz = 0;
        private int flow = 0;
        private int litresPoured = 0;
        private int pintsPoured = 0;
        private long clicks = 0;
        private long clickDelta = 0;
        private long lastClick = 0;
        private double thisPour = 0;
        private double totalPour = 0;
        public double PintsPoured
        {
            get { return litresPoured * PINTS_IN_A_LITRE; }
        }
        private double CurrentTime
        {
            get { return DateTime.Now.Ticks; }
        }

        public FlowMeter(InputPinConfiguration pin)
        {

            pinChange = lastPinChange;
            flowSensorPin = pin;
            try
            {
                cn = new GpioConnection(pin);
                cn.PinStatusChanged += PinStatusChanged;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed To Create FlowController");
                Console.WriteLine(ex.ToString());
            }


        }

        public void PinStatusChanged(object sender, PinStatusEventArgs e)
        {
            var pinState = cn.Pins[e.Configuration.Pin];
            clicks++;
            clickDelta = (long)Math.Max((CurrentTime - lastClick), 1);
            if (clickDelta < 1000)
            {
                hertz = (int)(MS_IN_SECOND / clickDelta);
                if (hertz > MIN_HZ && hertz < MAX_HZ)
                {
                    flow = (int)(hertz / (SECONDS_IN_A_MINUTE * 7.5));
                    var instPour = flow * (clickDelta / MS_IN_SECOND);
                    thisPour += instPour;
                    totalPour += instPour;
                }
            }
            lastClick = (long)CurrentTime;
            FlowChanged(this, new EventArgs());
        }

        public void Dispose()
        {
            cn.Close();
        }
    }

    public class FridgeController : IDisposable
    {
        /// <summary>
        /// Gets or sets the high temp.
        /// </summary>
        /// <value>The high temp.</value>
        public float HighTemp
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the low temp.
        /// </summary>
        /// <value>The low temp.</value>
        public float LowTemp
        {
            get;
            set;
        }

        private float _currentTemp;
        /// <summary>
        /// Current temperature, closer to previous temperature i guess.
        /// </summary>
        public float CurrentTemp
        {
            get { return _currentTemp; }
            set { _currentTemp = value; }
        }

        GpioConnection _cnFridge;
        OutputPinConfiguration _fridgePin;
        /// <summary>
        /// Initializes a ne
        /// </summary>
        /// <param name="pin">Pin. Pin that the temperature sensor is connected to.</param>
        public FridgeController(int sensorPin, OutputPinConfiguration fridgePin)
            : this(sensorPin, fridgePin, -1, 1)
        {

        }

        public FridgeController(int sensorPin, OutputPinConfiguration fridgePin, float lt, float ht)
        {

            HighTemp = ht;
            LowTemp = lt;
            this._fridgePin = fridgePin;
            try
            {
                _cnFridge = new GpioConnection(fridgePin);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed To Create FridgeController");
                Console.WriteLine(ex.ToString());
            }
        }

        public void Update()
        {
            Process p = new Process();
            var sbOutput = new StringBuilder();
            var sbError = new StringBuilder();
            p.OutputDataReceived += ((o, e) => sbOutput.Append(e.Data));
            p.ErrorDataReceived += ((o, e) => sbError.Append(e.Data));

            ProcessStartInfo psi = new ProcessStartInfo("sudo", "/Adafruit_DHT");
            psi.UseShellExecute = false;
            p.StartInfo = psi;
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit(); // Wait for DHT to return temperature.

            var matches = Regex.Match(sbOutput.ToString(), @"Temp =\+[D-9.]+");

#if DEBUG
            foreach (var match in matches.Groups)
            {
                Console.WriteLine(match.ToString());
            }
#endif
            if (matches.Groups.Count > 1)
            {
                CurrentTemp = float.Parse(matches.Groups[1].Value);
            }

            if (CurrentTemp >= HighTemp)
            {
                //Turn on
                _cnFridge.Toggle(_fridgePin);
            }
            else if (CurrentTemp <= LowTemp)
            {
                //Turn off
                _cnFridge.Toggle(_fridgePin);
            }

        }


        public void Dispose()
        {
            _cnFridge.Close();
        }
    }
}

