using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Use as a popup container for DotNetBar objects.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false),System.Runtime.InteropServices.ComVisible(false)]
	public class PopupContainer:Control
	{
		const int WM_MOUSEACTIVATE = 0x21;
		const int MA_NOACTIVATE = 3;
		const int MA_NOACTIVATEANDEAT = 4;
		const uint WS_POPUP=0x80000000;
		const uint WS_CLIPSIBLINGS=0x04000000;
		const uint WS_CLIPCHILDREN=0x02000000;
		const uint WS_EX_TOPMOST=0x00000008;
		const uint WS_EX_TOOLWINDOW=0x00000080;

		private PopupShadow m_DropShadow=null;
		private bool m_ShowDropShadow=false;

		const int SHADOW_OFFSET=5;

		public PopupContainer()
		{
			this.IsAccessible=false;
			this.SetStyle(ControlStyles.Selectable,false);
		}

		protected override void Dispose(bool disposing)
		{
			if(m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
			base.Dispose(disposing);
		}

		private MenuPanel HostedMenu
		{
			get
			{
				if(this.Controls.Count>0 && this.Controls[0] is MenuPanel)
					return this.Controls[0] as MenuPanel;
				return null;
			}
		}

		private Bar HostedBar
		{
			get
			{
				if(this.Controls.Count>0 && this.Controls[0] is Bar)
					return this.Controls[0] as Bar;
				return null;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if(!this.Visible && m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p=base.CreateParams;
				p.Style=unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
				p.ExStyle=(int)(WS_EX_TOPMOST | WS_EX_TOOLWINDOW);
				p.Caption=""; // Setting caption would show window under tasks in Windows Task Manager
				return p;
			}
		}
		
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}
		
		public void ShowShadow()
		{
			if(m_ShowDropShadow)
			{
				if(m_DropShadow!=null)
				{
					m_DropShadow.Hide();
					m_DropShadow.Dispose();
                    m_DropShadow = null;
				}

                if (this.Width > 5 && this.Height > 5)
                {
                    m_DropShadow = new PopupShadow(true);
                    m_DropShadow.CreateControl();
                    NativeFunctions.SetWindowPos(m_DropShadow.Handle, NativeFunctions.HWND_NOTOPMOST, this.Left + SHADOW_OFFSET, this.Top + SHADOW_OFFSET, this.Width - 2, this.Height - 2, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
                    m_DropShadow.UpdateShadow();
                }
			}
		}

		public bool ShowDropShadow
		{
			get {return m_ShowDropShadow;}
			set {m_ShowDropShadow=value;}
		}

		protected override void OnResize(EventArgs e)
		{
			if(m_DropShadow!=null)
			{
				NativeFunctions.SetWindowPos(m_DropShadow.Handle,NativeFunctions.HWND_NOTOPMOST,this.Left+SHADOW_OFFSET,this.Top+SHADOW_OFFSET,this.Width-2,this.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
				m_DropShadow.UpdateShadow();
			}
			base.OnResize(e);
		}
	}
}
