using System;
using System.Drawing;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents default system node and cell renderer.
	/// </summary>
	public class NodeSystemRenderer:NodeRenderer
	{
		#region Private Variables
		private NodeExpandEllipseDisplay m_NodeExpandEllipseDisplay=new NodeExpandEllipseDisplay();
		private NodeExpandRectDisplay m_NodeExpandRectDisplay=new NodeExpandRectDisplay();
		private NodeExpandImageDisplay m_NodeExpandImageDisplay=new NodeExpandImageDisplay();
		private NodeCommandDisplay m_NodeCommandDisplay=new NodeCommandDisplay();
		private ElementStyleDisplayInfo m_ElementStyleDisplayInfo=new ElementStyleDisplayInfo();
		private NodeSelectionDisplay m_SelectionDisplay=new NodeSelectionDisplay();
		private CurveConnectorDisplay m_CurveConnectorDisplay=null;
		private LineConnectorDisplay m_LineConnectorDisplay=null;
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
				case eNodeConnectorType.Curve:
				{
					if(m_CurveConnectorDisplay==null)
						m_CurveConnectorDisplay=new CurveConnectorDisplay();
					d=m_CurveConnectorDisplay;
					break;
				}
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
			ElementStyleDisplayInfo di=GetElementStyleDisplayInfo(e.Style,e.Graphics,DisplayHelp.GetDrawRectangle(e.NodeBounds));
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
			GetExpandDisplay(e.ExpandButtonType).DrawExpandButton(e);
			base.DrawNodeExpandPart(e);
		}
		
		private NodeExpandDisplay GetExpandDisplay(eExpandButtonType e)
		{
			NodeExpandDisplay d=null;
			switch(e)
			{
				case eExpandButtonType.Ellipse:
					d=m_NodeExpandEllipseDisplay;
					break;
				case eExpandButtonType.Rectangle:
					d=m_NodeExpandRectDisplay;
					break;
				case eExpandButtonType.Image:
					d= m_NodeExpandImageDisplay;
					break;
			}
			return d;
		}
		
		/// <summary>
		/// Draws node command part. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeCommandPart method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawNodeCommandPart(NodeCommandPartRendererEventArgs e)
		{
			m_NodeCommandDisplay.DrawCommandButton(e);
			base.DrawNodeCommandPart(e);
		}
		
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
			CellDisplay.PaintCellCheckBox(e);
			base.DrawCellCheckBox(e);
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
			m_SelectionDisplay.PaintSelection(e);
			base.DrawSelection(e);
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
			TreeGX tree = e.TreeGX;
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

		#endregion
	}
}
