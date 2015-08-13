using System;
using Gtk;
using System.Configuration;

namespace KegeratorUI
{
	public partial class PinSettings : Gtk.Dialog
	{
		public int FlowPin
		{
			get;
			set;
		}
		public int TemperaturePin
		{
			get;
			set;
		}
		public PinSettings ()
		{
			this.Build ();
			Configuration cfg = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
			var fp = cfg.AppSettings.Settings ["FlowPin"];
			FlowPin = Convert.ToInt32 (fp.Value);
			ent_Flow.Text = FlowPin.ToString();

			var tp = cfg.AppSettings.Settings ["TemperaturePin"];
			TemperaturePin = Convert.ToInt32 (tp.Value);
			ent_Temperature.Text = TemperaturePin.ToString();

		}

		protected void OnEntFlowEditingDone (object sender, EventArgs e)
		{
			var entFlow = sender as Entry;
			FlowPin = Convert.ToInt32 (entFlow);

		}

		protected void OnEntTemperatureEditingDone (object sender, EventArgs e)
		{
			var entTemperature = sender as Entry;
			TemperaturePin = Convert.ToInt32(entTemperature.Text);
		}

		protected void OnClose(object sender, EventArgs e)
		{
			Configuration cfg = ConfigurationManager.OpenExeConfiguration (ConfigurationUserLevel.None);
			cfg.AppSettings.Settings ["FlowPin"].Value = FlowPin.ToString ();
			cfg.AppSettings.Settings ["TemperaturePin"].Value = TemperaturePin.ToString ();
			cfg.Save (ConfigurationSaveMode.Modified, true);
		}

	}
}

