using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugeImages
    /// </summary>
    public class GaugeImageCollection : GenericCollection<GaugeImage>
    {
    }

    public class GaugeImage : GaugeItem
    {
        #region Private variables

        private Image _Image;

        private SizeF _Size;
        private PointF _Location;
        private float _Angle;

        private bool _AutoFit;

        private Rectangle _Bounds;
        private Point _Center;

        private bool _UnderScale;

        private GaugeControl _GaugeControl;

        #endregion

        public GaugeImage(GaugeControl gaugeControl)
            : this()
        {
            _GaugeControl = gaugeControl;

            InitGagueImage();
        }

        public GaugeImage()
        {
            InitGagueImage();
        }

        #region InitGagueImage

        private void InitGagueImage()
        {
            _Size = new SizeF(.1f, .1f);
            _Location = new PointF(.3f, .5f);

            _UnderScale = true;
        }

        #endregion

        #region Public properties

        #region Angle

        /// <summary>
        /// Gets or sets the amount to rotate the image, specified in degrees
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the amount to rotate the image, specified in degrees.")]
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

        #region AutoFit

        /// <summary>
        /// Gets or sets whether the image will be stretched to fit the given area
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(false)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines whether the image will be stretched to fit the given area.")]
        public bool AutoFit
        {
            get { return (_AutoFit); }

            set
            {
                if (_AutoFit != value)
                {
                    _AutoFit = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the Image to be displayed
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to be displayed.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// Gets or sets the location of the image area, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.LocationEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the location of the image area, specified as a percentage.")]
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
        /// Gets or sets the size of the image, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.SizeEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the size of the image, specified as a percentage.")]
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

        #region UnderScale

        /// <summary>
        /// Gets or sets whether the image is displayed under the scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(true)]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates whether the image is displayed under the scale.")]
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

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                bool autoCenter = _GaugeControl.Frame.AutoCenter;
                Size size = _GaugeControl.GetAbsSize(_Size, autoCenter);

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

                if (Image != null)
                {
                    if (_AutoFit == false)
                    {
                        r = new Rectangle(0, 0, _Image.Width, _Image.Height);

                        r.X -= _Image.Width / 2;
                        r.Y -= _Image.Height / 2;
                    }

                    g.DrawImage(_Image, r);
                }
                else
                {
                    g.FillRectangle(Brushes.White, r);

                    g.DrawLine(Pens.Red, new Point(r.X, r.Y), new Point(r.Right, r.Bottom));
                    g.DrawLine(Pens.Red, new Point(r.X, r.Bottom), new Point(r.Right, r.Y));

                    g.DrawRectangle(Pens.Black, r);
                }

                g.ResetTransform();
            }
        }

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
            GaugeImage copy = new GaugeImage();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeImage c = copy as GaugeImage;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Angle = _Angle;
                c.AutoFit = _AutoFit;
                c.Image = _Image;
                c.Location = _Location;
                c.Size = _Size;
                c.UnderScale = _UnderScale;
            }
        }

        #endregion

    }
}
