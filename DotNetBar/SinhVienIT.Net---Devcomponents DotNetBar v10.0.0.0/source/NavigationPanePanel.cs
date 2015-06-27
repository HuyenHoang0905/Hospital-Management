using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Panel on Navigation Pane control.
	/// </summary>
	[ToolboxItem(false),Designer("DevComponents.DotNetBar.Design.NavigationPanePanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class NavigationPanePanel:PanelEx
	{
		private BaseItem m_ParentItem;
		private const string INFO_TEXT="Drop controls on this panel to associate them with current selection";
		public NavigationPanePanel()
		{
			
		}

		/// <summary>
		/// Gets or sets button associated with the pane on the panel when hosted on NavigationPane control.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false)]
		public BaseItem ParentItem
		{
			get {return m_ParentItem;}
			set
			{
				m_ParentItem=value;
				if(m_ParentItem!=null && this.Parent is NavigationPane && m_ParentItem is ButtonItem)
				{
					if(((ButtonItem)m_ParentItem).Checked)
					{
						this.Visible=true;
						this.BringToFront();
					}
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if(this.DesignMode && this.Controls.Count==0 && this.Text=="")
			{
				Rectangle r=this.ClientRectangle;
				r.Inflate(-2,-2);
                eTextFormat sf = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.EndEllipsis;
				Font font=new Font(this.Font,FontStyle.Bold);
				TextDrawing.DrawString(e.Graphics,INFO_TEXT,font,ControlPaint.Dark(this.Style.BackColor1.Color),r,sf);
				font.Dispose();
			}
		}
	}
}
