using System.Drawing;
using System.Drawing.Drawing2D;

#if TREEGX
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
	/// <summary>
	/// Summary description for Display.
	/// </summary>
	internal class DisplayHelp
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
            // Fix for GDI issue
            width--;
            height--;

            Rectangle r = new Rectangle(x, y, width, height);

            SmoothingMode sm = g.SmoothingMode;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            
            using (GraphicsPath path = GetRoundedRectanglePath(r, cornerSize))
            {
                g.DrawPath(pen, path);
            }

            g.SmoothingMode = sm;
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerSize)
        {
            GraphicsPath path = new GraphicsPath();
            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopLeft);
            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.TopRight);
            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.BottomRight);
            ElementStyleDisplay.AddCornerArc(path, r, cornerSize, eCornerArc.BottomLeft);
            path.CloseAllFigures();
            return path;
        }

        public static GraphicsPath GetRoundedRectanglePath(Rectangle clientRectangle, int cornerSize, eStyleBackgroundPathPart pathPart,
            eCornerType topLeftCornerType, eCornerType topRightCornerType, eCornerType bottomLeftCornerType, eCornerType bottomRightCornerType)
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

            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.TopLeft);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.X, clientRectangle.Y + cornerSize, clientRectangle.X + cornerSize, clientRectangle.Y);
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
            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.TopRight);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.Right - cornerSize - 1, clientRectangle.Y, clientRectangle.Right, clientRectangle.Y + cornerSize);
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
            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.BottomRight);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.Right, clientRectangle.Bottom - cornerSize - 1, clientRectangle.Right - cornerSize - 1, clientRectangle.Bottom);
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
            if (corner == eCornerType.Rounded)
            {
                ArcData ad = ElementStyleDisplay.GetCornerArc(clientRectangle, cornerSize, eCornerArc.BottomLeft);
                path.AddArc(ad.X, ad.Y, ad.Width, ad.Height, ad.StartAngle, ad.SweepAngle);
            }
            else if (corner == eCornerType.Diagonal)
            {
                path.AddLine(clientRectangle.X + 2, clientRectangle.Bottom, clientRectangle.X, clientRectangle.Bottom - cornerSize - 1);
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
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillRectangle(brush, r);
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
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillRectangle(brush, r);
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

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(r);

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

            Rectangle r = bounds;
            // Workaround for GDI+ bug
            r.Width--;
            r.Height--;

            using (GraphicsPath roundPath = GetRoundedRectanglePath(r, roundCornerSize))
            {
                using (Pen pen = new Pen(color1, penWidth))
                    roundPath.Widen(pen);

                if (color2.IsEmpty)
                {
                    using (SolidBrush brush = new SolidBrush(color1))
                        g.FillPath(brush, roundPath);
                }
                else if(!color1.IsEmpty)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(bounds, color1, color2, gradientAngle))
                        g.FillPath(brush, roundPath);
                }
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
		#endif
        public static void DrawGradientLine(Graphics g, Point start, Point end, Color color1, Color color2, int gradientAngle, int penWidth)
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
	}
}
