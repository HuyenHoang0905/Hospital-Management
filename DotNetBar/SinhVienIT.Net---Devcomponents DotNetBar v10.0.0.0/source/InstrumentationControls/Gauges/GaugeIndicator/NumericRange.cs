using System.ComponentModel;
using System.Drawing;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class NumericRangeCollection : GenericCollection<NumericRange>
    {
        #region GetValueRange

        public NumericRange GetValueRange(double value)
        {
            foreach (NumericRange range in this)
            {
                if (value >= range.StartValue && value <= range.EndValue)
                    return (range);
            }

            return (null);
        }

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            NumericRangeCollection copy = new NumericRangeCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(NumericRangeCollection copy)
        {
            foreach (NumericRange item in this)
            {
                NumericRange ic = new NumericRange();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class NumericRange : IndicatorRange
    {
        #region Private variables

        private Color _DigitColor;
        private Color _DigitDimColor;
        private Color _DecimalColor;
        private Color _DecimalDimColor;

        private NumericIndicator _NumericIndicator;

        #endregion

        #region Public properties

        #region DecimalColor

        /// <summary>
        /// Gets or sets the default Decimal Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "FireBrick")]
        [Description("Indicates the default Decimal Color.")]
        public Color DecimalColor
        {
            get { return (_DecimalColor); }

            set
            {
                if (_DecimalColor != value)
                {
                    _DecimalColor = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region DecimalDimColor

        /// <summary>
        /// Gets or sets the default Decimal Dim Color (Dim LED color)
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Empty")]
        [Description("Indicates the default Decimal Dim Color (Dim LED color).")]
        public Color DecimalDimColor
        {
            get { return (_DecimalDimColor); }

            set
            {
                if (_DecimalDimColor != value)
                {
                    _DecimalDimColor = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region DigitColor

        /// <summary>
        /// Gets or sets the default Digit Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "SteelBlue")]
        [Description("Indicates the default Digit Color.")]
        public Color DigitColor
        {
            get { return (_DigitColor); }

            set
            {
                if (_DigitColor != value)
                {
                    _DigitColor = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region DigitDimColor

        /// <summary>
        /// Gets or sets the default Digit Dim Color (Dim LED color)
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Empty")]
        [Description("Indicates the default Digit Dim Color (Dim LED color).")]
        public Color DigitDimColor
        {
            get { return (_DigitDimColor); }

            set
            {
                if (_DigitDimColor != value)
                {
                    _DigitDimColor = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region NumericIndicator

        /// <summary>
        /// Gets or sets the NumericIndicator
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NumericIndicator NumericIndicator
        {
            get { return (_NumericIndicator); }
            internal set { _NumericIndicator = value; }
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            NumericRange copy = new NumericRange();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            NumericRange c = copy as NumericRange;

            if (c != null)
            {
                base.CopyToItem(c);

                c.DecimalColor = _DecimalColor;
                c.DecimalDimColor = _DecimalDimColor;
                c.DigitColor = _DigitColor;
                c.DigitDimColor = _DigitDimColor;
            }
        }

        #endregion

    }
}
