using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Summary description for NodeTreeDisplay.
	/// </summary>
	public class NodeTreeDisplay:NodeDisplay
	{
		#region Private Variables
		// Cashed objects
		ElementStyleDisplayInfo m_ElementStyleDisplayInfo=new ElementStyleDisplayInfo();
		NodeCellRendererEventArgs m_CellDisplayInfo=new NodeCellRendererEventArgs();
//		CurveConnectorDisplay m_CurveConnectorDisplay=null;
//		LineConnectorDisplay m_LineConnectorDisplay=null;
		private NodeSystemRenderer m_SystemRenderer=new NodeSystemRenderer();
		private NodeProfessionalRenderer m_ProfRenderer=new NodeProfessionalRenderer();
		#endregion
		/// <summary>Creates new instance of the class</summary>
		/// <param name="tree">Object to initialize class with.</param>
		public NodeTreeDisplay(TreeGX tree):base(tree)
		{
		}

		/// <summary>
		/// Paints the treee on canvas.
		/// </summary>
		public override void Paint(Graphics g, Rectangle clipRectangle)
		{
			base.Paint(g,clipRectangle);

			// Paint header

			// Paint Nodes...
			NodeDisplayContext context=new NodeDisplayContext();
			context.Graphics=g;
			context.DefaultFont=this.Tree.Font;
			context.ClipRectangle=clipRectangle;
			context.Offset=this.Offset;
			context.Styles=this.Tree.Styles;
			
			// Setup rendering
			context.NodeRenderer = m_SystemRenderer;
			if(this.Tree.NodeRenderer!=null && this.Tree.RenderMode==eNodeRenderMode.Custom)
				context.NodeRenderer=this.Tree.NodeRenderer;
			else if(this.Tree.RenderMode==eNodeRenderMode.Professional)
				context.NodeRenderer = m_ProfRenderer;
				
			context.NodeRendererEventArgs=new NodeRendererEventArgs();
				
			if(this.Tree.NodeStyle!=null)
				context.DefaultNodeStyle=this.Tree.NodeStyle;
			else
				context.DefaultNodeStyle=GetDefaultNodeStyle();
			if(this.Tree.NodeStyleExpanded!=null)
				context.ExpandedNodeStyle=this.Tree.NodeStyleExpanded;
			if(this.Tree.NodeStyleSelected!=null)
				context.SelectedNodeStyle=this.Tree.NodeStyleSelected;
			if(this.Tree.NodeStyleMouseOver!=null)
				context.MouseOverNodeStyle=this.Tree.NodeStyleMouseOver;

			if(this.Tree.CellStyleDefault!=null)
				context.CellStyleDefault=this.Tree.CellStyleDefault;
//			else
//				context.CellStyleDefault=ElementStyle.GetDefaultCellStyle(context.DefaultNodeStyle);
			if(this.Tree.CellStyleDisabled!=null)
				context.CellStyleDisabled=this.Tree.CellStyleDisabled;
			else
				context.CellStyleDisabled=ElementStyle.GetDefaultDisabledCellStyle();
			if(this.Tree.CellStyleMouseDown!=null)
				context.CellStyleMouseDown=this.Tree.CellStyleMouseDown;
			if(this.Tree.CellStyleMouseOver!=null)
				context.CellStyleMouseOver=this.Tree.CellStyleMouseOver;
			if(this.Tree.CellStyleSelected!=null)
				context.CellStyleSelected=this.Tree.CellStyleSelected;
			else
				context.CellStyleSelected=ElementStyle.GetDefaultSelectedCellStyle();

			// Setup connector
			//context.RootConnectorDisplay=GetConnectorDisplay(this.Tree.RootConnector);
			//context.NodesConnectorDisplay=GetConnectorDisplay(this.Tree.NodesConnector);
			//context.LinkConnectorDisplay=GetConnectorDisplay(this.Tree.LinkConnector);
			context.ConnectorDisplayInfo=new ConnectorRendererEventArgs();
			context.ConnectorDisplayInfo.Graphics=context.Graphics;
			if(this.Tree.SelectedPathConnector!=null)
			{
				// Prepare nodes for path display. Downstream nodes are easily recognized when
				// path upstream nodes need special treatment to improve performance
				if(this.Tree.SelectedNode!=null)
				{
					Node n=this.Tree.SelectedNode;
					while(n!=null)
					{
						// This marker will be reset once node is painted...
						n.SelectedConnectorMarker = true;
						n=n.Parent;
					}
				}
				//context.SelectedPathConnectorDisplay=GetConnectorDisplay(this.Tree.SelectedPathConnector);
			}

			// Setup Node Expander Painter
			context.ExpandDisplayInfo=new NodeExpandPartRendererEventArgs(context.Graphics);
			context.ExpandDisplayInfo.BackColor=this.Tree.GetColor(this.Tree.ExpandBackColor, this.Tree.ExpandBackColorSchemePart);
			context.ExpandDisplayInfo.BackColor2=this.Tree.GetColor(this.Tree.ExpandBackColor2, this.Tree.ExpandBackColor2SchemePart);
			context.ExpandDisplayInfo.BackColorGradientAngle=this.Tree.ExpandBackColorGradientAngle;
			context.ExpandDisplayInfo.BorderColor=this.Tree.GetColor(this.Tree.ExpandBorderColor, this.Tree.ExpandBorderColorSchemePart);
			context.ExpandDisplayInfo.ExpandLineColor=this.Tree.GetColor(this.Tree.ExpandLineColor, this.Tree.ExpandLineColorSchemePart);
			context.ExpandDisplayInfo.Graphics=context.Graphics;
            context.ExpandDisplayInfo.ExpandButtonType = this.Tree.ExpandButtonType;
			context.ExpandDisplayInfo.ExpandImage=this.Tree.ExpandImage;
			context.ExpandDisplayInfo.ExpandImageCollapse=this.Tree.ExpandImageCollapse;
			//context.ExpandDisplay=this.GetExpandDisplay(this.Tree.ExpandButtonType);


			// Setup Selection Display
			//context.SelectionDisplay=new NodeSelectionDisplay();
			context.SelectionDisplayInfo=new SelectionRendererEventArgs();
			context.SelectionDisplayInfo.Graphics=context.Graphics;
			context.SelectionDisplayInfo.FillColor=this.Tree.SelectionBoxFillColor;
			context.SelectionDisplayInfo.BorderColor=this.Tree.SelectionBoxBorderColor;
			context.SelectionDisplayInfo.Width=this.Tree.SelectionBoxSize;

			// Setup Node Command Display
			//context.CommandDisplay=new NodeCommandDisplay();
			context.CommandDisplayInfo=new NodeCommandPartRendererEventArgs(context.Graphics);
			context.CommandDisplayInfo.BackColor=this.Tree.GetColor(this.Tree.CommandBackColor, this.Tree.CommandBackColorSchemePart);
			context.CommandDisplayInfo.BackColor2=this.Tree.GetColor(this.Tree.CommandBackColor2, this.Tree.CommandBackColor2SchemePart);
			context.CommandDisplayInfo.BackColorGradientAngle=this.Tree.CommandBackColorGradientAngle;
			context.CommandDisplayInfo.ForeColor=this.Tree.GetColor(this.Tree.CommandForeColor, this.Tree.CommandForeColorSchemePart);
			context.CommandDisplayInfo.MouseOverBackColor=this.Tree.GetColor(this.Tree.CommandMouseOverBackColor, this.Tree.CommandMouseOverBackColorSchemePart);
			context.CommandDisplayInfo.MouseOverBackColor2=this.Tree.GetColor(this.Tree.CommandMouseOverBackColor2, this.Tree.CommandMouseOverBackColor2SchemePart);
			context.CommandDisplayInfo.MouseOverBackColorGradientAngle=this.Tree.CommandMouseOverBackColorGradientAngle;
			context.CommandDisplayInfo.MouseOverForeColor=this.Tree.GetColor(this.Tree.CommandMouseOverForeColor, this.Tree.CommandMouseOverForeColorSchemePart);
				
			if(this.Tree.DisplayRootNode!=null)
				this.PaintNode(this.Tree.DisplayRootNode, context);
			else
				this.PaintNodes(this.Tree.Nodes, context);

			Node dragNode=this.Tree.GetDragNode();
			if(dragNode!=null && dragNode.Parent==null)
			{
				Point p=context.Offset;
				context.Offset=Point.Empty;
				PaintNode(dragNode,context);
				context.Offset=p;
			}

			PaintSelection(context);
		}
		
		private void PaintSelection(NodeDisplayContext context)
		{
			if(this.Tree.SelectedNode!=null && this.Tree.SelectedNode.Visible && this.Tree.SelectionBox)
			{
				context.SelectionDisplayInfo.Node=this.Tree.SelectedNode;
				context.SelectionDisplayInfo.Bounds=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, context.SelectionDisplayInfo.Node, context.Offset);
				//context.SelectionDisplay.PaintSelection(context.SelectionDisplayInfo);
				context.NodeRenderer.DrawSelection(context.SelectionDisplayInfo);
			}
		}

		private void PaintNodes(NodeCollection nodes,NodeDisplayContext context)
		{
			foreach(Node node in nodes)
			{
				PaintNode(node, context);
			}
		}

		private void PaintNode(Node node, NodeDisplayContext context)
		{
			if(!node.Visible)
			{
				node.SelectedConnectorMarker = false;
				return;
			}

			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,context.Offset);
			if(r.IsEmpty)
			{
				node.SelectedConnectorMarker = false;
				return;
			}
			
			if(node.Parent!=null && !node.Parent.BoundsRelative.IsEmpty)
			{
				this.PaintConnector(node, context);
			}

			if(node.HasLinkedNodes)
			{
				foreach(LinkedNode linkedNode in node.LinkedNodes)
				{
					if(!linkedNode.Node.IsVisible)
						continue;
					this.PaintConnector(node,linkedNode.Node, context,true, linkedNode.ConnectorPoints);	
				}
			}
			
			node.SelectedConnectorMarker = false;

			if(r.IntersectsWith(context.ClipRectangle) || context.ClipRectangle.IsEmpty)
				PaintSingleNode(node, context);
