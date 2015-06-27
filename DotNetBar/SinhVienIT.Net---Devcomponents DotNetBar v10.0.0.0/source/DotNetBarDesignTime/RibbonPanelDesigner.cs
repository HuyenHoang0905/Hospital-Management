using System;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents designer for RibbonPanel control.
    /// </summary>
    public class RibbonPanelDesigner:PanelControlDesigner
    {
        #region Internal Implementation
        public override SelectionRules SelectionRules
		{
			get{return (SelectionRules.Locked | SelectionRules.Visible);}
        }

        protected override void SetDesignTimeDefaults()
        {
            RibbonPanel p = this.Control as RibbonPanel;
            #if FRAMEWORK20
            p.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            #else
            p.DockPadding.Left = 3;
            p.DockPadding.Right = 3;
            p.DockPadding.Bottom = 3;
            #endif
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs=new DesignerVerbCollection(new DesignerVerb[]
				    {
                        new DesignerVerb("Create RibbonBar", new EventHandler(CreateRibbonBar)),
					    new DesignerVerb("Layout Ribbons", new EventHandler(LayoutRibbons))
                    });
                //verbs.AddRange(base.Verbs);
                
                return verbs;
            }
        }

        private void LayoutRibbons(object sender, EventArgs e)
        {
            RibbonPanel panel = this.Control as RibbonPanel;
            if (panel == null)
                return;

            IDesignerHost host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            DesignerTransaction trans=null;
            if (host != null)
                trans=host.CreateTransaction("Rendering Layout");

            try
            {
                panel.LayoutRibbons();
            }
            finally
            {
                if (trans != null)
                    trans.Commit();
            }
        }

        private void CreateRibbonBar(object sender, EventArgs e)
        {
            RibbonPanel panel = this.Control as RibbonPanel;
            if (panel == null)
                return;

            IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (dh != null)
            {
                DesignerTransaction trans = dh.CreateTransaction("Create Default Ribbon Bar");
                IComponentChangeService cc = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                try
                {
                    RibbonBar bar = dh.CreateComponent(typeof(RibbonBar)) as RibbonBar;
                    TypeDescriptor.GetProperties(bar)["Width"].SetValue(bar, 100);
#if !TRIAL
                    string key = RibbonBarDesigner.GetLicenseKey();
                    bar.LicenseKey = key;
#endif
                    TypeDescriptor.GetProperties(bar)["Text"].SetValue(bar, bar.Name);

                    cc.OnComponentChanging(panel, TypeDescriptor.GetProperties(typeof(Control))["Controls"]);
                    panel.Controls.Add(bar);
                    bar.Dock = DockStyle.Left;
                    bar.BringToFront();
                    cc.OnComponentChanged(panel, TypeDescriptor.GetProperties(typeof(Control))["Controls"], null, null);
                }
                catch
                {
                    trans.Cancel();
                    throw;
                }
                finally
                {
                    if (!trans.Canceled)
                        trans.Commit();
                }
            }
        }

        /// <summary>
        /// Draws design-time border around the panel when panel does not have one.
        /// </summary>
        /// <param name="g"></param>
        protected override void DrawBorder(Graphics g)
        {
        }
        #endregion
    }
}
