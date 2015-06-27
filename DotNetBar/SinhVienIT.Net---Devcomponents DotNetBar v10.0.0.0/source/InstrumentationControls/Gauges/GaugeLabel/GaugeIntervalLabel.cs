using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(GaugeLabelConvertor))]
    public class GaugeIntervalLabel : GaugeBaseLabel
    {
        #region Private variables

        private double _Interval;
        private double _IntervalOffset;

        private LabelPoint[] _LabelPoints;

        private bool _ShowMaxLabel;
        private bool _ShowMinLabel;

        private string _FormatString;

        #endregion

        public GaugeIntervalLabel(GaugeScale scale)
        {
            Scale = scale;

            _Interval = double.NaN;
            _IntervalOffset = double.NaN;

            _ShowMinLabel = true;
            _ShowMaxLabel = true;
        }

        #region Hidden properties

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Name
        {
            get { return (base.Name); }
            set { base.Name = value; }
        }

        #endregion

        #region Public properties

        #region FormatString

        /// <summary>
        /// Gets or sets the .Net format string used to display all non-custom defined labels.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the .Net format string used to display all non-custom defined labels.")]
        [Editor("DevComponents.Instrumentation.Design.FormatStringEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public string FormatString
        {
            get { return (_FormatString); }

            set
            {
                if (_FormatString != value)
                {
                    _FormatString = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region Interval

        /// <summary>
        /// Gets or sets the Label Interval
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(double.NaN)]
        [Description("Indicates the Label Interval.")]
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

        #endregion

        #region IntervalOffset

        /// <summary>
        /// Gets or sets the Label Interval Offset
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(double.NaN)]
        [Description("Indicates the Label Interval Offset.")]
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

        #region ShowMaxLabel

        /// <summary>
        /// Gets or sets whether to show the Maximum Scale label
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether to show the Maximum Scale label.")]
        public bool ShowMaxLabel
        {
            get { return (_ShowMaxLabel); }

            set
            {
                if (_ShowMaxLabel != value)
                {
                    _ShowMaxLabel = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region ShowMinLabel

        /// <summary>
        /// Gets or sets whether to show the Minimum Scale label
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether to show the Minimum Scale label.")]
        public bool ShowMinLabel
        {
            get { return (_ShowMinLabel); }

            set
            {
                if (_ShowMinLabel != value)
                {
                    _ShowMinLabel = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region RecalcLayout

        public override void RecalcLayout()
        {
            if (NeedRecalcLayout == true)
            {
                base.RecalcLayout();

                CalcLabelPoints();
            }
        }

        #region CalcLabelPoints

        private void CalcLabelPoints()
        {
            if (Scale is GaugeCircularScale)
                CalcCircularLabelPoints(Scale as GaugeCircularScale);

            else if (Scale is GaugeLinearScale)
                CalcLinearLabelPoints(Scale as GaugeLinearScale);
        }

        #region CalcCircularLabelPoints

        private void CalcCircularLabelPoints(GaugeCircularScale scale)
        {
            double labelInterval = (_Interval.Equals(double.NaN) ?
                scale.MajorTickMarks.Interval : _Interval);

            double labelIntervalOffset = (_IntervalOffset.Equals(double.NaN) ?
                scale.MajorTickMarks.IntervalOffset : _IntervalOffset);

            double spread = scale.MaxValue - scale.MinValue;
            double dpt = scale.SweepAngle / spread;

            int n = GetPointCount(spread, labelInterval, labelIntervalOffset);

            if (n > 0)
            {
                double startAngle = scale.StartAngle;
                double interval = (_ShowMinLabel == true ? 0 : labelIntervalOffset > 0 ? labelIntervalOffset : labelInterval);

                int dir = (scale.Reversed ? -1 : 1);

                if (scale.Reversed == true)
                    startAngle += scale.SweepAngle;

                _LabelPoints = new LabelPoint[n];

                for (int i = 0; i < n; i++)
                {
                    _LabelPoints[i] = new LabelPoint();

                    if (interval + scale.MinValue > scale.MaxValue)
                        interval = scale.MaxValue - scale.MinValue;

                    _LabelPoints[i].Angle = (float)(startAngle + (interval * dpt) * dir);
                    _LabelPoints[i].Point = scale.GetPoint(Radius, _LabelPoints[i].Angle);
                    _LabelPoints[i].Interval = interval;

                    if (interval >= labelIntervalOffset)
                        interval += labelInterval;
                    else
                        interval = labelIntervalOffset;
                }
            }
            else
            {
                _LabelPoints = null;
            }
        }

        #endregion

        #region CalcLinearLabelPoints

        private void CalcLinearLabelPoints(GaugeLinearScale scale)
        {
            double labelInterval = (_Interval.Equals(double.NaN) ?
                scale.MajorTickMarks.Interval : _Interval);

            double labelIntervalOffset = (_IntervalOffset.Equals(double.NaN) ?
                scale.MajorTickMarks.IntervalOffset : _IntervalOffset);

            double spread = Math.Abs(scale.MaxValue - scale.MinValue);
            double dpt = scale.AbsScaleLength / spread;

            int n = GetPointCount(spread, labelInterval, labelIntervalOffset);

            if (n > 0)
            {
                if (scale.Orientation == Orientation.Horizontal)
                    CalcHorizontalLabelPoints(scale, n, dpt, labelInterval, labelIntervalOffset);
                else
                    CalcVerticalLabelPoints(scale, n, dpt, labelInterval, labelIntervalOffset);
            }
            else
            {
                _LabelPoints = null;
            }
        }

        #region CalcHorizontalLabelPoints

        private void CalcHorizontalLabelPoints(GaugeLinearScale scale,
            int n, double dpt, double labelInterval, double labelIntervalOffset)
        {
            double interval = (_ShowMinLabel == true ?
                0 : labelIntervalOffset > 0 ? labelIntervalOffset : labelInterval);

            int y = scale.ScaleBounds.Y + Offset;

            _LabelPoints = new LabelPoint[n];

            for (int i = 0; i < n; i++)
            {
                _LabelPoints[i] = new LabelPoint();

                if (interval + Scale.MinValue > Scale.MaxValue)
                    interval = Scale.MaxValue - Scale.MinValue;

                int x = (scale.Reversed == true)
                             ? (int)(Scale.Bounds.Right - dpt * interval)
                             : (int)(Scale.Bounds.X + dpt * interval);

                _LabelPoints[i].Point = new Point(x, y);
                _LabelPoints[i].Interval = interval;

                if (interval >= labelIntervalOffset)
                    interval += labelInterval;
                else
                    interval = labelIntervalOffset;
            }
        }

        #endregion

        #region CalcVerticalLabelPoints

        private void CalcVerticalLabelPoints(GaugeLinearScale scale,
            int n, double dpt, double labelInterval, double labelIntervalOffset)
        {
            double interval = (_ShowMinLabel == true ?
                0 : labelIntervalOffset > 0 ? labelIntervalOffset : labelInterval);

            int x = scale.ScaleBounds.X + Offset;

            _LabelPoints = new LabelPoint[n];

            for (int i = 0; i < n; i++)
            {
                _LabelPoints[i] = new LabelPoint();

                if (interval + Scale.MinValue > Scale.MaxValue)
                    interval = Scale.MaxValue - Scale.MinValue;

                int y = (scale.Reversed == true)
                     ? (int)(Scale.Bounds.Top + dpt * interval)
                     : (int)(Scale.Bounds.Bottom - dpt * interval);

                _LabelPoints[i].Point = new Point(x, y);
                _LabelPoints[i].Interval = interval;

                if (interval >= labelIntervalOffset)
                    interval += labelInterval;
                else
                    interval = labelIntervalOffset;
            }
        }

        #endregion

        #endregion

        #region GetPointCount

        private int GetPointCount(double spread,
            double labelInterval, double labelIntervalOffset)
        {
            int n = (int)Math.Ceiling((spread - labelIntervalOffset) / labelInterval) + 1;

            if (labelIntervalOffset == 0)
            {
                if (_ShowMinLabel == false)
                    n--;
            }
            else
            {
                if (_ShowMinLabel == true)
                    n++;
            }

            if (_ShowMaxLabel == false)
                n--;

            return (n);
        }

        #endregion

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RecalcLayout();

            if (Scale.GaugeControl.OnPreRenderScaleTickMarkLabels(e, Scale) == false)
            {
                if (_LabelPoints != null)
                {
                    Font font = AbsFont;
                    SolidBrush br = null;

                    try
                    {
                        foreach (LabelPoint lp in _LabelPoints)
                        {
                            if (CanDisplayLabel(lp) == true)
                            {
                                br = GetLabelBrush(br, lp);

                                PaintLabel(g, GetLabelText(lp), br, lp, font);
                            }
                        }
                    }
                    finally
                    {
                        if (br != null)
                            br.Dispose();
                    }
                }

                Scale.GaugeControl.OnPostRenderScaleTickMarkLabels(e, Scale);
            }
        }

        #region GetLabelText

        private string GetLabelText(LabelPoint labelPoint)
        {
            double n = Scale.GetIntervalValue(labelPoint.Interval);

            if (String.IsNullOrEmpty(_FormatString) == false)
            {
                try
                {
                    switch (_FormatString[0])
                    {
                        case 'X':
                        case 'x':
                            return (String.Format("{0:" + _FormatString + "}", (int) n));

                        default:
                            return (String.Format("{0:" + _FormatString + "}", n));
                    }
                }
                catch
                {
                }
            }

            return (n.ToString());
        }

        #endregion

        #region GetLabelBrush

        private SolidBrush GetLabelBrush(SolidBrush br, LabelPoint lp)
        {
            Color color = GetLabelColor(lp);

            if (br == null || br.Color != color)
            {
                if (br != null)
                    br.Dispose();

                br = new SolidBrush(color);
            }

            return (br);
        }

        #endregion

        #region GetLabelColor

        private Color GetLabelColor(LabelPoint lp)
        {
            Color labelColor = Scale.GetRangeLabelColor(lp.Interval);

            if (labelColor.IsEmpty == true)
                labelColor = Scale.GetSectionLabelColor(lp.Interval);

            if (labelColor.IsEmpty == true)
                labelColor = Layout.ForeColor;

            return (labelColor);
        }

        #endregion

        #region CanDisplayLabel

        private bool CanDisplayLabel(LabelPoint labelPoint)
        {
            if (labelPoint.Visible == false)
                return (false);

            if (Scale.HasCustomLabels == true)
            {
                foreach (GaugeCustomLabel label in Scale.CustomLabels)
                {
                    if (label.Visible == true)
                    {
                        if (label.Layout.Placement == Layout.Placement)
                        {
                            if (label.Value == Scale.MinValue + labelPoint.Interval)
                                return (false);
                        }
                    }
                }
            }

            return (true);
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeIntervalLabel copy = new GaugeIntervalLabel(Scale);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeIntervalLabel c = copy as GaugeIntervalLabel;

            if (c != null)
            {
                base.CopyToItem(c);

                c.FormatString = _FormatString;
                c.Interval = _Interval;
                c.IntervalOffset = _IntervalOffset;
                c.ShowMaxLabel = _ShowMaxLabel;
                c.ShowMinLabel = _ShowMinLabel;
            }
        }

        #endregion
    }

    #region GaugeLabelConvertor

    public class GaugeLabelConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                GaugeIntervalLabel label = value as GaugeIntervalLabel;

                if (label != null)
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
