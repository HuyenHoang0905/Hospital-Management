using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms Designer for GalleryContainer object.
    /// </summary>
    public class GalleryContainerDesigner : ItemContainerDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (component is GalleryContainer)
                ((GalleryContainer)component).PopupGalleryItem.SetDesignMode(true);
        }

        protected override void SetDesignTimeDefaults()
        {
            GalleryContainer gc = this.Component as GalleryContainer;
            gc.StretchGallery = true;
            gc.BackgroundStyle.Class = Rendering.ElementStyleClassKeys.RibbonGalleryContainerKey;
            base.SetDesignTimeDefaults();
        }

        protected override void AddNewItem(BaseItem newItem)
        {
            GalleryContainer gc = this.Component as GalleryContainer;
            if (gc != null && !gc.DesignTimeMouseDownPoint.IsEmpty)
            {
                if (gc.PopupGalleryButtonBounds.Contains(gc.DesignTimeMouseDownPoint))
                {
                    gc.DesignTimeMouseDownPoint = Point.Empty;
                    // Add new item to the PopupGalleryItems collection instead of to SubItems...
                    System.ComponentModel.Design.IComponentChangeService change = this.GetService(typeof(System.ComponentModel.Design.IComponentChangeService)) as IComponentChangeService;
                    if (change != null)
                        change.OnComponentChanging(this.Component, TypeDescriptor.GetProperties(this.Component).Find("PopupGalleryItems", true));

                    gc.PopupGalleryItems.Add(newItem);

                    if (change != null)
                        change.OnComponentChanged(this.Component, TypeDescriptor.GetProperties(this.Component).Find("PopupGalleryItems", true), null, null);
                    return;
                }
            }

            base.AddNewItem(newItem);
        }

        protected override void NewItemAdded(BaseItem itemAdded)
        {
            base.NewItemAdded(itemAdded);
            GalleryContainer gc = this.Component as GalleryContainer;
            if (gc != null && gc.SubItems.Contains(itemAdded) && !(gc.Parent is ButtonItem))
                gc.EnsureVisible(itemAdded);
        }

        protected override void RecalcLayout()
        {
            base.RecalcLayout();
            GalleryContainer gc = this.Component as GalleryContainer;
            if (gc.PopupGalleryItem.Expanded && gc.PopupGalleryItem.PopupControl != null)
            {
                Control control = gc.PopupGalleryItem.PopupControl;
                if (control is MenuPanel)
                    ((MenuPanel)control).RecalcSize();
                else if (control is Bar)
                    ((Bar)control).RecalcLayout();
            }
        }

        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                System.Collections.ArrayList components = new System.Collections.ArrayList();
                components.AddRange(base.AssociatedComponents);
                GalleryContainer gc = this.Component as GalleryContainer;
                if (gc == null)
                    return components;
                gc.PopupGalleryItems.CopyTo(components);
                return components;
            }
        }

        protected override void ComponentRemoved(ComponentEventArgs e)
        {
            if (e.Component is GalleryGroup)
            {
                GalleryGroup g = e.Component  as GalleryGroup;
                GalleryContainer c = this.Component as GalleryContainer;
                if (c != null && c.GalleryGroups.Contains(g))
                {
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (cc != null)
                        cc.OnComponentChanging(c, TypeDescriptor.GetProperties(c)["GalleryGroups"]);
                    c.GalleryGroups.Remove(g);
                    if (cc != null)
                        cc.OnComponentChanged(c, TypeDescriptor.GetProperties(c)["GalleryGroups"], null, null);
                }
            }
            base.ComponentRemoved(e);
        }
    }
}
