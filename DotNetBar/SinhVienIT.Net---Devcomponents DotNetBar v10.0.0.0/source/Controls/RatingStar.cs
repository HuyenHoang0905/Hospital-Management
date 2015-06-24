using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.Editors;
using System.Drawing;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents the Rating control.
    /// </summary>
    [ToolboxBitmap(typeof(RatingStar), "Controls.RatingStar.ico"), ToolboxItem(true), DefaultEvent("RatingChanged"), System.Runtime.InteropServices.ComVisible(true)]
    public class RatingStar : BaseItemControl, ICommandSource
    {
        #region Private Variables
        private RatingItem _RatingItem = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Rating property has changed.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event EventHandler RatingChanged;
        /// <summary>
        /// Occurs when RatingValue property has changed.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event EventHandler RatingValueChanged;
        /// <summary>
        /// Occurs when Rating property is about to be changed and provides opportunity to cancel the change.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event RatingChangeEventHandler RatingChanging;
        /// <summary>
        /// Occurs when AverageRating property has changed.
        /// </summary>
        [Description("Occurs when AverageRating property has changed.")]
        public event EventHandler AverageRatingChanged;
        /// <summary>
        /// Occurs when AverageRatingValue property has changed.
        /// </summary>
        [Description("Occurs when AverageRatingValue property has changed.")]
        public event EventHandler AverageRatingValueChanged;
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        /// <summary>
        /// Occurs when RatingValue property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseIntegerValueEventHandler ParseRatingValue;
        /// <summary>
        /// Occurs when AverageRatingValue property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseDoubleValueEventHandler ParseAverageRatingValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Rating class.
        /// </summary>
        public RatingStar()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            _RatingItem = new RatingItem();
            _RatingItem.Style = eDotNetBarStyle.Office2007;
            _RatingItem.RatingChanging += new RatingChangeEventHandler(RatingItemRatingChanging);
            _RatingItem.RatingChanged += new EventHandler(RatingItemRatingChanged);
            _RatingItem.AverageRatingChanged += new EventHandler(RatingItemAverageRatingChanged);
            _RatingItem.ParseAverageRatingValue += new DevComponents.Editors.ParseDoubleValueEventHandler(RatingItemParseAverageRatingValue);
            _RatingItem.ParseRatingValue += new DevComponents.Editors.ParseIntegerValueEventHandler(RatingItemParseRatingValue);
            this.HostItem = _RatingItem;
        }

        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets the rating value represented by the control. Default value is 0 which indicates
        /// that there is no rating set. Maximum value is 5.
        /// </summary>
        [DefaultValue(0), Category("Data"), Description("Indicates rating value represented by the control.")]
        public int Rating
        {
            get
            {
                return _RatingItem.Rating;
            }
            set
            {
            	_RatingItem.Rating = value;
            }
        }
        /// <summary>
        /// Gets or sets the average rating shown by control. Control will display average rating (if set) when no explicit
        /// Rating value is set through Rating property. Minimum value is 0 and Maximum value is 5.
        /// </summary>
        [DefaultValue(0d), Category("Data"), Description("Indicates average rating shown by control.")]
        public double AverageRating
        {
            get
            {
                return _RatingItem.AverageRating;
            }
            set
            {
            	_RatingItem.AverageRating = value;
            }
        }
        /// <summary>
        /// Gets or sets the AverageRating property. This property is provided for Data-Binding with NULL value support.
        /// </summary>
        [Bindable(true), Browsable(false), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter))]
        public object AverageRatingValue
        {
            get
            {
                return _RatingItem.AverageRatingValue;
            }
            set
            {
                _RatingItem.AverageRatingValue = value;
            }
        }
        /// <summary>
        /// Gets the reference to custom rating images.
        /// </summary>
        [Browsable(true), Category("Images"), Description("Gets the reference to custom rating images."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RatingImages CustomImages
        {
            get { return _RatingItem.CustomImages; }
        }
        /// <summary>
        /// Gets or sets whether text assigned to the check box is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether text assigned to the check box is visible.")]
        public bool TextVisible
        {
            get { return _RatingItem.TextVisible; }
            set
            {
                _RatingItem.TextVisible = value;
            }
        }
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return _RatingItem.EnableMarkup; }
            set
            {
                _RatingItem.EnableMarkup = value;
            }
        }
        /// <summary>
        /// Gets or sets whether rating can be edited. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether rating can be edited.")]
        public bool IsEditable
        {
            get { return _RatingItem.IsEditable; }
            set
            {
                _RatingItem.IsEditable = value;
            }
        }
        /// <summary>
        /// Gets or sets the orientation of rating control.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance"), Description("Gets or sets the orientation of rating control.")]
        public eOrientation RatingOrientation
        {
            get { return _RatingItem.RatingOrientation; }
            set
            {
                _RatingItem.RatingOrientation = value;
            }
        }
        /// <summary>
        /// Gets or sets the Rating property value. This property is provided for Data-Binding with NULL value support.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), RefreshProperties(RefreshProperties.All), Bindable(true)]
        public object RatingValue
        {
            get { return _RatingItem.RatingValue; }
            set
            {
                _RatingItem.RatingValue = value;
            }
        }
        /// <summary>
        /// Gets or sets the text color. Default value is Color.Empty which indicates that default color is used.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates text color.")]
        public Color TextColor
        {
            get { return _RatingItem.TextColor; }
            set
            {
                _RatingItem.TextColor = value;
            }
        }
        /// <summary>
        /// Gets or sets the spacing between optional text and the rating.
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Description("Gets or sets the spacing between optional text and the rating.")]
        public int TextSpacing
        {
            get { return _RatingItem.TextSpacing; }
            set
            {
                _RatingItem.TextSpacing = value;
            }
        }

        private void RatingItemParseRatingValue(object sender, DevComponents.Editors.ParseIntegerValueEventArgs e)
        {
            OnParseRatingValue(e);
        }
        /// <summary>
        /// Raises the ParseRating event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
        {
            if (ParseRatingValue != null)
                ParseRatingValue(this, e);
        }

        private void RatingItemParseAverageRatingValue(object sender, DevComponents.Editors.ParseDoubleValueEventArgs e)
        {
            OnParseAverageRatingValue(e);
        }
        /// <summary>
        /// Raises the ParseAverageRatingValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
        {
            if (ParseAverageRatingValue != null)
                ParseAverageRatingValue(this, e);
        }

        private void RatingItemAverageRatingChanged(object sender, EventArgs e)
        {
            OnAverageRatingChanged(e);
        }
        /// <summary>
        /// Raises the AverageRatingChanged event.
        /// </summary>
        /// <param name="eventArgs">Event data.</param>
        protected virtual void OnAverageRatingChanged(EventArgs eventArgs)
        {
            if (AverageRatingChanged != null) AverageRatingChanged(this, eventArgs);
            if (AverageRatingValueChanged != null) AverageRatingValueChanged(this, eventArgs);
        }

        private void RatingItemRatingChanged(object sender, EventArgs e)
        {
            OnRatingChanged(e);
        }
        /// <summary>
        /// Raises the RatingChanged event.
        /// </summary>
        /// <param name="eventArgs">Event data.</param>
        protected virtual void OnRatingChanged(EventArgs eventArgs)
        {
            EventHandler handler = RatingChanged;
            if (handler != null) handler(this, eventArgs);

            handler = RatingValueChanged;
            if (handler != null) handler(this, eventArgs);
        }

        private void RatingItemRatingChanging(object sender, RatingChangeEventArgs e)
        {
            OnRatingChanging(e);
        }
        /// <summary>
        /// Raises RatingChanging event.
        /// </summary>
        /// <param name="e">Event data</param>
        protected virtual void OnRatingChanging(RatingChangeEventArgs e)
        {
            if (RatingChanging != null)
                RatingChanging(this, e);
        }

#if FRAMEWORK20
        [Localizable(true), Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!BarFunctions.IsHandleValid(this))
                return base.GetPreferredSize(proposedSize);
            
            _RatingItem.RecalcSize();
            Size s = _RatingItem.Size;
            s.Width += 2;
            s.Height += 2;
            if (!this.TextVisible) s.Width += 2;
            s.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(this.GetBackgroundStyle());
            s.Height += ElementStyleLayout.VerticalStyleWhiteSpace(this.GetBackgroundStyle());
            _RatingItem.Bounds = GetItemBounds();
            return s;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized to display its entire contents. You can set MaximumSize.Width property to set the maximum width used by the control.
        /// </summary>
        [Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    AdjustSize();
                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                width = preferredSize.Width;
                height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void AdjustSize()
        {
            if (this.AutoSize)
            {
                this.Size = base.PreferredSize;
            }
        }

        protected override void OnVisualPropertyChanged()
        {
            base.OnVisualPropertyChanged();
            this.AdjustSize();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.AdjustSize();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.AutoSize)
                this.AdjustSize();
        }
#endif
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
