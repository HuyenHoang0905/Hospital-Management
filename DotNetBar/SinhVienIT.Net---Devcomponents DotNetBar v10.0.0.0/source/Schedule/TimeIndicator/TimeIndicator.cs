#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule
{
    [TypeConverter(typeof(TimeIndicatorConvertor))]
    public class TimeIndicator
    {
        #region Events

        /// <summary>
        /// Occurs when the collection has changed
        /// </summary>
        [Description("Occurs when the TimeIndicator has changed.")]
        public event EventHandler<EventArgs> TimeIndicatorChanged;

        /// <summary>
        /// Occurs when a TimeIndicator time has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator time has changed.")]
        public event EventHandler<TimeIndicatorTimeChangedEventArgs> TimeIndicatorTimeChanged;

        /// <summary>
        /// Occurs when a TimeIndicator Color has changed
        /// </summary>
        [Description("Occurs when a TimeIndicator Color has changed.")]
        public event EventHandler<TimeIndicatorColorChangedEventArgs> TimeIndicatorColorChanged;

        #endregion

        #region Private variables

        private DateTime _IndicatorTime;
        private TimeSpan _IndicatorTimeOffset;
        private eTimeIndicatorArea _IndicatorArea;
        private eTimeIndicatorSource _IndicatorSource = eTimeIndicatorSource.SystemTime;
        private eTimeIndicatorVisibility _Visibility = eTimeIndicatorVisibility.Hidden;

        private Color _BorderColor;
        private ColorDef _IndicatorColor = new ColorDef();

        private int _Thickness = 4;

        private bool _Enabled = true;
        private bool _IsProtected;
        private bool _IsDesignMode;

        private int _UpdateCount;
        private object _Tag;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeIndicator()
        {
            _IndicatorTime = DateTime.Now;

            _IndicatorColor.ColorDefChanged += IndicatorColor_ColorDefChanged;
        }

        #region Internal properties

        #region IndicatorDisplayTime

        /// <summary>
        /// Gets the Indicator display time.
        /// 
        /// The DisplayTime is the addition of the IndicatorTime
        /// and IndicatorTimeOffset.
        /// </summary>
        internal DateTime IndicatorDisplayTime
        {
            get
            {
                if (_IsDesignMode == true)
                    return (DateTime.Now.Date.AddHours(3));

                return (_IndicatorTime.Add(_IndicatorTimeOffset));
            }
        }

        #endregion

        #region DesignMode

        /// <summary>
        /// Gets or sets whether we are in design mode
        /// </summary>
        internal bool IsDesignMode
        {
            get { return (_IsDesignMode); }
            set { _IsDesignMode = value; }
        }

        #endregion

        #region IsProtected

        /// <summary>
        /// Gets or sets whether the timer indicator
        /// is protected (can't be deleted)
        /// </summary>
        internal bool IsProtected
        {
            get { return (_IsProtected); }
            set { _IsProtected = value; }
        }

        #endregion

        #endregion

        #region Public properties

        #region BorderColor

        /// <summary>
        /// Gets or sets the leading edge border color
        /// </summary>
        [Browsable(true), DefaultValue(typeof(Color), "Empty")]
        [Description("Indicates the leading edge border color.")]
        public Color BorderColor
        {
            get { return (_BorderColor); }

            set
            {
                if (_BorderColor.Equals(value) == false)
                {
                    _BorderColor = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #region Enabled

        /// <summary>
        /// Gets or sets whether automatic time updates are enabled.
        /// This property, whose default is true, is only utilized when
        /// the IndicatorSource is set to eTimeIndicatorSource.SystemTime
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Enabled
        {
            get { return (_Enabled); }

            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;

                    if (_Enabled == true &&
                        _IndicatorSource == eTimeIndicatorSource.SystemTime)
                    {
                        IndicatorTime = DateTime.Now;
                    }
                }
            }
        }

        #endregion

        #region IndicatorArea

        /// <summary>
        /// Gets or sets the Indicator Display Area.
        /// 
        /// This property determines where the Indicator is
        /// drawn: in the Time Header, View Content, or both.
        /// </summary>
        [Browsable(true), DefaultValue(eTimeIndicatorArea.Header)]
        [Description("Indicates the Indicator display area.")]
        public eTimeIndicatorArea IndicatorArea
        {
            get { return (_IndicatorArea); }

            set
            {
                if (_IndicatorArea != value)
                {
                    _IndicatorArea = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #region IndicatorColor

        /// <summary>
        /// Gets or sets the Indicator color
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the Indicator color.")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        public ColorDef IndicatorColor
        {
            get { return (_IndicatorColor); }

            set
            {
                if (_IndicatorColor != null)
                    _IndicatorColor.ColorDefChanged -= IndicatorColor_ColorDefChanged;

                _IndicatorColor = value;

                if (_IndicatorColor != null)
                    _IndicatorColor.ColorDefChanged += IndicatorColor_ColorDefChanged;

                OnTimeIndicatorColorChanged();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIndicatorColor()
        {
            IndicatorColor = new ColorDef();
        }

        #endregion

        #region IndicatorTime

        /// <summary>
        /// Gets or sets the Indicator time
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime IndicatorTime
        {
            get { return (_IndicatorTime); }

            set
            {
                if (_IndicatorTime.Equals(value) == false)
                {
                    DateTime oldValue = IndicatorDisplayTime;
                    _IndicatorTime = value;

                    OnTimeIndicatorTimeChanged(oldValue, _IndicatorTime.Add(_IndicatorTimeOffset));
                }
            }
        }

        #endregion

        #region IndicatorTimeOffset

        /// <summary>
        /// Gets or sets the Indicator time offset.
        /// 
        /// This value is added to the current IndicatorTime
        /// before displaying the indicator.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan IndicatorTimeOffset
        {
            get { return (_IndicatorTimeOffset); }

            set
            {
                if (_IndicatorTimeOffset.Equals(value) == false)
                {
                    _IndicatorTimeOffset = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #region Visibility

        /// <summary>
        /// Gets or sets the Indicator visibility
        /// </summary>
        [Browsable(true)]
        [DefaultValue(eTimeIndicatorVisibility.Hidden)]
        [Description("Indicates the Indicator visibility.")]
        public eTimeIndicatorVisibility Visibility
        {
            get { return (_Visibility); }

            set
            {
                if (_Visibility != value)
                {
                    _Visibility = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #region Tag

        /// <summary>
        /// Gets or sets the User defined data associated with the object
        /// </summary>
        [Browsable(false)]
        public object Tag
        {
            get { return (_Tag); }
            set { _Tag = value; }
        }

        #endregion

        #region Thickness

        /// <summary>
        /// Gets or sets the thickness of the Indicator
        /// </summary>
        [Browsable(true), DefaultValue(4)]
        [Description("Indicates the thickness of the Indicator.")]
        public int Thickness
        {
            get { return (_Thickness); }

            set
            {
                if (_Thickness != value)
                {
                    _Thickness = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #region IndicatorSource

        /// <summary>
        /// Gets or sets the IndicatorTime source
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public eTimeIndicatorSource IndicatorSource
        {
            get { return (_IndicatorSource); }

            set
            {
                if (_IndicatorSource != value)
                {
                    _IndicatorSource = value;

                    OnTimeIndicatorChanged();
                }
            }
        }

        #endregion

        #endregion

        #region IsVisible

        /// <summary>
        /// Gets whether the indicator is visible
        /// </summary>
        /// <returns></returns>
        internal bool IsVisible()
        {
            return (_Visibility != eTimeIndicatorVisibility.Hidden);
        }

        /// <summary>
        /// Gets whether the indicator is visible
        /// in the given view 
        /// </summary>
        /// <param name="calendarView"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        internal bool IsVisible(CalendarView calendarView, BaseView view)
        {
            if (_Visibility == eTimeIndicatorVisibility.Hidden)
                return (false);

            if (_Visibility == eTimeIndicatorVisibility.AllResources)
                return (true);

            return ((calendarView == null || view.DisplayedOwnerKeyIndex == -1 ||
                calendarView.SelectedOwnerIndex == view.DisplayedOwnerKeyIndex));
        }

        #endregion

        #region IndicatorColor_ColorDefChanged

        /// <summary>
        /// Handles ColorDefChanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IndicatorColor_ColorDefChanged(object sender, EventArgs e)
        {
            OnTimeIndicatorChanged();
        }

        #endregion

        #region OnTimeIndicatorChanged

        /// <summary>
        /// Handles TimeIndicatorChanged propagation
        /// </summary>
        private void OnTimeIndicatorChanged()
        {
            if (TimeIndicatorChanged != null)
                TimeIndicatorChanged(this, EventArgs.Empty);
        }

        #endregion

        #region OnTimeIndicatorColorChanged

        /// <summary>
        /// Handles OnTimeIndicatorColorChanged propagation
        /// </summary>
        private void OnTimeIndicatorColorChanged()
        {
            if (TimeIndicatorColorChanged != null)
                TimeIndicatorColorChanged(this, new TimeIndicatorColorChangedEventArgs(this));
        }

        #endregion

        #region OnTimeIndicatorTimeChanged

        /// <summary>
        /// Handles TimeIndicatorTimeChanged propagation
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnTimeIndicatorTimeChanged(DateTime oldValue, DateTime newValue)
        {
            if (TimeIndicatorTimeChanged != null)
                TimeIndicatorTimeChanged(this, new TimeIndicatorTimeChangedEventArgs(this, oldValue, newValue));
        }

        #endregion

        #region Begin/EndUpdate

        /// <summary>
        /// Begins Update block
        /// </summary>
        public void BeginUpdate()
        {
            _UpdateCount++;
        }

        /// <summary>
        /// Ends update block
        /// </summary>
        public void EndUpdate()
        {
            if (_UpdateCount == 0)
            {
                throw new InvalidOperationException(
                    "EndUpdate must be called After BeginUpdate");
            }

            _UpdateCount--;

            if (_UpdateCount == 0)
                OnTimeIndicatorChanged();
        }

        #endregion
    }

    #region TimeIndicatorConvertor

    /// <summary>
    /// TimeIndicatorConvertor
    /// </summary>
    public class TimeIndicatorConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (String.Empty);
        }
    }

    #endregion

    #region enums

    #region eTimeIndicatorSource

    /// <summary>
    /// Specifies the source for the IndicatorTime
    /// </summary>
    public enum eTimeIndicatorSource
    {
        SystemTime,
        UserSpecified
    }

    #endregion

    #region eTimeIndicatorVisibility

    /// <summary>
    /// Specifies the Indicator visibility
    /// </summary>
    public enum eTimeIndicatorVisibility
    {
        AllResources,
        SelectedResource,
        Hidden
    }

    #endregion

    #region eTimeIndicatorArea

    /// <summary>
    /// Specifies the Indicator display area
    /// </summary>
    public enum eTimeIndicatorArea
    {
        Header,
        Content,
        All
    }

    #endregion

    #endregion
}
#endif
