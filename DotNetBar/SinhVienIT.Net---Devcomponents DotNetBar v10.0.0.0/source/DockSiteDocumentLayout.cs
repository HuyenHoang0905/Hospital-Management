using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Provides fill layout for the dock site.
	/// </summary>
	public class DockSiteDocumentLayout
	{
		private int m_BarSpacing=2;
		private eOrientation m_Orientation=eOrientation.Horizontal;

		public DockSiteDocumentLayout()
		{
		}

		public void Layout(DockSite site)
		{
			if(m_Orientation==eOrientation.Horizontal)
				LayoutHorizontal(site);
			//else
			//	LayoutVertical(site);
		}

		private void LayoutHorizontal(DockSite site)
		{
			int height=site.ClientRectangle.Height;
			Rectangle containerRectangle=site.ClientRectangle;
			int totalWidth=0, specifiedWidth=0;
			int variableCount=0;
			ArrayList visibleBars=new ArrayList();

			// Calculate fixed/specified and variable width
			for(int i=0;i<site.Controls.Count;i++)
			{
				Bar bar=site.Controls[i] as Bar;
				bar.SetDockLine(0);
				if(bar==null || !bar.Visible)
					continue;
				if(bar.SplitDockWidth>0)
				{
					totalWidth+=bar.SplitDockWidth;
					specifiedWidth+=bar.SplitDockWidth;
				}
				else
				{
					totalWidth+=bar.MinimumSize(m_Orientation).Width;
					variableCount++;
				}

				if(i+1<site.Controls.Count)
					totalWidth+=m_BarSpacing;
				visibleBars.Add(bar);
			}
			if(specifiedWidth>0 && visibleBars.Count-variableCount-1>0)
				specifiedWidth+=(visibleBars.Count-variableCount-1)*m_BarSpacing;

			int x=containerRectangle.X, y=containerRectangle.Y;

			int variableWidth=0;
			int totalVariableWidth=(containerRectangle.Width-specifiedWidth-m_BarSpacing*(variableCount-1));
			if(variableCount>0)
				variableWidth=totalVariableWidth/variableCount;


			int availableWidth=containerRectangle.Width;
			int count=visibleBars.Count;
			for(int i=0;i<count;i++)
			{
				Bar bar=visibleBars[i] as Bar;
				bar.Location=new Point(x,y);
				
				if(i+1==count)
					variableWidth=availableWidth;

				if(bar.SplitDockWidth>0)
				{
					if(i+1==count)
						bar.RecalcSize(new Size(availableWidth,containerRectangle.Height));
					else
						bar.RecalcSize(new Size(bar.SplitDockWidth,containerRectangle.Height));
				}
				else
				{
					bar.SplitDockWidth=variableWidth;
					bar.RecalcSize(new Size(variableWidth,containerRectangle.Height));
					totalVariableWidth-=variableWidth;
				}
				x+=(m_BarSpacing+bar.Width);
				availableWidth-=(m_BarSpacing+bar.Width);
			}
		}

		public eOrientation Orientation
		{
			get {return m_Orientation;}
			set {m_Orientation=value;}
		}
	}
}
