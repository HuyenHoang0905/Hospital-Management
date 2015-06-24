using System;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents Windows Forms Designer for WizardPage.
    /// </summary>
    public class WizardPageDesigner : ScrollableControlDesigner
    {
        #region Internal Implementation
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = null;
                verbs = new DesignerVerb[]
					{
						new DesignerVerb("Create Inner Page", new EventHandler(CreateInnerPage)),
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

        public override SelectionRules SelectionRules
		{
			get{return SelectionRules.Locked;}
        }

        private Wizard GetWizard()
        {
            WizardPage page = this.Control as WizardPage;
            return page.Parent as Wizard;
        }

        private void GotoPage(object sender, EventArgs e)
        {
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;
            WizardDesigner.GotoPage(this.GetWizard(), cc, ss);
        }


        private void CreateInnerPage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            Wizard w = GetWizard();
            WizardDesigner.CreatePage(w, true, dh, cc, ss, w.ButtonStyle);
        }

        private void CreateWelcomePage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            Wizard w = GetWizard();
            WizardDesigner.CreateWelcomePage(w, dh, cc, ss, w.ButtonStyle);
        }

        private void CreateOuterPage(object sender, EventArgs e)
        {
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            ISelectionService ss = this.GetService(typeof(ISelectionService)) as ISelectionService;

            Wizard w = GetWizard();
            WizardDesigner.CreatePage(w, false, dh, cc, ss, w.ButtonStyle);
        }

        private void DeletePage(object sender, EventArgs e)
        {
            WizardPage page = this.Control as WizardPage;
            IDesignerHost dh = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;

            WizardDesigner.DeletePage(page, dh, cc);
        }

        private void NextPage(object sender, EventArgs e)
        {
            WizardDesigner.SelectNextPage(GetWizard());
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            WizardDesigner.SelectPreviousPage(GetWizard());
        }
        #endregion
    }
}
