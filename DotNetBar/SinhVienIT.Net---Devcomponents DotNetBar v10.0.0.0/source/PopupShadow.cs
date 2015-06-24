using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for PopupShadow.
	/// </summary>
	internal class PopupShadow : System.Windows.Forms.Control
	{
		private bool m_AlphaShadow=true;
		public PopupShadow(bool bAlphaShadow)
		{
			m_AlphaShadow=bAlphaShadow;
			this.Visible=false;
			this.SuspendLayout();
			if(!m_AlphaShadow)
				this.BackColor=SystemColors.ControlDark;
			//this.ControlBox = false;
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			//this.MaximizeBox = false;
			//this.MinimizeBox = false;
			//this.Opacity = 0.4;
			//this.ShowInTaskbar = false;
			//this.AutoScale=false;
			this.Size=new Size(12,12);
			//this.StartPosition=FormStartPosition.Manual;
			//this.MinimumSize=new Size(4,4);  // This was the key to fix the black box that was showing up for smaller windows

			this.SetStyle(ControlStyles.ContainerControl,false);
			//this.TopLevel=false;

			this.SetStyle(ControlStyles.Selectable,false);
			if(m_AlphaShadow)
			{
				this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
				this.SetStyle(DisplayHelp.DoubleBufferFlag,false);
			}
			this.ResumeLayout();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
            //NativeFunctions.SetLayeredWindowAttributes(this.Handle,Color.Black.ToArgb(),102,NativeFunctions.LWA_ALPHA);
			// We want to make the entire bitmap opaque 
		}
		
		private void PaintShadow()
		{
            if(this.Width <= 0 || this.Height <= 0) return;

			// Create bitmap for drawing onto
            using (Bitmap memoryBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(memoryBitmap))
                {
                    Rectangle area = new Rectangle(0, 0, this.Width, this.Height);

                    System.Drawing.Region reg = new System.Drawing.Region();
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    Rectangle r = this.ClientRectangle;
                    const int TRIANGLESIZE = 5;
                    path.StartFigure();
                    path.AddLine(r.Right - TRIANGLESIZE, r.Top, r.Right, r.Top);
                    path.AddLine(r.Right, r.Top, r.Right, r.Top + TRIANGLESIZE);
                    path.AddLine(r.Right, r.Top + TRIANGLESIZE, r.Right - TRIANGLESIZE, r.Top);
                    path.CloseFigure();
                    path.StartFigure();
                    path.AddLine(r.Left, r.Bottom - TRIANGLESIZE - 1, r.Left, r.Bottom);
                    path.AddLine(r.Left, r.Bottom, r.Left + TRIANGLESIZE, r.Bottom);
                    path.AddLine(r.Left + TRIANGLESIZE, r.Bottom, r.Left, r.Bottom - TRIANGLESIZE);
                    path.CloseFigure();
                    path.StartFigure();
                    path.AddRectangle(new Rectangle(0, 0, r.Width - TRIANGLESIZE, r.Height - TRIANGLESIZE));
                    path.CloseFigure();
                    reg.Xor(path);
                    g.SetClip(reg, System.Drawing.Drawing2D.CombineMode.Replace);
                    path.Dispose();
                    path = null;
                    reg.Dispose();
                    reg = null;
                    // Draw the background area
                    g.Clear(Color.Transparent);
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    // Draw Actual Shadow
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(Color.Black);
                    const int EDGE = 4;
                    r.Width--;
                    r.Height--;
                    Color[] clr = new Color[]{
										   Color.FromArgb(14,Color.Black),
										   Color.FromArgb(43,Color.Black),
										   Color.FromArgb(84,Color.Black),
										   Color.FromArgb(113,Color.Black),
										   Color.FromArgb(128,Color.Black)};

                    for (int i = 0; i < EDGE; i++)
                    {
                        using (Pen pen = new Pen(clr[i], 1))
                        {
                            using (GraphicsPath tempPath = GetPath(r, EDGE - i))
                                g.DrawPath(pen, tempPath);
                            r.Inflate(-1, -1);
                        }
                    }

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
                    blend.BlendOp = (byte)NativeFunctions.Win23AlphaFlags.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = 255;
                    blend.AlphaFormat = (byte)NativeFunctions.Win23AlphaFlags.AC_SRC_ALPHA;

                    // Tell operating system to use our bitmap for painting
                    NativeFunctions.UpdateLayeredWindow(Handle, hDC, ref topPos, ref ulwsize,
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
		}

		public void UpdateShadow()
		{
			if(!m_AlphaShadow)
				return;
			PaintShadow();
		}

		private System.Drawing.Drawing2D.GraphicsPath GetPath(Rectangle r, int pathSize)
		{
			System.Drawing.Drawing2D.GraphicsPath path=new System.Drawing.Drawing2D.GraphicsPath();
			path.AddArc(r.Right-pathSize,r.Y,pathSize,pathSize,270,90);
			path.AddLine(r.Right,r.Y+pathSize,r.Right,r.Bottom-pathSize);
			path.AddArc(r.Right-pathSize,r.Bottom-pathSize,pathSize,pathSize,0,90);
			path.AddLine(r.Right-pathSize,r.Bottom,r.X,r.Bottom);
			path.AddLine(r.X,r.Bottom,r.X,r.Top);
			return path;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if(!m_AlphaShadow)
			{
				e.Graphics.Clear(this.BackColor);
				return;
			}
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_MOUSEACTIVATE = 0x21;
			const int MA_NOACTIVATE = 3;
			//const int MA_NOACTIVATEANDEAT = 4;
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
				const int WS_EX_TOPMOST=0x00000008;
				const int WS_EX_LAYERED=0x00080000;
				const int WS_EX_TOOLWINDOW=0x00000080;
				CreateParams p=base.CreateParams;
				p.ExStyle=(p.ExStyle | WS_EX_TOPMOST | (m_AlphaShadow?WS_EX_LAYERED:0) | WS_EX_TOOLWINDOW);
				p.Style=unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
				p.Caption="";  // Setting caption would show window under tasks in Windows Task Manager
				return p;
			}
		}
	}
}
