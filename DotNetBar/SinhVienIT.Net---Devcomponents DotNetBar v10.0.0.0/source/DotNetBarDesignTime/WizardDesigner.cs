using System;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms Designer for Wizard control.
    /// </summary>
    public class WizardDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        #region Internal Implementation
		WizardPageNavigationControl m_NavigationControl=null;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
            {
                cc.ComponentRemoved += new ComponentEventHandler(this.OnComponentRemoved);
            }

			// Setup navigation box in lower left corner
			Wizard w=component as Wizard;
			if(w!=null)
			{
				WizardPageNavigationControl wpn=new WizardPageNavigationControl();
				wpn.Location=new Point(4, w.panelFooter.Height - wpn.Height - 4);
				w.panelFooter.Controls.Add(wpn);
				wpn.BackColor=SystemColors.Control;
				m_NavigationControl=wpn;
			}

#if !TRIAL
			IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (dh != null)
				dh.LoadComplete += new EventHandler(dh_LoadComplete);
#endif
        }
        protected override void Dispose(bool disposing)
        {
            IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            if (cc != null)
            {
                cc.ComponentRemoved -= new ComponentEventHandler(this.OnComponentRemoved);
            }

            base.Dispose(disposing);
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs e)
        {
            if (e.Component is WizardPage)
            {
                WizardPage page = e.Component as WizardPage;
                Wizard w = this.Control as Wizard;

                if (page != null && w.WizardPages.Contains(page))
                {
                    IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    if (cc != null)
                        cc.OnComponentChanging(w, TypeDescriptor.GetProperties(w)["WizardPages"]);

                    w.WizardPages.Remove(page);

                    if (cc != null)
                        cc.OnComponentChanged(w, TypeDescriptor.GetProperties(w)["WizardPages"], null, null);
                }
            }
        }

        public override void DoDefaultAction()
        {
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = null;
                verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Inner Page", new EventHandler(CreatePage)),
						new DesignerVerb("Create Welcome Page", new EventHandler(CreateWelcomePage)),
						new DesignerVerb("Create Outer Page", new EventHandler(CreateOuterPage)),
                        new DesignerVerb("Delete Page", new EventHandler(DeletePage)),
                        new DesignerVerb("Next Page", new EventHandler(NextPage)),
                        new DesignerVerb("Previous Page", new EventHandler(PreviousPage)),
                        new DesignerVerb("Goto Page/Change Order", new EventHandler(GotoPage))
                    };
                return new DesignerVerbCollection(verbs);
            }
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
			base.OnSetComponentDefaults();
            SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            Wizard w = this.Control as Wizard;
            if (w == null)
                return;
            TypeDescriptor.GetProperties(w)["HeaderImageVisible"].SetValue(w, true);
            TypeDescriptor.GetProperties(w)["Dock"].SetValue(w,DockStyle.Fill);

            SetDefaultFooterStyle(w);
#if !TRIAL
			string key = GetLicenseKey();
			w.LicenseKey = key;
