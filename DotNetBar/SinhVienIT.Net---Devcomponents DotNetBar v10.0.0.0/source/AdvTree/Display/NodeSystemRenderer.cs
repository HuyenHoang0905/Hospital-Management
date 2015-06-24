using System;
using System.Drawing;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents default system node and cell renderer.
	/// </summary>
	public class NodeSystemRenderer:TreeRenderer
	{
		#region Private Variables
		private NodeExpandEllipseDisplay m_NodeExpandEllipseDisplay=new NodeExpandEllipseDisplay();
		private NodeExpandRectDisplay m_NodeExpandRectDisplay=new NodeExpandRectDisplay();
        private NodeExpandTriangleDisplay m_NodeExpandTriangleDisplay = new NodeExpandTriangleDisplay();
		private NodeExpandImageDisplay m_NodeExpandImageDisplay=new NodeExpandImageDisplay();
		private ElementStyleDisplayInfo m_ElementStyleDisplayInfo=new ElementStyleDisplayInfo();
		private NodeSelectionDisplay m_SelectionDisplay=new NodeSelectionDisplay();
		private LineConnectorDisplay m_LineConnectorDisplay=null;
        private DragDropMarkerDisplay m_DragDropMarkerDisplay = new DragDropMarkerDisplay();
        private ColumnHeaderDisplay m_ColumnHeaderDisplay = new ColumnHeaderDisplay();
        private Office2007CheckBoxItemPainter m_CheckBoxPainter = new Office2007CheckBoxItemPainter();
        private NodeGroupLineDisplay _GroupLineDisplay = new NodeGroupLineDisplay();
		#endregion
		
		#region Internal Implementation
		/// <summary>
		/// Returns ElementStyleDisplayInfo class that provides information for ElementStyle rendering.
		/// </summary>
		/// <param name="style">Reference to style.</param>
		/// <param name="g">Reference to graphics object.</param>
		/// <param name="bounds">Style bounds</param>
		/// <returns>New instance of ElementStyleDisplayInfo</returns>
		protected ElementStyleDisplayInfo GetElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds)
		{
			m_ElementStyleDisplayInfo.Style=style;
			m_ElementStyleDisplayInfo.Graphics=g;
			m_ElementStyleDisplayInfo.Bounds=bounds;
			return m_ElementStyleDisplayInfo;
		}
		
		private NodeConnectorDisplay GetConnectorDisplay(NodeConnector c)
		{
			NodeConnectorDisplay d=null;
			if(c==null)
				return null;

			switch(c.ConnectorType)
			{
				case eNodeConnectorType.Line:
				{
					if(m_LineConnectorDisplay==null)
						m_LineConnectorDisplay=new LineConnectorDisplay();
					d=m_LineConnectorDisplay;
					break;
				}
			}
			return d;
		}
		
		/// <summary>
		/// Draws node background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawNodeBackground(NodeRendererEventArgs e)
		{
            ElementStyleDisplayInfo di = GetElementStyleDisplayInfo(e.Style, e.Graphics, e.NodeBounds);
			ElementStyleDisplay.Paint(di);
			
			base.DrawNodeBackground(e);
		}
		
		/// <summary>
		/// Draws node expand part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeExpandPart method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawNodeExpandPart(NodeExpandPartRendererEventArgs e)
		{
            NodeExpandDisplay expandDisplay = GetExpandDisplay(e.ExpandButtonType);
            expandDisplay.ColorTable = this.ColorTable;
            expandDisplay.DrawExpandButton(e);
            expandDisplay.ColorTable = null;
			base.DrawNodeExpandPart(e);
		}
		
		private NodeExpandDisplay GetExpandDisplay(eExpandButtonType e)
		{
			NodeExpandDisplay d=null;
			switch(e)
			{
				case eExpandButtonType.Rectangle:
					d=m_NodeExpandRectDisplay;
					break;
                case eExpandButtonType.Triangle:
                    d = m_NodeExpandTriangleDisplay;
                    break;
                case eExpandButtonType.Ellipse:
                    d = m_NodeExpandEllipseDisplay;
                    break;
				case eExpandButtonType.Image:
					d= m_NodeExpandImageDisplay;
					break;
			}
			return d;
		}
		
        ///// <summary>
        ///// Draws node command part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeCommandPart method so events can occur.
        ///// </summary>
        ///// <param name="e">Information provided for rendering.</param>
        //public override void DrawNodeCommandPart(NodeCommandPartRendererEventArgs e)
        //{
        //    m_NodeCommandDisplay.DrawCommandButton(e);
        //    base.DrawNodeCommandPart(e);
        //}
		
		/// <summary>
		/// Draws cell background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawCellBackground(NodeCellRendererEventArgs e)
		{
			ElementStyleDisplayInfo di=GetElementStyleDisplayInfo(e.Style,e.Graphics,DisplayHelp.GetDrawRectangle(e.CellBounds));
			ElementStyleDisplay.Paint(di);
			
			base.DrawCellBackground(e);
		}
		
		/// <summary>
		/// Draws cell check box.  If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellCheckBox method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawCellCheckBox(NodeCellRendererEventArgs e)
		{
            CellDisplay.CheckBoxPainter = m_CheckBoxPainter;
            if (Office2007ColorTable != null)
                CellDisplay.ColorTable = Office2007ColorTable.CheckBoxItem;
			CellDisplay.PaintCellCheckBox(e);
			base.DrawCellCheckBox(e);
            CellDisplay.CheckBoxPainter = null;
		}
		
		/// <summary>
		/// Draws cell image. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellImage method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawCellImage(NodeCellRendererEventArgs e)
		{
			CellDisplay.PaintCellImage(e);
			base.DrawCellImage(e);
		}
		
		/// <summary>
		/// Draws cell text. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellText method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawCellText(NodeCellRendererEventArgs e)
		{
			CellDisplay.PaintText(e);
			base.DrawCellText(e);
		}
		
		/// <summary>
		/// Draws selection for SelectedNode. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderSelection method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawSelection(SelectionRendererEventArgs e)
		{
            m_SelectionDisplay.SelectionColors = ColorTable.Selection;
			m_SelectionDisplay.PaintSelection(e);
			base.DrawSelection(e);
		}

        /// <summary>
        /// Draws hot-tracking marker for mouse over node. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderHotTracking method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public override void DrawHotTracking(SelectionRendererEventArgs e)
        {
            m_SelectionDisplay.SelectionColors = ColorTable.Selection;
            m_SelectionDisplay.PaintHotTracking(e);
            base.DrawHotTracking(e);
        }
		
		/// <summary>
		/// Draws connector between nodes. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderConnector method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawConnector(ConnectorRendererEventArgs e)
		{
			NodeConnectorDisplay display = GetConnectorDisplay(e.NodeConnector);
			if(display!=null)
				display.DrawConnector(e);

			base.DrawConnector(e);
		}
		
		/// <summary>
		/// Draws the tree background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderTreeBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawTreeBackground(TreeBackgroundRendererEventArgs e)
		{
			AdvTree tree = e.AdvTree;
			Graphics g = e.Graphics;
			
			if(!tree.BackColor.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(tree.BackColor))
					g.FillRectangle(brush,tree.DisplayRectangle);
			}

			ElementStyleDisplayInfo info=new ElementStyleDisplayInfo();
			info.Bounds=tree.DisplayRectangle;
			info.Graphics=g;
			info.Style=tree.BackgroundStyle;
			ElementStyleDisplay.Paint(info);
			
			base.DrawTreeBackground (e);
		}

        /// <summary>
        /// Draws the drag &amp; drop marker that indicates the insertion point for the node. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderDragDropMarker method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public override void DrawDragDropMarker(DragDropMarkerRendererEventArgs e)
        {
            m_DragDropMarkerDisplay.MarkerColor = this.ColorTable.DragDropMarker;
            m_DragDropMarkerDisplay.DrawMarker(e);
            base.DrawDragDropMarker(e);
        }

        /// <summary>
        /// Draws the column header. If you need to provide custom rendering this is the method that you should override in your custom renderer. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderColumnHeader method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public override void DrawColumnHeader(ColumnHeaderRendererEventArgs e)
        {
            ElementStyleDisplayInfo di = GetElementStyleDisplayInfo(e.Style, e.Graphics, e.Bounds);
            m_ColumnHeaderDisplay.DrawColumnHeader(e, di);
            base.DrawColumnHeader(e);
        }

        /// <summary>
        /// Draws node group line when in tile view. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderTileGroupLine method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public override void DrawTileGroupLine(NodeRendererEventArgs e)
        {
            _GroupLineDisplay.DrawGroupLine(e);
            OnRenderTileGroupLine(e);
        }
		#endregion
	}
}
