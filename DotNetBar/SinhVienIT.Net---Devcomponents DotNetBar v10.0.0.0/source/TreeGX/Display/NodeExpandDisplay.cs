using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree
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
				return new Pen(e.BorderColor,1);
			}

			protected Pen GetExpandPen(NodeExpandPartRendererEventArgs e)
			{
				return new Pen(e.ExpandLineColor,1);
			}

			protected Brush GetBackgroundBrush(NodeExpandPartRendererEventArgs e)
			{
				if(e.BackColor.IsEmpty && e.BackColor2.IsEmpty)
					return null;

				if(e.BackColor2.IsEmpty)
					return new SolidBrush(e.BackColor);
			
				System.Drawing.Drawing2D.LinearGradientBrush brush=DisplayHelp.CreateLinearGradientBrush(e.ExpandPartBounds,e.BackColor,e.BackColor2,e.BackColorGradientAngle);
				//brush.SetSigmaBellShape(0.8f);
				return brush;
			}
		
		}
	}
}
