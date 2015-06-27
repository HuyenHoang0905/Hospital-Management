using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using DevComponents.Instrumentation.Primitives;

namespace DevComponents.Instrumentation
{
    public class StateRangeCollection : GenericCollection<StateRange>
    {
        #region GetValueRange

        public StateRange GetValueRange(double value)
        {
            foreach (StateRange range in this)
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
            StateRangeCollection copy = new StateRangeCollection();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal void CopyToItem(StateRangeCollection copy)
        {
            foreach (StateRange item in this)
            {
                StateRange ic = new StateRange();

                item.CopyToItem(ic);

                copy.Add(ic);
            }
        }

        #endregion
    }

    public class StateRange : IndicatorRange
    {
        #region Private variables

        private Image _Image;
        private StateIndicator _StateIndicator;

        private string _Text;
        private Color _TextColor;
        private float _TextVerticalOffset;
        private float _TextHorizontalOffset;

        #endregion

        public StateRange()
        {
            TextColor = Color.Black;
        }

        #region Public properties

        #region Image

        /// <summary>
        /// Gets or sets the Image to use
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(null)]
        [Description("Indicates the Image to use.")]
        public Image Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region StateIndicator

        /// <summary>
        /// Gets or sets the StateIndicator
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StateIndicator StateIndicator
        {
            get { return (_StateIndicator); }
            internal set { _StateIndicator = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text to be displayed
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(null)]
        [Description("Indicates the text to be displayed.")]
        public string Text
        {
            get { return (_Text); }

            set
            {
                if (_Text != value)
                {
                    _Text = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets or sets the text Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        [Description("Indicates the text Color.")]
        public Color TextColor
        {
            get { return (_TextColor); }

            set
            {
                if (_TextColor != value)
                {
                    _TextColor = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region TextHorizontalOffset

        /// <summary>
        /// Gets or sets the horizontal distance to offset the Indicator Text, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the horizontal distance to offset the Indicator Text, measured as a percentage.")]
        public float TextHorizontalOffset
        {
            get { return (_TextHorizontalOffset); }

            set
            {
                if (_TextHorizontalOffset != value)
                {
                    _TextHorizontalOffset = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #region TextVerticalOffset

        /// <summary>
        /// Gets or sets the vertical distance to offset the Indicator Text, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the vertical distance to offset the Indicator Text, measured as a percentage.")]
        public float TextVerticalOffset
        {
            get { return (_TextVerticalOffset); }

            set
            {
                if (_TextVerticalOffset != value)
                {
                    _TextVerticalOffset = value;

                    OnIndicatorRangeChanged();
                }
            }
        }

        #endregion

        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            StateRange copy = new StateRange();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        public override void CopyToItem(GaugeItem copy)
        {
            StateRange c = copy as StateRange;

            if (c != null)
            {
                base.CopyToItem(c);

                c.Image = _Image;
                c.Text = _Text;
                c.TextColor = _TextColor;
                c.TextHorizontalOffset = _TextHorizontalOffset;
                c.TextVerticalOffset = _TextVerticalOffset;
            }
        }

        #endregion

    }
}
