using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

#if TREEGX
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Paints the ElementStyle
	/// </summary>
	public class ElementStyleDisplay
	{
		/// <summary>Creates new instance of the object.</summary>
		public ElementStyleDisplay()
		{
		}

        /// <summary>
        /// Paints text for given style.
        /// </summary>
        /// <param name="e">Display information.</param>
        /// <param name="text">Text to paint.</param>
        /// <param name="defaultFont">Default font if no font by style is specified.</param>
        public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont)
        {
            PaintText(e, text, defaultFont, false);
        }

		/// <summary>
        /// Paints text for given style.
		/// </summary>
        /// <param name="e">Display information.</param>
        /// <param name="text"><Text to paint./param>
        /// <param name="defaultFont">Default font if no font by style is specified.</param>
		/// <param name="useDefaultFont">Specifies whether to use default font for painting regardless of style settings.</param>
		public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont)
		{
            PaintText(e, text, defaultFont, useDefaultFont, e.Style.TextFormat);
		}

        /// <summary>
        /// Paints text for given style.
        /// </summary>
        /// <param name="e">Display information.</param>
        /// <param name="text"><Text to paint./param>
        /// <param name="defaultFont">Default font if no font by style is specified.</param>
        /// <param name="useDefaultFont">Specifies whether to use default font for painting regardless of style settings.</param>
        public static void PaintText(ElementStyleDisplayInfo e, string text, Font defaultFont, bool useDefaultFont, eTextFormat textFormat)
        {
            Rectangle textBounds = e.Bounds;
            ElementStyle style = GetElementStyle(e.Style);

            if (text == "" || textBounds.IsEmpty || style.TextColor.IsEmpty)
                return;

            Font font = style.Font;
            if (font == null || useDefaultFont)
                font = defaultFont;

            textBounds.X += style.MarginLeft;
            textBounds.Width -= (style.MarginLeft + style.MarginRight);
            textBounds.Y += style.MarginTop;
            textBounds.Height -= (style.MarginTop + style.MarginBottom);

            if (!style.TextShadowColor.IsEmpty && Math.Abs(style.TextShadowColor.GetBrightness() - style.TextColor.GetBrightness()) > .2)
            {
                using (SolidBrush brush = new SolidBrush(style.TextShadowColor))
                {
                    Rectangle r = textBounds;
                    r.Offset(style.TextShadowOffset);
                    TextDrawing.DrawString(e.Graphics, text, font, style.TextShadowColor, r, textFormat);
                    //e.Graphics.DrawString(text,font,brush,r,style.StringFormat);
                }
            }

            if (!style.TextColor.IsEmpty)
                TextDrawing.DrawString(e.Graphics, text, font, style.TextColor, textBounds, textFormat);
        }

		/// <summary>Returns new Region object for given ElementStyle.</summary>
		/// <returns>New instance of Region object.</returns>
		/// <param name="e">Information to describe ElementStyle.</param>
		public static Region GetStyleRegion(ElementStyleDisplayInfo e)
		{
            Rectangle rectPath = e.Bounds;
            if (e.Style.PaintBorder && e.Style.CornerType!=eCornerType.Square)
            {
                rectPath.Width--;
                rectPath.Height--;
            }

            GraphicsPath path = ElementStyleDisplay.GetBackgroundPath(e.Style, rectPath);
            Region r = new Region();
            r.MakeEmpty();
            r.Union(path);
            // Widen path for the border...
            if (e.Style.PaintBorder && (e.Style.CornerType == eCornerType.Rounded || e.Style.CornerType == eCornerType.Diagonal || 
                e.Style.CornerTypeTopLeft == eCornerType.Rounded || e.Style.CornerTypeTopLeft == eCornerType.Diagonal ||
                e.Style.CornerTypeTopRight==eCornerType.Rounded || e.Style.CornerTypeTopRight==eCornerType.Diagonal ||
                e.Style.CornerTypeBottomLeft==eCornerType.Rounded || e.Style.CornerTypeBottomLeft==eCornerType.Diagonal ||
                e.Style.CornerTypeBottomRight==eCornerType.Rounded || e.Style.CornerTypeBottomRight==eCornerType.Diagonal))
            {
                using (Pen pen = new Pen(Color.Black, (e.Style.BorderTopWidth>0?e.Style.BorderTopWidth:1)))
                {
                    path.Widen(pen);
                }
                //Region r2 = new Region(path);
                r.Union(path);
            }

            return r;
            //GraphicsPath path=ElementStyleDisplay.GetBackgroundPath(e.Style,e.Bounds,false);
            //return new Region(path);
		}

        /// <summary>
        /// Returns the clipping for the content of the element style.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static GraphicsPath GetInsideClip(ElementStyleDisplayInfo e)
        {
            Rectangle rectPath = e.Bounds;
            // GDI bug
            if (e.Style.PaintBorder)
            {
                if (e.Style.PaintTopBorder)
                {
                    rectPath.Y += e.Style.BorderTopWidth;
                    rectPath.Height -= e.Style.BorderTopWidth;

                    if (e.Style.BorderTop == eStyleBorderType.Etched || e.Style.BorderTop == eStyleBorderType.Double)
                    {
                        rectPath.Y += e.Style.BorderTopWidth;
                        rectPath.Height -= e.Style.BorderTopWidth;
                    }
                }

                if (e.Style.PaintBottomBorder)
                {
                    rectPath.Height -= e.Style.BorderBottomWidth;
                    if (e.Style.BorderBottom == eStyleBorderType.Etched || e.Style.BorderBottom == eStyleBorderType.Double)
                        rectPath.Height -= e.Style.BorderBottomWidth;
                }

                if (e.Style.PaintLeftBorder)
                {
                    rectPath.X += e.Style.BorderLeftWidth;
                    rectPath.Width -= e.Style.BorderLeftWidth;
                    if (e.Style.BorderLeft == eStyleBorderType.Etched || e.Style.BorderLeft == eStyleBorderType.Double)
                    {
                        rectPath.X += e.Style.BorderLeftWidth;
                        rectPath.Width -= e.Style.BorderLeftWidth;
                    }
                }

                if (e.Style.PaintRightBorder)
                {
                    rectPath.Width -= e.Style.BorderRightWidth;
                    if (e.Style.BorderRight == eStyleBorderType.Etched || e.Style.BorderRight == eStyleBorderType.Double)
                        rectPath.Width -= e.Style.BorderRightWidth;
                }
            }

            GraphicsPath path = ElementStyleDisplay.GetBackgroundPath(e.Style, rectPath);

            return path;
        }

		/// <summary>
		/// Paints the element style on the canvas.
		/// </summary>
		/// <param name="e">Holds information neccessary to paint style on canvas.</param>
		public static void Paint(ElementStyleDisplayInfo e)
		{
			ElementStyleDisplay.PaintBackground(e);
			ElementStyleDisplay.PaintBackgroundImage(e);
			ElementStyleDisplay.PaintBorder(e);
		}

		private static eCornerType GetCornerType(eCornerType baseType, eCornerType specificType)
		{
			if(specificType!=eCornerType.Inherit)
				return specificType;
			return baseType;
		}

		/// <summary>
		/// Paint style border.
		/// </summary>
		/// <param name="e">Style display information.</param>
		public static void PaintBorder(ElementStyleDisplayInfo e)
		{
            ElementStyle style = GetElementStyle(e.Style);
			Rectangle bounds=ElementStyleDisplay.GetBorderRectangle(style,e.Bounds);
            if (bounds.Width < 2 || bounds.Height < 2)
                return;

            eCornerType cornerTopLeft = GetCornerType(style.CornerType, style.CornerTypeTopLeft);
            eCornerType cornerTopRight = GetCornerType(style.CornerType, style.CornerTypeTopRight);
            eCornerType cornerBottomLeft = GetCornerType(style.CornerType, style.CornerTypeBottomLeft);
            eCornerType cornerBottomRight = GetCornerType(style.CornerType, style.CornerTypeBottomRight);

            if (!style.BorderColor2.IsEmpty || !style.BorderColorLight.IsEmpty)
            {
                if (style.Border == eStyleBorderType.Solid)
                {
                    using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(bounds, style.CornerDiameter, eStyleBackgroundPathPart.Complete,
                        cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight))
                    {
                        DisplayHelp.DrawGradientPathBorder(e.Graphics, path, style.BorderColor, style.BorderColor2, style.BorderGradientAngle, style.BorderWidth);
                    }
                    return;
                }
                else if (style.Border == eStyleBorderType.Etched)
                {
                    Rectangle r = bounds;
                    
                    r.Width -= style.BorderWidth;
                    r.Height -= style.BorderWidth;
                    r.Offset(style.BorderWidth, style.BorderWidth);
                    if (r.Width > 2 && r.Height > 2)
                    {
                        using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(r, style.CornerDiameter, eStyleBackgroundPathPart.Complete,
                            cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight))
                        {
                            DisplayHelp.DrawGradientPathBorder(e.Graphics, path, style.BorderColorLight, style.BorderColorLight2, style.BorderLightGradientAngle, style.BorderWidth);
                        }
                        r.Offset(-style.BorderWidth, -style.BorderWidth);
                        using (GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(r, style.CornerDiameter, eStyleBackgroundPathPart.Complete,
                            cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight))
                        {
                            DisplayHelp.DrawGradientPathBorder(e.Graphics, path, style.BorderColor, style.BorderColor2, style.BorderGradientAngle, style.BorderWidth);
                        }
                    }
                    return;
                }
                else if ((style.BorderTop == eStyleBorderType.Double && style.BorderLeft == eStyleBorderType.Double &&
                    style.BorderRight == eStyleBorderType.Double && style.BorderBottom == eStyleBorderType.Double) ||
                   (style.BorderTop == eStyleBorderType.None && style.BorderLeft == eStyleBorderType.Double &&
                   style.BorderRight == eStyleBorderType.Double && style.BorderBottom == eStyleBorderType.Double))
                    {
                    Rectangle r = bounds;
                    Region oldClip = null;
                    bool clipSet = false;
                    if (style.BorderTop == eStyleBorderType.None)
                    {
                        Rectangle clipRect = r;
                        clipRect.Width++;
                        clipRect.Height++;
                        e.Graphics.SetClip(clipRect);
                        r.Y -= 3;
                        r.Height += 3;
                    }
                    using(GraphicsPath path = DisplayHelp.GetBorderPath(r, style.CornerDiameter, eStyleBackgroundPathPart.Complete,
                        cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight, style.PaintLeftBorder, style.PaintRightBorder, 
                        style.PaintTopBorder, style.PaintBottomBorder))
                    {
                        using (Pen pen = new Pen(style.BorderColor, style.BorderWidth))
                            path.Widen(pen);
                        DisplayHelp.FillPath(e.Graphics, path, style.BorderColor, style.BorderColor2);
                    }
                    r.Inflate(-style.BorderWidth, -style.BorderWidth);
                    using (GraphicsPath path = DisplayHelp.GetBorderPath(r, style.CornerDiameter, eStyleBackgroundPathPart.Complete,
                        cornerTopLeft, cornerTopRight, cornerBottomLeft, cornerBottomRight, style.PaintLeftBorder, style.PaintRightBorder,
                        style.PaintTopBorder, style.PaintBottomBorder))
                    {
                        using (Pen pen = new Pen(style.BorderColor, style.BorderWidth))
                            path.Widen(pen);
                        DisplayHelp.FillPath(e.Graphics, path, style.BorderColorLight, style.BorderColorLight2);
                    }

                    if (clipSet)
                        e.Graphics.Clip = oldClip;

                    return;
                }
            }

            Color colorStart = style.BorderColor;
            Color colorEnd = style.BorderColor2;
            Color colorLightStart = style.BorderColorLight;
            Color colorLightEnd = style.BorderColorLight2;

			if(style.PaintLeftBorder)
			{
				Color color=style.BorderColor;
				if(!style.BorderLeftColor.IsEmpty)
					color=style.BorderLeftColor;
				Point[] p=new Point[2];

				// Corner type square is default setting
				p[0]=bounds.Location;
				p[1].X=bounds.X;
				p[1].Y=bounds.Bottom;

				if(cornerTopLeft!=eCornerType.Square)
				{
					p[0].Y+=style.CornerDiameter;
				}
				if(cornerBottomLeft!=eCornerType.Square)
				{
					p[1].Y-=style.CornerDiameter;
				}

                ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderLeft, style.BorderLeftWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.Left);

				if(style.PaintTopBorder && cornerTopLeft!=eCornerType.Square)
				{
					if(cornerTopLeft==eCornerType.Diagonal)
					{
						p[0].X=bounds.X;
						p[0].Y=bounds.Y+style.CornerDiameter;
						p[1].X=bounds.X+style.CornerDiameter;
						p[1].Y=bounds.Y;
                        ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderLeft, style.BorderLeftWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.TopLeft);
					}
					else if(cornerTopLeft==eCornerType.Rounded)
					{
						ArcData a=GetCornerArc(bounds,style.CornerDiameter,eCornerArc.TopLeft);// new ArcData(bounds.X,bounds.Y,style.CornerDiameter*2,style.CornerDiameter*2,180,90);
						ElementStyleDisplay.DrawCornerArc(e.Graphics,a,style.BorderLeft,style.BorderLeftWidth,color);
					}
				}

				if(style.PaintBottomBorder && cornerBottomLeft!=eCornerType.Square)
				{
					if(cornerBottomLeft==eCornerType.Diagonal)
					{
						p[0].X=bounds.X;
						p[0].Y=bounds.Bottom-style.CornerDiameter;
						p[1].X=bounds.X+style.CornerDiameter;
						p[1].Y=bounds.Bottom;
                        ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderLeft, style.BorderLeftWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.BottomLeft);
					}
					else if(cornerBottomLeft==eCornerType.Rounded)
					{
						ArcData a=GetCornerArc(bounds,style.CornerDiameter,eCornerArc.BottomLeft);// new ArcData(bounds.X,bounds.Y,style.CornerDiameter*2,style.CornerDiameter*2,180,90);
						ElementStyleDisplay.DrawCornerArc(e.Graphics,a,style.BorderLeft,style.BorderLeftWidth,color);
					}
				}
			}

			if(style.PaintTopBorder)
			{
				Color color=style.BorderColor;
				if(!style.BorderTopColor.IsEmpty)
					color=style.BorderTopColor;
				Point[] p=new Point[2];
				// Default setting for Square corner type on both sides
				p[0]=bounds.Location;
				p[1].X=bounds.Right;
				p[1].Y=bounds.Y;

				if(cornerTopLeft!=eCornerType.Square)
				{
					p[0].X+=style.CornerDiameter;
				}
				if(cornerTopRight!=eCornerType.Square)
				{
					p[1].X-=style.CornerDiameter;
				}
                ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderTop, style.BorderTopWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.Top);
			}

			if(style.PaintBottomBorder)
			{
				Color color=style.BorderColor;
				if(!style.BorderBottomColor.IsEmpty)
					color=style.BorderBottomColor;
				Point[] p=new Point[2];
				// Default for Square corner type on both sides
				p[0].X=bounds.X;
				p[0].Y=bounds.Bottom;
				p[1].X=bounds.Right;
				p[1].Y=bounds.Bottom;

				if(cornerBottomLeft!=eCornerType.Square)
				{
					p[0].X+=style.CornerDiameter;
				}
				if(cornerBottomRight!=eCornerType.Square)
				{
					p[1].X-=style.CornerDiameter;
				}

                ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderBottom, style.BorderBottomWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.Bottom);
			}

			if(style.PaintRightBorder)
			{
				Color color=style.BorderColor;
				if(!style.BorderRightColor.IsEmpty)
					color=style.BorderRightColor;
				Point[] p=new Point[2];
				// Default for Square corner type on both sides
				p[0].X=bounds.Right;
				p[0].Y=bounds.Y;
				p[1].X=bounds.Right;
				p[1].Y=bounds.Bottom;

				if(cornerTopRight!=eCornerType.Square)
				{
					p[0].Y+=style.CornerDiameter;
				}
				if(cornerBottomRight!=eCornerType.Square)
				{
					p[1].Y-=style.CornerDiameter;
				}

                ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderRight, style.BorderRightWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.Right);

				if(style.PaintTopBorder && cornerTopRight!=eCornerType.Square)
				{
					if(cornerTopRight==eCornerType.Diagonal)
					{
						p[0].X=bounds.Right-style.CornerDiameter;
						p[0].Y=bounds.Y;
						p[1].X=bounds.Right;
						p[1].Y=bounds.Y+style.CornerDiameter;
                        ElementStyleDisplay.DrawBorderLine(e.Graphics, p, style.BorderLeft, style.BorderRightWidth, color, colorEnd, colorLightStart, colorLightEnd, eBorderSide.TopRight);
					}
					else if(cornerTopRight==eCornerType.Rounded)
					{
						ArcData a=GetCornerArc(bounds,style.CornerDiameter,eCornerArc.TopRight);// new ArcData(bounds.X,bounds.Y,style.CornerDiameter*2,style.CornerDiameter*2,180,90);
						ElementStyleDisplay.DrawCornerArc(e.Graphics,a,style.BorderLeft,style.BorderLeftWidth,color);
					}
				}

				if(style.PaintBottomBorder && cornerBottomRight!=eCornerType.Square)
				{
					if(cornerBottomRight==eCornerType.Diagonal)
					{
						p[0].X=bounds.Right;
						p[0].Y=bounds.Bottom-style.CornerDiameter;
						p[1].X=bounds.Right-style.CornerDiameter;
						p[1].Y=bounds.Bottom;
						ElementStyleDisplay.DrawBorderLine(e.Graphics,p,style.BorderLeft,style.BorderRightWidth,color, colorEnd, colorLightStart, colorLightEnd,eBorderSide.BottomRight);
					}
					else if(cornerBottomRight==eCornerType.Rounded)
					{
						ArcData a=GetCornerArc(bounds,style.CornerDiameter,eCornerArc.BottomRight);// new ArcData(bounds.X,bounds.Y,style.CornerDiameter*2,style.CornerDiameter*2,180,90);
						ElementStyleDisplay.DrawCornerArc(e.Graphics,a,style.BorderLeft,style.BorderLeftWidth,color);
					}
				}
			}

		}

		private static Pen CreatePen(eStyleBorderType border,int lineWidth, Color color)
		{
			Pen pen=new Pen(color,lineWidth);
			pen.Alignment=PenAlignment.Inset;
			pen.DashStyle=ElementStyleDisplay.GetDashStyle(border);
			return pen;
		}

		private static void DrawBorderLine(Graphics g,Point[] p,eStyleBorderType border,int lineWidth, Color colorStart, Color colorEnd, Color colorLightStart, Color colorLightEnd, eBorderSide side)
		{
            if (border == eStyleBorderType.Etched || border == eStyleBorderType.Double)
            {
                if (colorLightStart.IsEmpty)
                {
                    colorLightStart = System.Windows.Forms.ControlPaint.Light(colorStart);
                    colorStart = System.Windows.Forms.ControlPaint.Dark(colorStart);
                }

                if (side == eBorderSide.Bottom || side == eBorderSide.Right)
                {
                    Color ct1 = colorStart;
                    Color ct2 = colorEnd;
                    colorStart = colorLightStart;
                    colorEnd = colorLightEnd;
                    colorLightStart = ct1;
                    colorLightEnd = ct2;
                }

                DisplayHelp.DrawGradientLine(g, p[0], p[1], colorStart, colorEnd, 90, lineWidth);
                
                if (side == eBorderSide.Top)
                {
                    p[0].Y+=lineWidth;
                    p[1].Y += lineWidth;
                }
                else if (side == eBorderSide.Bottom)
                {
                    p[0].Y -= lineWidth;
                    p[1].Y -= lineWidth;
                }
                else if (side == eBorderSide.Left || side == eBorderSide.BottomLeft || side == eBorderSide.TopLeft)
                {
                    p[0].X += lineWidth;
                    p[1].X += lineWidth;
                }
                else if (side == eBorderSide.Right || side == eBorderSide.BottomRight || side == eBorderSide.TopRight)
                {
                    p[0].X -= lineWidth;
                    p[1].X -= lineWidth;
                }

                DisplayHelp.DrawGradientLine(g, p[0], p[1], colorLightStart, colorLightEnd, 90, lineWidth);
            }
            else
            {
                DisplayHelp.DrawGradientLine(g, p[0], p[1], colorStart, colorEnd, 90, lineWidth);
            }
		}

		private static void DrawCornerArc(Graphics g,ArcData arc,eStyleBorderType border,int lineWidth, Color color)
		{
			using(Pen pen=CreatePen(border,lineWidth,color))
			{
				g.DrawArc(pen,arc.X,arc.Y,arc.Width,arc.Height,arc.StartAngle,arc.SweepAngle);
			}
		}

		private static DashStyle GetDashStyle(eStyleBorderType border)
		{
			DashStyle style=DashStyle.Solid;
			switch(border)
			{
				case eStyleBorderType.Dash:
                    style=DashStyle.Dash;
					break;
				case eStyleBorderType.DashDot:
					style=DashStyle.DashDot;
					break;
				case eStyleBorderType.DashDotDot:
					style=DashStyle.DashDotDot;
					break;
				case eStyleBorderType.Dot:
					style=DashStyle.Dot;
					break;
			}
			return style;
		}

        internal static ElementStyle GetElementStyle(ElementStyle style)
        {
            if (style.Class == "")
                return style;
            IElementStyleClassProvider provider = GetIElementStyleClassProvider();
            if (provider != null)
            {
                ElementStyle baseStyle = provider.GetClass(style.Class);
                if (baseStyle != null)
                {
                    return baseStyle;
                }
            }

            return style;
        }

        private static IElementStyleClassProvider GetIElementStyleClassProvider()
        {
#if DOTNETBAR
            if (Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
            {
                return ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable as IElementStyleClassProvider;
            }
#endif
            return null;
        }

		/// <summary>
		/// Paints style background.
		/// </summary>
		/// <param name="e">Style display information.</param>
		public static void PaintBackground(ElementStyleDisplayInfo e)
		{
            Region oldClip = e.Graphics.Clip;
            if (oldClip != null)
                e.Graphics.SetClip(e.Bounds, CombineMode.Intersect);
            else
                e.Graphics.SetClip(e.Bounds, CombineMode.Replace);

            ElementStyle style = GetElementStyle(e.Style);

            SmoothingMode sm = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.Default;

			// Paint Background
			Rectangle bounds=DisplayHelp.GetDrawRectangle(ElementStyleDisplay.GetBackgroundRectangle(style,e.Bounds));
			GraphicsPath path;
            path = ElementStyleDisplay.GetBackgroundPath(style, e.Bounds);

            eBackgroundColorBlendType blendType = style.BackColorBlend.GetBlendType();
            if (blendType!=eBackgroundColorBlendType.Invalid)
            {
                if (blendType == eBackgroundColorBlendType.Relative)
                {
                    try
                    {
                        if (style.BackColorGradientType == eGradientType.Linear)
                        {
                            Rectangle rb = bounds;
                            rb.Inflate(1, 1);
                            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, style.BackColor, style.BackColor2, style.BackColorGradientAngle))
                            {
                                brush.InterpolationColors = style.BackColorBlend.GetColorBlend();
                                e.Graphics.FillPath(brush, path);
                            }
                        }
                        else if (style.BackColorGradientType == eGradientType.Radial)
                        {
                            int d = (int)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) + 4;
                            GraphicsPath fillPath = new GraphicsPath();
                            fillPath.AddEllipse(bounds.X - (d - bounds.Width) / 2, bounds.Y - (d - bounds.Height) / 2, d, d);
                            using (PathGradientBrush brush = new PathGradientBrush(fillPath))
                            {
                                brush.CenterColor = style.BackColor;
                                brush.SurroundColors = new Color[] { style.BackColor2 };
                                brush.InterpolationColors = style.BackColorBlend.GetColorBlend();
                                e.Graphics.FillPath(brush, path);
                            }
                            fillPath.Dispose();
                        }
                    }
                    catch
                    {
                        blendType = eBackgroundColorBlendType.Invalid;
                    }
                }
                else
                {
                    Graphics g = e.Graphics;
                    bounds = e.Bounds;
                    if (oldClip != null)
                    {
                        e.Graphics.SetClip(oldClip, CombineMode.Replace);
                        e.Graphics.SetClip(path, CombineMode.Intersect);
                    }
                    else
                        e.Graphics.SetClip(path, CombineMode.Replace);
                    BackgroundColorBlendCollection bc = style.BackColorBlend;
                    for (int i = 0; i < bc.Count; i+=2)
                    {
                        BackgroundColorBlend b1 = bc[i];
                        BackgroundColorBlend b2 = null;
                        if (i < bc.Count)
                            b2 = bc[i + 1];
                        if (b1 != null && b2 != null)
                        {
                            Rectangle rb = new Rectangle(bounds.X, bounds.Y + (int)b1.Position, bounds.Width,
                                (b2.Position == 1f ? bounds.Height : (int)b2.Position) - (int)b1.Position);
                            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, b1.Color, b2.Color, style.BackColorGradientAngle))
                                g.FillRectangle(brush, rb);
                        }
                    }
                }
            }
            
            if(blendType==eBackgroundColorBlendType.Invalid)
            {
                if (style.BackColor2.IsEmpty)
                {
                    if (!style.BackColor.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(style.BackColor))
                            e.Graphics.FillPath(brush, path);
                        //if(e.Style.BackColor.A==255)
                        //{
                        //    // Correct problems with FillPath where path was not filled properly...
                        //    using(Pen pen=new Pen(e.Style.BackColor,1))
                        //        e.Graphics.DrawPath(pen,path);
                        //}
                    }
                }
                else if (!style.BackColor.IsEmpty)
                {
                    if (style.BackColorGradientType == eGradientType.Linear)
                    {
                        Rectangle rb = bounds;
                        rb.X--;
                        rb.Height++;
                        rb.Width += 2;
                        using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, style.BackColor, style.BackColor2, style.BackColorGradientAngle))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    else if (style.BackColorGradientType == eGradientType.Radial)
                    {
                        int d = (int)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) + 4;
                        GraphicsPath fillPath = new GraphicsPath();
                        fillPath.AddEllipse(bounds.X - (d - bounds.Width) / 2, bounds.Y - (d - bounds.Height) / 2, d, d);
                        using (PathGradientBrush brush = new PathGradientBrush(fillPath))
                        {
                            brush.CenterColor = style.BackColor;
                            brush.SurroundColors = new Color[] { style.BackColor2 };
                            e.Graphics.FillPath(brush, path);
                        }
                        fillPath.Dispose();
                    }
                }
            }

            e.Graphics.SmoothingMode = sm;

            if (oldClip != null)
                e.Graphics.Clip = oldClip;
            else
                e.Graphics.ResetClip();

			
		}
		
		/// <summary>
		/// Paints style background image.
		/// </summary>
		/// <param name="e">Style display information.</param>
		public static void PaintBackgroundImage(ElementStyleDisplayInfo e)
		{
            ElementStyle style = GetElementStyle(e.Style);
			if(style.BackgroundImage==null)
				return;

            Rectangle bounds = DisplayHelp.GetDrawRectangle(ElementStyleDisplay.GetBackgroundRectangle(style, e.Bounds));
			GraphicsPath path;
			if (e.Graphics.SmoothingMode == SmoothingMode.AntiAlias)
			{
				Rectangle r = e.Bounds;
				r.Width--;
				//r.Height--;
                path = ElementStyleDisplay.GetBackgroundPath(style, r);
			}
			else
                path = ElementStyleDisplay.GetBackgroundPath(style, e.Bounds);
			
			ImageAttributes imageAtt=null;

            if (style.BackgroundImageAlpha != 255)
			{
				ColorMatrix colorMatrix=new ColorMatrix();
                colorMatrix.Matrix33 = 255 - style.BackgroundImageAlpha;
				imageAtt=new ImageAttributes();
				imageAtt.SetColorMatrix(colorMatrix,ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
			}

			Region clip=e.Graphics.Clip;
			e.Graphics.SetClip(path);

            eStyleBackgroundImage imagePosition = style.BackgroundImagePosition;
            bool transform = false;
            Image backgroundImage = style.BackgroundImage;

            if (e.RightToLeft)
            {
                if (imagePosition == eStyleBackgroundImage.TopLeft)
                {
                    imagePosition = eStyleBackgroundImage.TopRight;
                    transform = true;
                }
                else if (imagePosition == eStyleBackgroundImage.TopRight)
                    imagePosition = eStyleBackgroundImage.TopLeft;
                else if (imagePosition == eStyleBackgroundImage.BottomLeft)
                    imagePosition = eStyleBackgroundImage.BottomRight;
                else if (imagePosition == eStyleBackgroundImage.BottomRight)
                    imagePosition = eStyleBackgroundImage.BottomLeft;
            }

            if (transform)
            {
                backgroundImage = backgroundImage.Clone() as Image;
                backgroundImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            switch (imagePosition)
			{
				case eStyleBackgroundImage.Stretch:
				{
					if(imageAtt!=null)
                        e.Graphics.DrawImage(backgroundImage, bounds, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
					else
                        e.Graphics.DrawImage(backgroundImage, bounds, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
					break;
				}
				case eStyleBackgroundImage.Center:
				{
                    Rectangle destRect = new Rectangle(bounds.X, bounds.Y, backgroundImage.Width, backgroundImage.Height);
                    if (bounds.Width > backgroundImage.Width)
                        destRect.X += (bounds.Width - backgroundImage.Width) / 2;
                    if (bounds.Height > backgroundImage.Height)
                        destRect.Y += (bounds.Height - backgroundImage.Height) / 2;
					if(imageAtt!=null)
                        e.Graphics.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
					else
                        e.Graphics.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
					
					break;
				}
				case eStyleBackgroundImage.TopLeft:
				case eStyleBackgroundImage.TopRight:
				case eStyleBackgroundImage.BottomLeft:
				case eStyleBackgroundImage.BottomRight:
				{
                    Rectangle destRect = new Rectangle(bounds.X, bounds.Y, backgroundImage.Width, backgroundImage.Height);
                    if (imagePosition == eStyleBackgroundImage.TopRight)
                        destRect.X = bounds.Right - backgroundImage.Width;
                    else if (imagePosition == eStyleBackgroundImage.BottomLeft)
                        destRect.Y = bounds.Bottom - backgroundImage.Height;
                    else if (imagePosition == eStyleBackgroundImage.BottomRight)
					{
                        destRect.Y = bounds.Bottom - backgroundImage.Height;
                        destRect.X = bounds.Right - backgroundImage.Width;
					}

					if(imageAtt!=null)
                        e.Graphics.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
					else
                        e.Graphics.DrawImage(backgroundImage, destRect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel);
					break;
				}
				case eStyleBackgroundImage.Tile:
				{
					if(imageAtt!=null)
					{
                        if (bounds.Width > backgroundImage.Width || bounds.Height > backgroundImage.Height)
						{
							int x=bounds.X,y=bounds.Y;
							while(y<bounds.Bottom)
							{
								while(x<bounds.Right)
								{
                                    Rectangle destRect = new Rectangle(x, y, backgroundImage.Width, backgroundImage.Height);
									if(destRect.Right>bounds.Right)
										destRect.Width=destRect.Width-(destRect.Right-bounds.Right);
									if(destRect.Bottom>bounds.Bottom)
										destRect.Height=destRect.Height-(destRect.Bottom-bounds.Bottom);
                                    e.Graphics.DrawImage(backgroundImage, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, imageAtt);
                                    x += backgroundImage.Width;
								}
								x=bounds.X;
                                y += backgroundImage.Height;
							}
						}
						else
						{
                            e.Graphics.DrawImage(backgroundImage, new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height), 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAtt);
						}
					}
					else
					{
                        TextureBrush brush = new TextureBrush(backgroundImage);
						brush.WrapMode=WrapMode.Tile;
						e.Graphics.FillPath(brush,path);
						brush.Dispose();
					}
					break;
				}
			}
            if (transform)
            {
                backgroundImage.Dispose();
            }

			if (clip != null)
				e.Graphics.Clip = clip;
			else
				e.Graphics.ResetClip();
		}

		/// <summary>
		/// Returns background rectangle for given style by taking in account margins.
		/// </summary>
		/// <param name="style">Reference to style object.</param>
		/// <param name="bounds">Style bounds</param>
		/// <returns>Background rectangle.</returns>
		public static Rectangle GetBackgroundRectangle(ElementStyle style, Rectangle bounds)
		{
			// Reduce the bounds rectangle size by the margin size
			bounds.X+=style.MarginLeft;
			bounds.Width-=(style.MarginLeft+style.MarginRight);
			bounds.Y+=style.MarginTop;
			bounds.Height-=(style.MarginTop+style.MarginBottom);
			return bounds;
		}

		private static Rectangle GetBorderRectangle(ElementStyle style, Rectangle bounds)
		{
			bounds=GetBackgroundRectangle(style,bounds);
			if(style.PaintRightBorder)
			{
				if(style.BorderRightWidth<=1)
					bounds.Width-=style.BorderRightWidth;
				else
					bounds.Width-=style.BorderRightWidth/2;
			}

			if(style.PaintBottomBorder)
			{
				if(style.BorderBottomWidth<=1)
					bounds.Height-=style.BorderBottomWidth;
				else
					bounds.Height-=style.BorderBottomWidth/2;
			}

			if(style.PaintLeftBorder && style.BorderLeftWidth>1)
			{
				bounds.X+=style.BorderLeftWidth/2;
				bounds.Width-=style.BorderLeftWidth/2;
			}

			if(style.PaintTopBorder && style.BorderTopWidth>1)
			{
				bounds.X+=style.BorderTopWidth/2;
				bounds.Width-=style.BorderTopWidth/2;
			}

			return bounds;
		}

		private static Rectangle GetTextRectangle(ElementStyle style, Rectangle bounds)
		{
			bounds=ElementStyleDisplay.GetBackgroundRectangle(style,bounds);

			// Reduce the bounds rectangle size by the padding size
			bounds.X+=style.PaddingLeft;
			bounds.Width-=(style.PaddingLeft+style.PaddingRight);
			bounds.Y+=style.PaddingTop;
			bounds.Height-=(style.PaddingTop+style.PaddingBottom);
			return bounds;
		}
		
		/// <summary>
		/// Returns GraphicsPath for given style.
		/// </summary>
		/// <param name="style">Reference to style.</param>
		/// <param name="bounds">Style bounds.</param>
		/// <returns>New instance of GraphicsPath</returns>
		public static GraphicsPath GetBackgroundPath(ElementStyle style, Rectangle bounds)
		{
			return GetBackgroundPath(style, bounds, eStyleBackgroundPathPart.Complete);
		}

		/// <summary>
		/// Returns GraphicsPath for given style.
		/// </summary>
		/// <param name="style">Reference to style.</param>
		/// <param name="bounds">Style bounds.</param>
		/// <returns>New instance of GraphicsPath</returns>
		public static GraphicsPath GetBackgroundPath(ElementStyle style, Rectangle bounds, eStyleBackgroundPathPart pathPart)
		{
			GraphicsPath path=new GraphicsPath();
			
			Rectangle clientRectangle;
			//if(drawPath)
				clientRectangle=/*DisplayHelp.GetDrawRectangle*/(ElementStyleDisplay.GetBackgroundRectangle(style,bounds));
			//else
            //   clientRectangle =/*DisplayHelp.GetDrawRectangle*/(ElementStyleDisplay.GetBackgroundRectangle(style, bounds));

            if (pathPart==eStyleBackgroundPathPart.Complete && style.CornerTypeBottomLeft == eCornerType.Inherit && style.CornerTypeBottomRight == eCornerType.Inherit &&
                style.CornerTypeTopLeft == eCornerType.Inherit && style.CornerTypeTopRight == eCornerType.Inherit)
            {
                switch (style.CornerType)
                {
                    case eCornerType.Square:
                        {
                            path.AddRectangle(clientRectangle);
                            break;
                        }
                    case eCornerType.Rounded:
                        {
                            ArcData ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.TopLeft);
                            path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                            ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.TopRight);
                            path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                            ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.BottomRight);
                            path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                            ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
                            path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                            path.CloseAllFigures();
                            break;
                        }
                    case eCornerType.Diagonal:
                        {
                            path.AddLine(clientRectangle.X, clientRectangle.Bottom - style.CornerDiameter - 1, clientRectangle.X, clientRectangle.Y + style.CornerDiameter);
                            path.AddLine(clientRectangle.X + style.CornerDiameter, clientRectangle.Y, clientRectangle.Right - style.CornerDiameter, clientRectangle.Y);
                            path.AddLine(clientRectangle.Right, clientRectangle.Y + style.CornerDiameter, clientRectangle.Right, clientRectangle.Bottom - style.CornerDiameter - 1);
                            path.AddLine(clientRectangle.Right - style.CornerDiameter - 1, clientRectangle.Bottom, clientRectangle.X + style.CornerDiameter, clientRectangle.Bottom);
                            path.CloseAllFigures();
                            break;
                        }
                }
            }
            else
            {
				if(pathPart==eStyleBackgroundPathPart.TopHalf)
					clientRectangle.Height = clientRectangle.Height/2;
				else if(pathPart==eStyleBackgroundPathPart.BottomHalf)
				{
					int h=clientRectangle.Height;
					clientRectangle.Height = clientRectangle.Height/2;
					clientRectangle.Y += (h - clientRectangle.Height-1);
				}
            	
                eCornerType corner = style.CornerTypeTopLeft;
                if (corner == eCornerType.Inherit)
                    corner = style.CornerType;

            	if(pathPart==eStyleBackgroundPathPart.BottomHalf)
            		corner=eCornerType.Square;
            		
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.TopLeft);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.X, clientRectangle.Y + style.CornerDiameter, clientRectangle.X + style.CornerDiameter, clientRectangle.Y);
                }
                else
                {
                    path.AddLine(clientRectangle.X, clientRectangle.Y + 2, clientRectangle.X, clientRectangle.Y);
                    path.AddLine(clientRectangle.X, clientRectangle.Y ,clientRectangle.X+2, clientRectangle.Y);
                }

                corner = style.CornerTypeTopRight;
                if (corner == eCornerType.Inherit)
                    corner = style.CornerType;
				if(pathPart==eStyleBackgroundPathPart.BottomHalf)
					corner=eCornerType.Square;
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.TopRight);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.Right-style.CornerDiameter-1, clientRectangle.Y , clientRectangle.Right, clientRectangle.Y+style.CornerDiameter);
                }
                else
                {
                    path.AddLine(clientRectangle.Right-2, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y);
                    path.AddLine(clientRectangle.Right, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y+2);
                }

                corner = style.CornerTypeBottomRight;
                if (corner == eCornerType.Inherit)
                    corner = style.CornerType;
				if(pathPart==eStyleBackgroundPathPart.TopHalf)
					corner=eCornerType.Square;
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.BottomRight);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.Right, clientRectangle.Bottom - style.CornerDiameter - 1, clientRectangle.Right - style.CornerDiameter - 1, clientRectangle.Bottom);
                }
                else
                {
                    path.AddLine(clientRectangle.Right, clientRectangle.Bottom-2, clientRectangle.Right, clientRectangle.Bottom);
                    path.AddLine(clientRectangle.Right, clientRectangle.Bottom, clientRectangle.Right-2, clientRectangle.Bottom);
                }

                corner = style.CornerTypeBottomLeft;
                if (corner == eCornerType.Inherit)
                    corner = style.CornerType;
				if(pathPart==eStyleBackgroundPathPart.TopHalf)
					corner=eCornerType.Square;
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = GetCornerArc(clientRectangle, style.CornerDiameter, eCornerArc.BottomLeft);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - style.CornerDiameter - 1);
                }
                else
                {
                    path.AddLine(clientRectangle.X+2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom);
                    path.AddLine(clientRectangle.X, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom-2);
                }
            }

			return path;
		}

        internal static void AddCornerArc(GraphicsPath path, Rectangle bounds, int cornerDiameter, eCornerArc corner)
        {
            ArcData a = GetCornerArc(bounds, cornerDiameter, corner);
            path.AddArc(a.X, a.Y, a.Width, a.Height, a.StartAngle, a.SweepAngle);
        }

		internal static ArcData GetCornerArc(Rectangle bounds, int cornerDiameter, eCornerArc corner)
		{
			ArcData a;
			int diameter=cornerDiameter*2;
			switch(corner)
			{
				case eCornerArc.TopLeft:
					a=new ArcData(bounds.X,bounds.Y,diameter,diameter,180,90);
					break;
                case eCornerArc.TopRight:
					a=new ArcData(bounds.Right-diameter,bounds.Y,diameter,diameter,270,90);
					break;
				case eCornerArc.BottomLeft:
					a=new ArcData(bounds.X,bounds.Bottom-diameter,diameter,diameter,90,90);
					break;
				default: // eCornerArc.BottomRight:
					a=new ArcData(bounds.Right-diameter,bounds.Bottom-diameter,diameter,diameter,0,90);
					break;
			}
			return a; 
		}

		enum eBorderSide
		{
			Top,
			Bottom,
			Left,
			Right,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}
	}

    internal enum eCornerArc
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

	/// <summary>
	/// Represents information neccessary to paint the style on canvas.
	/// </summary>
	public class ElementStyleDisplayInfo
	{
		/// <summary>Reference to ElementStyle object.</summary>
		public ElementStyle Style=null;
		/// <summary>Reference to Graphics object.</summary>
		public Graphics Graphics=null;
		/// <summary>ElementStyle bounds.</summary>
		public Rectangle Bounds=Rectangle.Empty;
        /// <summary>Get or sets whether layout is right-to-left.</summary>
        public bool RightToLeft = false;

        /// <summary>Creates new instance of the object.</summary>
		public ElementStyleDisplayInfo()
		{
		}

		/// <summary>Creates new instance of the object and initializes it with default values.</summary>
		/// <param name="style">Style to initialize object with.</param>
		/// <param name="g">Graphics object to initialize object with.</param>
		/// <param name="bounds">Bounds to initialize object with.</param>
		public ElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds)
		{
			this.Style=style;
			this.Graphics=g;
			this.Bounds=bounds;
		}
//		public ElementStyleDisplayInfo(ElementStyle style, System.Drawing.Graphics g, Rectangle bounds, string text, Rectangle textBounds)
//		{
//			this.Style=style;
//			this.Graphics=g;
//			this.Bounds=bounds;
//			this.Text=text;
//			this.TextBounds=textBounds;
//		}
	}
	
	#region eStyleBackgroundPathPart
	/// <summary>
	/// Specifies part of the background path.
	/// </summary>
	public enum eStyleBackgroundPathPart
	{
		/// <summary>
		/// Indicates complete background path
		/// </summary>
		Complete,
		/// <summary>
		/// Indicates Top half of background path
		/// </summary>
		TopHalf,
		/// <summary>
		/// Indicates Bottom half of background path
		/// </summary>
		BottomHalf
	}
	#endregion

	#region ArcData
	internal struct ArcData
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public float StartAngle;
		public float SweepAngle;
		public ArcData(int x, int y, int width, int height, float startAngle, float sweepAngle)
		{
			this.X=x;
			this.Y=y;
			this.Width=width;
			this.Height=height;
			this.StartAngle=startAngle;
			this.SweepAngle=sweepAngle;
		}
	}
	#endregion
}
