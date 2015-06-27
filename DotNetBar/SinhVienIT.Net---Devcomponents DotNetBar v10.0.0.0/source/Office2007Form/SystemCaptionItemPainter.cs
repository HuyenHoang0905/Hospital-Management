using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the base class for the SystemCaptionItem painter.
    /// </summary>
    internal class SystemCaptionItemPainter
    {
        public virtual void Paint(SystemCaptionItemRendererEventArgs e)
        {
            if (e.SystemCaptionItem.IsSystemIcon)
                PaintSystemIcon(e);
            else
                PaintFormButtons(e);
        }

        public virtual void PaintSystemIcon(SystemCaptionItemRendererEventArgs e) {}

        public virtual void PaintFormButtons(SystemCaptionItemRendererEventArgs e) { }
    }
}
