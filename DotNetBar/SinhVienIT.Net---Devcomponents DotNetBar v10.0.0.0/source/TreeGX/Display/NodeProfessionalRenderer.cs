using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Represents professional rendered which provides extensive rendering above and beyond the NodeSystemRenderer.
	/// </summary>
	public class NodeProfessionalRenderer : NodeSystemRenderer
	{
		#region Private Variables
		private NodeProfessionalColorTable m_ColorTable=new NodeProfessionalColorTable();
		#endregion
		
		/// <summary>
		/// Gets or sets the color table used by renderer.
		/// </summary>
		public NodeProfessionalColorTable ColorTable
		{
			get { return m_ColorTable;}
			set
			{
				m_ColorTable = value;
			}
		}
		/// <summary>
		/// Draws node background. If you need to provide custom rendering this is the method that you should override in your custom rendered. If you
		/// do not want default rendering to occur do not call the base implementation. You can call OnRenderNodeBackground method so events can occur.
		/// </summary>
		/// <param name="e">Information provided for rendering.</param>
		public override void DrawNodeBackground(NodeRendererEventArgs e)
		{
			ElementStyleDisplayInfo di=GetElementStyleDisplayInfo(e.Style,e.Graphics,DisplayHelp.GetDrawRectangle(e.NodeBounds));
			PaintBackground(e);
			ElementStyleDisplay.PaintBackgroundImage(di);
			ElementStyleDisplay.PaintBorder(di);
			
			// Let events occur
			OnRenderNodeBackground(e);
		}
		
		private void PaintBackgroundPart(Graphics g, Rectangle bounds, GraphicsPath path, Color color1, Color color2, eGradientType gradientType, int gradientAngle)
		{
			if(color2.IsEmpty)
			{
				if(!color1.IsEmpty)
				{
					using(SolidBrush brush=new SolidBrush(color1))
						g.FillPath(brush,path);
				}
			}
			else if(!m_ColorTable.NodeTopGradientBegin.IsEmpty)
			{
				if (gradientType == eGradientType.Linear)
				{
					Rectangle rb = bounds;
					rb.X--;
					rb.Height++;
					rb.Width += 2;
					using (LinearGradientBrush brush = DisplayHelp.CreateLinearGradientBrush(rb, color1, color2, gradientAngle))
					{
						g.FillPath(brush, path);
					}
				}
				else if (gradientType == eGradientType.Radial)
				{
					int d = (int)Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
					GraphicsPath fillPath = new GraphicsPath();
					fillPath.AddEllipse(bounds.X - (d - bounds.Width), bounds.Y - (d - bounds.Height) / 2, d, d);
					using (PathGradientBrush brush = new PathGradientBrush(fillPath))
					{
						brush.CenterColor = color1;
						brush.SurroundColors = new Color[] { color2 };
						g.FillPath(brush, path);
					}
					fillPath.Dispose();
				}
			}
		}
		
		/// <summary>
		/// Paints background of the node.
		/// </summary>
		/// <param name="e">Context information.</param>
		protected virtual void PaintBackground(NodeRendererEventArgs e)
		{
			Graphics g = e.Graphics;
			bool mouseOver = e.Node.IsMouseOver;
			
			Region oldClip = g.Clip;
			Rectangle clipRect=e.NodeBounds;
			clipRect.Width--;
			clipRect.Height--;
			g.SetClip(clipRect, CombineMode.Replace);
			
			// Prepare colors
			NodeColors colors=new NodeColors();
			colors.NodeTopGradientBegin = m_ColorTable.NodeTopGradientBegin;
			colors.NodeTopGradientMiddle = m_ColorTable.NodeTopGradientMiddle;
			colors.NodeTopGradientEnd = m_ColorTable.NodeTopGradientEnd;
			colors.NodeTopGradientType = m_ColorTable.NodeTopGradientType;
			colors.NodeTopGradientAngle = m_ColorTable.NodeTopGradientAngle;
			colors.NodeBottomGradientBegin = m_ColorTable.NodeBottomGradientBegin;
			colors.NodeBottomGradientMiddle = m_ColorTable.NodeBottomGradientMiddle;
			colors.NodeBottomGradientEnd = m_ColorTable.NodeBottomGradientEnd;
			colors.NodeBottomGradientType = m_ColorTable.NodeBottomGradientType;
			colors.NodeBottomGradientAngle = m_ColorTable.NodeBottomGradientAngle;
			if(mouseOver)
			{
				colors.NodeTopGradientBegin = m_ColorTable.NodeMouseOverTopGradientBegin;
				colors.NodeTopGradientMiddle = m_ColorTable.NodeMouseOverTopGradientMiddle;
				colors.NodeTopGradientEnd = m_ColorTable.NodeMouseOverTopGradientEnd;
				colors.NodeTopGradientType = m_ColorTable.NodeMouseOverTopGradientType;
				colors.NodeTopGradientAngle = m_ColorTable.NodeMouseOverTopGradientAngle;
				colors.NodeBottomGradientBegin = m_ColorTable.NodeMouseOverBottomGradientBegin;
				colors.NodeBottomGradientMiddle = m_ColorTable.NodeMouseOverBottomGradientMiddle;
				colors.NodeBottomGradientEnd = m_ColorTable.NodeMouseOverBottomGradientEnd;
				colors.NodeBottomGradientType = m_ColorTable.NodeMouseOverBottomGradientType;
				colors.NodeBottomGradientAngle = m_ColorTable.NodeMouseOverBottomGradientAngle;
			}
			
			// Paint Background, Top Part
			Rectangle bounds=DisplayHelp.GetDrawRectangle(ElementStyleDisplay.GetBackgroundRectangle(e.Style,e.NodeBounds));
			GraphicsPath path;
			if (g.SmoothingMode == SmoothingMode.AntiAlias)
			{
				Rectangle r = bounds;
				r.Width--;
				path = ElementStyleDisplay.GetBackgroundPath(e.Style, r, eStyleBackgroundPathPart.TopHalf);
			}
			else
				path = ElementStyleDisplay.GetBackgroundPath(e.Style, bounds, eStyleBackgroundPathPart.TopHalf);
			path.CloseAllFigures();
			Rectangle backRect = bounds;
			backRect.Height = backRect.Height/2;
			PaintBackgroundPart(g, backRect, path, colors.NodeTopGradientBegin, colors.NodeTopGradientEnd,
			                    colors.NodeTopGradientType, colors.NodeTopGradientAngle);
			
			Rectangle ellipse = new Rectangle(bounds.X, bounds.Y - bounds.Height / 2, bounds.Width, bounds.Height);
			GraphicsPath pathFill=new GraphicsPath();
			pathFill.AddEllipse(ellipse);
			PathGradientBrush pathBrush = new PathGradientBrush(pathFill);
			pathBrush.CenterColor = colors.NodeTopGradientMiddle;
			pathBrush.SurroundColors = new Color[] { Color.Transparent };
			pathBrush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, bounds.Y);
			Blend blend = new Blend();
			blend.Factors = new float[] { 0f, .8f, 1f };
			blend.Positions = new float[] { .0f, .8f, 1f };
			pathBrush.Blend = blend;
			pathFill.Dispose();
			g.FillPath(pathBrush, path);
			pathBrush.Dispose();
			path.Dispose();
			
			// Bottom Part
			if (g.SmoothingMode == SmoothingMode.AntiAlias)
			{
				Rectangle r = bounds;
				r.Width--;
				path = ElementStyleDisplay.GetBackgroundPath(e.Style, r, eStyleBackgroundPathPart.BottomHalf);
			}
			else
				path = ElementStyleDisplay.GetBackgroundPath(e.Style, bounds, eStyleBackgroundPathPart.BottomHalf);
			path.CloseAllFigures();
			
			backRect.Y += backRect.Height;
			PaintBackgroundPart(g, backRect, path, colors.NodeBottomGradientBegin, colors.NodeBottomGradientEnd,
				colors.NodeBottomGradientType, colors.NodeBottomGradientAngle);
			
			ellipse = new Rectangle(bounds.X, bounds.Y + bounds.Height / 2 - 2, bounds.Width, bounds.Height + 4);
			pathFill=new GraphicsPath();
			pathFill.AddEllipse(ellipse);
			pathBrush = new PathGradientBrush(pathFill);
			pathBrush.CenterColor = colors.NodeBottomGradientMiddle;
			pathBrush.SurroundColors = new Color[] { Color.Transparent };
			pathBrush.CenterPoint = new PointF(ellipse.X + ellipse.Width / 2, bounds.Bottom);
			blend = new Blend();
			blend.Factors = new float[] { 0f, .5f, 1f };
			blend.Positions = new float[] { .0f, .4f, 1f };
			pathBrush.Blend = blend;
			//path.Dispose();

			g.FillPath(pathBrush, path);
			pathBrush.Dispose();
			pathFill.Dispose();
			path.Dispose();
			
			if (oldClip != null)
				g.Clip = oldClip;
			else
				g.ResetClip();
			
		}
		
		private class NodeColors
		{
			public Color NodeTopGradientBegin;
			public Color NodeTopGradientMiddle;
			public Color NodeTopGradientEnd;
			public eGradientType NodeTopGradientType=eGradientType.Linear;
			public int NodeTopGradientAngle=90;
		
			public Color NodeBottomGradientBegin;
			public Color NodeBottomGradientMiddle;
			public Color NodeBottomGradientEnd;
			public eGradientType NodeBottomGradientType=eGradientType.Linear;
			public int NodeBottomGradientAngle=90;
		}
		
	}
}
