using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that holds User Interface static methods.
	/// </summary>
	internal class UIGraphics
	{
		private UIGraphics()
		{
		}

		/// <summary>
		/// Gets the graphics path that represents triangle.
		/// </summary>
		/// <param name="p">Top left position of the triangle.</param>
		/// <param name="size">Size of the triangle.</param>
		/// <param name="direction">Pointing direction of the triangle.</param>
		/// <returns>Returns graphics path for the triangle of given size and pointing in given direction.</returns>
		public static GraphicsPath GetTrianglePath(Point p, int size, eTriangleDirection direction)
		{
			GraphicsPath path=new GraphicsPath();
			switch(direction)
			{
				case eTriangleDirection.Left:
				{
					p.X--;
					path.AddLine(p.X+size/2,p.Y,p.X+size/2,p.Y+size);
					path.AddLine(p.X,p.Y+size/2,p.X+size/2,p.Y);
					path.CloseAllFigures();
					break;
				}
				case eTriangleDirection.Right:
				{
					path.AddLine(p.X,p.Y,p.X,p.Y+size);
					path.AddLine(p.X+size/2,p.Y+size/2,p.X,p.Y);
					path.CloseAllFigures();
					break;
				}
				case eTriangleDirection.Top:
				{
                    int midY = (int)Math.Ceiling(p.Y + (float)size / 2);
                    int midX = (int)Math.Ceiling(p.X + (float)size / 2);
                    path.AddLine(p.X, midY, p.X + size, midY);
                    path.AddLine(midX, p.Y, p.X, midY);
					path.CloseAllFigures();
					break;
				}
				case eTriangleDirection.Bottom:
				{
                    int midY = (int)Math.Floor(p.Y + (float)size / 2);
                    int midX = (int)Math.Floor(p.X + (float)size / 2);
                    path.AddLine(p.X, p.Y, p.X + size - 1, p.Y); // -1 hack for GDI+ FillPath bug
                    path.AddLine(midX, midY, p.X, p.Y);
					path.CloseAllFigures();
					break;
				}
			}

			return path;
		}

        /// <summary>
        /// Creates the double arrow >> collapse expand image for the collapsable controls.
        /// </summary>
        /// <param name="collapse">Indicates the direction of the arrow</param>
        /// <param name="color">Color for the arrows</param>
        /// <param name="verticalCollapse">Indicates whether image is for vertical collapse/expand</param>
        /// <returns></returns>
        public static Image CreateExpandButtonImage(bool collapse, Color color, bool verticalCollapse)
        {
            Bitmap bmp = new Bitmap(16, 16, PixelFormat.Format24bppRgb);
            bmp.MakeTransparent();
            Image img = bmp;
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (collapse)
            {
                if (verticalCollapse)
                {
                    using (Pen pen = new Pen(color, 1))
                    {
                        g.DrawLine(pen, 4, 7, 7, 4);
                        g.DrawLine(pen, 7, 4, 10, 7);
                        g.DrawLine(pen, 5, 7, 7, 5);
                        g.DrawLine(pen, 7, 5, 9, 7);

                        g.DrawLine(pen, 4, 10, 7, 7);
                        g.DrawLine(pen, 7, 7, 10, 10);
                        g.DrawLine(pen, 5, 10, 7, 8);
                        g.DrawLine(pen, 7, 8, 9, 10);
                    }
                }
                else
                {
                    using (Pen pen = new Pen(color, 1))
                    {
                        g.DrawLine(pen, 7, 4, 4, 7);
                        g.DrawLine(pen, 4, 7, 7, 10);
                        g.DrawLine(pen, 7, 5, 5, 7);
                        g.DrawLine(pen, 5, 7, 7, 9);

                        g.DrawLine(pen, 10, 4, 7, 7);
                        g.DrawLine(pen, 7, 7, 10, 10);
                        g.DrawLine(pen, 10, 5, 8, 7);
                        g.DrawLine(pen, 8, 7, 10, 9);
                    }
                }
            }
            else
            {
                if (verticalCollapse)
                {
                    using (Pen pen = new Pen(color, 1))
                    {
                        g.DrawLine(pen, 4, 4, 7, 7);
                        g.DrawLine(pen, 7, 7, 10, 4);
                        g.DrawLine(pen, 5, 4, 7, 6);
                        g.DrawLine(pen, 7, 6, 9, 4);

                        g.DrawLine(pen, 4, 7, 7, 10);
                        g.DrawLine(pen, 7, 10, 10, 7);
                        g.DrawLine(pen, 5, 7, 7, 9);
                        g.DrawLine(pen, 7, 9, 9, 7);
                    }
                }
                else
                {
                    using (Pen pen = new Pen(color, 1))
                    {
                        g.DrawLine(pen, 4, 4, 7, 7);
                        g.DrawLine(pen, 7, 7, 4, 10);
                        g.DrawLine(pen, 4, 5, 6, 7);
                        g.DrawLine(pen, 6, 7, 4, 9);

                        g.DrawLine(pen, 7, 4, 10, 7);
                        g.DrawLine(pen, 10, 7, 7, 10);
                        g.DrawLine(pen, 7, 5, 9, 7);
                        g.DrawLine(pen, 9, 7, 7, 9);
                    }
                }
            }
            g.Dispose();

            return img;
        }
	}

	/// <summary>
	/// Specifies the pointing direction of triangle.
	/// </summary>
	internal enum eTriangleDirection
	{
		/// <summary>
		/// Triangle point to the left.
		/// </summary>
		Left,
		/// <summary>
		/// Triangle point to the right.
		/// </summary>
		Right,
		/// <summary>
		/// Triangle point to the top.
		/// </summary>
		Top,
		/// <summary>
		/// Triangle point to the bottom.
		/// </summary>
		Bottom
	}
}