#endif
        }

        private void GotoPage(object sender, EventArgs e)
        {
            Wizard w = this.Control as Wizard;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;
            GotoPage(w, cc, ss);
        }

        internal static void GotoPage(Wizard w, IComponentChangeService cc, ISelectionService ss)
        {
            if (w == null)
                return;

            WizardPageOrderDialog d = new WizardPageOrderDialog();
            d.SetWizard(w);
            d.StartPosition = FormStartPosition.CenterScreen;
            if (d.ShowDialog() == DialogResult.OK)
            {
                string pageName = d.SelectedPageName;
                
                if (d.OrderChanged)
                {
                    if (cc != null)
                        cc.OnComponentChanging(w, TypeDescriptor.GetProperties(w)["WizardPages"]);

                    string[] newOrder = d.OrderedPageNames;
                    w.WizardPages.IgnoreEvents = true;
                    try
                    {
                        WizardPageCollection col = new WizardPageCollection();
                        w.WizardPages.CopyTo(col);
                        w.WizardPages.Clear();
                        foreach (string pn in newOrder)
                            w.WizardPages.Add(col[pn]);
                    }
                    finally
                    {
                        w.WizardPages.IgnoreEvents = false;
                    }

                    if (cc != null)
                        cc.OnComponentChanged(w, TypeDescriptor.GetProperties(w)["WizardPages"], null, null);
                }

                if (pageName != "")
                    w.SelectedPage = w.WizardPages[pageName];
                else if (d.OrderChanged)
                    w.SelectedPageIndex = 0;

                if (ss != null && (pageName!="" || d.OrderChanged))
                    ss.SetSelectedComponents(new WizardPage[] { w.SelectedPage }, SelectionTypes.Replace);
            }
            d.Dispose();
        }

        private void CreatePage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            CreatePage(this.Control as Wizard, true, dh, cc, ss, m_WizardStyle);
        }

        private void CreateWelcomePage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            CreateWelcomePage(this.Control as Wizard, dh, cc, ss, this.WizardStyle);
        }

        private void CreateOuterPage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            CreatePage(this.Control as Wizard, false, dh, cc, ss, m_WizardStyle);
        }

        private void DeletePage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            Wizard w = this.Control as Wizard;
            if (w == null)
                return;
            DeletePage(w.SelectedPage, dh, cc);
        }

        internal static void DeletePage(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
        {
            if (page == null || !(page.Parent is Wizard))
                return;

            Wizard w = page.Parent as Wizard;

            DesignerTransaction dt = dh.CreateTransaction();

            try
            {
                if (cc != null)
                    cc.OnComponentChanging(w, TypeDescriptor.GetProperties(w)["WizardPages"]);

                w.WizardPages.Remove(page);

                if (cc != null)
                    cc.OnComponentChanged(w, TypeDescriptor.GetProperties(w)["WizardPages"], null, null);

                dh.DestroyComponent(page);
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled)
                    dt.Commit();
            }
        }

        private void NextPage(object sender, EventArgs e)
        {
            SelectNextPage(this.Control as Wizard);
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            SelectPreviousPage(this.Control as Wizard);
        }

        internal static bool SelectNextPage(Wizard w)
        {
            if (w == null)
                return false;

            WizardPage page = w.GetNextPage();
            if (page != null)
            {
                w.SelectedPage = page;
                return true;
            }
            return false;
        }

        internal static bool SelectPreviousPage(Wizard w)
        {
            if (w == null)
                return false;

            WizardPage page = w.GetBackPage();
            if (page != null)
            {
                w.SelectedPage = page;
                return true;
            }
            return false;
        }

        private enum eWizardImages
        {
            WelcomeDefault,
            BackgroundOffice2007
        }

        private static Image LoadWizardImage(eWizardImages wizardImage)
        {
            string wizardImageName = "WizardWelcomeImage.png";
            if (wizardImage == eWizardImages.BackgroundOffice2007)
                wizardImageName = "WizardOffice2007Background.png";
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                string path = "";
                try
                {
                    if (key != null)
                        key = key.OpenSubKey("Software\\DevComponents\\DotNetBar");
                    if (key != null)
                        path = key.GetValue("InstallationFolder", "").ToString();
                }
                finally { if (key != null) key.Close(); }

                if (path != "")
                {
                    if (path.Substring(path.Length - 1, 1) != "\\")
                        path += "\\";

                    if (System.IO.File.Exists(path + wizardImageName))
                        path += wizardImageName;
                    else
                        path = "";
                }

                if (path != "")
                {
                    return new Bitmap(path);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        internal static void ApplyOffice2007WelcomePageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
        {
            DesignerTransaction dt = null;
            if (dh != null) dt = dh.CreateTransaction();
            
            try
            {
                if (cc != null) cc.OnComponentChanging(page, null);
                page.BackColor = Color.Transparent;
                page.Style.Reset();
                //page.BackColor = ColorScheme.GetColor(0xBBDBF7);
                //page.CanvasColor = page.BackColor;
                //page.Style.BackColor = Color.Empty;
                //page.Style.BackColor2 = Color.Empty;
                //page.Style.BackColorBlend.Clear();
                //page.Style.BackColorBlend.AddRange(new BackgroundColorBlend[] {
                //new BackgroundColorBlend(ColorScheme.GetColor(0xBBDBF7), 0f),
                //new BackgroundColorBlend(ColorScheme.GetColor(0xBBDBF7), .3f),
                //new BackgroundColorBlend(ColorScheme.GetColor(0xF3F9FE), .9f),
                //new BackgroundColorBlend(ColorScheme.GetColor(0xFEFFFF), 1f)});
                //page.Style.BackColorGradientAngle = 90;
                //page.Style.BackgroundImage = GetWelcomeImage(eWizardImages.BackgroundOffice2007);
                //page.Style.BackgroundImagePosition = eStyleBackgroundImage.TopLeft;

                if (cc != null) cc.OnComponentChanged(page, null, null, null);
            }
            catch
            {
                dt.Cancel();
                throw;
            }
            finally
            {
                if (dt != null && !dt.Canceled)
                    dt.Commit();
            }
        }

        internal static void ApplyOffice2007InnerPageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
        {
            if (cc != null) cc.OnComponentChanging(page, null);
            
            page.BackColor = Color.Transparent;
            page.Style.Reset();
            
            if (cc != null) cc.OnComponentChanged(page, null, null, null);
        }

        internal static void ApplyDefaultInnerPageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
        {
            if (cc != null) cc.OnComponentChanging(page, null);

            page.BackColor = SystemColors.Control;
            if (!page.InteriorPage)
            {
                TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
            }
            
            if (cc != null) cc.OnComponentChanged(page, null, null, null);
        }

        internal static void ApplyDefaultWelcomePageStyle(WizardPage page, IDesignerHost dh, IComponentChangeService cc)
        {
            DesignerTransaction dt = null;
            if (dh != null) dt = dh.CreateTransaction();
            
            try
            {
                page.BackColor = Color.White;
                page.Style.BackColorBlend.Clear();
                TypeDescriptor.GetProperties(page)["InteriorPage"].SetValue(page, false);
                TypeDescriptor.GetProperties(page)["BackColor"].SetValue(page, Color.White);
                TypeDescriptor.GetProperties(page)["CanvasColor"].SetValue(page, Color.White);
                TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
                TypeDescriptor.GetProperties(page.Style)["BackColor2"].SetValue(page.Style, Color.Empty);
                TypeDescriptor.GetProperties(page.Style)["BackgroundImage"].SetValue(page.Style, LoadWizardImage(eWizardImages.WelcomeDefault));
                TypeDescriptor.GetProperties(page.Style)["BackgroundImagePosition"].SetValue(page.Style, eStyleBackgroundImage.TopLeft);
            }
            catch
            {
                dt.Cancel();
                throw;
            }
            finally
            {
                if (dt != null && !dt.Canceled)
                    dt.Commit();
            }
        }

        internal static WizardPage CreateWelcomePage(Wizard parent, IDesignerHost dh, IComponentChangeService cc, ISelectionService ss, eWizardStyle style)
        {
            DesignerTransaction dt = dh.CreateTransaction();
            WizardPage page = null;
            try
            {
                page = dh.CreateComponent(typeof(WizardPage)) as WizardPage;

                if(style == eWizardStyle.Default)
                    ApplyDefaultWelcomePageStyle(page, null, null);
                else
                    ApplyOffice2007WelcomePageStyle(page, null, null);
                //TypeDescriptor.GetProperties(page)["InteriorPage"].SetValue(page, false);
                //TypeDescriptor.GetProperties(page)["BackColor"].SetValue(page, Color.White);
                //TypeDescriptor.GetProperties(page)["CanvasColor"].SetValue(page, Color.White);
                //TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
                //TypeDescriptor.GetProperties(page.Style)["BackgroundImage"].SetValue(page.Style, GetWelcomeImage(false));
                //TypeDescriptor.GetProperties(page.Style)["BackgroundImagePosition"].SetValue(page.Style, eStyleBackgroundImage.TopLeft);
                page.Size = new Size(534, 289);

                // Load labels onto the page, first Welcome to the Wizard...
                System.Windows.Forms.Label label = dh.CreateComponent(typeof(System.Windows.Forms.Label)) as System.Windows.Forms.Label;
                page.Controls.Add(label);
                label.Location = new Point(210, 18);
                label.Text = "Welcome to the <Wizard Name> Wizard";
                label.BackColor = Color.Transparent;
                try
                {
                    label.Font = new Font("Tahoma", 16);
                    TypeDescriptor.GetProperties(label)["AutoSize"].SetValue(label, false);
                }
                catch { }
                label.Size = new Size(310, 66);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                // Wizard description label...
                label = dh.CreateComponent(typeof(System.Windows.Forms.Label)) as System.Windows.Forms.Label;
                page.Controls.Add(label);
                label.Location = new Point(210, 100);
                label.Text = "This wizard will guide you through the <Enter Process Name>.\r\n\r\n<Enter brief description of the process wizard is covering.>";
                label.BackColor = Color.Transparent;
                try
                {
                    TypeDescriptor.GetProperties(label)["AutoSize"].SetValue(label, false);
                }
                catch { }
                label.Size = new Size(309, 157);
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                // Click Next to Continue label...
                label = dh.CreateComponent(typeof(System.Windows.Forms.Label)) as System.Windows.Forms.Label;
                page.Controls.Add(label);
                label.Location = new Point(210, 266);
                label.Text = "To continue, click Next.";
                label.Size = new Size(120, 13);
                label.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
                label.BackColor = Color.Transparent;
                
                if (cc != null)
                    cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["WizardPages"]);
                parent.WizardPages.Add(page);
                if (cc != null)
                    cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["WizardPages"], null, null);


                if (ss != null)
                    ss.SetSelectedComponents(new WizardPage[] { page }, SelectionTypes.Replace);

                TypeDescriptor.GetProperties(parent)["SelectedPageIndex"].SetValue(parent, parent.WizardPages.IndexOf(page));
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled)
                    dt.Commit();
            }

            return page;
        }

        internal static WizardPage CreatePage(Wizard parent, bool innerPage, IDesignerHost dh, IComponentChangeService cc, ISelectionService ss, eWizardStyle wizardStyle)
        {
            DesignerTransaction dt = dh.CreateTransaction();
            WizardPage page=null;
            try
            {
                page = dh.CreateComponent(typeof(WizardPage)) as WizardPage;
				
				page.AntiAlias=false;
                page.InteriorPage = innerPage;
                if (innerPage)
                {
                    page.PageTitle = "< Wizard step title >";
                    page.PageDescription = "< Wizard step description >";
                }
                if (wizardStyle == eWizardStyle.Default)
                    ApplyDefaultInnerPageStyle(page, dh, cc);
                else if (wizardStyle == eWizardStyle.Office2007)
                    ApplyOffice2007InnerPageStyle(page, dh, cc);
                //else
                //{
                //    TypeDescriptor.GetProperties(page.Style)["BackColor"].SetValue(page.Style, Color.White);
                //}

                if (cc != null)
                    cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["WizardPages"]);
                parent.WizardPages.Add(page);
                if (cc != null)
                    cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["WizardPages"], null, null);

                if (ss != null)
                    ss.SetSelectedComponents(new WizardPage[] { page }, SelectionTypes.Replace);

                TypeDescriptor.GetProperties(parent)["SelectedPageIndex"].SetValue(parent, parent.WizardPages.IndexOf(page));
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if (!dt.Canceled)
                    dt.Commit();
            }

            return page;
        }

        protected override void OnMouseDragBegin(int x, int y)
        {
            Wizard w = this.Control as Wizard;

            if (w != null)
            {
                Point p = w.NextButtonControl.Parent.PointToClient(new Point(x, y));
                if (w.NextButtonControl.Enabled && w.NextButtonControl.Visible && w.NextButtonControl.Bounds.Contains(p))
                {
                    SelectNextPage(w);
                    return;
                }
                else if (w.BackButtonControl.Enabled && w.BackButtonControl.Visible && w.BackButtonControl.Bounds.Contains(p))
                {
                    SelectPreviousPage(w);
                    return;
                }
				if(m_NavigationControl!=null)
				{
					p = m_NavigationControl.PointToClient(new Point(x, y));
					if(m_NavigationControl.LinkNext.Bounds.Contains(p))
					{
						SelectNextPage(w);
						return;
					}
					else if(m_NavigationControl.LinkBack.Bounds.Contains(p))
					{
						SelectPreviousPage(w);
						return;
					}
				}
            }

            base.OnMouseDragBegin(x, y);
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);

            properties["WizardStyle"] = TypeDescriptor.CreateProperty(
                this.GetType(),
                "WizardStyle",
                typeof(eWizardStyle),
                new Attribute[] { 
                    new BrowsableAttribute(true), 
                    new DesignOnlyAttribute(true),
                    new DefaultValueAttribute(eWizardStyle.Default), 
                    new DescriptionAttribute("Indicates the visual appearance style of the Wizard")});
        }

        private eWizardStyle m_WizardStyle = eWizardStyle.Default;
        public eWizardStyle WizardStyle
        {
            get { return m_WizardStyle; }
            set
            {
                if(m_WizardStyle!=value)
                {
                    m_WizardStyle = value;
                    ((Wizard)this.Control).ButtonStyle = m_WizardStyle;

                    IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if(dh!=null && !dh.Loading)
                        ChangeWizardStyle(m_WizardStyle);
                }
            }
        }

        private void SetDefaultFooterStyle(Wizard w)
        {
            w.FooterStyle.BackColor = System.Drawing.SystemColors.Control;
            w.FooterStyle.BackColorGradientAngle = 90;
            w.FooterStyle.BorderBottomWidth = 1;
            w.FooterStyle.BorderColor = System.Drawing.SystemColors.Control;
            w.FooterStyle.BorderLeftWidth = 1;
            w.FooterStyle.BorderRightWidth = 1;
            w.FooterStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Etched;
            w.FooterStyle.BorderTopColor = System.Drawing.SystemColors.Control;
            w.FooterStyle.BorderTopWidth = 1;
            w.FooterStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            w.FooterStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
        }

        private void ChangeWizardStyle(eWizardStyle style)
        {
            IComponentChangeService cc = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;

            DesignerTransaction dt = dh.CreateTransaction();
            try
            {
                Wizard w = this.Control as Wizard;

                w.FooterStyle.Reset();
                if (style == eWizardStyle.Office2007)
                {
                    w.FooterStyle.BackColor = Color.Transparent;
                    w.BackColor = Color.FromArgb(205, 229, 253);
                    w.ForeColor = ColorScheme.GetColor(0x0F3981);
                    w.BackgroundImage = LoadWizardImage(eWizardImages.BackgroundOffice2007);
                    w.HeaderStyle.Reset();
                    w.HeaderStyle.BackColor = Color.Transparent;// ColorScheme.GetColor(0xBFD7F3);
                    w.HeaderStyle.BackColor2 = Color.Empty; // ColorScheme.GetColor(0xDBF1FE);
                    w.HeaderStyle.BackColorGradientAngle = 90;
                    w.HeaderStyle.BorderBottomColor = ColorScheme.GetColor(0x799DB6);
                    w.HeaderStyle.BorderBottomWidth = 1;
                    w.HeaderStyle.BorderBottom = eStyleBorderType.Solid;
                    w.HeaderHeight = 90;
                    w.HeaderDescriptionVisible = false;
                    w.HeaderImageAlignment = eWizardTitleImageAlignment.Left;
                    w.HeaderCaptionFont = new Font(w.Font.FontFamily, 12, FontStyle.Bold);
                }
                else if (style == eWizardStyle.Default)
                {
                    SetDefaultFooterStyle(w);
                    w.BackgroundImage = null;
                    w.BackColor = SystemColors.Control;
                    w.ForeColor = SystemColors.ControlText;
                    w.HeaderStyle.Reset();
                    w.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                    w.HeaderStyle.BackColorGradientAngle = 90;
                    w.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Etched;
                    w.HeaderStyle.BorderBottomWidth = 1;
                    w.HeaderStyle.BorderColor = System.Drawing.SystemColors.Control;
                    w.HeaderStyle.BorderLeftWidth = 1;
                    w.HeaderStyle.BorderRightWidth = 1;
                    w.HeaderStyle.BorderTopWidth = 1;
                    w.HeaderStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    w.HeaderStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
                    w.HeaderHeight = 60;
                    w.HeaderDescriptionVisible = true;
                    w.HeaderCaptionFont = new Font(w.Font, FontStyle.Bold);
                    w.HeaderImageAlignment = eWizardTitleImageAlignment.Right;
                }

                for (int i = 0; i < w.WizardPages.Count; i++)
                {
                    WizardPage p = w.WizardPages[i];
                    if (!p.InteriorPage && i==0)
                    {
                        if (style == eWizardStyle.Default)
                            ApplyDefaultWelcomePageStyle(p, null, cc);
                        else if (style == eWizardStyle.Office2007)
                            ApplyOffice2007WelcomePageStyle(p, null, cc);
                    }
                    else
                    {
                        if (style == eWizardStyle.Default)
                            ApplyDefaultInnerPageStyle(p, null, cc);
                        else if (style == eWizardStyle.Office2007)
                            ApplyOffice2007InnerPageStyle(p, null, cc);
                    }
                }
            }
            catch
            {
                dt.Cancel();
            }
            finally
            {
                if(!dt.Canceled)
                    dt.Commit();
            }
        }
        #endregion

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
			Wizard w = this.Control as Wizard;
			if (key != "" && w != null && w.LicenseKey == "" && w.LicenseKey != key)
				TypeDescriptor.GetProperties(w)["LicenseKey"].SetValue(w, key);
		}
#endif
        #endregion
    }
}
