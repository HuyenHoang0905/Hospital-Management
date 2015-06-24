using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DevComponents.DotNetBar.ScrollBar;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    [ToolboxItem(false), DefaultEvent("Scroll"), DefaultProperty("Value")]
    public abstract class ScrollBarAdv : Control, ICommandSource
    {
        #region Private Variables
        private ScrollBarCore _ScrollBar = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the Value property is changed, either by a Scroll event or programmatically.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Occurs when the scroll box has been moved by either a mouse or keyboard action.
        /// </summary>
        public event ScrollEventHandler Scroll;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ScrollBarAdv class.
        /// </summary>
        public ScrollBarAdv()
        {
            this.TabStop = false;
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.ContainerControl, false);
            this.SetStyle(ControlStyles.Selectable, true);
            CreateScrollBarCore();
            StyleManager.Register(this);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Called by StyleManager to notify control that style on manager has changed and that control should refresh its appearance if
        /// its style is controlled by StyleManager.
        /// </summary>
        /// <param name="newStyle">New active style.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
        {
            if (_ScrollBar != null)
                _ScrollBar.InvalidateScrollBar();
        }

        protected override void Dispose(bool disposing)
        {
            if (_ScrollBar != null)
            {
                _ScrollBar.Dispose();
                _ScrollBar = null;
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Gets whether scrollbar is vertical.
        /// </summary>
        /// <returns>true if scrollbar is vertical otherwise false for horizontal scrollbar</returns>
        protected abstract bool IsVertical();

        private void CreateScrollBarCore()
        {
            _ScrollBar = new ScrollBarCore(this, false);
            _ScrollBar.Orientation = IsVertical() ? eOrientation.Vertical : eOrientation.Horizontal;
            _ScrollBar.LargeChange = _LargeChange;
            _ScrollBar.Maximum = _Maximum;
            _ScrollBar.Minimum = _Minimum;
            _ScrollBar.SmallChange = this.SmallChange;
            _ScrollBar.Value = _Value;
            _ScrollBar.ValueChanged += new EventHandler(CoreValueChanged);
            _ScrollBar.Scroll += new ScrollEventHandler(CoreScroll);
        }

        private void CoreScroll(object sender, ScrollEventArgs e)
        {
            OnScroll(e);
        }

        private void CoreValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
            ExecuteCommand();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_ScrollBar != null)
            {
                _ScrollBar.Paint(GetItemPaintArgs(e));
            }
            base.OnPaint(e);
        }

        private ItemPaintArgs GetItemPaintArgs(PaintEventArgs e)
        {
            ItemPaintArgs p = new ItemPaintArgs(null, this, e.Graphics, null);
            return p;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.MouseMove(e);
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.TabStop && !this.Focused)
            {
                this.Select();
            }
            if (_ScrollBar != null) _ScrollBar.MouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.InvalidateScrollBar();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.InvalidateScrollBar();
            base.OnLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.MouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.MouseLeave();
            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (_ScrollBar != null) _ScrollBar.DisplayRectangle = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
            base.OnResize(e);
        }

        private int _LargeChange = 10;
        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the <paramref name="Value">Value</paramref> property when the scroll box is moved a large distance.
        /// <value>A numeric value. The default value is 10.</value>
        /// <remarks>ArgumentOutOfRangeException is raised if assigned value is less than 0.  </remarks>
        /// </summary>
        [DefaultValue(10), Category("Behavior"), Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a large distance")]
        public int LargeChange
        {
            get { return _LargeChange; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                if (_LargeChange != value)
                {
                    _LargeChange = value;
                    if (_ScrollBar != null) _ScrollBar.LargeChange = _LargeChange;
                }
            }
        }

        private int _Maximum = 100;
        /// <summary>
        /// Gets or sets the upper limit of values of the scrollable range.
        /// <value>A numeric value. The default value is 100.</value>
        /// </summary>
        [DefaultValue(100), Category("Behavior"), Description("Indicates upper limit of values of the scrollable range."), RefreshProperties(RefreshProperties.Repaint)]
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                if (_Maximum != value)
                {
                    _Maximum = value;
                    if (_ScrollBar != null) _ScrollBar.Maximum = _Maximum;
                }
            }
        }

        private int _Minimum = 0;
        /// <summary>
        /// Gets or sets the lower limit of values of the scrollable range.
        /// <value>A numeric value. The default value is 0.</value>
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates lower limit of values of the scrollable range."), RefreshProperties(RefreshProperties.Repaint)]
        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                if (_Minimum != value)
                {
                    _Minimum = value;
                    if (_ScrollBar != null) _ScrollBar.Minimum = _Minimum;
                }
            }
        }

        private int _SmallChange = 1;
        /// <summary>
        /// Gets or sets the value to be added to or subtracted from the <paramref name="Value">Value</paramref> property when the scroll box is moved a small distance.
        /// <value>A numeric value. The default value is 1.</value>
        /// <remarks>ArgumentOutOfRangeException is raised if assigned value is less than 0.  </remarks>
        /// </summary>
        [DefaultValue(1), Category("Behavior"), Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a small distance.")]
        public int SmallChange
        {
            get
            {
                return Math.Min(_SmallChange, _LargeChange);
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                if (_SmallChange != value)
                {
                    _SmallChange = value;

                    if (_ScrollBar != null)
                        _ScrollBar.SmallChange = this.SmallChange;
                }
            }
        }

        private int _Value = 0;
        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box on the scroll bar control.
        /// <value>A numeric value that is within the Minimum and Maximum range. The default value is 0.</value>
        /// <remarks>ArgumentOutOfRangeException is raised if assigned value is less than the Minimum property value or assigned value is greater than the Maximum property value.</remarks>
        /// </summary>
        [DefaultValue(0), Category("Behavior"), Description("Indicates numeric value that represents the current position of the scroll box on the scroll bar control"), RefreshProperties(RefreshProperties.Repaint)]
        public int Value
        {
            get { if (_ScrollBar != null) return _ScrollBar.Value; else return 0; }
            set
            {
                if (value < Minimum)
                    throw new ArgumentOutOfRangeException("Value must be greater or equal than Minimum property value.");
                if(value>Maximum)
                    throw new ArgumentOutOfRangeException("Value must be less or equal than Maximum property value.");
                if (_ScrollBar != null) _ScrollBar.Value = value;
            }
        }

        internal void DoScroll(int newValue, ScrollEventType eventType)
        {
            int oldValue = this.Value;
            this.Value = newValue;
#if FRAMEWORK20
            OnScroll(new ScrollEventArgs(eventType, oldValue, newValue, _ScrollBar.Orientation == eOrientation.Vertical ? ScrollOrientation.VerticalScroll : ScrollOrientation.HorizontalScroll));
#else
            OnScroll(new ScrollEventArgs(eventType, newValue));
#endif
        }

        /// <summary>
        /// Raises the Scroll event.
        /// </summary>
        /// <param name="e">Provides Event arguments.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler h = this.Scroll;
            if (h != null)
                h(this, e);
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="e">Provides Event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler h = this.ValueChanged;
            if (h != null)
                h(this, e);
        }

        private eScrollBarAppearance _Appearance = eScrollBarAppearance.Default;
        /// <summary>
        /// Gets or sets the scroll bar appearance style.
        /// </summary>
        [DefaultValue(eScrollBarAppearance.Default), Category("Appearance"), Description("Indicates scroll bar appearance style."), RefreshProperties(RefreshProperties.Repaint)]
        public eScrollBarAppearance Appearance
        {
            get { return _Appearance; }
            set
            {
                if (_Appearance != value)
                {
                    _Appearance = value;
                    OnAppearanceChanged();
                }
            }
        }

        private void OnAppearanceChanged()
        {
            if (_ScrollBar != null)
            {
                if (_Appearance == eScrollBarAppearance.Default)
                    _ScrollBar.IsAppScrollBarStyle = false;
                else
                    _ScrollBar.IsAppScrollBarStyle = true;
            }
            this.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_ScrollBar!=null)
                return _ScrollBar.ProcessCmdKey(ref msg, keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Property Hiding
#if FRAMEWORK20
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler AutoSizeChanged
        {
            add
            {
                base.AutoSizeChanged += value;
            }
            remove
            {
                base.AutoSizeChanged -= value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event MouseEventHandler MouseClick
        {
            add
            {
                base.MouseClick += value;
            }
            remove
            {
                base.MouseClick -= value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event MouseEventHandler MouseDoubleClick
        {
            add
            {
                base.MouseDoubleClick += value;
            }
            remove
            {
                base.MouseDoubleClick -= value;
            }
        }
#endif
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler BackColorChanged
        {
            add
            {
                base.BackColorChanged += value;
            }
            remove
            {
                base.BackColorChanged -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler BackgroundImageChanged
        {
            add
            {
                base.BackgroundImageChanged += value;
            }
            remove
            {
                base.BackgroundImageChanged -= value;
            }
        }
#if FRAMEWORK20
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                base.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                base.BackgroundImageLayoutChanged -= value;
            }
        }
#endif
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new ImeMode ImeMode
        {
            get
            {
                return base.ImeMode;
            }
            set
            {
                base.ImeMode = value;
            }
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = value;
            }
        }

        [Bindable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler Click
        {
            add
            {
                base.Click += value;
            }
            remove
            {
                base.Click -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler DoubleClick
        {
            add
            {
                base.DoubleClick += value;
            }
            remove
            {
                base.DoubleClick -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler FontChanged
        {
            add
            {
                base.FontChanged += value;
            }
            remove
            {
                base.FontChanged -= value;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler ForeColorChanged
        {
            add
            {
                base.ForeColorChanged += value;
            }
            remove
            {
                base.ForeColorChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler ImeModeChanged
        {
            add
            {
                base.ImeModeChanged += value;
            }
            remove
            {
                base.ImeModeChanged -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event MouseEventHandler MouseDown
        {
            add
            {
                base.MouseDown += value;
            }
            remove
            {
                base.MouseDown -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event MouseEventHandler MouseMove
        {
            add
            {
                base.MouseMove += value;
            }
            remove
            {
                base.MouseMove -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event MouseEventHandler MouseUp
        {
            add
            {
                base.MouseUp += value;
            }
            remove
            {
                base.MouseUp -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event PaintEventHandler Paint
        {
            add
            {
                base.Paint += value;
            }
            remove
            {
                base.Paint -= value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new event EventHandler TextChanged
        {
            add
            {
                base.TextChanged += value;
            }
            remove
            {
                base.TextChanged -= value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return RightToLeft.No;
            }
            set
            {
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public event EventHandler RightToLeftChanged
        {
            add
            {
                base.RightToLeftChanged += value;
            }
            remove
            {
                base.RightToLeftChanged -= value;
            }
        }
        #endregion

        #region ICommandSource Members
        protected virtual void ExecuteCommand()
        {
            if (_Command == null) return;
            CommandManager.ExecuteCommand(this);
        }

        /// <summary>
        /// Gets or sets the command assigned to the item. Default value is null.
        /// <remarks>Note that if this property is set to null Enabled property will be set to false automatically to disable the item.</remarks>
        /// </summary>
        [DefaultValue(null), Category("Commands"), Description("Indicates the command assigned to the item.")]
        public Command Command
        {
            get { return (Command)((ICommandSource)this).Command; }
            set
            {
                ((ICommandSource)this).Command = value;
            }
        }

        private ICommand _Command = null;
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ICommand ICommandSource.Command
        {
            get
            {
                return _Command;
            }
            set
            {
                bool changed = false;
                if (_Command != value)
                    changed = true;

                if (_Command != null)
                    CommandManager.UnRegisterCommandSource(this, _Command);
                _Command = value;
                if (value != null)
                    CommandManager.RegisterCommand(this, value);
                if (changed)
                    OnCommandChanged();
            }
        }

        /// <summary>
        /// Called when Command property value changes.
        /// </summary>
        protected virtual void OnCommandChanged()
        {
        }

        private object _CommandParameter = null;
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        [Browsable(true), DefaultValue(null), Category("Commands"), Description("Indicates user defined data value that can be passed to the command when it is executed."), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter)), System.ComponentModel.Localizable(true)]
        public object CommandParameter
        {
            get
            {
                return _CommandParameter;
            }
            set
            {
                _CommandParameter = value;
            }
        }

        #endregion

    }
}
