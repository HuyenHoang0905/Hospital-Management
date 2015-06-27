using System;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the color table for SuperTabItem states
    /// </summary>
    [TypeConverter(typeof(SuperTabItemColorTableConvertor))]
    public class SuperTabItemColorTable : ICloneable, IDisposable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabItemColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabItemColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private SuperTabColorStates _Default;
        private SuperTabColorStates _Left;
        private SuperTabColorStates _Bottom;
        private SuperTabColorStates _Right;

        #endregion

        public SuperTabItemColorTable()
        {
            _Default = new SuperTabColorStates();
            _Default.ColorTableChanged += SctColorTableChanged;

            _Left = new SuperTabColorStates();
            _Left.ColorTableChanged += SctColorTableChanged;

            _Bottom = new SuperTabColorStates();
            _Bottom.ColorTableChanged += SctColorTableChanged;

            _Right = new SuperTabColorStates();
            _Right.ColorTableChanged += SctColorTableChanged;
        }

        #region Public properties

        #region Default

        /// <summary>
        /// Gets or sets the Default tab color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Default tab color settings.")]
        public SuperTabColorStates Default
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
            Default = new SuperTabColorStates();
        }

        #endregion

        #region Left

        /// <summary>
        /// Gets or sets the Left Aligned tab color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Left Aligned tab color settings.")]
        public SuperTabColorStates Left
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
            Left = new SuperTabColorStates();
        }

        #endregion

        #region Bottom

        /// <summary>
        /// Gets or sets the Bottom Aligned tab color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Bottom Aligned tab color settings.")]
        public SuperTabColorStates Bottom
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
            Bottom = new SuperTabColorStates();
        }

        #endregion

        #region Right

        /// <summary>
        /// Gets or sets the Right Aligned tab color settings
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the Right Aligned tab color settings.")]
        public SuperTabColorStates Right
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
            Right = new SuperTabColorStates();
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return (_Default.IsEmpty == true  && _Left.IsEmpty == true  &&
                    _Bottom.IsEmpty == true && _Right.IsEmpty == true);
            }
        }

        #endregion

        #endregion

        #region SetNewColorTable

        private void SetNewColorTable(
            ref SuperTabColorStates sct, SuperTabColorStates newSct)
        {
            if (sct != null)
                sct.ColorTableChanged -= SctColorTableChanged;

            sct = newSct;

            if (sct != null)
                sct.ColorTableChanged += SctColorTableChanged;

            OnColorTableChanged();
        }

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
            SuperTabItemColorTable sct = new SuperTabItemColorTable();

            sct.Default = (SuperTabColorStates) Default.Clone();
            sct.Left = (SuperTabColorStates) Left.Clone();
            sct.Bottom = (SuperTabColorStates) Bottom.Clone();
            sct.Right = (SuperTabColorStates) Right.Clone();

            return (sct);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Default = null;
            Left = null;
            Bottom = null;
            Right = null;
        }

        #endregion
    }

    #region SuperTabColorStates

    [TypeConverter(typeof(SuperTabItemColorTableConvertor))]
    public class SuperTabColorStates
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabColorStates is changed
        /// </summary>
        [Description("Event raised when the SuperTabColorStates is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private data

        private SuperTabItemStateColorTable _Normal;
        private SuperTabItemStateColorTable _Selected;
        private SuperTabItemStateColorTable _MouseOver;
        private SuperTabItemStateColorTable _SelectedMouseOver;

        #endregion

        public SuperTabColorStates()
        {
            _Normal = new SuperTabItemStateColorTable();
            _Normal.ColorTableChanged += SctColorTableChanged;

            _Selected = new SuperTabItemStateColorTable();
            _Selected.ColorTableChanged += SctColorTableChanged;

            _MouseOver = new SuperTabItemStateColorTable();
            _MouseOver.ColorTableChanged += SctColorTableChanged;

            _SelectedMouseOver = new SuperTabItemStateColorTable();
            _SelectedMouseOver.ColorTableChanged += SctColorTableChanged;
        }

        #region Public properties

        #region Normal

        /// <summary>
        /// Gets or sets the tab colors when the tab is not selected, and the mouse is not over it
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the tab colors when the tab is not selected, and the mouse is not over it.")]
        public SuperTabItemStateColorTable Normal
        {
            get { return (_Normal); }
            set { SetNewColorTable(ref _Normal, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeNormal()
        {
            return (_Normal.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetNormal()
        {
            Normal = new SuperTabItemStateColorTable();
        }

        #endregion

        #region Selected

        /// <summary>
        /// Gets or sets the tab colors when the tab is selected, but the mouse is not over the it
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the tab colors when the tab is selected, but the mouse is not over the it.")]
        public SuperTabItemStateColorTable Selected
        {
            get { return (_Selected); }
            set { SetNewColorTable(ref _Selected, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelected()
        {
            return (_Selected.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelected()
        {
            Selected = new SuperTabItemStateColorTable();
        }

        #endregion

        #region MouseOver

        /// <summary>
        /// Gets or sets the tab colors when the tab is not selected, but the mouse is over it
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the tab colors when the tab is not selected, but the mouse is over it.")]
        public SuperTabItemStateColorTable MouseOver
        {
            get { return (_MouseOver); }
            set { SetNewColorTable(ref _MouseOver, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMouseOver()
        {
            return (_MouseOver.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetMouseOver()
        {
            MouseOver = new SuperTabItemStateColorTable();
        }

        #endregion

        #region SelectedMouseOver

        /// <summary>
        /// Gets or sets the tab colors when the tab is selected, and the mouse is over it
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("Indicates the tab colors when the tab is selected, and the mouse is over it.")]
        public SuperTabItemStateColorTable SelectedMouseOver
        {
            get { return (_SelectedMouseOver); }
            set { SetNewColorTable(ref _SelectedMouseOver, value); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedMouseOver()
        {
            return (_SelectedMouseOver.IsEmpty == false);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedMouseOver()
        {
            SelectedMouseOver = new SuperTabItemStateColorTable();
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (_Normal.IsEmpty == false)
                    return (false);

                if (_Selected.IsEmpty == false)
                    return (false);

                if (_MouseOver.IsEmpty == false)
                    return (false);

                if (_SelectedMouseOver.IsEmpty == false)
                    return (false);

                return (true);
            }
        }

        #endregion

        #endregion

        #region SetNewColorTable

        private void SetNewColorTable(
            ref SuperTabItemStateColorTable sct, SuperTabItemStateColorTable newSct)
        {
            if (sct != null)
                sct.ColorTableChanged -= SctColorTableChanged;

            sct = newSct;

            if (sct != null)
                sct.ColorTableChanged += SctColorTableChanged;

            OnColorTableChanged();
        }

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
            SuperTabColorStates sct = new SuperTabColorStates();

            sct.Normal = (SuperTabItemStateColorTable)Normal.Clone();
            sct.Selected = (SuperTabItemStateColorTable)Selected.Clone();
            sct.MouseOver = (SuperTabItemStateColorTable)MouseOver.Clone();
            sct.SelectedMouseOver = (SuperTabItemStateColorTable)SelectedMouseOver.Clone();

            return (sct);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Normal = null;
            MouseOver = null;
            SelectedMouseOver = null;
            Selected = null;
        }

        #endregion
    }

    #region SuperTabItemColorTableConvertor

    public class SuperTabItemColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (String.Empty);
        }
    }

    #endregion

    #endregion
}
