using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the SuperTabPanel
    /// </summary>
    [TypeConverter(typeof(SuperTabPanelColorTableConvertor))]
    public class SuperTabPanelColorTable : ICloneable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabPanelColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabPanelColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private SuperTabPanelItemColorTable _Default;
        private SuperTabPanelItemColorTable _Left;
        private SuperTabPanelItemColorTable _Bottom;
        private SuperTabPanelItemColorTable _Right;

        #endregion

        public SuperTabPanelColorTable()
        {
            _Default = new SuperTabPanelItemColorTable();
            _Default.ColorTableChanged += PctColorTableChanged;

            _Left = new SuperTabPanelItemColorTable();
            _Left.ColorTableChanged += PctColorTableChanged;

            _Bottom = new SuperTabPanelItemColorTable();
            _Bottom.ColorTableChanged += PctColorTableChanged;

            _Right = new SuperTabPanelItemColorTable();
            _Right.ColorTableChanged += PctColorTableChanged;
        }

        #region Public properties

        #region Default

        /// <summary>
        /// Gets or sets the Default tab panel color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Default tab panel color settings.")]
        public SuperTabPanelItemColorTable Default
        {
            get { return (_Default); }
            set { SetNewColorTable(ref _Default, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDefault()
        {
            return (_Default.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDefault()
        {
            Default = new SuperTabPanelItemColorTable();
        }

        #endregion

        #region Left

        /// <summary>
        /// Gets or sets the Left Aligned tab panel color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Left Aligned tab panel color settings.")]
        public SuperTabPanelItemColorTable Left
        {
            get { return (_Left); }
            set { SetNewColorTable(ref _Left, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLeft()
        {
            return (_Left.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLeft()
        {
            Left = new SuperTabPanelItemColorTable();
        }

        #endregion

        #region Bottom

        /// <summary>
        /// Gets or sets the Bottom Aligned tab panel color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Bottom Aligned tab panel color settings.")]
        public SuperTabPanelItemColorTable Bottom
        {
            get { return (_Bottom); }
            set { SetNewColorTable(ref _Bottom, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBottom()
        {
            return (_Bottom.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBottom()
        {
            Bottom = new SuperTabPanelItemColorTable();
        }

        #endregion

        #region Right

        /// <summary>
        /// Gets or sets the Right Aligned tab panel color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Right Aligned tab panel color settings.")]
        public SuperTabPanelItemColorTable Right
        {
            get { return (_Right); }
            set { SetNewColorTable(ref _Right, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeRight()
        {
            return (_Right.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetRight()
        {
            Right = new SuperTabPanelItemColorTable();
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return (_Default.IsEmpty == true && _Left.IsEmpty == true &&
                    _Bottom.IsEmpty == true && _Right.IsEmpty == true);
            }
        }

        #endregion

        #endregion

        #region SetNewColorTable

        private void SetNewColorTable(
            ref SuperTabPanelItemColorTable sct, SuperTabPanelItemColorTable newSct)
        {
            if (sct != null)
                sct.ColorTableChanged -= PctColorTableChanged;

            sct = newSct;

            if (sct != null)
                sct.ColorTableChanged += PctColorTableChanged;

            OnColorTableChanged();
        }

        #endregion

        #region PctColorTableChanged

        void PctColorTableChanged(object sender, EventArgs e)
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
            SuperTabPanelColorTable pct = new SuperTabPanelColorTable();

            pct.Default = (SuperTabPanelItemColorTable)Default.Clone();
            pct.Left = (SuperTabPanelItemColorTable)Left.Clone();
            pct.Bottom = (SuperTabPanelItemColorTable)Bottom.Clone();
            pct.Right = (SuperTabPanelItemColorTable)Right.Clone();

            return (pct);
        }

        #endregion
    }

    #region SuperTabPaneColorTableConvertor

    public class SuperTabPanelColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (String.Empty);
        }
    }

    #endregion

    #region SuperTabPanelItemColorTable

    [TypeConverter(typeof(SuperTabPanelItemColorTableConvertor))]
    public class SuperTabPanelItemColorTable : ICloneable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabPanelItemColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabPanelItemColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _OuterBorder = Color.Empty;
        private Color _InnerBorder = Color.Empty;
        private SuperTabLinearGradientColorTable _Background;

        #endregion

        public SuperTabPanelItemColorTable()
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
        /// Gets or sets the colors for the outer border.
        /// </summary>
        [Browsable(true)]
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
        [Browsable(true)]
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
            SuperTabPanelItemColorTable sct = new SuperTabPanelItemColorTable();

            sct.Background = (SuperTabLinearGradientColorTable)Background.Clone();

            sct.OuterBorder = OuterBorder;
            sct.InnerBorder = InnerBorder;

            return (sct);
        }

        #endregion
    }

    #region SuperTabPanelItemColorTableConvertor

    public class SuperTabPanelItemColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                SuperTabPanelItemColorTable sct = value as SuperTabPanelItemColorTable;

                if (sct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (sct.Background.Colors != null)
                        return (cvt.ConvertToString(sct.Background.Colors[0]));

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

    #endregion
}
