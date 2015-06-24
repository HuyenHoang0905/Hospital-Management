#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.Editors.DateTimeAdv;
using DevComponents.DotNetBar;

namespace DevComponents.DotNetBar.Design
{
    public class DateTimeInputDesigner : VisualControlBaseDesigner
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the DateTimeInputDesigner class.
        /// </summary>
        public DateTimeInputDesigner()
        {
            this.AutoResizeHandles = true;
        }

        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            DateTimeInput c = this.Control as DateTimeInput;
            if (c != null)
            {
                c.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = eColorSchemePart.PanelBackground;
                c.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
                c.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
                c.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
                c.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
                c.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
                c.MonthCalendar.CommandsBackgroundStyle.BorderTop = eStyleBorderType.Solid;
                c.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = eColorSchemePart.BarDockedBorder;
                c.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
                c.ButtonDropDown.Visible = true;
                c.MonthCalendar.TodayButtonVisible = true;
                c.MonthCalendar.ClearButtonVisible = true;
                c.ButtonDropDown.Shortcut = eShortcut.AltDown;
                c.Style = eDotNetBarStyle.StyleManagerControlled;
            }
            base.InitializeNewComponent(defaultValues);
        }

        public override System.Windows.Forms.Design.SelectionRules SelectionRules
        {
            get
            {
                SelectionRules rules = base.SelectionRules;
                if (!this.Control.AutoSize)
                {
                    rules &= ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
                }
                return rules;
            }
        }
        #endregion
    }
}
#endif

