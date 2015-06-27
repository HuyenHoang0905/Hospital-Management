using System;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display
{
	/// <summary>
	/// Represents abstract renderer class for node objects.
	/// </summary>
	public abstract class TreeRenderer
	{
		#region Events
		/// <summary>
		/// Occurs when node background is being drawn.
		/// </summary>
		public event NodeRendererEventHandler RenderNodeBackground;
		/// <summary>
		/// Occurs when node expand part is being drawn.
		/// </summary>
		public event NodeExpandPartRendererEventHandler RenderNodeExpandPart;
        ///// <summary>
        ///// Occurs when node command part is being drawn.
        ///// </summary>
        //public event NodeCommandPartRendererEventHandler RenderNodeCommandPart;
		/// <summary>
		/// Occurs when cell background is being drawn.
		/// </summary>
		public event NodeCellRendererEventHandler RenderCellBackground;
		/// <summary>
		/// Occurs when cell check-box is being drawn.
		/// </summary>
		public event NodeCellRendererEventHandler RenderCellCheckBox;
		/// <summary>
		/// Occurs when cell image is being drawn.
		/// </summary>
		public event NodeCellRendererEventHandler RenderCellImage;
		/// <summary>
		/// Occurs when cell text is being drawn.
		/// </summary>
		public event NodeCellRendererEventHandler RenderCellText;
		/// <summary>
		/// Occurs when node selection marker is rendered.
		/// </summary>
		public event SelectionRendererEventHandler RenderSelection;
        /// <summary>
        /// Occurs when node hot-tracking marker is rendered.
        /// </summary>
        public event SelectionRendererEventHandler RenderHotTracking;
		/// <summary>
		/// Occurs when node connector is being drawn.
		/// </summary>
		public event ConnectorRendererEventHandler RenderConnector;
		
		/// <summary>
		/// Occurs when tree background is rendered.
		/// </summary>
		public event TreeBackgroundRendererEventHandler RenderTreeBackground;

        /// <summary>
        /// Occurs when drag & drop marker is rendered.
        /// </summary>
        public event DragDropMarkerRendererEventHandler RenderDragDropMarker;
        /// <summary>
        /// Renders the Column Header.
        /// </summary>
        public event ColumnHeaderRendererEventHandler RenderColumnHeader;
        /// <summary>
        /// Occurs when node group line is being rendered while control is in tile view.
        /// </summary>
        public event NodeRendererEventHandler RenderTileGroupLine;
		#endregion
		
		#region Private Variables
		#endregion
		
		#region Constructor
		public TreeRenderer()
		{
		}
		#endregion
		
		#region Internal Implementation
		/// <summary>
		/// Draws node background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawNodeBackground(NodeRendererEventArgs e)
		{
			OnRenderNodeBackground(e);
		}
		
		/// <summary>
		/// Raises RenderNodeBackground event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnRenderNodeBackground(NodeRendererEventArgs e)
		{
			if(RenderNodeBackground!=null)
				RenderNodeBackground(this,e);
		}
		
		/// <summary>
		/// Draws node expand part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeExpandPart method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawNodeExpandPart(NodeExpandPartRendererEventArgs e)
		{
			OnRenderNodeExpandPart(e);
		}
		
		/// <summary>
		/// Raises RenderNodeExpandPart event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRenderNodeExpandPart(NodeExpandPartRendererEventArgs e)
		{
			if(RenderNodeExpandPart!=null)
				RenderNodeExpandPart(this,e);
		}
		
        ///// <summary>
        ///// Draws node command part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        ///// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeCommandPart method so events can occur.
        ///// </summary>
        ///// <param name="e">Information provided for rendering.</param>
        //public virtual void DrawNodeCommandPart(NodeCommandPartRendererEventArgs e)
        //{
        //    OnRenderNodeCommandPart(e);
        //}
		
        ///// <summary>
        ///// Raises RenderNodeCommandPart event.
        ///// </summary>
        ///// <param name="e">Event arguments.</param>
        //protected virtual void OnRenderNodeCommandPart(NodeCommandPartRendererEventArgs e)
        //{
        //    if(RenderNodeCommandPart!=null)
        //        RenderNodeCommandPart(this,e);
        //}
		
		/// <summary>
		/// Draws cell background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawCellBackground(NodeCellRendererEventArgs e)
		{
			OnRenderCellBackground(e);
		}
		
		/// <summary>
		/// Raises RenderCellBackground event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnRenderCellBackground(NodeCellRendererEventArgs e)
		{
			if(RenderCellBackground!=null)
				RenderCellBackground(this, e);
		}
		
		/// <summary>
		/// Draws cell check box.  If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellCheckBox method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawCellCheckBox(NodeCellRendererEventArgs e)
		{
			OnRenderCellCheckBox(e);
		}
		
		/// <summary>
		/// Raises RenderCellCheckBox event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnRenderCellCheckBox(NodeCellRendererEventArgs e)
		{
			if(RenderCellCheckBox!=null)
				RenderCellCheckBox(this, e);
		}
		
		/// <summary>
		/// Draws cell image. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellImage method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawCellImage(NodeCellRendererEventArgs e)
		{
			OnRenderCellImage(e);
		}
		
		/// <summary>
		/// Raises RenderCellImage event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnRenderCellImage(NodeCellRendererEventArgs e)
		{
			if(RenderCellImage!=null)
				RenderCellImage(this, e);
		}
		
		/// <summary>
		/// Draws cell text. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderCellText method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawCellText(NodeCellRendererEventArgs e)
		{
			OnRenderCellText(e);
		}
		
		/// <summary>
		/// Raises RenderCellImage event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected virtual void OnRenderCellText(NodeCellRendererEventArgs e)
		{
			if(RenderCellText!=null)
				RenderCellText(this, e);
		}
		
		/// <summary>
		/// Draws selection for SelectedNode. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderSelection method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawSelection(SelectionRendererEventArgs e)
		{
			OnRenderSelection(e);
		}

		/// <summary>
		/// Raises RenderSelection event.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnRenderSelection(SelectionRendererEventArgs e)
		{
			if(RenderSelection!=null)
				RenderSelection(this, e);
		}

        /// <summary>
        /// Draws hot-tracking marker for mouse over node. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderHotTracking method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public virtual void DrawHotTracking(SelectionRendererEventArgs e)
        {
            OnRenderHotTracking(e);
        }

        /// <summary>
        /// Raises RenderHotTracking event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnRenderHotTracking(SelectionRendererEventArgs e)
        {
            if (RenderHotTracking != null)
                RenderHotTracking(this, e);
        }
		
		/// <summary>
		/// Draws connector between nodes. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderConnector method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawConnector(ConnectorRendererEventArgs e)
		{
			OnRenderConnector(e);
		}

		/// <summary>
		/// Raises RenderConnector event.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnRenderConnector(ConnectorRendererEventArgs e)
		{
			if(RenderConnector!=null)
				RenderConnector(this, e);
		}

		/// <summary>
		/// Draws the tree background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderTreeBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawTreeBackground(TreeBackgroundRendererEventArgs e)
		{
			OnRenderTreeBackground(e);
		}

		/// <summary>
		/// Raises RenderTreeBackground event.
		/// </summary>
		/// <param name="e">Event data.</param>
		protected virtual void OnRenderTreeBackground(TreeBackgroundRendererEventArgs e)
		{
			if(RenderTreeBackground!=null)
				RenderTreeBackground(this, e);
		}

        /// <summary>
        /// Draws the drag &amp; drop marker that indicates the insertion point for the node. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderDragDropMarker method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
        public virtual void DrawDragDropMarker(DragDropMarkerRendererEventArgs e)
		{
            OnRenderDragDropMarker(e);
		}

		/// <summary>
		/// Raises RenderDragDropMarker event.
		/// </summary>
		/// <param name="e">Event data.</param>
        protected virtual void OnRenderDragDropMarker(DragDropMarkerRendererEventArgs e)
		{
            if (RenderDragDropMarker != null)
                RenderDragDropMarker(this, e);
		}

        /// <summary>
        /// Draws the column header. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderColumnHeader method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public virtual void DrawColumnHeader(ColumnHeaderRendererEventArgs e)
        {
            OnRenderColumnHeader(e);
        }

        /// <summary>
        /// Raises RenderDragDropMarker event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnRenderColumnHeader(ColumnHeaderRendererEventArgs e)
        {
            if (RenderColumnHeader != null)
                RenderColumnHeader(this, e);
        }

        private TreeColorTable _ColorTable = null;
        /// <summary>
        /// Gets or sets the color table used by the renderer.
        /// </summary>
        public TreeColorTable ColorTable
        {
            get { return _ColorTable; }
            set { _ColorTable = value; }
        }

        private Office2007ColorTable _Office2007ColorTable = null;
        /// <summary>
        /// Gets or sets the color table used by the renderer.
        /// </summary>
        internal Office2007ColorTable Office2007ColorTable
        {
            get { return _Office2007ColorTable; }
            set { _Office2007ColorTable = value; }
        }

        /// <summary>
        /// Draws node group line when in tile view. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
        /// do not want default rendering to occur do not call the base implementation. You can call OnRenderTileGroupLine method so events can occur.
        /// </summary>
        /// <param name="e">Information provided for rendering.</param>
        public virtual void DrawTileGroupLine(NodeRendererEventArgs e)
        {
            OnRenderTileGroupLine(e);
        }

        /// <summary>
        /// Raises RenderNodeBackground event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnRenderTileGroupLine(NodeRendererEventArgs e)
        {
            if (RenderTileGroupLine != null)
                RenderTileGroupLine(this, e);
        }
		
		#endregion
	}

}
