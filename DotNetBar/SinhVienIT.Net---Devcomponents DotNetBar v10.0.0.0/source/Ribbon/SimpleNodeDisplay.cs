using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SimpleNodeDisplay.
	/// </summary>
	internal class SimpleNodeDisplay
	{
		public static void Paint(SimpleNodeDisplayInfo di)
		{
			Rectangle r=di.Element.Bounds;
			//di.Graphics.SetClip(r);

            Region oldClip = di.Graphics.Clip;
            if (oldClip != null)
                di.Graphics.SetClip(r, CombineMode.Intersect);
            else
                di.Graphics.SetClip(r, CombineMode.Replace);


			//r.Inflate(1,1);
			ElementStyleDisplayInfo displayInfo=new ElementStyleDisplayInfo(di.Style,di.Graphics,r);
			ElementStyleDisplay.Paint(displayInfo);
			di.Graphics.ResetClip();

			if(di.Element.ImageVisible)
				SimpleNodeDisplay.PaintImage(di);
			if(di.Element.TextVisible)
			{
				displayInfo.Bounds=(di.TextBounds.IsEmpty?di.Element.TextBounds:di.TextBounds);
                eTextFormat format = di.Style.TextFormat;
                if (di.RightToLeft)
                    format |= eTextFormat.RightToLeft;
				ElementStyleDisplay.PaintText(displayInfo,di.Element.Text,di.Font, false, format);
			}

            if (oldClip != null)
                di.Graphics.Clip = oldClip;
            else
                di.Graphics.ResetClip();
		}

		private static void PaintImage(SimpleNodeDisplayInfo di)
		{
			if(di.Element.ImageLayoutSize.IsEmpty || di.Element.Image==null)
				return;
			Rectangle r=di.Element.ImageBounds;

			di.Graphics.DrawImage(di.Element.Image,r.X+(r.Width-di.Element.Image.Width)/2,
				r.Y+(r.Height-di.Element.Image.Height)/2);
		}

//		private static void PaintText(SimpleNodeDisplayInfo di)
//		{
//			ISimpleElement e=di.Element;
//			if(e.Text=="" || e.TextBounds.IsEmpty || di.Style.TextColor.IsEmpty)
//				return;
//
//			Rectangle bounds=e.TextBounds;
//			
//			Font font=di.Style.Font;
//			if(font==null)
//				font=di.Font;
//
//			using(SolidBrush brush=new SolidBrush(di.Style.TextColor))
//			{
//				di.Graphics.DrawString(e.Text,font,brush,bounds,di.Style.StringFormat);
//			}
//		}
	}

	/// <summary>
	/// Represents information neccessary to paint the cell on canvas.
	/// </summary>
	internal class SimpleNodeDisplayInfo
	{
		public ElementStyle Style=null;
		public System.Drawing.Graphics Graphics=null;
		public ISimpleElement Element=null;
		public System.Drawing.Font Font=null;
		public Rectangle TextBounds=Rectangle.Empty;
        public bool RightToLeft = false;

		public SimpleNodeDisplayInfo(ElementStyle style, System.Drawing.Graphics g, ISimpleElement elem, Font font, bool rightToLeft)
		{
			this.Style=style;
			this.Graphics=g;
			this.Element=elem;
			this.Font=font;
            this.RightToLeft = rightToLeft;
		}
	}
}
