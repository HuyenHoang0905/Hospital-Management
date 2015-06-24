using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal abstract class GaugePointerRenderer
    {
        #region Private variables

        private GaugePointer _GaugePointer;

        private int _Width;
        private int _Length;

        private float _IntervalAngle;
        private Point _IntervalPoint;

        private double _Dpt;

        private GraphicsPath _PointerPath;

        #endregion

        protected GaugePointerRenderer(GaugePointer gaugePointer)
        {
            _GaugePointer = gaugePointer;
        }

        #region Abstract methods

        public abstract void RenderCircular(PaintEventArgs e);
        public abstract void RenderLinear(PaintEventArgs e);
        public abstract GraphicsPath GetPointerPath();

        #endregion

        #region Protected properties

        #region GaugePointer

        protected GaugePointer GaugePointer
        {
            get { return (_GaugePointer); }
        }

        #endregion

        #region IntervalAngle

        internal float IntervalAngle
        {
            get { return (_IntervalAngle); }
            set { _IntervalAngle = value; }
        }

        #endregion

        #region IntervalPoint

        internal Point IntervalPoint
        {
            get { return (_IntervalPoint); }
            set { _IntervalPoint = value; }
        }

        #endregion

        #region Length

        protected int Length
        {
            get { return (_Length); }
            set { _Length = value; }
        }

        #endregion

        #region MarkerStyle
        
        protected GaugeMarkerStyle MarkerStyle
        {
            get { return (_GaugePointer.MarkerStyle); }
        }

        #endregion

        #region PointerPath

        internal GraphicsPath PointerPath
        {
            get { return (_PointerPath); }
            
            set
            {
                if (_PointerPath != value)
                {
                    if (_PointerPath != null)
                        _PointerPath.Dispose();

                    _PointerPath = value;
                }
            }
        }

        #endregion

        #region Radius

        protected int Radius
        {
            get { return (_GaugePointer.Radius); }
        }

        #endregion

        #region Scale

        protected GaugeScale Scale
        {
            get { return (_GaugePointer.Scale); }
        }

        #endregion

        #region Value

        protected double Value
        {
            get { return (_GaugePointer.DValue); }
        }

        #endregion

        #region Width

        protected int Width
        {
            get { return (_Width); }
            set { _Width = value; }
        }

        #endregion

        #endregion

        #region Internal properties

        #region Dpt

        internal double Dpt
        {
            get { return (_Dpt > 0 ? _Dpt : 1); }
            set { _Dpt = value; }
        }

        #endregion

        #endregion

        #region GetCapFillColor

        protected GradientFillColor GetCapFillColor(double interval)
        {
            const ColorSourceFillEntry entry = ColorSourceFillEntry.Cap;

            GradientFillColor fillColor = (Scale.GetRangeFillColor(interval, entry) ??
                                           Scale.GetSectionFillColor(interval, entry)) ?? GaugePointer.CapFillColor;

            return (fillColor);
        }

        #endregion

        #region GetPointerFillColor

        protected GradientFillColor GetPointerFillColor(double interval)
        {
            const ColorSourceFillEntry entry = ColorSourceFillEntry.Pointer;

            GradientFillColor fillColor = (Scale.GetRangeFillColor(interval, entry) ??
                                           Scale.GetSectionFillColor(interval, entry)) ?? GaugePointer.FillColor;

            return (fillColor);
        }

        #endregion

        #region GetInterval

        internal double GetInterval(double interval)
        {
            bool pegged;

            return (GetIntervalEx(interval, out pegged));
        }

        internal double GetInterval(double interval, out bool pegged)
        {
            double min, max;
            GetRange(out min, out max);

            pegged = true;

            if (interval.Equals(double.NaN))
                return (min);

            if (interval < min)
                interval = min;

            else if (interval > max)
                interval = max;
            else
                pegged = false;

            return (interval);
        }

        internal double GetIntervalEx(double interval, out bool pegged)
        {
            int n = (GaugePointer.Scale is GaugeLinearScale)
                        ? ((GaugeLinearScale) Scale).AbsLength
                        : 360;

            double minPinValue = GaugePointer.HonorMinPin ? (Scale.MinPinEndOffset*n)/Dpt : 0;
            double maxPinValue = GaugePointer.HonorMaxPin ? (Scale.MaxPinEndOffset*n)/Dpt : 0;

            double min = Scale.MinValue;
            double max = Scale.MaxValue;

            if (Scale.MinValue <= Scale.AbsMinLimit)
                min -= minPinValue;

            if (Scale.MaxValue >= Scale.AbsMaxLimit)
                max += maxPinValue;

            pegged = true;

            if (interval.Equals(double.NaN))
                return (min);

            if (interval < min)
            {
                interval = Scale.MinValue > Scale.AbsMinLimit
                               ? Math.Max(min - 1, Scale.AbsMinLimit)
                               : min;
            }
            else if (interval > max)
            {
                interval = Scale.MaxValue < Scale.AbsMaxLimit
                               ? Math.Min(max + 1, Scale.AbsMaxLimit)
                               : max;
            }
            else
            {
                pegged = false;
            }

            return (interval);
        }

        #endregion

        #region GetRange

        internal void GetRange(out double min, out double max)
        {
            int n = (GaugePointer.Scale is GaugeLinearScale)
                        ? ((GaugeLinearScale) Scale).AbsLength
                        : 360;

            double minPinValue = GaugePointer.HonorMinPin ? (Scale.MinPinEndOffset * n) / Dpt : 0;
            double maxPinValue = GaugePointer.HonorMaxPin ? (Scale.MaxPinEndOffset * n) / Dpt : 0;

            min = Scale.MinValue - minPinValue;
            max = Scale.MaxValue + maxPinValue;
         }

        #endregion

        #region GetRangeEx

        internal void GetRangeEx(out double min, out double max)
        {
            int n = (GaugePointer.Scale is GaugeLinearScale)
                        ? ((GaugeLinearScale)Scale).AbsLength
                        : 360;

            double minPinValue = GaugePointer.HonorMinPin ? (Scale.MinPinEndOffset * n) / Dpt : 0;
            double maxPinValue = GaugePointer.HonorMaxPin ? (Scale.MaxPinEndOffset * n) / Dpt : 0;

            min = Scale.AbsMinLimit - minPinValue;
            max = Scale.AbsMaxLimit + maxPinValue;
        }

        #endregion

        #region SwapDoubles

        protected void SwapDoubles(ref double marker, ref double origin)
        {
            double temp = marker;

            marker = origin;
            origin = temp;
        }

        #endregion

        #region RecalcLayout

        public virtual void RecalcLayout()
        {
            PointerPath = null;

            if (Scale is GaugeCircularScale)
                CalcCircularMetrics(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearMetrics(Scale as GaugeLinearScale);
        }

        #region CalcCircularMetrics

        private void CalcCircularMetrics(GaugeCircularScale scale)
        {
            _Width = (int)(scale.AbsRadius * GaugePointer.Width);
            _Length = (int)(scale.AbsRadius * GaugePointer.Length);

            if (_Width % 2 != 0)
                _Width++;
        }

        #endregion

        #region CalcLinearMetrics

        private void CalcLinearMetrics(GaugeLinearScale scale)
        {
            int n = scale.AbsWidth;

            _Width = (int)(n * GaugePointer.Width);
            _Length = (int)(n * GaugePointer.Length);

            if (_Width % 2 != 0)
                _Width++;
        }

        #endregion

        #endregion

        #region GetValueFromPoint

        public virtual double GetValueFromPoint(Point pt)
        {
            if (Scale is GaugeCircularScale)
                return (GetCValueFromPoint(Scale as GaugeCircularScale, pt));

            return (GetLValueFromPoint(Scale as GaugeLinearScale, pt));
        }

        #region GetCValueFromPoint

        private double GetCValueFromPoint(GaugeCircularScale scale, Point pt)
        {
            double minValue = scale.MinValue;
            double maxValue = scale.MaxValue;

            double startAngle = scale.StartAngle;
            double sweepAngle = scale.SweepAngle;

            double radians = GetPointRadians(pt);

            double spread = scale.MaxValue - scale.MinValue;
            double dpt = spread / scale.SweepAngle;

            if (minValue <= scale.AbsMinLimit)
                minValue -= GetCMinPinOffset(dpt, ref startAngle, ref sweepAngle);

            if (maxValue >= scale.AbsMaxLimit)
                maxValue += GetCMaxPinOffset(dpt, ref startAngle, ref sweepAngle);

            double angle = (GaugePointer.MouseDownAngle +
                scale.GetDegrees(radians - GaugePointer.MouseDownRadians) - startAngle) % 360;

            if (angle < 0)
                angle += 360;

            if (scale.Reversed == true)
            {
                angle = sweepAngle - angle;

                if (angle < 0)
                    angle += 360;
            }

            if (angle < sweepAngle)
            {
                double value = angle * dpt + minValue;

                if (GaugePointer.SnapInterval > 0)
                    value = (int)(value / GaugePointer.SnapInterval) * GaugePointer.SnapInterval;

                return (value);
            }

            if (angle > sweepAngle + (360 - sweepAngle) / 2)
            {
                double limit = minValue;

                if (limit > scale.AbsMinLimit)
                    limit = Math.Max(limit - 1, scale.AbsMinLimit);

                return (limit);
            }

            double limit2 = maxValue;

            if (limit2 < scale.AbsMaxLimit)
                limit2 = Math.Min(limit2 + 1, scale.AbsMaxLimit);

            return (limit2);
        }

        #region GetCMinPinOffset

        private double GetCMinPinOffset(
            double dpt, ref double startAngle, ref double sweepAngle)
        {
            if (GaugePointer.HonorMinPin == true)
            {
                double d = Scale.MinPinEndOffset * 360;

                if (Scale.Reversed == false)
                    startAngle -= d;

                sweepAngle += d;

                return (d * dpt);
            }

            return (0);
        }

        #endregion

        #region GetCMaxPinOffset

        private double GetCMaxPinOffset(
            double dpt, ref double startAngle, ref double sweepAngle)
        {
            if (GaugePointer.HonorMaxPin == true)
            {
                double d = Scale.MaxPinEndOffset * 360;

                if (Scale.Reversed == true)
                    startAngle -= d;

                sweepAngle += d;

                return (d * dpt);
            }

            return (0);
        }

        #endregion

        #region GetPointRadians

        internal double GetPointRadians(Point pt)
        {
            int dx = pt.X - Scale.Center.X;
            int dy = pt.Y - Scale.Center.Y;

            if (dx >= 0)
            {
                if (dy >= 0)
                    return (Math.Atan((double)dy / dx));

                return (-Math.Atan((double)dx / dy) + Math.PI * 1.5);
            }

            if (dy >= 0)
                return (-Math.Atan((double)dx / dy) + Math.PI / 2);

            return (Math.Atan((double)dy / dx) + Math.PI);
        }

        #endregion

        #endregion

        #region GetLValueFromPoint

        private double GetLValueFromPoint(GaugeLinearScale scale, Point pt)
        {
            bool pegged;

            double value = (scale.Orientation == Orientation.Horizontal)
                               ? GetLhValueFromPoint(scale, pt, out pegged)
                               : GetLvValueFromPoint(scale, pt, out pegged);

            if (GaugePointer.SnapInterval > 0)
            {
                if (pegged == false)
                    value = (int)(value / GaugePointer.SnapInterval) * GaugePointer.SnapInterval;
            }

            return (value);
        }

        #region GetLhValueFromPoint

        private double GetLhValueFromPoint(GaugeLinearScale scale, Point pt, out bool pegged)
        {
            double spread = scale.MaxValue - scale.MinValue;
            double tpd = spread / scale.ScaleBounds.Width;

            double interval = (pt.X - scale.ScaleBounds.X) * tpd;

            if (scale.Reversed == true)
                return (GetIntervalEx(scale.MaxValue - interval, out pegged));

            return (GetIntervalEx(scale.MinValue + interval, out pegged));
        }

        #endregion

        #region GetLvValueFromPoint

        private double GetLvValueFromPoint(GaugeLinearScale scale, Point pt, out bool pegged)
        {
            double spread = scale.MaxValue - scale.MinValue;
            double tpd = spread / scale.ScaleBounds.Height;

            double interval = (pt.Y - scale.ScaleBounds.Y) * tpd;

            if (scale.Reversed == true)
                return (GetIntervalEx(scale.MinValue + interval, out pegged));

            return (GetIntervalEx(scale.MaxValue - interval, out pegged));
        }

        #endregion

        #endregion

        #endregion

        #region OnMouseDown

        internal virtual void OnMouseDown(MouseEventArgs e)
        {
            GaugePointer.MouseDownAngle = 0;
            GaugePointer.MouseDownRadians = 0;
        }

        #endregion
    }
}
