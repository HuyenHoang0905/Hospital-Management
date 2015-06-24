using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeTickMarkConvertor))]
    public class GaugeTickMark : GaugeTickMarkBase
    {
        #region Private variables

        private double _Interval;
        private double _IntervalOffset;
        private double _DefaultInterval;

        private TickPoint[] _TickPoints;

        #endregion

        public GaugeTickMark(GaugeScale scale, GaugeTickMarkRank rank,
            GaugeMarkerStyle style, float width, float length, double interval)
            : base(scale, rank, style, width, length)
        {
            _Interval = interval;
            _DefaultInterval = interval;
        }

        #region Public properties

        #region Interval

        /// <summary>
        /// Gets or sets the TickMark Interval spacing
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        [Description("Indicates the TickMark Interval spacing.")]
        public double Interval
        {
            get { return (_Interval); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");
                
                if (_Interval != value)
                {
                    _Interval = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeInterval()
        {
            return (_Interval != _DefaultInterval);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetInterval()
        {
            Interval = _DefaultInterval;
        }

        #endregion

        #region IntervalOffset

        /// <summary>
        /// Gets or sets the initial TickMark Interval Offset
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0d)]
        [Description("Indicates the initial TickMark Interval Offset.")]
        public double IntervalOffset
        {
            get { return (_IntervalOffset); }

            set
            {
                if (value < 0)
                    throw new ArgumentException("Value can not be less than zero.");
                
                if (_IntervalOffset != value)
                {
                    _IntervalOffset = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region TickPoints

        internal TickPoint[] TickPoints
        {
            get { return (_TickPoints); }
            set { _TickPoints = value; }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                CalcTickPoints();

                Scale.NeedLabelRecalcLayout = true;
            }
        }

        #region CalcTickPoints

        private void CalcTickPoints()
        {
            _TickPoints = null;

            if (_Interval > 0)
            {
                double ticks = Scale.MaxValue - Scale.MinValue;

                if (ticks > 0)
                {
                    int n = (int) ((ticks - _IntervalOffset)/_Interval) + 1;

                    if (n > 0)
                    {
                        if (Scale is GaugeCircularScale)
                            CalcCircularTickPoints(Scale as GaugeCircularScale, ticks, n);

                        else if (Scale is GaugeLinearScale)
                            CalcLinearTickPoints(Scale as GaugeLinearScale, ticks, n);
                    }
                }
            }
        }

        #region CalcCircularTickPoints

        private void CalcCircularTickPoints(
            GaugeCircularScale scale, double ticks, int n)
        {
            double dpt = scale.SweepAngle / ticks;
            double theta = _Interval * dpt;

            double startAngle = scale.StartAngle;

            if (scale.Reversed == true)
                startAngle += scale.SweepAngle;

            int dir = scale.Reversed ? -1 : 1;
          
            startAngle += (dpt * _IntervalOffset * dir);

            _TickPoints = new TickPoint[n];

            double interval = _IntervalOffset;

            for (int i = 0; i < n; i++)
            {
                _TickPoints[i] = new TickPoint(this);

                _TickPoints[i].Angle = (float)(startAngle + (i * theta * dir));
                _TickPoints[i].Point = scale.GetPoint(Radius, _TickPoints[i].Angle);
                _TickPoints[i].Interval = interval;

                interval += _Interval;
            }
        }

        #endregion

        #region CalcLinearTickPoints

        private void CalcLinearTickPoints(
            GaugeLinearScale scale, double ticks, int n)
        {
            double dpt = scale.AbsScaleLength/ticks;

            if (scale.Orientation == Orientation.Horizontal)
                CalcHorizontalTickPoints(n, dpt);
            else
                CalcVerticalTickPoints(n, dpt);
        }

        #region CalcHorizontalTickPoints

        private void CalcHorizontalTickPoints(int n, double dpt)
        {
            _TickPoints = new TickPoint[n];

            double interval = _IntervalOffset;

            for (int i = 0; i < n; i++)
            {
                _TickPoints[i] = new TickPoint(this);

                int x = (Scale.Reversed == true)
                    ? Bounds.Right - (int)(dpt * interval)
                    : Bounds.X + (int)(dpt * interval);

                _TickPoints[i].Point = new Point(x, Bounds.Y);
                _TickPoints[i].Interval = interval;

                interval += _Interval;
            }
        }

        #endregion

        #region CalcVerticalTickPoints

        private void CalcVerticalTickPoints(int n, double dpt)
        {
            _TickPoints = new TickPoint[n];

            double interval = _IntervalOffset;

            for (int i = 0; i < n; i++)
            {
                _TickPoints[i] = new TickPoint(this);

                int y = (Scale.Reversed == true)
                    ? Bounds.Top + (int)(dpt * interval)
                    : Bounds.Bottom - (int)(dpt * interval);

                _TickPoints[i].Point = new Point(Bounds.X, y);
                _TickPoints[i].Interval = interval;

                interval += _Interval;
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeTickMark copy = new
                GaugeTickMark(Scale, Rank, Layout.Style, Width, Length, Interval);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeTickMark c = copy as GaugeTickMark;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Interval = _Interval;
                c.IntervalOffset = _IntervalOffset;

                c._DefaultInterval = _DefaultInterval;
            }
        }

        #endregion
    }

    #region GaugeTickMarkConvertor

    public class GaugeTickMarkConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeTickMark tickMark = value as GaugeTickMark;

                if (tickMark != null)
                {
                    //ColorConverter cvt = new ColorConverter();

                    //if (lct.Start != Color.Empty)
                    //    return (cvt.ConvertToString(lct.Start));

                    //if (lct.End != Color.Empty)
                    //    return (cvt.ConvertToString(lct.End));

                    //if (lct.GradientAngle != 90)
                    //    return (lct.GradientAngle.ToString());

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
