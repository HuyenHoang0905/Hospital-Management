using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree
{
	namespace Display
	{
		/// <summary>
		/// Base class for node expand button display.
		/// </summary>
		public abstract class NodeExpandDisplay
		{
			/// <summary>Creates new instance of the class</summary>
			public NodeExpandDisplay()
			{
			}

			/// <summary>Draws expand button.</summary>
			/// <param name="e">Context parameters for drawing expand button.</param>
			public abstract void DrawExpandButton(NodeExpandPartRendererEventArgs e);

			protected Pen GetBorderPen(NodeExpandPartRendererEventArgs e)
			{
                if(!e.BorderColor.IsEmpty)
                    return new Pen(e.BorderColor,1);
                if (_ColorTable != null)
                {
                    bool expanded = e.Node.Expanded;
                    TreeExpandColorTable ct = GetExpandColorTable(e);
                    if (ct == null) return null;
                    if (expanded)
                    {
                        // Collapse node colors
                        if (!e.IsMouseOver && ct.CollapseBorder != null)
                            return ct.CollapseBorder.CreatePen();
                        else if (e.IsMouseOver && ct.CollapseMouseOverBorder != null)
                            return ct.CollapseMouseOverBorder.CreatePen();
                    }
                    else
                    {
                        // Expand node colors
                        if (!e.IsMouseOver && ct.ExpandBorder != null)
                            return ct.ExpandBorder.CreatePen();
                        else if (e.IsMouseOver && ct.ExpandMouseOverBorder != null)
                            return ct.ExpandMouseOverBorder.CreatePen();
                    }
                }
                return null;
			}

            private TreeExpandColorTable GetExpandColorTable(NodeExpandPartRendererEventArgs e)
            {
                TreeExpandColorTable ct = null;
                if (e.ExpandButtonType == eExpandButtonType.Rectangle)
                    ct = _ColorTable.ExpandRectangle;
                else if (e.ExpandButtonType == eExpandButtonType.Triangle)
                    ct = _ColorTable.ExpandTriangle;
                else if (e.ExpandButtonType == eExpandButtonType.Ellipse)
                    ct = _ColorTable.ExpandEllipse;
                return ct;
            }

			protected Pen GetExpandPen(NodeExpandPartRendererEventArgs e)
			{
                if (e.ExpandLineColor.IsEmpty)
                {
                    TreeExpandColorTable ct = GetExpandColorTable(e);
                    if (ct != null)
                    {
                        bool expanded = e.Node.Expanded;
                        if (expanded)
                        {
                            // Collapse node colors
                            if (!e.IsMouseOver && ct.CollapseForeground != null)
                                return ct.CollapseForeground.CreatePen(1);
                            else if (e.IsMouseOver && ct.CollapseMouseOverForeground != null)
                                return ct.CollapseMouseOverForeground.CreatePen(1);
                        }
                        else
                        {
                            // Collapse node colors
                            if (!e.IsMouseOver && ct.ExpandForeground != null)
                                return ct.ExpandForeground.CreatePen(1);
                            else if (e.IsMouseOver && ct.ExpandMouseOverForeground != null)
                                return ct.ExpandMouseOverForeground.CreatePen(1);
                        }
                    }

                    return GetBorderPen(e);
                }

				return new Pen(e.ExpandLineColor,1);
			}

			protected Brush GetBackgroundBrush(NodeExpandPartRendererEventArgs e)
			{
                if (e.BackColor.IsEmpty && e.BackColor2.IsEmpty)
                {
                    bool expanded = e.Node.Expanded;
                    TreeExpandColorTable ct = GetExpandColorTable(e);
                    if (ct == null) return null;
                    if (expanded)
                    {
                        // Collapse node colors
                        if (!e.IsMouseOver && ct.CollapseFill != null)
                            return ct.CollapseFill.CreateBrush(e.ExpandPartBounds);
                        else if (e.IsMouseOver && ct.CollapseMouseOverFill != null)
                            return ct.CollapseMouseOverFill.CreateBrush(e.ExpandPartBounds);
                    }
                    else
                    {
                        // Expand node colors
                        if (!e.IsMouseOver && ct.ExpandFill != null)
                            return ct.ExpandFill.CreateBrush(e.ExpandPartBounds);
                        else if (e.IsMouseOver && ct.ExpandMouseOverFill != null)
                            return ct.ExpandMouseOverFill.CreateBrush(e.ExpandPartBounds);
                    }

                    return null;
                }

				if(e.BackColor2.IsEmpty)
					return new SolidBrush(e.BackColor);
			
				System.Drawing.Drawing2D.LinearGradientBrush brush=DisplayHelp.CreateLinearGradientBrush(e.ExpandPartBounds,e.BackColor,e.BackColor2,e.BackColorGradientAngle);
				//brush.SetSigmaBellShape(0.8f);
				return brush;
			}

            private TreeColorTable _ColorTable;
            public TreeColorTable ColorTable
            {
                get { return _ColorTable; }
                set { _ColorTable = value; }
            }
		}
	}
}
