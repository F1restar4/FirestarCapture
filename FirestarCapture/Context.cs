
using System.Reflection;

namespace FirestarCapture
{
	public class Context : ApplicationContext
	{
		NotifyIcon icon;
		ShortcutHandler shortcutHandler = new ShortcutHandler();
		KeyBind Keybindingsform = new KeyBind();
		bool OpenOnStartEnabled = false;
		DateTime LastTrigger = DateTime.Now;
		readonly string AutoStartupFileLoc = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\FirestarCapture.bat";

		public Context()
		{
			OpenOnStartEnabled = IsAutoStartupEnabled();
			var strip = new ContextMenuStrip();

			var item = new ToolStripMenuItem("Capture")
			{
				Name = "Capture"
			};
			item.Click += CaptureClick;
			strip.Items.Add(item);

			item = new ToolStripMenuItem("Change binding")
			{
				Name = "ChangeBinding"
			};
			item.Click += (s, e) =>
			{
				if (Keybindingsform.IsDisposed)
				{
					Keybindingsform = new KeyBind();
					Keybindingsform.SubmitButtonTrigger += SubmitKeybinding;
				}
				Keybindingsform.ChangeBindingLabel(shortcutHandler.hotkey);
				Keybindingsform.SetKeybind = shortcutHandler.hotkey;
				Keybindingsform.Show();
			};
			
			strip.Items.Add(item);

			item = new ToolStripMenuItem("Open on startup")
			{
				Name = "OpenOnStartup",
				Checked = OpenOnStartEnabled,
			};
			item.Click += (s, e) =>
			{
				var self = s as ToolStripMenuItem;
				self.Checked = !self.Checked;
				ToggleStartupShortcut();
			};
			strip.Items.Add(item);

			item = new ToolStripMenuItem("Exit")
			{
				Name = "Exit"
			};
			item.Click += ExitClick;
			strip.Items.Add(item);

			icon = new NotifyIcon()
			{
				Icon = Properties.Resources.Alex_Headshot,
				ContextMenuStrip = strip,
				Visible = true,
				Text = "FirestarCapture"
			};

			shortcutHandler.RegisterHotkey();
			shortcutHandler.TriggerOnKey += CaptureClick;
			Keybindingsform.SubmitButtonTrigger += SubmitKeybinding;
			this.ThreadExit += ExitThread;
		}
		
		public void CaptureClick(object? sender, EventArgs e)
		{
			if (DateTime.Now.Subtract(LastTrigger).TotalMilliseconds < 500)
				return;
			LastTrigger = DateTime.Now;
			var Capture = new Capture();
			Capture.StartCapture();
		}

		private void ExitClick(object? sender, EventArgs e)
		{
			icon.Visible = false;
			Application.Exit();
		}

		private void ExitThread(object? sender, EventArgs e)
		{
			shortcutHandler.UnRegisterHotkey();
		}

		private void SubmitKeybinding(object? sender, ControlsSubmitEventArgs e)
		{
			shortcutHandler.UnRegisterHotkey();
			shortcutHandler.hotkey = e.hotkey;
			shortcutHandler.RegisterHotkey();
			File.WriteAllText("keybind.txt", $"{(int)e.hotkey.Modifiers}\n{(int)e.hotkey.key}");
		}

		private void ToggleStartupShortcut()
		{
			if (OpenOnStartEnabled)
				File.Delete(AutoStartupFileLoc);
			else
				File.WriteAllText(AutoStartupFileLoc, $"start /d \"{Environment.CurrentDirectory}\" FirestarCapture.exe");

			OpenOnStartEnabled = IsAutoStartupEnabled();
		}

		private bool IsAutoStartupEnabled()
			=> File.Exists(AutoStartupFileLoc);
	}

	public class ShortcutHandler : Form
	{
		public Hotkey hotkey = new Hotkey(KeyModifiers.CONTROL | KeyModifiers.SHIFT, Keys.C);
		public event EventHandler TriggerOnKey;
		public DateTime LastTrigger = DateTime.Now;

		public void RegisterHotkey()
		{
			var result = KeyboardHook.RegisterHotKey(this.Handle, 0, Convert.ToUInt32(hotkey.Modifiers), Convert.ToUInt32(hotkey.key));
		}

		public void UnRegisterHotkey()
		{
			KeyboardHook.UnregisterHotKey(this.Handle, 0);
		}

		public ShortcutHandler()
		{
			Visible = false;
			if (!File.Exists("keybind.txt"))
			{
				File.WriteAllText("keybind.txt", $"{(int)hotkey.Modifiers}\n{(int)hotkey.key}");
				return;
			}
			var data = File.ReadAllLines("keybind.txt");
			KeyModifiers mod = (KeyModifiers)Convert.ToInt32(data[0]);
			Keys key = (Keys)Convert.ToInt32(data[1]);
			hotkey = new Hotkey(mod, key);
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg != 0x0312)
				return;
			Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF); 
			KeyModifiers modifier = (KeyModifiers)((int)m.LParam & 0xFFFF);
			if (key == hotkey.key && modifier == hotkey.Modifiers)
			{
				TriggerOnKey?.Invoke(this, new EventArgs());
			}

		}
	}
}
