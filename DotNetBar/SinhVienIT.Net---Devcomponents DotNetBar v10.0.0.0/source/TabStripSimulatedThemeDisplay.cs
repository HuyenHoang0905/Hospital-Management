using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Provides display support for SimualtedTheme tab style.
	/// </summary>
	internal class TabStripSimulatedThemeDisplay:TabStripBaseDisplay
	{
		public TabStripSimulatedThemeDisplay()
		{
			this.HorizontalText=true;
		}

		#region Methods
		public override void Paint(Graphics g, TabStrip tabStrip)
		{
			base.Paint(g,tabStrip);

			TabColorScheme colorScheme=tabStrip.ColorScheme;
			Rectangle clientRect=tabStrip.ClientRectangle;

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

			tabStrip.ClipExcludeSystemBox(g);

			DrawBackgroundInternal(clientRect,g,colorScheme,new Region(tabStrip.DisplayRectangle),tabStrip.TabAlignment, tabStrip.TabItemsBounds);

			for(int i=tabStrip.Tabs.Count-1;i>=0;i--)
			{
				TabItem tab=tabStrip.Tabs[i];

				if(!tab.Visible || tab.IsSelected || !tab.DisplayRectangle.IntersectsWith(clientRect))
					continue;
			
				PaintTab(g,tab,false,false);
			}

			if(tabStrip.SelectedTab!=null && tabStrip.Tabs.Contains(tabStrip.SelectedTab))
				PaintTab(g,tabStrip.SelectedTab,false,false);

			g.ResetClip();
			tabStrip.PaintTabSystemBox(g);
		}


		protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
		{
			Rectangle r=tab.DisplayRectangle;

			GraphicsPath path=new GraphicsPath();

			if(!tab.IsSelected && !tab.IsMouseOver)
			{
				if(tab.TabAlignment==eTabStripAlignment.Bottom)
				{
					r.Height--;
				}
				else if(tab.TabAlignment==eTabStripAlignment.Top)
				{
					r.Height--;
					r.Y++;
				}
				else if(tab.TabAlignment==eTabStripAlignment.Left)
				{
					r.Width--;
					r.X++;
				}
				else if(tab.TabAlignment==eTabStripAlignment.Right)
				{
					r.Width--;
				}

				path.AddRectangle(r);
			}
			else
			{
				if(tab.TabAlignment==eTabStripAlignment.Bottom)
				{
					//r.Height++;
					path.AddLine(r.X+2,r.Bottom,r.X,r.Bottom-2);
					path.AddLine(r.X,r.Y,r.Right,r.Y);
					path.AddLine(r.Right,r.Bottom-2,r.Right-2,r.Bottom);
					path.AddLine(r.Right-2,r.Bottom,r.X+2,r.Bottom);
				}
				else if(tab.TabAlignment==eTabStripAlignment.Top)
				{
					//r.Height++;
					//r.Y--;
					path.AddLine(r.X,r.Y+2,r.X+2,r.Y);
					path.AddLine(r.Right-2,r.Y,r.Right,r.Y+2);
					path.AddLine(r.Right,r.Bottom,r.X,r.Bottom);
				}
				else if(tab.TabAlignment==eTabStripAlignment.Left)
				{
					//r.Width++;
					//r.X--;
					path.AddLine(r.Right,r.Bottom,r.X+2,r.Bottom);
					path.AddLine(r.X+2,r.Bottom,r.X,r.Bottom-2);
					path.AddLine(r.X,r.Bottom-2,r.X,r.Y+2);
					path.AddLine(r.X,r.Y+2,r.X+2,r.Y);
					path.AddLine(r.Right-1,r.Y,r.Right-1,r.Bottom);
				}
				else if(tab.TabAlignment==eTabStripAlignment.Right)
				{
					//r.Width++;
					path.AddLine(r.X,r.Y,r.Right-2,r.Y);
					path.AddLine(r.Right-2,r.Y,r.Right,r.Y+2);
					path.AddLine(r.Right,r.Y+2,r.Right,r.Bottom-2);
					path.AddLine(r.Right,r.Bottom-2,r.Right-2,r.Bottom);
					path.AddLine(r.X,r.Bottom,r.X,r.Y);
				}
				path.CloseAllFigures();
			}

			return path;
		}

		private GraphicsPath GetTabEdge(TabItem tab)
		{
			GraphicsPath path=new GraphicsPath();
			Rectangle r=tab.DisplayRectangle;

			if(tab.TabAlignment==eTabStripAlignment.Bottom)
			{
				r.Height++;
				path.AddLine(r.Right,r.Bottom-2,r.Right-2,r.Bottom);
				path.AddLine(r.Right-2,r.Bottom,r.X+2,r.Bottom);
				path.AddLine(r.X+2,r.Bottom,r.X,r.Bottom-2);
			}
			else if(tab.TabAlignment==eTabStripAlignment.Top)
			{
				r.Height++;
				r.Y--;
				path.AddLine(r.X,r.Y+2,r.X+2,r.Y);
				path.AddLine(r.X+2,r.Y,r.Right-2,r.Y);
				path.AddLine(r.Right-2,r.Y,r.Right,r.Y+2);
				
			}
			else if(tab.TabAlignment==eTabStripAlignment.Left)
			{
				r.Width++;
				r.X--;
				//path.AddLine(r.Right,r.Bottom,r.X+2,r.Bottom);
				path.AddLine(r.X+2,r.Bottom,r.X,r.Bottom-2);
				path.AddLine(r.X,r.Bottom-2,r.X,r.Y+2);
				path.AddLine(r.X,r.Y+2,r.X+2,r.Y);
				//path.AddLine(r.Right,r.Y,r.Right,r.Bottom);
			}
			else if(tab.TabAlignment==eTabStripAlignment.Right)
			{
				r.Width++;
				//path.AddLine(r.X,r.Y,r.Right-2,r.Y);
				path.AddLine(r.Right-2,r.Y,r.Right,r.Y+2);
				path.AddLine(r.Right,r.Y+2,r.Right,r.Bottom-2);
				path.AddLine(r.Right,r.Bottom-2,r.Right-2,r.Bottom);
				//path.AddLine(r.X,r.Bottom,r.X,r.Y);
			}
			//path.CloseAllFigures();

			return path;
		}

		private void DrawBackgroundInternal(Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle tabItemsBounds)
		{
			int cornerDiameter=3;
			Rectangle br;
			GraphicsPath path=new GraphicsPath();
			GraphicsPath backPath=null;

			if(!tabItemsBounds.IsEmpty)
			{
				if(tabAlignment==eTabStripAlignment.Top)
				{
					tabItemsBounds.Width+=3;
					br=new Rectangle(tabStripRect.X,tabItemsBounds.Y+1,(tabItemsBounds.Right>tabStripRect.Right?tabStripRect.Width:tabItemsBounds.Right),tabItemsBounds.Height-1);
					path.AddLine(br.X,br.Bottom,br.X,br.Top+cornerDiameter);
					ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopLeft);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.X+cornerDiameter,br.Y,br.Right-cornerDiameter,br.Y);
					ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopRight);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.Right,br.Y+cornerDiameter,br.Right,br.Bottom);
					
					backPath=path.Clone() as GraphicsPath;
					backPath.CloseAllFigures();

					if(tabItemsBounds.Right<tabStripRect.Right)
						path.AddLine(br.Right,br.Bottom,tabStripRect.Right,br.Bottom);
				}
				else if(tabAlignment==eTabStripAlignment.Bottom)
				{
					tabItemsBounds.Width+=3;
					br=new Rectangle(tabStripRect.X,tabStripRect.Y,(tabItemsBounds.Right>tabStripRect.Right?tabStripRect.Width:tabItemsBounds.Right),tabItemsBounds.Height-1);
					path.AddLine(br.Right,br.Y,br.Right,br.Bottom-cornerDiameter);
					ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomRight);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.Right-cornerDiameter,br.Bottom,br.X+cornerDiameter,br.Bottom);
					ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomLeft);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.X,br.Bottom-cornerDiameter,br.X,br.Y);

					backPath=path.Clone() as GraphicsPath;
					backPath.CloseAllFigures();

					if(tabItemsBounds.Right<tabStripRect.Right)
						path.AddLine(br.Right,br.Y,tabStripRect.Right,br.Y);
				}
				else if(tabAlignment==eTabStripAlignment.Left)
				{
					tabItemsBounds.Height+=3;
					br=new Rectangle(tabItemsBounds.X+1,tabStripRect.Y,tabItemsBounds.Width-1,(tabItemsBounds.Bottom>tabStripRect.Bottom?tabStripRect.Height:tabItemsBounds.Bottom));
                    
                    path.AddLine(br.Right-1,br.Bottom,br.X+cornerDiameter,br.Bottom);
                    ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomLeft);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.X,br.Bottom-cornerDiameter,br.X,br.Y+cornerDiameter);
					ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopLeft);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
                    path.AddLine(br.X+cornerDiameter,br.Y,br.Right-1,br.Y);

					backPath=path.Clone() as GraphicsPath;
					backPath.CloseAllFigures();

					if(tabItemsBounds.Bottom<tabStripRect.Bottom)
						path.AddLine(br.Right-1,br.Bottom,br.Right-1,tabStripRect.Bottom);
				}
				else if(tabAlignment==eTabStripAlignment.Right)
				{
					tabItemsBounds.Height+=3;
					br=new Rectangle(tabStripRect.X,tabStripRect.Y,tabItemsBounds.Width-1,(tabItemsBounds.Bottom>tabStripRect.Bottom?tabStripRect.Height:tabItemsBounds.Bottom));
					path.AddLine(br.X,br.Y,br.Right-cornerDiameter,br.Y);
					ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopRight);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.Right,br.Y+cornerDiameter,br.Right,br.Bottom-cornerDiameter);
					ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomRight);
					path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
					path.AddLine(br.Right-cornerDiameter,br.Bottom,br.X,br.Bottom);

					backPath=path.Clone() as GraphicsPath;
					backPath.CloseAllFigures();

					if(tabItemsBounds.Bottom<tabStripRect.Bottom)
						path.AddLine(br.X,br.Bottom,br.X,tabStripRect.Bottom);
				}
			}
			else
			{
				path.AddRectangle(tabStripRect);
			}

			// Paint background of the tab control
			//g.SetClip(backPath,CombineMode.Replace);
			//g.SetClip(tabsRegion,CombineMode.Exclude);
			
			if(colors.TabPanelBackground2.IsEmpty)
			{
				if(!colors.TabPanelBackground.IsEmpty)
				{
					using(SolidBrush brush=new SolidBrush(colors.TabPanelBackground))
						g.FillPath(brush,path);
				}
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(Color.White))
					g.FillPath(brush,path);
				using(LinearGradientBrush brush=CreateTabGradientBrush(tabStripRect,colors.TabPanelBackground,colors.TabPanelBackground2,colors.TabPanelBackgroundGradientAngle))
					g.FillPath(brush,path);
			}

			if(!colors.TabBorder.IsEmpty)
			{
				path.CloseAllFigures();
				using(Pen pen=new Pen(colors.TabBorder,1))
					g.DrawPath(pen,path);
			}

            if (backPath != null)
                backPath.Dispose();
            if (path != null)
                path.Dispose();
		}

		protected override void DrawTabItemBackground(TabItem tab, GraphicsPath path, TabColors colors, Graphics g)
		{
			RectangleF rf=path.GetBounds();
			Rectangle tabRect=new Rectangle((int)rf.X, (int)rf.Y, (int)rf.Width, (int)rf.Height);

			if(colors.BackColor2.IsEmpty)
			{
				if(!colors.BackColor.IsEmpty)
				{
					using(SolidBrush brush=new SolidBrush(colors.BackColor))
						g.FillPath(brush,path);
				}
			}
			else
			{
				using(SolidBrush brush=new SolidBrush(Color.White))
					g.FillPath(brush,path);
				using(LinearGradientBrush brush=CreateTabGradientBrush(tabRect,colors.BackColor,colors.BackColor2,colors.BackColorGradientAngle))
					g.FillPath(brush,path);
			}

			using(Pen pen=new Pen(colors.BorderColor,1))
			{
				if(!colors.BorderColor.IsEmpty && (tab.IsSelected || tab.IsMouseOver))
				{
					g.DrawPath(pen,path);
				}
				else
				{
					if(tab.TabAlignment==eTabStripAlignment.Top || tab.TabAlignment==eTabStripAlignment.Bottom)
					{
						if(!colors.LightBorderColor.IsEmpty)
						{
							using(Pen border=new Pen(colors.LightBorderColor,1))
								g.DrawLine(border,tabRect.Right-1,tabRect.Y+4,tabRect.Right-1,tabRect.Bottom-4);
						}
						if(!colors.DarkBorderColor.IsEmpty)
						{
							using(Pen border=new Pen(colors.DarkBorderColor,1))
								g.DrawLine(border,tabRect.Right,tabRect.Y+4,tabRect.Right,tabRect.Bottom-4);
						}

						g.DrawLine(pen,tabRect.X,tabRect.Y,tabRect.Right,tabRect.Y);
						g.DrawLine(pen,tabRect.X,tabRect.Bottom,tabRect.Right,tabRect.Bottom);
					}
					else if(tab.TabAlignment==eTabStripAlignment.Left || tab.TabAlignment==eTabStripAlignment.Right)
					{
						if(!colors.LightBorderColor.IsEmpty)
						{
							using(Pen border=new Pen(colors.LightBorderColor,1))
								g.DrawLine(border,tabRect.X+4,tabRect.Bottom-1,tabRect.Right-4,tabRect.Bottom-1);
						}
						if(!colors.DarkBorderColor.IsEmpty)
						{
							using(Pen border=new Pen(colors.DarkBorderColor,1))
								g.DrawLine(border,tabRect.X+4,tabRect.Bottom,tabRect.Right-4,tabRect.Bottom);
						}

						g.DrawLine(pen,tabRect.X,tabRect.Y,tabRect.X,tabRect.Bottom);
						if(tab.TabAlignment==eTabStripAlignment.Left)
							g.DrawLine(pen,tabRect.Right-1,tabRect.Y,tabRect.Right-1,tabRect.Bottom);
						else
							g.DrawLine(pen,tabRect.Right,tabRect.Y,tabRect.Right,tabRect.Bottom);
					}
				}
			}

			if(tab.IsSelected || tab.IsMouseOver)
			{
				GraphicsPath edge=GetTabEdge(tab);
				if(!colors.LightBorderColor.IsEmpty)
				{
					GraphicsPath edgeFill=edge.Clone() as GraphicsPath;
					edgeFill.CloseAllFigures();
					using(SolidBrush brush=new SolidBrush(colors.LightBorderColor))
						g.FillPath(brush,edgeFill);
					edgeFill.Dispose();
				}
				if(!colors.DarkBorderColor.IsEmpty)
				{
					using(Pen pen=new Pen(colors.DarkBorderColor,1))
						g.DrawPath(pen,edge);
				}
				edge.Dispose();
			}
		}

		#endregion

	}
}
