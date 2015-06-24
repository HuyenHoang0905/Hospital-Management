using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines base class for QAT overflow item painter.
    /// </summary>
    internal abstract class QatOverflowPainter
    {
        public abstract void Paint(QatOverflowItemRendererEventArgs e);
    }
}
