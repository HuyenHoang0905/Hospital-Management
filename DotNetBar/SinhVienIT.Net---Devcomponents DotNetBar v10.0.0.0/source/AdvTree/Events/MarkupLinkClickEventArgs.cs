using System;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Provides more information about MarkupLinkClick event.
	/// </summary>
	public class MarkupLinkClickEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the value of href attribute from the markup link that was clicked.
		/// </summary>
		public readonly string HRef = "";

		/// <summary>
		/// Gets the value of name attribute from the markup link that was clicked.
		/// </summary>
		public readonly string Name = "";

		/// <summary>
		/// Creates new instance of the object.
		/// </summary>
		/// <param name="name">Value of name attribute.</param>
		/// <param name="href">Value of href attribute.</param>
		public MarkupLinkClickEventArgs(string name, string href)
		{
			this.HRef = href;
			this.Name = name;
		}
	}

	/// <summary>
	/// Defines delegate for MarkupLinkClick event.
	/// </summary>
	public delegate void MarkupLinkClickEventHandler(object sender, MarkupLinkClickEventArgs e);
}
