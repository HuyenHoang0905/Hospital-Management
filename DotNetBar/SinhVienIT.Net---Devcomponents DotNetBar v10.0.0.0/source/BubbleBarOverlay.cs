using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents overlay class to support bubble effects on BubbleBar control.
	/// </summary>
	[ToolboxItem(false)]
	internal class BubbleBarOverlay:Control
	{
		#region Private Variables
		private BubbleBar m_BubbleBar=null;
		private Timer m_VisibilityTimer=null;
		#endregion

		#region Internal Implementation
		public BubbleBarOverlay(BubbleBar parent)
		{
			if(parent==null)
				throw new InvalidOperationException("Parent BubbleBar object for BubbleBarOverlay cannot be null.");

			m_BubbleBar=parent;
			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			//this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor,true);
			this.SetStyle(ControlStyles.ContainerControl,false);
			this.SetStyle(ControlStyles.Selectable,false);
			this.BackColor=Color.Transparent;
		}

		protected override void OnPaint(PaintEventArgs e){}

		internal void UpdateWindow()
		{
			// Create bitmap for drawing onto
			Bitmap memoryBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);

			using(Graphics g = Graphics.FromImage(memoryBitmap))
			{
				// Draw the background area
				g.Clear(Color.Transparent);
				g.CompositingMode=System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

				PaintEventArgs p=new PaintEventArgs(g,this.DisplayRectangle);
				m_BubbleBar.OnPaintOverlay(p);
				
				// Get hold of the screen DC
				IntPtr hDC = NativeFunctions.GetDC(IntPtr.Zero);
				
				// Create a memory based DC compatible with the screen DC
				IntPtr memoryDC = WinApi.CreateCompatibleDC(hDC);
				
				// Get access to the bitmap handle contained in the Bitmap object
				IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));

				// Select this bitmap for updating the window presentation
				IntPtr oldBitmap = WinApi.SelectObject(memoryDC, hBitmap);

				// New window size
				NativeFunctions.SIZE ulwsize;
				ulwsize.cx = this.Width;
				ulwsize.cy = this.Height;

				// New window position
				NativeFunctions.POINT topPos;
				topPos.x = this.Left;
				topPos.y = this.Top;

				// Offset into memory bitmap is always zero
				NativeFunctions.POINT pointSource;
				pointSource.x = 0;
				pointSource.y = 0;

				// We want to make the entire bitmap opaque 
				NativeFunctions.BLENDFUNCTION blend = new NativeFunctions.BLENDFUNCTION();
				blend.BlendOp             = (byte)NativeFunctions.Win23AlphaFlags.AC_SRC_OVER;
				blend.BlendFlags          = 0;
				blend.SourceConstantAlpha = 255;
				blend.AlphaFormat         = (byte)NativeFunctions.Win23AlphaFlags.AC_SRC_ALPHA;

				// Tell operating system to use our bitmap for painting
				NativeFunctions.UpdateLayeredWindow(this.Handle, hDC, ref topPos, ref ulwsize, 
					memoryDC, ref pointSource, 0, ref blend, 
					(int)NativeFunctions.Win32UpdateLayeredWindowsFlags.ULW_ALPHA);
				// Put back the old bitmap handle
				WinApi.SelectObject(memoryDC, oldBitmap);

				// Cleanup resources
                WinApi.ReleaseDC(IntPtr.Zero, hDC);
                WinApi.DeleteObject(hBitmap);
                WinApi.DeleteDC(memoryDC);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			this.Capture=true;
			base.OnMouseEnter (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(!this.DisplayRectangle.Contains(e.X,e.Y))
				this.Capture=false;
			m_BubbleBar.MouseMoveMessage(e);
			base.OnMouseMove (e);	
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			m_BubbleBar.MouseLeaveMessage(e);
			base.OnMouseLeave(e);	
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			m_BubbleBar.OverlayMouseDownMessage(e);
			base.OnMouseDown (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			m_BubbleBar.OverlayMouseUpMessage(e);
			base.OnMouseUp(e);
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_MOUSEACTIVATE = 0x21;
			const int MA_NOACTIVATE = 3;
			if(m.Msg==WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}

		protected override CreateParams CreateParams
		{
			get
			{
				const uint WS_POPUP=0x80000000;
				const uint WS_CLIPSIBLINGS=0x04000000;
				const uint WS_CLIPCHILDREN=0x02000000;
				const int WS_EX_LAYERED=0x00080000;
				const int WS_EX_TOOLWINDOW=0x00000080;
				CreateParams p=base.CreateParams;
				p.ExStyle=(p.ExStyle | WS_EX_LAYERED | WS_EX_TOOLWINDOW);
				p.Style=unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
				p.Caption="";
				return p;
			}
		}

		private void CreateTimer()
		{
			if(m_VisibilityTimer!=null)
				return;
			m_VisibilityTimer=new Timer();
			m_VisibilityTimer.Tick+=new EventHandler(this.CheckVisibility);
            m_VisibilityTimer.Interval=500;
			m_VisibilityTimer.Enabled=true;
			m_VisibilityTimer.Start();
		}

		private void DestroyTimer()
		{
			if(m_VisibilityTimer==null)
				return;
			m_VisibilityTimer.Stop();
			m_VisibilityTimer.Enabled=false;
			m_VisibilityTimer.Dispose();
			m_VisibilityTimer=null;
		}

		private void CheckVisibility(object sender,EventArgs e)
		{
            try
            {
                BubbleBar bar = m_BubbleBar;
                if (bar == null || bar.IsDisposed)
                {
                    DestroyTimer();
                    return;
                }

                Form form = bar.FindForm();
                IntPtr foregroundWindow = NativeFunctions.GetForegroundWindow();
                Control c = null;
                if (foregroundWindow != IntPtr.Zero)
                    c = Control.FromHandle(foregroundWindow);
                if (c == null)
                {
                    OnOverlayInactive();
                    return;
                }
                if (form != null)
                {
                    if (Form.ActiveForm == form)
                        return;
                    Form parentForm = form;
                    while (parentForm.ParentForm != null)
                    {
                        if (parentForm.ParentForm == Form.ActiveForm)
                            return;
                        parentForm = parentForm.ParentForm;
                    }

                    if (form.IsMdiChild && form.MdiParent != null)
                    {
                        if (form.MdiParent.ActiveMdiChild == form)
                            return;
                    }
                }

                NativeFunctions.POINT p = new NativeFunctions.POINT();
                Point pm = Control.MousePosition;
                p.x = pm.X;
                p.x = pm.Y;
                IntPtr window = IntPtr.Zero;
                if (bar.Parent != null)
                    NativeFunctions.ChildWindowFromPoint(bar.Parent.Handle, p);
                if (window == IntPtr.Zero)
                {
                    window = NativeFunctions.WindowFromPoint(p);
                }
                if (window != IntPtr.Zero && window != this.Handle && window != bar.Handle && bar.Parent != null && bar.Parent.Handle != window)
                {
                    OnOverlayInactive();
                }
                else
                {
                    Point pc = this.PointToClient(pm);
                    if (!this.DisplayRectangle.Contains(pc))
                    {
                        bar.MouseLeaveMessage(e);
                    }
                }
            }
            catch (NullReferenceException)
            {
                DestroyTimer();
            }
		}

		private void OnOverlayInactive()
		{
			this.Capture=false;
			DestroyTimer();
			m_BubbleBar.OverlayInactive();
		}

		/// <summary>
		/// Called just before window is shown.
		/// </summary>
		internal void BeforeShow()
		{
			CreateTimer();
		}

		/// <summary>
		/// Called after window is closed.
		/// </summary>
		internal void AfterClose()
		{
			DestroyTimer();
		}
		#endregion
	}
}
