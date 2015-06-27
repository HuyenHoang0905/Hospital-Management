using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.AdvTree.Layout
{
    /// <summary>
    /// Performs ListView Tile style layout.
    /// </summary>
    internal class NodeTileLayout : NodeLayout
    {
        public NodeTileLayout(AdvTree treeControl, Rectangle clientArea, LayoutSettings layoutSettings)
            : base(treeControl, clientArea, layoutSettings)
		{
		}

        public override void UpdateTopLevelColumnsWidth()
        {
            // Columns are not visible in tile layout
            this.Tree.Columns.SetBounds(Rectangle.Empty);
        }

        /// <summary>
        /// Returns default top-level columns for tree control.
        /// </summary>
        /// <returns>Returns array list of ColumnInfo objects.</returns>
        protected override NodeColumnInfo GetDefaultColumnInfo()
        {
            // There are no columns in Tile view
            List<ColumnInfo> ci = new List<ColumnInfo>();
            NodeColumnInfo info = new NodeColumnInfo(ci, false);
            return info;
        }

        protected override NodeLayoutContextInfo GetDefaultNodeLayoutContextInfo(System.Drawing.Graphics graphics)
        {
            NodeLayoutContextInfo context = base.GetDefaultNodeLayoutContextInfo(graphics);
            context.IsViewGroupping = this.Groupping;
            return context;
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

                //if (defaultColInfoList.HasAutoSizeColumn)
                //{
                //    foreach (ColumnInfo columnInfo in defaultColInfoList.ColumnInfo)
                //    {
                //        if (columnInfo.AutoSize)
                //        {
                //            columnInfo.AutoSize = false;
                //            columnInfo.Width = columnInfo.MaxWidth;
                //            columnInfo.ColumnHeader.Width.SetAutoSizeWidth(columnInfo.MaxWidth);
                //            columnInfo.MaxWidth = 0;
                //        }
                //    }
                //    layoutInfo.ContextNode = null;
                //    LayoutTopLevelColumns(layoutInfo);
                //    layoutInfo.Top = defaultTop;
                //    area = ProcessTopLevelNodes(Rectangle.Empty, layoutInfo, topLevelNodes);
                //}
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

            m_Width = area.Width;
            m_Height = area.Height;
		}

        private void LayoutTopLevelColumns(NodeLayoutContextInfo layoutInfo)
        {
            // There are no columns in Tile view
            this.Tree.SetColumnHeaderControlVisibility(false);
        }

        private Rectangle ProcessTopLevelNodes(Rectangle area, NodeLayoutContextInfo layoutInfo, Node[] topLevelNodes)
        {
            layoutInfo.CurrentLevelLeft = layoutInfo.Left;
            bool isPreviousGroupNode = false;
            foreach (Node childNode in topLevelNodes)
            {
                layoutInfo.ContextNode = childNode;
                if (childNode.Visible)
                {
                    if (_Groupping && childNode.HasChildNodes)
                    {
                        if (layoutInfo.CurrentLineHeight > 0)
                            layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                        layoutInfo.CurrentLineHeight = 0;
                        layoutInfo.Left = layoutInfo.CurrentLevelLeft;
                        isPreviousGroupNode = true;
                    }
                    else
                    {
                        if (isPreviousGroupNode)
                        {
                            if (layoutInfo.CurrentLineHeight > 0)
                                layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                            layoutInfo.CurrentLineHeight = 0;
                            layoutInfo.Left = layoutInfo.CurrentLevelLeft;
                        }
                        isPreviousGroupNode = false;
                    }
                }

                ProcessNode(layoutInfo);

                if (childNode.Visible)
                {
                    area = Rectangle.Union(area, childNode.BoundsRelative);
                    if (childNode.Expanded && childNode.HasChildNodes)
                        area = Rectangle.Union(area, childNode.ChildNodesBounds);
                    if (!(_Groupping && childNode.HasChildNodes))
                        layoutInfo.Left += childNode.BoundsRelative.Width + this.LayoutSettings.NodeHorizontalSpacing;
                }
            }
            return area;
        }

        private Rectangle ProcessChildNodes(NodeLayoutContextInfo layoutInfo, Node node, int nodeVerticalSpacing, NodeColumnInfo childColumns)
        {
            Rectangle childNodesBounds = new Rectangle(layoutInfo.Left, layoutInfo.Top, 0, 0);
            bool isPreviousGroupNode = false;

            foreach (Node childNode in node.Nodes)
            {
                if (!childNode.Visible) continue;

                if (_Groupping && childNode.HasChildNodes)
                {
                    if (layoutInfo.CurrentLineHeight > 0)
                        layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                    layoutInfo.CurrentLineHeight = 0;
                    layoutInfo.Left = layoutInfo.CurrentLevelLeft;
                    isPreviousGroupNode = true;
                }
                else
                {
                    if (isPreviousGroupNode)
                    {
                        if (layoutInfo.CurrentLineHeight > 0)
                            layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                        layoutInfo.CurrentLineHeight = 0;
                        layoutInfo.Left = layoutInfo.CurrentLevelLeft;
                        isPreviousGroupNode = false;
                    }
                }

                layoutInfo.ContextNode = childNode;
                layoutInfo.ChildColumns = childColumns;
                ProcessNode(layoutInfo);

                if (!(_Groupping && childNode.HasChildNodes))
                    layoutInfo.Left += childNode.BoundsRelative.Width + this.LayoutSettings.NodeHorizontalSpacing;
                if (isPreviousGroupNode)
                {
                    childNodesBounds.Width = Math.Max(childNodesBounds.Width,
                        Math.Max(childNode.BoundsRelative.Width, (childNode.Expanded && childNode.ChildNodesBounds.Width > 0 ? childNode.ChildNodesBounds.Right - childNodesBounds.X : 0)));
                    childNodesBounds.Height += childNode.BoundsRelative.Height + (childNode.Expanded ? childNode.ChildNodesBounds.Height + childNode.ColumnHeaderHeight : 0) + nodeVerticalSpacing;
                }
                else
                {
                    childNodesBounds = Rectangle.Union(childNodesBounds, childNode.Bounds);
                }
            }
            return childNodesBounds;
        }


		#region Node routines
		private void ProcessNode(NodeLayoutContextInfo layoutInfo)
		{
			Node node=layoutInfo.ContextNode;
            if (!node.Visible || node.Cells.Count == 0) return;

            if (node.SizeChanged || _Groupping && node.HasChildNodes)
            {
                // Calculate size of the node itself...
                LayoutNode(layoutInfo);
            }
            if (node.FullRowBackground)
                layoutInfo.FullRowBackgroundNodes.Add(node);

            // Position the node
            if (_Groupping && node.HasChildNodes)
            {
                if (layoutInfo.CurrentLineHeight > 0)
                    layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                layoutInfo.CurrentLineHeight = 0;
            }
            else
            {
                if (layoutInfo.Left + node.BoundsRelative.Width > this.ClientArea.Right)
                {
                    layoutInfo.Left = layoutInfo.CurrentLevelLeft;
                    layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                    layoutInfo.CurrentLineHeight = 0;
                }
            }
            layoutInfo.CurrentLineHeight = Math.Max(layoutInfo.CurrentLineHeight, node.BoundsRelative.Height);

            if (node.BoundsRelative.X != layoutInfo.Left || node.BoundsRelative.Y != layoutInfo.Top)
            {
                // Adjust top position
                node.SetBounds(new Rectangle(layoutInfo.Left,layoutInfo.Top,node.BoundsRelative.Width,node.BoundsRelative.Height));
            }

            // Position the node
            if (_Groupping && node.HasChildNodes)
            {
                if (layoutInfo.CurrentLineHeight > 0)
                    layoutInfo.Top += layoutInfo.CurrentLineHeight + this.LayoutSettings.NodeVerticalSpacing;
                layoutInfo.CurrentLineHeight = 0;
            }

            int nodeVerticalSpacing = this.LayoutSettings.NodeVerticalSpacing;
			// Need to set the Top position properly
            //layoutInfo.Top += (node.BoundsRelative.Height + nodeVerticalSpacing);
            // No columns in tile view
            //if (DevComponents.AdvTree.Display.NodeDisplay.HasColumnsVisible(node))
            //    layoutInfo.Top += node.ColumnHeaderHeight;

			if(_Groupping && node.HasChildNodes && node.Expanded)
			{
				int originalLevelOffset=layoutInfo.Left;
                int originalLevelLeft = layoutInfo.CurrentLevelLeft;
                int childNodesTop = layoutInfo.Top;
                layoutInfo.Left += this.NodeLevelOffset + node.NodesIndent;
                layoutInfo.CurrentLevelLeft = layoutInfo.Left;
                NodeColumnInfo parentColumns = layoutInfo.ChildColumns;
                NodeColumnInfo childColumns = GetNodeColumnInfo(node);
                Rectangle childNodesBounds = ProcessChildNodes(layoutInfo, node, nodeVerticalSpacing, childColumns);

                //if (childColumns != null && childColumns.HasAutoSizeColumn)
                //{
                //    foreach (ColumnInfo columnInfo in childColumns.ColumnInfo)
                //    {
                //        if (columnInfo.AutoSize)
                //        {
                //            columnInfo.Width = columnInfo.MaxWidth;
                //            columnInfo.ColumnHeader.Width.SetAutoSizeWidth(columnInfo.MaxWidth);
                //            columnInfo.AutoSize = false;
                //            columnInfo.MaxWidth = 0;
                //        }
                //    }
                //    layoutInfo.Top = originalTop;
                //    layoutInfo.Left = originalLevelOffset;
                //    layoutInfo.ContextNode = node;
                //    layoutInfo.ChildColumns = null;
                //    LayoutNode(layoutInfo);
                //    layoutInfo.Top = childNodesTop;
                //    layoutInfo.Left += this.NodeLevelOffset + node.NodesIndent;
                //    childNodesBounds = ProcessChildNodes(layoutInfo, node, nodeVerticalSpacing, childColumns);
                //}

                node.ChildNodesBounds = childNodesBounds;

				layoutInfo.ChildColumns=parentColumns;

				layoutInfo.ContextNode=node;
				layoutInfo.Left=originalLevelOffset;
                layoutInfo.CurrentLevelLeft = originalLevelLeft;
			}
            else
                node.ChildNodesBounds = Rectangle.Empty;
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

        /// <summary>
        /// Returns column information for a given node.
        /// </summary>
        /// <param name="node">Node to return column information for</param>
        /// <returns>Returns array list of ColumnInfo objects or null if there are no columns defined.</returns>
        protected override NodeColumnInfo GetNodeColumnInfo(Node node)
        {
            // No columns in tile-view
            return null;
        }

        /// <summary>
        /// Returns true if expand part space should be accounted for even if they expand part is not visible or need to be displayed. Default value is false.
        /// </summary>
        protected override bool ReserveExpandPartSpace
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if given node has expand part.
        /// </summary>
        /// <param name="layoutInfo">Layout context information.</param>
        /// <returns></returns>
        protected override bool HasExpandPart(NodeLayoutContextInfo layoutInfo)
        {
            Node node = layoutInfo.ContextNode;
            if (node.ExpandVisibility == eNodeExpandVisibility.Auto)
            {
                if (!_Groupping || !NodeOperations.GetAnyVisibleNodes(node))
                    return false;
                return true;
            }
            else
                return (node.ExpandVisibility == eNodeExpandVisibility.Visible);
        }


        private bool _Groupping = true;
        /// <summary>
        /// Gets or sets whether parent/child node relationship is displayed as groups.
        /// </summary>
        public bool Groupping
        {
            get { return _Groupping; }
            set
            {
                _Groupping = value;
            }
        }

        private CellTileLayout _CellLayout = null;
        /// <summary>
        /// Returns class responsible for cell layout.
        /// </summary>
        /// <returns>Cell layout class.</returns>
        protected internal override CellLayout GetCellLayout()
        {
            if (_CellLayout == null)
                _CellLayout = new CellTileLayout(this.LayoutSettings);
            return _CellLayout;
        }
		#endregion
    }
}
