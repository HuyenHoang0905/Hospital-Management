using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines base class for QAT customize item painter.
    /// </summary>
    internal abstract class QatCustomizeItemPainter
    {
        public abstract void Paint(QatCustomizeItemRendererEventArgs e);
    }
}
