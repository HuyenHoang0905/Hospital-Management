using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents editor for Time-Zone ID selection.
    /// </summary>
    public class TimeZoneSelectionEditor : System.Drawing.Design.UITypeEditor
    {
        #region Constructor

        #endregion

        #region Implementation
        private IWindowsFormsEditorService _EditorService = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                ElementStyle es = value as ElementStyle;

                _EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (_EditorService != null)
                {
                    ListBox lb = new ListBox();
                    List<DevComponents.Schedule.TimeZoneInfo> timeZonesList = DevComponents.Schedule.TimeZoneInfo.GetSystemTimeZones();
                    int selectedIndex = -1;
                    for (int i = 0; i < timeZonesList.Count; i++)
                    {
                        DevComponents.Schedule.TimeZoneInfo timeZoneInfo = timeZonesList[i];
                        lb.Items.Add(timeZoneInfo.Id);
                        if (timeZoneInfo.Id.Equals((string)value))
                            selectedIndex = i;
                    }
                    lb.SelectedIndex = selectedIndex;
                    lb.SelectedIndexChanged += new EventHandler(this.SelectedChanged);
                    _EditorService.DropDownControl(lb);
                    return lb.SelectedItem;
                }
            }

            return value;
        }

        private void SelectedChanged(object sender, EventArgs e)
        {
            if (_EditorService != null)
                _EditorService.CloseDropDown();
        }

        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by EditValue. If the UITypeEditor does not support this method, then GetEditStyle will return None</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }
        #endregion
    }
}
