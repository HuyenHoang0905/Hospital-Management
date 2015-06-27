using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents class that provide display support for simple tabs.
	/// </summary>
	public class SimpleTabDisplay
	{
		#region Private Variables
		private int m_SideWidth=8;
		private int m_TextTopOffset=2;
		#endregion

		#region Internal Implementation
		/// <summary>
		/// Default constructor.
		/// </summary>
		public SimpleTabDisplay()
		{
		}

		public virtual void Paint(Graphics g, ISimpleTab[] tabs)
		{
			int iSelected=-1;
			for(int i=tabs.Length-1;i>=0;i--)
			{
				if(tabs[i].IsSelected)
				{
					iSelected=i;
					continue;
				}
				PaintTab(g,tabs[i]);
			}

			if(iSelected!=-1)
				PaintTab(g,tabs[iSelected]);
		}

		protected virtual void PaintTab(Graphics g, ISimpleTab tab)
		{
			TabColors c=GetTabColors(tab);
			Rectangle r=tab.DisplayRectangle;
			

			r.Width+=(m_SideWidth*2);
			r.X-=m_SideWidth;

			GraphicsPath path=this.GetTabPath(r,tab.TabAlignment);
			if(c.BackColor2.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(c.BackColor))
					g.FillPath(brush,path);
			}
			else
			{
				using(LinearGradientBrush brush=BarFunctions.CreateLinearGradientBrush(tab.DisplayRectangle,c.BackColor,c.BackColor2,c.BackColorGradientAngle))
					g.FillPath(brush,path);
			}
			r.Width--;
			r.Height--;
			//Rectangle r=tab.DisplayRectangle;
//			r.Width--;
//			r.Height--;
			if(!c.BorderColor.IsEmpty)
			{
				using(Pen pen=new Pen(c.BorderColor,1))
				{
					Line l=GetLeftLine(r,tab.TabAlignment);
					g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					l=GetRightLine(r,tab.TabAlignment);
					g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					if(tab.TabAlignment==eTabStripAlignment.Top)
					{
						l=GetTopLine(r,tab.TabAlignment);
						g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					}
					else if(tab.TabAlignment==eTabStripAlignment.Bottom)
					{
						l=GetBottomLine(r,tab.TabAlignment);
						g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					}
				}
			}
			
			if(!c.LightBorderColor.IsEmpty)
			{
				using(Pen pen=new Pen(c.LightBorderColor,1))
				{
					Line l=GetLeftLine(r,tab.TabAlignment);
					g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					if(tab.TabAlignment==eTabStripAlignment.Top)
					{
						l=GetTopLine(r,tab.TabAlignment);
						g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					}
					else if(tab.TabAlignment==eTabStripAlignment.Bottom)
					{
						l=GetBottomLine(r,tab.TabAlignment);
						g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
					}
				}
			}

			if(!c.DarkBorderColor.IsEmpty)
			{
				using(Pen pen=new Pen(c.DarkBorderColor,1))
				{
					Line l=GetRightLine(r,tab.TabAlignment);
					g.DrawLine(pen,l.X1,l.Y1,l.X2,l.Y2);
				}
			}
            
			DrawTabText(g,tab,GetTextRectangle(tab),c);

//			if(tab is BubbleBarTab && ((BubbleBarTab)tab).Focus)
//			{
//				Region clip=g.Clip;
//				g.ResetClip();
//				r=tab.DisplayRectangle;
//				r.Width+=(m_SideWidth*2);
//				r.X-=m_SideWidth;
//				r.Inflate(1,1);
//				DesignTime.DrawDesignTimeSelection(g,r,Color.Navy);
//				g.Clip=clip;
//			}
		}

		private TabColors GetTabColors(ISimpleTab tab)
		{
			TabColors c=new TabColors();
			c.BackColor=tab.BackColor;
			c.BackColor2=tab.BackColor2;
			c.BackColorGradientAngle=tab.BackColorGradientAngle;
			c.BorderColor=tab.BorderColor;
			c.DarkBorderColor=tab.DarkBorderColor;
			c.LightBorderColor=tab.LightBorderColor;
			c.TextColor=tab.TextColor;

			if(tab.IsSelected)
			{
				if(tab is BubbleBarTab && ((BubbleBarTab)tab).Parent!=null)
				{
					TabColors sel=((BubbleBarTab)tab).Parent.SelectedTabColors;
					if(!sel.BackColor.IsEmpty)
						c.BackColor=sel.BackColor;
					if(!sel.BackColor2.IsEmpty)
					{
						c.BackColor2=sel.BackColor2;
						c.BackColorGradientAngle=sel.BackColorGradientAngle;
					}
					if(!sel.BorderColor.IsEmpty)
						c.BorderColor=sel.BorderColor;
					if(!sel.DarkBorderColor.IsEmpty)
						c.DarkBorderColor=sel.DarkBorderColor;
					if(!sel.LightBorderColor.IsEmpty)
						c.LightBorderColor=sel.LightBorderColor;
					if(!sel.TextColor.IsEmpty)
						c.TextColor=sel.TextColor;
				}
			}
			
			if(tab.IsMouseOver)
			{
				if(tab is BubbleBarTab && ((BubbleBarTab)tab).Parent!=null)
				{
					TabColors sel=((BubbleBarTab)tab).Parent.MouseOverTabColors;
					if(!sel.BackColor.IsEmpty)
						c.BackColor=sel.BackColor;
					if(!sel.BackColor2.IsEmpty)
					{
						c.BackColor2=sel.BackColor2;
						c.BackColorGradientAngle=sel.BackColorGradientAngle;
					}
					if(!sel.BorderColor.IsEmpty)
						c.BorderColor=sel.BorderColor;
					if(!sel.DarkBorderColor.IsEmpty)
						c.DarkBorderColor=sel.DarkBorderColor;
					if(!sel.LightBorderColor.IsEmpty)
						c.LightBorderColor=sel.LightBorderColor;
					if(!sel.TextColor.IsEmpty)
						c.TextColor=sel.TextColor;
				}
			}

			return c;
		}

		protected virtual Rectangle GetTextRectangle(ISimpleTab tab)
		{
			Rectangle rText=tab.DisplayRectangle;
			rText.Y+=m_TextTopOffset;
			rText.Height-=m_TextTopOffset;
			//rText.X+=m_SideWidth/2;
			//rText.Width-=m_SideWidth;
			return rText;
		}

		protected virtual GraphicsPath GetTabPath(Rectangle r,eTabStripAlignment align)
		{
			GraphicsPath path=new GraphicsPath();
			Line l=GetLeftLine(r,align);
			path.AddLine(l.X1,l.Y1,l.X2,l.Y2);
			l=GetTopLine(r,align);
			path.AddLine(l.X1,l.Y1,l.X2,l.Y2);
			l=GetRightLine(r,align);
			path.AddLine(l.X1,l.Y1,l.X2,l.Y2);
			l=GetBottomLine(r,align);
			path.AddLine(l.X1,l.Y1,l.X2,l.Y2);
			path.CloseAllFigures();
			return path;
		}

		protected virtual Line GetLeftLine(Rectangle r,eTabStripAlignment align)
		{
			if(align==eTabStripAlignment.Top)
				return new Line(r.X,r.Bottom,r.X+m_SideWidth,r.Y);
			else if(align==eTabStripAlignment.Bottom)
				return new Line(r.X+m_SideWidth,r.Bottom,r.X,r.Y);
			return null;
		}

		protected virtual Line GetRightLine(Rectangle r,eTabStripAlignment align)
		{
			if(align==eTabStripAlignment.Top)
				return new Line(r.Right-m_SideWidth,r.Y,r.Right,r.Bottom);
			else if(align==eTabStripAlignment.Bottom)
				return new Line(r.Right,r.Y,r.Right-m_SideWidth,r.Bottom);
			return null;
		}

		protected virtual Line GetTopLine(Rectangle r,eTabStripAlignment align)
		{
			if(align==eTabStripAlignment.Top)
				return new Line(r.X+m_SideWidth,r.Y,r.Right-m_SideWidth,r.Y);
			else if(align==eTabStripAlignment.Bottom)
				return new Line(r.X,r.Y,r.Right,r.Y);
			return null;
		}

		protected virtual Line GetBottomLine(Rectangle r,eTabStripAlignment align)
		{
			if(align==eTabStripAlignment.Top)
				return new Line(r.X,r.Bottom,r.Right,r.Bottom);
			else if(align==eTabStripAlignment.Bottom)
				return new Line(r.Right-m_SideWidth,r.Bottom,r.X+m_SideWidth,r.Bottom);
			return null;
		}

		protected virtual void DrawTabText(Graphics g, ISimpleTab tab, Rectangle rText, TabColors c)
		{
			eTextFormat strFormat=GetStringFormat();

			Font font=tab.GetTabFont();
			
			if(tab.TabAlignment==eTabStripAlignment.Left || tab.TabAlignment==eTabStripAlignment.Right)
			{
				g.RotateTransform(90);
				rText=new Rectangle(rText.Top,-rText.Right,rText.Height,rText.Width);
			}

            if (tab.TabAlignment == eTabStripAlignment.Left || tab.TabAlignment == eTabStripAlignment.Right)
                TextDrawing.DrawStringLegacy(g, tab.Text, font, c.TextColor, rText, strFormat);
            else
		        TextDrawing.DrawString(g,tab.Text,font,c.TextColor,rText,strFormat);

			if(tab.TabAlignment==eTabStripAlignment.Left || tab.TabAlignment==eTabStripAlignment.Right)
				g.ResetTransform();
		}

		protected virtual eTextFormat GetStringFormat()
		{
            return eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter |
                eTextFormat.SingleLine | eTextFormat.EndEllipsis;

            //StringFormat strFormat=BarFunctions.CreateStringFormat(); //new StringFormat(StringFormat.GenericDefault);
            //strFormat.FormatFlags=strFormat.FormatFlags | StringFormatFlags.NoWrap;
            //strFormat.Trimming=StringTrimming.EllipsisCharacter;
            //strFormat.LineAlignment=StringAlignment.Center;
            //strFormat.Alignment=StringAlignment.Center;
            //strFormat.HotkeyPrefix=HotkeyPrefix.Show;
            //return strFormat;
		}
		#endregion

		#region Line class
		protected class Line
		{
			public int X1=0;
			public int Y1=0;
			public int X2=0;
			public int Y2=0;
			public Line(int x1, int y1, int x2, int y2)
			{
				this.X1=x1;
				this.Y1=y1;
				this.X2=x2;
				this.Y2=y2;
			}
		}
		#endregion
	}
}
