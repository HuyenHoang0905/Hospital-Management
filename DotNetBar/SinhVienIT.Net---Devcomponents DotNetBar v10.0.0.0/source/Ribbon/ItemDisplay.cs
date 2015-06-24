using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class for item display.
	/// </summary>
	internal class ItemDisplay
	{
		public ItemDisplay()
		{
		}

		public void Paint(ItemContainer container, ItemPaintArgs p)
		{
			foreach(BaseItem item in container.SubItems)
			{
				if(item.Visible && item.Displayed)
				{
                    if (item.BeginGroup)
                    {
                        if (p.Renderer != null)
                            p.Renderer.DrawItemContainerSeparator(new ItemContainerSeparatorRendererEventArgs(p.Graphics, container, item));
                    }
                    if (p.ClipRectangle.IsEmpty || p.ClipRectangle.IntersectsWith(item.DisplayRectangle))
                    {
                        Region oldClip = p.Graphics.Clip as Region;
                        p.Graphics.SetClip(item.DisplayRectangle, CombineMode.Intersect);
                        if (!p.Graphics.IsClipEmpty)
                            item.Paint(p);
                        p.Graphics.Clip = oldClip;
                        if (oldClip != null) oldClip.Dispose();
                    }
				}
			}
		}

        public void Paint(BaseItem container, ItemPaintArgs p)
        {
            foreach (BaseItem item in container.SubItems)
            {
                if (item.Visible && item.Displayed)
                {
                    if (p.ClipRectangle.IsEmpty || p.ClipRectangle.IntersectsWith(item.DisplayRectangle))
                    {
                        Region oldClip = p.Graphics.Clip; //.Clone() as Region;
                        p.Graphics.SetClip(item.DisplayRectangle, CombineMode.Intersect);
                        item.Paint(p);
                        p.Graphics.Clip = oldClip;
                        if (oldClip != null)
                            oldClip.Dispose();
                    }
                }
            }
        }
	}
}
