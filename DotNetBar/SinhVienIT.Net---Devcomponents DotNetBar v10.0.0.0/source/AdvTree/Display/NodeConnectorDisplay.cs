using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree
{
	namespace Display
	{
		/// <summary>
		/// Base class for drawing node connectors.
		/// </summary>
		public abstract class NodeConnectorDisplay
		{
			//private bool m_RootNode=false;
			//private bool m_DrawRootAllLevels=false;
			//private bool m_EndCap=true;
			//private bool m_DrawConnectorUnderNodes=true;

			/// <summary>
			/// Creates new instance of the object.
			/// </summary>
			public NodeConnectorDisplay()
			{
			}

			/// <summary>
			/// Draws connector line between two nodes.
			/// </summary>
			/// <param name="info">Connector context information.</param>
			public virtual void DrawConnector(ConnectorRendererEventArgs info){}

            ///// <summary>
            ///// Returns the connector starting coordinates.
            ///// </summary>
            ///// <param name="info">Connector display information.</param>
            ///// <returns>Point object.</returns>
            //protected virtual Point GetStartPoint(ConnectorRendererEventArgs info)
            //{
            //    Point p=Point.Empty;

            //    if(info.IsRootNode)
            //    {
            //        //int toMidPoint=info.ToNode.Bounds.Top+info.ToNode.Bounds.Height/2;
            //        //if(info.FromNode.Bounds.Top>toMidPoint)
            //        if(IsAbove(info.FromNode,info.ToNode))
            //            p=new Point(info.FromNode.BoundsRelative.Left+info.FromNode.BoundsRelative.Width/2,info.FromNode.BoundsRelative.Top);
            //            //else if(info.FromNode.Bounds.Bottom<toMidPoint)
            //        else if(IsBelow(info.FromNode,info.ToNode))
            //            p=new Point(info.FromNode.BoundsRelative.Left+info.FromNode.BoundsRelative.Width/2,info.FromNode.BoundsRelative.Bottom-1);
            //    }

            //    if(p.IsEmpty)
            //    {
            //        // To element to the Left
            //        if(this.IsOnLeftSide(info.FromNode,info.ToNode))
            //            p=new Point(info.FromNode.BoundsRelative.Left,info.FromNode.BoundsRelative.Top+info.FromNode.BoundsRelative.Height/2);
            //        else
            //        {
            //            p=new Point(info.FromNode.BoundsRelative.Right,info.FromNode.BoundsRelative.Top+info.FromNode.BoundsRelative.Height/2);
            //            if(info.IsRootNode)
            //                p.X--;
            //            if(!NodeDisplay.DrawExpandPart(info.FromNode) && info.FromNode.ExpandVisibility==eNodeExpandVisibility.Auto)
            //                p.X-=(info.FromNode.BoundsRelative.Width-info.FromNode.ContentBounds.Width);
            //        }
            //    }

            //    if(!p.IsEmpty)
            //        p.Offset(info.Offset.X,info.Offset.Y);

            //    return p;
            //}

            ///// <summary>
            ///// Returns true if fromNode is above the toNode.
            ///// </summary>
            ///// <param name="fromNode">From Node object.</param>
            ///// <param name="toNode">To Node object</param>
            ///// <returns>True if fromNode is above toNode.</returns>
            //protected bool IsAbove(Node fromNode, Node toNode)
            //{
            //    //int toMidPoint=toNode.Bounds.Top+toNode.Bounds.Height/2;
            //    //if(fromNode.Bounds.Top>toMidPoint)
            //    if(fromNode.BoundsRelative.Top>toNode.BoundsRelative.Bottom)
            //        return true;
            //    return false;
            //}

            ///// <summary>
            ///// Returns true if fromNode is below toNode.
            ///// </summary>
            ///// <param name="fromNode">From Node object.</param>
            ///// <param name="toNode">To Node object.</param>
            ///// <returns>True if fromNode is below toNode.</returns>
            //protected bool IsBelow(Node fromNode, Node toNode)
            //{
            //    int toMidPoint=toNode.BoundsRelative.Top+toNode.BoundsRelative.Height/2;
            //    if(fromNode.BoundsRelative.Bottom<toMidPoint)
            //        return true;
            //    return false;
            //}

            ///// <summary>
            ///// Returns whether connector is extended to underline the node.
            ///// </summary>
            ///// <param name="nodeStyle">Refernce to Node style.</param>
            ///// <returns>True if node should be underlined by connector.</returns>
            //protected bool UnderlineNode(ElementStyle nodeStyle)
            //{
            //    if(!nodeStyle.PaintBottomBorder && !nodeStyle.PaintTopBorder &&
            //       !nodeStyle.PaintLeftBorder && !nodeStyle.PaintRightBorder)
            //        return true;
            //    return false;
            //}

            ///// <summary>
            ///// Returns the connector end point. The array of end points. Two valid points will be returned if node needs to be underlined by connector.
            ///// </summary>
            ///// <param name="info">Connector display info.</param>
            ///// <returns>Array of point objects.</returns>
            //protected Point[] GetEndPoint(ConnectorRendererEventArgs info)
            //{
            //    // If to element is to the right of the from node and has left border end point is the vertical mid-point
            //    // If to element is to the left of the from node and has right border end point is the vertical mid-point
            //    // If there is no border end point is text bottom
            //    // If this is link connector the end point is the middle bottom or top point of the node

            //    Point p=Point.Empty;
            //    Point pLineEnd=Point.Empty;
            //    int capWidthOffset = 0; // GetCapWidthOffset(info.NodeConnector.EndCap, info.NodeConnector.EndCapSize);
            //    bool leftSide=this.IsOnLeftSide(info.FromNode,info.ToNode);

            //    if(info.LinkConnector && info.FromNode.BoundsRelative.Top>info.ToNode.BoundsRelative.Bottom)
            //        p=new Point(info.ToNode.BoundsRelative.X+info.ToNode.BoundsRelative.Width/2+(leftSide?capWidthOffset:-capWidthOffset),info.ToNode.BoundsRelative.Bottom+1);
            //    else if(info.LinkConnector && info.FromNode.BoundsRelative.Bottom<info.ToNode.BoundsRelative.Top)
            //        p=new Point(info.ToNode.BoundsRelative.X+info.ToNode.BoundsRelative.Width/2+(leftSide?capWidthOffset:-capWidthOffset),info.ToNode.BoundsRelative.Top-info.NodeConnector.EndCapSize.Height);
            //    else
            //    {
            //        if(leftSide)
            //        {
            //            // To element is to the left of from node
            //            Rectangle r=info.ToNode.BoundsRelative;
            //            if(info.StyleToNode==null || UnderlineNode(info.StyleToNode))
            //            {
            //                p=new Point(r.Right,r.Bottom);
            //                if(m_EndCap)
            //                    p.X+=capWidthOffset;

            //                if(info.NodeConnector.UnderlineNoBorderNode)
            //                {
            //                    Rectangle rc=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,info.ToNode,Point.Empty);
            //                    pLineEnd=new Point(rc.Left+1,r.Bottom);
            //                }
            //            }
            //            else
            //            {
            //                p=new Point(r.Right,r.Y+r.Height/2);
            //                if(m_EndCap)
            //                    p.X+=capWidthOffset;
            //            }
            //        }
            //        else
            //        {
            //            // To element to the right of from node
            //            Rectangle r=info.ToNode.BoundsRelative;
            //            if(info.StyleToNode==null || UnderlineNode(info.StyleToNode))
            //            {
            //                //r=NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds,info.ToNode.Cells[0],Point.Empty);
            //                //r=info.ToNode.Cells[0].TextContentBounds;
            //                p=new Point(r.X,r.Bottom);
            //                if(m_EndCap)
            //                    p.X-=capWidthOffset;
            //                if(info.NodeConnector.UnderlineNoBorderNode)
            //                {
            //                    Rectangle rc=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,info.ToNode,Point.Empty);
            //                    pLineEnd=new Point(rc.Right-1,r.Bottom);
            //                }
            //            }
            //            else
            //            {
            //                p=new Point(r.X,r.Y+r.Height/2);
            //                if(m_EndCap)
            //                    p.X-=capWidthOffset;
            //            }
            //        }
            //    }
			
            //    if(!p.IsEmpty)
            //        p.Offset(info.Offset.X,info.Offset.Y);
            //    if(!pLineEnd.IsEmpty)
            //        pLineEnd.Offset(info.Offset.X,info.Offset.Y);

            //    return new Point[] {p,pLineEnd};
            //}

            ///// <summary>
            ///// Returns the offest for the node connector cap.
            ///// </summary>
            ///// <param name="cap">Cap type.</param>
            ///// <param name="size">Cap size.</param>
            ///// <returns></returns>
            //protected int GetCapWidthOffset(eConnectorCap cap,Size size)
            //{
            //    int capWidthOffset=0;
            //    switch(cap)
            //    {
            //        case eConnectorCap.Arrow:
            //            capWidthOffset=size.Width+1;
            //            break;
            //        case eConnectorCap.Ellipse:
            //            capWidthOffset=size.Width;
            //            break;
            //    }
            //    return capWidthOffset;
            //}

            ///// <summary>
            ///// Returns true if source node is on the left side of the target node.
            ///// </summary>
            ///// <param name="source">Reference to source node.</param>
            ///// <param name="target">Reference to target node.</param>
            ///// <returns>True if source is on the left side of target.</returns>
            //protected bool IsOnLeftSide(Node source, Node target)
            //{
            //    if((source.BoundsRelative.Left+source.BoundsRelative.Width/2)>target.BoundsRelative.Left)
            //        return true;
            //    return false;
            //}

			/// <summary>
			/// Returns new instance of pen object for node connector line. Caller is responsible for
			/// disposing of this object.
			/// </summary>
			/// <param name="info">Node connector display info.</param>
			/// <returns>New instance of Pen object.</returns>
			protected Pen GetLinePen(ConnectorRendererEventArgs info)
			{
                Pen pen = new Pen(info.NodeConnector.LineColor, info.NodeConnector.LineWidth);
                pen.DashStyle = info.NodeConnector.DashStyle;
                return pen;
			}

            ///// <summary>
            ///// Returns new instance of pen object for the end node connector line. Caller is responsible for
            ///// disposing of this object.
            ///// </summary>
            ///// <param name="info">Node connector display info.</param>
            ///// <returns>New instance of Pen object.</returns>
            //protected Pen GetEndLinePen(ConnectorRendererEventArgs info)
            //{
            //    return new Pen(info.NodeConnector.LineColor,EndLineWidth);
            //}

            ///// <summary>
            ///// Returns new instance of pen object for the node underline line. Caller is responsible for
            ///// disposing of this object.
            ///// </summary>
            ///// <param name="info">Node connector display info.</param>
            ///// <returns>New instance of Pen object.</returns>
            //protected Pen GetEndUnderlinePen(ConnectorRendererEventArgs info)
            //{
            //    return new Pen(info.NodeConnector.LineColor,EndLineWidth);
            //}

            //private int EndLineWidth
            //{
            //    get {return 1;}
            //}

//            /// <summary>
//            /// Draws straight line connector between start and end point.
//            /// </summary>
//            /// <param name="info">Node connector display info.</param>
//            /// <param name="pStart">Start point.</param>
//            /// <param name="pEnd">End point.</param>
//            /// <param name="pEndUnderLine">Underline end point if any.</param>
//            protected void DrawStraightLineConnector(ConnectorRendererEventArgs info, Point pStart, Point pEnd)
//            {
//                using (Pen pen = this.GetLinePen(info))
//                {
//                    if (pen.DashStyle != DashStyle.Solid)
//                    {
//                        SmoothingMode sm = info.Graphics.SmoothingMode;
//                        info.Graphics.SmoothingMode = SmoothingMode.Default;
//                        info.Graphics.DrawLine(pen, pStart, pEnd);
//                        info.Graphics.SmoothingMode = sm;
//                    }
//                    else
//                        info.Graphics.DrawLine(pen, pStart, pEnd);
//                }
//            }

//            /// <summary>
//            /// Draws straight line connector between start and end point.
//            /// </summary>
//            /// <param name="info">Node connector display info.</param>
//            /// <param name="pStart">Start point.</param>
//            /// <param name="pEnd">End point.</param>
//            /// <param name="pEndUnderLine">Underline end point if any.</param>
//            protected void DrawLineConnector(ConnectorRendererEventArgs info,Point pStart,Point pEnd, Point pEndUnderLine)
//            {
//                if(info.NodeConnector.LineWidth>1)
//                {
//                    // Merge lines nicely by filling and creating path...
//                    int rootLineWidth=this.EndLineWidth;
//                    int lineWidth=info.NodeConnector.LineWidth;
				
//                    using(Brush brush=GetLineBrush(info))
//                    {
//                        GraphicsPath path=GetConnectingPath(pStart,pEnd,lineWidth,rootLineWidth,info.IsRootNode,!(IsAbove(info.FromNode,info.ToNode) || IsBelow(info.FromNode,info.ToNode)));
//                        info.Graphics.FillPath(brush,path);
//                    }
//                }
//                else
//                {
//                    using(Pen pen=this.GetLinePen(info))
//                    {
//                        info.Graphics.DrawLine(pen,pStart,pEnd);
//                    }
//                }

//                if(!pEndUnderLine.IsEmpty)
//                {
//                    using(Pen pen=this.GetEndUnderlinePen(info))
//                    {
//                        info.Graphics.DrawLine(pen,pEnd,pEndUnderLine);
//                    }
//                }
//            }

//            private GraphicsPath GetConnectingPath(Point pStart, Point pEnd, int lineStartWidth, int lineEndWidth, bool bRoot, bool bRootSide)
//            {
//                int direction=1;
//                if(pStart.X>pEnd.X)
//                    direction=-1;
//                lineStartWidth++;
//                lineEndWidth++;
//                GraphicsPath path=new GraphicsPath();
//                if(bRoot && !bRootSide)
//                {
//                    path.AddLine(pStart.X,pStart.Y,pStart.X+lineStartWidth*direction,pStart.Y);
////					if(direction>0)
////						path.AddLine(pEnd.X+lineEndWidth*direction,pEnd.Y,pEnd.X,pEnd.Y);
////					else
////						path.AddLine(pEnd.X,pEnd.Y,pEnd.X+lineEndWidth*direction,pEnd.Y);
//                    if(direction>0)
//                    {
//                        path.AddLine(pStart.X+lineStartWidth*direction,pStart.Y, pEnd.X, pEnd.Y);
//                        path.AddLine(pEnd.X, pEnd.Y, pEnd.X, pEnd.Y + lineEndWidth*direction);
//                        path.AddLine(pEnd.X, pEnd.Y + lineEndWidth*direction, pStart.X, pStart.Y);
//                    }
//                    else
//                        path.AddLine(pEnd.X, pEnd.Y, pEnd.X, pEnd.Y + lineEndWidth*direction);
						
//                    path.CloseAllFigures();
////					if(Math.Abs(pEnd.Y-pStart.Y)<=8)
////						path.Widen(SystemPens.Highlight);
//                }
//                else
//                {
//                    int offsetStart=lineStartWidth/2;
//                    int offsetEnd=lineEndWidth/2;
//                    path.AddLine(pStart.X,pStart.Y-offsetStart,pStart.X,pStart.Y+offsetStart);
//                    path.AddLine(pEnd.X,pEnd.Y+offsetEnd,pEnd.X,pEnd.Y-offsetEnd);
//                    path.AddLine(pEnd.X,pEnd.Y-offsetEnd,pStart.X,pStart.Y-offsetStart);
//                    path.CloseAllFigures();
//                }
			
//                return path;
//            }

//            protected Brush GetLineBrush(ConnectorRendererEventArgs info)
//            {
//                return new SolidBrush(info.NodeConnector.LineColor);
//            }

//            protected void DrawEndLine(ConnectorRendererEventArgs info,Point pStart,Point pEnd,Point pEndUnderLine)
//            {
//                if(pEndUnderLine.IsEmpty)
//                {
//                    switch(info.NodeConnector.EndCap)
//                    {
//                        case eConnectorCap.Ellipse:
//                            {
//                                using(Pen pen=this.GetEndLinePen(info))
//                                {
//                                    Size endCapSize=info.NodeConnector.EndCapSize;
//                                    if(pStart.X<pEnd.X)
//                                        info.Graphics.DrawEllipse(pen,pEnd.X-1,pEnd.Y-endCapSize.Height/2,endCapSize.Width,endCapSize.Height);
//                                    else
//                                        info.Graphics.DrawEllipse(pen,pEnd.X-endCapSize.Width,pEnd.Y-endCapSize.Height/2,endCapSize.Width,endCapSize.Height);
//                                }
//                                break;
//                            }
//                        case eConnectorCap.Arrow:
//                            {
//                                using(Pen pen=this.GetEndLinePen(info))
//                                {
//                                    // Connects connector line to arrow
//                                    int direction=1;
//                                    if(pStart.X>pEnd.X)
//                                        direction=-1;
//                                    info.Graphics.DrawLine(pen,pEnd,new Point(pEnd.X+info.NodeConnector.EndCapSize.Width/3*direction,pEnd.Y));

//                                    Size endCapSize=info.NodeConnector.EndCapSize;
//                                    GraphicsPath arrow=GetArrowPath(endCapSize,pStart,pEnd);
//                                    info.Graphics.DrawPath(pen,arrow);
//                                }
//                                break;
//                            }
//                    }
//                }
//                else
//                {
//                    using(Pen pen=this.GetEndUnderlinePen(info))
//                    {
//                        info.Graphics.DrawLine(pen,pEnd,pEndUnderLine);
					
//                        // Connect underline to expand part
//                        if(NodeDisplay.DrawExpandPart(info.ToNode))
//                        {
//                            Rectangle re=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds,info.ToNode,info.Offset);
//                            Point p2=new Point((re.X>pEndUnderLine.X?re.X:re.Right)+(re.Width/2*(re.X>pEndUnderLine.X?1:-1)),re.Bottom);
//                            Point p1=new Point(p2.X,pEndUnderLine.Y+(pEndUnderLine.Y>p2.Y?(p2.Y-pEndUnderLine.Y)/2:-(p2.Y-pEndUnderLine.Y)/2));
//                            info.Graphics.DrawCurve(pen,new Point[]{pEndUnderLine,p1,p2},.5f);
//                        }
//                    }
//                }
//            }

//            private GraphicsPath GetArrowPath(Size capSize,Point pStart,Point pEnd)
//            {
//                GraphicsPath path=new GraphicsPath();
//                int direction=1;
//                if(pStart.X>pEnd.X)
//                    direction=-1;
			
//                pEnd.X+=(GetCapWidthOffset(eConnectorCap.Arrow,capSize)*direction);
//                path.AddLine(pEnd.X,pEnd.Y,pEnd.X-capSize.Width*direction,pEnd.Y-capSize.Height/2);
//                path.AddLine(pEnd.X-(2*capSize.Width/3*direction),pEnd.Y,pEnd.X-capSize.Width*direction,pEnd.Y+capSize.Height/2);

//                path.CloseAllFigures();
//                return path;
//            }

            //internal virtual ConnectorPointInfo GetConnectorPointInfo(ConnectorRendererEventArgs info, Point pStart, Point pEnd)
            //{
            //    ConnectorPointInfo pointInfo=new ConnectorPointInfo();

            //    int xMulti=1/*, yMulti=1*/;
            //    int lineWidth=info.NodeConnector.LineWidth;

            //    // used for direction control
            //    if(pStart.X>pEnd.X)
            //        xMulti=-1;
            //    //if(pStart.Y>pEnd.Y)
            //    //	yMulti=-1;

            //    if(info.ConnectorPoints!=null)
            //    {
            //        Point connPointsOffset=info.ToNode.BoundsRelative.Location;
            //        connPointsOffset.Offset(info.Offset.X,info.Offset.Y);
            //        GraphicsPath path=new GraphicsPath();
				
            //        pointInfo.Points1=new Point[info.ConnectorPoints.Count+2];
            //        pointInfo.Points1[0]=pStart;
            //        pointInfo.Points1[pointInfo.Points1.Length-1]=pEnd;

            //        if(lineWidth>1)
            //        {
            //            pointInfo.Points2=new Point[info.ConnectorPoints.Count+2];
            //            pointInfo.Points2[pointInfo.Points2.Length-1]=pStart;
            //            pointInfo.Points2[0]=pEnd;

            //            int i=pointInfo.Points1.Length-2;
            //            int k=1;

            //            foreach(Point pcp in info.ConnectorPoints)
            //            {
            //                pointInfo.Points1[i]=pcp;
            //                pointInfo.Points1[i].Offset(connPointsOffset.X,connPointsOffset.Y);
            //                pointInfo.Points2[k]=new Point(pcp.X+lineWidth*xMulti,pcp.Y);
            //                pointInfo.Points2[k].Offset(connPointsOffset.X,connPointsOffset.Y);
            //                k++;
            //                i--;
            //            }
            //        }
            //        else
            //        {
            //            int i=pointInfo.Points1.Length-2;
            //            foreach(Point pcp in info.ConnectorPoints)
            //            {
            //                pointInfo.Points1[i]=pcp;
            //                pointInfo.Points1[i].Offset(connPointsOffset.X,connPointsOffset.Y);
            //                i--;
            //            }
            //        }	
            //    }
            //    return pointInfo;
            //}
		}
	}

	/// <summary>
	/// Represents custom connector path info.
	/// </summary>
	internal class ConnectorPointInfo
	{
		public Point[] Points1=null;
		public Point[] Points2=null;
	}
}
