using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Drawing.Text;

#if AdvTree
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Summary description for Display.
	/// </summary>
	public class DisplayHelp
	{
		private DisplayHelp()
		{
		}

		public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r,Color color1, Color color2,float gradientAngle)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
		}

		public static LinearGradientBrush CreateLinearGradientBrush(RectangleF r,Color color1, Color color2,float gradientAngle)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new RectangleF(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle);
		}

		public static LinearGradientBrush CreateLinearGradientBrush(Rectangle r,Color color1, Color color2,float gradientAngle, bool isAngleScalable)
		{
			if(r.Width<=0)
				r.Width=1;
			if(r.Height<=0)
				r.Height=1;
			return new LinearGradientBrush(new Rectangle(r.X,r.Y-1,r.Width,r.Height+1),color1,color2,gradientAngle,isAngleScalable);
		}

		public static Rectangle GetDrawRectangle(Rectangle r)
		{
			r.Width--;
			r.Height--;
			return r;
		}

		public static Rectangle GetPathRectangle(Rectangle r)
		{
			//r.Width++;
			//r.Height++;
			return r;
		}

        public static void DrawRectangle(System.Drawing.Graphics g, Color color, int x, int y, int width, int height)
        {
            using (Pen pen = new Pen(color, 1))
                DrawRectangle(g, pen, x, y, width, height);
        }
        public static void DrawRectangle(System.Drawing.Graphics g, Color color, System.Drawing.Rectangle r)
        {
            DrawRectangle(g, color, r.X, r.Y, r.Width, r.Height);
        }

		public static void DrawRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen,  int x, int y, int width, int height)
		{
			// Fix for GDI issue
			width--;
			height--;
			g.DrawRectangle(pen,x,y,width,height);
		}
		public static void DrawRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen, System.Drawing.Rectangle r)
		{
			DrawRectangle(g,pen,r.X,r.Y,r.Width,r.Height);
		}

        public static void DrawRoundedRectangle(System.Drawing.Graphics g, Color color, DevComponents.DotNetBar.Rendering.LinearGradientColorTable fillColor, Rectangle bounds, int cornerSize)
        {
            using (Brush fill = CreateBrush(bounds, fillColor))
            {
                using (Pen pen = new Pen(color))
                    DrawRoundedRectangle(g, pen, fill, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
            }
        }

        public static void DrawRoundedRectangle(System.Drawing.Graphics g, Color color, Color fillColor, Rectangle bounds, int cornerSize)
        {
            using (Brush fill = new SolidBrush(fillColor))
            {
                using (Pen pen = new Pen(color))
                    DrawRoundedRectangle(g, pen, fill, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
            }
        }

        public static void DrawRoundedRectangle(System.Drawing.Graphics g, Color color, Rectangle bounds, int cornerSize)
        {
            if (!color.IsEmpty)
            {
                using (Pen pen = new Pen(color))
                    DrawRoundedRectangle(g, pen, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
            }
        }

        public static void DrawRoundedRectangle(System.Drawing.Graphics g, Color color, int x, int y, int width, int height, int cornerSize)
        {
            if (!color.IsEmpty)
            {
                using (Pen pen = new Pen(color))
                    DrawRoundedRectangle(g, pen, x, y, width, height, cornerSize);
            }
        }

        public static void DrawRoundedRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen, Rectangle bounds, int cornerSize)
        {
            DrawRoundedRectangle(g, pen, bounds.X, bounds.Y, bounds.Width, bounds.Height, cornerSize);
        }
        public static void DrawRoundedRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen, int x, int y, int width, int height, int cornerSize)
        {
            DrawRoundedRectangle(g, pen, null, x, y, width, height, cornerSize);
        }
        public static void DrawRoundedRectangle(System.Drawing.Graphics g, System.Drawing.Pen pen, Brush fill, int x, int y, int width, int height, int cornerSize)
        {
            // Fix for GDI issue
            width--;
            height--;

            Rectangle r = new Rectangle(x, y, width, height);

            //SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            
            using (GraphicsPath path = GetRoundedRectanglePath(r, cornerSize))
            {
                if (fill != null)
                    g.FillPath(fill, path);
                g.DrawPath(pen, path);
            }

            //g.SmoothingMode = sm;
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerSize)
        {
            GraphicsPath path = new GraphicsPath();
            if (cornerSize == 0)
                path.AddRectangle(r);
            else
            {
                ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopLeft);
                ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopRight);
                ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.BottomRight);
                ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.BottomLeft);
                path.CloseAllFigures();
            }
            return path;
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerTopLeft, int cornerTopRight, int cornerBottomRight, int cornerBottomLeft)
        {
            GraphicsPath path = new GraphicsPath();
            ElementStyleDisplay.AddCornerArc(path, r, cornerTopLeft, eCornerArc.TopLeft);
            ElementStyleDisplay.AddCornerArc(path, r, cornerTopRight, eCornerArc.TopRight);
            ElementStyleDisplay.AddCornerArc(path, r, cornerBottomRight, eCornerArc.BottomRight);
            ElementStyleDisplay.AddCornerArc(path, r, cornerBottomLeft, eCornerArc.BottomLeft);
            path.CloseAllFigures();
            return path;
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart,
            eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType)
        {
            return GetRoundedRectanglePath(clientRectangle, cornerSize, pathPart, topLeftCornerType, topRightCornerType, bottomLeftCornerType, bottomRightCornerType, 0);
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart,
            eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType, float partSize)
        {
            return GetRoundedRectanglePath(clientRectangle, cornerSize, cornerSize, cornerSize, cornerSize, pathPart, topLeftCornerType, topRightCornerType, bottomLeftCornerType, bottomRightCornerType, partSize);
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int topLeftCornerSize, int topRightCornerSize, int bottomLeftCornerSize, int bottomRightCornerSize, eStyleBackgroundPathPart pathPart,
            eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType, float partSize)
        {
            GraphicsPath path = new GraphicsPath();

            if (pathPart == eStyleBackgroundPathPart.TopHalf)
            {
                if (partSize == 0)
                    clientRectangle.Height = clientRectangle.Height / 2;
                else
                    clientRectangle.Height = (int)(clientRectangle.Height * partSize);
            }
            else if (pathPart == eStyleBackgroundPathPart.BottomHalf)
            {
                int h = clientRectangle.Height;
                if (partSize == 0)
                    clientRectangle.Height = clientRectangle.Height / 2;
                else
                    clientRectangle.Height = (int)(clientRectangle.Height * partSize);
                clientRectangle.Y += (h - clientRectangle.Height - 1);
            }

            eCornerType corner = topLeftCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;

            if (pathPart == eStyleBackgroundPathPart.BottomHalf)
                corner = eCornerType.Square;

            if (corner == eCornerType.Rounded && topLeftCornerSize>0)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, topLeftCornerSize, eCornerArc.TopLeft);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.X, clientRectangle.Y + topLeftCornerSize, clientRectangle.X + topLeftCornerSize, clientRectangle.Y);
            }
            else
            {
                path.AddLine(clientRectangle.X, clientRectangle.Y + 2, clientRectangle.X, clientRectangle.Y);
                path.AddLine(clientRectangle.X, clientRectangle.Y, clientRectangle.X + 2, clientRectangle.Y);
            }

            corner = topRightCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.BottomHalf)
                corner = eCornerType.Square;
            if (corner == eCornerType.Rounded && topRightCornerSize>0)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, topRightCornerSize, eCornerArc.TopRight);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.Right - topRightCornerSize - 1, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + topRightCornerSize);
            }
            else
            {
                path.AddLine(clientRectangle.Right - 2, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y);
                path.AddLine(clientRectangle.Right, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + 2);
            }

            corner = bottomRightCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.TopHalf)
                corner = eCornerType.Square;
            if (corner == eCornerType.Rounded && bottomRightCornerSize>0)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, bottomRightCornerSize, eCornerArc.BottomRight);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.Right, clientRectangle.Bottom - bottomRightCornerSize - 1, clientRectangle.Right - bottomRightCornerSize - 1, clientRectangle.Bottom);
            }
            else
            {
                path.AddLine(clientRectangle.Right, clientRectangle.Bottom - 2, clientRectangle.Right, clientRectangle.Bottom);
                path.AddLine(clientRectangle.Right, clientRectangle.Bottom, clientRectangle.Right - 2, clientRectangle.Bottom);
            }

            corner = bottomLeftCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.TopHalf)
                corner = eCornerType.Square;
            if (corner == eCornerType.Rounded && bottomLeftCornerSize>0)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, bottomLeftCornerSize, eCornerArc.BottomLeft);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - bottomLeftCornerSize - 1);
            }
            else
            {
                path.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom);
                path.AddLine(clientRectangle.X, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - 2);
            }

            path.CloseAllFigures();
            return path;
        }

        public static GraphicsPath GetBorderPath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart,
            eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType,
            bool leftBorder, bool rightBorder, bool topBorder, bool bottomBorder)
        {
            GraphicsPath path = new GraphicsPath();

            if (pathPart == eStyleBackgroundPathPart.TopHalf)
                clientRectangle.Height = clientRectangle.Height / 2;
            else if (pathPart == eStyleBackgroundPathPart.BottomHalf)
            {
                int h = clientRectangle.Height;
                clientRectangle.Height = clientRectangle.Height / 2;
                clientRectangle.Y += (h - clientRectangle.Height - 1);
            }

            eCornerType corner = topLeftCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;

            if (pathPart == eStyleBackgroundPathPart.BottomHalf)
                corner = eCornerType.Square;

            if (leftBorder)
            {
                path.AddLine(clientRectangle.X, clientRectangle.Bottom -
                    (bottomBorder && (bottomLeftCornerType == eCornerType.Diagonal || bottomLeftCornerType == eCornerType.Rounded) ? cornerSize : 0),
                    clientRectangle.X, clientRectangle.Y +
                    (topBorder && (topLeftCornerType == eCornerType.Diagonal || topLeftCornerType == eCornerType.Rounded) ? cornerSize : 0));
            }

            if (leftBorder && topBorder)
            {
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.TopLeft);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.X, clientRectangle.Y + cornerSize, clientRectangle.X + cornerSize, clientRectangle.Y);
                }
            }

            if (topBorder)
            {
                path.AddLine(clientRectangle.X + 
                    ((topLeftCornerType == eCornerType.Diagonal || topLeftCornerType == eCornerType.Rounded) ? cornerSize : 0)
                    , clientRectangle.Y, clientRectangle.Right -
                    (rightBorder && (topRightCornerType == eCornerType.Diagonal || topRightCornerType == eCornerType.Rounded) ? cornerSize : 0),
                    clientRectangle.Y);
            }

            corner = topRightCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.BottomHalf)
                corner = eCornerType.Square;

            if (topBorder && rightBorder)
            {
                if (corner == eCornerType.Rounded)
                {
                    ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.TopRight);
                    path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
                }
                else if (corner == eCornerType.Diagonal)
                {
                    path.AddLine(clientRectangle.Right - cornerSize - 1, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + cornerSize);
                }
            }

            if (rightBorder)
            {
                path.AddLine(clientRectangle.Right, clientRectangle.Y +
                    ((topRightCornerType == eCornerType.Diagonal || topRightCornerType == eCornerType.Rounded) ? cornerSize : 0),
                    clientRectangle.Right, clientRectangle.Bottom -
                    (bottomBorder && (bottomRightCornerType == eCornerType.Diagonal || bottomRightCornerType == eCornerType.Rounded) ? cornerSize : 0));
            }

            corner = bottomRightCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.TopHalf)
                corner = eCornerType.Square;
            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.BottomRight);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.Right, clientRectangle.Bottom - cornerSize - 1, clientRectangle.Right - cornerSize - 1, clientRectangle.Bottom);
            }

            if (bottomBorder)
            {
                path.AddLine(clientRectangle.Right -
                    ((bottomRightCornerType == eCornerType.Diagonal || bottomRightCornerType == eCornerType.Rounded) ? cornerSize : 0),
                    clientRectangle.Bottom,
                    clientRectangle.X + 
                    ((bottomLeftCornerType == eCornerType.Diagonal || bottomLeftCornerType == eCornerType.Rounded) ? cornerSize : 0),
                    clientRectangle.Bottom);
            }

            corner = bottomLeftCornerType;
            if (corner == eCornerType.Inherit)
                corner = eCornerType.Square;
            if (pathPart == eStyleBackgroundPathPart.TopHalf)
                corner = eCornerType.Square;
            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.BottomLeft);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - cornerSize - 1);
            }
            
            return path;
        }

        public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1, Color color2, int gradientAngle)
        {
            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(color1))
                        FillRoundedRectangle(g, brush, bounds, cornerSize);
                }
            }
            else
            {
                using (LinearGradientBrush brush = CreateLinearGradientBrush(bounds, color1, color2, gradientAngle))
                    FillRoundedRectangle(g, brush, bounds, cornerSize);
            }
        }

        public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1, Color color2)
        {
            FillRoundedRectangle(g, bounds, cornerSize, color1, color2, 90);
        }

        public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerSize, Color color1)
        {
            using (SolidBrush brush = new SolidBrush(color1))
                FillRoundedRectangle(g, brush, bounds, cornerSize);
        }

        public static void FillRoundedRectangle(Graphics g, Brush brush, Rectangle bounds, int cornerSize)
        {
            // Fix for GDI issue
            bounds.Width--;
            bounds.Height--;

            using (GraphicsPath path = GetRoundedRectanglePath(bounds, cornerSize))
            {
                g.FillPath(brush, path);
            }
        }

        public static void FillRectangle(Graphics g, Rectangle bounds, Color color1)
        {
            FillRectangle(g, bounds, color1, Color.Empty, 90);
        }

        public static void FillRectangle(Graphics g, Rectangle bounds, Color color1, Color color2)
        {
            FillRectangle(g, bounds, color1, color2, 90);
        }

		#if DOTNETBAR
        public static void FillRectangle(Graphics g, Rectangle r, Rendering.LinearGradientColorTable table)
        {
            FillRectangle(g, r, table.Start, table.End, table.GradientAngle);
        }
		#endif

        public static void FillRectangle(Graphics g, Rectangle r, Color color1, Color color2, int gradientAngle)
        {
            if (r.Width == 0 || r.Height == 0)
                return;

            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.None;
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillRectangle(brush, r);
                    g.SmoothingMode = sm;
                }
            }
            else
            {
                using (LinearGradientBrush brush = CreateLinearGradientBrush(r, color1, color2, gradientAngle))
                    g.FillRectangle(brush, r);
            }
        }

        public static void FillRectangle(Graphics g, Rectangle r, Color color1, Color color2, int gradientAngle, float[] factors, float[] positions)
        {
            if (r.Width == 0 || r.Height == 0)
                return;

            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.None;
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillRectangle(brush, r);
                    g.SmoothingMode = sm;
                }
            }
            else
            {
                using (LinearGradientBrush brush = CreateLinearGradientBrush(r, color1, color2, gradientAngle))
                {
                    Blend blend = new Blend(factors.Length);
                    blend.Factors = factors;
                    blend.Positions = positions;
                    brush.Blend = blend;
                    g.FillRectangle(brush, r);
                }
            }
        }

        public static void FillPath(Graphics g, GraphicsPath path, Color color1, Color color2)
        {
            FillPath(g, path, color1, color2, 90);
        }
        
		#if DOTNETBAR
        public static void FillPath(Graphics g, GraphicsPath path, Rendering.LinearGradientColorTable table)
        {
            FillPath(g, path, table.Start, table.End, table.GradientAngle);
        }
		#endif

        public static void FillPath(Graphics g, GraphicsPath path, Color color1, Color color2, int gradientAngle)
        {
            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillPath(brush, path);
                }
            }
            else if(!color1.IsEmpty)
            {
                using (LinearGradientBrush brush = CreateLinearGradientBrush(path.GetBounds(), color1, color2, gradientAngle))
                    g.FillPath(brush, path);
            }
        }

        //public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth)
        //{
        //    if (color1.IsEmpty || penWidth <= 0 || start == end)
        //        return;

        //    using (GraphicsPath path = new GraphicsPath())
        //    {
        //        start.Offset(-1, -1);
        //        end.Offset(-1, -1);
        //        path.AddLine(start, end);
        //        using (Pen pen = new Pen(Color.White, penWidth))
        //            path.Widen(pen);
        //        Rectangle r = Rectangle.Ceiling(path.GetBounds());
        //        r.Inflate(1, 1);
        //        using (LinearGradientBrush brush = CreateLinearGradientBrush(r, color1, color2, gradientAngle))
        //            g.FillPath(brush, path);
        //    }
        //}

        public static void DrawLine(Graphics g, Point start, Point end, Color color, int penWidth)
        {
            if (!color.IsEmpty)
            {
                using (Pen pen = new Pen(color, penWidth))
                    g.DrawLine(pen, start, end);
            }
        }

        public static void DrawLine(Graphics g, int x1, int y1, int x2, int y2, Color color, int penWidth)
        {
            if (!color.IsEmpty)
            {
                using (Pen pen = new Pen(color, penWidth))
                    g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

		#if DOTNETBAR
        public static void DrawGradientRectangle(Graphics g, Rectangle bounds, Rendering.LinearGradientColorTable table, int penWidth)
        {
            DrawGradientRectangle(g, bounds, table.Start, table.End, table.GradientAngle, penWidth);
        }
		#endif

        public static void DrawGradientRectangle(Graphics g, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth)
        {
            if (color1.IsEmpty || bounds.Width <= 0 || bounds.Height <= 0 || penWidth <= 0)
                return;

            Rectangle r = bounds;
            // Workaround for GDI+ bug
            r.Width--;
            r.Height--;

            if (color2.IsEmpty)
            {
                using (Pen pen = new Pen(color1, penWidth))
                    g.DrawRectangle(pen, r);
                return;
            }

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(r);

                DrawGradientPath(g, path, r, color1, color2, gradientAngle, penWidth);
            }
        }

        public static void DrawGradientPath(Graphics g, GraphicsPath path, Rectangle bounds, Rendering.LinearGradientColorTable table, int penWidth)
        {
            DrawGradientPath(g, path, bounds, table.Start, table.End, table.GradientAngle, penWidth);
        }

        public static void DrawGradientPath(Graphics g, GraphicsPath path, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth)
        {
            using (Pen pen = new Pen(color1, penWidth))
                path.Widen(pen);

            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillPath(brush, path);
                }
            }
            else if (!color1.IsEmpty)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(bounds, color1, color2, gradientAngle))
                    g.FillPath(brush, path);
            }
        }

		#if DOTNETBAR
        public static void DrawRoundGradientRectangle(Graphics g, Rectangle bounds, Rendering.LinearGradientColorTable table, int penWidth, int roundCornerSize)
        {
            DrawRoundGradientRectangle(g, bounds, table.Start, table.End, table.GradientAngle, penWidth, roundCornerSize);
        }
		#endif

        public static void DrawRoundGradientRectangle(Graphics g, Rectangle bounds, Color color1, Color color2, int gradientAngle, int penWidth, int roundCornerSize)
        {
            if (color1.IsEmpty && color2.IsEmpty || bounds.Width <= 0 || bounds.Height <= 0 || roundCornerSize <= 0 || penWidth <= 0)
                return;

            if (color2.IsEmpty)
            {
                using (Pen pen = new Pen(color1, penWidth))
                    DrawRoundedRectangle(g, pen, bounds, roundCornerSize);
                return;
            }

            Rectangle r = bounds;
            // Workaround for GDI+ bug
            r.Width--;
            r.Height--;

            using (GraphicsPath roundPath = GetRoundedRectanglePath(r, roundCornerSize))
            {
                using (Pen pen = new Pen(color1, penWidth))
                    roundPath.Widen(pen);

                using (LinearGradientBrush brush = new LinearGradientBrush(bounds, color1, color2, gradientAngle))
                    g.FillPath(brush, roundPath);
            }
        }

		#if DOTNETBAR
        public static void DrawGradientPathBorder(Graphics g, GraphicsPath path, Rendering.LinearGradientColorTable table, int penWidth)
        {
            DrawGradientPathBorder(g, path, table.Start, table.End, table.GradientAngle, penWidth);
        }
		#endif

        public static void DrawGradientPathBorder(Graphics g, GraphicsPath path, Color color1, Color color2, int gradientAngle, int penWidth)
        {
            using (Pen pen = new Pen(color1, penWidth))
                path.Widen(pen);

            Rectangle r = Rectangle.Ceiling(path.GetBounds());
            r.Inflate(1, 1);

            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillPath(brush, path);
                }
            }
            else if(!color1.IsEmpty)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(r, color1, color2, gradientAngle))
                    g.FillPath(brush, path);
            }
        }
		#if DOTNETBAR
        public static void DrawGradientLine(Graphics g, Point start, Point end, Rendering.LinearGradientColorTable table, int penWidth)
        {
            DrawGradientLine(g, start, end, table.Start, table.End, table.GradientAngle, penWidth);
        }
        public static void DrawGradientLine(Graphics g, int x1, int y1, int x2, int y2, Rendering.LinearGradientColorTable table, int penWidth)
        {
            DrawGradientLine(g, new Point(x1, y1), new Point(x2, y2), table.Start, table.End, table.GradientAngle, penWidth);
        }
		#endif
        public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth)
        {
            if (color2.IsEmpty || penWidth == 1 && start.Y == end.Y && gradientAngle == 90)
            {
                if (!color1.IsEmpty)
                {
                    using (Pen pen = new Pen(color1, penWidth))
                    {
                        g.DrawLine(pen, start, end);
                    }
                }
            }
            else if (!color1.IsEmpty)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    start.Offset(-1, -1);
                    end.Offset(-1, -1);
                    path.AddLine(start, end);
                    using (Pen pen = new Pen(color1, penWidth))
                        path.Widen(pen);
                    Rectangle r = Rectangle.Ceiling(path.GetBounds());
                    r.Inflate(1, 1);
                    using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(r, color1, color2, gradientAngle))
                        g.FillPath(brush, path);
                }
            }
        }

        public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth, float[] factors, float[] positions)
        {
            if (color2.IsEmpty)
            {
                if (!color1.IsEmpty)
                {
                    using (Pen pen = new Pen(color1, penWidth))
                        g.DrawLine(pen, start, end);
                }
            }
            else if (!color1.IsEmpty)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    start.Offset(-1, -1);
                    end.Offset(-1, -1);
                    path.AddLine(start, end);
                    using (Pen pen = new Pen(color1, penWidth))
                        path.Widen(pen);
                    Rectangle r = Rectangle.Ceiling(path.GetBounds());
                    r.Inflate(1, 1);
                    using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(r, color1, color2, gradientAngle))
                    {
                        Blend blend = new Blend(factors.Length);
                        blend.Factors = factors;
                        blend.Positions = positions;
                        brush.Blend = blend;
                        g.FillPath(brush, path);
                    }
                }
            }
        }
