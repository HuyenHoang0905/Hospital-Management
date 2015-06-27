using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for BarTextBox.
	/// </summary>
	internal class BarTextBox:TextBox
	{
		private int m_LastFocusWindow;
		private string m_OriginalText;
		public BarTextBox()
		{
			this.BorderStyle=System.Windows.Forms.BorderStyle.None;
			this.AutoSize=false;
			this.TabStop=false;
			m_LastFocusWindow=0;
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==NativeFunctions.WM_SETFOCUS)
			{
				m_LastFocusWindow=m.WParam.ToInt32();
				m_OriginalText=this.Text;
			}
			base.WndProc(ref m);
		}

		public void ReleaseFocus()
		{
			if(this.Focused && m_LastFocusWindow!=0)
			{
				int focus=m_LastFocusWindow;
				m_LastFocusWindow=0;
				Control ctrl=Control.FromChildHandle(new System.IntPtr(focus));
				if(ctrl!=this)
				{
					Control p=this.Parent;
					while(p!=null)
					{
						if(p==ctrl)
							return;
						p=p.Parent;
					}

					if(ctrl!=null)
						ctrl.Focus();
					else
					{
						NativeFunctions.SetFocus(focus);
					}
				}
                
				//this.OnLostFocus(new System.EventArgs());
				//this.InvokeLostFocus(this,new System.EventArgs());
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Enter)
				ReleaseFocus();
			else if(e.KeyCode==Keys.Escape)
			{
				this.Text=m_OriginalText;
				ReleaseFocus();
			}

			base.OnKeyDown(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			m_LastFocusWindow=0;
			base.OnLostFocus(e);
		}
	}
}
