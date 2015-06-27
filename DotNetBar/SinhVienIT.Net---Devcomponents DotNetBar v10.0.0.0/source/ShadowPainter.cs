using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that provides shadows to elements.
	/// </summary>
	public class ShadowPainter
	{
		/// <summary>
		/// Creates new instance of shadow painter.
		/// </summary>
		public ShadowPainter()
		{
		}

		private static System.Drawing.Drawing2D.GraphicsPath GetPath(Rectangle r)
		{
			System.Drawing.Drawing2D.GraphicsPath path=new System.Drawing.Drawing2D.GraphicsPath();
			path.AddLine(r.Left+1,r.Y,r.Right-1,r.Y);
			path.AddLine(r.Right-1,r.Y,r.Right-1,r.Y+1);
			path.AddLine(r.Right-1,r.Y+1,r.Right,r.Y+1);
			path.AddLine(r.Right,r.Y+1,r.Right,r.Bottom-1);
			path.AddLine(r.Right,r.Bottom-1,r.Right-1,r.Bottom-1);
			path.AddLine(r.Right-1,r.Bottom-1,r.Right-1,r.Bottom);
			path.AddLine(r.Right-1,r.Bottom,r.Left+1,r.Bottom);
			path.AddLine(r.Left+1,r.Bottom,r.Left+1,r.Bottom-1);
			path.AddLine(r.Left+1,r.Bottom-1,r.Left,r.Bottom-1);
			path.AddLine(r.Left,r.Bottom-1,r.Left,r.Top+1);
			return path;
		}

		public static void Paint(ShadowPaintInfo info)
		{
			Graphics g=info.Graphics;
			Region oldClip=g.Clip;
            if(info.ClipRectangle.IsEmpty)
			    g.SetClip(info.Rectangle,CombineMode.Exclude);
            else
                g.SetClip(info.ClipRectangle, CombineMode.Exclude);
			Color[] clr=new Color[]{
									   Color.FromArgb(14,Color.Black),
									   Color.FromArgb(43,Color.Black),
									   Color.FromArgb(84,Color.Black),
									   Color.FromArgb(113,Color.Black),
									   Color.FromArgb(128,Color.Black)};

			
			Rectangle r=info.Rectangle;
            if (info.IsSquare)
            {
                r.Inflate(info.Size, info.Size);
                r.Width += info.Size;
            }
            else
            {
                r.Width--;
                r.Height--;
                int offset = info.Size / 2;
                r.Offset(offset + 1, offset);
                r.Width += (info.Size - offset);
                r.Height += (info.Size - offset);
            }
            //r.Width--;
            //r.Height--;
            //int offset = info.Size / 2;
            //r.Offset(offset,offset);
            //r.Width+=(info.Size-offset);
            //r.Height+=(info.Size-offset);
			
			for(int i=0;i<info.Size;i++)
			{
				using(Pen pen=new Pen(clr[i],1))
				{
                    using(GraphicsPath path = GetPath(r))
					    g.DrawPath(pen,path);
					r.Inflate(-1,-1);
				}
			}

			g.Clip=oldClip;
		}

        public static void Paint2(ShadowPaintInfo info)
        {
            if (info.Size <= 2) return;
            Graphics g = info.Graphics;
            Color c = Color.FromArgb(128, Color.Black);
            Rectangle r=info.Rectangle;
            r.Offset(info.Size - 1, info.Size - 1);

            using (Bitmap bmp = new Bitmap(info.Size, info.Size))
            {
                using (Graphics bg = Graphics.FromImage(bmp))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(0, 0, info.Size * 2, info.Size * 2);
                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = c;
                            brush.SurroundColors = new Color[] { Color.Transparent };
                            bg.FillRectangle(brush, new Rectangle(0, 0, info.Size, info.Size));
                        }
                    }
                }
                g.DrawImage(bmp, r.X, r.Y);
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                g.DrawImage(bmp, r.Right - info.Size, r.Y);
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                g.DrawImage(bmp, r.Right - info.Size, r.Bottom - info.Size);
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                g.DrawImage(bmp, r.X, r.Bottom - info.Size);
            }
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;
            Rectangle rb = new Rectangle(r.X+info.Size, r.Y + 1, r.Width - info.Size*2, info.Size - 1);
            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, Color.Transparent, c, 90))
                g.FillRectangle(brush, rb);
            rb.Offset(0, r.Height - info.Size - 1);
            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, c, Color.Transparent, 90))
                g.FillRectangle(brush, rb);

            rb = new Rectangle(r.X, r.Y + info.Size, info.Size, r.Height - info.Size * 2);
            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, Color.Transparent, c, 0))
                g.FillRectangle(brush, rb);

            rb.Offset(r.Width - info.Size - 1, 0);
            using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, c, Color.Transparent, 0))
                g.FillRectangle(brush, rb);

            g.SmoothingMode = sm;
        }
	}

	#region ShadowPaintInfo class
	/// <summary>
	/// Represents class that provides display context for shadow painter.
	/// </summary>
	public class ShadowPaintInfo
	{
		public System.Drawing.Graphics Graphics=null;
		public System.Drawing.Rectangle Rectangle=Rectangle.Empty;
		public int Size=3;
        public System.Drawing.Rectangle ClipRectangle = Rectangle.Empty;
        public bool IsSquare = false;
	}
	#endregion
}
