using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class GaugeCustomLabelCollection : GenericCollection<GaugeCustomLabel>
    {
        #region ICloneable Members

        public override object Clone()
        {
            GaugeCustomLabelCollection copy = new GaugeCustomLabelCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(GaugeCustomLabelCollection copy)
        {
            foreach (GaugeCustomLabel item in this)
            {
                GaugeCustomLabel ic = new GaugeCustomLabel();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class GaugeCustomLabel : GaugeBaseLabel
    {
        #region Private variables

        private string _Text;
        private double _Value;

        private GaugeTickMarkLabel _TickMark;
        private LabelPoint _LabelPoint;

        #endregion

        public GaugeCustomLabel()
        {
            _Text = "Text";
            _Value = double.NaN;

            _TickMark = new GaugeTickMarkLabel(
                Scale, GaugeTickMarkRank.Custom, GaugeMarkerStyle.Trapezoid, .09f, .14f, double.NaN);

            HookEvents(true);
        }

        #region Public properties

        #region Scale

        /// <summary>
        /// Gets the label's associated Scale
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override GaugeScale Scale
        {
            get { return (base.Scale); }

            internal set
            {
                base.Scale = value;

                _TickMark.Scale = value;
            }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the Label text
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue("Text")]
        [Description("Indicates the Label text.")]
        public string Text
        {
            get { return (_Text); }

            set
            {
                if (value != null && value.Equals(_Text) == false)
                {
                    _Text = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #region TickMark

        /// <summary>
        /// Gets the Label Tickmark definition
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Contains the Label TickMark layout properties.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeTickMarkLabel TickMark
        {
            get { return (_TickMark); }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the Label scale value
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(double.NaN)]
        [Description("Indicates the Label scale value.")]
        public double Value
        {
            get { return (_Value); }

            set
            {
                if (_Value != value)
                {
                    _Value = value;

                    OnGaugeItemChanged(true);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region NeedRecalcLayout

        internal override bool NeedRecalcLayout
        {
            get { return (base.NeedRecalcLayout); }

            set
            {
                base.NeedRecalcLayout = value;

                if (value == true)
                    _TickMark.NeedRecalcLayout = true;
            }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _TickMark.GaugeItemChanged += LabelTickMark_GaugeItemChanged;
            }
            else
            {
                _TickMark.GaugeItemChanged -= LabelTickMark_GaugeItemChanged;
            }
        }

        #endregion

        #region Event processing

        void LabelTickMark_GaugeItemChanged(object sender, EventArgs e)
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

                if (_Value >= Scale.MinValue && _Value <= Scale.MaxValue)
                {
                    if (Scale is GaugeCircularScale)
                        CalcCircularLabelPoint(Scale as GaugeCircularScale);

                    else if (Scale is GaugeLinearScale)
                        CalcLinearLabelPoint(Scale as GaugeLinearScale);

                    _TickMark.Interval = _Value - Scale.MinValue;
                    _TickMark.NeedRecalcLayout = true;

                    _TickMark.RecalcLayout();
                }
                else
                {
                    _LabelPoint = null;
                    _TickMark.TickPoint = null;
                }
            }
        }

        #region CalcCircularLabelPoint

        private void CalcCircularLabelPoint(GaugeCircularScale scale)
        {
            double spread = scale.MaxValue - scale.MinValue;
            double dpt = scale.SweepAngle / spread;
            double interval = _Value - scale.MinValue;

            double n = interval * dpt;
            
            _LabelPoint = new LabelPoint();

            _LabelPoint.Angle = (float)(scale.StartAngle + (scale.Reversed ? scale.SweepAngle - n : n));
            _LabelPoint.Point = scale.GetPoint(Radius, _LabelPoint.Angle);
            _LabelPoint.Interval = interval;
        }

        #endregion

        #region CalcLinearLabelPoint

        private void CalcLinearLabelPoint(GaugeLinearScale scale)
        {
            double ticks = scale.MaxValue - scale.MinValue;

            _LabelPoint = new LabelPoint();

            if (scale.Orientation == Orientation.Horizontal)
            {
                double dpt = scale.ScaleBounds.Width / ticks;
                int dx = (int)((_Value - Scale.MinValue) * dpt);

                int x = (scale.Reversed == true)
                    ? Scale.Bounds.Right - dx : Scale.Bounds.X + dx;

                _LabelPoint.Point = new Point(x, scale.ScaleBounds.Y + Offset);
            }
            else
            {
                double dpt = scale.ScaleBounds.Height / ticks;
                int dy = (int)((_Value - Scale.MinValue) * dpt);

                int y = (scale.Reversed == true)
                    ? Scale.Bounds.Top + dy : Scale.Bounds.Bottom - dy;

                _LabelPoint.Point = new Point(scale.ScaleBounds.X + Offset, y);
            }

            _LabelPoint.Interval = _Value;
        }

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RecalcLayout();

            if (_LabelPoint != null)
            {
                if (_LabelPoint.Visible == true)
                {
                    if (Scale.GaugeControl.OnPreRenderScaleCustomLabel(e, this) == false)
                    {
                        using (Brush br = new SolidBrush(Layout.ForeColor))
                            PaintLabel(g, _Text, br, _LabelPoint, AbsFont);

                        Scale.GaugeControl.OnPostRenderScaleCustomLabel(e, this);
                    }
                }
            }
        }

        #endregion

        #region OnDispose

        protected override void OnDispose()
        {
            HookEvents(false);

            base.OnDispose();
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugeCustomLabel copy = new GaugeCustomLabel();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugeCustomLabel c = copy as GaugeCustomLabel;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Text = _Text;
                c.Value = _Value;

                _TickMark.CopyToItem(c.TickMark);
            }
        }

        #endregion
    }
}
