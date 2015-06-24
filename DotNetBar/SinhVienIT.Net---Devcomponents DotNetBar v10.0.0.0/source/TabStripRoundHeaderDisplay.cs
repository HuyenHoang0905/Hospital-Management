using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class performing the display of tabs with RoundHeader style.
	/// </summary>
	internal class TabStripRoundHeaderDisplay:TabStripBaseDisplay
	{
		#region Methods
		public override void Paint(Graphics g, TabStrip tabStrip)
		{
			base.Paint(g,tabStrip);

			TabsCollection tabs=tabStrip.Tabs;
			TabColorScheme colorScheme=tabStrip.ColorScheme;
			Rectangle clientRect=tabStrip.ClientRectangle;
			
			Rectangle r=tabStrip.DisplayRectangle;
			r.Inflate(2,2);
			using(SolidBrush brush=new SolidBrush(tabStrip.BackColor))
				g.FillRectangle(brush,r);
			//g.Clear(tabStrip.BackColor);

			TabItem lastTab=GetLastVisibleTab(tabs);

            Rectangle selectedRect = Rectangle.Empty;
            if (tabStrip.SelectedTab != null)
                selectedRect = tabStrip.SelectedTab.DisplayRectangle;
            DrawBackground(tabStrip, clientRect, g, colorScheme, GetTabsRegion(tabs, lastTab), tabStrip.TabAlignment, selectedRect);

			bool bFirstVisible=true;

			foreach(TabItem tab in tabs)
			{
				if(!tab.Visible)
					continue;
                if(tab.DisplayRectangle.IntersectsWith(clientRect))
				    PaintTab(g,tab,bFirstVisible,(tab==lastTab));
				
				bFirstVisible=false;
			}
		}

		protected override Region GetTabsRegion(TabsCollection tabs, TabItem lastTab)
		{
			bool bFirstVisible=true;
			Region tabRegion=new Region();
			tabRegion.MakeEmpty();
			foreach(TabItem tab in tabs)
			{
				if(!tab.Visible)
					continue;				
				GraphicsPath path=GetTabItemPath(tab,bFirstVisible,(tab==lastTab));
				tabRegion.Union(path);
				bFirstVisible=false;
			}
			return tabRegion;
		}

		protected override LinearGradientBrush CreateTabGradientBrush(Rectangle r,Color color1,Color color2,int gradientAngle)
		{
			LinearGradientBrush brush=base.CreateTabGradientBrush(r,color1,color2,gradientAngle);
			
			Blend blend=new Blend(5);
			blend.Factors=new float[]{0,.6f,1f,.6f,0f};
			blend.Positions=new float[]{0,.3f,.5f,.7f,1f};
			brush.Blend=blend;

			return brush;
		}

        protected override void DrawBackground(TabStrip tabStrip, Rectangle tabStripRect, Graphics g, TabColorScheme colors, Region tabsRegion, eTabStripAlignment tabAlignment, Rectangle selectedTabRect)
		{
			int cornerDiameter=5;
			Rectangle br;
			GraphicsPath path=new GraphicsPath();
			if(tabAlignment==eTabStripAlignment.Top)
			{
				br=new Rectangle(tabStripRect.X,tabStripRect.Y+tabStripRect.Height/2,tabStripRect.Width-1,tabStripRect.Height-tabStripRect.Height/2);
				path.AddLine(br.X,br.Bottom,br.X,br.Top+cornerDiameter);
				ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopLeft);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.X+cornerDiameter,br.Y,br.Right-cornerDiameter,br.Y);
				ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopRight);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.Right,br.Y+cornerDiameter,br.Right,br.Bottom);
			}
			else if(tabAlignment==eTabStripAlignment.Bottom)
			{
				br=new Rectangle(tabStripRect.X,tabStripRect.Y,tabStripRect.Width-1,tabStripRect.Height/2);
				path.AddLine(br.Right,br.Y,br.Right,br.Bottom-cornerDiameter);
				ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomRight);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.Right-cornerDiameter,br.Bottom,br.X+cornerDiameter,br.Bottom);
				ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomLeft);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.X,br.Bottom-cornerDiameter,br.X,br.Y);
			}
			else if(tabAlignment==eTabStripAlignment.Left)
			{
				br=new Rectangle(tabStripRect.X+tabStripRect.Width/2,tabStripRect.Y,tabStripRect.Width/2+1,tabStripRect.Height-1);
				path.AddLine(br.Right,br.Bottom,br.X+cornerDiameter,br.Bottom);
				ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomLeft);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.X,br.Bottom-cornerDiameter,br.X,br.Y+cornerDiameter);
				ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopLeft);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.X+cornerDiameter,br.Y,br.Right,br.Y);
			}
			else if(tabAlignment==eTabStripAlignment.Right)
			{
				br=new Rectangle(tabStripRect.X,tabStripRect.Y,tabStripRect.Width/2,tabStripRect.Height-1);
				path.AddLine(br.X,br.Y,br.Right-cornerDiameter,br.Y);
				ArcData ad=GetCornerArc(br,cornerDiameter,eCornerArc.TopRight);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.Right,br.Y+cornerDiameter,br.Right,br.Bottom-cornerDiameter);
				ad=GetCornerArc(br,cornerDiameter,eCornerArc.BottomRight);
				path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
				path.AddLine(br.Right-cornerDiameter,br.Bottom,br.X,br.Bottom);
			}

			// Paint background of the tab control
			GraphicsPath backPath=path.Clone() as GraphicsPath;
			backPath.CloseAllFigures();
			g.SetClip(backPath,CombineMode.Replace);
			g.SetClip(tabsRegion,CombineMode.Exclude);
			g.Clear(colors.TabPanelBackground);
			g.ResetClip();
			backPath.Dispose();

			g.SetClip(tabsRegion,CombineMode.Exclude);
			
			using(Pen pen=new Pen(colors.TabBorder,1))
				g.DrawPath(pen,path);
			path.Dispose();
			g.ResetClip();
		}
		#endregion

		#region Tab Path Functions
		protected override GraphicsPath GetTabItemPath(TabItem tab, bool bFirst, bool bLast)
		{
			GraphicsPath path=new GraphicsPath();
			int cornerDiameter=3;
			
			Rectangle clientRectangle=tab.DisplayRectangle;

			if(!bFirst && !bLast)
				path.AddRectangle(clientRectangle);
			else
			{
				if(bFirst)
				{
					if(tab.Parent.TabAlignment==eTabStripAlignment.Top || tab.Parent.TabAlignment==eTabStripAlignment.Bottom)
					{
						path.AddLine(clientRectangle.Right-(bLast?cornerDiameter:0),clientRectangle.Bottom,clientRectangle.X+cornerDiameter,clientRectangle.Bottom);
						ArcData ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.BottomLeft);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.X,clientRectangle.Bottom-cornerDiameter,clientRectangle.X,clientRectangle.Y+cornerDiameter);
						ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.TopLeft);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.X+cornerDiameter,clientRectangle.Y,clientRectangle.Right-(bLast?cornerDiameter:0),clientRectangle.Y);
					}
					else
					{
						ArcData ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.TopLeft);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.X+cornerDiameter,clientRectangle.Y,clientRectangle.Right-cornerDiameter,clientRectangle.Y);
						ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.TopRight);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.Right,clientRectangle.Y+cornerDiameter,clientRectangle.Right,clientRectangle.Bottom-(bLast?cornerDiameter:0));
					}
				}
				else
					path.AddLine(clientRectangle.X,clientRectangle.Y,clientRectangle.Right,clientRectangle.Y);
				if(bLast)
				{
					if(tab.Parent.TabAlignment==eTabStripAlignment.Top || tab.Parent.TabAlignment==eTabStripAlignment.Bottom)
					{
						if(!bFirst)
							path.AddLine(clientRectangle.X,clientRectangle.Y,clientRectangle.Right-cornerDiameter,clientRectangle.Y);
						ArcData ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.TopRight);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.Right,clientRectangle.Y+cornerDiameter,clientRectangle.Right,clientRectangle.Bottom-cornerDiameter);
						ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.BottomRight);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						if(!bFirst)
							path.AddLine(clientRectangle.Right-cornerDiameter,clientRectangle.Bottom,clientRectangle.X,clientRectangle.Bottom);
					}
					else
					{
						if(!bFirst)
							path.AddLine(clientRectangle.Right,clientRectangle.Y,clientRectangle.Right,clientRectangle.Bottom-cornerDiameter);
						ArcData ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.BottomRight);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						path.AddLine(clientRectangle.Right-cornerDiameter,clientRectangle.Bottom,clientRectangle.X+cornerDiameter,clientRectangle.Bottom);
						ad=GetCornerArc(clientRectangle,cornerDiameter,eCornerArc.BottomLeft);
						path.AddArc(ad.X,ad.Y,ad.Width,ad.Height,ad.StartAngle,ad.SweepAngle);
						if(!bFirst)
							path.AddLine(clientRectangle.X,clientRectangle.Bottom-cornerDiameter,clientRectangle.X,clientRectangle.Y);
					}
				}
				else
					path.AddLine(clientRectangle.Right,clientRectangle.Bottom,clientRectangle.X,clientRectangle.Bottom);
				path.CloseAllFigures();

			}

			return path;
		}
		#endregion
	}
}
