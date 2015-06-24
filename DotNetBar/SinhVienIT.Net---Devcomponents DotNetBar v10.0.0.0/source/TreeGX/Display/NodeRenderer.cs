using System;
using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents abstract renderer class for node objects.
	/// </summary>
	public abstract class NodeRenderer
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
		/// <summary>
		/// Occurs when node command part is being drawn.
		/// </summary>
		public event NodeCommandPartRendererEventHandler RenderNodeCommandPart;
		/// <summary>
		/// Occurs when cell bacgkround is being drawn.
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
		/// Occurs when cell text is being drawn.
		/// </summary>
		public event SelectionRendererEventHandler RenderSelection;
		/// <summary>
		/// Occurs when node connector is being drawn.
		/// </summary>
		public event ConnectorRendererEventHandler RenderConnector;
		
		/// <summary>
		/// Occurs when tree background is rendered.
		/// </summary>
		public event TreeBackgroundRendererEventHandler RenderTreeBackground;
		#endregion
		
		#region Private Variables
		#endregion
		
		#region Constructor
		public NodeRenderer()
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
		
		/// <summary>
		/// Draws node command part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeCommandPart method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public virtual void DrawNodeCommandPart(NodeCommandPartRendererEventArgs e)
		{
			OnRenderNodeCommandPart(e);
		}
		
		/// <summary>
		/// Raises RenderNodeCommandPart event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected virtual void OnRenderNodeCommandPart(NodeCommandPartRendererEventArgs e)
		{
			if(RenderNodeCommandPart!=null)
				RenderNodeCommandPart(this,e);
		}
		
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
		#endregion
	}

}
