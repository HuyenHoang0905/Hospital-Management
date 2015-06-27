using System;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for SimpleElementLayout.
	/// </summary>
	internal class SimpleElementLayout
	{
		internal static void LayoutSimpleElement(SimpleElementLayoutInfo info)
		{
			Size textSize=Size.Empty;
			Font font=info.Font;
			int height=0;
			if(info.LayoutStyle.Font!=null)
				font=info.LayoutStyle.Font;

			// Calculate Text Width and Height
			if(info.Element.FixedWidth==0)
			{
				if(info.Element.TextVisible)
				{
					string text=info.Element.Text;
					if(text!="")
					{
						textSize=TextDrawing.MeasureString(info.Graphics,text,font);
						//textSize=info.Graphics.MeasureString(text,font);
                        if (info.LayoutStyle != null && !info.LayoutStyle.TextShadowColor.IsEmpty && !info.LayoutStyle.TextShadowOffset.IsEmpty)
                            textSize.Height += info.LayoutStyle.TextShadowOffset.Y;
					}
				}
			}
			else
			{
				int availTextWidth=info.Element.FixedWidth-
					ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);

				if(info.Element.ImageVisible)
					availTextWidth-=info.Element.ImageLayoutSize.Width;

				if(info.Element.TextVisible)
				{
					int elementHeight=font.Height;
                    if (info.LayoutStyle != null && !info.LayoutStyle.TextShadowColor.IsEmpty && !info.LayoutStyle.TextShadowOffset.IsEmpty)
                        elementHeight += info.LayoutStyle.TextShadowOffset.Y;

					if(info.LayoutStyle.WordWrap)
					{
						elementHeight=info.LayoutStyle.MaximumHeight-info.LayoutStyle.MarginTop-
						              info.LayoutStyle.MarginBottom-info.LayoutStyle.PaddingTop-info.LayoutStyle.PaddingBottom;
						if(availTextWidth>0)
						{
							if(elementHeight>0)
							{
								textSize=TextDrawing.MeasureString(info.Graphics,info.Element.Text,font,new Size(availTextWidth,elementHeight),info.LayoutStyle.TextFormat);
							}
							else
								textSize=TextDrawing.MeasureString(info.Graphics,info.Element.Text,font,availTextWidth,info.LayoutStyle.TextFormat);
						}
					}
					else
						textSize=new Size(availTextWidth,elementHeight);
				}
			}

			if(info.Element.TextVisible && !info.LayoutStyle.TextShadowColor.IsEmpty)
			{
				textSize.Height+=info.LayoutStyle.TextShadowOffset.Y;
			}
			height=textSize.Height;

			if(info.VerticalPartAlignment)
			{
				if(info.Element.ImageVisible && info.Element.ImageLayoutSize.Height>0)
					height+=info.Element.ImageLayoutSize.Height;
			}
			else
			{
				if(info.Element.ImageVisible && info.Element.ImageLayoutSize.Height>height)
					height=info.Element.ImageLayoutSize.Height;
			}

			Rectangle r=new Rectangle(info.Left+ElementStyleLayout.LeftWhiteSpace(info.LayoutStyle),
				info.Top+ElementStyleLayout.TopWhiteSpace(info.LayoutStyle)
				,info.Element.FixedWidth-ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle),height);

			if(r.Width==0)
			{
				if(info.VerticalPartAlignment)
				{
					if(info.Element.TextVisible)
						r.Width=textSize.Width;
					if(info.Element.ImageVisible && info.Element.ImageLayoutSize.Width>r.Width)
						r.Width=(info.Element.ImageLayoutSize.Width+info.Element.ImageTextSpacing);
				}
				else
				{
					if(info.Element.TextVisible)
						r.Width=textSize.Width;
					if(info.Element.ImageVisible && info.Element.ImageLayoutSize.Width>0)
						r.Width+=(info.Element.ImageLayoutSize.Width+info.Element.ImageTextSpacing);
				}
			}

			// Now that we have element bounds store them
			Rectangle rElementBounds=new Rectangle(info.Left,info.Top,info.Element.FixedWidth,r.Height+info.LayoutStyle.MarginTop+info.LayoutStyle.MarginBottom+info.LayoutStyle.PaddingTop+info.LayoutStyle.PaddingBottom);
			if(rElementBounds.Width==0)
				rElementBounds.Width=r.Width+ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
			info.Element.Bounds=rElementBounds;

			// Set Position of the image
			if(info.Element.ImageVisible && !info.Element.ImageLayoutSize.IsEmpty)
			{
				eVerticalAlign va=GetImageVerticalAlign(info.Element.ImageAlignment);
				eHorizontalAlign ha=GetImageHorizontalAlign(info.Element.ImageAlignment,info.LeftToRight);
				if(info.VerticalPartAlignment)
					info.Element.ImageBounds=AlignContentVertical(info.Element.ImageLayoutSize, ref r, ha, va, info.Element.ImageTextSpacing);
				else
					info.Element.ImageBounds=AlignContent(info.Element.ImageLayoutSize, ref r, ha, va, info.Element.ImageTextSpacing);
			}
			else
				info.Element.ImageBounds=Rectangle.Empty;

			
			// Set position of the text
			if(!textSize.IsEmpty)
				info.Element.TextBounds=r;
			else
				info.Element.TextBounds=Rectangle.Empty;
            
		}

		private static Rectangle AlignContent(System.Drawing.Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
		{
			Rectangle contentRect=new Rectangle(Point.Empty,contentSize);
			switch(horizAlign)
			{
				case eHorizontalAlign.Right:
				{
					contentRect.X=boundingRectangle.Right-contentRect.Width;
					boundingRectangle.Width-=(contentRect.Width+contentSpacing);
					break;
				}
					//case eHorizontalAlign.Left:
				default:
				{
					contentRect.X=boundingRectangle.X;
					boundingRectangle.X=contentRect.Right+contentSpacing;
					boundingRectangle.Width-=(contentRect.Width+contentSpacing);
					break;
				}
					//				case eHorizontalAlign.Center:
					//				{
					//					contentRect.X=boundingRectangle.X+(boundingRectangle.Width-contentRect.Width)/2;
					//					break;
					//				}
			}

			switch(vertAlign)
			{
				case eVerticalAlign.Top:
				{
					contentRect.Y=boundingRectangle.Y;
					break;
				}
				case eVerticalAlign.Middle:
				{
					contentRect.Y=boundingRectangle.Y+(boundingRectangle.Height-contentRect.Height)/2;
					break;
				}
				case eVerticalAlign.Bottom:
				{
					contentRect.Y=boundingRectangle.Bottom-contentRect.Height;
					break;
				}
			}

			return contentRect;
		}

		private static Rectangle AlignContentVertical(System.Drawing.Size contentSize, ref Rectangle boundingRectangle, eHorizontalAlign horizAlign, eVerticalAlign vertAlign, int contentSpacing)
		{
			Rectangle contentRect=new Rectangle(Point.Empty,contentSize);
			switch(horizAlign)
			{
				case eHorizontalAlign.Left:
				{
					contentRect.X=boundingRectangle.X;
					break;
				}
				case eHorizontalAlign.Right:
				{
					contentRect.X=boundingRectangle.Right-contentRect.Width;
					break;
				}
				case eHorizontalAlign.Center:
				{
					contentRect.X=boundingRectangle.X+(boundingRectangle.Width-contentRect.Width)/2;
					break;
				}
			}

			switch(vertAlign)
			{
				case eVerticalAlign.Bottom:
				{
					contentRect.Y=boundingRectangle.Bottom-contentRect.Height;
					boundingRectangle.Height-=(contentRect.Height+contentSpacing);
					break;
				}
					//case eVerticalAlign.Top:
				default:
				{
					contentRect.Y=boundingRectangle.Y;
					boundingRectangle.Y=contentRect.Bottom+contentSpacing;
					boundingRectangle.Height-=(contentRect.Height+contentSpacing);
					break;
				}
					//				case eVerticalAlign.Middle:
					//				{
					//					contentRect.Y=boundingRectangle.Y+(boundingRectangle.Height-contentRect.Height)/2;
					//					break;
					//				}
			}

			return contentRect;
		}

		private static eHorizontalAlign GetImageHorizontalAlign(eSimplePartAlignment align, bool leftToRight)
		{
			if(((align==eSimplePartAlignment.NearBottom || align==eSimplePartAlignment.NearCenter ||
				align==eSimplePartAlignment.NearTop) && leftToRight) ||
				((align==eSimplePartAlignment.FarBottom || align==eSimplePartAlignment.FarCenter ||
				align==eSimplePartAlignment.FarTop) && !leftToRight))
				return eHorizontalAlign.Left;
			else if(align==eSimplePartAlignment.CenterBottom || align==eSimplePartAlignment.CenterTop)
				return eHorizontalAlign.Center;
			return eHorizontalAlign.Right;
		}

		private static eVerticalAlign GetImageVerticalAlign(eSimplePartAlignment align)
		{
			eVerticalAlign va=eVerticalAlign.Middle;

			switch(align)
			{
				case eSimplePartAlignment.FarBottom:
				case eSimplePartAlignment.NearBottom:
				case eSimplePartAlignment.CenterBottom:
					va=eVerticalAlign.Bottom;
					break;
				case eSimplePartAlignment.FarTop:
				case eSimplePartAlignment.NearTop:
				case eSimplePartAlignment.CenterTop:
					va=eVerticalAlign.Top;
					break;
			}

			return va;
		}

		/// <summary>
		/// Indicates absolute vertical alignment of the content.
		/// </summary>
		private enum eVerticalAlign
		{
			/// <summary>
			/// Content is aligned to the top
			/// </summary>
			Top,
			/// <summary>
			/// Content is aligned in the middle
			/// </summary>
			Middle,
			/// <summary>
			/// Content is aligned at the bottom
			/// </summary>
			Bottom
		}

		/// <summary>
		/// Indicates absolute horizontal alignment
		/// </summary>
		private enum eHorizontalAlign
		{
			/// <summary>
			/// Content is left aligned
			/// </summary>
			Left,
			/// <summary>
			/// Content is centered
			/// </summary>
			Center,
			/// <summary>
			/// Content is right aligned
			/// </summary>
			Right
		}
	}

	/// <summary>Indicates alignment of a part of the cell like image or check box in relation to the text.</summary>
	public enum eSimplePartAlignment:int
	{
		/// <summary>
		/// Part is aligned to the left center of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearCenter=0,
		/// <summary>
		/// Part is aligned to the right center of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarCenter=1,
		/// <summary>
		/// Part is aligned to the top left of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearTop=2,
		/// <summary>Part is aligned above the text and centered.</summary>
		CenterTop=3,
		/// <summary>
		/// Part is aligned to the top right of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarTop=4,
		/// <summary>
		/// Part is aligned to the bottom left of the text assuming left-to-right
		/// orientation.
		/// </summary>
		NearBottom=5,
		/// <summary>Part is aligned below the text and centered.</summary>
		CenterBottom=6,
		/// <summary>
		/// Part is aligned to the bottom right of the text assuming left-to-right
		/// orientation.
		/// </summary>
		FarBottom=7
	}

	internal class SimpleElementLayoutInfo
	{
		public ISimpleElement Element=null;
		public System.Drawing.Graphics Graphics=null;
		public System.Drawing.Font Font=null;
		public int Left=0;
		public int Top=0;
		public ElementStyle LayoutStyle=null;
		public bool LeftToRight=true;
		public bool VerticalPartAlignment=false;
	}
}
