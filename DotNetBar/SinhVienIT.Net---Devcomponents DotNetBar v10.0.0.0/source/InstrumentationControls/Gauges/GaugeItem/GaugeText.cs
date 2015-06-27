using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugeText items
    /// </summary>
    public class GaugeTextCollection : GenericCollection<GaugeText>
    {
    }

    public class GaugeText : GaugeItem
    {
        #region Private variables

        private string _Text;

        private SizeF _Size;
        private PointF _Location;
        private float _Angle;

        private Font _Font;
        private Font _AbsFont;

        private bool _AutoSize;

        private TextAlignment _TextAlignment;

        private Color _ForeColor;
        private GradientFillColor _BackColor;

        private Rectangle _Bounds;
        private Point _Center;

        private bool _UnderScale;

        private GaugeControl _GaugeControl;

        #endregion

        public GaugeText(GaugeControl gaugeControl)
            : this()
        {
            _GaugeControl = gaugeControl;

            InitGaugeText();
        }

        public GaugeText()
        {
            InitGaugeText();
        }

        #region InitGagueText

        private void InitGaugeText()
        {
            _Text = "Text";

            BackColor.BorderColor = Color.Black;

            _ForeColor = Color.Black;

            _Size = new SizeF(.2f, .2f);
            _Location = new PointF(.3f, .5f);

            _AutoSize = true;
            _TextAlignment = TextAlignment.MiddleCenter;

            _UnderScale = true;
        }

        #endregion

        #region Public properties

        #region Angle

        /// <summary>
        /// Gets or sets the amount to rotate the text, specified in degrees.
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the amount to rotate the text, specified in degrees.")]
        public float Angle
        {
            get { return (_Angle); }

            set
            {
                if (_Angle != value)
                {
                    _Angle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region AutoSize

        /// <summary>
        /// Gets or sets whether the text Font size is auto sized
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(true)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates whether the text Font size is auto sized.")]
        public bool AutoSize
        {
            get { return (_AutoSize); }

            set
            {
                if (_AutoSize != value)
                {
                    _AutoSize = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Gets or sets the text BackColor
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the BackColor.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GradientFillColor BackColor
        {
            get
            {
                if (_BackColor == null)
                {
                    _BackColor = new GradientFillColor();
                    _BackColor.ColorTableChanged += BackColor_ColorTableChanged;
                }

                return (_BackColor);
            }

            set
            {
                if (_BackColor != null)
                    _BackColor.ColorTableChanged -= BackColor_ColorTableChanged;

                _BackColor = value;

                if (_BackColor != null)
                    _BackColor.ColorTableChanged += BackColor_ColorTableChanged;

                OnGaugeItemChanged();
            }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets or sets the text Font
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the text Font.")]
        public Font Font
        {
            get
            {
                if (_Font == null)
                    _Font = new Font("Microsoft SanSerif", 12);

                return (_Font);
            }

            set
            {
                if (_Font != null)
                    _Font.Dispose();

                _Font = value;
                AbsFont = null;

                OnGaugeItemChanged(true);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeFont()
        {
            if (_Font == null)
                return (false);

            using (Font font = new Font("Microsoft SanSerif", 12))
                return (_Font.Equals(font) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetFont()
        {
            Font = new Font("Microsoft SanSerif", 12);
        }

        #endregion

        #region ForeColor

        /// <summary>
        /// Gets or sets the text ForeColor
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        [Description("Indicates the text ForeColor.")]
        public Color ForeColor
        {
            get { return (_ForeColor); }

            set
            {
                if (_ForeColor != value)
                {
                    _ForeColor = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// Gets or sets the center location of the text area, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.LocationEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the center location of the text area, specified as a percentage.")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Location
        {
            get { return (_Location); }

            set
            {
                if (_Location != value)
                {
                    _Location = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLocation()
        {
            return (_Location.X != .3f || _Location.Y != .5f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLocation()
        {
            _Location = new PointF(.3f, .5f);
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets or sets the size of the text area, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.SizeEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the size of the text area, specified as a percentage.")]
        public SizeF Size
        {
            get { return (_Size); }

            set
            {
                if (_Size != value)
                {
                    _Size = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSize()
        {
            return (_Size.Width != .2f || _Size.Height != .2f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSize()
        {
            _Size = new SizeF(.2f, .2f);
        }
        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text to be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue("Text")]
        [Description("Indicates the text to be displayed.")]
        public string Text
        {
            get { return (_Text); }

            set
            {
                if (_Text != value)
                {
                    _Text = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TextAlignment

        /// <summary>
        /// Gets or sets the alignment of the text
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(TextAlignment.MiddleCenter)]
        [Description("Indicates the alignment of the text.")]
        public TextAlignment TextAlignment
        {
            get { return (_TextAlignment); }

            set
            {
                if (_TextAlignment != value)
                {
                    _TextAlignment = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region UnderScale

        /// <summary>
        /// Gets or sets whether the text is displayed under the scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(true)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates whether the text is displayed under the scale.")]
        public bool UnderScale
        {
            get { return (_UnderScale); }

            set
            {
                if (_UnderScale != value)
                {
                    _UnderScale = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsFont

        internal Font AbsFont
        {
            get
            {
                if (_AutoSize == false)
                    return (Font);

                if (_AbsFont == null)
                {
                    int n = (_GaugeControl.Frame.AutoCenter == true)
                        ? Math.Min(_GaugeControl.Width, _GaugeControl.Height) / 2
                        : Math.Max(_GaugeControl.Width, _GaugeControl.Height) / 3;

                    float emSize = Font.SizeInPoints;
                    emSize = (emSize / 170) * n;

                    AbsFont = new Font(_Font.FontFamily, emSize, _Font.Style);
                }

                return (_AbsFont);
            }

            set
            {
                if (_AbsFont != null)
                    _AbsFont.Dispose();

                _AbsFont = value;
            }
        }

        #endregion

        #region Bounds

        internal Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        #endregion

        #region GaugeControl

        internal GaugeControl GaugeControl
        {
            get { return (_GaugeControl); }
            set { _GaugeControl = value; }
        }

        #endregion

        #endregion

        #region Event processing

        void BackColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnGaugeItemChanged(true);
        }

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                bool autoCenter = _GaugeControl.Frame.AutoCenter;
                Size size = _GaugeControl.GetAbsSize(_Size, autoCenter);

                if (size.Equals(_Bounds.Size) == false)
                    AbsFont = null;

                _Center = _GaugeControl.GetAbsPoint(_Location, autoCenter);

                _Bounds = new Rectangle(
                    _Center.X - size.Width / 2, _Center.Y - size.Height / 2,
                    size.Width, size.Height);
            }
        }

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            Graphics g = e.Graphics;

            if (_Bounds.Width > 0 && _Bounds.Height > 0)
            {
                g.TranslateTransform(_Center.X, _Center.Y);
                g.RotateTransform(_Angle % 360);

                Rectangle r = new Rectangle(0, 0, _Bounds.Width, _Bounds.Height);

                r.X -= _Bounds.Width / 2;
                r.Y -= _Bounds.Height / 2;

                if (_BackColor.Color1.IsEmpty == false)
                    PaintBackColor(e, r);

                if (_BackColor.BorderWidth > 0)
                {
                    using (Pen pen = new Pen(_BackColor.BorderColor, _BackColor.BorderWidth))
                        g.DrawRectangle(pen, r);
                }

                if (string.IsNullOrEmpty(_Text) == false)
                {
                    using (StringFormat sf = new StringFormat())
                    {
                        SetStringAlignment(sf);

                        if (_BackColor.BorderWidth > 0)
                            r.Inflate(-(_BackColor.BorderWidth + 1), -(_BackColor.BorderWidth + 1));

                        using (Brush br = new SolidBrush(_ForeColor))
                            g.DrawString(_Text, AbsFont, br, r, sf);
                    }
                }

                g.ResetTransform();
            }
        }

        #region PaintBackColor

        private void PaintBackColor(PaintEventArgs e, Rectangle r)
        {
            Graphics g = e.Graphics;

            GradientFillType fillType = _BackColor.GradientFillType;
            
            if (_BackColor.End.IsEmpty == true || (_BackColor.Color1 == _BackColor.Color2))
                fillType = GradientFillType.None;

            switch (fillType)
            {
                case GradientFillType.Auto:
                case GradientFillType.Angle:
                    using (Brush br = _BackColor.GetBrush(r))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
                    }
                    break;

                case GradientFillType.StartToEnd:
                    using (Brush br = _BackColor.GetBrush(r, 90))
                    {
                        if (br is LinearGradientBrush)
                            ((LinearGradientBrush)br).WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
                    }
                    break;

                case GradientFillType.HorizontalCenter:
                    Rectangle t1 = r;
                    t1.Height /= 2;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(t1, _BackColor.Start, _BackColor.End, 90))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
                    }
                    break;

                case GradientFillType.VerticalCenter:
                    Rectangle t2 = r;
                    t2.Width /= 2;

                    using (LinearGradientBrush br = new
                        LinearGradientBrush(t2, _BackColor.Start, _BackColor.End, 0f))
                    {
                        br.WrapMode = WrapMode.TileFlipXY;

                        g.FillRectangle(br, r);
                    }
                    break;

                case GradientFillType.Center:
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(r);

                        using (PathGradientBrush br = new PathGradientBrush(path))
                        {
                            br.CenterColor = _BackColor.Start;
                            br.SurroundColors = new Color[] {_BackColor.End};

                            g.FillRectangle(br, r);
                        }
                    }
                    break;

                default:
                    using (Brush br = new SolidBrush(_BackColor.Start))
                        g.FillRectangle(br, r);

                    break;
            }
        }

        #endregion

        #region SetStringAlignment

        private void SetStringAlignment(StringFormat sf)
        {
            switch (_TextAlignment)
            {
                case TextAlignment.TopLeft:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.TopCenter:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.TopRight:
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Alignment = StringAlignment.Far;
                    break;

                case TextAlignment.MiddleLeft:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.MiddleCenter:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.MiddleRight:
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Far;
                    break;

                case TextAlignment.BottomLeft:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Near;
                    break;

                case TextAlignment.BottomCenter:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Center;
                    break;

                case TextAlignment.BottomRight:
                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Far;
                    break;
            }
        }

        #endregion

        #endregion

        #region Contains

        internal bool Contains(Point pt)
        {
            if (Angle == 0)
            {
                return (_Bounds.Contains(pt));
            }
            
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(Bounds);

                Matrix matrix = new Matrix();
                matrix.RotateAt(_Angle, _Center);

                path.Transform(matrix);

                return (path.IsVisible(pt));
            }
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeText copy = new GaugeText();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeText c = copy as GaugeText;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Angle = _Angle;
                c.AutoSize = _AutoSize;

                if (_BackColor != null)
                    c.BackColor = (GradientFillColor)_BackColor.Clone();

                if (_Font != null)
                    c.Font = (Font)_Font.Clone();

                c.ForeColor = _ForeColor;
                c.Location = _Location;
                c.Size = _Size;
                c.Text = _Text;
                c.TextAlignment = _TextAlignment;
                c.UnderScale = _UnderScale;
            }
        }

        #endregion
    }

    #region Enums

    public enum TextAlignment
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    #endregion
}
