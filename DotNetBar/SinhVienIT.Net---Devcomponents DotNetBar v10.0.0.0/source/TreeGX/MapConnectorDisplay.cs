using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree
{
	/// <summary>
	/// Represents the connector display for map type.
	/// </summary>
	public class MapConnectorDisplay:NodeConnectorDisplay
	{
		private int m_RootNodeConnectorWidth=4;
		public MapConnectorDisplay()
		{
		}

		/// <summary>
		/// Draws connector line between two nodes.
		/// </summary>
		/// <param name="info">Connector context information.</param>
		public override void DrawConnector(NodeConnectorDisplayInfo info)
		{
			if(info.LineColor.IsEmpty && info.FillColor.IsEmpty)
				return;

			DrawCurveConnector(info);
		}

		private void DrawCurveConnector(NodeConnectorDisplayInfo info)
		{
			Point pStart=this.GetStartPoint(info);
			Point pEnd=this.GetEndPoint(info);

			pStart.Offset(info.Offset.X,info.Offset.Y);
			pEnd.Offset(info.Offset.X,info.Offset.Y);

			// Determine whether curve can be drawn
			if(Math.Abs(pStart.X-pEnd.X)<=6 || Math.Abs(pStart.Y-pEnd.Y)<=10)
			{
				using(Pen pen=this.GetCurvePen(info))
				{
					info.Graphics.DrawLine(pen,pStart,pEnd);
				}
			}
			else
			{
				// Create two points in between the start and end point
				Point[] p=new Point[4];
				int xMulti=1, yMulti=1;
				
				// used for direction control
				if(pStart.X>pEnd.X)
					xMulti=-1;
				if(pStart.Y>pEnd.Y)
					yMulti=-1;
				
                p[1].X=pStart.X+(int)(Math.Abs(pStart.X-pEnd.X)*.15f*xMulti);
				p[1].Y=pStart.Y+(int)(Math.Abs(pStart.Y-pEnd.Y)*.15f*yMulti);

				p[2].X=pStart.X+(int)(Math.Abs(pStart.X-pEnd.X)*.5f*xMulti);
				p[2].Y=pEnd.Y-(int)(yMulti*(Math.Abs(pStart.Y-pEnd.Y)*.15f));

				p[0]=pStart;
				p[3]=pEnd;

				if(info.IsRootNode && m_RootNodeConnectorWidth>1)
				{
					GraphicsPath path=new GraphicsPath();
					path.AddCurve(p,.5f);
					path.AddLine(p[0].X,p[0].Y,p[0].X+m_RootNodeConnectorWidth*xMulti,p[0].Y);
					p[0].X+=(m_RootNodeConnectorWidth*xMulti);
					p[1].X+=(m_RootNodeConnectorWidth*xMulti);
					p[2].X+=(m_RootNodeConnectorWidth*xMulti);
					p[3].Y-=yMulti;
					path.AddCurve(p,.5f);
					path.AddLine(p[3].X,p[3].Y,p[3].X,p[3].Y+yMulti);
					path.CloseAllFigures();
					using(Brush brush=this.GetCurveBrush(info))
					{
						info.Graphics.FillPath(brush,path);
					}
				}
				else
				{
					using(Pen pen=this.GetCurvePen(info))
					{
						info.Graphics.DrawCurve(pen,p,.5f);
					}
				}
			}
		}

		private Pen GetCurvePen(NodeConnectorDisplayInfo info)
		{
			return new Pen(info.LineColor,1);
		}

		private Brush GetCurveBrush(NodeConnectorDisplayInfo info)
		{
			return new SolidBrush(info.LineColor);
		}

		private Point GetStartPoint(NodeConnectorDisplayInfo info)
		{
			Point p=Point.Empty;

			if(info.IsRootNode)
			{
				int toMidPoint=info.ToNode.Bounds.Top+info.ToNode.Bounds.Height/2;
				if(info.FromNode.Bounds.Top>toMidPoint)
					p=new Point(info.FromNode.Bounds.Left+info.FromNode.Bounds.Width/2,info.FromNode.Bounds.Top);
				else if(info.FromNode.Bounds.Bottom<toMidPoint)
					p=new Point(info.FromNode.Bounds.Left+info.FromNode.Bounds.Width/2,info.FromNode.Bounds.Bottom-1);
			}

			if(p.IsEmpty)
			{
				// To element to the Left of from node
				if(this.IsOnLeftSide(info.FromNode,info.ToNode))
					p=new Point(info.FromNode.Bounds.Left,info.FromNode.Bounds.Top+info.FromNode.Bounds.Height/2);
				else
					p=new Point(info.FromNode.Bounds.Right,info.FromNode.Bounds.Top+info.FromNode.Bounds.Height/2);
			}

			return p;
		}

		private Point GetEndPoint(NodeConnectorDisplayInfo info)
		{
			// If to element is to the right of the from node and has left border end point is the vertical mid-point
			// If to element is to the left of the from node and has right border end point is the vertical mid-point
			// If there is no border end point is text bottom

			Point p=Point.Empty;

			if(this.IsOnLeftSide(info.FromNode,info.ToNode))
			{
				// To element is to the left of from node
				Rectangle r=info.ToNode.Bounds;
				if(info.StyleToNode==null || !info.StyleToNode.PaintRightBorder)
					r=info.ToNode.Cells[0].TextContentBounds;

				p=new Point(r.Right,r.Y+r.Height/2);
			}
			else
			{
				// To element to the right of from node
				Rectangle r=info.ToNode.Bounds;
				if(info.StyleToNode==null || !info.StyleToNode.PaintLeftBorder)
					r=info.ToNode.Cells[0].TextContentBounds;
				
				p=new Point(r.X,r.Y+r.Height/2);
			}

			return p;
		}

		private bool IsOnLeftSide(Node source, Node target)
		{
			if((source.Bounds.Left+source.Bounds.Width/2)>target.Bounds.Left)
				return true;
			return false;
		}
	}
}
