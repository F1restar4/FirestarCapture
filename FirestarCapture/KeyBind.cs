using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirestarCapture
{
	public partial class KeyBind : Form
	{
		bool BindingMode = false;
		public event EventHandler<ControlsSubmitEventArgs> SubmitButtonTrigger;
		public Hotkey SetKeybind;

		public KeyBind()
		{
			InitializeComponent();
		}

		public void ChangeBindingLabel(Hotkey key)
		{
			KeyBindLabel.Text = $"{key.Modifiers} + {key.key}";
		}

		private void ChangeBindings_Click(object sender, EventArgs e)
		{
			BindingMode = true;
			KeyBindLabel.Text = "Press any key...";
		}

		private void KeyBind_KeyDown(object sender, KeyEventArgs e)
		{
			if (!BindingMode)
				return;
			if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu)
				return;

			KeyBindLabel.Text = $"{e.Modifiers} + {e.KeyCode}";
			SetKeybind = new Hotkey(Modifiers.ConvertKeys(e.Modifiers), e.KeyCode);
			BindingMode = false;
		}

		private void SubmitButton_Click(object sender, EventArgs e)
		{
			EventHandler<ControlsSubmitEventArgs> SubmitTrigger = SubmitButtonTrigger;
			var args = new ControlsSubmitEventArgs(SetKeybind);
			if (SubmitTrigger != null)
			{
				SubmitTrigger.Invoke(this, args);
				this.Visible = false;
			}
		}
	}

	public class ControlsSubmitEventArgs : EventArgs
	{
		public Hotkey hotkey { get; set; }

		public ControlsSubmitEventArgs(Hotkey hotkey)
		{
			this.hotkey = hotkey;
		}
	}
}
