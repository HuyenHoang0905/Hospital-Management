#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors
{
    public enum eCollectionChangeType
    {
        Removing,
        Removed,
        Clearing,
        Cleared,
        Adding,
        Added
    }

    /// <summary>
    /// Indicates the hour format.
    /// </summary>
    public enum eHourPeriod
    {
        /// <summary>
        /// Indicates Ante Meridiem period, before the middle day.
        /// </summary>
        AM,
        /// <summary>
        /// Indicates Post Meridiem after the middle day".
        /// </summary>
        PM
    }

    /// <summary>
    /// Identifies the date time part.
    /// </summary>
    public enum eDateTimePart
    {
        Year,
        Month,
        Day,
        DayName,
        DayOfYear,
        Hour,
        Minute,
        Second
    }

    /// <summary>
    /// Specifies the input fields alignment inside of the control.
    /// </summary>
    public enum eHorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Specifies the text alignment.
    /// </summary>
    public enum eTextAlignment
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Specifies the format of the date time picker.
    /// </summary>
    public enum eDateTimePickerFormat
    {
        /// <summary>
        /// Indicates that custom format specified by CustomFormat property is used.
        /// </summary>
        Custom,
        /// <summary>
        /// The DateTimePicker control displays the date/time value in the long date format set by the user's operating system. 
        /// </summary>
        Long,
        /// <summary>
        /// The DateTimePicker control displays the date/time value in the short date format set by the user's operating system. 
        /// </summary>
        Short,
        /// <summary>
        /// The DateTimePicker control displays the date/time value in the time format set by the user's operating system. 
        /// </summary>
        LongTime,
        /// <summary>
        /// The DateTimePicker control displays the date/time value in the short time format set by the user's operating system. 
        /// </summary>
        ShortTime
    }

    /// <summary>
    /// Specifies the display format of the year in input control.
    /// </summary>
    public enum eYearDisplayFormat
    {
        FourDigit,
        TwoDigit,
        OneDigit
    }

    /// <summary>
    /// Specifies the visual item alignment inside the parent group.
    /// </summary>
    public enum eItemAlignment
    {
        Left,
        Right
    }

    /// <summary>
    /// Specifies the vertical alignment.
    /// </summary>
    public enum eVerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }

    internal enum eSystemItemType
    {
        Default,
        /// <summary>
        /// Describes the system button item type. System buttons are buttons created internally by controls.
        /// </summary>
        SystemButton
    }

    /// <summary>
    /// Specifies the auto-change item when VisualUpDownButton control is clicked.
    /// </summary>
    public enum eUpDownButtonAutoChange
    {
        /// <summary>
        /// No item is automatically changed.
        /// </summary>
        None,
        /// <summary>
        /// Auto-change focused item in parent group
        /// </summary>
        FocusedItem,
        /// <summary>
        /// Auto change first input item before the Up/Down button in parent group.
        /// </summary>
        PreviousInputItem
    }

    /// <summary>
    /// Specifies the keys used for navigation between input fields.
    /// </summary>
    [Flags()]
    public enum eInputFieldNavigation
    {
        /// <summary>
        /// No key is used for the navigation.
        /// </summary>
        None = 0,
        /// <summary>
        /// Tab key is used to advance to the next input field.
        /// </summary>
        Tab = 1,
        /// <summary>
        /// Arrow keys are used to advance to the next input field.
        /// </summary>
        Arrows = 2,
        /// <summary>
        /// Enter key is used to advance to the next input field.
        /// </summary>
        Enter = 4,
        /// <summary>
        /// Tab, Arrows and Enter keys are used to advance to the next input field.
        /// </summary>
        All = Tab | Arrows | Enter
    }
}
#endif

