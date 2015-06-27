using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display
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
		//private NodeProfessionalRenderer m_ProfRenderer=new NodeProfessionalRenderer();
		#endregion
		/// <summary>Creates new instance of the class</summary>
		/// <param name="tree">Object to initialize class with.</param>
		public NodeTreeDisplay(AdvTree tree):base(tree)
		{
            m_SystemRenderer.ColorTable = new TreeColorTable();
            ColorTableInitializer.InitOffice2007Blue(m_SystemRenderer.ColorTable, new ColorFactory());
		}

        public NodeSystemRenderer SystemRenderer
        {
            get { return m_SystemRenderer; }
            set { m_SystemRenderer = value; }
        }

        public TreeRenderer GetTreeRenderer()
        {
            TreeRenderer renderer = m_SystemRenderer;
            if (this.Tree.NodeRenderer != null && this.Tree.RenderMode == eNodeRenderMode.Custom)
                renderer = this.Tree.NodeRenderer;
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                renderer.Office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                renderer.ColorTable = renderer.Office2007ColorTable.AdvTree;
            }

            return renderer;
        }

        private NodeDisplayContext CreateDisplayContext(Graphics g, Rectangle clipRectangle)
        {
            NodeDisplayContext context = new NodeDisplayContext();
            context.Graphics = g;
            context.DefaultFont = this.Tree.Font;
            context.ClipRectangle = clipRectangle;
            context.Offset = this.Offset;
            context.Styles = this.Tree.Styles;
            context.IsBackgroundSelection = IsBackgroundSelection;
            context.ClientRectangle = ElementStyleLayout.GetInnerRect(Tree.BackgroundStyle, Tree.ClientRectangle);
            context.ColorScheme = Tree.ColorScheme;
            context.GridRowLines = Tree.GridRowLines;
            context.GridRowLineColor = Tree.GridLinesColor;
            context.CellSelection = Tree.SelectionPerCell;
            context.AlternateRowBrush = CreateBrush(Tree.AlternateRowColor);
            context.DrawAlternateRowBackground = !Tree.AlternateRowColor.IsEmpty;
            context.View = this.Tree.View;
            context.TileGroupLineColor = this.Tree.TileGroupLineColor;
            // Setup rendering
            context.NodeRenderer = m_SystemRenderer;
            if (this.Tree.NodeRenderer != null && this.Tree.RenderMode == eNodeRenderMode.Custom)
                context.NodeRenderer = this.Tree.NodeRenderer;
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                context.NodeRenderer.Office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                context.NodeRenderer.ColorTable = context.NodeRenderer.Office2007ColorTable.AdvTree;
            }

            context.NodeRendererEventArgs = new NodeRendererEventArgs();

            if (this.Tree.NodeStyle != null)
                context.DefaultNodeStyle = this.Tree.NodeStyle;
            else
                context.DefaultNodeStyle = GetDefaultNodeStyle();

            if (this.Tree.NodeStyleExpanded != null)
                context.ExpandedNodeStyle = this.Tree.NodeStyleExpanded;
            if (this.Tree.NodeStyleSelected != null)
                context.SelectedNodeStyle = this.Tree.NodeStyleSelected;
            if (this.Tree.NodeStyleMouseOver != null)
                context.MouseOverNodeStyle = this.Tree.NodeStyleMouseOver;

            if (this.Tree.CellStyleDefault != null)
                context.CellStyleDefault = this.Tree.CellStyleDefault;
            if (this.Tree.CellStyleDisabled != null)
                context.CellStyleDisabled = this.Tree.CellStyleDisabled;
            else
                context.CellStyleDisabled = ElementStyle.GetDefaultDisabledCellStyle();
            if (this.Tree.CellStyleMouseDown != null)
                context.CellStyleMouseDown = this.Tree.CellStyleMouseDown;
            if (this.Tree.CellStyleMouseOver != null)
                context.CellStyleMouseOver = this.Tree.CellStyleMouseOver;
            if (this.Tree.CellStyleSelected != null)
                context.CellStyleSelected = this.Tree.CellStyleSelected;
            else
                context.CellStyleSelected = ElementStyle.GetDefaultSelectedCellStyle();

            // Setup connector
            context.ConnectorDisplayInfo = new ConnectorRendererEventArgs();
            context.ConnectorDisplayInfo.Graphics = context.Graphics;
            
            // Setup Node Expander Painter
            context.ExpandDisplayInfo = new NodeExpandPartRendererEventArgs(context.Graphics);
            context.ExpandDisplayInfo.BackColor = this.Tree.GetColor(this.Tree.ExpandBackColor, this.Tree.ExpandBackColorSchemePart);
            context.ExpandDisplayInfo.BackColor2 = this.Tree.GetColor(this.Tree.ExpandBackColor2, this.Tree.ExpandBackColor2SchemePart);
            
            context.ExpandDisplayInfo.BackColorGradientAngle = this.Tree.ExpandBackColorGradientAngle;
            context.ExpandDisplayInfo.BorderColor = this.Tree.GetColor(this.Tree.ExpandBorderColor, this.Tree.ExpandBorderColorSchemePart);
            context.ExpandDisplayInfo.ExpandLineColor = this.Tree.GetColor(this.Tree.ExpandLineColor, this.Tree.ExpandLineColorSchemePart);
            context.ExpandDisplayInfo.Graphics = context.Graphics;
            context.ExpandDisplayInfo.ExpandButtonType = this.Tree.ExpandButtonType;
            context.ExpandDisplayInfo.ExpandImage = this.Tree.ExpandImage;
            context.ExpandDisplayInfo.ExpandImageCollapse = this.Tree.ExpandImageCollapse;
            
            // Setup Selection Display
            context.SelectionDisplayInfo = new SelectionRendererEventArgs();
            context.SelectionDisplayInfo.Graphics = context.Graphics;
            context.SelectionDisplayInfo.SelectionBoxStyle = this.Tree.SelectionBoxStyle;
            context.SelectionDisplayInfo.TreeActive = this.Tree.IsKeyboardFocusWithin || !this.Tree.SelectionFocusAware;

            return context;
        }

        private Brush CreateBrush(Color color)
        {
            if (color.IsEmpty) return null;
            return new SolidBrush(color);
        }

		/// <summary>
		/// Paints the tree on canvas.
		/// </summary>
		public override void Paint(Graphics g, Rectangle clipRectangle)
		{
			base.Paint(g,clipRectangle);

            // Setup custom check box images if any
            m_CellDisplayInfo.CheckBoxImageChecked = this.Tree.CheckBoxImageChecked;
            m_CellDisplayInfo.CheckBoxImageUnChecked = this.Tree.CheckBoxImageUnChecked;
            m_CellDisplayInfo.CheckBoxImageIndeterminate = this.Tree.CheckBoxImageIndeterminate;
            m_CellDisplayInfo.ItemPaintArgs = new ItemPaintArgs(null, Tree, g, new ColorScheme());

			// Paint header
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                m_SystemRenderer.Office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                m_CellDisplayInfo.ItemPaintArgs.Colors = m_SystemRenderer.Office2007ColorTable.LegacyColors;
                m_CellDisplayInfo.ItemPaintArgs.Renderer = GlobalManager.Renderer;
            }
			// Paint Nodes...
            NodeDisplayContext context = CreateDisplayContext(g, clipRectangle);
            
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

            if (context.DrawAlternateRowBackground)
            {
                PaintAlternateRowBackground(context);
            }

            // Paint Grid Lines
            if (this.Tree.GridColumnLines && this.Tree.Columns.Count > 0)
            {
                PaintGridLines(context);
            }

            // Paint background for nodes with full row background
            if (this.Tree.FullRowBackgroundNodes != null && this.Tree.FullRowBackgroundNodes.Count > 0)
                PaintFullRowBackgroundForNodes(context, this.Tree.FullRowBackgroundNodes);

            // Paint selection
            if (context.IsBackgroundSelection)
            {
                PaintSelection(context);
            }
            if (Tree.HotTracking)
                PaintHotTracking(context);
            Rectangle innerRect = Tree.GetPaintRectangle();
		    Rectangle clientRect = Tree.ClientRectangle;
            if(innerRect == clipRectangle || clientRect == clipRectangle || clipRectangle.IsEmpty)
                _PaintedNodes.Clear();
            
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
            if (this.Tree.IsDragDropInProgress)
            {
                PaintDragInsertMarker(context, this.Tree.GetDragInsertionBounds(this.Tree.GetNodeDragInfo()));
            }

            if(!context.IsBackgroundSelection)
			    PaintSelection(context);

            if (context.AlternateRowBrush != null)
            {
                context.AlternateRowBrush.Dispose();
                context.AlternateRowBrush = null;
            }
		}

        private void PaintAlternateRowBackground(NodeDisplayContext context)
        {
            if (this.Tree.DisplayRootNode != null)
            {
                context.AlternateRowFlag = true;
                PaintAlternateRowBackground(context, this.Tree.DisplayRootNode.Nodes);
            }
            else
            {
                PaintAlternateRowBackground(context, this.Tree.Nodes);
            }
        }

        private void PaintAlternateRowBackground(NodeDisplayContext context, NodeCollection nodeCollection)
        {
            foreach (Node node in nodeCollection)
            {
                if (!node.Visible) continue;
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, context.Offset);
                if (r.Y > context.ClientRectangle.Bottom) break;

                if (r.Y >= context.ClientRectangle.Y)
                {
                    if (context.AlternateRowFlag)
                    {
                        Rectangle backRect = new Rectangle(context.ClientRectangle.X, r.Y - 1, context.ClientRectangle.Width, r.Height + 1);
                        context.Graphics.FillRectangle(context.AlternateRowBrush, backRect);
                    }
                }
                context.AlternateRowFlag = !context.AlternateRowFlag;

                if (node.HasChildNodes) PaintAlternateRowBackground(context, node.Nodes);
            }
        }

        public void ExternalPaintNode(Node node, Graphics g, Rectangle bounds)
        {
            // Setup custom check box images if any
            m_CellDisplayInfo.CheckBoxImageChecked = this.Tree.CheckBoxImageChecked;
            m_CellDisplayInfo.CheckBoxImageUnChecked = this.Tree.CheckBoxImageUnChecked;
            m_CellDisplayInfo.CheckBoxImageIndeterminate = this.Tree.CheckBoxImageIndeterminate;

            // Paint header
            if (GlobalManager.Renderer is Office2007Renderer)
                m_SystemRenderer.Office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
            // Paint Nodes...
            NodeDisplayContext context = CreateDisplayContext(g, bounds);
            context.ExpandDisplayInfo = null;
            context.ConnectorDisplayInfo = null;

            // Update offset
            context.Offset = new Point(bounds.X - node.BoundsRelative.X, bounds.Y - node.BoundsRelative.Y);

            PaintSingleNode(node, context);
        }

        private void PaintGridLines(NodeDisplayContext context)
        {
            PaintGridLines(context, Tree.Columns, context.ClientRectangle);   
        }

        private void PaintGridLines(NodeDisplayContext context, ColumnHeaderCollection columnHeaderCollection, Rectangle bounds)
        {
            Color color = this.Tree.GridLinesColor.IsEmpty ? context.NodeRenderer.ColorTable.GridLines : this.Tree.GridLinesColor;
            Graphics g = context.Graphics;
            for (int i = 0; i < columnHeaderCollection.Count; i++)
            {
                ColumnHeader columnHeader = columnHeaderCollection.ColumnAtDisplayIndex(i);
                if (!columnHeader.Visible || columnHeader.Bounds.Width <= 0) continue;
                Rectangle r = columnHeader.Bounds;
                r.Offset(context.Offset);
                if (columnHeaderCollection.ParentNode == null)
                {
                    r.Offset(bounds.Location);
                    if (!columnHeader.CellsBackColor.IsEmpty)
                    {
                        SmoothingMode oldSmoothing = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.None;
                        Rectangle rBack = new Rectangle(r.X - 5, context.ClientRectangle.Y, r.Width + 4, context.ClientRectangle.Height);
                        using (SolidBrush brush = new SolidBrush(columnHeader.CellsBackColor))
                            g.FillRectangle(brush, rBack);
                        g.SmoothingMode = oldSmoothing;
                    }
                }
                DisplayHelp.DrawLine(g, r.Right - (columnHeader.IsLastVisible ? 0 : 1), bounds.Y, r.Right - (columnHeader.IsLastVisible ? 0 : 1), bounds.Bottom, color, 1);
            }
        }

        private void PaintFullRowBackgroundForNodes(NodeDisplayContext context, ArrayList nodesCollection)
        {
            Rectangle clientRectangle = context.ClientRectangle;
            if (this.Tree.VScrollBar != null) clientRectangle.Width -= this.Tree.VScrollBar.Width;

            foreach (Node node in nodesCollection)
            {
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, context.Offset);

                if (!r.IsEmpty)
                {
                    r.X = clientRectangle.X;
                    r.Width = clientRectangle.Width;
                    r.Inflate(0, 1);
                }

                if (r.IsEmpty || !r.IntersectsWith(context.ClipRectangle) && !context.ClipRectangle.IsEmpty)
                    continue;
                ElementStyle style = GetEffectiveNodeBackgroundStyle(node, context);
                if (style == null)
                    continue;
                TreeRenderer renderer = context.NodeRenderer;
                if (node.NodeRenderer != null && node.RenderMode == eNodeRenderMode.Custom)
                    renderer = node.NodeRenderer;
                if (style != null)
                {
                    context.NodeRendererEventArgs.Graphics = context.Graphics;
                    context.NodeRendererEventArgs.Node = node;
                    context.NodeRendererEventArgs.NodeBounds = r;
                    context.NodeRendererEventArgs.Style = style;
                    renderer.DrawNodeBackground(context.NodeRendererEventArgs);
                }
            }
        }

        private void PaintDragInsertMarker(NodeDisplayContext context, Rectangle dragInsertionBounds)
        {
            if (dragInsertionBounds.IsEmpty) return;
            context.NodeRenderer.DrawDragDropMarker(new DragDropMarkerRendererEventArgs(context.Graphics, dragInsertionBounds));
        }
        
        public bool IsBackgroundSelection
        {
            get { return this.Tree.SelectionBoxStyle != eSelectionStyle.NodeMarker; }
        }
        
		private void PaintSelection(NodeDisplayContext context)
		{
            if (!this.Tree.MultiSelect && this.Tree.SelectedNode != null && this.Tree.SelectedNode.Visible && this.Tree.SelectionBox && !(this.Tree.HideSelection && !this.Tree.Focused))
			{
                PaintNodeSelection(this.Tree.SelectedNode, context);
			}
            else if (this.Tree.MultiSelect && this.Tree.SelectedNodes.Count>0 && this.Tree.SelectionBox && !(this.Tree.HideSelection && !this.Tree.Focused))
            {
                foreach (Node node in Tree.SelectedNodes)
                {
                    if(node.IsVisible)
                        PaintNodeSelection(node, context);    
                }
            }
		}

        private void PaintNodeSelection(Node node, NodeDisplayContext context)
        {
            if (!node.IsSelectionVisible) return;

            context.SelectionDisplayInfo.Node = node;
            Rectangle r;
            if (this.Tree.SelectionBoxStyle == eSelectionStyle.FullRowSelect)
            {
                if (context.CellSelection)
                {
                    Cell cell = context.SelectionDisplayInfo.Node.SelectedCell != null ? context.SelectionDisplayInfo.Node.SelectedCell : context.SelectionDisplayInfo.Node.Cells[0];
                    r = NodeDisplay.GetCellRectangle(eCellRectanglePart.CellBounds, 
                        cell, context.Offset);
                }
                else
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, context.SelectionDisplayInfo.Node, context.Offset);
                    r.X = context.ClientRectangle.X;
                    r.Width = context.ClientRectangle.Width;
                }
                if(context.GridRowLines)
                    r.Inflate(0, (int)Math.Floor((float)Tree.NodeSpacing / 2));
                else
                    r.Inflate(0, (int)Math.Ceiling((float)Tree.NodeSpacing / 2));
            }
            else if (this.Tree.SelectionBoxStyle == eSelectionStyle.HighlightCells)
            {
                if (context.CellSelection)
                {
                    Cell cell = context.SelectionDisplayInfo.Node.SelectedCell != null ? context.SelectionDisplayInfo.Node.SelectedCell : context.SelectionDisplayInfo.Node.Cells[0];
                    r = NodeDisplay.GetCellRectangle(eCellRectanglePart.CellBounds,
                        cell, context.Offset);
                }
                else
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds, context.SelectionDisplayInfo.Node, context.Offset);
                }
                r.Inflate(2, 2);
                if (r.Right > context.ClientRectangle.Right)
                    r.Width -= (r.Right - context.ClientRectangle.Right);
                    
            }
            else
            {
                if (context.CellSelection)
                {
                    Cell cell = context.SelectionDisplayInfo.Node.SelectedCell != null ? context.SelectionDisplayInfo.Node.SelectedCell : context.SelectionDisplayInfo.Node.Cells[0];
                    r = NodeDisplay.GetCellRectangle(eCellRectanglePart.CellBounds,
                        cell, context.Offset);
                }
                else
                {
                    r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, context.SelectionDisplayInfo.Node, context.Offset);
                }
            }

            context.SelectionDisplayInfo.Bounds = r;
            context.NodeRenderer.DrawSelection(context.SelectionDisplayInfo);
        }

        private void PaintHotTracking(NodeDisplayContext context)
        {
            Node node = this.Tree.MouseOverNode;
            if (node != null && !node.IsSelected && node.Selectable)
            {
                Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.CellsBounds, node, context.Offset);
                r.Inflate(2, 2);
                context.SelectionDisplayInfo.Node = node;
                context.SelectionDisplayInfo.Bounds = r;
                context.NodeRenderer.DrawHotTracking(context.SelectionDisplayInfo);
            }
        }

        private Node PaintNodes(NodeCollection nodes, NodeDisplayContext context)
		{
            Node lastPainted = null;
            int count = nodes.Count;
            int start = 0;

            // Find first node to render in case there are lot of them
            if (context.View == eView.Tree && count > 99 && /*this.Tree.NodesConnector == null &&*/ context.Offset.Y != 0)
            {
                int testIndex = count / 2;
                int chunk = testIndex;
                while (testIndex > 0 && testIndex < count)
                {
                    chunk = chunk / 2;
                    Node node = nodes[testIndex];
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, context.Offset);
                    if (chunk>0 && r.Y > context.ClipRectangle.Bottom) // go back
                        testIndex -= chunk;
                    else if (chunk > 0 && r.Y < context.ClipRectangle.Y) // go forward
                        testIndex += chunk;
                    else
                    {
                        if (testIndex == 0) break;
                        testIndex--;
                        for (int i = testIndex; i >= 0; i--)
                        {
                            node = nodes[i];
                            r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, context.Offset);
                            if (r.Y < context.ClipRectangle.Y)
                            {
                                start = i;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            for (int i = start; i < count; i++)
            {
                Node node = nodes[i];
                if (PaintNode(node, context))
                {
                    lastPainted = node;
                    Rectangle r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, node, context.Offset);
                    if (r.Y > context.ClipRectangle.Bottom && !context.ClientRectangle.IsEmpty)
                        break;
                }
			}
            return lastPainted;
		}

		private bool PaintNode(Node node, NodeDisplayContext context)
		{
			if(!node.Visible)
			{
				node.SelectedConnectorMarker = false;
				return false;
			}

			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,context.Offset);
			if(r.IsEmpty)
			{
				node.SelectedConnectorMarker = false;
                return false;
			}

            if (context.GridRowLines && !node.IsDragNode)
            {
                DisplayHelp.DrawLine(context.Graphics, context.ClientRectangle.X, r.Bottom+1, context.ClientRectangle.Right, r.Bottom+1, context.GridRowLineColor, 1);
            }

            bool paintChildNodes = (node.Expanded && node.Nodes.Count > 0);
            
            if (!node.IsDragNode && (node.Parent != null || node.ExpandVisibility != eNodeExpandVisibility.Hidden))
                this.PaintConnector(null, node, context, false);

            if (paintChildNodes)
            {
                Node lastChildNode = NodeOperations.GetLastVisibleChildNode(node);
                if (lastChildNode != null)
                    this.PaintConnector(node, lastChildNode, context, false);
            }

            node.SelectedConnectorMarker = false;

            if (NodeDisplay.HasColumnsVisible(node))
            {
                r.Height += node.ColumnHeaderHeight;
                r.Width = Math.Max(r.Width, node.NodesColumns.Bounds.Width);
            }
            if (r.IntersectsWith(context.ClipRectangle) || context.ClipRectangle.IsEmpty)
                PaintSingleNode(node, context);
            
            if (paintChildNodes)
            {
                PaintNodes(node.Nodes, context);
            }

            return true;
		}

        private ElementStyle GetEffectiveNodeBackgroundStyle(Node node, NodeDisplayContext context)
        {
            ElementStyle style = context.DefaultNodeStyle.Copy();

            bool bApplyStyles = true;
            bool isSelected = (node.IsSelected && node.IsSelectionVisible);

            if (isSelected && context.SelectedNodeStyle == null && node.StyleSelected == null && !node.FullRowBackground) bApplyStyles = false;

            if (node.Style != null)
            {
                if (bApplyStyles)
                    style.ApplyStyle(node.Style); //=node.Style.Copy();
                else
                    style.ApplyFontStyle(node.Style);
            }

            if (bApplyStyles && node.MouseOverNodePart == eMouseOverNodePart.Node && style != null)
            {
                ElementStyle mouseOverStyle = context.MouseOverNodeStyle;
                if (node.StyleMouseOver != null)
                {
                    mouseOverStyle = node.StyleMouseOver;
                    bApplyStyles = false;
                }
                if (mouseOverStyle != null)
                {
                    style.ApplyStyle(mouseOverStyle);
                    bApplyStyles = false;
                }
            }

            // On default style apply expanded style
            if (bApplyStyles && node.Expanded && style != null)
            {
                ElementStyle expandedStyle = context.ExpandedNodeStyle;
                if (node.StyleExpanded != null)
                    expandedStyle = node.StyleExpanded;
                if (expandedStyle != null)
                    style.ApplyStyle(expandedStyle);
            }

            // Apply selected style if needed too
            if (bApplyStyles && node.IsSelected && style != null)
            {
                ElementStyle selectedStyle = context.SelectedNodeStyle;
                if (node.StyleSelected != null)
                    selectedStyle = node.StyleSelected;
                if (selectedStyle != null)
                    style.ApplyStyle(selectedStyle);
            }

            if (style != null)
            {
                if (style.Font == null)
                    style.Font = context.DefaultFont;
            }

            return style;
        }

		private void PaintSingleNode(Node node, NodeDisplayContext context)
		{
            _PaintedNodes.Add(node);
			Rectangle r=NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds,node,context.Offset);
			TreeRenderer renderer=context.NodeRenderer;
			if(node.NodeRenderer!=null && node.RenderMode==eNodeRenderMode.Custom)
				renderer = node.NodeRenderer;
            bool isSelected = node.IsSelectionVisible && node.IsSelected;

			// Paint node background
            ElementStyle style = GetEffectiveNodeBackgroundStyle(node, context);

			Region backRegion=null;
			if(style!=null)
			{
				context.NodeRendererEventArgs.Graphics=context.Graphics;
				context.NodeRendererEventArgs.Node = node;
				context.NodeRendererEventArgs.NodeBounds = r;
				context.NodeRendererEventArgs.Style = style;
                if (!node.FullRowBackground) // Node full row backgrounds are drawn first...
                    renderer.DrawNodeBackground(context.NodeRendererEventArgs);
                ElementStyleDisplayInfo di = new ElementStyleDisplayInfo(style, context.Graphics, context.NodeRendererEventArgs.NodeBounds);
                di.Bounds.Inflate(1, 1);
				backRegion=ElementStyleDisplay.GetStyleRegion(di);
                di.Bounds = r;
			}

            if (NodeDisplay.DrawExpandPart(node) && context.ExpandDisplayInfo != null)
            {
                r = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, node, context.Offset);
                context.ExpandDisplayInfo.Node = node;
                context.ExpandDisplayInfo.ExpandPartBounds = r;
                context.ExpandDisplayInfo.IsMouseOver = node.MouseOverNodePart == eMouseOverNodePart.Expand;
                renderer.DrawNodeExpandPart(context.ExpandDisplayInfo);
            }

            if (NodeDisplay.HasColumnsVisible(node))
            {
                PaintColumnHeaders(node.NodesColumns, context.Graphics, false);
            }

			Region oldRegion=null;
			if(backRegion!=null)
			{
				oldRegion=context.Graphics.Clip;
				context.Graphics.SetClip(backRegion,CombineMode.Intersect);
			}
			
			ElementStyle cellStyle = null;
            if (context.CellStyleDefault == null)
            {
                cellStyle = new ElementStyle();
                cellStyle.TextColor = style.TextColor;
                cellStyle.TextShadowColor = style.TextShadowColor;
                cellStyle.TextShadowOffset = style.TextShadowOffset;
                cellStyle.TextAlignment = style.TextAlignment;
                cellStyle.TextLineAlignment = style.TextLineAlignment;
                cellStyle.TextTrimming = style.TextTrimming;
                cellStyle.WordWrap = style.WordWrap;
                cellStyle.Font = style.Font;
                cellStyle.UseMnemonic = style.UseMnemonic;
            }
            
			foreach(Cell cell in node.Cells)
			{
                if (!cell.IsVisible) continue;
                if (cell.StyleNormal != null)
                {
                    if (context.CellStyleDefault != null)
                        style = context.CellStyleDefault.Copy();
                    else
                        style = cellStyle.Copy();
                    style.ApplyStyle(cell.StyleNormal);
                }
                else if (context.CellStyleDefault != null)
                    style = context.CellStyleDefault.Copy();
                else
                {
                    if (!cell.Enabled || cell.IsMouseDown && (cell.StyleMouseDown != null || context.CellStyleMouseDown != null) ||
                        cell.IsSelected && (cell.StyleSelected != null || context.CellStyleSelected != null) ||
                        cell.IsMouseOver && (cell.StyleMouseOver != null || context.CellStyleMouseOver != null))
                        style = cellStyle.Copy();
                    else
                        style = cellStyle;
                }

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
                    else if (cell.IsSelected && context.CellStyleSelected != null && context.CellStyleSelected.Custom)
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
                    if (isSelected)
                    {
                        style.BackColor = Color.Empty;
                        style.BackColorSchemePart = eColorSchemePart.None;
                        style.BackColor2 = Color.Empty;
                        style.BackColor2SchemePart = eColorSchemePart.None;
                    }

					Rectangle rCell=cell.BoundsRelative;
					Rectangle rText=cell.TextContentBounds;
					rCell.Offset(r.Location);
					rText.Offset(r.Location);
					ElementStyleDisplayInfo di=GetElementStyleDisplayInfo(style,context.Graphics,rCell);
					ElementStyleDisplay.Paint(di);
					NodeCellRendererEventArgs ci=GetCellDisplayInfo(style,context.Graphics,cell,r.Location, context.ColorScheme);
					
					if(ci.Cell.CheckBoxVisible)
						renderer.DrawCellCheckBox(ci);
					if(!ci.Cell.Images.LargestImageSize.IsEmpty)
						renderer.DrawCellImage(ci);
					renderer.DrawCellText(ci);
                    if (context.View == eView.Tile && node.HasChildNodes && !context.TileGroupLineColor.IsEmpty)
                        renderer.DrawTileGroupLine(new NodeRendererEventArgs(context.Graphics, node, r, style, context.TileGroupLineColor));                        
				}
			}

            if (backRegion != null)
            {
                context.Graphics.SetClip(oldRegion, CombineMode.Replace);
                backRegion.Dispose();
            }
            if(oldRegion!=null) oldRegion.Dispose();
		}

		private void PaintConnector(Node fromNode, Node toNode, NodeDisplayContext context,bool linkConnector)
		{
			PaintConnector(fromNode, toNode, context, linkConnector, null);
		}
		
		private void PaintConnector(Node fromNode, Node toNode, NodeDisplayContext context,bool linkConnector, ConnectorPointsCollection connectorPoints)
		{
            bool isRootNode = fromNode != null ? IsRootNode(fromNode) : IsRootNode(toNode);

            if (context.View == eView.Tile || isRootNode && fromNode == null && toNode.Nodes.Count == 0 && toNode.ExpandVisibility != eNodeExpandVisibility.Visible || context.ConnectorDisplayInfo == null)
                return;

			context.ConnectorDisplayInfo.FromNode=fromNode;
			context.ConnectorDisplayInfo.ToNode=toNode;
			context.ConnectorDisplayInfo.IsRootNode=isRootNode;
			context.ConnectorDisplayInfo.LinkConnector=linkConnector;
            if (fromNode != null && fromNode.Style != null)
                context.ConnectorDisplayInfo.StyleFromNode = fromNode.Style;
            else
                context.ConnectorDisplayInfo.StyleFromNode = context.DefaultNodeStyle;
			if(toNode.Style!=null)
				context.ConnectorDisplayInfo.StyleToNode=toNode.Style;
			else
				context.ConnectorDisplayInfo.StyleToNode=context.DefaultNodeStyle;
			context.ConnectorDisplayInfo.Offset=context.Offset;
			
			/*if(linkConnector)
			{
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.LinkConnector;
			}
			else*/ if(toNode.SelectedConnectorMarker)
			{
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.SelectedPathConnector;
			}
			else if(toNode.ParentConnector!=null)
			{
				context.ConnectorDisplayInfo.NodeConnector=toNode.ParentConnector;
			}
            //else if(isRootNode)
            //{
            //    context.ConnectorDisplayInfo.NodeConnector=this.Tree.RootConnector;
            //}
			else
			{
				context.ConnectorDisplayInfo.NodeConnector=this.Tree.NodesConnector;
			}

			if(linkConnector)
			{
				context.ConnectorDisplayInfo.ConnectorPoints=connectorPoints;
			}
			else
			{
				if(connectorPoints!=null)
					context.ConnectorDisplayInfo.ConnectorPoints=connectorPoints;
				else
					context.ConnectorDisplayInfo.ConnectorPoints=null;
			}

			context.NodeRenderer.DrawConnector(context.ConnectorDisplayInfo);
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

		private NodeCellRendererEventArgs GetCellDisplayInfo(ElementStyle style, Graphics g, Cell cell, Point cellOffset, ColorScheme cs)
		{
			m_CellDisplayInfo.Cell=cell;
			m_CellDisplayInfo.Graphics=g;
			m_CellDisplayInfo.Style=style;
			m_CellDisplayInfo.CellOffset=cellOffset;
            m_CellDisplayInfo.ColorScheme = cs;
			return m_CellDisplayInfo;
		}
        internal override void PaintColumnHeaders(ColumnHeaderCollection columns, Graphics g, bool treeControlHeader)
        {
             // Setup rendering
            TreeRenderer renderer = m_SystemRenderer;
            if (this.Tree.NodeRenderer != null && this.Tree.RenderMode == eNodeRenderMode.Custom)
                renderer = this.Tree.NodeRenderer;
            //else if (this.Tree.RenderMode == eNodeRenderMode.Professional)
            //    renderer = m_ProfRenderer;
            PaintColumnHeaders(renderer, columns, g, treeControlHeader);
        }

        internal void PaintColumnHeaders(TreeRenderer renderer, ColumnHeaderCollection columns, Graphics g, bool treeControlHeader)
        {
            ColumnHeaderRendererEventArgs ce = new ColumnHeaderRendererEventArgs();
            ce.Graphics = g;
            ce.Tree = this.Tree;
            ce.SortIndicatorColor = renderer.ColorTable.ColumnSortIndicatorColor;

            ElementStyle defaultNormalStyle = GetDefaultColumnStyleNormal(renderer);
            ElementStyle headerStyle = null;
            if(treeControlHeader)
                headerStyle = this.Tree.ColumnsBackgroundStyle == null ? GetDefaultHeaderStyle(renderer) : this.Tree.ColumnsBackgroundStyle;
            else
                headerStyle = this.Tree.NodesColumnsBackgroundStyle == null ? GetDefaultNodesHeaderStyle(renderer) : this.Tree.NodesColumnsBackgroundStyle;

            if (Tree.ColumnStyleNormal != null && Tree.ColumnStyleNormal.Custom)
                defaultNormalStyle = Tree.ColumnStyleNormal;

            Point offset = Point.Empty;
            if (this.Tree.AutoScroll)
            {
                offset = this.Tree.GetAutoScrollPositionOffset();
                if (treeControlHeader)
                    offset.Y = 0;
            }

            Rectangle columnsBounds = columns.Bounds;
            if (!treeControlHeader) columnsBounds.Offset(offset);
            ElementStyleDisplayInfo di = new ElementStyleDisplayInfo(headerStyle, g, columnsBounds);
            ElementStyleDisplay.Paint(di);
            Color columnSeparator = (headerStyle != null && !headerStyle.BorderColor.IsEmpty) ? headerStyle.BorderColor : Color.Empty;
            for (int i = 0; i < columns.Count; i++)
            {
                ColumnHeader column = columns.ColumnAtDisplayIndex(i);
                if (!column.Visible) continue;
                ElementStyle style = null;
                if (column.StyleNormal != "")
                    style = Tree.Styles[column.StyleNormal].Copy();
                else
                    style = defaultNormalStyle.Copy();

                if (column.IsMouseDown)
                {
                    if (column.StyleMouseDown != "")
                        style.ApplyStyle(Tree.Styles[column.StyleMouseDown]);
                    else if (Tree.ColumnStyleMouseDown != null)
                        style.ApplyStyle(Tree.ColumnStyleMouseDown);
                }
                else if (column.IsMouseOver)
                {
                    if (column.StyleMouseOver != "")
                        style.ApplyStyle(Tree.Styles[column.StyleMouseOver]);
                    else if (Tree.ColumnStyleMouseOver != null)
                        style.ApplyStyle(Tree.ColumnStyleMouseOver);
                }

                ce.ColumnHeader = column;
                Rectangle columnBounds = column.Bounds;
                columnBounds.Offset(offset);
                ce.Bounds = columnBounds;
                ce.Style = style;
                renderer.DrawColumnHeader(ce);
                if (!columnSeparator.IsEmpty)
                    DisplayHelp.DrawLine(g, columnBounds.Right - (column.IsLastVisible ? 0 : 1), columnBounds.Y, columnBounds.Right - (column.IsLastVisible ? 0 : 1), columnBounds.Bottom - 1, columnSeparator, 1);
            }
        }

        private ElementStyle GetDefaultHeaderStyle(TreeRenderer renderer)
        {
            if (renderer != null && renderer.Office2007ColorTable != null)
            {
                return renderer.Office2007ColorTable.StyleClasses[ElementStyleClassKeys.TreeColumnsHeaderKey] as ElementStyle;
            }
            else
            {
                ElementStyle style = new ElementStyle();
                style.BackColor = ColorScheme.GetColor(0xF9FCFD);
                style.BackColor2 = ColorScheme.GetColor(0xD3DBE9);
                style.BackColorGradientAngle = 90;
                style.TextColor = ColorScheme.GetColor(0x000000);
                style.BorderColor = ColorScheme.GetColor(0x9EB6CE);
                style.BorderBottom = eStyleBorderType.Solid;
                style.BorderBottomWidth = 1;
                return style;
            }
        }

        private ElementStyle GetDefaultNodesHeaderStyle(TreeRenderer renderer)
        {
            if (renderer != null && renderer.Office2007ColorTable != null)
            {
                return renderer.Office2007ColorTable.StyleClasses[ElementStyleClassKeys.TreeNodesColumnsHeaderKey] as ElementStyle;
            }
            else
            {
                ElementStyle style = new ElementStyle();
                style.BackColor = ColorScheme.GetColor(0xF9FCFD);
                style.BackColor2 = ColorScheme.GetColor(0xD3DBE9);
                style.BackColorGradientAngle = 90;
                style.TextColor = ColorScheme.GetColor(0x000000);
                style.BorderColor = ColorScheme.GetColor(0x9EB6CE);
                style.BorderBottom = eStyleBorderType.Solid;
                style.BorderBottomWidth = 1;
                style.BorderLeft = eStyleBorderType.Solid;
                style.BorderLeftWidth = 1;
                style.BorderTop = eStyleBorderType.Solid;
                style.BorderTopWidth = 1;
                return style;
            }
        }

        private ElementStyle GetDefaultColumnStyleNormal(TreeRenderer renderer)
        {
            if (renderer != null && renderer.Office2007ColorTable != null)
            {
                return renderer.Office2007ColorTable.StyleClasses[ElementStyleClassKeys.TreeColumnKey] as ElementStyle;
            }
            else
            {
                ElementStyle style = new ElementStyle();
                style.TextColor = ColorScheme.GetColor(0x000000);
                return style;
            }
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
			//public NodeCommandPartRendererEventArgs CommandDisplayInfo=null;
			public bool SelectedNodePath=false;
			public TreeRenderer NodeRenderer=null;
			public NodeRendererEventArgs NodeRendererEventArgs=null;
            public bool IsBackgroundSelection = false;
            public Rectangle ClientRectangle = Rectangle.Empty;
            public ColorScheme ColorScheme = null;
            public bool GridRowLines = false;
            public Color GridRowLineColor = Color.Empty;
            public bool CellSelection = false;
            public Brush AlternateRowBrush = null;
            public bool DrawAlternateRowBackground = false;
            public bool AlternateRowFlag = false;
            public eView View = eView.Tree;
            public Color TileGroupLineColor = Color.Empty;
		}

		
	}
}
