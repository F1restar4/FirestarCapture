using System.Runtime.InteropServices;

namespace FirestarCapture
{
	public static class KeyboardHook
	{
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsmodifiers, uint vk);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hwnd, int id);
	}

	public class Hotkey
	{
		public Hotkey(KeyModifiers mod, Keys key)
		{
			this.Modifiers = mod;
			this.key = key;
		}
		public KeyModifiers Modifiers { get; }
		public Keys key { get; }
		
	}

	public class Modifiers
	{
		public static KeyModifiers ConvertKeys(Keys key)
		{
			var outkey = new KeyModifiers();
			if (key.HasFlag(Keys.Control))
				outkey = KeyModifiers.CONTROL;
			if (key.HasFlag(Keys.Alt))
				outkey = outkey | KeyModifiers.ALT;
			if (key.HasFlag(Keys.Shift))
				outkey = outkey | KeyModifiers.SHIFT;
			return outkey;
		}
	}

	[Flags]
	public enum KeyModifiers
	{
		None = 0x0000,
		ALT = 0x0001,
		CONTROL = 0x0002,
		NOREPEAT = 0x4000,
		SHIFT = 0x0004,
		WIN = 0x0008
	}

}
