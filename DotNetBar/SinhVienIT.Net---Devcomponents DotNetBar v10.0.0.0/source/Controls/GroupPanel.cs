using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(GroupPanel), "Controls.GroupPanel.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.GroupPanelDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class GroupPanel : PanelControl, INonClientControl
    {
        #region Private Variables
        private NonClientPaintHandler m_NCPainter = null;
        private Image m_TitleImage = null;
        private eTitleImagePosition m_TitleImagePosition = eTitleImagePosition.Left;
        private bool m_DrawTitleBox = true;
        private bool m_IsShadowEnabled = false;
        #endregion

        #region Constructor
        public GroupPanel()
        {
            m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
            m_NCPainter.BeforeBorderPaint += new CustomNCPaintEventHandler(NCBeforeBorderPaint);
            m_NCPainter.AfterBorderPaint += new CustomNCPaintEventHandler(NCAfterBorderPaint);
        }
        #endregion

        #region Internal Implementation
        private ePanelColorTable _ColorTable = ePanelColorTable.Default;
        /// <summary>
        /// Gets or sets the panel color scheme.
        /// </summary>
        [DefaultValue(ePanelColorTable.Default), Category("Appearance"), Description("Indicates panel color scheme.")]
        public ePanelColorTable ColorTable
        {
            get { return _ColorTable; }
            set 
            {
                _ColorTable = value;
                SetColorTable(value);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            InvalidateNonClient();
            base.OnResize(e);
        }
        /// <summary>
        /// Gets or sets the image that appears in title with text.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Visual"), Description("Indicates image that appears in title with text.")]
        public Image TitleImage
        {
            get { return m_TitleImage; }
            set
            {
                m_TitleImage = value;
                RefreshTextClientRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the position of the title image. Default value is left.
        /// </summary>
        [Browsable(true), DefaultValue(eTitleImagePosition.Left), Category("Visual"), Description("Indicates position of the title image.")]
        public eTitleImagePosition TitleImagePosition
        {
            get { return m_TitleImagePosition; }
            set
            {
                m_TitleImagePosition = value;
                RefreshTextClientRectangle();
                this.Invalidate();
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        /// Gets or sets the scrollbar skining type when control is using Office 2007 style.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eScrollBarSkin ScrollbarSkin
        {
            get { return m_NCPainter.SkinScrollbars; }
            set { m_NCPainter.SkinScrollbars = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (m_NCPainter != null)
            {
                m_NCPainter.Dispose();
                //m_NCPainter = null;
            }
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_TitleImage);
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false), Category("Appearance"), Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }

        /// <summary>
        /// Gets or sets whether box around the title of the group is drawn. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("")]
        public bool DrawTitleBox
        {
            get { return m_DrawTitleBox; }
            set
            {
                if (m_DrawTitleBox != value)
                {
                    m_DrawTitleBox = value;
                    this.InvalidateNonClient();
                }
            }
        }

        /// <summary>
        /// Invalidates non-client area of the control.
        /// </summary>
        public void InvalidateNonClient()
        {
            if (!BarFunctions.IsHandleValid(this)) return;
            const int RDW_INVALIDATE = 0x0001;
            const int RDW_FRAME = 0x0400;
            NativeFunctions.RECT r = new NativeFunctions.RECT(0, 0, this.Width, this.Height);
            NativeFunctions.RedrawWindow(this.Handle, ref r, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME);
        }

        /// <summary>
        /// Paints insides of the control.
        /// </summary>
        /// <param name="e">Paint event arguments.</param>
        protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
        {
            Graphics g = e.Graphics;
            if (this.TextMarkupElement == null)
                RefreshTextClientRectangle();
            Rectangle r = this.DisplayRectangle;
#if FRAMEWORK20
            r.X -= this.Padding.Left;
            r.Y -= this.Padding.Top;
            r.Width += this.Padding.Horizontal;
            r.Height += this.Padding.Vertical;
#else
            r.X -= this.DockPadding.Left;
            r.Y -= this.DockPadding.Top;
            r.Width += this.DockPadding.Left + this.DockPadding.Right;
            r.Height += this.DockPadding.Top + this.DockPadding.Bottom;
#endif
            r.Inflate(2, 2);
            ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, g, r);
            info.RightToLeft = (this.RightToLeft == RightToLeft.Yes);
            ElementStyleDisplay.PaintBackground(info, false);
            if (style.BackgroundImage != null) ElementStyleDisplay.PaintBackgroundImage(info);
            if (!m_IsShadowEnabled) return;
            ShadowPaintInfo pi = new ShadowPaintInfo();
            pi.Graphics = g;
            pi.Size = 6;
            foreach (Control c in this.Controls)
            {
                if (!c.Visible || c.BackColor == Color.Transparent && !(c is GroupPanel)) continue;
                if (c is GroupPanel)
                {
                    GroupPanel p = c as GroupPanel;
                    pi.Rectangle = new Rectangle(c.Bounds.X, c.Bounds.Y + p.GetInternalClientRectangle().Y / 2, c.Bounds.Width, c.Bounds.Height - p.GetInternalClientRectangle().Y / 2);
                }
                else
                    pi.Rectangle = c.Bounds;
                ShadowPainter.Paint2(pi);
            }
        }

        private void NCAfterBorderPaint(object sender, CustomNCPaintEventArgs e)
        {
            Graphics g = e.Graphics;

            TextRenderingHint th = g.TextRenderingHint;
            SmoothingMode sm = g.SmoothingMode;
            if (this.AntiAlias)
            {
                g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            g.ResetClip();
            ElementStyle style = this.Style;

            if (!this.Enabled)
            {
                style = style.Copy();
                style.TextColor = GetColorScheme().ItemDisabledText;
            }

            if (m_DrawTitleBox && !m_TitleArea.IsEmpty)
            {
                DisplayHelp.FillRoundedRectangle(g, m_TitleArea, 2, style.BackColor, style.BackColor2, -90);
                DisplayHelp.DrawRoundedRectangle(g, this.Style.BorderColor, m_TitleArea, 2);
            }

            Rectangle rText = new Rectangle(m_NCPainter.ClientRectangle.X + 4, 1, this.ClientRectangle.Width - 8, m_NCPainter.ClientRectangle.Y - 1);
            if (m_TitleImage != null)
            {
                Size textSize = GetAutoSize(rText.Width);
                if (m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.No || m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.Yes)
                {
                    g.DrawImage(m_TitleImage, rText.X - 1, rText.Y, m_TitleImage.Width, m_TitleImage.Height);
                    rText.X += m_TitleImage.Width;
                    rText.Width -= m_TitleImage.Width;
                }
                else if (m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.No || m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.Yes)
                {
                    g.DrawImage(m_TitleImage, rText.Right - m_TitleImage.Width, rText.Y, m_TitleImage.Width, m_TitleImage.Height);
                    rText.Width -= m_TitleImage.Width;
                }
                else if (m_TitleImagePosition == eTitleImagePosition.Center)
                {
                    g.DrawImage(m_TitleImage, rText.X + (rText.Width - m_TitleImage.Width) / 2, rText.Y, m_TitleImage.Width, m_TitleImage.Height);
                }
                rText.Y = rText.Bottom - textSize.Height - 2;
            }

            // Paint text
            if (this.TextMarkupElement == null)
            {
                ElementStyleDisplayInfo info = new ElementStyleDisplayInfo(style, g, rText);
                info.RightToLeft = (this.RightToLeft == RightToLeft.Yes);
                ElementStyleDisplay.PaintText(info, this.Text, this.Font);
            }
            else
            {
                TextRenderingHint tr = g.TextRenderingHint;
                if (this.AntiAlias)
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, style.TextColor, (this.RightToLeft == RightToLeft.Yes), Rectangle.Empty, true);
                Rectangle r = this.TextMarkupElement.Bounds;
                if (style.TextAlignment == eStyleTextAlignment.Center)
                    this.TextMarkupElement.Bounds = new Rectangle(this.TextMarkupElement.Bounds.X + (rText.Width - this.TextMarkupElement.Bounds.Width) / 2, this.TextMarkupElement.Bounds.Y,
                        this.TextMarkupElement.Bounds.Width, this.TextMarkupElement.Bounds.Height);
                else if(style.TextAlignment == eStyleTextAlignment.Far && this.RightToLeft == RightToLeft.No || this.RightToLeft == RightToLeft.Yes && style.TextAlignment== eStyleTextAlignment.Near)
                    this.TextMarkupElement.Bounds = new Rectangle(rText.Right - this.TextMarkupElement.Bounds.Width, this.TextMarkupElement.Bounds.Y,
                        this.TextMarkupElement.Bounds.Width, this.TextMarkupElement.Bounds.Height);
                this.TextMarkupElement.Render(d);
                g.TextRenderingHint = tr;
                this.TextMarkupElement.Bounds = r;
            }

            g.TextRenderingHint = th;
            g.SmoothingMode = sm;
        }
        private Rectangle m_TitleArea = Rectangle.Empty;
        private void NCBeforeBorderPaint(object sender, CustomNCPaintEventArgs e)
        {
            m_TitleArea = Rectangle.Empty;
            // Exclude text area from border rendering
            if (this.Text != null && this.Text.Length > 0)
            {
                Size s = GetAutoSize(this.ClientRectangle.Width - 8);
                Rectangle r = new Rectangle(m_NCPainter.ClientRectangle.X  + 3, 0, s.Width, m_NCPainter.ClientRectangle.Y);
                Size availSize = new Size(m_NCPainter.ClientRectangle.Width - 8, m_NCPainter.ClientRectangle.Y);
                
                    if (m_TitleImage != null && (m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.Yes ||
                            m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.No))
                    {
                        //r.X -= m_TitleImage.Width;
                        availSize.Width -= m_TitleImage.Width;
                    }
                    else if (m_TitleImage != null && (m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.Yes ||
                            m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.No))
                    {
                        r.X += m_TitleImage.Width;
                        availSize.Width -= m_TitleImage.Width;
                    }
                //if (this.TextMarkupElement == null)
                {
                    if (this.Style.TextAlignment == eStyleTextAlignment.Center)
                    {
                        //r.X = m_NCPainter.ClientRectangle.X;
                        r.X += (availSize.Width - r.Width) / 2;
                    }
                    else if (this.Style.TextAlignment == eStyleTextAlignment.Far || this.RightToLeft == RightToLeft.Yes && this.Style.TextAlignment == eStyleTextAlignment.Near)
                    {
                        r.X = r.X + (availSize.Width - r.Width);
                    }
                }
                
                if (!r.IsEmpty)
                {
                    r.Inflate(2, 0);
                    r.Width += 3;
                }

                e.Graphics.SetClip(r, System.Drawing.Drawing2D.CombineMode.Exclude);
                m_TitleArea = r;
            }

            if (m_TitleImage != null)
            {
                Rectangle r = new Rectangle(m_NCPainter.ClientRectangle.X + 3, 0, m_TitleImage.Width, m_TitleImage.Height);
                if (m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.Yes ||
                    m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.No)
                {
                    r.X = m_NCPainter.ClientRectangle.Right - r.Width - 4;
                }
                else if (m_TitleImagePosition == eTitleImagePosition.Center)
                {
                    r.X = m_NCPainter.ClientRectangle.X;
                    r.X += (m_NCPainter.ClientRectangle.Width - r.Width) / 2;
                }

                e.Graphics.SetClip(r, System.Drawing.Drawing2D.CombineMode.Exclude);
                if (m_TitleArea.IsEmpty)
                    m_TitleArea = r;
                else
                    m_TitleArea = Rectangle.Union(r, m_TitleArea);
            }
        }

        /// <summary>
        /// Returns the size of the panel calculated based on the text assigned.
        /// </summary>
        /// <returns>Calculated size of the panel or Size.Empty if panel size cannot be calculated.</returns>
        private Size GetAutoSize(int preferedWidth)
        {
            Size size = Size.Empty;
            
            if (TextMarkupElement != null)
            {
                if (preferedWidth == 0)
                {
                    size = TextMarkupElement.Bounds.Size;
                }
                else
                {
                    size = GetMarkupSize(preferedWidth);
                }
                size.Width += 4;
                //size.Height += 1;
            }
            else if (this.Text.Length > 0)
            {
                Font font = this.Font;
                if (this.Style.Font != null) font = this.Style.Font;
                eTextFormat tf = eTextFormat.Default | eTextFormat.SingleLine;
                using (Graphics g = BarFunctions.CreateGraphics(this))
                {
                    if (preferedWidth <= 0)
                        size = TextDrawing.MeasureString(g, this.Text, font, 0, tf);
                    else
                        size = TextDrawing.MeasureString(g, this.Text, font, preferedWidth, tf);
                }
                size.Width += 2;
                size.Height += 2;
            }

            if (size.IsEmpty) return size;

            size.Width += this.Style.MarginLeft + this.Style.MarginRight;
            size.Height += this.Style.MarginTop + this.Style.MarginBottom;

            return size;
        }

        private Size GetMarkupSize(int proposedWidth)
        {
            Size size = Size.Empty;
            if (TextMarkupElement != null)
            {
                Rectangle r = new Rectangle(0, 0, proposedWidth, 500);
                r.Inflate(-2, -2);
                Graphics g = this.CreateGraphics();
                TextMarkup.BodyElement markup = TextMarkup.MarkupParser.Parse(this.Text);
                try
                {
                    if (AntiAlias)
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                    }
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.Font, SystemColors.Control, (this.RightToLeft == RightToLeft.Yes));
                    markup.Measure(r.Size, d);
                    size = markup.Bounds.Size;
                }
                finally
                {
                    g.Dispose();
                }
            }

            return size;
        }

        protected override void RefreshTextClientRectangle()
        {
            if (m_NCPainter != null)
            {
                Rectangle r = new Rectangle(m_NCPainter.ClientRectangle.X, 0, m_NCPainter.ClientRectangle.Width, this.Height / 2);
                r.Inflate(-2, 0);
                if (m_TitleImage != null)
                {
                    if (m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.No ||
                        m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.Yes)
                    {
                        r.X += m_TitleImage.Width;
                        r.Width -= m_TitleImage.Width;
                    }
                    else if (m_TitleImagePosition == eTitleImagePosition.Left && this.RightToLeft == RightToLeft.Yes ||
                        m_TitleImagePosition == eTitleImagePosition.Right && this.RightToLeft == RightToLeft.No)
                    {
                        r.Width -= m_TitleImage.Width;
                    }
                    Size s = GetMarkupSize(r.Width);
                    r.Y = Math.Max(0, m_NCPainter.ClientRectangle.Y - s.Height - 4);
                }
                this.ClientTextRectangle = r;
            }
            else
                this.ClientTextRectangle = this.ClientRectangle;
            
            ResizeMarkup();
        }

        /// <summary>
        /// Gets or sets whether text rectangle painted on panel is considering docked controls inside the panel. 
        /// </summary>
        [Browsable(false), DefaultValue(true), Category("Appearance"), Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
        public override bool TextDockConstrained
        {
            get { return base.TextDockConstrained; }
            set { base.TextDockConstrained = value; }

        }

        /// <summary>
        /// Gets or sets whether panel automatically provides shadows for child controls. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether panel automatically provides shadows for child controls.")]
        public bool IsShadowEnabled
        {
            get { return m_IsShadowEnabled; }
            set
            {
                if (m_IsShadowEnabled != value)
                {
                    m_IsShadowEnabled = value;
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// Applies color scheme to the panel.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetColorTable(ePanelColorTable colorScheme)
        {
            GroupPanel p = this;
            DevComponents.DotNetBar.Rendering.ColorFactory factory = DevComponents.DotNetBar.Rendering.ColorFactory.Empty;
            p.CanvasColor = SystemColors.Control;
            p.ResetStyle();
            p.ColorSchemeStyle = eDotNetBarStyle.Office2007;

            if (colorScheme == ePanelColorTable.Default)
                SetDefaultPanelStyle();
            else if (colorScheme == ePanelColorTable.Green)
            {
                p.Style.BackColor2 = factory.GetColor(0x9CBF8B);
                p.Style.BackColorGradientAngle = 90;
                p.Style.BackColor = factory.GetColor(0xC3D9B9);
                p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                p.Style.BorderWidth = 1;
                p.Style.BorderColor = factory.GetColor(0x72A45A);
                p.Style.CornerDiameter = 4;
                p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
                p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                p.Style.TextColor = factory.GetColor(0x3C4A1F);
                p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            }
            else if (colorScheme == ePanelColorTable.Orange)
            {
                p.Style.BackColor = factory.GetColor(0xFAC08F);
                p.Style.BackColor2 = factory.GetColor(0xF79646);
                p.Style.BorderColor = factory.GetColor(0x974806);
                p.Style.TextColor = factory.GetColor(0x7F3D06);
                p.Style.BackColorGradientAngle = 90;
                p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                p.Style.BorderWidth = 1;
                p.Style.CornerDiameter = 4;
                p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
                p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            }
            else if (colorScheme == ePanelColorTable.Red)
            {
                p.Style.BackColor = factory.GetColor(0xE5BFBF);
                p.Style.BackColor2 = factory.GetColor(0xD39696);
                p.Style.BorderColor = factory.GetColor(0x953734);
                p.Style.TextColor = factory.GetColor(0x632423);
                p.Style.BackColorGradientAngle = 90;
                p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                p.Style.BorderWidth = 1;
                p.Style.CornerDiameter = 4;
                p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
                p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            }
            else if (colorScheme == ePanelColorTable.Yellow)
            {
                p.Style.BackColor = factory.GetColor(0xFFF3B2);
                p.Style.BackColor2 = factory.GetColor(0xFAD945);
                p.Style.BorderColor = factory.GetColor(0xEE9311);
                p.Style.TextColor = factory.GetColor(0x3F3F00);
                p.Style.BackColorGradientAngle = 90;
                p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                p.Style.BorderWidth = 1;
                p.Style.CornerDiameter = 4;
                p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
                p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            }
            else if (colorScheme == ePanelColorTable.Magenta)
            {
                p.Style.BackColor = factory.GetColor(0xEF91B4);
                p.Style.BackColor2 = factory.GetColor(0xE66896);
                p.Style.BorderColor = factory.GetColor(0xB12753);
                p.Style.TextColor = factory.GetColor(0x8E2648);
                p.Style.BackColorGradientAngle = 90;
                p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                p.Style.BorderWidth = 1;
                p.Style.CornerDiameter = 4;
                p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
                p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            }
        }

        /// <summary>
        /// Applies default group panel style to the control.
        /// </summary>
        public void SetDefaultPanelStyle()
        {
            GroupPanel p = this;

            p.CanvasColor = SystemColors.Control;
            p.ResetStyle();
            p.ColorSchemeStyle = eDotNetBarStyle.Office2007;
            p.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            p.Style.BackColorGradientAngle = 90;
            p.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            p.Style.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
            p.Style.BorderWidth = 1;
            p.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            p.Style.CornerDiameter = 4;
            p.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            p.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            p.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            p.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
        }
        #endregion

        #region INonClientControl Members

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m_NCPainter == null)
            {
                base.WndProc(ref m);
                return;
            }

            if (m.Msg == (int)WinApi.WindowsMessages.WM_HSCROLL || m.Msg == (int)WinApi.WindowsMessages.WM_VSCROLL ||
                m.Msg == (int)WinApi.WindowsMessages.WM_MOUSEWHEEL)
            {
                //Region reg = new Region(new Rectangle(0, 0, this.Width, this.Height));
                //SuspendPaint = true;
                //try
                //{
                bool callBase = m_NCPainter.WndProc(ref m);
                if (callBase)
                    BaseWndProc(ref m);
                //}
                //finally
                //{
                //SuspendPaint = false;
                //}
                //foreach (Control c in this.Controls)
                //{
                //    if (c.Visible)
                //        reg.Exclude(c.Bounds);
                //}
                RefreshTextClientRectangle();
                //this.Invalidate(reg, false);
                //if (this.Controls.Count > 0)
                //    this.Update();
                //reg.Dispose();
            }
            else
            {
                bool callBase = m_NCPainter.WndProc(ref m);

                if (callBase)
                    BaseWndProc(ref m);
            }
        }

        void INonClientControl.BaseWndProc(ref Message m)
        {
            BaseWndProc(ref m);
        }

        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            return null;
        }

        ItemPaintArgs INonClientControl.GetItemPaintArgs(System.Drawing.Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(this as IOwner, this, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false; // m_DesignerSelection;
            pa.GlassEnabled = !this.DesignMode && WinApi.IsGlassEnabled;
            return pa;
        }

        ElementStyle INonClientControl.BorderStyle
        {
            get { return this.Style; }
        }

        void INonClientControl.PaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        IntPtr INonClientControl.Handle
        {
            get { return this.Handle; }
        }

        int INonClientControl.Width
        {
            get { return this.Width; }
        }

        int INonClientControl.Height
        {
            get { return this.Height; }
        }

        bool INonClientControl.IsHandleCreated
        {
            get { return this.IsHandleCreated; }
        }

        System.Drawing.Point INonClientControl.PointToScreen(System.Drawing.Point client)
        {
            return this.PointToScreen(client);
        }

        System.Drawing.Color INonClientControl.BackColor
        {
            get { return this.BackColor; }
        }

        void DevComponents.DotNetBar.Controls.INonClientControl.RenderNonClient(Graphics g) { }

        void DevComponents.DotNetBar.Controls.INonClientControl.AdjustClientRectangle(ref Rectangle r)
        {
            Size textSize = GetAutoSize(r.Width);
            if (m_TitleImage != null)
            {
                textSize.Height = Math.Max(m_TitleImage.Height, textSize.Height);
            }
            if (textSize.Height > r.Height) textSize.Height = r.Height - 8;
            r.Y += textSize.Height;
            r.Height -= textSize.Height;
        }

        void INonClientControl.AdjustBorderRectangle(ref Rectangle r)
        {
            if (this.Text != "")
            {
                int h = GetNonClientTopHeight();
                r.Y += h;
                r.Height -= h;
            }
        }

        private int GetNonClientTopHeight()
        {
            Font f = this.Font;
            if (this.Style.Font != null)
                f = this.Style.Font;
            int h = (int)(f.Height * .7f);
            if (m_TitleImage != null)
            {
                h = Math.Max(m_TitleImage.Height - (f.Height - h - 1), h);
            }
            return h;
        }
        internal Rectangle GetInternalClientRectangle()
        {
            return m_NCPainter.ClientRectangle;
        }
        #endregion
    }
    /// <summary>
    /// Defines predefined color schemes for panel control.
    /// </summary>
    public enum ePanelColorTable
    {
        Default,
        Green,
        Orange,
        Red,
        Yellow,
        Magenta
    }
}
