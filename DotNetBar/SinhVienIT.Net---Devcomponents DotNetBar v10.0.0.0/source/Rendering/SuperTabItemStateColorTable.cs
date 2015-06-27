using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the SuperTabItem states
    /// </summary>
    [TypeConverter(typeof(SuperTabItemStateColorTableConvertor))]
    public class SuperTabItemStateColorTable : ICloneable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabItemStateColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabItemStateColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _Text = Color.Empty;
        private Color _InnerBorder = Color.Empty;
        private Color _OuterBorder = Color.Empty;
        private Color _CloseMarker = Color.Empty;
        private Color _SelectionMarker = Color.Empty;

        private SuperTabLinearGradientColorTable _Background;

        #endregion

        public SuperTabItemStateColorTable()
        {
            _Background = new SuperTabLinearGradientColorTable();
            _Background.ColorTableChanged += Background_ColorTableChanged;
        }

        #region Public properties

        #region Background

        /// <summary>
        /// Gets or sets the background colors.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the background colors.")]
        public SuperTabLinearGradientColorTable Background
        {
            get { return (_Background); }

            set
            {
                if (_Background.Equals(value) == false)
                {
                    if (_Background != null)
                        _Background.ColorTableChanged -= Background_ColorTableChanged;

                    _Background = value;

                    if (_Background != null)
                        _Background.ColorTableChanged += Background_ColorTableChanged;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackground()
        {
            return (_Background.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackground()
        {
            Background = new SuperTabLinearGradientColorTable();
        }

        #endregion

        #region OuterBorder

        /// <summary>
        /// Gets or sets the outer border color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the outer border color.")]
        public Color OuterBorder
        {
            get { return (_OuterBorder); }

            set
            {
                if (_OuterBorder != value)
                {
                    _OuterBorder = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOuterBorder()
        {
            return (_OuterBorder != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetOuterBorder()
        {
            OuterBorder = Color.Empty;
        }

        #endregion

        #region InnerBorder

        /// <summary>
        /// Gets or sets the inner border color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the inner border color.")]
        public Color InnerBorder
        {
            get { return (_InnerBorder); }

            set
            {
                if (_InnerBorder != value)
                {
                    _InnerBorder = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeInnerBorder()
        {
            return (_InnerBorder != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetInnerBorder()
        {
            InnerBorder = Color.Empty;
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Text color.")]
        public Color Text
        {
            get { return (_Text); }

            set
            {
                if (_Text != value)
                {
                    _Text = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeText()
        {
            return (_Text.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetText()
        {
            Text = Color.Empty;
        }

        #endregion

        #region CloseMarker

        /// <summary>
        /// Gets or sets the Close Marker color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Close Marker color.")]
        public Color CloseMarker
        {
            get { return (_CloseMarker); }

            set
            {
                if (_CloseMarker != value)
                {
                    _CloseMarker = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCloseMarker()
        {
            return (_CloseMarker != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetCloseMarker()
        {
            CloseMarker = Color.Empty;
        }

        #endregion

        #region SelectionMarker

        /// <summary>
        /// Gets or sets the Selection Marker color.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Selection Marker color.")]
        public Color SelectionMarker
        {
            get { return (_SelectionMarker); }

            set
            {
                if (_SelectionMarker != value)
                {
                    _SelectionMarker = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectionMarker()
        {
            return (_SelectionMarker != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectionMarker()
        {
            SelectionMarker = Color.Empty;
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (_Background.IsEmpty == false)
                    return (false);

                if (_OuterBorder != Color.Empty)
                    return (false);

                if (_InnerBorder != Color.Empty)
                    return (false);

                if (_Text != Color.Empty)
                    return (false);

                if (_CloseMarker != Color.Empty)
                    return (false);

                if (_SelectionMarker != Color.Empty)
                    return (false);

                return (true);
            }
        }

        #endregion

        #endregion

        #region Background_ColorTableChanged

        void Background_ColorTableChanged(object sender, EventArgs e)
        {
            OnColorTableChanged();
        }

        #endregion

        #region OnColorTableChanged

        private void OnColorTableChanged()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            SuperTabItemStateColorTable sct = new SuperTabItemStateColorTable();

            sct.Background = (SuperTabLinearGradientColorTable)Background.Clone();

            sct.Text = Text;
            sct.OuterBorder = OuterBorder;
            sct.InnerBorder = InnerBorder;
            sct.CloseMarker = CloseMarker;
            sct.SelectionMarker = SelectionMarker;

            return (sct);
        }

        #endregion
    }

    #region SuperTabItemStateColorTableConvertor

    public class SuperTabItemStateColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                SuperTabItemStateColorTable sct = value as SuperTabItemStateColorTable;

                if (sct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (sct.Background.Colors != null)
                        return (cvt.ConvertToString(sct.Background.Colors[0]));

                    if (sct.InnerBorder.IsEmpty == false)
                        return (cvt.ConvertToString(sct.InnerBorder));

                    if (sct.OuterBorder.IsEmpty == false)
                        return (cvt.ConvertToString(sct.OuterBorder));

                    if (sct.Text.IsEmpty == false)
                        return (cvt.ConvertToString(sct.Text));

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
