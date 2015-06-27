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
    /// Collection of GaugeCircularScales
    /// </summary>
    public class GaugeCircularScaleCollection : GenericCollection<GaugeCircularScale>
    {
    }

    [TypeConverter(typeof(GaugeScaleConvertor))]
    public class GaugeCircularScale : GaugeScale
    {
        #region Private variables

        private float _StartAngle;
        private float _SweepAngle;
        private float _Radius;

        private PointF _PivotPoint;
        private int _SquareSize;

        #endregion

        public GaugeCircularScale(GaugeControl gaugeControl)
            : base(gaugeControl)
        {
            InitGaugeScale();
        }

        public GaugeCircularScale()
        {
            InitGaugeScale();
        }

        #region InitGaugeScale

        private void InitGaugeScale()
        {
            Style = GaugeScaleStyle.Circular;

            _PivotPoint = new PointF(.5f, .5f);

            Radius = .38f;
            StartAngle = 110;
            SweepAngle = 320;

            Width = .065f;
        }

        #endregion

        #region Public properties

        #region PivotPoint

        /// <summary>
        /// Gets or sets the Scale pivot point, specified as a percentage
        /// </summary>
        [Browsable(true), Category("Layout")]
        [Editor("DevComponents.Instrumentation.Design.PivotPointEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Scale pivot point, specified as a percentage.")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF PivotPoint
        {
            get { return (_PivotPoint); }

            set
            {
                if (_PivotPoint.Equals(value) == false)
                {
                    _PivotPoint = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal bool ShouldSerializePivotPoint()
        {
            return (_PivotPoint.X != .5f || _PivotPoint.Y != .5f);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal void ResetPivotPoint()
        {
            _PivotPoint = new PointF(.5f, .5f);
        }

        #endregion

        #region Radius

        /// <summary>
        /// Gets or sets the Radius of Scale, specified as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(.38f)]
        [Editor("DevComponents.Instrumentation.Design.RadiusRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the Radius of Scale, specified as a percentage.")]
        [NotifyParentProperty(true)]
        public float Radius
        {
            get { return (_Radius); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");

                if (_Radius != value)
                {
                    _Radius = value;

                    NeedLabelRecalcLayout = true;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region StartAngle

        /// <summary>
        /// Gets and sets the angle measured from the x-axis to the starting point of the scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(110f)]
        [Description("Indicates the angle measured from the x-axis to the starting point of the scale.")]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float StartAngle
        {
            get { return (_StartAngle); }

            set
            {
                if (value < 0 || value > 360)
                    throw new ArgumentException("Value must be between 0 and 360 degrees.");

                if (_StartAngle != value)
                {
                    _StartAngle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region SweepAngle

        /// <summary>
        /// Get and sets the angle measured from the StartAngle to the ending point of the scale.
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(320f)]
        [Description("Indicates the angle measured from the StartAngle to the ending point of the scale.")]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public float SweepAngle
        {
            get { return (_SweepAngle); }

            set
            {
                if (value < 0 || value > 360)
                    throw new ArgumentException("Value must be between -0 and +360.");

                if (_SweepAngle != value)
                {
                    _SweepAngle = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsRadius

        internal int AbsRadius
        {
            get { return (int)(SquareSize * _Radius); }
        }

        #endregion

        #region AbsScaleWidth

        internal int AbsScaleWidth
        {
            get { return (int)(AbsRadius * Width); }
        }

        #endregion

        #region SquareSize

        internal int SquareSize
        {
            get { return (_SquareSize); }
            set { _SquareSize = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                RecalcMetrics();

                NeedLabelRecalcLayout = true;
                NeedSectionRecalcLayout = true;
                NeedRangeRecalcLayout = true;
                NeedTickMarkRecalcLayout = true;
                NeedPointerRecalcLayout = true;
                NeedPinRecalcLayout = true;

                base.RecalcLayout();
            }
        }

        #region RecalcMetrics

        private void RecalcMetrics()
        {
            base.RecalcLayout();

            bool autoCenter = GaugeControl.Frame.AutoCenter;

            _SquareSize = GaugeControl.GetAbsSize(new SizeF(1, 1), true).Width;
            Center = GaugeControl.GetAbsPoint(_PivotPoint, autoCenter);

            int radius = (int)(_Radius * _SquareSize);

            Rectangle r = new Rectangle();
            r.Size = new Size(radius * 2, radius * 2);
            r.Location = new Point(Center.X - r.Size.Width / 2, Center.Y - r.Size.Height / 2);

            Bounds = r;
        }

        #endregion

        #endregion

        #region PaintBorder

        protected override void PaintBorder(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (BorderWidth > 0)
            {
                int radius = AbsRadius;
                int n = (int) (radius*Width);

                if (radius > 0 && n > 0)
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        Rectangle r = Bounds;
                        r.Inflate(n/2, n/2);

                        path.AddArc(r, _StartAngle, _SweepAngle);

                        r.Inflate(-n, -n);

                        path.AddArc(r, _StartAngle + _SweepAngle, -_SweepAngle);

                        path.CloseAllFigures();

                        using (Pen pen = new Pen(BorderColor, BorderWidth))
                            g.DrawPath(pen, path);
                    }
                }
            }
        }

        #endregion

        #region GetPoint

        internal Point GetPoint(int radius, float angle)
        {
            Point pt = new Point();

            // Normalize the angle and calculate some
            // working vars

            if (angle < 0)
                angle += 360;

            angle = angle % 360;

            // Determine the angle quadrant, and then calculate
            // the intersecting coordinate accordingly

            double radians = GetRadians(angle % 90);

            if (angle < 90)
            {
                pt.X = (int)(Math.Cos(radians) * radius);
                pt.Y = (int)(Math.Sin(radians) * radius);
            }
            else if (angle < 180)
            {
                pt.X = -(int)(Math.Sin(radians) * radius);
                pt.Y = (int)(Math.Cos(radians) * radius);
            }
            else if (angle < 270)
            {
                pt.X = -(int)(Math.Cos(radians) * radius);
                pt.Y = -(int)(Math.Sin(radians) * radius);
            }
            else
            {
                pt.X = (int)(Math.Sin(radians) * radius);
                pt.Y = -(int)(Math.Cos(radians) * radius);
            }

            pt.X += Center.X;
            pt.Y += Center.Y;

            return (pt);
        }

        #endregion

        #region GetRadians

        /// <summary>
        /// Converts Degrees to Radians
        /// </summary>
        /// <param name="theta">Degrees</param>
        /// <returns>Radians</returns>
        private double GetRadians(float theta)
        {
            return (theta * Math.PI / 180);
        }

        #endregion

        #region GetDegrees

        internal double GetDegrees(double radians)
        {
            return (radians * 180 / Math.PI);
        }

        #endregion

        #region GetNearLabelRadius

        internal int GetNearLabelRadius()
        {
            int scaleRadius = AbsRadius;
            int scaleWidth = AbsScaleWidth;

            int radius = scaleRadius - scaleWidth / 2;

            radius = GetNearLabelRadius(MajorTickMarks, scaleRadius, radius);
            radius = GetNearLabelRadius(MinorTickMarks, scaleRadius, radius);

            return (radius);
        }

        private int GetNearLabelRadius(GaugeTickMark tickMarks, int scaleRadius, int radius)
        {
            if (tickMarks.Visible == true)
            {
                int tickMarkRadius = tickMarks.Radius;

                if (tickMarks.Layout.Placement != DisplayPlacement.Near)
                    tickMarkRadius -= (int)(tickMarks.Layout.Length * scaleRadius);

                if (tickMarkRadius < radius)
                    radius = tickMarkRadius;
            }

            return (radius);
        }

        #endregion

        #region GetFarLabelRadius

        internal int GetFarLabelRadius()
        {
            int scaleRadius = AbsRadius;
            int scaleWidth = AbsScaleWidth;

            int radius = scaleRadius + scaleWidth / 2;

            radius = GetFarLabelRadius(MajorTickMarks, scaleRadius, radius);
            radius = GetFarLabelRadius(MinorTickMarks, scaleRadius, radius);

            return (radius);
        }

        private int GetFarLabelRadius(GaugeTickMark tickMarks, int scaleRadius, int radius)
        {
            if (tickMarks.Visible == true)
            {
                int tickMarkRadius = tickMarks.Radius;

                if (tickMarks.Layout.Placement == DisplayPlacement.Near)
                    tickMarkRadius += (int)(tickMarks.Layout.Length * scaleRadius);

                if (tickMarkRadius > radius)
                    radius = tickMarkRadius;
            }

            return (radius);
        }

        #endregion

        #region CreateGradient

        internal PathGradientBrush CreateGradient(Rectangle r,
            float startAngle, float sweepAngle, GradientFillColor fillColor, int n)
        {
            r.Inflate(n, n);

            const int count = 15;

            float n1 = sweepAngle / count;

            float sa = startAngle + (sweepAngle > 0 ? -1 : 1);
            float ea = sa + sweepAngle;

            PointF[] pts = new PointF[count + 1];
            Color[] cls = new Color[count + 1];

            Color c1 = fillColor.Start;
            Color c2 = fillColor.End.IsEmpty == false ? fillColor.End : fillColor.Start;

            // Calculate the RGB color deltas

            float dr = (c2.R - c1.R) / count;
            float dg = (c2.G - c1.G) / count;
            float db = (c2.B - c1.B) / count;

            int radius = r.Width / 2;

            for (int i = 0; i < count; i++)
            {
                pts[i] = GetPoint(radius, sa + i * n1);

                Color c3 = Color.FromArgb(
                    (int)(c1.R + dr * i),
                    (int)(c1.G + dg * i),
                    (int)(c1.B + db * i));

                cls[i] = c3;
            }

            float d = Math.Abs(sweepAngle);
            float delta = (d < 180 ? (180 - d) : 2);

            pts[count] = GetPoint(radius, ea + (sweepAngle < 0 ? -delta : delta));
            cls[count] = c2;

            PathGradientBrush pgb = new PathGradientBrush(pts);

            pgb.CenterColor = Color.White;
            pgb.CenterPoint = Center;
            pgb.SurroundColors = cls;

            pgb.FocusScales = new PointF(0f, 0f);

            Blend blnd = new Blend();
            blnd.Positions = new float[] { 0f, 1f };
            blnd.Factors = new float[] { 1f, 1f };
            pgb.Blend = blnd;

            return (pgb);
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeCircularScale copy = new GaugeCircularScale();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeCircularScale c = copy as GaugeCircularScale;

            if (c != null)
            {
                base.CopyToItem(c);

                c.PivotPoint = _PivotPoint;
                c.Radius = _Radius;
                c.StartAngle = _StartAngle;
                c.SweepAngle = _SweepAngle;
            }
        }

        #endregion
    }
}
