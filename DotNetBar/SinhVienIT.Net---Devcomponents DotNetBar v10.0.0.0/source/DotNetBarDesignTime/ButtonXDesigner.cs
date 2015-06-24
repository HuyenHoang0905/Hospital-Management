using System;
using System.Text;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms designer for ButtonX control.
    /// </summary>
    public class ButtonXDesigner : BarBaseControlDesigner
    {
        #region Constructor
        public ButtonXDesigner()
		{
			this.EnableItemDragDrop=false;
            this.PassiveContainer = true;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

            ((ButtonX)component).SetDesignMode(true);
        }
        #endregion

        #region Internal Implementation
#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
            SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            ButtonX button = this.Control as ButtonX;
            if (button == null)
                return;
            button.ColorTable = eButtonColor.OrangeWithBackground;
            button.Style = eDotNetBarStyle.StyleManagerControlled;
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                Bar bar = this.Control as Bar;
                DesignerVerb[] verbs = null;
                verbs = new DesignerVerb[]
						{
							new DesignerVerb("Add Button", new EventHandler(CreateButton)),
							new DesignerVerb("Add Horizontal Container", new EventHandler(CreateHorizontalContainer)),
							new DesignerVerb("Add Vertical Container", new EventHandler(CreateVerticalContainer)),
                            new DesignerVerb("Add Gallery Container", new EventHandler(CreateGalleryContainer)),
                            new DesignerVerb("Add Scrollable Container", new EventHandler(CreateScrollContainer)),
							new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
							new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
							new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
                            new DesignerVerb("Add Check Box", new EventHandler(CreateCheckBox)),
                            new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                            new DesignerVerb("Add Switch Button", new EventHandler(CreateSwitch)),
                            new DesignerVerb("Add Slider", new EventHandler(CreateSliderItem)),
                            new DesignerVerb("Add Rating Item", new EventHandler(CreateRatingItem)),
                            new DesignerVerb("Add Control Container", new EventHandler(CreateControlContainer)),
							new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker))};
                return new DesignerVerbCollection(verbs);
            }
        }

        protected override void OnitemCreated(BaseItem item)
        {
            TypeDescriptor.GetProperties(item)["GlobalItem"].SetValue(item, false);
        }

        private void CreateScrollContainer(object sender, EventArgs e)
        {
            GalleryContainer item = CreateComponent(typeof(GalleryContainer)) as GalleryContainer;
            TypeDescriptor.GetProperties(item)["MinimumSize"].SetValue(item, new Size(150, 200));
            TypeDescriptor.GetProperties(((GalleryContainer)item).BackgroundStyle)["Class"].SetValue(((GalleryContainer)item).BackgroundStyle, "");
            TypeDescriptor.GetProperties(item)["EnableGalleryPopup"].SetValue(item, false);
            TypeDescriptor.GetProperties(item)["LayoutOrientation"].SetValue(item, DevComponents.DotNetBar.eOrientation.Vertical);
            TypeDescriptor.GetProperties(item)["MultiLine"].SetValue(item, false);
            TypeDescriptor.GetProperties(item)["PopupUsesStandardScrollbars"].SetValue(item, false);
            item.NeedRecalcSize = true;
            this.RecalcLayout();
        }


        private void CreateVerticalContainer(object sender, EventArgs e)
        {
            CreateContainer(this.GetItemContainer(), eOrientation.Vertical);
        }

        private void CreateHorizontalContainer(object sender, EventArgs e)
        {
            CreateContainer(this.GetItemContainer(), eOrientation.Horizontal);
        }

        private void CreateContainer(BaseItem parent, eOrientation orientation)
        {
            try
            {
                m_CreatingItem = true;
                DesignerSupport.CreateItemContainer(this, parent, orientation);
            }
            finally
            {
                m_CreatingItem = false;
            }
            this.RecalcLayout();
        }

        protected override BaseItem GetItemContainer()
        {
            ButtonX button = this.Control as ButtonX;
            if (button != null)
                return button.InternalItem;
            return base.GetItemContainer();
        }

        public override bool CanParent(Control control)
        {
            return false;
        }

        protected override void OnMouseDragBegin(int x, int y)
        {
            ButtonX ctrl = this.GetItemContainerControl() as ButtonX;
            if (ctrl != null)
            {
                ISelectionService ss = GetService(typeof(ISelectionService)) as ISelectionService;
                bool selected = false;
                if (ss != null) selected = ss.PrimarySelection == this.Component;
                ButtonItem container = this.GetItemContainer() as ButtonItem;
                Point pos = ctrl.PointToClient(new Point(x, y));
                MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, pos.X, pos.Y, 0);
                if (container != null &&
                    (!container.SubItemsRect.IsEmpty && container.SubItemsRect.Contains(pos) || 
                    (!container.ShowSubItems || container.SubItemsExpandWidth <= 1) && selected) 
                    && container.SubItems.Count > 0)
                {
                    container.Expanded = !container.Expanded;
                    return;
                }
            }
            base.OnMouseDragBegin(x, y);
        }

        protected override bool OnMouseDown(ref Message m, MouseButtons button)
        {
            return false;
        }

        protected override bool OnMouseUp(ref Message m)
        {
            ButtonX ctrl = this.GetItemContainerControl() as ButtonX;
            if (ctrl != null)
            {
                if (ctrl.Expanded && !this.DragInProgress)
                    return true;
            }

            bool bProcessed = false;

            BaseItem container = this.GetItemContainer();
            IOwner owner = this.GetIOwner();

            if (ctrl == null || owner == null || container == null)
                return false;

            Point pos = ctrl.PointToClient(System.Windows.Forms.Control.MousePosition);
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, pos.X, pos.Y, 0);
            container.InternalMouseUp(e);

            if (this.DragItem != null)
            {
                MouseDragDrop(pos.X, pos.Y);
                return true;
            }

            return bProcessed;
        }

        /// <summary>
        /// Triggered when some other component on the form is removed.
        /// </summary>
        protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
        {
            bool callBase = true;
            if (e.Component is BaseItem)
            {
                BaseItem item = e.Component as BaseItem;
                BaseItem parent = this.GetItemContainer();
                if (item!=null && parent!=null && parent.SubItems.Contains(item))
                {
                    parent.SubItems.Remove(item);
                    this.RecalcLayout();
                    callBase = false;
                }
            }
            if(callBase)
                base.OtherComponentRemoving(sender, e);
        }
        #endregion
    }
}
