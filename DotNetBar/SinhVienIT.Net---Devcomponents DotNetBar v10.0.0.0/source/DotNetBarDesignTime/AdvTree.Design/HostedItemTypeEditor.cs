using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing.Design;

namespace DevComponents.AdvTree.Design
{
    public class HostedItemTypeEditor : System.Drawing.Design.UITypeEditor
    {
        #region Private Variables
        private IWindowsFormsEditorService m_EditorService = null;
        private const string OPTION_REMOVE = "Delete Item";
        private Dictionary<string, Type> _Items = new Dictionary<string, Type>();
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the HostedItemTypeEditor class.
        /// </summary>
        public HostedItemTypeEditor() : base()
        {
            _Items.Add("Button", typeof(ButtonItem));
            _Items.Add("Check-box", typeof(CheckBoxItem));
            _Items.Add("Micro-Chart", typeof(MicroChartItem));
            _Items.Add("Switch", typeof(SwitchButtonItem));
            _Items.Add("Slider", typeof(SliderItem));
            _Items.Add("Rating", typeof(RatingItem));
            _Items.Add("Progress bar", typeof(ProgressBarItem));
            _Items.Add("Container", typeof(ItemContainer));
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                m_EditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (m_EditorService != null)
                {
                    Cell currentCell = null;
                    if (context.Instance is Node)
                        currentCell = ((Node)context.Instance).Cells[0];
                    else if (context.Instance is Cell)
                        currentCell = (Cell)context.Instance;
                    if (currentCell == null)
                    {
                        MessageBox.Show(string.Format("Invalid context.Instance. Instance: {0}", context.Instance));
                        return value;
                    }

                    ListBox lb = new ListBox();

                    if (currentCell.HostedItem != null)
                    {
                        lb.Items.Add(OPTION_REMOVE);
                        lb.Items.Add("------------------");
                    }
                    foreach (KeyValuePair<string, Type> item in _Items)
                    {
                        lb.Items.Add(item.Key);
                    }
                    

                    lb.SelectedIndexChanged += new EventHandler(this.SelectedChanged);
                    m_EditorService.DropDownControl(lb);

                    IDesignerHost dh = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
                    if (lb.SelectedItem != null && dh != null)
                    {
                        if (lb.SelectedItem == OPTION_REMOVE)
                        {
                            BaseItem item = currentCell.HostedItem;
                            dh.DestroyComponent(item);
                            value = null;
                        }
                        else if (_Items.ContainsKey(lb.SelectedItem.ToString()))
                        {
                            if (currentCell.HostedItem != null)
                            {
                                BaseItem item = currentCell.HostedItem;
                                dh.DestroyComponent(item);
                            }

                            BaseItem newItem = dh.CreateComponent(_Items[(string)lb.SelectedItem]) as BaseItem;
                            if (!string.IsNullOrEmpty(newItem.Name) && string.IsNullOrEmpty(newItem.Text))
                                TypeDescriptor.GetProperties(newItem)["Text"].SetValue(newItem, newItem.Name);
                            value = newItem;
                        }
                    }
                }
            }

            return value;
        }

        private void SelectedChanged(object sender, EventArgs e)
        {
            if (m_EditorService != null)
                m_EditorService.CloseDropDown();
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
