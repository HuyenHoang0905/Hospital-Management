using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Layout
{
	/// <summary>
	/// Performs classic TreeView layout.
	/// </summary>
	internal class NodeTreeLayout:NodeLayout
	{
        public NodeTreeLayout(AdvTree treeControl, Rectangle clientArea, LayoutSettings layoutSettings)
            : base(treeControl, clientArea, layoutSettings)
		{
		}

        public override void UpdateTopLevelColumnsWidth()
        {
            if (this.Tree.Columns.Count > 0)
            {
                Rectangle columnsBounds = DevComponents.DotNetBar.ElementStyleLayout.GetInnerRect(this.Tree.BackgroundStyle, this.Tree.ClientRectangle);
                if (this.Tree.VScrollBar != null) columnsBounds.Width -= this.Tree.VScrollBar.Width;
                columnsBounds.Height = this.Tree.Columns.Bounds.Height;
                if(this.Tree.Columns.Bounds.Width<columnsBounds.Width)
                    this.Tree.Columns.SetBounds(columnsBounds);
            }
        }

		public override void PerformLayout()
		{
			this.PrepareStyles();
            Rectangle area = Rectangle.Empty;
			Graphics g=this.GetGraphics();
            SmoothingMode sm = g.SmoothingMode;
            TextRenderingHint th = g.TextRenderingHint;
            if (m_Tree.AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            NodeLayoutContextInfo layoutInfo = GetDefaultNodeLayoutContextInfo(g);
            layoutInfo.ExpandPartAlignedLeft = true;
            layoutInfo.Left = ClientArea.X;
            layoutInfo.Top = ClientArea.Y;

            CellLayout cellLayout = this.GetCellLayout();
            cellLayout.ResetCheckBoxSize();
            if (this.Tree.CheckBoxImageChecked != null)
                cellLayout.CheckBoxSize = this.Tree.CheckBoxImageChecked.Size;

            LayoutTopLevelColumns(layoutInfo);

            // Get default Columns
            NodeColumnInfo defaultColInfoList = this.GetDefaultColumnInfo();
            layoutInfo.DefaultColumns = defaultColInfoList;
			try
			{
				// Loop through each top-level node
				Node[] topLevelNodes=this.GetTopLevelNodes();
                int defaultTop = layoutInfo.Top;
                area = ProcessTopLevelNodes(area, layoutInfo, topLevelNodes);
                bool hasMinColumnAutoSizeWidth = false;
                bool hasStretchToFillColumn = false;
                if (defaultColInfoList.HasAutoSizeColumn)
                {
                    foreach (ColumnInfo columnInfo in defaultColInfoList.ColumnInfo)
                    {
                        if (columnInfo.AutoSize)
                        {
                            columnInfo.AutoSize = false;
                            columnInfo.Width = columnInfo.MaxWidth;
                            columnInfo.ColumnHeader.Width.SetAutoSizeWidth(columnInfo.MaxWidth);
                            columnInfo.MaxWidth = 0;
                            if (columnInfo.ColumnHeader.Width.AutoSizeMinHeader)
                                hasMinColumnAutoSizeWidth = true;
                            if (columnInfo.ColumnHeader.StretchToFill)
                                hasStretchToFillColumn = true;
                        }
                    }
                    layoutInfo.ContextNode = null;
                    LayoutTopLevelColumns(layoutInfo);
                    // Adjust the of auto sized columns in case minimum header width is used
                    if (hasMinColumnAutoSizeWidth || hasStretchToFillColumn)
                    {
                        foreach (ColumnInfo columnInfo in defaultColInfoList.ColumnInfo)
                        {
                            if (columnInfo.ColumnHeader.Width.AutoSize && columnInfo.ColumnHeader.Width.AutoSizeMinHeader || columnInfo.ColumnHeader.StretchToFill)
                            {
                                columnInfo.Width = columnInfo.ColumnHeader.Bounds.Width;
                            }
                        }
                    }

                    layoutInfo.Top = defaultTop;
                    area = ProcessTopLevelNodes(Rectangle.Empty, layoutInfo, topLevelNodes);
                }
			}
			finally
			{
                if (m_Tree.AntiAlias)
                {
                    g.SmoothingMode = sm;
                    //g.TextRenderingHint = th;
                }

				if(this.DisposeGraphics)
					g.Dispose();
			}
            if (layoutInfo.FullRowBackgroundNodes.Count > 0)
                Tree.FullRowBackgroundNodes = layoutInfo.FullRowBackgroundNodes;
            else
                Tree.FullRowBackgroundNodes = null;
            //if (columnsVisible && layoutInfo.DefaultColumns != null && layoutInfo.DefaultColumns.Count > 0)
            //{
            //    bool layoutColumns = false;
            //    for (int i = 0; i < layoutInfo.DefaultColumns.Count; i++)
            //    {
            //        ColumnInfo ci = (ColumnInfo)layoutInfo.DefaultColumns[i];
            //        if (ci.Width == 0 && ci.MaxWidth > 0)
            //        {
            //            ci.ColumnHeader.ContentWidth = ci.MaxWidth;
            //            layoutColumns = true;
            //        }
            //    }
            //    if (layoutColumns)
            //    {
            //        layoutInfo.ContextNode = null;
            //        layoutInfo.TreeColumns = this.Tree.Columns;
            //        Layout.ColumnHeaderLayout.LayoutColumnHeader(layoutInfo, ClientArea.X,
            //        ClientArea.Y, ClientArea.Width, this.GetCellLayout().CellHorizontalSpacing);
            //    }
            //}

            m_Width = area.Width;
            m_Height = area.Height;
		}

        private void LayoutTopLevelColumns(NodeLayoutContextInfo layoutInfo)
        {
            // Layout tree columns
            if (this.Tree.Columns.Count > 0)
            {
                Rectangle columnsBounds = DevComponents.DotNetBar.ElementStyleLayout.GetInnerRect(this.Tree.BackgroundStyle, this.Tree.ClientRectangle);
                if (this.Tree.VScrollBar != null) columnsBounds.Width -= this.Tree.VScrollBar.Width;
                layoutInfo.TreeColumns = this.Tree.Columns;
                int columnHeight = Layout.ColumnHeaderLayout.LayoutColumnHeader(layoutInfo, 0,
                    0, columnsBounds.Width, this.GetCellLayout().LayoutSettings.CellHorizontalSpacing);
                columnHeight += this.LayoutSettings.NodeVerticalSpacing;
                if (this.Tree.ColumnsVisible)
                {
                    Rectangle headerBounds = layoutInfo.TreeColumns.Bounds;
                    if (headerBounds.Width > 0 && headerBounds.Width < columnsBounds.Width)
                    {
                        headerBounds.Width = columnsBounds.Width;
                        layoutInfo.TreeColumns.SetBounds(headerBounds);
                    }
                    layoutInfo.Top += columnHeight;
                    this.Tree.SetColumnHeaderControlVisibility(true);
                }
                else
                    this.Tree.SetColumnHeaderControlVisibility(false);
                layoutInfo.TreeColumns = null;
            }
            else
                this.Tree.SetColumnHeaderControlVisibility(false);
        }

        private Rectangle ProcessTopLevelNodes(Rectangle area, NodeLayoutContextInfo layoutInfo, Node[] topLevelNodes)
        {
            foreach (Node childNode in topLevelNodes)
            {
                layoutInfo.ContextNode = childNode;
                ProcessNode(layoutInfo);
                if (childNode.Visible)
                {
                    area = Rectangle.Union(area, childNode.BoundsRelative);
                    if (childNode.Expanded)
                        area = Rectangle.Union(area, childNode.ChildNodesBounds);
                }
            }
            return area;
        }

		#region Node routines
		private void ProcessNode(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;
            if (!node.Visible) return;

            int originalTop = layoutInfo.Top;

            if (node.SizeChanged || node.HasColumns || layoutInfo.DefaultColumns!=null && layoutInfo.DefaultColumns.HasAutoSizeColumn || layoutInfo.ChildColumns!=null && layoutInfo.ChildColumns.HasAutoSizeColumn)
            {
                // Calculate size of the node itself...
                LayoutNode(layoutInfo);
            }
            if (node.FullRowBackground)
                layoutInfo.FullRowBackgroundNodes.Add(node);

            if (node.BoundsRelative.X != layoutInfo.Left || node.BoundsRelative.Y != layoutInfo.Top)
            {
                // Adjust top position
                node.SetBounds(new Rectangle(layoutInfo.Left,layoutInfo.Top,node.BoundsRelative.Width,node.BoundsRelative.Height));
                //foreach(Cell c in node.Cells)
                //    c.SetBounds(new Rectangle(c.BoundsRelative.X + layoutInfo.Left, c.BoundsRelative.Y+layoutInfo.Top, c.BoundsRelative.Width, c.BoundsRelative.Height));
            }

            int nodeVerticalSpacing = this.LayoutSettings.NodeVerticalSpacing;
			// Need to set the Top position properly
            layoutInfo.Top += (node.BoundsRelative.Height + nodeVerticalSpacing);
            if (DevComponents.AdvTree.Display.NodeDisplay.HasColumnsVisible(node))
                layoutInfo.Top += node.ColumnHeaderHeight;
			
			if(node.Expanded)
			{
				int originalLevelOffset=layoutInfo.Left;
                int childNodesTop = layoutInfo.Top;
                layoutInfo.Left += this.NodeLevelOffset + node.NodesIndent;
                NodeColumnInfo parentColumns = layoutInfo.ChildColumns;
                NodeColumnInfo childColumns = GetNodeColumnInfo(node);
                Rectangle childNodesBounds = ProcessChildNodes(layoutInfo, node, nodeVerticalSpacing, childColumns);

                if (childColumns != null && childColumns.HasAutoSizeColumn)
                {
                    bool hasMinColumnAutoSizeWidth = false;
                    foreach (ColumnInfo columnInfo in childColumns.ColumnInfo)
                    {
                        if (columnInfo.AutoSize)
                        {
                            columnInfo.Width = columnInfo.MaxWidth;
                            columnInfo.ColumnHeader.Width.SetAutoSizeWidth(columnInfo.MaxWidth);
                            columnInfo.AutoSize = false;
                            columnInfo.MaxWidth = 0;
                            if (columnInfo.ColumnHeader.Width.AutoSizeMinHeader)
                                hasMinColumnAutoSizeWidth = true;
                        }
                    }
                    layoutInfo.Top = originalTop;
                    layoutInfo.Left = originalLevelOffset;
                    layoutInfo.ContextNode = node;
                    layoutInfo.ChildColumns = parentColumns;
                    LayoutNode(layoutInfo);
                    layoutInfo.Top = childNodesTop;
                    layoutInfo.Left += this.NodeLevelOffset + node.NodesIndent;

                    // Adjust the of auto sized columns in case minimum header width is used
                    if (hasMinColumnAutoSizeWidth)
                    {
                        foreach (ColumnInfo columnInfo in childColumns.ColumnInfo)
                        {
                            if (columnInfo.ColumnHeader.Width.AutoSize && columnInfo.ColumnHeader.Width.AutoSizeMinHeader)
                            {
                                columnInfo.Width = columnInfo.ColumnHeader.Bounds.Width;
                            }
                        }
                    }

                    childNodesBounds = ProcessChildNodes(layoutInfo, node, nodeVerticalSpacing, childColumns);
                }

                node.ChildNodesBounds = childNodesBounds;

				layoutInfo.ChildColumns=parentColumns;

				layoutInfo.ContextNode=node;
				layoutInfo.Left=originalLevelOffset;
			}
		}

        private Rectangle ProcessChildNodes(NodeLayoutContextInfo layoutInfo, Node node, int nodeVerticalSpacing, NodeColumnInfo childColumns)
        {
            Rectangle childNodesBounds = new Rectangle(layoutInfo.Left, layoutInfo.Top, 0, 0);

            foreach (Node childNode in node.Nodes)
            {
                if (!childNode.Visible) continue;
                layoutInfo.ContextNode = childNode;
                layoutInfo.ChildColumns = childColumns;
                ProcessNode(layoutInfo);
                childNodesBounds.Width = Math.Max(childNodesBounds.Width,
                    Math.Max(childNode.BoundsRelative.Width, (childNode.Expanded && childNode.ChildNodesBounds.Width > 0 ? childNode.ChildNodesBounds.Right - childNodesBounds.X : 0)));
                childNodesBounds.Height += childNode.BoundsRelative.Height + (childNode.Expanded ? childNode.ChildNodesBounds.Height + childNode.ColumnHeaderHeight : 0) + nodeVerticalSpacing;
            }
            return childNodesBounds;
        }

		/// <summary>
		/// Returns true if expand part space should be accounted for even if they expand part is not visible or need to be displayed. Default value is false.
		/// </summary>
		protected override bool ReserveExpandPartSpace
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets whether the expand part of the node +/- is aligned to the left of the node in left-to-right layout.
		/// </summary>
		/// <param name="node">Node to get expand part alignment for</param>
		/// <returns>true if node expand part is aligned to the left in left-to-right layout.</returns>
		private bool ExpandPartAlignedNear(Node node)
		{
			return true; // If changed LayoutExpandPart needs to be updated as well
		}

//		private NodeCollection GetTopLevelNodes()
//		{
//			return m_Tree.Nodes;
//		}

        

		#endregion
	}
}
