using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents the connector display for map type.
	/// </summary>
	public class CurveConnectorDisplay:NodeConnectorDisplay
	{		
		/// <summary>
		/// Draws connector line between two nodes.
		/// </summary>
		/// <param name="info">Connector context information.</param>
		public override void DrawConnector(ConnectorRendererEventArgs info)
		{
			if(info.NodeConnector.LineColor.IsEmpty || info.NodeConnector.LineWidth<=0)
				return;

			DrawCurveConnector(info);
		}

		private void DrawCurveConnector(ConnectorRendererEventArgs info)
		{
			Point pStart=this.GetStartPoint(info);
			Point[] parr=this.GetEndPoint(info);
			Point pEnd=parr[0];
			Point pEndUnderLine=parr[1];
			int lineWidth=info.NodeConnector.LineWidth;

			int xMulti=1, yMulti=1;
					
			// used for direction control
			if(pStart.X>pEnd.X)
				xMulti=-1;
			if(pStart.Y>pEnd.Y)
				yMulti=-1;

			if(info.ConnectorPoints==null || info.ConnectorPoints.Count == 0)
			{
				// Determine whether curve can be drawn
				if(Math.Abs(pStart.X-pEnd.X)<=6 || Math.Abs(pStart.Y-pEnd.Y)<=10)
				{
					DrawLineConnector(info,pStart,pEnd,pEndUnderLine);
				}
				else
				{
					// Create two points in between the start and end point
					Point[] p=new Point[4];

					p[1].X=pStart.X+(int)(Math.Abs(pStart.X-pEnd.X)*.15f*xMulti);
					p[1].Y=pStart.Y+(int)(Math.Abs(pStart.Y-pEnd.Y)*.15f*yMulti);

					p[2].X=pStart.X+(int)(Math.Abs(pStart.X-pEnd.X)*.5f*xMulti);
					p[2].Y=pEnd.Y-(int)(yMulti*(Math.Abs(pStart.Y-pEnd.Y)*.15f));

					p[0]=pStart;
					p[3]=pEnd;

					if(lineWidth>1)
					{
						GraphicsPath path=new GraphicsPath();
						
						path.AddCurve(p,.5f);
						
						// Check whether pStart is starting from left or right side
						Rectangle fromNodeRect=info.FromNode.BoundsRelative;
						fromNodeRect.Offset(info.Offset);
						fromNodeRect.Inflate(-1,-1);
//						if(pStart.Y>fromNodeRect.Y && pStart.Y<fromNodeRect.Bottom && pStart.X>=fromNodeRect.Right)
//						{
//							path.AddLine(pStart.X, pStart.Y-lineWidth*xMulti, pStart.X, pStart.Y);
//						}
						
						path.AddLine(p[0].X,p[0].Y,p[0].X+lineWidth*xMulti,p[0].Y);
						
						if(pStart.Y>fromNodeRect.Y && pStart.Y<fromNodeRect.Bottom && pStart.X>=fromNodeRect.Right && info.IsRootNode)
						{
							path.AddLine(p[0].X-1,p[0].Y,p[0].X-1,p[0].Y+lineWidth*yMulti);
							p[0].Y -= lineWidth*yMulti;
						}
						else if(pStart.Y>fromNodeRect.Y && pStart.Y<fromNodeRect.Bottom && pStart.X<=fromNodeRect.Left && info.IsRootNode)
						{
							path.AddLine(p[0].X+1,p[0].Y,p[0].X+1,p[0].Y+lineWidth*yMulti);
							p[0].Y -= lineWidth*yMulti;
						}
						else
							p[0].X+=(lineWidth*xMulti);
						
						p[1].X+=(lineWidth*xMulti);
						p[2].X+=(lineWidth*xMulti);
						p[3].Y-=yMulti;
						path.AddCurve(p,.5f);
						path.AddLine(p[3].X,p[3].Y,p[3].X,p[3].Y+yMulti);
						path.CloseAllFigures();
						using(Brush brush=this.GetLineBrush(info))
						{
							info.Graphics.FillPath(brush,path);
						}
					}
					else
					{
						using(Pen pen=this.GetLinePen(info))
						{
							info.Graphics.DrawCurve(pen,p,.5f);
						}
					}
				}
			}
			else
			{
				ConnectorPointInfo pointInfo=GetConnectorPointInfo(info,pStart,pEnd);
				
				if(pointInfo.Points2==null)
				{
					using(Pen pen=this.GetLinePen(info))
					{
						info.Graphics.DrawCurve(pen,pointInfo.Points1,.5f);
					}
				}
				else
				{
					using(GraphicsPath path=new GraphicsPath())
					{
						path.AddCurve(pointInfo.Points1,.5f);
						path.AddCurve(pointInfo.Points2,.5f);
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
