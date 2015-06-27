#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents check-box style property node in AdvPropertyGrid.
    /// </summary>
    public class PropertyCheckBoxNode : PropertyNode
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the PropertyNode class.
        /// </summary>
        /// <param name="property"></param>
        public PropertyCheckBoxNode(PropertyDescriptor property):base(property)
        {
        }

        internal override void OnLoaded()
        {
            base.OnLoaded();
            Cell cell = this.EditCell;
            cell.CheckBoxStyle = eCheckBoxStyle.CheckBox;
            cell.CheckBoxAlignment = eCellPartAlignment.NearCenter;
            cell.CheckBoxVisible = true;
        } 

        protected override void UpdateDisplayedValue(object propertyValue)
        {
            base.UpdateDisplayedValue(propertyValue);
            if (propertyValue is bool)
            {
                this.EditCell.Checked = (bool)propertyValue;
            }
            else
                this.EditCell.Checked = false;
        }

        public override void EnterEditorMode(eTreeAction action, bool focusEditor)
        {
        }

        protected override void OnBeforeCellCheck(Cell cell, AdvTreeCellBeforeCheckEventArgs e)
        {
            if (this.IsReadOnly && e.Action != eTreeAction.Code)
                e.Cancel = true;
            else
            {
                if (e.Action != eTreeAction.Code)
                {
                    bool newValue = (e.NewCheckState == System.Windows.Forms.CheckState.Checked);
                    e.Cancel = !ApplyValue(newValue, null);
                }
            }
            base.OnBeforeCellCheck(cell, e);
        }

        #endregion
    }
}
#endif