using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms Designer for the ItemPanel control.
    /// </summary>
    public class ItemPanelDesigner : BarBaseControlDesigner
    {
        public ItemPanelDesigner()
		{
			this.EnableItemDragDrop=true;
		}

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (component == null || component.Site == null || !component.Site.DesignMode)
                return;

#if !TRIAL
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete += new EventHandler(dh_LoadComplete);
#endif
        }

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
            SetDesignTimeDefaults();
			base.OnSetComponentDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            ItemPanel panel = this.Control as ItemPanel;
            panel.LayoutOrientation = eOrientation.Vertical;
            panel.BackgroundStyle.Class = ElementStyleClassKeys.ItemPanelKey;
            //panel.BackgroundStyle.Border = eStyleBorderType.Solid;
            //panel.BackgroundStyle.BorderColor = ColorScheme.GetColor("7F9DB9");
            //panel.BackgroundStyle.BorderWidth = 1;
            //panel.BackgroundStyle.PaddingLeft = 1;
            //panel.BackgroundStyle.PaddingRight = 1;
            //panel.BackgroundStyle.PaddingTop = 1;
            //panel.BackgroundStyle.PaddingBottom = 1;
            //panel.BackgroundStyle.BackColor = Color.White;
#if !TRIAL
            string key = GetLicenseKey();
            panel.LicenseKey = key;
#endif
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
							new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
							new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
							new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
							new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                            new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                            new DesignerVerb("Add Switch Button", new EventHandler(CreateSwitch)),
                            new DesignerVerb("Add Progress bar", new EventHandler(CreateProgressBar)),
                            new DesignerVerb("Add Check box", new EventHandler(CreateCheckBox)),
                            new DesignerVerb("Add WinForms Control Container", new EventHandler(CreateControlContainer)),
                            new DesignerVerb("Apply Panel Style", new EventHandler(ApplyPanelStyle)),
                            new DesignerVerb("Apply Default Style", new EventHandler(ApplyDefaultStyle))
                        };
                return new DesignerVerbCollection(verbs);
            }
        }

        private void ApplyPanelStyle(object sender, EventArgs e)
        {
            ItemPanel p = this.Control as ItemPanel;
            if (p == null)
                return;

            IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (change != null)
                change.OnComponentChanging(this.Component, null);

            ElementStyle bs = p.BackgroundStyle;
            bs.Reset();

            bs.Border = eStyleBorderType.Solid;
            bs.BorderWidth = 1;
            bs.BorderColorSchemePart = eColorSchemePart.PanelBorder;
            bs.BackColorSchemePart = eColorSchemePart.PanelBackground;
            bs.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
            bs.BackColorGradientAngle = 90;
            
            if (change != null)
                change.OnComponentChanged(this.Component, null, null, null);
        }

        private void ApplyDefaultStyle(object sender, EventArgs e)
        {
            ItemPanel p = this.Control as ItemPanel;
            if (p == null)
                return;

            IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (change != null)
                change.OnComponentChanging(this.Component, null);

            ElementStyle bs = p.BackgroundStyle;
            bs.Reset();

            bs.Border = eStyleBorderType.Solid;
            bs.BorderWidth = 1;
            bs.BorderColorSchemePart = eColorSchemePart.PanelBorder;
            bs.BackColor = Color.White;

            if (change != null)
                change.OnComponentChanged(this.Component, null, null, null);
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
            m_CreatingItem = true;
            try
            {
                DesignerSupport.CreateItemContainer(this, parent, orientation);
            }
            finally
            {
                m_CreatingItem = false;
            }
            this.RecalcLayout();
        }

        #region Licensing Stuff
#if !TRIAL
        private string GetLicenseKey()
        {
            string key = "";
            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine;
            regkey = regkey.OpenSubKey("Software\\DevComponents\\Licenses", false);
            if (regkey != null)
            {
                object keyValue = regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2");
                if (keyValue != null)
                    key = keyValue.ToString();
            }
            return key;
        }
        private void dh_LoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (dh != null)
                dh.LoadComplete -= new EventHandler(dh_LoadComplete);

            string key = GetLicenseKey();
            ItemPanel bar = this.Control as ItemPanel;
            if (key != "" && bar != null && bar.LicenseKey == "" && bar.LicenseKey != key)
                TypeDescriptor.GetProperties(bar)["LicenseKey"].SetValue(bar, key);
        }
#endif
        #endregion
    }
}
