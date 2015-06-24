using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;

namespace DevComponents.AdvTree.Interop
{
	/// <summary>
	/// Provides WinApi functions to rest of the application.
	/// </summary>
	internal class WinApi
	{
	
		#region API Calls Declaration
		
		[DllImport("user32")]
		private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT tme);
		
		[StructLayout(LayoutKind.Sequential)]
		private struct TRACKMOUSEEVENT 
		{
			public int cbSize;
			public uint dwFlags;
			public int dwHoverTime;
			public IntPtr hwndTrack;
		}
		
		// Track Mouse Event Flags
		private const uint
			TME_HOVER=0x00000001,
			TME_LEAVE=0x00000002,
			TME_NONCLIENT=0x00000010,
			TME_QUERY=0x40000000,
			TME_CANCEL=0x80000000,
			HOVER_DEFAULT=0xFFFFFFFF;

		#endregion
		
		#region Functions
		/// <summary>
		/// Resets Hoover timer for specified control.
		/// </summary>
		public static void ResetHover(System.Windows.Forms.Control c)
		{
			if (c==null || !c.IsHandleCreated)
				return;
			
			// We need to reset hover thing since it is fired only first time mouse hovers inside the window and we need it for each of our items
			TRACKMOUSEEVENT tme = new TRACKMOUSEEVENT();
			tme.dwFlags = TME_QUERY;
			tme.hwndTrack = c.Handle;
			tme.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(tme);
			TrackMouseEvent(ref tme);
			tme.dwFlags = tme.dwFlags | TME_HOVER;
			TrackMouseEvent(ref tme);
		}
		#endregion
	}
}
