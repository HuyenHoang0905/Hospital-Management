using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents GalleryGroup type editor provided for Windows Forms designer support.
    /// </summary>
    public class GalleryGroupTypeEditor : UITypeEditor
    {
        private IWindowsFormsEditorService m_EdSvc = null;
        private const string CREATE_NEW_GROUP = "<Create new group>";

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            if (context != null && context.Instance != null && provider != null)
            {
                m_EdSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (m_EdSvc != null)
                {
                    GalleryGroupCollection groups = null;
                    BaseItem itemToAssign = null;
                    GalleryContainer gallery = null;
                    if (context.Instance is BaseItem)
                    {
                        itemToAssign = (BaseItem)context.Instance;
                        gallery = itemToAssign.Parent as GalleryContainer;
                        if (gallery != null)
                            groups = gallery.GalleryGroups;
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Item does not belong to the Gallery. Cannot edit groups.");
                            return value;
                        }
                    }
                    if (groups == null && context.Instance != null)
                        System.Windows.Forms.MessageBox.Show("Unknow control using GalleryGroupTypeEditor. Cannot edit groups. [" + context.Instance.ToString() + "]");
                    else if (groups == null)
                        System.Windows.Forms.MessageBox.Show("Unknow control using GalleryGroupTypeEditor. Cannot edit groups. [context instance null]");

                    GalleryGroup selectedGroup = gallery.GetGalleryGroup(itemToAssign);
                    ListBox listBox = new ListBox();
                    foreach (GalleryGroup g in groups)
                    {
                        listBox.Items.Add(g);
                        if (g == selectedGroup)
                            listBox.SelectedItem = g;
                    }

                    listBox.Items.Add(CREATE_NEW_GROUP);

                    listBox.SelectedIndexChanged += new EventHandler(this.SelectedChanged);
                    m_EdSvc.DropDownControl(listBox);
                    if (listBox.SelectedItem is string && listBox.SelectedItem.ToString() == CREATE_NEW_GROUP)
                        value = DesignerSupport.CreateGalleryGroup(gallery, provider);
                    else
                        value = listBox.SelectedItem;
                }
            }

            return value;
        }

        private void SelectedChanged(object sender, EventArgs e)
        {
            if (m_EdSvc != null)
                m_EdSvc.CloseDropDown();
        }
    }
}
