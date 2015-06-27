using System;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents SuperTooltipControl.
    /// </summary>
    [ToolboxItem(false)]
    public class SuperTooltipControl:PanelControl
    {
        #region Events
        
        #endregion

        #region Private variables, Constructor
        const long WS_POPUP = 0x80000000L;
        const long WS_CLIPSIBLINGS = 0x04000000L;
        const long WS_CLIPCHILDREN = 0x02000000L;
        const long WS_EX_TOOLWINDOW = 0x00000080L;
        const long WS_EX_TOPMOST = 0x00000008L;

        private string m_HeaderText="";
        private TextMarkup.BodyElement m_HeaderMarkup = null;
        private string m_FooterText = "";
        private TextMarkup.BodyElement m_FooterMarkup = null;
        private Image m_FooterImage = null;
        private Image m_BodyImage = null;
        private Size m_MinimumTooltipSize = new Size(100,18);
        private bool m_HeaderVisible = true;
        private bool m_FooterVisible = true;
        private bool m_FooterSeparator = true;
        private int m_FooterImageSpacing = 8;
        private PopupShadow m_DropShadow = null;
        private bool m_StandardControl = false;
        private TextMarkup.BodyElement m_BodyMarkup = null;
        private bool m_MouseActivateEnabled = true;
        private int m_MaximumWidth = 0;
        private bool m_ShowTooltipDescription = true;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SuperTooltipControl():base()
        {
            
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets whether complete tooltip is shown including header, body and footer. Default value is true. When set to false only tooltip header will be shown.
        /// Providing this option to your end users as part of your application setting allows them to customize the level of information displayed and reduce it after they are familiar with your product.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether complete tooltip is shown including header, body and footer. When set to false only tooltip header is shown.")]
        public bool ShowTooltipDescription
        {
            get
            {
                return m_ShowTooltipDescription;
            }
            set
            {
                m_ShowTooltipDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum width of the super tooltip. Default value is 0 which indicates that maximum width is not used. The maximum width property
        /// will not be used if custom size is specified.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Appearance"), Description("Indicates maximum width of the super tooltip.")]
        public int MaximumWidth
        {
            get { return m_MaximumWidth; }
            set { m_MaximumWidth = value; }
        }

        private eTooltipColor m_PredefinedColor = eTooltipColor.Default;
        [DefaultValue(eTooltipColor.Default)]
        public eTooltipColor PredefinedColor
        {
            get { return m_PredefinedColor; }
            set
            {
                m_PredefinedColor = value;
                this.ApplyColors(value);
                this.Refresh();
            }
        }

        internal TextMarkup.BodyElement BodyMarkup
        {
            get { return m_BodyMarkup; }
        }

        /// <summary>
        /// Gets or sets the minimum tooltip size. Default value is 150, 50
        /// </summary>
        public Size MinimumTooltipSize
        {
            get { return m_MinimumTooltipSize; }
            set { m_MinimumTooltipSize = value; }
        }

        /// <summary>
        /// Gets or sets image used next to body text.
        /// </summary>
        [DefaultValue(null)]
        public Image BodyImage
        {
            get { return m_BodyImage; }
            set { m_BodyImage = value; }
        }

        /// <summary>
        /// Gets or sets image used next to footer text.
        /// </summary>
        [DefaultValue(null)]
        public Image FooterImage
        {
            get { return m_FooterImage; }
            set { m_FooterImage = value; }
        }

        /// <summary>
        /// Gets or sets text displayed in header of tooltip
        /// </summary>
        [Browsable(true),DefaultValue("")]
        public string HeaderText
        {
            get { return m_HeaderText; }
            set
            {
                m_HeaderText = value;
                if (TextMarkup.MarkupParser.IsMarkup(ref m_HeaderText))
                {
                    m_HeaderMarkup = TextMarkup.MarkupParser.Parse(m_HeaderText);
                    if (m_HeaderMarkup != null)
                        m_HeaderMarkup.HyperLinkClick += new EventHandler(Markup_HyperLinkClick);
                }
                else
                    m_HeaderMarkup = null;
            }
        }

        /// <summary>
        /// Gets or sets whether header in tooltip is visible. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool HeaderVisible
        {
            get { return m_HeaderVisible; }
            set
            {
                m_HeaderVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets text displayed in footer of the tooltip
        /// </summary>
        [Browsable(true), DefaultValue("")]
        public string FooterText
        {
            get { return m_FooterText; }
            set
            {
                m_FooterText = value;
                if (TextMarkup.MarkupParser.IsMarkup(ref m_FooterText))
                {
                    m_FooterMarkup = TextMarkup.MarkupParser.Parse(m_FooterText);
                    if (m_FooterMarkup != null)
                        m_FooterMarkup.HyperLinkClick += new EventHandler(Markup_HyperLinkClick);
                }
                else
                    m_FooterMarkup = null;
            }
        }

        /// <summary>
        /// Gets or sets whether footer in tooltip is visible. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool FooterVisible
        {
            get { return m_FooterVisible; }
            set
            {
                m_FooterVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether line above footer text is drawn to separate footer from body text. Default value is true.
        /// </summary>
        [Browsable(true),DefaultValue(true)]
        public bool FooterSeparator
        {
            get { return m_FooterSeparator; }
            set { m_FooterSeparator = value; }
        }
#if FRAMEWORK20
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!BarFunctions.IsHandleValid(this))
                return proposedSize;
            return GetAutoSize();
        }
#endif

        /// <summary>
        /// Gets or sets whether mouse click on super tooltip will activate it, make it active window. Default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool MouseActivateEnabled
        {
            get { return m_MouseActivateEnabled; }
            set
            {
                m_MouseActivateEnabled = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            string s = this.Text;
            if (TextMarkup.MarkupParser.IsMarkup(ref s))
            {
                m_BodyMarkup = TextMarkup.MarkupParser.Parse(s);
                if(m_BodyMarkup!=null)
                    m_BodyMarkup.HyperLinkClick += new EventHandler(Markup_HyperLinkClick);
            }
            else
                m_BodyMarkup = null;

            base.OnTextChanged(e);
        }

        private void Markup_HyperLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
            {
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_BodyMarkup != null)
                m_BodyMarkup.MouseMove(this, e);
            if (m_HeaderMarkup != null)
                m_HeaderMarkup.MouseMove(this, e);
            if (m_FooterMarkup != null)
                m_FooterMarkup.MouseMove(this, e);
            
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (m_BodyMarkup != null)
                m_BodyMarkup.MouseDown(this, e);
            if (m_HeaderMarkup != null)
                m_HeaderMarkup.MouseDown(this, e);
            if (m_FooterMarkup != null)
                m_FooterMarkup.MouseDown(this, e);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (m_BodyMarkup != null)
                m_BodyMarkup.MouseUp(this, e);
            if (m_HeaderMarkup != null)
                m_HeaderMarkup.MouseUp(this, e);
            if (m_FooterMarkup != null)
                m_FooterMarkup.MouseUp(this, e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (m_BodyMarkup != null)
                m_BodyMarkup.MouseLeave(this);
            if (m_HeaderMarkup != null)
                m_HeaderMarkup.MouseLeave(this);
            if (m_FooterMarkup != null)
                m_FooterMarkup.MouseLeave(this);

            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (m_BodyMarkup != null)
                m_BodyMarkup.Click(this);
            if (m_HeaderMarkup != null)
                m_HeaderMarkup.Click(this);
            if (m_FooterMarkup != null)
                m_FooterMarkup.Click(this);
            base.OnClick(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.WM_MOUSEACTIVATE && !m_MouseActivateEnabled)
            {
                const int MA_NOACTIVATE = 3;
                m.Result = new System.IntPtr(MA_NOACTIVATE);
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Paints insides of the control.
        /// </summary>
        /// <param name="e">Paint event arguments.</param>
        protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
        {
            base.PaintInnerContent(e, style, false);
            if (!paintText)
                return;

            Rectangle r = this.ClientRectangle;
            r.X += ElementStyleLayout.LeftWhiteSpace(this.Style);
            r.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(this.Style);
            r.Y += ElementStyleLayout.TopWhiteSpace(this.Style);
            r.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(this.Style);
            if (r.Width <= 4 || r.Height <= 4) return;

            Graphics g = e.Graphics;

            Font font = this.Font;
            if (this.Style.Font != null)
                font = this.Style.Font;
            Font headerFont = new Font(font, (m_ShowTooltipDescription || m_HeaderText == "") ? FontStyle.Bold : FontStyle.Regular);
            Padding headerPadding = GetHeaderPadding();
            Padding footerPadding = GetFooterPadding();
            Padding textPadding = GetTextPadding();
            Padding imagePadding = GetImagePadding();

            try
            {
                ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, g, Rectangle.Empty);
                if (m_HeaderText != "" && HeaderVisible)
                {
                    Rectangle headerRect = new Rectangle(r.X + headerPadding.Left, r.Y + headerPadding.Top, r.Width - headerPadding.Horizontal, r.Height - headerPadding.Vertical);
                    Size headerSize = Size.Empty;
                    if (m_HeaderMarkup == null)
                    {
                        info.Bounds = headerRect;
                        eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                        if (this.RightToLeft == RightToLeft.Yes) format |= eTextFormat.RightToLeft;
                        ElementStyleDisplay.PaintText(info, m_HeaderText, headerFont, true, format);
                        headerSize = TextDrawing.MeasureString(g, m_HeaderText, headerFont, headerRect.Width, format);
                    }
                    else if(headerRect.Width>0 && headerRect.Height>0)
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, info.Style.TextColor,
                            (this.RightToLeft == RightToLeft.Yes), headerRect, true);
                        m_HeaderMarkup.Arrange(headerRect, d);
                        m_HeaderMarkup.Render(d);
                        headerSize = m_HeaderMarkup.Bounds.Size;
                    }
                    headerSize.Width += headerPadding.Horizontal;
                    headerSize.Height += headerPadding.Vertical;
                    r.Y += headerSize.Height;
                    r.Height -= headerSize.Height;
                }

                if (m_FooterText != "" && FooterVisible && r.Width>0 && r.Height>0 && (m_ShowTooltipDescription || m_HeaderText==""))
                {
                    Size footerSize = Size.Empty;
                    eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                    if (m_FooterMarkup == null)
                        footerSize = TextDrawing.MeasureString(g, m_FooterText, headerFont, r.Width - footerPadding.Horizontal, format);
                    else
                        footerSize = m_FooterMarkup.Bounds.Size;

                    if (m_FooterImage != null && m_FooterImage.Height > footerSize.Height)
                        footerSize.Height = m_FooterImage.Height;

                    Rectangle footerRect = new Rectangle(r.X+footerPadding.Left,
                        r.Bottom-footerSize.Height-footerPadding.Bottom,
                        r.Width - footerPadding.Horizontal, footerSize.Height);
                    if (this.FooterSeparator)
                    {
                        using (Pen pen = new Pen(style.BorderColor, 1))
                            g.DrawLine(pen, 0, footerRect.Y - footerPadding.Top - 1, this.ClientRectangle.Right, footerRect.Y - footerPadding.Top - 1);
                    }
                    if (m_FooterImage != null)
                    {
                        g.DrawImage(m_FooterImage, new Rectangle(footerRect.X, footerRect.Y + (footerRect.Height - m_FooterImage.Height) / 2, m_FooterImage.Width, m_FooterImage.Height));
                        footerRect.X += (m_FooterImage.Width + m_FooterImageSpacing);
                        footerRect.Width -= (m_FooterImage.Width + m_FooterImageSpacing);
                    }
                    if (footerRect.Width > 0 && footerRect.Height > 0)
                    {
                        if (m_FooterMarkup == null)
                        {
                            info.Bounds = footerRect;

                            format |= eTextFormat.VerticalCenter;
                            if (this.RightToLeft == RightToLeft.Yes) format |= eTextFormat.RightToLeft;
                            ElementStyleDisplay.PaintText(info, m_FooterText, headerFont, true, format);
                        }
                        else
                        {
                            TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, info.Style.TextColor,
                            (this.RightToLeft == RightToLeft.Yes), footerRect, true);
                            m_FooterMarkup.Arrange(footerRect, d);
                            m_FooterMarkup.Render(d);
                            footerSize = m_FooterMarkup.Bounds.Size;
                        }
                    }

                    footerSize.Width += footerPadding.Horizontal;
                    footerSize.Height += footerPadding.Vertical;
                    r.Height -= footerSize.Height;
                }

                if (m_ShowTooltipDescription || m_HeaderText == "")
                {
                    if (m_BodyImage != null)
                    {
                        Rectangle imageRect = new Rectangle(r.X + imagePadding.Left, r.Y + imagePadding.Top, m_BodyImage.Width, m_BodyImage.Height);
                        g.DrawImage(m_BodyImage, imageRect);
                        r.X += (imagePadding.Horizontal + m_BodyImage.Width);
                        r.Width -= (imagePadding.Horizontal + m_BodyImage.Width);
                    }

                    if (this.Text != "" && r.Width > 0 && r.Height > 0)
                    {
                        Rectangle textRect = new Rectangle(r.X + textPadding.Left, r.Y + textPadding.Top, r.Width - textPadding.Horizontal, r.Height - textPadding.Vertical);
                        if (m_BodyMarkup == null)
                        {
                            info.Bounds = textRect;
                            if (textRect.Width > 0 && textRect.Height > 0)
                            {
                                eTextFormat format = info.Style.TextFormat;
                                if (this.RightToLeft == RightToLeft.Yes) format |= eTextFormat.RightToLeft;
                                ElementStyleDisplay.PaintText(info, this.Text, font, false, format);
                            }
                        }
                        else if (textRect.Width > 0 && textRect.Height > 0)
                        {
                            TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, info.Style.TextColor,
                                (this.RightToLeft == RightToLeft.Yes), textRect, true);
                            m_BodyMarkup.Arrange(textRect, d);
                            m_BodyMarkup.Render(d);
                        }
                    }
                }
            }
            finally
            {
                headerFont.Dispose();
            }

#if !TRIAL
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(128, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
#endif
        }

        private Padding GetHeaderPadding()
        {
            return new Padding(6, 6, 2, 2);
        }

        private Padding GetFooterPadding()
        {
            return new Padding(6, 6, 4, 6);
        }

        private Padding GetTextPadding()
        {
            if ((m_HeaderText == "" || !HeaderVisible) && (m_FooterText == "" || !FooterVisible))
                return new Padding(1, 1, 1, 1);
            if(this.RightToLeft==RightToLeft.Yes)
                return new Padding(6, 14, 4, 4);
            else
                return new Padding(14, 6, 4, 4);
        }

        private Padding GetImagePadding()
        {
            return new Padding(6, 6, 6, 6);
        }

        /// <summary>
        /// Recalculates and set size of the control based on the content that is made available to it.
        /// </summary>
        public void RecalcSize()
        {
            this.Size = GetAutoSize();
        }

        /// <summary>
        /// Calculates the tooltip height based on the specified width.
        /// </summary>
        /// <param name="width">Tooltip width</param>
        /// <returns>Size of the tooltip based on specified width.</returns>
        public Size GetFixedWidthSize(int width)
        {
            Padding headerPadding = GetHeaderPadding();
            Padding footerPadding = GetFooterPadding();
            Padding textPadding = GetTextPadding();
            Padding imagePadding = GetImagePadding();

            Size size = Size.Empty;
            int innerWidth = width - ElementStyleLayout.HorizontalStyleWhiteSpace(this.Style);
            if (innerWidth <= 6) innerWidth = 16;
            Font font = this.Font;
            if (this.Style.Font != null)
                font = this.Style.Font;

            Font headerFont = new Font(font, (m_ShowTooltipDescription || m_HeaderText == "") ? FontStyle.Bold : FontStyle.Regular);
            Graphics g = this.CreateGraphics();

            if (this.AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            try
            {
                if (m_HeaderText != "" && HeaderVisible)
                {
                    Size headerSize = Size.Empty;
                    int w = innerWidth - headerPadding.Horizontal;
                    if (w <= 1) w = 2;

                    if (m_HeaderMarkup == null)
                    {
                        headerSize = TextDrawing.MeasureString(g, m_HeaderText, headerFont, w, eTextFormat.Default | eTextFormat.WordBreak);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft == RightToLeft.Yes));
                        m_HeaderMarkup.Measure(new Size(w, 1), d);
                        headerSize = m_HeaderMarkup.Bounds.Size;
                        m_HeaderMarkup.Arrange(new Rectangle(Point.Empty, headerSize), d);
                        headerSize = m_HeaderMarkup.Bounds.Size;
                    }
                    //headerSize.Width += 2;
                    headerSize.Height += 2;
                    //headerSize.Width += headerPadding.Horizontal;
                    headerSize.Height += headerPadding.Vertical;
                    //if (headerSize.Width > size.Width)
                    //    size.Width = headerSize.Width;
                    size.Height += headerSize.Height;
                }

                if (m_FooterText != "" && FooterVisible && (m_ShowTooltipDescription || m_HeaderText == ""))
                {
                    Size footerSize = Size.Empty;
                    int w = innerWidth - footerPadding.Horizontal;
                    if (m_FooterImage != null)
                        w-= (m_FooterImage.Width + m_FooterImageSpacing);
                    if (w <= 1) w = 2;

                    if (m_FooterMarkup == null)
                    {
                        footerSize = TextDrawing.MeasureString(g, m_FooterText, headerFont, w, eTextFormat.Default | eTextFormat.WordBreak);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft == RightToLeft.Yes));
                        m_FooterMarkup.Measure(new Size(w, 1), d);
                        footerSize = m_FooterMarkup.Bounds.Size;
                        m_FooterMarkup.Arrange(new Rectangle(Point.Empty, footerSize), d);
                        footerSize = m_FooterMarkup.Bounds.Size;
                    }
                    //footerSize.Width += 2;
                    footerSize.Height += 2;
                    if (m_FooterImage != null)
                    {
                        //footerSize.Width += (m_FooterImage.Width + m_FooterImageSpacing);
                        if (m_FooterImage.Height > footerSize.Height)
                            footerSize.Height = m_FooterImage.Height;
                    }
                    //footerSize.Width += footerPadding.Horizontal;
                    footerSize.Height += footerPadding.Vertical;
                    //if (footerSize.Width > size.Width)
                    //    size.Width = footerSize.Width;
                    size.Height += footerSize.Height;
                }

                if (m_ShowTooltipDescription || m_HeaderText == "")
                {
                    if (this.Text != "")
                    {
                        int textArea = innerWidth - textPadding.Horizontal;
                        if (m_BodyImage != null)
                            textArea -= (m_BodyImage.Width + imagePadding.Horizontal);

                        if (textArea <= 1)
                            textArea = 2;

                        Size textSize = Size.Empty;

                        if (m_BodyMarkup == null)
                        {
                            textSize = TextDrawing.MeasureString(g, this.Text, font, textArea, this.Style.TextFormat);
                        }
                        else
                        {
                            TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft == RightToLeft.Yes));
                            m_BodyMarkup.Measure(new Size(textArea, 1), d);
                            textSize = m_BodyMarkup.Bounds.Size;

                            if (textSize.Height > textSize.Width * 1.75)
                                textArea = (int)(textSize.Height * .75);

                            m_BodyMarkup.Arrange(new Rectangle(Point.Empty, new Size(textArea, 1)), d);
                            textSize = m_BodyMarkup.Bounds.Size;
                        }

                        //textSize.Width += textPadding.Horizontal;
                        textSize.Height += textPadding.Vertical;

                        if (m_BodyImage != null)
                        {
                            //textSize.Width += (m_BodyImage.Width + imagePadding.Horizontal);
                            if (m_BodyImage.Height + imagePadding.Vertical > textSize.Height)
                                textSize.Height = (m_BodyImage.Height + imagePadding.Vertical);
                        }

                        //if (textSize.Width > size.Width)
                        //    size.Width = textSize.Width;
                        size.Height += textSize.Height;
                    }
                    else
                    {
                        if (m_BodyImage != null)
                        {
                            //if (m_BodyImage.Width + imagePadding.Horizontal > size.Width)
                            //    size.Width = (m_BodyImage.Width + imagePadding.Horizontal);
                            size.Height += (m_BodyImage.Height + imagePadding.Vertical);
                        }
                    }
                }
            }
            finally
            {
                g.Dispose();
                headerFont.Dispose();
            }

            size.Width = width;
            size.Height += ElementStyleLayout.VerticalStyleWhiteSpace(this.Style);

            return size;
        }

        private Size GetAutoSize()
        {
            Padding headerPadding = GetHeaderPadding();
            Padding footerPadding = GetFooterPadding();
            Padding textPadding = GetTextPadding();
            Padding imagePadding = GetImagePadding();

            Size size = Size.Empty;

            Font font = this.Font;
            if (this.Style.Font != null)
                font = this.Style.Font;

            Font headerFont = new Font(font, (m_ShowTooltipDescription || m_HeaderText == "") ? FontStyle.Bold : FontStyle.Regular);
            Graphics g = this.CreateGraphics();

            if (this.AntiAlias)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

            try
            {
                if (m_HeaderText != "" && HeaderVisible)
                {
                    Size headerSize = Size.Empty;
                    if (m_HeaderMarkup == null)
                    {
                        headerSize = TextDrawing.MeasureString(g, m_HeaderText, headerFont);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d=new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft==RightToLeft.Yes));
                        m_HeaderMarkup.Measure(new Size(1024, 1), d);
                        headerSize = m_HeaderMarkup.Bounds.Size;
                        m_HeaderMarkup.Arrange(new Rectangle(Point.Empty, headerSize), d);
                        headerSize = m_HeaderMarkup.Bounds.Size;
                    }
					headerSize.Width+=2;
					headerSize.Height+=2;
                    headerSize.Width += headerPadding.Horizontal;
                    headerSize.Height += headerPadding.Vertical;
                    if (headerSize.Width > size.Width)
                        size.Width = headerSize.Width;
                    size.Height += headerSize.Height;
                }

                if (m_FooterText != "" && FooterVisible && (m_ShowTooltipDescription || m_HeaderText == ""))
                {
                    Size footerSize = Size.Empty;
                    if (m_FooterMarkup == null)
                    {
                        footerSize = TextDrawing.MeasureString(g, m_FooterText, headerFont);
                    }
                    else
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft == RightToLeft.Yes));
                        m_FooterMarkup.Measure(new Size(1024, 1), d);
                        footerSize = m_FooterMarkup.Bounds.Size;
                        m_FooterMarkup.Arrange(new Rectangle(Point.Empty, footerSize), d);
                        footerSize = m_FooterMarkup.Bounds.Size;
                    }
					footerSize.Width+=2;
					footerSize.Height+=2;
                    if (m_FooterImage != null)
                    {
                        footerSize.Width += (m_FooterImage.Width + m_FooterImageSpacing);
                        if (m_FooterImage.Height > footerSize.Height)
                            footerSize.Height = m_FooterImage.Height;
                    }
                    footerSize.Width += footerPadding.Horizontal;
                    footerSize.Height += footerPadding.Vertical;
                    if (footerSize.Width > size.Width)
                        size.Width = footerSize.Width;
                    size.Height += footerSize.Height;
                }

                if (m_ShowTooltipDescription || m_HeaderText == "")
                {
                    if (this.Text != "")
                    {
                        int textArea = size.Width;
                        if (textArea < m_MinimumTooltipSize.Width)
                            textArea = m_MinimumTooltipSize.Width;
                        if (m_MaximumWidth > 0)
                            textArea = 5000;

                        Size textSize = Size.Empty;

                        if (m_BodyMarkup == null)
                        {
                            textSize = TextDrawing.MeasureString(g, this.Text, font, textArea, this.Style.TextFormat);
                            if (textSize.Height > textSize.Width * 1.75 && textSize.Width > font.Height * 2)
                            {
                                textArea = (int)(textSize.Height * .75);
                                textSize = TextDrawing.MeasureString(g, this.Text, font, textArea, this.Style.TextFormat);
                            }
                        }
                        else
                        {
                            TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, Color.Black, (this.RightToLeft == RightToLeft.Yes));
                            m_BodyMarkup.Measure(new Size(textArea, 1), d);
                            textSize = m_BodyMarkup.Bounds.Size;

                            if (textSize.Height > textSize.Width * 1.75 && textSize.Width > font.Height * 2)
                                textArea = (int)(textSize.Height * .75);

                            m_BodyMarkup.Arrange(new Rectangle(Point.Empty, new Size(textArea, 1)), d);
                            textSize = m_BodyMarkup.Bounds.Size;
                        }

                        textSize.Width += textPadding.Horizontal;
                        textSize.Height += textPadding.Vertical;

                        if (m_BodyImage != null)
                        {
                            textSize.Width += (m_BodyImage.Width + imagePadding.Horizontal);
                            if (m_BodyImage.Height + imagePadding.Vertical > textSize.Height)
                                textSize.Height = (m_BodyImage.Height + imagePadding.Vertical);
                        }

                        if (textSize.Width > size.Width)
                            size.Width = textSize.Width;
                        size.Height += textSize.Height;
                    }
                    else
                    {
                        if (m_BodyImage != null)
                        {
                            if (m_BodyImage.Width + imagePadding.Horizontal > size.Width)
                                size.Width = (m_BodyImage.Width + imagePadding.Horizontal);
                            size.Height += (m_BodyImage.Height + imagePadding.Vertical);
                        }
                    }
                }
            }
            finally
            {
                g.Dispose();
                headerFont.Dispose();
            }

            size.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(this.Style);
            size.Height += ElementStyleLayout.VerticalStyleWhiteSpace(this.Style);

            if (size.Width < m_MinimumTooltipSize.Width)
                size.Width = m_MinimumTooltipSize.Width;
            if (size.Height < m_MinimumTooltipSize.Height)
                size.Height = m_MinimumTooltipSize.Height;

            return size;
        }

        private void ApplyColors(eTooltipColor c)
        {
            this.ResetStyle();
            this.ResetStyleMouseOver();
            this.ResetStyleMouseDown();

            ElementStyle style = this.Style;

            Color color1 = Color.Empty;
            Color color2 = Color.Empty;

            TypeDescriptor.GetProperties(style)["WordWrap"].SetValue(style, true);
            TypeDescriptor.GetProperties(style)["TextAlignment"].SetValue(style, eStyleTextAlignment.Near);
            TypeDescriptor.GetProperties(style)["TextLineAlignment"].SetValue(style, eStyleTextAlignment.Near);
            TypeDescriptor.GetProperties(style)["BackColorGradientAngle"].SetValue(style, 90);
            TypeDescriptor.GetProperties(style)["BorderColor"].SetValue(style, Color.DimGray);
            TypeDescriptor.GetProperties(style)["BorderWidth"].SetValue(style, 1);
            TypeDescriptor.GetProperties(style)["TextColor"].SetValue(style, Color.Black);
            TypeDescriptor.GetProperties(style)["Border"].SetValue(style, eStyleBorderType.Solid);
            TypeDescriptor.GetProperties(style)["CornerType"].SetValue(style, eCornerType.Rounded);
            TypeDescriptor.GetProperties(style)["CornerDiameter"].SetValue(style, 2);
            
            switch (c)
            {
                case eTooltipColor.System:
                    {
                        if (GlobalManager.Renderer is Office2007Renderer)
                        {
                            Office2007ColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                            color1 = ct.SuperTooltip.BackgroundColors.Start;
                            color2 = ct.SuperTooltip.BackgroundColors.End;
                            style.TextColor = ct.SuperTooltip.TextColor;
                        }
                        break;
                    }
                case eTooltipColor.Apple:
                    {
                        color1 = Color.FromArgb(232, 248, 224);
                        color2 = Color.FromArgb(173, 231, 146);
                        break;
                    }
                case eTooltipColor.Blue:
                    {
                        color1 = Color.FromArgb(221, 230, 247);
                        color2 = Color.FromArgb(138, 168, 228);
                        break;
                    }
                case eTooltipColor.BlueMist:
                    {
                        color1 = Color.FromArgb(227, 236, 243);
                        color2 = Color.FromArgb(155, 187, 210);
                        break;
                    }
                case eTooltipColor.Cyan:
                    {
                        color1 = Color.FromArgb(227, 236, 243);
                        color2 = Color.FromArgb(155, 187, 210);
                        break;
                    }
                case eTooltipColor.Green:
                    {
                        color1 = Color.FromArgb(234, 240, 226);
                        color2 = Color.FromArgb(183, 201, 151);
                        break;
                    }
                case eTooltipColor.Lemon:
                    {
                        color1 = Color.FromArgb(252, 253, 215);
                        color2 = Color.FromArgb(245, 249, 111);
                        break;
                    }
                case eTooltipColor.Magenta:
                    {
                        color1 = Color.FromArgb(243, 229, 236);
                        color2 = Color.FromArgb(213, 164, 187);
                        break;
                    }
                case eTooltipColor.Orange:
                    {
                        color1 = Color.FromArgb(252, 233, 217);
                        color2 = Color.FromArgb(246, 176, 120);
                        break;
                    }
                case eTooltipColor.Purple:
                    {
                        color1 = Color.FromArgb(234, 227, 245);
                        color2 = Color.FromArgb(180, 158, 222);
                        break;
                    }
                case eTooltipColor.PurpleMist:
                    {
                        color1 = Color.FromArgb(232, 227, 234);
                        color2 = Color.FromArgb(171, 156, 183);
                        break;
                    }
                case eTooltipColor.Red:
                    {
                        color1 = Color.FromArgb(249, 225, 226);
                        color2 = Color.FromArgb(238, 149, 151);
                        break;
                    }
                case eTooltipColor.Silver:
                    {
                        color1 = Color.FromArgb(225, 225, 232);
                        color2 = Color.FromArgb(149, 149, 170);
                        break;
                    }
                case eTooltipColor.Tan:
                    {
                        color1 = Color.FromArgb(248, 242, 226);
                        color2 = Color.FromArgb(232, 209, 153);
                        break;
                    }
                case eTooltipColor.Teal:
                    {
                        color1 = Color.FromArgb(205, 236, 240);
                        color2 = Color.FromArgb(78, 188, 202);
                        break;
                    }
                case eTooltipColor.Yellow:
                    {
                        color1 = Color.FromArgb(255, 244, 213);
                        color2 = Color.FromArgb(255, 216, 105);
                        break;
                    }
                case eTooltipColor.Gray:
                    {
                        color1 = Color.White;
                        color2 = ColorScheme.GetColor("E4E4F0");
                        break;
                    }
                case eTooltipColor.Office2003:
                    {
                        TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, Color.White);
                        TypeDescriptor.GetProperties(style)["BackColor2SchemePart"].SetValue(style, eColorSchemePart.MenuSide);
                        break;
                    }
                default:
                    {
                        color1 = Color.Empty;
                        color2 = Color.Empty;
                        break;
                    }
            }

            if(!color1.IsEmpty)
                TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, color1);
            if(!color2.IsEmpty)
                TypeDescriptor.GetProperties(style)["BackColor2"].SetValue(style, color2);

            this.SetRegion();
        }

        public void UpdateWithSuperTooltipInfo(SuperTooltipInfo info)
        {
            if (info.BodyText == null) info.BodyText = "";
            if (info.FooterText == null) info.FooterText = "";
            m_BodyImage = info.BodyImage;
            this.Text = info.BodyText;
            m_FooterImage = info.FooterImage;
            FooterText = info.FooterText;
            m_FooterVisible = info.FooterVisible;
            HeaderText = info.HeaderText;
            m_HeaderVisible = info.HeaderVisible;
            this.PredefinedColor = info.Color;
        }
        #endregion

        #region Popup Support
        /// <summary>
        /// Gets or sets whether tooltip control is popup tooltip or standard control. Default is false which means tooltip is popup style.
        /// </summary>
        public bool StandardControl
        {
            get { return m_StandardControl; }
            set { m_StandardControl = value; }
        }

        internal void UpdateSuperTooltipSize(SuperTooltipInfo info)
        {
            UpdateWithSuperTooltipInfo(info);
            if (info.CustomSize.Width > ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 && info.CustomSize.Height == 0)
                this.Size = GetFixedWidthSize(info.CustomSize.Width);
            else if (info.CustomSize.IsEmpty || info.CustomSize.Width < ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 ||
                info.CustomSize.Height < ElementStyleLayout.VerticalStyleWhiteSpace(GetStyle()) + 4)
            {
                this.RecalcSize();
                if (m_MaximumWidth > 0 && this.Size.Width > m_MaximumWidth)
                {
                    // Enforce maximum width
                    this.Size = GetFixedWidthSize(m_MaximumWidth);
                }
            }
            else
                this.Size = info.CustomSize;
        }
        
        /// <summary>
        /// Shows tooltip at specified screen coordinates.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="enforceScreenPosition">Indicates whether to enforce the screen position of tooltip if tooltip falls out of screen bounds.</param>
        public void ShowTooltip(SuperTooltipInfo info, int x, int y, bool enforceScreenPosition)
        {
            UpdateWithSuperTooltipInfo(info);
            if (info.CustomSize.Width > ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 && info.CustomSize.Height == 0)
                this.Size = GetFixedWidthSize(info.CustomSize.Width);
            else if (info.CustomSize.IsEmpty || info.CustomSize.Width < ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 ||
                info.CustomSize.Height < ElementStyleLayout.VerticalStyleWhiteSpace(GetStyle()) + 4)
            {
                this.RecalcSize();
                if (m_MaximumWidth > 0 && this.Size.Width > m_MaximumWidth)
                {
                    // Enforce maximum width
                    this.Size = GetFixedWidthSize(m_MaximumWidth);
                }
            }
            else
                this.Size = info.CustomSize;

            bool setLocation = true;

            if (enforceScreenPosition)
            {
                Point mousePosition = Control.MousePosition;
                ScreenInformation screen = BarFunctions.ScreenFromPoint(mousePosition);
                if (screen != null)
                {
                    Rectangle r = new Rectangle(x, y, this.Width, this.Height);
                    System.Drawing.Size layoutArea = screen.WorkingArea.Size;
                    layoutArea.Width -= (int)(layoutArea.Width * .2f);

                    if (r.Right > screen.WorkingArea.Right)
                        r.X=r.X - (r.Right - screen.WorkingArea.Right);
                    if (r.Bottom > screen.Bounds.Bottom)
                        r.Y = screen.Bounds.Bottom - r.Height;

                    if (r.Contains(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y))
                    {
                        // We have to move it out of mouse position
                        if (r.Height + System.Windows.Forms.Control.MousePosition.Y + 1 <= screen.WorkingArea.Height)
                            r.Y = System.Windows.Forms.Control.MousePosition.Y + SystemInformation.CursorSize.Height;
                        else
                            r.Y = System.Windows.Forms.Control.MousePosition.Y - r.Height - SystemInformation.CursorSize.Height;
                    }

                    this.Bounds = r;
                    setLocation = false;
                }
            }

            if (!this.IsHandleCreated)
                this.CreateControl();

            Point p = Point.Empty;
            if (setLocation)
            {
                p = new Point(x, y);
                this.Location = p;
            }
            else
                p = this.Location;

            if (NativeFunctions.ShowDropShadow)
            {
                if (m_DropShadow == null)
                {
                    m_DropShadow = new PopupShadow(NativeFunctions.AlphaBlendingSupported);
                    m_DropShadow.CreateControl();
                }
                m_DropShadow.Hide();
            }
            if (NativeFunctions.ShowDropShadow && Environment.OSVersion.Version.Major >= 5 && !NativeFunctions.IsTerminalSession())
            {
                NativeFunctions.AnimateWindow(this.Handle.ToInt32(), BarFunctions.ANIMATION_INTERVAL, NativeFunctions.AW_BLEND);
            }
            else
                NativeFunctions.SetWindowPos(this.Handle, NativeFunctions.HWND_TOP, 0, 0, 0, 0, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOSIZE | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE);
            if (m_DropShadow != null)
            {
                NativeFunctions.SetWindowPos(m_DropShadow.Handle, this.Handle.ToInt32(), p.X + 3, p.Y + 3, this.Width, this.Height, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
                m_DropShadow.UpdateShadow();
            }
        }

        /// <summary>
        /// Updates the popup shadow size and position if shadow is visible.
        /// </summary>
        public void UpdateShadow()
        {
            if (m_DropShadow != null)
            {
                NativeFunctions.SetWindowPos(m_DropShadow.Handle, this.Handle.ToInt32(), this.Left + 3, this.Top + 3, this.Width, this.Height, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE);
                m_DropShadow.UpdateShadow();
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (m_DropShadow != null)
            {
                NativeFunctions.SetWindowPos(m_DropShadow.Handle, NativeFunctions.HWND_TOP, this.Left + 3, this.Top + 3, 0, 0, NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOSIZE);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                if (!m_StandardControl)
                {
                    p.Style = unchecked((int)(WS_POPUP | WS_CLIPSIBLINGS | WS_CLIPCHILDREN));
                    p.ExStyle = (int)(WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
                    p.Caption = "";
                }
                return p;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!this.Visible && m_DropShadow != null)
            {
                m_DropShadow.Hide();
                m_DropShadow.Dispose();
                m_DropShadow = null;
            }
            base.OnVisibleChanged(e);
        }

        #endregion
    }

    #region Padding Class

    /// <summary>
    /// Represents class that holds padding information for user interface elements.
    /// </summary>
    [TypeConverter(typeof(PaddingConvertor))]
    public class Padding : INotifyPropertyChanged
    {
        #region Private variables

        /// <summary>
        /// Gets or sets padding on left side. Default value is 0
        /// </summary>
        private int _Left;

        /// <summary>
        /// Gets or sets padding on right side. Default value is 0
        /// </summary>
        private int _Right;

        /// <summary>
        /// Gets or sets padding on top side. Default value is 0
        /// </summary>
        private int _Top;

        /// <summary>
        /// Gets or sets padding on bottom side. Default value is 0
        /// </summary>
        private int _Bottom;

        #endregion

        /// <summary>
        /// Creates new instance of the class and initializes it.
        /// </summary>
        /// <param name="all">Padding for all sides</param>
        public Padding(int all)
        {
            _Left = all;
            _Right = all;
            _Top = all;
            _Bottom = all;
        }

        /// <summary>
        /// Creates new instance of the class and initializes it.
        /// </summary>
        /// <param name="left">Left padding</param>
        /// <param name="right">Right padding</param>
        /// <param name="top">Top padding</param>
        /// <param name="bottom">Bottom padding</param>
        public Padding(int left, int right, int top, int bottom)
        {
            _Left = left;
            _Right = right;
            _Top = top;
            _Bottom = bottom;
        }

        #region Public properties

        /// <summary>
        /// Gets amount of Top padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Top padding.")]
        [NotifyParentProperty(true)]
        public int Top
        {
            get { return (_Top); }
            set { _Top = value; OnPropertyChanged(new PropertyChangedEventArgs("Top")); }
        }

        /// <summary>
        /// Gets amount of Left padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Left padding.")]
        [NotifyParentProperty(true)]
        public int Left
        {
            get { return (_Left); }
            set { _Left = value; OnPropertyChanged(new PropertyChangedEventArgs("Left")); }
        }

        /// <summary>
        /// Gets amount of Bottom padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Bottom padding.")]
        [NotifyParentProperty(true)]
        public int Bottom
        {
            get { return (_Bottom); }
            set { _Bottom = value; OnPropertyChanged(new PropertyChangedEventArgs("Bottom")); }
        }

        /// <summary>
        /// Gets amount of Right padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Right padding.")]
        [NotifyParentProperty(true)]
        public int Right
        {
            get { return (_Right); }
            set { _Right = value; OnPropertyChanged(new PropertyChangedEventArgs("Right")); }
        }

        /// <summary>
        /// Gets amount of horizontal padding (Left+Right)
        /// </summary>
        [Browsable(false)]
        public int Horizontal
        {
            get { return (_Left + _Right); }
        }

        /// <summary>
        /// Gets amount of vertical padding (Top+Bottom)
        /// </summary>
        [Browsable(false)]
        public int Vertical
        {
            get { return (_Top + _Bottom); }
        }

        /// <summary>
        /// Gets whether Padding is empty.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get { return (_Left == 0 && _Right == 0 && _Top == 0 && _Bottom == 0); }
        }

        /// <summary>
        /// Gets or sets the padding for all sides.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int All
        {
            get
            {
                return _Top;
            }
            set
            {
                Top = value;
                Bottom = value;
                Left = value;
                Right = value;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eh = PropertyChanged;
            if (eh != null) eh(this, e);
        }
        #endregion
    }

    #endregion

    #region PaddingConvertor

    public class PaddingConvertor : ExpandableObjectConverter
    {
        public override bool CanConvertTo(
            ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (true);

            return (base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Padding pad = value as Padding;

                if (pad != null)
                {
                    return (String.Format("{0:D}, {1:D}, {2:D}, {3:D}",
                        pad.Bottom, pad.Left, pad.Right, pad.Top));
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }

        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return (true);

            return (base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] values = ((string)value).Split(',');

                if (values.Length != 4)
                    throw new ArgumentException("Invalid value to convert.");

                try
                {
                    int bottom = int.Parse(values[0]);
                    int left = int.Parse(values[1]);
                    int right = int.Parse(values[2]);
                    int top = int.Parse(values[3]);

                    Padding pad = new Padding(left, right, top, bottom);

                    return (pad);
                }
                catch (Exception exp)
                {
                    throw new ArgumentException("Invalid value to convert.");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    #endregion

}
