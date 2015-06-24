using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;

namespace DevComponents.Instrumentation
{
    [TypeConverter(typeof(LabelLayoutConvertor))]
    public class LabelLayout : IDisposable, ICloneable
    {
        #region Events

        public event EventHandler<EventArgs> LabelLayoutChanged;

        #endregion

        #region Private variables

        private DisplayPlacement _Placement;
        private float _ScaleOffset;

        private Font _Font;
        private Font _AbsFont;
        private float _Angle;

        private bool _AutoSize;
        private bool _RotateLabel;
        private bool _AdaptiveLabel;
        private bool _AutoOrientLabel;

        private Color _ForeColor;

        #endregion

        public LabelLayout()
        {
            _ForeColor = Color.Black;

            _Placement = DisplayPlacement.Near;

            _AutoSize = true;
            _AutoOrientLabel = true;
            _RotateLabel = true;
        }

        #region Public properties

        #region AdaptiveLabel

        /// <summary>
        /// Gets or sets whether labels are to adapt to the scale shape
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(false)]
        [Description("Indicates whether labels are to adapt to the scale shape.")]
        public bool AdaptiveLabel
        {
            get { return (_AdaptiveLabel); }

            set
            {
                if (_AdaptiveLabel != value)
                {
                    _AdaptiveLabel = value;

                    OnLabelLayoutChanged();
                }
            }
        }

                #endregion

        #region Angle

        /// <summary>
        /// Gets or sets the additional number of degrees the label will be rotated
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(0f)]
        [Description("Indicates the additional number of degrees the label will be rotated.")]
        [Editor("DevComponents.Instrumentation.Design.AngleRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [NotifyParentProperty(true)]
        public float Angle
        {
            get { return (_Angle); }

            set
            {
                if (_Angle != value)
                {
                    _Angle = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region AutoOrientLabel

        /// <summary>
        /// Gets or sets whether the label will be auto oriented away from being upside down
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether the label will be auto oriented away from being upside down.")]
        public bool AutoOrientLabel
        {
            get { return (_AutoOrientLabel); }

            set
            {
                if (_AutoOrientLabel != value)
                {
                    _AutoOrientLabel = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region AutoSize

        /// <summary>
        /// Gets or sets whether the label Font size is auto sized
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether the label Font size is auto sized.")]
        public bool AutoSize
        {
            get { return (_AutoSize); }

            set
            {
                if (_AutoSize != value)
                {
                    _AutoSize = value;
                    AbsFont = null;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region Font

        /// <summary>
        /// Gets or sets the Font to use for the label
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Indicates the Font to use for the label.")]
        public Font Font
        {
            get
            {
                if (_Font == null)
                    _Font = new Font("Microsoft SanSerif", 12);

                return (_Font);
            }

            set
            {
                if (_Font != null)
                    _Font.Dispose();

                _Font = value;
                AbsFont = null;

                OnLabelLayoutChanged();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual bool ShouldSerializeFont()
        {
            if (_Font == null)
                return (false);

            using (Font font = new Font("Microsoft SanSerif", 12))
                return (_Font.Equals(font) == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        internal virtual void ResetFont()
        {
            Font = new Font("Microsoft SanSerif", 12);
        }

        #endregion

        #region ForeColor

        /// <summary>
        /// Gets or sets the Label text Color
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        [Description("Indicates the Label text Color.")]
        public Color ForeColor
        {
            get { return (_ForeColor); }

            set
            {
                if (_ForeColor != value)
                {
                    _ForeColor = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region Placement

        /// <summary>
        /// Gets or sets the Placement of the Label with respect to the Scale
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(DisplayPlacement.Near)]
        [Description("Indicates the Placement of the Label with respect to the Scale.")]
        public DisplayPlacement Placement
        {
            get { return (_Placement); }

            set
            {
                if (_Placement != value)
                {
                    _Placement = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region RotateLabel

        /// <summary>
        /// Gets or sets whether labels are rotated along the scale
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(true)]
        [Description("Indicates whether labels are rotated along the scale.")]
        public bool RotateLabel
        {
            get { return (_RotateLabel); }

            set
            {
                if (_RotateLabel != value)
                {
                    _RotateLabel = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #region ScaleOffset

        /// <summary>
        /// Gets or sets the distance from the Label to the Scale, measured as a percentage
        /// </summary>
        [Browsable(true)]
        [Category("Layout"), DefaultValue(0f)]
        [Editor("DevComponents.Instrumentation.Design.OffsetRangeValueEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        [Description("Indicates the distance from the Label to the Scale, measured as a percentage.")]
        public float ScaleOffset
        {
            get { return (_ScaleOffset); }

            set
            {
                if (_ScaleOffset != value)
                {
                    _ScaleOffset = value;

                    OnLabelLayoutChanged();
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsFont

        internal Font AbsFont
        {
            get { return (_AbsFont); }

            set
            {
                if (_AbsFont != null)
                    _AbsFont.Dispose();

                _AbsFont = value;
            }
        }

        #endregion

        #endregion

        #region OnLabelLayoutChanged

        private void OnLabelLayoutChanged()
        {
            if (LabelLayoutChanged != null)
                LabelLayoutChanged(this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Font = null;
            AbsFont = null;
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            LabelLayout copy = new LabelLayout();

            CopyToItem(copy);

            return (copy);
        }

        #endregion

        #region CopyToItem

        internal virtual void CopyToItem(LabelLayout copy)
        {
            copy.AdaptiveLabel = _AdaptiveLabel;
            copy.Angle = _Angle;
            copy.AutoOrientLabel = _AutoOrientLabel;
            copy.AutoSize = _AutoSize;

            if (_Font != null)
                copy.Font = (Font)_Font.Clone();

            copy.ForeColor = _ForeColor;
            copy.Placement = _Placement;
            copy.RotateLabel = _RotateLabel;
            copy.ScaleOffset = _ScaleOffset;
        }

        #endregion
    }

    #region LabelLayoutConvertor

    public class LabelLayoutConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                LabelLayout ll = value as LabelLayout;

                if (ll != null)
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
