using System;
using System.ComponentModel;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class IndicatorRangeCollection : GenericCollection<IndicatorRange>
    {
        #region ICloneable Members

        public override object Clone()
        {
            IndicatorRangeCollection copy = new IndicatorRangeCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(IndicatorRangeCollection copy)
        {
            foreach (IndicatorRange item in this)
            {
                IndicatorRange ic = new IndicatorRange();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class IndicatorRange : GaugeItem
    {
        #region Events

        [Description("Occurs when the IndicatorRange changes.")]
        public event EventHandler<EventArgs> IndicatorRangeChanged;

        #endregion

        #region Private variables

        private double _StartValue;
        private double _EndValue;

        private GradientFillColor _BackColor;

        #endregion

        #region Hidden properties

        #region Tooltip

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Tooltip
        {
            get { return (base.Tooltip); }
            set { base.Tooltip = value; }
        }

        #endregion

        #region Visible

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Visible
        {
            get { return (base.Visible); }
            set { base.Visible = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region BackColor

        /// <summary>
        /// Gets or sets the BackColor
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

                OnIndicatorRangeChanged();
            }
        }

        #endregion

        #region EndValue

        /// <summary>
        /// Gets or sets the Ending range value
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(double.NaN)]
        [Description("Indicates the Ending range value.")]
        public double EndValue
        {
            get { return (_EndValue); }

            set
            {
                if (_EndValue != value)
                {
                    _EndValue = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region StartValue

        /// <summary>
        /// Gets or sets the Starting range value
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(double.NaN)]
        [Description("Indicates the Starting range value.")]
        public double StartValue
        {
            get { return (_StartValue); }

            set
            {
                if (_StartValue != value)
                {
                    _StartValue = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Event processing

        #region BackColor processing

        void BackColor_ColorTableChanged(object sender, EventArgs e)
        {
            OnIndicatorRangeChanged();
        }

        #endregion

        #region OnIndicatorRangeChanged

        internal void OnIndicatorRangeChanged()
        {
            if (IndicatorRangeChanged != null)
                IndicatorRangeChanged(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            IndicatorRange copy = new IndicatorRange();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            IndicatorRange c = copy as IndicatorRange;

            if (c != null)
            {
                base.CopyToItem(c);

                c.EndValue = _EndValue;
                c.StartValue = _StartValue;

                if (_BackColor != null)
                    c.BackColor = (GradientFillColor) _BackColor.Clone();
            }
        }

        #endregion

    }
}
