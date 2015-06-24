using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Events;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(SwitchButton), "Controls.SwitchButton.ico"), ToolboxItem(true), DefaultEvent("CheckedChanged"), System.Runtime.InteropServices.ComVisible(false), Designer("DevComponents.DotNetBar.Design.SwitchButtonDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class SwitchButton : BaseItemControl, ICommandSource
    {
        #region Events
        /// <summary>
        /// Occurs before Value property has changed and it allows you to cancel the change.
        /// </summary>
        public event EventHandler ValueChanging;
        /// <summary>
        /// Raises ValueChanging event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanging(EventArgs e)
        {
            EventHandler handler = ValueChanging;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Occurs after Value property has changed.
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Constructor, Dispose
        private SwitchButtonItem _SwitchButton = null;
        public SwitchButton()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            _SwitchButton = new SwitchButtonItem();
            _SwitchButton.TextVisible = false;
            _SwitchButton.ValueChanging += new EventHandler(SwitchButtonItemValueChanging);
            _SwitchButton.ValueChanged += new EventHandler(SwitchButtonItemValueChanged);
            
            this.HostItem = _SwitchButton;
        }
        #endregion


        #region Internal Implementation
        protected override void OnHandleCreated(EventArgs e)
        {
            this.RecalcLayout();
            base.OnHandleCreated(e);
        }
        /// <summary>
        /// Forces the button to perform internal layout.
        /// </summary>
        public override void RecalcLayout()
        {
            if (_SwitchButton == null) return;
            _SwitchButton.ButtonWidth = Math.Max(4, this.Width - this.Padding.Horizontal);
            _SwitchButton.ButtonHeight = Math.Max(4, this.Height - this.Padding.Vertical);
            base.RecalcLayout();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && this.Enabled && !IsReadOnly)
            {
                _SwitchButton.SetValueAndAnimate(!_SwitchButton.Value, eEventSource.Keyboard);
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Gets or sets the switch value.
        /// </summary>
        [DefaultValue(false), Category("Appearance"), Description("Indicates switch value."), Bindable(true)]
        public bool Value
        {
            get { return _SwitchButton.Value; }
            set
            {
                _SwitchButton.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the item border color.
        /// </summary>
        [Category("Appearance"), Description("Indicates item border color.")]
        public Color BorderColor
        {
            get { return _SwitchButton.BorderColor; }
            set { _SwitchButton.BorderColor = value;  }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColor()
        {
            return _SwitchButton.ShouldSerializeBorderColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBorderColor()
        {
            this.BorderColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets whether state transition animation is enabled.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether state transition animation is enabled.")]
        public bool AnimationEnabled
        {
            get { return _SwitchButton.AnimationEnabled; }
            set
            {
                _SwitchButton.AnimationEnabled = value;
            }
        }

        void SwitchButtonItemValueChanged(object sender, EventArgs e)
        {
            ExecuteCommand();
            OnValueChanged(e);
        }

        void SwitchButtonItemValueChanging(object sender, EventArgs e)
        {
            OnValueChanging(e);
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return new System.Drawing.Size(66, 22);
            }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            _SwitchButton.Margin.Bottom = this.Padding.Bottom;
            _SwitchButton.Margin.Top = this.Padding.Top;
            _SwitchButton.Margin.Left = this.Padding.Left;
            _SwitchButton.Margin.Right = this.Padding.Right;

            base.OnPaddingChanged(e);
        }

        /// <summary>
        /// Gets or sets the color of the OFF state background.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of OFF state background.")]
        public Color OffBackColor
        {
            get { return _SwitchButton.OffBackColor; }
            set { _SwitchButton.OffBackColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOffBackColor()
        {
            return _SwitchButton.ShouldSerializeOffBackColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetOffBackColor()
        {
            this.OffBackColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color of the ON state background.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of ON state background.")]
        public Color OnBackColor
        {
            get { return _SwitchButton.OnBackColor; }
            set { _SwitchButton.OnBackColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOnBackColor()
        {
            return _SwitchButton.ShouldSerializeOnBackColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetOnBackColor()
        {
            this.OnBackColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color of the ON state text.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of ON state text.")]
        public Color OnTextColor
        {
            get { return _SwitchButton.OnTextColor; }
            set { _SwitchButton.OnTextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOnTextColor()
        {
            return _SwitchButton.ShouldSerializeOnTextColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetOnTextColor()
        {
            this.OnTextColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the color of the OFF state text.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of OFF state text.")]
        public Color OffTextColor
        {
            get { return _SwitchButton.OffTextColor; }
            set { _SwitchButton.OffTextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOffTextColor()
        {
            return _SwitchButton.ShouldSerializeOffTextColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetOffTextColor()
        {
            this.OffTextColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the border color of the button switch.
        /// </summary>
        [Category("Appearance"), Description("Indicates border color of the button switch.")]
        public Color SwitchBorderColor
        {
            get { return _SwitchButton.SwitchBorderColor; }
            set { _SwitchButton.SwitchBorderColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSwitchBorderColor()
        {
            return _SwitchButton.ShouldSerializeSwitchBorderColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSwitchBorderColor()
        {
            this.SwitchBorderColor = Color.Empty;
        }
        /// <summary>
        /// Gets or sets the background color of the switch button.
        /// </summary>
        [Category("Appearance"), Description("Indicates background color of the switch button.")]
        public Color SwitchBackColor
        {
            get { return _SwitchButton.SwitchBackColor; }
            set { _SwitchButton.SwitchBackColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSwitchBackColor()
        {
            return _SwitchButton.ShouldSerializeSwitchBackColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSwitchBackColor()
        {
            this.SwitchBackColor = Color.Empty;
        }


        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [Category("Appearance"), Description("Indicates text color."), Browsable(false)]
        public Color TextColor
        {
            get { return _SwitchButton.TextColor; }
            set { _SwitchButton.TextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return _SwitchButton.ShouldSerializeTextColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        /// <summary>
        /// Gets or sets the font that is used to draw ON/OFF text on the switch button.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indicates font that is used to draw ON/OFF text on the switch button.")]
        public Font SwitchFont
        {
            get { return _SwitchButton.SwitchFont; }
            set { _SwitchButton.SwitchFont = value; }
        }


        /// <summary>
        /// Gets or sets the text that is displayed on switch when Value property is set to true.
        /// </summary>
        [DefaultValue("ON"), Localizable(true), Category("Appearance"), Description("Indicates text that is displayed on switch when Value property is set to true.")]
        public string OnText
        {
            get { return _SwitchButton.OnText; }
            set
            {
                _SwitchButton.OnText = value;
            }
        }

        /// <summary>
        /// Gets or sets the text that is displayed on switch when Value property is set to false.
        /// </summary>
        [DefaultValue("OFF"), Localizable(true), Category("Appearance"), Description("Indicates text that is displayed on switch when Value property is set to true.")]
        public string OffText
        {
            get { return _SwitchButton.OffText; }
            set
            {
                _SwitchButton.OffText = value;
            }
        }

        [Browsable(false)]
        public override eDotNetBarStyle Style
        {
            get
            {
                return base.Style;
            }
            set
            {
                base.Style = value;
            }
        }
        [Browsable(false)]
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
        [Browsable(false)]
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

        /// <summary>
        /// Gets or sets the width in pixels of the switch part of the button. Minimum value is 6.
        /// </summary>
        [DefaultValue(28), Category("Appearance"), Description("Indicates width in pixels of the switch part of the button.")]
        public int SwitchWidth
        {
            get { return _SwitchButton.SwitchWidth; }
            set
            {
                _SwitchButton.SwitchWidth = value;
            }
        }

        /// <summary>
        /// Gets the switch bounds.
        /// </summary>
        [Browsable(false)]
        public Rectangle SwitchBounds
        {
            get { return _SwitchButton.SwitchBounds; }
        }

        /// <summary>
        /// Gets the On part of the switch button bounds excluding the SwitchBounds.
        /// </summary>
        [Browsable(false)]
        public Rectangle OnPartBounds
        {
            get { return _SwitchButton.OnPartBounds; }
        }

        /// <summary>
        /// Gets the Off part of the switch button bounds excluding the SwitchBounds.
        /// </summary>
        [Browsable(false)]
        public Rectangle OffPartBounds
        {
            get { return _SwitchButton.OffPartBounds; }
        }

        /// <summary>
        /// Sets the value of the control with state transition animation (if enabled) and specifies the source of the action.
        /// </summary>
        /// <param name="newValue">New value for Value property.</param>
        /// <param name="source">Source of the action.</param>
        public void SetValueAndAnimate(bool value, eEventSource source)
        {
            _SwitchButton.SetValueAndAnimate(value, source);
        }

        /// <summary>
        /// Sets the value of the control and specifies the source of the action.
        /// </summary>
        /// <param name="newValue">New value for Value property.</param>
        /// <param name="source">Source of the action.</param>
        public void SetValue(bool newValue, eEventSource source)
        {
            _SwitchButton.SetValue(newValue, source);
        }

        /// <summary>
        /// Gets or sets whether button is in read-only state meaning that it appears as enabled but user cannot change its state.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("Indicates whether button is in read-only state meaning that it appears as enabled but user cannot change its state.")]
        public bool IsReadOnly
        {
            get { return _SwitchButton.IsReadOnly; }
            set { _SwitchButton.IsReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets whether lock marker is visible on face of the control when IsReadOnly is set to true.
        /// Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether lock marker is visible on face of the control when IsReadOnly is set to true.")]
        public bool ShowReadOnlyMarker
        {
            get { return _SwitchButton.ShowReadOnlyMarker; }
            set { _SwitchButton.ShowReadOnlyMarker = value; }
        }

        /// <summary>
        /// Gets or sets the color of the read-only marker.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of read-only marker.")]
        public Color ReadOnlyMarkerColor
        {
            get { return _SwitchButton.ReadOnlyMarkerColor; }
            set { _SwitchButton.ReadOnlyMarkerColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReadOnlyMarkerColor()
        {
            return _SwitchButton.ShouldSerializeReadOnlyMarkerColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetReadOnlyMarkerColor()
        {
            _SwitchButton.ResetReadOnlyMarkerColor();
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