//			else if(node.HasHostedControls) // Move controls to proper location
//				MoveHostedControls(node, context);

			if(node.Expanded && node.Nodes.Count>0)
				PaintNodes(node.Nodes,context);
		}

		private void PaintSingleNode(Node node, NodeDisplayContext context)
		{
			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,context.Offset);
			NodeRenderer renderer=context.NodeRenderer;
			if(node.NodeRenderer!=null && node.RenderMode==eNodeRenderMode.Custom)
				renderer = node.NodeRenderer;
			
			// Paint node background
			ElementStyle style=context.DefaultNodeStyle.Copy();
			if(node.Style!=null)
				style=node.Style.Copy();

			bool bApllyStyles=true;

			if(node.MouseOverNodePart==eMouseOverNodePart.Node && style!=null)
			{
				ElementStyle mouseOverStyle=context.MouseOverNodeStyle;
				if(node.StyleMouseOver!=null)
					mouseOverStyle=node.StyleMouseOver;
                if (mouseOverStyle != null)
                {
                    style.ApplyStyle(mouseOverStyle);
                    bApllyStyles = false;
                }
			}

			// On default style apply expanded style
			if(bApllyStyles && node.Expanded && style!=null)
			{
				ElementStyle expandedStyle=context.ExpandedNodeStyle;
				if(node.StyleExpanded!=null)
					expandedStyle=node.StyleExpanded;
				if(expandedStyle!=null)
					style.ApplyStyle(expandedStyle);
			}

			// Apply selected style if needed too
			if(bApllyStyles && node.IsSelected && style!=null)
			{
				ElementStyle selectedStyle=context.SelectedNodeStyle;
				if(node.StyleSelected!=null)
					selectedStyle=node.StyleSelected;
				if(selectedStyle!=null)
					style.ApplyStyle(selectedStyle);
			}

			Region backRegion=null;
			if(style!=null)
			{
				if(style.Font==null)
					style.Font=context.DefaultFont;
				context.NodeRendererEventArgs.Graphics=context.Graphics;
				context.NodeRendererEventArgs.Node = node;
				context.NodeRendererEventArgs.NodeBounds = r;
				context.NodeRendererEventArgs.Style = style;
				renderer.DrawNodeBackground(context.NodeRendererEventArgs);
				ElementStyleDisplayInfo di=new ElementStyleDisplayInfo(style,context.Graphics,DisplayHelp.GetDrawRectangle(r));
				//ElementStyleDisplay.Paint(di);
				backRegion=ElementStyleDisplay.GetStyleRegion(di);
			}
						
			if(NodeDisplay.DrawExpandPart(node))
			{
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds,node,context.Offset);
				context.ExpandDisplayInfo.Node=node;
				context.ExpandDisplayInfo.ExpandPartBounds=r;
				//context.ExpandDisplayInfo.MouseOver=(node.MouseOverNodePart==eMouseOverNodePart.Expand);
				//context.ExpandDisplay.DrawExpandButton(context.ExpandDisplayInfo);
				
				renderer.DrawNodeExpandPart(context.ExpandDisplayInfo);
			}

			// TODO: Support for display of columns should go here
