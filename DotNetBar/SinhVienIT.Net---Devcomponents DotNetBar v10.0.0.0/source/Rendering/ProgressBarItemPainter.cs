using System;
using System.Text;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines abstract class for the ProgressBarItem painter.
    /// </summary>
    internal abstract class ProgressBarItemPainter
    {
        public abstract void Paint(ProgressBarItemRenderEventArgs e);
    }
}
