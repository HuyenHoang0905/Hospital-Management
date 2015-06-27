using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    internal abstract class ItemContainerPainter
    {
        public abstract void PaintBackground(ItemContainerRendererEventArgs e);

        public abstract void PaintItemSeparator(ItemContainerSeparatorRendererEventArgs e);
    }
}
