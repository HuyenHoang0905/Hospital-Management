using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Provides display capabilites for TabStrip with VS2005Dock style.
	/// </summary>
	internal class TabStripVS2005DockDisplay:TabStripBaseDisplay
	{
		#region Private Variable
		private int m_xTabOffset=8;
		#endregion
		/// <summary>
		/// Creates new instance of the class.
		/// </summary>
		public TabStripVS2005DockDisplay(){}

		#region Methods
		public override void Paint(Graphics g, TabStrip tabStrip)
		{
			base.Paint(g,tabStrip);

			TabColorScheme colorScheme=tabStrip.ColorScheme;
			Rectangle clientRect=tabStrip.DisplayRectangle;

            if (tabStrip.BackColor != Color.Transparent || colorScheme.TabBackground != Color.Transparent)
            {
                if (colorScheme.TabPanelBackground2.IsEmpty)
                {
                    if (!colorScheme.TabPanelBackground.IsEmpty)
                    {
                        using (SolidBrush brush = new SolidBrush(colorScheme.TabPanelBackground))
                            g.FillRectangle(brush, clientRect);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        g.FillRectangle(brush, clientRect);
                    using (LinearGradientBrush brush = CreateTabGradientBrush(clientRect, colorScheme.TabPanelBackground, colorScheme.TabPanelBackground2, colorScheme.TabPanelBackgroundGradientAngle))
                        g.FillRectangle(brush, clientRect);
                }
            }

            Rectangle selectedRect = Rectangle.Empty;
            if (tabStrip.SelectedTab != null)
                selectedRect = tabStrip.SelectedTab.DisplayRectangle;
			DrawBackground(tabStrip, clientRect,g,colorScheme,new Region(tabStrip.DisplayRectangle),tabStrip.TabAlignment, selectedRect);

            tabStrip.ClipExcludeSystemBox(g);

			for(int i=tabStrip.Tabs.Count-1;i>=0;i--)
			{
				TabItem tab=tabStrip.Tabs[i];

				if(!tab.Visible || tab==tabStrip.SelectedTab || !tab.DisplayRectangle.IntersectsWith(clientRect))
					continue;
			
				PaintTab(g,tab,false,false);				
			}

			if(tabStrip.SelectedTab!=null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
			{
				PaintTab(g,tabStrip.SelectedTab,false,false);
			}

			g.ResetClip();
			tabStrip.PaintTabSystemBox(g);
		}

		protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
		{
			Rectangle r=tab.DisplayRectangle;
			if(tab.TabAlignment==eTabStripAlignment.Right)
				r=new Rectangle(r.X,r.Y,r.Height,r.Width);
			else if(tab.TabAlignment==eTabStripAlignment.Left)
                r=new Rectangle(r.X,r.Y,r.Height,r.Width);

			r.Offset(0,1);

			GraphicsPath path=new GraphicsPath();
			
			// Left line
			path.AddPath(GetLeftLine(r),true);
//			path.AddLine(r.X-m_xTabOffset,r.Bottom,r.X,r.Y+5);
//			Point[] pc=new Point[3];
//			pc[0]=new Point(r.X,r.Y+5);
//			pc[1]=new Point(r.X+2,r.Y+2);
//			pc[2]=new Point(r.X+5,r.Y);
//			path.AddCurve(pc,.9f);

			// Top line
			path.AddLine(r.X+6,r.Y,r.Right-5,r.Y);

			// Right line
//			pc[0]=new Point(r.Right-5,r.Y);
//			pc[1]=new Point(r.Right-2,r.Y+2);
//			pc[2]=new Point(r.Right,r.Y+5);
//			path.AddCurve(pc,.9f);
//			path.AddLine(r.Right,r.Y+5,r.Right+m_xTabOffset,r.Bottom);
			path.AddPath(GetRightLine(r),true);

			// Bottom line
			path.AddLine(r.Right+m_xTabOffset,r.Bottom,r.X-m_xTabOffset,r.Bottom);

			path.CloseAllFigures();

			if(tab.TabAlignment==eTabStripAlignment.Bottom)
			{
				// Bottom
				Matrix m=new Matrix();
				//RectangleF rf=path.GetBounds();
				m.RotateAt(180,new PointF(r.X+r.Width/2,r.Y+r.Height/2));
				path.Transform(m);
			}
			else if(tab.TabAlignment==eTabStripAlignment.Left)
			{
				// Left
				Matrix m=new Matrix();
				//RectangleF rf=path.GetBounds();
				m.RotateAt(-90,new PointF(r.X,r.Bottom));
				m.Translate(r.Height,r.Width-r.Height,MatrixOrder.Append);
				path.Transform(m);
			}
			else if(tab.TabAlignment==eTabStripAlignment.Right)
			{
				// Right
				Matrix m=new Matrix();
				//RectangleF rf=path.GetBounds();
				m.RotateAt(90,new PointF(r.Right,r.Bottom));
				m.Translate(-r.Width,r.Width-(r.Height-1),MatrixOrder.Append);
				path.Transform(m);
			}

			return path;
		}

		private GraphicsPath GetLeftLine(Rectangle r)
		{
			GraphicsPath path=new GraphicsPath();
			// Left line
			path.AddLine(r.X-m_xTabOffset,r.Bottom,r.X,r.Y+5);
			Point[] pc=new Point[3];
			pc[0]=new Point(r.X,r.Y+5);
			pc[1]=new Point(r.X+2,r.Y+2);
			pc[2]=new Point(r.X+5,r.Y);
			path.AddCurve(pc,.9f);
			return path;
		}

		private GraphicsPath GetRightLine(Rectangle r)
		{
			GraphicsPath path=new GraphicsPath();
			// Right line
			Point[] pc=new Point[3];
			pc[0]=new Point(r.Right-5,r.Y);
			pc[1]=new Point(r.Right-2,r.Y+2);
			pc[2]=new Point(r.Right,r.Y+5);
			path.AddCurve(pc,.9f);
			path.AddLine(r.Right,r.Y+5,r.Right+m_xTabOffset,r.Bottom);
			return path;
		}

        protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
		{
			base.DrawBackground(tabStrip, tabStripRect,g,colors,tabsRegion,tabAlignment, selectedTabRect);

			if(!colors.TabItemBorder.IsEmpty)
			{
				using(Pen pen=new Pen(colors.TabItemBorder,1))
				{
					if(tabAlignment==eTabStripAlignment.Bottom)
					{
						g.DrawLine(pen,tabStripRect.X,tabStripRect.Y+1,tabStripRect.Right,tabStripRect.Y+1);
					}
					else if(tabAlignment==eTabStripAlignment.Left)
					{
						g.DrawLine(pen,tabStripRect.Right-1,tabStripRect.Y,tabStripRect.Right-1,tabStripRect.Bottom);
					}
					else if(tabAlignment==eTabStripAlignment.Right)
					{
						g.DrawLine(pen,tabStripRect.X,tabStripRect.Y,tabStripRect.X,tabStripRect.Bottom);
					}
					else if(tabAlignment==eTabStripAlignment.Top)
					{
						g.DrawLine(pen,tabStripRect.X,tabStripRect.Bottom-1,tabStripRect.Right,tabStripRect.Bottom-1);
					}
				}
			}
		}
		#endregion

		
	}
}
