using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for SuperTab states
    /// </summary>
    [TypeConverter(typeof(SuperTabColorTableConvertor))]
    public class SuperTabColorTable : ICloneable, IDisposable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _OuterBorder = Color.Empty;
        private Color _InnerBorder = Color.Empty;
        private SuperTabLinearGradientColorTable _Background;

        private SuperTabControlBoxStateColorTable _ControlBoxDefault;
        private SuperTabControlBoxStateColorTable _ControlBoxMouseOver;
        private SuperTabControlBoxStateColorTable _ControlBoxPressed;

        private Color _InsertMarker = Color.Empty;
        private Color _SelectionMarker = Color.Empty;

        #endregion

        public SuperTabColorTable()
        {
            _Background = new SuperTabLinearGradientColorTable();
            _Background.ColorTableChanged += SctColorTableChanged;

            _ControlBoxDefault = new SuperTabControlBoxStateColorTable();
            _ControlBoxDefault.ColorTableChanged += SctColorTableChanged;

            _ControlBoxMouseOver = new SuperTabControlBoxStateColorTable();
            _ControlBoxMouseOver.ColorTableChanged += SctColorTableChanged;

            _ControlBoxPressed = new SuperTabControlBoxStateColorTable();
            _ControlBoxPressed.ColorTableChanged += SctColorTableChanged;
        }

        #region Public properties

        #region Background

        /// <summary>
        /// Gets or sets the Background color
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Background color.")]
        [NotifyParentProperty(true)]
        public SuperTabLinearGradientColorTable Background
        {
            get { return (_Background); }

            set
            {
                if (_Background.Equals(value) == false)
                {
                    if (_Background != null)
                        _Background.ColorTableChanged -= SctColorTableChanged;

                    _Background = value;

                    if (_Background != null)
                        _Background.ColorTableChanged += SctColorTableChanged;

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
        /// Gets or sets the colors for the outer border.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the colors for the outer border.")]
        [NotifyParentProperty(true)]
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
            _OuterBorder = Color.Empty;
        }

        #endregion

        #region InnerBorder

        /// <summary>
        /// Gets or sets the colors for the inner border.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the colors for the inner border.")]
        [NotifyParentProperty(true)]
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
            _InnerBorder = Color.Empty;
        }

        #endregion

        #region ControlBoxDefault

        /// <summary>
        /// Gets or sets the ControlBoxDefault colors
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the ControlBoxDefault colors.")]
        [NotifyParentProperty(true)]
        public SuperTabControlBoxStateColorTable ControlBoxDefault
        {
            get { return (_ControlBoxDefault); }

            set
            {
                if (_ControlBoxDefault.Equals(value) == false)
                {
                    if (_ControlBoxDefault != null)
                        _ControlBoxDefault.ColorTableChanged -= SctColorTableChanged;

                    _ControlBoxDefault = value;

                    if (_ControlBoxDefault != null)
                        _ControlBoxDefault.ColorTableChanged += SctColorTableChanged;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeControlBoxDefault()
        {
            return (_ControlBoxDefault.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetControlBoxDefault()
        {
            ControlBoxDefault = new SuperTabControlBoxStateColorTable();
        }

        #endregion

        #region ControlBoxDefault

        /// <summary>
        /// Gets or sets the ControlBoxMouseOver colors
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the ControlBoxMouseOver colors.")]
        [NotifyParentProperty(true)]
        public SuperTabControlBoxStateColorTable ControlBoxMouseOver
        {
            get { return (_ControlBoxMouseOver); }

            set
            {
                if (_ControlBoxMouseOver.Equals(value) == false)
                {
                    if (_ControlBoxMouseOver != null)
                        _ControlBoxMouseOver.ColorTableChanged -= SctColorTableChanged;

                    _ControlBoxMouseOver = value;

                    if (_ControlBoxMouseOver != null)
                        _ControlBoxMouseOver.ColorTableChanged += SctColorTableChanged;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeControlBoxMouseOver()
        {
            return (_ControlBoxMouseOver.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetControlBoxMouseOver()
        {
            ControlBoxMouseOver = new SuperTabControlBoxStateColorTable();
        }

        #endregion

        #region ControlBoxPressed

        /// <summary>
        /// Gets or sets the ControlBoxPressed colors
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the ControlBoxPressed colors.")]
        [NotifyParentProperty(true)]
        public SuperTabControlBoxStateColorTable ControlBoxPressed
        {
            get { return (_ControlBoxPressed); }

            set
            {
                if (_ControlBoxPressed.Equals(value) == false)
                {
                    if (_ControlBoxPressed != null)
                        _ControlBoxPressed.ColorTableChanged -= SctColorTableChanged;

                    _ControlBoxPressed = value;

                    if (_ControlBoxPressed != null)
                        _ControlBoxPressed.ColorTableChanged += SctColorTableChanged;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeControlBoxPressed()
        {
            return (_ControlBoxPressed.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetControlBoxPressed()
        {
            ControlBoxPressed = new SuperTabControlBoxStateColorTable();
        }

        #endregion

        #region InsertMarker

        /// <summary>
        /// Gets or sets the colors for the InsertMarker.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the color for the InsertMarker.")]
        [NotifyParentProperty(true)]
        public Color InsertMarker
        {
            get { return (_InsertMarker); }

            set
            {
                if (_InsertMarker != value)
                {
                    _InsertMarker = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeInsertMarker()
        {
            return (_InsertMarker != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetInsertMarker()
        {
            _InsertMarker = Color.Empty;
        }

        #endregion

        #region SelectionMarker

        /// <summary>
        /// Gets or sets the color for the SelectionMarker.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the color for the SelectionMarker.")]
        [NotifyParentProperty(true)]
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
            _SelectionMarker = Color.Empty;
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// Gets whether the ColorTable is empty.
        /// </summary>
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

                if (_ControlBoxDefault.IsEmpty == false)
                    return (false);

                if (_ControlBoxMouseOver.IsEmpty == false)
                    return (false);

                if (_ControlBoxPressed.IsEmpty == false)
                    return (false);

                if (_InsertMarker != Color.Empty)
                    return (false);

                if (_SelectionMarker != Color.Empty)
                    return (false);

                return (true);
            }
        }

        #endregion

        #endregion

        #region SctColorTableChanged

        void SctColorTableChanged(object sender, EventArgs e)
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
            SuperTabColorTable sct = new SuperTabColorTable();

            sct.Background = (SuperTabLinearGradientColorTable)Background.Clone();
            sct.OuterBorder = OuterBorder;
            sct.InnerBorder = InnerBorder;

            sct.ControlBoxDefault = (SuperTabControlBoxStateColorTable)ControlBoxDefault.Clone();
            sct.ControlBoxMouseOver = (SuperTabControlBoxStateColorTable)ControlBoxMouseOver.Clone();
            sct.ControlBoxPressed = (SuperTabControlBoxStateColorTable)ControlBoxPressed.Clone();

            sct.InsertMarker = InsertMarker;
            sct.SelectionMarker = SelectionMarker;

            return (sct);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Background = null;
            ControlBoxDefault = null;
            ControlBoxMouseOver = null;
            ControlBoxPressed = null;
        }

        #endregion
    }

    #region SuperTabColorTableConvertor

    public class SuperTabColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                SuperTabColorTable sct = value as SuperTabColorTable;

                if (sct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (sct.Background.Colors != null)
                        return (cvt.ConvertToString(sct.Background.Colors));

                    if (sct.InnerBorder.IsEmpty == false)
                        return (cvt.ConvertToString(sct.InnerBorder));

                    if (sct.OuterBorder.IsEmpty == false)
                        return (cvt.ConvertToString(sct.OuterBorder));

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion

}
