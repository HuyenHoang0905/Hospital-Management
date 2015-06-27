using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.Tree.Display;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents the line connector display class.
	/// </summary>
	public class LineConnectorDisplay:NodeConnectorDisplay
	{
		/// <summary>
		/// Draws connector line between two nodes.
		/// </summary>
		/// <param name="info">Connector context information.</param>
		public override void DrawConnector(ConnectorRendererEventArgs info)
		{
			if(info.NodeConnector.LineColor.IsEmpty || info.NodeConnector.LineWidth<=0)
				return;

			DrawStarConnector(info);
		}

		private void DrawStarConnector(ConnectorRendererEventArgs info)
		{
			Point pStart=this.GetStartPoint(info);
			Point[] parr=this.GetEndPoint(info);
			Point pEnd=parr[0];
			Point pEndUnderLine=parr[1];

			if(info.ConnectorPoints==null)
			{
				// Determine whether curve can be drawn
				if(Math.Abs(pStart.X-pEnd.X)<=6 || Math.Abs(pStart.Y-pEnd.Y)<=10)
				{
					DrawLineConnector(info,pStart,pEnd,pEndUnderLine);
				}
				else
				{
					Point pBottom=new Point();
					Point pTop=new Point();
					if(pEnd.X<pStart.X)
					{
						pBottom.X=pStart.X-info.NodeConnector.LineWidth;
						pTop.X=pEnd.X+1;
					}
					else
					{
						pBottom.X=pStart.X+info.NodeConnector.LineWidth;
						pTop.X=pEnd.X-1;
					}
					pBottom.Y=pStart.Y;
					pTop.Y=pEnd.Y;

					GraphicsPath path=new GraphicsPath();
					path.AddLine(pStart,pBottom);
					path.AddLine(pBottom,pEnd);
					path.AddLine(pEnd,pTop);
					path.AddLine(pTop,pStart);
					path.CloseAllFigures();

					using(Brush brush=this.GetLineBrush(info))
						info.Graphics.FillPath(brush,path);
				}
			}
			else
			{
				ConnectorPointInfo pointInfo=GetConnectorPointInfo(info,pStart,pEnd);
				
				if(pointInfo.Points2==null)
				{
					using(Pen pen=this.GetLinePen(info))
					{
						info.Graphics.DrawLines(pen,pointInfo.Points1);
					}
				}
				else
				{
					using(GraphicsPath path=new GraphicsPath())
					{
						path.AddLines(pointInfo.Points1);
						path.AddLines(pointInfo.Points2);
						path.CloseAllFigures();

						using(Brush brush=this.GetLineBrush(info))
						{
							info.Graphics.FillPath(brush,path);
						}
					}
				}
			}

			DrawEndLine(info,pStart,pEnd,pEndUnderLine);
		}
	}
}
