namespace DevComponents.DotNetBar
{
    using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Drawing.Text;
    using System.Drawing.Drawing2D;

    /// <summary>
    ///    Summary description for Tooltip.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
    public class ToolTip : System.Windows.Forms.Control
    {
		const long WS_POPUP=0x80000000L;
		const long WS_CLIPSIBLINGS=0x04000000L;
		const long WS_CLIPCHILDREN=0x02000000L;
		const long WS_EX_TOOLWINDOW=0x00000080L;
		const long WS_EX_TOPMOST=0x00000008L;

		// When set to true tooltip will be shown Immediately without delay
		//static bool bShowToolTip;
		
		private string m_ToolTip;
        private TextMarkup.BodyElement m_TextMarkup = null;
        private eDotNetBarStyle m_Style = eDotNetBarStyle.Office2003;
		private PopupShadow m_DropShadow=null;
        private bool m_AntiAlias = false;
        /// <summary>
        /// Gets or sets the rectangle of the control or item tooltip is displayed for.
        /// </summary>
        public Rectangle ReferenceRectangle = Rectangle.Empty;

        public ToolTip()
        {
			m_ToolTip="";
			this.SetStyle(ControlStyles.Selectable,false);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
            this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
			this.BackColor=SystemColors.Control;
			this.ForeColor=SystemColors.ControlText;
        }

		public new string Text
		{
			get
			{
				return m_ToolTip;
			}
			set
			{
				m_ToolTip=value;
                if (MarkupEnabled && TextMarkup.MarkupParser.IsMarkup(ref m_ToolTip))
                    m_TextMarkup = TextMarkup.MarkupParser.Parse(m_ToolTip);
                else
                    m_TextMarkup = null;
                if (m_TextMarkup != null)
                    AntiAlias = true;
			}
		}

