using System;
using System.Collections;
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class for default layout of the BubbleButton objects.
	/// </summary>
	public class BubbleButtonLayoutManager:BlockLayoutManager
	{
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public BubbleButtonLayoutManager()
		{
		}

		/// <summary>
		/// Resizes the content block and sets it's Bounds property to reflect new size.
		/// </summary>
		/// <param name="block">Content block to resize.</param>
        public override void Layout(IBlock block, Size availableSize)
		{
			BubbleButton button=block as BubbleButton;
			BubbleBar bar=button.GetBubbleBar();
			if(bar==null)
			{
				if(button.Site!=null && button.Site.DesignMode)
					return;
				throw new InvalidOperationException("BubbleBar object could not be found for button named: '"+button.Name+"' in BubbleButtonLayoutManager.Layout");
			}
			Size defaultSize=bar.ImageSizeNormal;
			button.SetDisplayRectangle(new Rectangle(button.DisplayRectangle.Location,defaultSize));
		}

        public override Rectangle FinalizeLayout(Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
        {
            return (blocksBounds);
        }
	}
}
