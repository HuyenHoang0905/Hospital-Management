using System;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Base class for painting expandable splitter control.
	/// </summary>
	internal abstract class SplitterPainter
	{
		#region Private Variables
		private int m_ArrowSize=6;
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SplitterPainter()
		{
		}

		#region Methods
		/// <summary>
		/// Paints splitter.
		/// </summary>
		/// <param name="info">Paint information.</param>
		public virtual void Paint(SplitterPaintInfo info)
		{
		}

		protected virtual void PaintArrow(Point p, SplitterPaintInfo info)
		{
			System.Drawing.Drawing2D.GraphicsPath path=UIGraphics.GetTrianglePath(p,ArrowSize,this.GetDirection(info));
			if(!info.Colors.ExpandFillColor.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(info.Colors.ExpandFillColor))
					info.Graphics.FillPath(brush,path);
			}
			if(!info.Colors.ExpandLineColor.IsEmpty)
			{
				using(Pen pen=new Pen(info.Colors.ExpandLineColor,1))
					info.Graphics.DrawPath(pen,path);
			}
			path.Dispose();
		}

		protected eTriangleDirection GetDirection(SplitterPaintInfo info)
		{
			if(info.Dock==DockStyle.Top && info.Expanded || info.Dock==DockStyle.Bottom && !info.Expanded)
			{
				// Points up
				return eTriangleDirection.Top;
			}
			else if(info.Dock==DockStyle.Top && !info.Expanded || info.Dock==DockStyle.Bottom && info.Expanded)
			{
				// Points down
				return eTriangleDirection.Bottom;
			}
			else if(info.Dock==DockStyle.Left && info.Expanded || info.Dock==DockStyle.Right && !info.Expanded)
			{
				// Points left
				return eTriangleDirection.Left;
			}
			return eTriangleDirection.Right;
		}

		protected System.Drawing.Drawing2D.LinearGradientBrush CreateLinearGradientBrush(Rectangle r,Color color1, Color color2,float gradientAngle)
		{
			return new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(r.X-1,r.Y-1,r.Width+1,r.Height+1),color1,color2,gradientAngle);
		}

		protected virtual void PaintBackground(SplitterPaintInfo info)
		{
			int gradientAngle=info.Colors.BackColorGradientAngle;
			Rectangle r=info.DisplayRectangle;

			if(info.Dock==DockStyle.Top || info.Dock==DockStyle.Bottom)
				gradientAngle+=90;

			if(info.Colors.BackColor2.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(info.Colors.BackColor))
					info.Graphics.FillRectangle(brush,r);
			}
			else
			{
				using(System.Drawing.Drawing2D.LinearGradientBrush brush=CreateLinearGradientBrush(r,info.Colors.BackColor,info.Colors.BackColor2,gradientAngle))
					info.Graphics.FillRectangle(brush,r);
			}
		}
		#endregion

		#region Properties
		protected int ArrowSize
		{
			get {return m_ArrowSize;}
			set {m_ArrowSize=value;}
		}
		#endregion
	}

	#region SplitterPaintInfo
	/// <summary>
	/// Represents class that holds information neccessary to paint the expandable splitter.
	/// </summary>
	internal class SplitterPaintInfo
	{
		/// <summary>
		/// Specifies reference to graphics canvas.
		/// </summary>
		public System.Drawing.Graphics Graphics;
		/// <summary>
		/// Specifies splitter display rectangle.
		/// </summary>
		public System.Drawing.Rectangle DisplayRectangle;
		/// <summary>
		/// Holds color settings for painting.
		/// </summary>
		public SplitterColors Colors;
		/// <summary>
		/// Specifies whether splitter is expandable or not.
		/// </summary>
		public bool Expandable;
		/// <summary>
		/// Specifies whether splitter is expanded or not.
		/// </summary>
		public bool Expanded;
		/// <summary>
		/// Specifies the splitter dock.
		/// </summary>
		public System.Windows.Forms.DockStyle Dock;
	}
	#endregion

	#region SplitterColors
	/// <summary>
	/// Represents class that holds colors for the splitter display.
	/// </summary>
	internal class SplitterColors
	{
		/// <summary>
		/// Specifies back color.
		/// </summary>
		public Color BackColor=Color.Empty;
		/// <summary>
		/// Specifies target gradient background color.
		/// </summary>
		public Color BackColor2=Color.Empty;
		/// <summary>
		/// Specifies background gradient angle.
		/// </summary>
		public int BackColorGradientAngle=0;
		/// <summary>
		/// Specifies grip part dark color.
		/// </summary>
		public Color GripDarkColor=Color.Empty;
		/// <summary>
		/// Specifies grip part light color.
		/// </summary>
		public Color GripLightColor=Color.Empty;
		/// <summary>
		/// Specifies expand part line color.
		/// </summary>
		public Color ExpandLineColor=Color.Empty;
		/// <summary>
		/// Specifies expand part fill color.
		/// </summary>
		public Color ExpandFillColor=Color.Empty;
	}
	#endregion
}