//			if(node.NodesColumnHeaderVisible)
//			{
//				foreach(ColumnHeader col in node.NodesColumns)
//				{
//					r=col.Bounds;
//					r.Offset(context.Offset);
//					r.Offset(node.Bounds.Location);
//					using(Pen pen=new Pen(Color.Yellow))
//						context.Graphics.DrawRectangle(pen,Display.GetDrawRectangle(r));
//				}
//			}

			Region oldRegion=null;
			if(backRegion!=null)
			{
				oldRegion=context.Graphics.Clip;
				context.Graphics.SetClip(backRegion,CombineMode.Replace);
			}

			if(!node.CommandBoundsRelative.IsEmpty)
			{
				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CommandBounds,node,context.Offset);
                context.CommandDisplayInfo.Node=node;
				context.CommandDisplayInfo.CommandPartBounds=r;
				context.CommandDisplayInfo.Graphics = context.Graphics;
				//context.CommandDisplay.DrawCommandButton(context.CommandDisplayInfo);
				renderer.DrawNodeCommandPart(context.CommandDisplayInfo);
				context.CommandDisplayInfo.Graphics = null;
			}
			
			ElementStyle cellStyle = null;
			if(context.CellStyleDefault==null)
			{
				cellStyle=new ElementStyle();
				cellStyle.TextColor = style.TextColor;
				cellStyle.TextShadowColor = style.TextShadowColor;
				cellStyle.TextShadowOffset = style.TextShadowOffset;
				cellStyle.TextAlignment = style.TextAlignment;
				cellStyle.TextLineAlignment = style.TextLineAlignment;
				cellStyle.WordWrap = style.WordWrap;
				cellStyle.Font = style.Font;
			}

			foreach(Cell cell in node.Cells)
			{
				if(cell.StyleNormal!=null)
					style=cell.StyleNormal;
				else if(context.CellStyleDefault!=null)
					style=context.CellStyleDefault.Copy();
				else
					style = cellStyle;

				if(!cell.Enabled && cell.StyleDisabled!=null)
					style.ApplyStyle(cell.StyleDisabled);
				else if(!cell.Enabled && context.CellStyleDisabled!=null)
					style.ApplyStyle(context.CellStyleDisabled);
				else if(cell.IsMouseDown && cell.StyleMouseDown!=null)
					style.ApplyStyle(cell.StyleMouseDown);
				else if(cell.IsMouseDown && context.CellStyleMouseDown!=null)
					style.ApplyStyle(context.CellStyleMouseDown);
				else 
				{
					if(cell.IsSelected && cell.StyleSelected!=null)
						style.ApplyStyle(cell.StyleSelected);
					else if(cell.IsSelected && context.CellStyleSelected!=null)
						style.ApplyStyle(context.CellStyleSelected);

					if(cell.IsMouseOver && cell.StyleMouseOver!=null)
						style.ApplyStyle(cell.StyleMouseOver);
					else if(cell.IsMouseOver && context.CellStyleMouseOver!=null)
						style.ApplyStyle(context.CellStyleMouseOver);
				}

				r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeBounds,node,context.Offset);
				
				if(style!=null)
				{
					if(style.Font==null)
						style.Font=context.DefaultFont;
					Rectangle rCell=cell.BoundsRelative;
					Rectangle rText=cell.TextContentBounds;
					rCell.Offset(r.Location);
					rText.Offset(r.Location);
					ElementStyleDisplayInfo di=GetElementStyleDisplayInfo(style,context.Graphics,DisplayHelp.GetDrawRectangle(rCell));
					ElementStyleDisplay.Paint(di);
					NodeCellRendererEventArgs ci=GetCellDisplayInfo(style,context.Graphics,cell,r.Location, rCell, rText);
					
					if(ci.Cell.CheckBoxVisible)
						renderer.DrawCellCheckBox(ci); //CellDisplay.PaintCellCheckBox(ci);
					if(!ci.Cell.Images.LargestImageSize.IsEmpty)
						renderer.DrawCellImage(ci); //CellDisplay.PaintCellImage(ci);
					renderer.DrawCellText(ci) ; //CellDisplay.PaintText(ci);
					
					//CellDisplay.PaintCell(ci);
				}
			}

			if(backRegion!=null)
				context.Graphics.SetClip(oldRegion,CombineMode.Replace);
		}

		private void PaintConnector(Node fromNode, Node toNode, NodeDisplayContext context,bool linkConnector)
		{
			PaintConnector(fromNode, toNode, context, linkConnector, null);
		}
		
		private void PaintConnector(Node fromNode, Node toNode, NodeDisplayContext context,bool linkConnector, ConnectorPointsCollection connectorPoints)
		{
			bool isRootNode=IsRootNode(fromNode);
//			if(context.RootConnectorDisplay==null && isRootNode || context.NodesConnectorDisplay==null && !isRootNode || linkConnector && context.LinkConnectorDisplay==null)
//				return;

			context.ConnectorDisplayInfo.FromNode=fromNode;
			context.ConnectorDisplayInfo.ToNode=toNode;
			context.ConnectorDisplayInfo.IsRootNode=isRootNode;
			context.ConnectorDisplayInfo.LinkConnector=linkConnector;
			if(fromNode.Style!=null)
				context.ConnectorDisplayInfo.StyleFromNode=fromNode.Style;
			else
				context.ConnectorDisplayInfo.StyleFromNode=context.DefaultNodeStyle;
			if(toNode.Style!=null)
				context.ConnectorDisplayInfo.StyleToNode=toNode.Style;
			else
				context.ConnectorDisplayInfo.StyleToNode=context.DefaultNodeStyle;
			context.ConnectorDisplayInfo.Offset=context.Offset;
			
			//NodeConnectorDisplay display=null;
			if(linkConnector)
			{
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.LinkConnector;
			}
			else if(toNode.SelectedConnectorMarker)
			{
				//display = context.SelectedPathConnectorDisplay;
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.SelectedPathConnector;
			}
			else if(toNode.ParentConnector!=null)
			{
				//display=this.GetConnectorDisplay(toNode.ParentConnector);
				context.ConnectorDisplayInfo.NodeConnector=toNode.ParentConnector;
			}
			else if(isRootNode)
			{
				//display=context.RootConnectorDisplay;
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.RootConnector;
			}
			else
			{
				//display=context.NodesConnectorDisplay;
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.NodesConnector;
			}

			if(linkConnector)
			{
                if (connectorPoints != null && connectorPoints.Count > 0)
				    context.ConnectorDisplayInfo.ConnectorPoints=connectorPoints;
			}
			else
			{
				if(connectorPoints!=null)
					context.ConnectorDisplayInfo.ConnectorPoints=connectorPoints;
				else if(toNode.ParentConnectorPoints.Count>0)
					context.ConnectorDisplayInfo.ConnectorPoints=toNode.ParentConnectorPoints;
				else
					context.ConnectorDisplayInfo.ConnectorPoints=null;
			}

			context.NodeRenderer.DrawConnector(context.ConnectorDisplayInfo);
			//if(display!=null)
			//	display.DrawConnector(context.ConnectorDisplayInfo);
		}

		private void PaintConnector(Node toNode, NodeDisplayContext context)
		{
			PaintConnector(toNode.Parent, toNode, context,false);
		}
		
