using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.ColorPickerItem
{
    [ToolboxItem(false)]
	internal class ColorSelectionPreviewControl : System.Windows.Forms.UserControl
	{
		#region Private Variables
		private Color m_CurrentColor = Color.Black;
		private Color m_NewColor = Color.Empty;
		#endregion
		
		#region Constructor
		public ColorSelectionPreviewControl()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}
		#endregion
		
		#region Internal Implementation
		protected override void OnPaint(PaintEventArgs e)
		{
			if(this.BackColor == Color.Transparent)
			{
				base.OnPaintBackground(e);
			}

			Graphics g = e.Graphics;
			
			Rectangle r = this.ClientRectangle;
			
			g.FillRectangle(SystemBrushes.Control, r);
			
			if(!m_NewColor.IsEmpty)
			{
				using(SolidBrush brush = new SolidBrush(m_NewColor))
					g.FillRectangle(brush, r.X, r.Y, r.Width, r.Height / 2);
			}
			
			if(!m_CurrentColor.IsEmpty)
			{
				using(SolidBrush brush = new SolidBrush(m_CurrentColor))
					g.FillRectangle(brush, r.X, r.Y + r.Height / 2, r.Width, r.Height / 2);
			}
			
			using(Pen pen=new Pen(Color.Black))
			{
				r.Width--;
				r.Height--;
				g.DrawRectangle(pen, r);
				
				g.DrawLine(pen, r.X, r.Y + r.Height / 2, r.Right, r.Y + r.Height / 2);
			}
		}
		
		public Color NewColor
		{
			get { return m_NewColor;}
			set
			{
				m_NewColor = value;
				this.Invalidate();
			}
		}
		
		public Color CurrentColor
		{
			get { return m_CurrentColor;}
			set
			{
				m_CurrentColor = value;
				this.Invalidate();
			}
		}
		#endregion
	}
}
