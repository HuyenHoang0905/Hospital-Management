using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents painter for Mozilla style splitter.
	/// </summary>
	internal class SplitterMozillaPainter:SplitterPainter
	{
		/// <summary>
		/// Creates new instance of splitter painter.
		/// </summary>
		public SplitterMozillaPainter(){}

		#region Painting
		/// <summary>
		/// Paints splitter.
		/// </summary>
		/// <param name="info">Paint information.</param>
		public override void Paint(SplitterPaintInfo info)
		{
			int gripWidth=42;
			int spacing=8;
			Rectangle r=info.DisplayRectangle;
			Graphics g=info.Graphics;

			PaintBackground(info);

			if(info.Dock==DockStyle.Top || info.Dock==DockStyle.Bottom)
			{
				if(info.DisplayRectangle.Width-ArrowSize*2<gripWidth)
				{
					gripWidth=info.DisplayRectangle.Width-ArrowSize*2;
					if(gripWidth<0)
						gripWidth=info.DisplayRectangle.Width;
				}
				
				Point pStart=new Point(r.X+(r.Width-gripWidth)/2,r.Y+(r.Height-5)/2);
				SolidBrush brushLight=new SolidBrush(info.Colors.GripLightColor);
				SolidBrush brushDark=new SolidBrush(info.Colors.GripDarkColor);
				for(int i=pStart.X;i<pStart.X+gripWidth;i+=3)
				{
					g.FillRectangle(brushLight,i,pStart.Y+3,1,1);
					g.FillRectangle(brushDark,i+1,pStart.Y+4,1,1);
					g.FillRectangle(brushLight,i+1,pStart.Y,1,1);
					g.FillRectangle(brushDark,i+2,pStart.Y+1,1,1);
				}
				brushLight.Dispose();
				brushDark.Dispose();
				if(info.Expandable)
				{
					Point p=new Point(r.X+(r.Width-gripWidth)/2-this.ArrowSize-spacing,r.Y+(r.Height-ArrowSize/2)/2);
					this.PaintArrow(p,info);
					p.Offset(gripWidth+spacing*2+ArrowSize,0);
					this.PaintArrow(p,info);
				}
			}
			else
			{
				if(info.DisplayRectangle.Height-ArrowSize*2<gripWidth)
				{
					gripWidth=info.DisplayRectangle.Height-ArrowSize*2;
					if(gripWidth<0)
						gripWidth=info.DisplayRectangle.Height;
				}

				Point pStart=new Point(r.X+(r.Width-5)/2,r.Y+(r.Height-gripWidth)/2);
				SolidBrush brushLight=new SolidBrush(info.Colors.GripLightColor);
				SolidBrush brushDark=new SolidBrush(info.Colors.GripDarkColor);
				for(int i=pStart.Y;i<pStart.Y+gripWidth;i+=3)
				{
					g.FillRectangle(brushLight,pStart.X+3,i,1,1);
					g.FillRectangle(brushDark,pStart.X+4,i+1,1,1);
					g.FillRectangle(brushLight,pStart.X,i+1,1,1);
					g.FillRectangle(brushDark,pStart.X+1,i+2,1,1);
				}
				brushLight.Dispose();
				brushDark.Dispose();

				if(info.Expandable)
				{
					Point p=new Point(r.X+(r.Width-ArrowSize/2)/2,r.Y+(r.Height-gripWidth)/2-this.ArrowSize-spacing);
					this.PaintArrow(p,info);
					p.Offset(0,gripWidth+spacing*2+ArrowSize);
					this.PaintArrow(p,info);
				}
			}
		}
		#endregion
	}
}