        private static bool _MarkupEnabled = true;
        /// <summary>
        /// Gets or sets whether text-markup is enabled for the tooltips.
        /// </summary>
        public static bool MarkupEnabled
        {
            get { return _MarkupEnabled; }
            set
            {
                _MarkupEnabled = value;
            }
        }
        

        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set { m_AntiAlias = value; }
        }
        private int _ColorDepth = -1;
		protected override void OnPaint(PaintEventArgs e)
		{
            // Get the Color of ToolTip text
            Color textColor = SystemColors.InfoText;
            Color backColor = SystemColors.Info;
            Color backColor2=Color.Empty;
            Color borderColor = ColorScheme.GetColor("767676");

            if (_ColorDepth == -1)
            {
                _ColorDepth = Screen.FromControl(this).BitsPerPixel;
            }
            if (_ColorDepth < 8)
            {
                backColor = Color.White;
                textColor = Color.Black;
            }
            else
            {
                if (BarFunctions.IsOffice2007Style(m_Style) && this.BackColor == SystemColors.Control)
                {
                    backColor = ColorScheme.GetColor("FFFFFF");
                    backColor2 = ColorScheme.GetColor("E4E4F0");
                    textColor = ColorScheme.GetColor("4C4C4C");
                }
                else
                {
                    textColor = GetToolTipColor();
                    if (this.BackColor != SystemColors.Control)
                        backColor = this.BackColor;
                }
            }

			Graphics g=e.Graphics;
			Rectangle r=this.ClientRectangle;

            if (m_AntiAlias)
            {
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;
            if (BarFunctions.IsOffice2007Style(m_Style))
            {
                DisplayHelp.DrawRectangle(g, borderColor, r);
            }
            else
            {
                System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.Raised, System.Windows.Forms.Border3DSide.All);
            }
			
			r.Inflate(-1,-1);
            if (backColor2.IsEmpty)
                DisplayHelp.FillRectangle(g, r, backColor);
            else
                DisplayHelp.FillRectangle(g, r, backColor, backColor2);
			
			r.Offset(1,0);

            g.SmoothingMode = sm;

            if (m_TextMarkup == null)
                TextDrawing.DrawString(g, GetDrawText(), this.Font, textColor, r, GetStringFormat());
            else
            {
                TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, textColor, (this.RightToLeft == RightToLeft.Yes), e.ClipRectangle, true);
                m_TextMarkup.Bounds = new Rectangle(r.Location, m_TextMarkup.Bounds.Size);
                m_TextMarkup.Render(d);
            }
		}

		public void ShowToolTip()
		{
			if (!this.IsHandleCreated)
				this.CreateControl();

            Size sz = Size.Empty;
			// Calculate Size of the window
			Graphics g=this.CreateGraphics();

            if (m_AntiAlias)
            {
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }

            try
            {
                g.PageUnit = GraphicsUnit.Pixel;
                if (m_TextMarkup == null)
                    sz = TextDrawing.MeasureString(g, GetDrawText(), this.Font, Screen.PrimaryScreen.WorkingArea.Size, GetStringFormat());
                else
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, SystemColors.Control, (this.RightToLeft == RightToLeft.Yes));
                    m_TextMarkup.Measure(Screen.PrimaryScreen.WorkingArea.Size, d);
                    sz = m_TextMarkup.Bounds.Size;
                    m_TextMarkup.Arrange(new Rectangle(Point.Empty, sz), d);
                }
            }
            finally
            {
                g.SmoothingMode = SmoothingMode.Default;
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
                g.Dispose();
            }
			g=null;

			Point mousePosition=System.Windows.Forms.Control.MousePosition;
			Rectangle r=new Rectangle(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y,(int)sz.Width,(int)sz.Height);
			r.Inflate(2,2);
			r.Offset(12,24);

			ScreenInformation screen=BarFunctions.ScreenFromPoint(mousePosition);
			if(screen!=null)
			{
				System.Drawing.Size layoutArea=screen.WorkingArea.Size;
				layoutArea.Width-=(int)(layoutArea.Width*.2f);

                if (r.Right > screen.WorkingArea.Right)
                {
                    r.X = r.Left - (r.Right - screen.WorkingArea.Right);
                }

                if (r.Bottom > screen.Bounds.Bottom)
                {
                    if (ReferenceRectangle.IsEmpty)
                        r.Y = mousePosition.Y - r.Height;
                    else
                    {
                        r.Y = ReferenceRectangle.Y - r.Height - 1;
                    }
                }
				
				if(r.Contains(System.Windows.Forms.Control.MousePosition.X,System.Windows.Forms.Control.MousePosition.Y))
				{
					// We have to move it out of mouse position
                    if (r.Height + System.Windows.Forms.Control.MousePosition.Y + 1 <= screen.WorkingArea.Height && (ReferenceRectangle.IsEmpty || !ReferenceRectangle.IntersectsWith(new Rectangle(r.X, System.Windows.Forms.Control.MousePosition.Y + 1, r.Width, r.Height))))
						r.Y=System.Windows.Forms.Control.MousePosition.Y+1;
					else
						r.Y=System.Windows.Forms.Control.MousePosition.Y-r.Height-1;
				}
			}

			this.Location=r.Location;
			this.ClientSize=r.Size;

			if(NativeFunctions.ShowDropShadow)
			{
				if(m_DropShadow==null)
				{
					m_DropShadow=new PopupShadow(NativeFunctions.AlphaBlendingSupported);
					m_DropShadow.CreateControl();
				}
				//m_DropShadow.Location=new Point(r.Left+4,r.Top+4);
				//m_DropShadow.Size=r.Size;
				// TODO: Bug Cannot set size and location correctly using the Size and Location because Form caption is hidden
				m_DropShadow.Hide();
			}
			NativeFunctions.SetWindowPos(this.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOSIZE | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE);
			if(m_DropShadow!=null)
			{
				NativeFunctions.SetWindowPos(m_DropShadow.Handle,this.Handle.ToInt32(),r.Left+5,r.Top+5,r.Width-2,r.Height-2,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
				m_DropShadow.UpdateShadow();
			}
		}

        private string GetDrawText()
        {
            string s = m_ToolTip.Replace(@"\\n", "{spec_nl}");
            s = s.Replace(@"\n", Environment.NewLine);
            return s.Replace("{spec_nl}",@"\n");
        }

		private eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak | eTextFormat.VerticalCenter;
            return format;
		}

		private Color GetToolTipColor()
		{
			if(this.ForeColor!=SystemColors.ControlText)
				return this.ForeColor;

			Color clrRet=SystemColors.WindowText;

			try
			{
				Microsoft.Win32.RegistryKey objReg=Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Control Panel\\Colors\\",false);
				string sColor=objReg.GetValue("InfoText").ToString();
				objReg.Close();
				objReg=null;
				
				if(sColor==null || sColor=="")
					return clrRet;

				Char [] separator = {' '};
				string[] sArr=sColor.Split(separator,3);
				if(sArr.GetUpperBound(0)==2)
					clrRet=Color.FromArgb(System.Convert.ToInt16(sArr[0]),System.Convert.ToInt16(sArr[1]),System.Convert.ToInt16(sArr[2]));
			}
			catch(Exception)
			{
			}

			return clrRet;
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			if(m_DropShadow!=null)
			{
				NativeFunctions.SetWindowPos(m_DropShadow.Handle,NativeFunctions.HWND_TOP,this.Left+5,this.Top+5,0,0,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOSIZE);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams p=base.CreateParams;
				p.Style=unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
				p.ExStyle=(int)(WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
				p.Caption="";
				return p;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			if(!this.Visible && m_DropShadow!=null)
			{
				m_DropShadow.Hide();
				m_DropShadow.Dispose();
				m_DropShadow=null;
			}
			base.OnVisibleChanged(e);
		}

        public eDotNetBarStyle Style
        {
            get { return m_Style; }
            set
            {
                m_Style = value;
                if (BarFunctions.IsOffice2007Style(m_Style))
                    this.AntiAlias = true;
            }
        }

    }
}
