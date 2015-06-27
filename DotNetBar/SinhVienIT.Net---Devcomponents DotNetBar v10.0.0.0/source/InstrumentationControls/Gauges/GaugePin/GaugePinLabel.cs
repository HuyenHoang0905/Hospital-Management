using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class GaugePinLabel : GaugeBaseLabel
    {
        #region Private variables

        private string _Text;

        private GaugePin _GaugePin;
        private LabelPoint _LabelPoint;

        #endregion

        public GaugePinLabel(GaugePin gaugePin)
        {
            _Text = "";

            _GaugePin = gaugePin;

            Scale = gaugePin.Scale;
        }

        #region Public properties

        #region Text

        /// <summary>
        /// Gets or sets the Label text
        /// </summary>
        [Browsable(true)]
        [Category("Appearance"), DefaultValue("")]
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

        #endregion

        #region Internal properties

        #region GaugePin

        internal GaugePin GaugePin
        {
            get { return (_GaugePin); }

            set
            {
                _GaugePin = value;

                OnGaugeItemChanged(true);
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

                _LabelPoint = null;

                if (String.IsNullOrEmpty(_Text) == false)
                {
                    if (Scale is GaugeCircularScale)
                        CalcCircularLabelPoint(Scale as GaugeCircularScale);

                    else if (Scale is GaugeLinearScale)
                        CalcLinearLabelPoint(Scale as GaugeLinearScale);
                }
            }
        }

        #region CalcCircularLabelPoint

        private void CalcCircularLabelPoint(GaugeCircularScale scale)
        {
            if (Radius > 0)
            {
                _LabelPoint = new LabelPoint();

                _LabelPoint.Angle = _GaugePin.Angle;
                _LabelPoint.Point = scale.GetPoint(Radius, _LabelPoint.Angle);
            }
        }

        #endregion

        #region CalcLinearLabelPoint

        private void CalcLinearLabelPoint(GaugeLinearScale scale)
        {
            _LabelPoint = new LabelPoint();

            if (scale.Orientation == Orientation.Horizontal)
            {
                _LabelPoint.Point = new Point(
                    _GaugePin.Bounds.X + _GaugePin.Bounds.Width / 2,
                    scale.ScaleBounds.Y + Offset);
            }
            else
            {
                _LabelPoint.Point = new Point(
                   scale.ScaleBounds.X + Offset,
                   _GaugePin.Bounds.Y + _GaugePin.Bounds.Height / 2);
            }
        }

        #endregion

        #endregion

        #region OnPaint

        public override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            RecalcLayout();

            if (_LabelPoint != null && _LabelPoint.Visible == true)
            {
                using (Brush br = new SolidBrush(Layout.ForeColor))
                    PaintLabel(g, _Text, br, _LabelPoint, AbsFont);
            }
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            GaugePinLabel copy = new GaugePinLabel(_GaugePin);

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            GaugePinLabel c = copy as GaugePinLabel;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Text = _Text;
            }
        }

        #endregion
    }
}
