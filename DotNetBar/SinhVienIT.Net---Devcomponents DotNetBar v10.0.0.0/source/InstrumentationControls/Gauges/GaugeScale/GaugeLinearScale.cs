using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Collection of GaugeLinearScales
    /// </summary>
    public class GaugeLinearScaleCollection : GenericCollection<GaugeLinearScale>
    {
    }

    [TypeConverter(typeof(GaugeScaleConvertor))]
    public class GaugeLinearScale : GaugeScale
    {
        #region Private variables

        private SizeF _Size;
        private PointF _Location;
        private Orientation _Orientation;

        private Rectangle _ScaleBounds;

        #endregion

        public GaugeLinearScale(GaugeControl gaugeControl)
            : base(gaugeControl)
        {
            InitGaugeScale();
        }

        public GaugeLinearScale()
        {
            InitGaugeScale();
        }

        #region InitGaugeScale

        private void InitGaugeScale()
        {
            Style = GaugeScaleStyle.Linear;

            _Location = new PointF(.5f, .5f);
            _Size = new SizeF(.8f, .8f);

            _Orientation = Orientation.Horizontal;
        }

        #endregion

        #region Public properties

        #region Location

        /// <summary>
        /// Gets or sets the Scale location, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.LocationEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Scale location, specified as a percentage.")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Location
        {
            get { return (_Location); }

            set
            {
                if (_Location.Equals(value) == false)
                {
                    _Location = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePivotPoint()
        {
            return (_Location.X != .5f || _Location.Y != .5f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPivotPoint()
        {
            _Location = new PointF(.5f, .5f);
        }

        #endregion

        #region Orientation

        /// <summary>
        /// Gets or sets the Scale display orientation
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(Orientation.Horizontal)]
        [Description("Indicates the Scale display orientation.")]
        public Orientation Orientation
        {
            get { return (_Orientation); }

            set
            {
                if (_Orientation != value)
                {
                    _Orientation = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets or sets the bounding size of the Scale, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.SizeEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Determines the bounding size of the Scale, specified as a percentage.")]
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
        internal bool ShouldSerializeSize()
        {
            return (_Size.Width != .8f || _Size.Height != .8f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetSize()
        {
            _Size = new SizeF(.8f, .8f);
        }
        #endregion

        #endregion

        #region Internal properties

        #region AbsLength

        internal int AbsLength
        {
            get
            {
                if (_Orientation == Orientation.Horizontal)
                    return (Bounds.Width);

                return (Bounds.Height);
            }
        }

        #endregion

        #region AbsWidth

        internal int AbsWidth
        {
            get
            {
                if (_Orientation == Orientation.Horizontal)
                    return (Bounds.Height);

                return (Bounds.Width);
            }
        }

        #endregion

        #region AbsScaleLength

        internal int AbsScaleLength
        {
            get
            {
                if (_Orientation == Orientation.Horizontal)
                    return (_ScaleBounds.Width);

                return (_ScaleBounds.Height);
            }
        }

        #endregion

        #region AbsScaleWidth

        internal int AbsScaleWidth
        {
            get
            {
                if (_Orientation == Orientation.Horizontal)
                    return (_ScaleBounds.Height);

                return (_ScaleBounds.Width);
            }
        }

        #endregion

        #region ScaleBounds

        internal Rectangle ScaleBounds
        {
            get { return (_ScaleBounds); }
            set { _ScaleBounds = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                RecalcMetrics();
            }
        }

        #region RecalcMetrics

        private void RecalcMetrics()
        {
            Center = GaugeControl.GetAbsPoint(_Location, false);

            Rectangle r = new Rectangle();

            r.Size = GaugeControl.GetAbsSize(_Size, false);
            r.Location = new Point(Center.X - r.Size.Width / 2, Center.Y - r.Size.Height / 2);

            Bounds = r;

            _ScaleBounds = new Rectangle();

            if (Orientation == Orientation.Horizontal)
            {
                _ScaleBounds.Size = new Size(r.Width, (int)(r.Height * Width));
                _ScaleBounds.Location = new Point(r.X, Center.Y - _ScaleBounds.Size.Height / 2);
            }
            else
            {
                _ScaleBounds.Size = new Size((int)(r.Width * Width), r.Height);
                _ScaleBounds.Location = new Point(Center.X - _ScaleBounds.Size.Width / 2, r.Y);
            }
        }

        #endregion

        #endregion

        #region PaintBorder

        protected override void PaintBorder(PaintEventArgs e)
        {
            if (BorderWidth > 0)
            {
                if (Bounds.Width > 0 && Bounds.Height > 0)
                {
                    using (Pen pen = new Pen(BorderColor, BorderWidth))
                        e.Graphics.DrawRectangle(pen, _ScaleBounds);
                }
            }
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeLinearScale copy = new GaugeLinearScale();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeLinearScale c = copy as GaugeLinearScale;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Location = _Location;
                c.Orientation = _Orientation;
                c.Size = _Size;
            }
        }

        #endregion
    }
}
