using System;
using System.Drawing;

namespace DevComponents.AdvTree
{
	/// <summary>
	/// Data form RenderSelection event.
	/// </summary>
	public class SelectionRendererEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets reference to Graphics object, canvas node is rendered on.
		/// </summary>
		public System.Drawing.Graphics Graphics=null;
		/// <summary>
		/// Gets or sets the reference to selected Node object.
		/// </summary>
		public DevComponents.AdvTree.Node Node=null;
		/// <summary>
		/// Gets or sets the selection bounds.
		/// </summary>
		public Rectangle Bounds=Rectangle.Empty;
        /// <summary>
        /// Gets or sets the node selection box style.
        /// </summary>
        public eSelectionStyle SelectionBoxStyle = eSelectionStyle.HighlightCells;
        /// <summary>
        /// Gets or sets whether tree control is active, focused.
        /// </summary>
        public bool TreeActive = false;
	}
}