//		private void MoveHostedControls(Node node, NodeDisplayContext context)
//		{
//			foreach(Cell cell in node.Cells)
//			{
//				if(cell.HostedControl!=null && cell.HostedControl.Visible)
//				{
//					Rectangle bounds = NodeDisplay.GetCellRectangle(eCellRectanglePart.TextBounds, cell, context.Offset);
//					if(cell.HostedControl.Bounds!=bounds)
//						cell.HostedControl.Bounds=bounds;
//					return;
//				}
//			}
//		}

		private ElementStyleDisplayInfo GetElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds)
		{
			m_ElementStyleDisplayInfo.Style=style;
			m_ElementStyleDisplayInfo.Graphics=g;
			m_ElementStyleDisplayInfo.Bounds=bounds;
			return m_ElementStyleDisplayInfo;
		}

		private NodeCellRendererEventArgs GetCellDisplayInfo(ElementStyle style, Graphics g, Cell cell, Point cellOffset, Rectangle cellBounds, Rectangle cellTextBounds)
		{
			m_CellDisplayInfo.Cell=cell;
			m_CellDisplayInfo.Graphics=g;
			m_CellDisplayInfo.Style=style;
			m_CellDisplayInfo.CellOffset=cellOffset;
            m_CellDisplayInfo.CellBounds = cellBounds;
            m_CellDisplayInfo.CellTextBounds = cellTextBounds;
			return m_CellDisplayInfo;
		}

