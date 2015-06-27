using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for Office2003RibbonTabItemPainter.
	/// </summary>
	internal class Office2003RibbonTabItemPainter:Office2003ButtonItemPainter
	{
		/// <summary>
		/// Paints state of the button, either hot, pressed or checked
		/// </summary>
		/// <param name="button"></param>
		/// <param name="pa"></param>
		/// <param name="image"></param>
		public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
		{
			PaintButtonCheck(button,pa,image,r);
		}

        public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
		{
            bool isOnMenu = IsOnMenu(button, pa);
			if(isOnMenu)
				base.PaintButtonBackground(button,pa, image);
			else
				PaintButtonCheck(button,pa,image,button.DisplayRectangle);
		}

		public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
		{
            bool isOnMenu = IsOnMenu(button, pa);
			if(isOnMenu)
			{
				base.PaintButtonCheck(button,pa,image,r);
				return;
			}

			// TODO: Cleanup code here, tabs were drawn for every state that code is still left
			if(!button.IsMouseOver && !button.Checked)
				return;

			Color topColor=pa.Colors.BarBackground2;
			Color bottomColor=pa.Colors.BarBackground;
			Color borderColor=pa.Colors.BarFloatingBorder;
			int angle=pa.Colors.BarBackgroundGradientAngle;
			
			if(button.IsMouseOver && !button.Checked)
			{
				topColor=pa.Colors.ItemHotBackground2;
				bottomColor=pa.Colors.ItemHotBackground;
				angle=pa.Colors.ItemHotBackgroundGradientAngle;
			}
			else if(button.Checked)
			{
				topColor=Color.White;//pa.Colors.BarBackground2;
				bottomColor=pa.Colors.BarBackground; //pa.Colors.BarBackground;
				//topColor=pa.Colors.ItemPressedBackground;
				//bottomColor=pa.Colors.ItemPressedBackground2;
				angle=pa.Colors.ItemPressedBackgroundGradientAngle;
			}

			Color lightColor=ControlPaint.LightLight(bottomColor);
			Color darkColor=Color.FromArgb(100,ControlPaint.Dark(topColor));

			int cornerDiameter=4;
			
			Graphics g=pa.Graphics;
			GraphicsPath path=GetTabPath(r,cornerDiameter);
			path.CloseAllFigures();
            
			// Background
			using(LinearGradientBrush brush=DisplayHelp.CreateLinearGradientBrush(r,topColor,bottomColor,angle))
			{
				brush.GammaCorrection=true;
				g.FillPath(brush,path);
			}
			path.Dispose();

			// Border light
			using(path=GetTabPathLight(r,cornerDiameter-1))
			{
				using(Pen pen=new Pen(darkColor/*lightColor*/,1))
					g.DrawPath(pen,path);
			}

			// Border dark
			using(path=GetTabPathDark(r,cornerDiameter-1))
			{
				using(Pen pen=new Pen(darkColor,1))
					g.DrawPath(pen,path);
			}

			// Border
			using(path=GetTabPath(r,cornerDiameter))
			{
				using(Pen pen=new Pen(borderColor,1))
					g.DrawPath(pen,path);
			}
		}

		private GraphicsPath GetTabPathLight(Rectangle r, int cornerDiameter)
		{
			r.X++;
			r.Y++;
			r.Width-=2;
			r.Height--;
			// Get graphics path for the tab bounds
			GraphicsPath path=new GraphicsPath();

			path.AddLine(r.X,r.Bottom,r.X,r.Y+cornerDiameter);
			
			ArcData ad=ElementStyleDisplay.GetCornerArc(r,cornerDiameter,eCornerArc.TopLeft);
			path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
			//path.AddLine(r.X,r.Y+cornerDiameter,r.X+cornerDiameter,r.Y);
			
			path.AddLine(r.X+cornerDiameter,r.Y,r.Right-cornerDiameter,r.Y);
			
			return path;
		}

		private GraphicsPath GetTabPathDark(Rectangle r, int cornerDiameter)
		{
			r.X++;
			r.Y++;
			r.Width-=3;
			r.Height--;
			// Get graphics path for the tab bounds
			GraphicsPath path=new GraphicsPath();

			ArcData ad=ElementStyleDisplay.GetCornerArc(r,cornerDiameter,eCornerArc.TopRight);
			path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
			//path.AddLine(r.Right-cornerDiameter,r.Y,r.Right,r.Y+cornerDiameter);

			path.AddLine(r.Right,r.Y+cornerDiameter,r.Right,r.Bottom);
			
			return path;
		}


		private GraphicsPath GetTabPath(Rectangle r, int cornerDiameter)
		{
			r.Width--;
			//r.Height--;
			// Get graphics path for the tab bounds
			GraphicsPath path=new GraphicsPath();

			path.AddLine(r.X,r.Bottom,r.X,r.Y+cornerDiameter);
			//path.AddLine(r.X,r.Y+cornerDiameter,r.X+cornerDiameter,r.Y);
			ArcData ad=ElementStyleDisplay.GetCornerArc(r,cornerDiameter,eCornerArc.TopLeft);
			path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
			
			path.AddLine(r.X+cornerDiameter,r.Y,r.Right-cornerDiameter,r.Y);
			//path.AddLine(r.Right-cornerDiameter,r.Y,r.Right,r.Y+cornerDiameter);
			ad=ElementStyleDisplay.GetCornerArc(r,cornerDiameter,eCornerArc.TopRight);
			path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);

			path.AddLine(r.Right,r.Y+cornerDiameter,r.Right,r.Bottom);
			
			return path;
		}

		public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
		{
			base.PaintButtonText(button,pa,textColor,image);
		}
	}
}
