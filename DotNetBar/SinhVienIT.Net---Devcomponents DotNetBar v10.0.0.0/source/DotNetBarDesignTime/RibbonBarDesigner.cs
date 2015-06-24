using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents Windows Forms Designer for RibbonBar control
	/// </summary>
	public class RibbonBarDesigner:BarBaseControlDesigner
	{
		public RibbonBarDesigner()
		{
			this.EnableItemDragDrop=true;
		}

		public override void Initialize(IComponent component) 
		{
			base.Initialize(component);
			if(component==null || component.Site==null || !component.Site.DesignMode)
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
            RibbonBar b = this.Control as RibbonBar;
            this.Style = eDotNetBarStyle.StyleManagerControlled;
#if !TRIAL
            string key = GetLicenseKey();
            b.LicenseKey = key;
#endif
        }

		public override DesignerVerbCollection Verbs 
		{
			get 
			{
				Bar bar=this.Control as Bar;
				DesignerVerb[] verbs=null;
				verbs = new DesignerVerb[]
						{
							new DesignerVerb("Add Button", new EventHandler(CreateButton)),
							new DesignerVerb("Add Horizontal Container", new EventHandler(CreateHorizontalContainer)),
							new DesignerVerb("Add Vertical Container", new EventHandler(CreateVerticalContainer)),
							new DesignerVerb("Add Text Box", new EventHandler(CreateTextBox)),
							new DesignerVerb("Add Combo Box", new EventHandler(CreateComboBox)),
							new DesignerVerb("Add Label", new EventHandler(CreateLabel)),
							new DesignerVerb("Add Color Picker", new EventHandler(CreateColorPicker)),
                            new DesignerVerb("Add Check Box", new EventHandler(CreateCheckBox)),
                            new DesignerVerb("Add Micro-Chart", new EventHandler(CreateMicroChart)),
                            new DesignerVerb("Add Switch Button", new EventHandler(CreateSwitch)),
                            new DesignerVerb("Add Slider", new EventHandler(CreateSliderItem)),
                            new DesignerVerb("Add Rating Item", new EventHandler(CreateRatingItem)),
                            new DesignerVerb("Add Circular Progress", new EventHandler(CreateCircularProgressItem)),
                            new DesignerVerb("Add Gallery Container", new EventHandler(CreateGalleryContainer)),
                            new DesignerVerb("Add Control Container", new EventHandler(CreateControlContainer))};
				return new DesignerVerbCollection(verbs);
			}
		}

		private void CreateVerticalContainer(object sender, EventArgs e)
		{
			CreateContainer(this.GetItemContainer(),eOrientation.Vertical);
		}

		private void CreateHorizontalContainer(object sender, EventArgs e)
		{
			CreateContainer(this.GetItemContainer(),eOrientation.Horizontal);
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
		protected override void PreFilterProperties(
			System.Collections.IDictionary properties) 
		{
			base.PreFilterProperties(properties);
			properties["Style"] = 
				TypeDescriptor.CreateProperty(typeof(RibbonBarDesigner), 
				(PropertyDescriptor)properties["Style"], 
				new Attribute[0]);
		}

		/// <summary>
		/// Gets/Sets the visual style of the control.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("Specifies the visual style of the control."),DefaultValue(eDotNetBarStyle.Office2003)]
		public eDotNetBarStyle Style
		{
			get
			{
				RibbonBar bar=this.Control as RibbonBar;
				return bar.Style;
			}
			set 
			{
				RibbonBar bar=this.Control as RibbonBar;
				bar.Style=value;
				IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if(dh!=null && !dh.Loading)
				{
                    RibbonPredefinedColorSchemes.SetRibbonBarStyle(bar, value);
				}
			}
		}

        protected override void OnitemCreated(BaseItem item)
        {
            if (item is ButtonItem)
            {
                ButtonItem b = item as ButtonItem;
                TypeDescriptor.GetProperties(b)["SubItemsExpandWidth"].SetValue(b, 14);
            }
            base.OnitemCreated(item);
        }

        #region Licensing Stuff
#if !TRIAL
        internal static string GetLicenseKey()
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
            RibbonBar bar = this.Control as RibbonBar;
            if (key != "" && bar != null && bar.LicenseKey == "" && bar.LicenseKey != key)
                TypeDescriptor.GetProperties(bar)["LicenseKey"].SetValue(bar, key);
        }
#endif
        #endregion
	}
}
