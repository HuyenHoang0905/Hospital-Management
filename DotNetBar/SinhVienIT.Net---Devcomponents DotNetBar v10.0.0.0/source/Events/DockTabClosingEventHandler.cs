using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents event arguments for DockTabClosing event.
	/// </summary>
	public class DockTabClosingEventArgs:EventArgs
	{
		/// <summary>
		/// Indicates the DockContainerItem that is about to close.
		/// </summary>
		public readonly DockContainerItem DockContainerItem;
		/// <summary>
		/// Provides ability to cancel closing of the DockContainerItem. Default value is false.
		/// </summary>
		public bool Cancel=false;
		/// <summary>
		/// Set to true to automatically remove DockContainerItem from the Bar.Items collection after it is closed. Default value is false
		/// which means that DockContainerItem will be kept in collection but it will be hidden after this event is complete.
		/// </summary>
		public bool RemoveDockTab=false;
		/// <summary>
		/// Returns source of the event: keyboard, mouse or code.
		/// </summary>
		public readonly eEventSource Source;

		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		/// <param name="item">Reference to DockContainerItem that is about to close</param>
		public DockTabClosingEventArgs(DockContainerItem item, eEventSource source)
		{
			this.DockContainerItem=item;
			this.Source=source;
		}
	}

	/// <summary>
	/// Delegate for DockTabClosing event.
	/// </summary>
	public delegate void DockTabClosingEventHandler(object sender, DockTabClosingEventArgs e);
}
