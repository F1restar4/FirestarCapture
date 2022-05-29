
namespace FirestarCapture
{
	public class Context : ApplicationContext
	{
		NotifyIcon icon;

		public Context()
		{
			var strip = new ContextMenuStrip();

			var item = new ToolStripMenuItem("Capture")
			{
				Name = "Exit"
			};
			item.Click += CaptureClick;
			strip.Items.Add(item);

			item = new ToolStripMenuItem("Exit")
			{
				Name = "Exit"
			};
			item.Click += ExitClick;
			strip.Items.Add(item);

			icon = new NotifyIcon()
			{
				Icon = Properties.Resources.test,
				ContextMenuStrip = strip,
				Visible = true,
				Text = "FirestarCapture"
			};
			
		}

		public void CaptureClick(object? sender, EventArgs e)
		{
			var Capture = new Capture();
			Capture.StartCapture();
		}

		private void ExitClick(object? sender, EventArgs e)
		{
			icon.Visible = false;
			Application.Exit();
		}
	}
}