#if DOTNETBAR
        public static Brush CreateBrush(Rectangle bounds, GradientColorTable colorBlend)
        {
            return CreateBrush(bounds, colorBlend.Colors, colorBlend.LinearGradientAngle, colorBlend.GradientType);
        }
        public static Brush CreateBrush(Rectangle bounds, DevComponents.DotNetBar.Rendering.LinearGradientColorTable colorTable)
        {
            if (colorTable.End.IsEmpty) return new SolidBrush(colorTable.Start);
            return new LinearGradientBrush(bounds, colorTable.Start, colorTable.End, colorTable.GradientAngle);
        }
#endif
        public static Brush CreateBrush(Rectangle bounds, BackgroundColorBlendCollection colorBlend, int gradientAngle, eGradientType gradientType)
        {
            eBackgroundColorBlendType blendType = colorBlend.GetBlendType();
            if (blendType == eBackgroundColorBlendType.Invalid)
                return null;

            if (blendType == eBackgroundColorBlendType.Relative)
            {
                try
                {
                    if (gradientType == eGradientType.Linear)
                    {
                        bounds.Inflate(1, 1);
                        LinearGradientBrush brush =
                        DisplayHelp.CreateLinearGradientBrush(bounds, colorBlend[0].Color, colorBlend[colorBlend.Count - 1].Color,
                                                          gradientAngle);
                        brush.InterpolationColors = colorBlend.GetColorBlend();
                        return brush;
                    }
                    else if (gradientType == eGradientType.Radial)
                    {
                        int d = (int)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) + 4;
                        GraphicsPath fillPath = new GraphicsPath();
                        fillPath.AddEllipse(bounds.X - (d - bounds.Width) / 2, bounds.Y - (d - bounds.Height) / 2, d, d);
                        PathGradientBrush brush = new PathGradientBrush(fillPath);
                        brush.CenterColor = colorBlend[0].Color;
                        brush.SurroundColors = new Color[] { colorBlend[colorBlend.Count - 1].Color };
                        brush.InterpolationColors = colorBlend.GetColorBlend();
                        return brush;
                    }
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                BackgroundColorBlendCollection bc = colorBlend;
                for (int i = 0; i < bc.Count; i += 2)
                {
                    BackgroundColorBlend b1 = bc[i];
                    BackgroundColorBlend b2 = null;
                    if (i < bc.Count)
                        b2 = bc[i + 1];
                    if (b1 != null && b2 != null)
                    {
                        Rectangle rb = new Rectangle(bounds.X, bounds.Y + (int)b1.Position, bounds.Width,
                            (b2.Position == 1f ? bounds.Height : (int)b2.Position) - (int)b1.Position);
                        return DisplayHelp.CreateLinearGradientBrush(rb, b1.Color, b2.Color, gradientAngle);
                    }
                }
            }

            return null;
        }

        public static System.Windows.Forms.ControlStyles DoubleBufferFlag
        {
            get
            {
                #if FRAMEWORK20
                return System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer;
#else
                return System.Windows.Forms.ControlStyles.DoubleBuffer;
#endif
            }
        }

        private static TextRenderingHint s_AntiAliasTextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        public static TextRenderingHint AntiAliasTextRenderingHint
        {
            get
            {
                return s_AntiAliasTextRenderingHint;
            }
            set
            {
                s_AntiAliasTextRenderingHint = value;
            }
        }

        public static Rectangle[] ExcludeRectangle(Rectangle r1, Rectangle exclude)
        {
            if (r1.X >= exclude.X && exclude.Right < r1.Right) // Left aligned
                return new Rectangle[] { new Rectangle(exclude.Right, r1.Y, r1.Width - (exclude.Right - r1.X), r1.Height) };
            else if (r1.Right <= exclude.Right && exclude.X > r1.X) // Right aligned
                return new Rectangle[] { new Rectangle(r1.X, r1.Y, r1.Width - (r1.Right - exclude.X), r1.Height) };
            else if (exclude.X > r1.X && exclude.Right < r1.Right)
            {
                return new Rectangle[] { new Rectangle(r1.X, r1.Y, exclude.X-r1.X, r1.Height),
                    new Rectangle(exclude.Right, r1.Y, r1.Right - exclude.Right, r1.Height)};
            }
            else if (exclude.Bottom >= r1.Bottom && exclude.Y > r1.Y) // Bottom Aligned
            {
                return new Rectangle[] { new Rectangle(r1.X, r1.Y, r1.Width, Math.Max(0, exclude.Y - r1.Y)) };
            }
            else if (exclude.Y <= r1.Y && exclude.Bottom < r1.Bottom) // Top Aligned
            {
                return new Rectangle[] { new Rectangle(r1.X, exclude.Bottom, r1.Width, r1.Bottom - exclude.Bottom) };
            }
            return new Rectangle[] { };
        }

        internal static Size MaxSize(Size size1, Size size2)
        {
            if (size2.Width > size1.Width) size1.Width = size2.Width;
            if (size2.Height > size1.Height) size1.Height = size2.Height;
            return size1;
        }

        internal static void ExcludeEdgeRect(ref Rectangle captionRect, Rectangle exclude)
        {
            if (exclude.X + exclude.Width / 2 < captionRect.X + captionRect.Width / 2)
            {
                // Left aligned
                int r = exclude.Right - captionRect.X;
                captionRect.X = exclude.Right;
                captionRect.Width -= r;
            }
            else
            {
                // Right aligned
                captionRect.Width -= (captionRect.Right - exclude.X);
            }
        }
    }
}
