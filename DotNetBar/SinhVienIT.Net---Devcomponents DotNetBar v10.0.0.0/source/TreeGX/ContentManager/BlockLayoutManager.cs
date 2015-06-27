using System.Collections;
using System.Drawing;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.UI.ContentManager
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
	/// <summary>
	/// Represents block layout manager responsible for sizing the content blocks.
	/// </summary>
	public abstract class BlockLayoutManager
	{
		private Graphics m_Graphics;

	    /// <summary>
		/// Resizes the content block and sets it's Bounds property to reflect new size.
		/// </summary>
		/// <param name="block">Content block to resize.</param>
        /// <param name="availableSize">Content size available for the block in the given line.</param>
		public abstract void Layout(IBlock block, Size availableSize);

        /// <summary>
        /// Performs layout finalization
        /// </summary>
        /// <param name="containerBounds"></param>
        /// <param name="blocksBounds"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public abstract Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines);

		/// <summary>
		/// Gets or sets the graphics object used by layout manager.
		/// </summary>
		public System.Drawing.Graphics Graphics
		{
			get {return m_Graphics;}
			set {m_Graphics=value;}
		}
	}
}
