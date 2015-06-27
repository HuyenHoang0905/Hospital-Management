using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents a progress bar item.
    /// </summary>
    [ToolboxItem(false), Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class ProgressBarItem : ImageItem, IPersonalizedMenuItem
    {
        // IPersonalizedMenuItem Implementation
        private eMenuVisibility m_MenuVisibility = eMenuVisibility.VisibleAlways;
        private bool m_RecentlyUsed = false;
        private int m_Maximum = 100;
        private int m_Minimum = 0;
        private int m_Value = 0;
        private int m_Step = 1;
        private bool m_TextVisible = false;
        public event EventHandler ValueChanged;
        private int m_Width = 96;
        private int m_Height = 0;
        //private ItemStyle m_BackgroundStyle = new ItemStyle();
        private ElementStyle m_BackgroundStyle = null;
        private Color m_ChunkColor = Color.Empty;
        private Color m_ChunkColor2 = Color.Empty;
        private float m_ChunkGradientAngle = 0;
        private eProgressItemType m_ProgressType = eProgressItemType.Standard;
        private int m_MarqueeAnimationSpeed = 100;
        private int m_MarqueeValue = 0;
        private eProgressBarItemColor m_ColorTable = eProgressBarItemColor.Normal;

        /// <summary>
        /// Creates new instance of ProgressBarItem.
        /// </summary>
        public ProgressBarItem() : this("", "") { }
        /// <summary>
        /// Creates new instance of ProgressBarItem and assigns the name to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        public ProgressBarItem(string sItemName) : this(sItemName, "") { }
        /// <summary>
        /// Creates new instance of ProgressBarItem and assigns the name and text to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        /// <param name="ItemText">item text.</param>
        public ProgressBarItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            ResetBackgroundStyle();
        }
        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            ProgressBarItem objCopy = new ProgressBarItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the ProgressBarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ProgressBarItem instance.</param>
        internal void InternalCopyToItem(ProgressBarItem copy)
        {
            CopyToItem(copy);
        }

        /// <summary>
        /// Copies the ProgressBarItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ProgressBarItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            ProgressBarItem objCopy = copy as ProgressBarItem;
            base.CopyToItem(objCopy);

            objCopy.Minimum = this.Minimum;
            objCopy.Maximum = this.Maximum;
            objCopy.Step = this.Step;
            objCopy.TextVisible = this.TextVisible;
            objCopy.Width = this.Width;
            objCopy.Style = this.Style;
            objCopy.Value = this.Value;

            objCopy.ChunkColor = ChunkColor;
            objCopy.ChunkColor2 = ChunkColor2;
            objCopy.ChunkGradientAngle = ChunkGradientAngle;
            objCopy.ColorTable = ColorTable;
        }

        private Brush GetChunkBrush(Rectangle rect, ColorDescription cd)
        {
            if (cd.ChunkColor2.IsEmpty)
                return new SolidBrush(cd.ChunkColor);
            
            return DisplayHelp.CreateLinearGradientBrush(rect, cd.ChunkColor, cd.ChunkColor2, cd.ChunkGradientAngle);
        }

        /// <summary>
        /// Overriden. Draws the item.
        /// </summary>
        /// <param name="g">Target Graphics object.</param>
        public override void Paint(ItemPaintArgs pa)
        {
            const float chunkFraction = .65f;
            const int chunkSpacing = 2;

            if (this.SuspendLayout)
                return;

            if (this.IsThemed && PaintThemed(pa))
                return;

            ColorDescription cd = GetColorDescription();
            cd.BackgroundStyle.SetColorScheme(pa.Colors);
            
            Graphics g = pa.Graphics;

            Font font = this.GetFont();

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                Rendering.BaseRenderer renderer = pa.Renderer;
                if (renderer != null)
                {
                    renderer.DrawProgressBarItem(new ProgressBarItemRenderEventArgs(pa.Graphics, this, pa.Font, pa.RightToLeft));
                }
            }
            else
            {
                Rectangle rect = m_Rect;

                //rect.Inflate(-1, 0);

                // Paint Background
                ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(cd.BackgroundStyle, g, rect));
                
                // Paint progress
                rect.Inflate(-ElementStyleLayout.HorizontalStyleWhiteSpace(cd.BackgroundStyle), -ElementStyleLayout.VerticalStyleWhiteSpace(cd.BackgroundStyle));

                switch (this.EffectiveStyle)
                {
                    case eDotNetBarStyle.OfficeXP:
                        {
                            Rectangle origRect = rect;
                            // Bar progress
                            int chunkWidth = (int)Math.Floor(rect.Height * chunkFraction);
                            if (chunkWidth <= 0)
                                chunkWidth = 2;
                            int chunk100 = (int)Math.Ceiling((double)rect.Width / (chunkWidth + chunkSpacing));
                            Region oldClip = g.Clip;
                            g.SetClip(rect, CombineMode.Intersect);
                            int chunkCount = (int)(chunk100 * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));

                            int startX = rect.X;
                            if (m_ProgressType == eProgressItemType.Marquee)
                            {
                                chunkCount = 5;
                                startX += rect.Width * m_MarqueeValue / 100 - (int)(chunkWidth * chunkCount * .65);
                            }

                            using(Brush brush = GetChunkBrush(origRect, cd))
                            {
                                int x = startX;
                                for (int i = 0; i < chunkCount; i++)
                                {
                                    g.FillRectangle(brush, x, rect.Y, chunkWidth, rect.Height);
                                    x += (chunkWidth + chunkSpacing);
                                    if (m_ProgressType == eProgressItemType.Marquee && x > rect.Right)
                                        x = rect.X;
                                }
                            }

                            g.Clip = oldClip;
                            break;
                        }
                    case eDotNetBarStyle.Office2000:
                        {
                            // Simple Progress Bar
                            Region oldClip = g.Clip;
                            g.SetClip(rect, CombineMode.Intersect);

                            Rectangle origRect = rect;
                            if (m_ProgressType == eProgressItemType.Marquee)
                            {
                                rect.Width = (int)(rect.Width * .29);
                                rect.X += origRect.Width * m_MarqueeValue / 100 - (int)(rect.Width/2);
                            }
                            else
                                rect.Width = (int)(rect.Width * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));

                            using(Brush brush = GetChunkBrush(origRect, cd))
                            {
                                g.FillRectangle(brush, rect);
                                if (m_ProgressType == eProgressItemType.Marquee && rect.Right > origRect.Right + 4)
                                {
                                    rect = new Rectangle(origRect.X, origRect.Y, rect.Right - origRect.Right - 4, origRect.Height);
                                    g.FillRectangle(brush, rect);
                                }
                            }

                            g.Clip = oldClip;
                            break;
                        }
                    case eDotNetBarStyle.Office2003:
                    case eDotNetBarStyle.VS2005:
                        {
                            // Bar progress
                            int chunkWidth = (int)Math.Floor(rect.Height * chunkFraction);
                            if (chunkWidth <= 0)
                                chunkWidth = 2;
                            int chunk100 = (int)Math.Ceiling((double)rect.Width / (chunkWidth + chunkSpacing));
                            Region oldClip = g.Clip;
                            g.SetClip(rect, CombineMode.Intersect);
                            int chunkCount = (int)(chunk100 * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));

                            int startX = rect.X;
                            if (m_ProgressType == eProgressItemType.Marquee)
                            {
                                chunkCount = 5;
                                startX += rect.Width * m_MarqueeValue / 100 - (int)(chunkWidth * chunkCount * .65);
                            }

                            Brush brush = null;
                            if (cd.ChunkColor2.IsEmpty)
                                brush = new SolidBrush(cd.ChunkColor);
                            else
                            {
                                LinearGradientBrush lg = DisplayHelp.CreateLinearGradientBrush(new Rectangle(0, 0, chunkWidth, rect.Height), cd.ChunkColor, cd.ChunkColor2, cd.ChunkGradientAngle);
                                lg.SetBlendTriangularShape(.1f);
                                brush = lg;
                            }
                            try
                            {
                                int x = startX;
                                for (int i = 0; i < chunkCount; i++)
                                {
                                    g.FillRectangle(brush, x, rect.Y, chunkWidth, rect.Height);
                                    x += (chunkWidth + chunkSpacing);
                                    if (m_ProgressType == eProgressItemType.Marquee && x > rect.Right)
                                        x = rect.X;
                                }
                            }
                            finally
                            {
                                brush.Dispose();
                            }
                            
                            g.Clip = oldClip;
                            break;
                        }
                }
            }

            // Paint Text On Top
            if (m_TextVisible)
                ElementStyleDisplay.PaintText(new ElementStyleDisplayInfo(cd.BackgroundStyle, g, m_Rect), m_Text, font);

            if (this.DesignMode && this.Focused)
            {
                Rectangle r = m_Rect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
            }
        }

        private bool PaintThemed(ItemPaintArgs pa)
        {
            if (this.SuspendLayout)
                return true;
            if (m_NeedRecalcSize)
                RecalcSize();

            System.Drawing.Graphics g = pa.Graphics;

            ThemeProgress theme = pa.ThemeProgress;
            if (theme == null)
                return false;
            ThemeProgressParts part = ThemeProgressParts.Bar;

            Rectangle r = m_Rect;
            r.Inflate(-1, -1);
            theme.DrawBackground(g, part, ThemeProgressStates.Normal, r);

            part = ThemeProgressParts.Chunk;
            r.Width = (int)(r.Width * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));
            if (r.Width > 5)
            {
                r.Inflate(-4, -4);
                theme.DrawBackground(g, part, ThemeProgressStates.Normal, r);
            }

            if (this.DesignMode && this.Focused)
            {
                r = m_Rect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
            }

            return true;
        }

        protected virtual Font GetFont()
        {
            // TODO: Check performance implications of cloning the font all the time instead of
            // getting the stored copy all the time
            System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
            if (objCtrl != null)
                return (Font)objCtrl.Font;
            return (Font)System.Windows.Forms.SystemInformation.MenuFont;
        }

        /// <summary>
        /// Overriden. Recalculates the size of the item.
        /// </summary>
        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;

            int height = m_Height, width = m_Width;

            if ((m_Width == 0 || m_Height == 0) && m_TextVisible)
            {
                // Auto-size item
                System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                if (!IsHandleValid(objCtrl))
                    return;

                Graphics g = BarFunctions.CreateGraphics(objCtrl);
                try
                {
                    ElementStyle backgroundStyle = GetBackgroundStyle();
                    // Measure string
                    Font objCurrentFont = GetFont();

                    Size objStringSize = Size.Empty;
                    eTextFormat objStringFormat = backgroundStyle.TextFormat;

                    if (m_Text != "")
                    {
                        objStringSize = TextDrawing.MeasureString(g, m_Text, objCurrentFont, 0, objStringFormat);
                        objStringSize.Width += 4;
                    }
                    else
                    {
                        objStringSize = TextDrawing.MeasureString(g, " ", objCurrentFont, 0, objStringFormat);
                        objStringSize.Width += 4;
                    }
                    if (backgroundStyle.Border != eStyleBorderType.None)
                        objStringSize.Width += 4;

                    objStringSize.Height += 6;

                    if (m_Width == 0)
                        width = (int)objStringSize.Width;
                    if (m_Height == 0)
                        height = (int)objStringSize.Height;
                }
                finally
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    g.SmoothingMode = SmoothingMode.Default;
                    g.Dispose();
                }
            }
            else
            {
                if (m_Height == 0)
                    height = this.ImageSize.Height;
                else
                    height = m_Height;
                if (m_Width == 0)
                    width = 96;
                else
                    width = m_Width;
            }

            if (this.Orientation == eOrientation.Horizontal)
            {
                this.HeightInternal = height;
                this.WidthInternal = width;
            }
            else
            {
                this.HeightInternal = width;
                this.WidthInternal = height;
            }

            base.RecalcSize();
        }

        /// <summary>
        /// Gets or sets the item background style.
        /// </summary>
        [Obsolete("BackgroundStyle property is replaced with the BackStyle property"), Browsable(false), Category("Style"), Description("Gets or sets the background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemStyle BackgroundStyle
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Specifies the background style of the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets bar background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackStyle
        {
            get { return m_BackgroundStyle; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Null is not valid value for this property.");
                m_BackgroundStyle = value;
            }
        }

        /// <summary>
        /// Resets style to default value. Used by windows forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackgroundStyle()
        {
            if(m_BackgroundStyle!=null)
                m_BackgroundStyle.StyleChanged -= new EventHandler(this.VisualPropertyChanged);
            m_BackgroundStyle = new ElementStyle();
            m_BackgroundStyle.StyleChanged += new EventHandler(this.VisualPropertyChanged);
            this.Refresh();
        }

        private void VisualPropertyChanged(object sender, EventArgs e)
        {
            this.OnAppearanceChanged();
        }

        private class ColorDescription
        {
            public ElementStyle BackgroundStyle = null;
            public Color ChunkColor = Color.Empty;
            public Color ChunkColor2 = Color.Empty;
            public int ChunkGradientAngle = 0;
            public ColorDescription() { }
            public ColorDescription(ElementStyle style, Color chunkColor, Color chunkColor2, int chunkGradientAngle)
            {
                this.BackgroundStyle = style;
                this.ChunkColor = chunkColor;
                this.ChunkColor2 = chunkColor2;
                this.ChunkGradientAngle = chunkGradientAngle;
            }
        }

        private ColorDescription GetColorDescription()
        {
            ElementStyle style = null;
            Color chunkColor = Color.Empty;
            Color chunkColor2 = Color.Empty;
            int chunkGradientAngle = (int)m_ChunkGradientAngle;
            if (m_BackgroundStyle.Custom)
                style = m_BackgroundStyle;
            else
                style = GetBackgroundStyle();

            if (!m_ChunkColor.IsEmpty)
            {
                chunkColor = m_ChunkColor;
                chunkColor2 = m_ChunkColor2;
            }
            else
            {
                switch (this.EffectiveStyle)
                {
                    case eDotNetBarStyle.Office2000:
                    case eDotNetBarStyle.OfficeXP:
                        {
                            chunkColor = SystemColors.Highlight;
                            chunkColor2 = Color.Empty;
                            break;
                        }
                    default:
                        {
                            ColorScheme scheme = GetColorScheme();
                            chunkGradientAngle = 90;
                            chunkColor = scheme.ItemPressedBackground;
                            chunkColor2 = scheme.ItemPressedBackground2;
                            break;
                        }
                }
            }

            if (this.Orientation == eOrientation.Vertical)
                style.TextOrientation = eOrientation.Vertical;
            else
                style.TextOrientation = eOrientation.Horizontal;

            return new ColorDescription(style, chunkColor, chunkColor2, chunkGradientAngle);
        }

        private ColorScheme GetColorScheme()
        {
            ColorScheme scheme = null;
            Control cc = this.ContainerControl as Control;
            if (cc is Bar)
                scheme = ((Bar)cc).ColorScheme;
            else if (cc is MenuPanel)
                scheme = ((MenuPanel)cc).ColorScheme;
            else if (cc is ItemControl)
                scheme = ((ItemControl)cc).ColorScheme;
            else if (cc is BaseItemControl)
                scheme = ((BaseItemControl)cc).ColorScheme;

            if (scheme == null)
                scheme = new ColorScheme(this.EffectiveStyle);

            return scheme;
        }
        private ElementStyle m_CachedStyle = null;
        private ElementStyle GetBackgroundStyle()
        {
            if (m_CachedStyle != null)
                return m_CachedStyle;

            ElementStyle style = new ElementStyle();

            if (this.EffectiveStyle == eDotNetBarStyle.Office2000 || this.EffectiveStyle == eDotNetBarStyle.OfficeXP)
            {
                style.Border = eStyleBorderType.Solid;
                style.BorderWidth = 1;
                style.BorderColor = SystemColors.ControlDark;
                style.BackColor = SystemColors.Control;
                style.TextColor = SystemColors.ControlText;
            }
            else if (this.EffectiveStyle == eDotNetBarStyle.Office2003 || this.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                ColorScheme scheme = null;
                if (this.ContainerControl is Bar)
                    scheme = ((Bar)this.ContainerControl).ColorScheme;
                else if (this.ContainerControl is MenuPanel)
                    scheme = ((MenuPanel)this.ContainerControl).ColorScheme;
                if (scheme == null)
                    scheme = new ColorScheme(this.EffectiveStyle);

                style.Border = eStyleBorderType.Solid;
                style.BorderWidth = 1;
                style.BorderColorSchemePart = eColorSchemePart.BarDockedBorder;
                style.BackColorSchemePart = eColorSchemePart.BarBackground;
                style.BackColor2SchemePart = eColorSchemePart.BarBackground2;
                style.BackColorGradientAngle = 90;
                style.TextColorSchemePart = eColorSchemePart.ItemPressedText;
            }
            style.TextLineAlignment = eStyleTextAlignment.Center;
            style.TextAlignment = eStyleTextAlignment.Center;
            m_CachedStyle = style;

            return m_CachedStyle;
        }

        protected override void OnStyleChanged()
        {
            m_CachedStyle = null;
            base.OnStyleChanged();
        }

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the maximum value of the range of the control."), Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return m_Maximum;
            }
            set
            {
                m_Maximum = value;
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the minimum value of the range of the control."), Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return m_Minimum;
            }
            set
            {
                m_Minimum = value;
                this.Refresh();
                OnAppearanceChanged();
            }
        }
        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the current position of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Value
        {
            get { return m_Value; }
            set
            {
                int oldValue = m_Value;

                if (value < m_Minimum)
                    m_Value = m_Minimum;
                else if (value > m_Maximum)
                    m_Value = m_Maximum;
                else
                    m_Value = value;
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

                if (GetNeedVisualUpdate(oldValue, m_Value))
                {
                    this.Refresh();
                    OnAppearanceChanged();
                    Control c = this.ContainerControl as Control;
                    if (c != null) c.Update();
                }
            }
        }

        private bool GetNeedVisualUpdate(int oldValue, int newValue)
        {
            int oldW = (int)(this.WidthInternal * ((float)oldValue / (m_Maximum - m_Minimum)));
            int newW = (int)(this.WidthInternal * ((float)newValue / (m_Maximum - m_Minimum)));
            return oldW != newW;
        }

        /// <summary>
        /// Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar."), Category("Behavior"), DefaultValue(1)]
        public int Step
        {
            get
            {
                return m_Step;
            }
            set
            {
                m_Step = value;
            }
        }

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the Step property.
        /// </summary>
        public void PerformStep()
        {
            this.Value += m_Step;
        }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position. </param>
        public void Increment(int value)
        {
            this.Value += value;
        }

        /// <summary>
        /// Gets or sets whether the text inside the progress bar is displayed.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Description("Gets or sets whether the text inside the progress bar is displayed."), Category("Behavior"), DefaultValue(false)]
        public bool TextVisible
        {
            get
            {
                return m_TextVisible;
            }
            set
            {
                m_TextVisible = value;
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the width of the item in pixels. 0 value indicates the auto-sizing item based on the text contained in it.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(96), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates the width of the label in pixels.")]
        public int Width
        {
            get
            {
                return m_Width;
            }
            set
            {
                if (m_Width == value)
                    return;
                m_Width = value;
                NeedRecalcSize = true;
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the height of the label. 0 value indicates the auto-sizing item based on the text height.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.DefaultValue(0), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates height of the label in pixels.")]
        public int Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                if (m_Height == value)
                    return;
                m_Height = value;
                NeedRecalcSize = true;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the color of the progress chunk.")]
        public Color ChunkColor
        {
            get
            {
                return m_ChunkColor;
            }
            set
            {
                m_ChunkColor = value;
                OnAppearanceChanged();
                this.Refresh();
            }
        }
        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor()
        {
            return (!m_ChunkColor.IsEmpty);
        }
        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor()
        {
            m_ChunkColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the target gradient color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the target gradient color of the progress chunk.")]
        public Color ChunkColor2
        {
            get
            {
                return m_ChunkColor2;
            }
            set
            {
                m_ChunkColor2 = value;
                OnAppearanceChanged();
                this.Refresh();
            }
        }
        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor2()
        {
            return (!m_ChunkColor2.IsEmpty);
        }
        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor2()
        {
            m_ChunkColor2 = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the gradient angle of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Gets or sets the gradient angle of the progress chunk."), DefaultValue(0)]
        public float ChunkGradientAngle
        {
            get
            {
                return m_ChunkGradientAngle;
            }
            set
            {
                m_ChunkGradientAngle = value;
                if (!m_ChunkColor2.IsEmpty)
                    this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Overloaded. Deserializes the Item from the XmlElement.
        /// </summary>
        /// <param name="ItemXmlSource">Source XmlElement.</param>
        public override void Deserialize(ItemSerializationContext context)
        {
            base.Deserialize(context);

            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;

            m_Width = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("width"));
            m_Height = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("height"));
            m_Value = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("value"));
            m_Minimum = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("min"));
            m_Maximum = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("max"));
            m_TextVisible = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("textvisible"));
            m_Step = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("step"));
            if (ItemXmlSource.HasAttribute("chunkcolor"))
                m_ChunkColor = BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("chunkcolor"));
            if (ItemXmlSource.HasAttribute("chunkcolor2"))
                m_ChunkColor2 = BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("chunkcolor2"));
            m_ChunkGradientAngle = System.Xml.XmlConvert.ToSingle(ItemXmlSource.GetAttribute("chunkga"));

            foreach (System.Xml.XmlElement xmlElem in ItemXmlSource.ChildNodes)
            {
                switch (xmlElem.Name)
                {
                    case "backstyle2":
                        {
                            ElementSerializer.Deserialize(m_BackgroundStyle, xmlElem);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Overloaded. Serializes the item and all sub-items into the XmlElement.
        /// </summary>
        /// <param name="ThisItem">XmlElement to serialize the item to.</param>
        protected internal override void Serialize(ItemSerializationContext context)
        {
            base.Serialize(context);

            System.Xml.XmlElement ThisItem = context.ItemXmlElement;

            ThisItem.SetAttribute("width", System.Xml.XmlConvert.ToString(m_Width));
            ThisItem.SetAttribute("height", System.Xml.XmlConvert.ToString(m_Height));
            ThisItem.SetAttribute("value", System.Xml.XmlConvert.ToString(m_Value));
            ThisItem.SetAttribute("min", System.Xml.XmlConvert.ToString(m_Minimum));
            ThisItem.SetAttribute("max", System.Xml.XmlConvert.ToString(m_Maximum));
            ThisItem.SetAttribute("textvisible", System.Xml.XmlConvert.ToString(m_TextVisible));
            ThisItem.SetAttribute("step", System.Xml.XmlConvert.ToString(m_Step));
            if (!m_ChunkColor.IsEmpty)
                ThisItem.SetAttribute("chunkcolor", BarFunctions.ColorToString(m_ChunkColor));
            if (!m_ChunkColor2.IsEmpty)
                ThisItem.SetAttribute("chunkcolor2", BarFunctions.ColorToString(m_ChunkColor2));
            ThisItem.SetAttribute("chunkga", System.Xml.XmlConvert.ToString(m_ChunkGradientAngle));

            if (m_BackgroundStyle.Custom)
            {
                System.Xml.XmlElement style = ThisItem.OwnerDocument.CreateElement("backstyle2");
                ThisItem.AppendChild(style);
                ElementSerializer.Serialize(m_BackgroundStyle, style);
            }

        }

        // IPersonalizedMenuItem Impementation
        /// <summary>
        /// Indicates item's visiblity when on pop-up menu.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates item's visiblity when on pop-up menu.")]
        public eMenuVisibility MenuVisibility
        {
            get
            {
                return m_MenuVisibility;
            }
            set
            {
                if (m_MenuVisibility != value)
                {
                    m_MenuVisibility = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "MenuVisibility");
                }
            }
        }
        /// <summary>
        /// Indicates whether item was recently used.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public bool RecentlyUsed
        {
            get
            {
                return m_RecentlyUsed;
            }
            set
            {
                if (m_RecentlyUsed != value)
                {
                    m_RecentlyUsed = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "RecentlyUsed");
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of progress bar used to indicate progress. The Standard style displays the progress based on Minimum, Maximum and current Value.
        /// The Marquee type is automatically moving progress bar that is used to indicate an ongoing operation for which the actual duration cannot be estimated.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(eProgressItemType.Standard), Description("Indicates type of progress bar used to indicate progress.")]
        public eProgressItemType ProgressType
        {
            get { return m_ProgressType; }
            set
            {
                m_ProgressType = value;
                OnProgressTypeChanged();
            }
        }

        protected override void OnDesignModeChanged()
        {
            if (this.DesignMode)
            {
                if (m_ProgressType == eProgressItemType.Marquee)
                    DisposeTimer();
            }

            base.OnDesignModeChanged();
        }

        private void OnProgressTypeChanged()
        {
            if (this.DesignMode) return;

            DisposeTimer();
            if (m_ProgressType == eProgressItemType.Marquee)
                SetupTimer();
            this.OnAppearanceChanged();
            this.Refresh();
        }

        protected internal override void OnVisibleChanged(bool newValue)
        {
            base.OnVisibleChanged(newValue);
            if (m_ProgressType == eProgressItemType.Marquee && m_MarqueeTimer!=null)
            {
                if (newValue)
                    m_MarqueeTimer.Start();
                else
                    m_MarqueeTimer.Stop();
            }
        }

        protected override void Dispose(bool disposing)
        {
            DisposeTimer();
            base.Dispose(disposing);
        }

        private Timer m_MarqueeTimer = null;
        private void DisposeTimer()
        {
            if (m_MarqueeTimer != null)
            {
                m_MarqueeTimer.Stop();
                m_MarqueeTimer.Dispose();
                m_MarqueeTimer = null;
            }
        }

        private void SetupTimer()
        {
            if (m_MarqueeTimer != null) DisposeTimer();

            m_MarqueeValue = 0;
            if (m_MarqueeAnimationSpeed == 0) return;

            m_MarqueeTimer = new Timer();
            m_MarqueeTimer.Interval = m_MarqueeAnimationSpeed;
            m_MarqueeTimer.Tick += new EventHandler(MarqueeTimer_Tick);
            if (this.Visible)
                m_MarqueeTimer.Start();
        }

        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            m_MarqueeValue+=5;
            if (m_MarqueeValue > 100)
                m_MarqueeValue = 0;
            this.Refresh();
        }

        internal int MarqueeValue
        {
            get { return m_MarqueeValue; }
        }

        /// <summary>
        /// Gets or sets the marquee animation speed in milliseconds.
        /// </summary>
        [Browsable(true), DefaultValue(100), Category("Behavior"), Description("Indicates marquee animation speed in milliseconds.")]
        public int MarqueeAnimationSpeed
        {
            get { return m_MarqueeAnimationSpeed; }
            set
            {
                m_MarqueeAnimationSpeed = value;
                OnProgressTypeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the predefined color state table for progress bar. Color specified applies to items with Office 2007 style only. It does not have
        /// any effect on other styles. You can use ColorTable to indicate the state of the operation that Progress Bar is tracking. Default value is eProgressBarItemColor.Normal.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eProgressBarItemColor.Normal), Category("Appearance"), Description("Indicates predefined color of item when Office 2007 style is used.")]
        public eProgressBarItemColor ColorTable
        {
            get { return m_ColorTable; }
            set
            {
                if (m_ColorTable != value)
                {
                    m_ColorTable = value;
                    this.Refresh();
                }
            }
        }
    }
}