//		private NodeConnectorDisplay GetConnectorDisplay(NodeConnector c)
//		{
//			NodeConnectorDisplay d=null;
//			if(c==null)
//				return null;
//
//			switch(c.ConnectorType)
//			{
//				case eNodeConnectorType.Curve:
//				{
//					if(m_CurveConnectorDisplay==null)
//						m_CurveConnectorDisplay=new CurveConnectorDisplay();
//					d=m_CurveConnectorDisplay;
//					break;
//				}
//				case eNodeConnectorType.Line:
//				{
//					if(m_LineConnectorDisplay==null)
//						m_LineConnectorDisplay=new LineConnectorDisplay();
//					d=m_LineConnectorDisplay;
//					break;
//				}
//			}
//			return d;
//		}

		private class NodeDisplayContext
		{
			public Graphics Graphics=null;
			public Font DefaultFont=null;
			public Rectangle ClipRectangle=Rectangle.Empty;
			public Point Offset=Point.Empty;
			public ElementStyle DefaultNodeStyle=null;
			public ElementStyle ExpandedNodeStyle=null;
			public ElementStyle SelectedNodeStyle=null;
			public ElementStyle MouseOverNodeStyle=null;
			public ElementStyle CellStyleDefault=null;
			public ElementStyle CellStyleMouseDown=null;
			public ElementStyle CellStyleMouseOver=null;
			public ElementStyle CellStyleSelected=null;
			public ElementStyle CellStyleDisabled=null;
			public ElementStyleCollection Styles=null;
			//public NodeConnectorDisplay RootConnectorDisplay=null;
			//public NodeConnectorDisplay NodesConnectorDisplay=null;
			//public NodeConnectorDisplay LinkConnectorDisplay=null;
			//public NodeConnectorDisplay SelectedPathConnectorDisplay=null;
			public ConnectorRendererEventArgs ConnectorDisplayInfo=null;
			public NodeExpandPartRendererEventArgs ExpandDisplayInfo=null;
			//public NodeExpandDisplay ExpandDisplay=null;
			//public NodeSelectionDisplay SelectionDisplay=null;
			public SelectionRendererEventArgs SelectionDisplayInfo=null;
			//public NodeCommandDisplay CommandDisplay=null;
			public NodeCommandPartRendererEventArgs CommandDisplayInfo=null;
			public bool SelectedNodePath=false;
			public NodeRenderer NodeRenderer=null;
			public NodeRendererEventArgs NodeRendererEventArgs=null;
		}

		
	}
}
