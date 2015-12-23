using System;
using Gtk;
using RPi;
using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.Components.Sensors.Temperature.Dht;
using KegeratorUI;
using System.Drawing;
using System.Collections.Generic;
using Gdk;
using System.Runtime.InteropServices;

public partial class MainWindow: Gtk.Window
{

	FlowMeter fm;
	FridgeController fc;


	public List<Pixbuf> BeerImages {
		get;//{ return _beerImages; }
		private set;// { _beerImages = value; }
	}

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		fm = new FlowMeter (ConnectorPin.P1Pin03.Input());
		fm.FlowChanged += FlowChanged;
		fc = new FridgeController (4, ConnectorPin.P1Pin05.Output (), (float)spn_OffTemp.Value,  (float)spn_OnTemp.Value);


		//Setup building the beer image list.
		try
		{

			var img = new Pixbuf ("8BitBeer.bmp");
			BeerImages = new List<Pixbuf> ();
			int beerwidth = 224;
			int beerheight = 224;

			//Builds list of beer images.
			for (int i = 0; i < 11; i++) 
			{
				BeerImages.Add(new Pixbuf (img, beerwidth * i,0, beerwidth, beerheight));
			}
			img_Beer.Pixbuf =BeerImages[0];
		}
		catch(Exception ex) 
		{
			
		}
	}

	/// <summary>
	/// Handles  flow change event on the flow meter. Does the ui updatey things.
	/// </summary>
	private void FlowChanged(object sender, EventArgs e)
	{
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit (); 
		a.RetVal = true;
	}

	protected void OnSpnOnTempValueChanged (object sender, EventArgs e)
	{
		fc.HighTemp = (float)spn_OnTemp.Value;
	}

	protected void OnSpnOffTempValueChanged (object sender, EventArgs e)
	{
		fc.LowTemp = (float)spn_OffTemp.Value;
	}


	protected void OnSettingsActionActivated (object sender, EventArgs e)
	{
		var ps = new PinSettings ();
		ConnectorPin FlowPin = IntToPin (ps.FlowPin);
		ConnectorPin TemperaturePin = IntToPin (ps.TemperaturePin);
		ps.Show ();
	}

	private ConnectorPin IntToPin(int pin)
	{
		ConnectorPin result;
		switch(pin)
		{
		case 11:
			result = ConnectorPin.P1Pin11;
			break;
		case 13:
			result = ConnectorPin.P1Pin13;
			break;
		case 7:
			result = ConnectorPin.P1Pin07;
			break;
		case 15:
			result = ConnectorPin.P1Pin15;
			break;
		case 16:
			result = ConnectorPin.P1Pin16;
			break;
		case 18:
			result = ConnectorPin.P1Pin18;
			break;
		case 22:
			result = ConnectorPin.P1Pin22;
			break;
		case 12:
			result = ConnectorPin.P1Pin12;
			break;
		default:
			throw new Exception ("Invalid pin selection");
			break;
		}
		return result;
	}
}
