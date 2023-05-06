using FirestarCapture.Properties;
using System.Drawing.Imaging;

namespace FirestarCapture
{
	public class Capture : IDisposable
	{
		Overlay Overlay;
		Point FirstPoint;
		Point SecondPoint;
		Point RelativePoint;
		Rectangle rect = new Rectangle();
		bool MouseDown = false;
		SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

		public void Dispose()
		{
			Overlay.Dispose();
			brush.Dispose();
		}

		public void StartCapture()
		{
			Overlay = new Overlay();
			var target = Screen.FromPoint(Cursor.Position);
			Overlay.Location = new Point(target.WorkingArea.Left, target.WorkingArea.Top);
			Overlay.Size = new Size(target.WorkingArea.Width, target.WorkingArea.Height);
			Overlay.Icon = Resources.Alex_Headshot;
			Overlay.Visible = true;
			Overlay.LostFocus += (sender, e) => Overlay.Focus();
			Overlay.MouseDown += ClickDown;
			Overlay.MouseUp += ClickRelease;
			Overlay.KeyPress += RegisterKey;
			Overlay.MouseMove += UpdateRectangle;
			Overlay.Paint += OnPaint;

		}

		private void RegisterKey(object? sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)27)
			{
				Overlay.Visible = false;
				Overlay.Dispose();
			}
		}

		private void OnPaint(object? sender, PaintEventArgs e)
		{
			if (!MouseDown)
				return;
			e.Graphics.Clear(Color.Transparent);
			e.Graphics.FillRectangle(brush, rect);
		}

		private void ClickRelease(object? sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			MouseDown = false;
			SecondPoint = Cursor.Position;
			Overlay.Visible = false;
			Overlay.Dispose();
			DoCapture();
		}

		private void ClickDown(object? sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			FirstPoint = Cursor.Position;
			RelativePoint = e.Location;
			rect.Location = RelativePoint;
			MouseDown = true;
		}

		private void UpdateRectangle(object? sender, MouseEventArgs e)
		{
			if (!MouseDown)
				return;
			var X = Math.Min(RelativePoint.X, e.X);
			var Y = Math.Min(RelativePoint.Y, e.Y);
			var X2 = Math.Max(RelativePoint.X, e.X);
			var Y2 = Math.Max(RelativePoint.Y, e.Y);
			rect = Rectangle.FromLTRB(X, Y, X2, Y2);
			Overlay.Invalidate();
		}

		private void DoCapture()
		{
			int X = Math.Min(FirstPoint.X, SecondPoint.X);
			int Y = Math.Min(FirstPoint.Y, SecondPoint.Y);
			var width = Math.Abs(FirstPoint.X - SecondPoint.X);
			var height = Math.Abs(FirstPoint.Y - SecondPoint.Y);
			if (width == 0 || height == 0)
				return;
			var img = new Bitmap(width, height, PixelFormat.Format32bppRgb);
			var gr = Graphics.FromImage(img);

			gr.CopyFromScreen(X, Y, 0, 0, img.Size);
			Clipboard.SetImage(img);
			this.Dispose();
		}
	}
}
