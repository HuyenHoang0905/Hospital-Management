#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Defines the MonthCalendar and SingleMonthCalendar colors for customization.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class MonthCalendarColors
    {
        private void Refresh()
        {
            if (_Parent != null)
            {
                _Parent.NeedRecalcSize = true;
                _Parent.Refresh();
            }
        }

        private BaseItem _Parent;
        internal BaseItem Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;
                _WeekOfYear.Parent = value;
                _Day.Parent = value;
                _Weekend.Parent = value;
                _TrailingDay.Parent = value;
                _Today.Parent = value;
                _TrailingWeekend.Parent = value;
                _DayLabel.Parent = value;
                _DayMarker.Parent = value;
                _MonthlyMarker.Parent = value;
                _AnnualMarker.Parent = value;
                _Selection.Parent = value;
            }
        }

        private DateAppearanceDescription _Today = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the todays date.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the todays date."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription Today
        {
            get { return _Today; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetToday()
        {
            _Today = new DateAppearanceDescription();
            _Today.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _Selection = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for selected days.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for selected days."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription Selection
        {
            get { return _Selection; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelection()
        {
            _Selection = new DateAppearanceDescription();
            _Selection.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _TrailingDay = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the trailing days on calendar.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the trailing days on calendar."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription TrailingDay
        {
            get { return _TrailingDay; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTrailingDay()
        {
            _TrailingDay = new DateAppearanceDescription();
            _TrailingDay.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _WeekOfYear = new DateAppearanceDescription();
        /// <summary>
        /// Gets or sets the appearance settings for the labels that show week of year number.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the labels that show week of year number."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription WeekOfYear
        {
            get { return _WeekOfYear; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWeekOfYear()
        {
            _WeekOfYear = new DateAppearanceDescription();
            _WeekOfYear.Parent = _Parent;
            this.Refresh();
        }

        private Color _DaysDividerBorderColors = Color.Empty;
        /// <summary>
        /// Gets or sets the days divider line color.
        /// </summary>
        [Category("Colors"), Description("Indicates days divider line color.")]
        public Color DaysDividerBorderColors
        {
            get { return _DaysDividerBorderColors; }
            set
            {
                _DaysDividerBorderColors = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDaysDividerBorderColors()
        {
            return !DaysDividerBorderColors.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDaysDividerBorderColors()
        {
            DaysDividerBorderColors = Color.Empty;
        }

        private DateAppearanceDescription _Day = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the numeric day of month label.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the numeric day of month label."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription Day
        {
            get { return _Day; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDay()
        {
            _Day = new DateAppearanceDescription();
            _Day.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _Weekend = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the weekend days on calendar (Saturday and Sunday).
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the weekend days on calendar (Saturday and Sunday)."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription Weekend
        {
            get { return _Weekend; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWeekend()
        {
            _Weekend = new DateAppearanceDescription();
            _Weekend.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _TrailingWeekend = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the trailing weekend days on calendar (Saturday and Sunday).
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the trailing weekend days on calendar (Saturday and Sunday)."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription TrailingWeekend
        {
            get { return _TrailingWeekend; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTrailingWeekend()
        {
            _TrailingWeekend = new DateAppearanceDescription();
            _TrailingWeekend.Parent = _Parent;
            this.Refresh();
        }

        private DateAppearanceDescription _DayLabel = new DateAppearanceDescription();
        /// <summary>
        /// Gets the appearance settings for the labels that display day name in calendar header.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Indicates appearance settings for the labels that display day name in calendar header."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription DayLabel
        {
            get { return _DayLabel; }
        }
        /// <summary>
        /// Resets property to its default value. Provided for Windows Forms designer support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetDayLabel()
        {
            _DayLabel = new DateAppearanceDescription();
            _DayLabel.Parent = _Parent;
            this.Refresh();
        }


        private DateAppearanceDescription _MonthlyMarker = new DateAppearanceDescription();
        /// <summary>
        /// Gets or sets the marker settings for days specified by MonthCalendarItem.MonthlyMarkedDates property.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Gets marker settings for days specified by MonthCalendarAdv.MonthlyMarkedDates property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription MonthlyMarker
        {
            get { return _MonthlyMarker; }
        }

        private DateAppearanceDescription _AnnualMarker = new DateAppearanceDescription();
        /// <summary>
        /// Gets or sets the marker settings for days specified by MonthCalendarItem.AnnuallyMarkedDates property.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Gets marker settings for days specified by MonthCalendarAdv.AnnuallyMarkedDates property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription AnnualMarker
        {
            get { return _AnnualMarker; }
        }

        private DateAppearanceDescription _DayMarker = new DateAppearanceDescription();
        /// <summary>
        /// Gets or sets the marker settings for days specified by MonthCalendarItem.MarkedDates property.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Gets marker settings for days specified by MonthCalendarAdv.MarkedDates property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription DayMarker
        {
            get { return _DayMarker; }
        }

        private DateAppearanceDescription _WeeklyMarker = new DateAppearanceDescription();
        /// <summary>
        /// Gets or sets the marker settings for days specified by MonthCalendarItem.WeeklyMarkedDays property.
        /// </summary>
        [NotifyParentPropertyAttribute(true), Description("Gets marker settings for days specified by MonthCalendarAdv.WeeklyMarkedDays property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DateAppearanceDescription WeeklyMarker
        {
            get { return _WeeklyMarker; }
        }
    }
}
#endif

