using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeTickMarkLabelConvertor))]
    public class GaugeTickMarkLabel : GaugeTickMarkBase
    {
        #region Private properties

        private TickPoint _TickPoint;
        private double _Interval;

        #endregion

        public GaugeTickMarkLabel(GaugeScale scale, GaugeTickMarkRank rank,
            GaugeMarkerStyle style, float width, float length, double interval)
            : base(scale, rank, style, width, length)
        {
            _Interval = interval;
        }

        #region Internal properties

        internal double Interval
        {
            get { return (_Interval); }
            set { _Interval = value; }
        }

        internal TickPoint TickPoint
        {
            get { return (_TickPoint); }
            set { _TickPoint = value; }
        }

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                CalcTickPoint();
            }
        }

        #region CalcTickPoint

        private void CalcTickPoint()
        {
            _TickPoint = null;

            if (Scale is GaugeCircularScale)
                CalcCircularTickPoint(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearTickPoint(Scale as GaugeLinearScale);
        }

        #region CalcCircularTickPoint

        private void CalcCircularTickPoint(GaugeCircularScale scale)
        {
            double spread = Scale.MaxValue - Scale.MinValue;
            double dpt = scale.SweepAngle / spread;

            if (_Interval >= 0 && _Interval <= spread)
            {
                _TickPoint = new TickPoint(this);

                if (scale.Reversed == true)
                    _TickPoint.Angle = (float)(scale.StartAngle + scale.SweepAngle - (_Interval * dpt));
                else
                    _TickPoint.Angle = (float)(scale.StartAngle + _Interval * dpt);

                _TickPoint.Point = scale.GetPoint(Radius, _TickPoint.Angle);
                _TickPoint.Interval = _Interval;
            }
        }

        #endregion

        #region CalcLinearTickPoint

        private void CalcLinearTickPoint(GaugeLinearScale scale)
        {
            double spread = Math.Abs(scale.MaxValue - scale.MinValue);
            double dpt = scale.AbsScaleLength / spread;

            if (_Interval >= 0 && _Interval <= spread)
            {
                if (scale.Orientation == Orientation.Horizontal)
                    CalcHorizontalTickPoint(scale, dpt);
                else
                    CalcVerticalTickPoint(scale, dpt);
            }
        }

        #region CalcHorizontalTickPoint

        private void CalcHorizontalTickPoint(GaugeLinearScale scale, double dpt)
        {
            _TickPoint = new TickPoint(this);

            int x = (scale.Reversed == true)
                ? Scale.Bounds.Right - (int)(dpt * _Interval)
                : Scale.Bounds.X + (int)(dpt * _Interval);

            int y = scale.ScaleBounds.Y + Offset;

            _TickPoint.Point = new Point(x, y);
            _TickPoint.Interval = _Interval;
        }

        #endregion

        #region CalcVerticalTickPoint

        private void CalcVerticalTickPoint(GaugeLinearScale scale, double dpt)
        {
            _TickPoint = new TickPoint(this);

            int x = scale.ScaleBounds.X + Offset;

            int y = (scale.Reversed == true)
                ? Scale.Bounds.Top + (int)(dpt * _Interval)
                : Scale.Bounds.Bottom - (int)(dpt * _Interval);

            _TickPoint.Point = new Point(x, y);
            _TickPoint.Interval = _Interval;
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }

    #region GaugeLabelTickMarkConvertor

    public class GaugeTickMarkLabelConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeTickMarkLabel tickMark = value as GaugeTickMarkLabel;

                if (tickMark != null)
                {
                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
