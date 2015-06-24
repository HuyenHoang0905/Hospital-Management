#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.AdvTree;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the option list property node for AdvPropertyGrid.
    /// </summary>
    public class PropertyOptionListNode : PropertyNode
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the PropertyNode class.
        /// </summary>
        /// <param name="property"></param>
        public PropertyOptionListNode(PropertyDescriptor property)
            : base(property)
        {
        }

        internal override void OnLoaded()
        {
            base.OnLoaded();

            Cell cell = this.EditCell;

            ItemContainer container = new ItemContainer();
            container.LayoutOrientation = eOrientation.Vertical;

            string[] optionsList = GetPopupListContent();
            foreach (string optionText in optionsList)
            {
                CheckBoxItem item = new CheckBoxItem();
                item.CheckBoxStyle = eCheckBoxStyle.RadioButton;
                item.Text = optionText;
                item.TextColor = Color.Black;
                item.VerticalPadding = 0;
                item.CheckSignSize = new Size(11, 11);
                item.CheckedChanging += ItemCheckedChanging;
                container.SubItems.Add(item);
            }
            container.Style = eDotNetBarStyle.Office2007;
            cell.HostedItem = container;
        }

        protected override void Dispose(bool disposing)
        {
            BaseItem container = this.EditCell.HostedItem;
            if (container != null)
            {
                this.EditCell.HostedItem = null;
                foreach (BaseItem item in container.SubItems)
                {
                    CheckBoxItem checkItem = item as CheckBoxItem;
                    if (checkItem != null) checkItem.CheckedChanging -= ItemCheckedChanging;
                }
                container.Dispose();
            }
            base.Dispose(disposing);
        }

        void ItemCheckedChanging(object sender, CheckBoxChangeEventArgs e)
        {
            if (e.EventSource != eEventSource.Code)
            {
                Exception valueException = null;
                object value = GetValueFromString(((CheckBoxItem)sender).Text, out valueException);
                e.Cancel = !ApplyValue(value, valueException);
            }
        }

        protected override void UpdateDisplayedValue(object propertyValue)
        {
            base.UpdateDisplayedValue(propertyValue);
            string text = GetPropertyTextValue(propertyValue);
            Cell cell = this.EditCell;
            if (cell.HostedItem == null) return;
            foreach (BaseItem item in cell.HostedItem.SubItems)
            {
                CheckBoxItem checkItem = item as CheckBoxItem;
                if (checkItem != null)
                    checkItem.Checked = checkItem.Text.Equals(text);
            }
        }

        public override void EnterEditorMode(eTreeAction action, bool focusEditor)
        {
        }

        
        #endregion
    }
}
#endif