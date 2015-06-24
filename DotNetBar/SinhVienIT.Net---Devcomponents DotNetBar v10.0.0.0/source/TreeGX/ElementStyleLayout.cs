using System;
using System.Drawing;
using System.Collections;

#if TREEGX
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Represents the layout for the element style.
	/// </summary>
	internal class ElementStyleLayout
	{
		public ElementStyleLayout()
		{
		}

		/// <summary>
		/// Calculates size of an style element.
		/// </summary>
		/// <param name="style">Style to calculate size for.</param>
		/// <param name="defaultFont">Default font that will be used by style if style does not uses it's own font.</param>
		/// <returns>Size of the style element. At this time only Height memeber will be calculated.</returns>
		public static void CalculateStyleSize(ElementStyle style, Font defaultFont)
		{
			int height=defaultFont.Height;
            ElementStyle s = ElementStyleDisplay.GetElementStyle(style);
			if(s.Font!=null)
				height=s.Font.Height;
			
			if(s.PaintTopBorder)
			{
				height+=s.BorderTopWidth;
			}

			if(s.PaintBottomBorder)
			{
				height+=s.BorderBottomWidth;
			}
			
			height+=(s.MarginTop+style.MarginBottom);
			height+=(s.PaddingTop+s.PaddingBottom);
			
			style.SetSize(new Size(0,height));
		}

		/// <summary>
		/// Returns the total white space for a style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for</param>
		/// <returns></returns>
		public static int HorizontalStyleWhiteSpace(ElementStyle es)
		{
			return LeftWhiteSpace(es)+RightWhiteSpace(es);
		}

		/// <summary>
		/// Returns the total white space for a style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for.</param>
		/// <returns></returns>
		public static int VerticalStyleWhiteSpace(ElementStyle es)
		{
			return TopWhiteSpace(es)+BottomWhiteSpace(es);
		}

		/// <summary>
		/// Returns total white space for left side of the style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for.</param>
		/// <returns></returns>
		public static int LeftWhiteSpace(ElementStyle es)
		{
			return ElementStyleLayout.StyleSpacing(es,eSpacePart.Border | eSpacePart.Margin | eSpacePart.Padding,eStyleSide.Left);
		}

		/// <summary>
		/// Returns total white space for right side of the style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for.</param>
		/// <returns></returns>
		public static int RightWhiteSpace(ElementStyle es)
		{
			return ElementStyleLayout.StyleSpacing(es,eSpacePart.Border | eSpacePart.Margin | eSpacePart.Padding,eStyleSide.Right);
		}

		/// <summary>
		/// Returns total white space for top side of the style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for.</param>
		/// <returns></returns>
		public static int TopWhiteSpace(ElementStyle es)
		{
			return ElementStyleLayout.StyleSpacing(es,eSpacePart.Border | eSpacePart.Margin | eSpacePart.Padding,eStyleSide.Top);
		}

		/// <summary>
		/// Returns total white space for top side of the style. Whitespace is the space between the edge of the element and inner content of the element.
		/// </summary>
		/// <param name="es">Style to return white space for.</param>
		/// <returns></returns>
		public static int BottomWhiteSpace(ElementStyle es)
		{
			return ElementStyleLayout.StyleSpacing(es,eSpacePart.Border | eSpacePart.Margin | eSpacePart.Padding,eStyleSide.Bottom);
		}

		/// <summary>
		/// Returns amount of spacing for specified style parts.
		/// </summary>
		/// <param name="es">Style to calculate spacing for.</param>
		/// <param name="part">Part of the style spacing is calculated for. Values can be combined.</param>
		/// <param name="side">Side of the style to use for calculation.</param>
		/// <returns></returns>
		public static int StyleSpacing(ElementStyle style, eSpacePart part, eStyleSide side)
		{
            ElementStyle es = ElementStyleDisplay.GetElementStyle(style);
			int space=0;
			if((part & eSpacePart.Margin)==eSpacePart.Margin)
			{
				switch(side)
				{
					case eStyleSide.Bottom:
						space+=es.MarginBottom;
						break;
					case eStyleSide.Left:
						space+=es.MarginLeft;
						break;
					case eStyleSide.Right:
						space+=es.MarginRight;
						break;
					case eStyleSide.Top:
						space+=es.MarginTop;
						break;
				}
			}

			if((part & eSpacePart.Padding)==eSpacePart.Padding)
			{
				switch(side)
				{
					case eStyleSide.Bottom:
						space+=es.PaddingBottom;
						break;
					case eStyleSide.Left:
						space+=es.PaddingLeft;
						break;
					case eStyleSide.Right:
						space+=es.PaddingRight;
						break;
					case eStyleSide.Top:
						space+=es.PaddingTop;
						break;
				}
			}

			if((part & eSpacePart.Border)==eSpacePart.Border)
			{
				switch(side)
				{
					case eStyleSide.Bottom:
					{
						if(es.PaintBottomBorder)
							space+=es.BorderBottomWidth;
                        if (es.BorderBottom == eStyleBorderType.Etched || es.BorderBottom == eStyleBorderType.Double)
                            space += es.BorderBottomWidth;
						break;
					}
					case eStyleSide.Left:
					{
						if(es.PaintLeftBorder)
							space+=es.BorderLeftWidth;
                        if (es.BorderLeft == eStyleBorderType.Etched || es.BorderLeft == eStyleBorderType.Double)
                            space += es.BorderLeftWidth;
						break;
					}
					case eStyleSide.Right:
					{
						if(es.PaintRightBorder)
							space+=es.BorderRightWidth;
                        if (es.BorderRight == eStyleBorderType.Etched || es.BorderRight == eStyleBorderType.Double)
                            space += es.BorderRightWidth;
						break;
					}
					case eStyleSide.Top:
					{
						if(es.PaintTopBorder)
							space+=es.BorderTopWidth;
                        if (es.BorderTop == eStyleBorderType.Etched || es.BorderTop == eStyleBorderType.Double)
                            space += es.BorderTopWidth;
						break;
					}
				}
			}

			return space;
		}

        /// <summary>
        /// Gets inner rectangle taking in account style padding, margins and border.
        /// </summary>
        public static Rectangle GetInnerRect(ElementStyle style, Rectangle bounds)
        {
            bounds.X += LeftWhiteSpace(style);
            bounds.Width -= HorizontalStyleWhiteSpace(style);
            bounds.Y += TopWhiteSpace(style);
            bounds.Height -= VerticalStyleWhiteSpace(style);

            return bounds;
        }
	}
}
