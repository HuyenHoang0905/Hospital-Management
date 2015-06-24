using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.Tree.Display
{
	/// <summary>
	/// Paints node command button.
	/// </summary>
	internal class NodeCommandDisplay
	{
		public virtual void DrawCommandButton(NodeCommandPartRendererEventArgs info)
		{
			bool mouseOver = (info.Node.MouseOverNodePart == eMouseOverNodePart.Command);
			CommandColors c=new CommandColors();
			if(mouseOver)
			{
				c.BackColor = info.MouseOverBackColor;
				c.BackColor2 = info.MouseOverBackColor2;
				c.BackColorGradientAngle = info.MouseOverBackColorGradientAngle;
				c.ForeColor = info.MouseOverForeColor;
			}
			else
			{
				c.BackColor = info.BackColor;
				c.BackColor2 = info.BackColor2;
				c.BackColorGradientAngle = info.BackColorGradientAngle;
				c.ForeColor = info.ForeColor;
			}
			
			Rectangle fillBounds = info.CommandPartBounds;
			fillBounds.Width--;
			fillBounds.Height--;
			
			Region oldRegion = info.Graphics.Clip.Clone() as Region;
			info.Graphics.SetClip(fillBounds,CombineMode.Intersect);
			
			if(c.BackColor2.IsEmpty)
			{
				if(!c.BackColor.IsEmpty)
				{
					using(SolidBrush brush=new SolidBrush(c.BackColor))
						info.Graphics.FillRectangle(brush,fillBounds);
				}
			}
			else
			{
				using(LinearGradientBrush brush=DisplayHelp.CreateLinearGradientBrush(info.CommandPartBounds, c.BackColor, c.BackColor2, c.BackColorGradientAngle))
					info.Graphics.FillRectangle(brush,fillBounds);
			}

			if(c.ForeColor.IsEmpty) return;

			int width=6;
			int height=3;
			GraphicsPath path=new GraphicsPath();
			Point p=new Point(info.CommandPartBounds.X+(info.CommandPartBounds.Width-width)/2,info.CommandPartBounds.Y+(info.CommandPartBounds.Height-height)/2);
			path.AddLine(p.X, p.Y, p.X+width,p.Y);
			path.AddLine(p.X+width,p.Y,p.X+width/2,p.Y+height);
			path.AddLine(p.X, p.Y,p.X+width/2,p.Y+height);
			path.CloseAllFigures();
			
			using(SolidBrush brush=new SolidBrush(c.ForeColor))
				info.Graphics.FillPath(brush,path);

			path.Dispose();
			
			info.Graphics.Clip = oldRegion;
		}

		private class CommandColors
		{
			public Color BackColor=Color.Empty;
			public Color BackColor2=Color.Empty;
			public Color ForeColor=Color.Empty;
			public int BackColorGradientAngle=0;
		}
	}
}
