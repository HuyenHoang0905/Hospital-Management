#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.Editors.DateTimeAdv;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms designer for MonthCalendarAdv control.
    /// </summary>
    public class MonthCalendarAdvDesigner : ControlDesigner
    {
        #region Private Variables
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            MonthCalendarAdv mc = this.Control as MonthCalendarAdv;
            if (mc != null)
            {
                mc.BackgroundStyle.BackColor = SystemColors.Window;
                mc.BackgroundStyle.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
                mc.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
                mc.BackgroundStyle.BorderWidth = 1;
                mc.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
                mc.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
                mc.NavigationBackgroundStyle.BackColorGradientAngle = 90;
                mc.AutoSize = true;
            }
            base.InitializeNewComponent(defaultValues);
        }
        #endregion

    }
}
#endif

