using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace FirestarCapture
{
	public class Capture : IDisposable
	{
		Overlay Overlay;
		bool AvoidRecursion = false;
		Point FirstPoint;
		Point SecondPoint;
		Rectangle rect = new Rectangle();
		Pen pen = new Pen(Brushes.White, 10);
		bool MouseDown = false;
		SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

		public void Dispose()
		{
			pen.Dispose();
			brush.Dispose();
			Overlay.MouseDown -= ClickDown;
			Overlay.MouseUp -= ClickRelease;
			Overlay.KeyPress -= RegisterKey;
			Overlay.MouseMove -= UpdateRectangle;
			Overlay.Paint -= OnPaint;
			Overlay.Dispose();
			GC.Collect();
		}

		public void StartCapture()
		{
			Overlay = new Overlay();
			Overlay.Visible = true;
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
			SecondPoint = e.Location;
			Overlay.Visible = false;
			Overlay.Dispose();
			DoCapture();
		}

		private void ClickDown(object? sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			FirstPoint = e.Location;
			rect.Location = FirstPoint;
			MouseDown = true;
		}

		private void UpdateRectangle(object? sender, MouseEventArgs e)
		{
			if (!MouseDown)
				return;
			var X = Math.Min(FirstPoint.X, e.X);
			var Y = Math.Min(FirstPoint.Y, e.Y);
			var X2 = Math.Max(FirstPoint.X, e.X);
			var Y2 = Math.Max(FirstPoint.Y, e.Y);
			rect = Rectangle.FromLTRB(X, Y, X2, Y2);
			Overlay.Invalidate();
		}

		private void DoCapture()
		{
			int X = Math.Min(FirstPoint.X, SecondPoint.X);
			int Y = Math.Min(FirstPoint.Y, SecondPoint.Y);
			var width = Math.Abs(FirstPoint.X - SecondPoint.X);
			var height = Math.Abs(FirstPoint.Y - SecondPoint.Y);
			var img = new Bitmap(width, height);
			var gr = Graphics.FromImage(img);
			gr.CopyFromScreen(X, Y, 0, 0, img.Size);
			Clipboard.SetImage(img);
			this.Dispose();
		}
	}
}
