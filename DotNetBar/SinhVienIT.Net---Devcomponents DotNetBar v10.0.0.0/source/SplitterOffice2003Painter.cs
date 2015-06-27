using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents painter for Office 2003 style splitter.
	/// </summary>
	internal class SplitterOffice2003Painter:SplitterPainter
	{
		/// <summary>
		/// Creates new instance of splitter painter.
		/// </summary>
		public SplitterOffice2003Painter(){}

		#region Painting
		/// <summary>
		/// Paints splitter.
		/// </summary>
		/// <param name="info">Paint information.</param>
		public override void Paint(SplitterPaintInfo info)
		{
			int spacing=8;
			Rectangle r=info.DisplayRectangle;
			Graphics g=info.Graphics;
			SplitterColors colors=info.Colors;

			PaintBackground(info);

			if(info.Dock==DockStyle.Top || info.Dock==DockStyle.Bottom)
			{
				Point pStart=new Point(r.X+(r.Width-34)/2-1,r.Y+(r.Height-4)/2);
				using(SolidBrush brush=new SolidBrush(colors.GripLightColor)) //White
				{
					int x=pStart.X+1;
					int y=pStart.Y+1;
					for(int i=0;i<9;i++)
					{
						g.FillRectangle(brush,x,y,2,2);
						x+=4;
					}
				}

				using(SolidBrush brush=new SolidBrush(colors.GripDarkColor)) //Color.FromArgb(128,ControlPaint.Dark(colors.PanelBackground))))
				{
					int x=pStart.X;
					int y=pStart.Y;
					for(int i=0;i<9;i++)
					{
						g.FillRectangle(brush,x,y,2,2);
						x+=4;
					}
				}

				if(info.Expandable)
				{
					Point p=new Point(r.X+(r.Width-36)/2-this.ArrowSize-spacing,r.Y+(r.Height-ArrowSize/2)/2);
					this.PaintArrow(p,info);
					p.Offset(36+spacing*2+ArrowSize,0);
					this.PaintArrow(p,info);
				}
			}
			else
			{
				Point pStart=new Point(r.X+(r.Width-4)/2,r.Y+(r.Height-34)/2);
				using(SolidBrush brush=new SolidBrush(colors.GripLightColor))
				{
					int y=pStart.Y; //r.Y+(r.Height-34)/2;
					int x=pStart.X+1; // r.X+2;
					for(int i=0;i<9;i++)
					{
						g.FillRectangle(brush,x,y,2,2);
						y+=4;
					}
				}

				using(SolidBrush brush=new SolidBrush(colors.GripDarkColor))
				{
					int y=pStart.Y-1; //r.Y+(r.Height-34)/2-1;
					int x=pStart.X; //r.X+1;
					for(int i=0;i<9;i++)
					{
						g.FillRectangle(brush,x,y,2,2);
						y+=4;
					}
				}

				if(info.Expandable)
				{
					Point p=new Point(r.X+(r.Width-ArrowSize/2)/2,r.Y+(r.Height-36)/2-this.ArrowSize-spacing);
					this.PaintArrow(p,info);
					p.Offset(0,36+spacing*2+ArrowSize);
					this.PaintArrow(p,info);
				}
			}
		}
		#endregion
	}
}
