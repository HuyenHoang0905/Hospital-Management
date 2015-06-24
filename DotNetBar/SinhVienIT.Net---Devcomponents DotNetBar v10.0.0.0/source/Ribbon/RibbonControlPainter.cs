using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents base class for Ribbon Control painting.
    /// </summary>
    internal class RibbonControlPainter
    {
        /// <summary>
        /// Paints controls background
        /// </summary>
        public virtual void PaintBackground(RibbonControlRendererEventArgs e)
        {
        }

        /// <summary>
        /// Paints form caption background
        /// </summary>
        public virtual void PaintCaptionBackground(RibbonControlRendererEventArgs e, Rectangle displayRect)
        {
        }

        /// <summary>
        /// Paints form caption text when ribbon control is displaying form caption
        /// </summary>
        public virtual void PaintCaptionText(RibbonControlRendererEventArgs e) { }

        /// <summary>
        /// Paints the background of quick access toolbar.
        /// </summary>
        public virtual void PaintQuickAccessToolbarBackground(RibbonControlRendererEventArgs e) { }
    }
}
