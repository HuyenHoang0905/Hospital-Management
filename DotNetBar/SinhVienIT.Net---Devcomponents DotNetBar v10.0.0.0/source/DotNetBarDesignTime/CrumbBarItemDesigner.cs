using System;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    public class CrumbBarItemDesigner : ComponentDesigner
    {
        #region Internal Implementation
        public override System.Collections.ICollection AssociatedComponents
        {
            get
            {
                ArrayList c = new ArrayList(base.AssociatedComponents);
                CrumbBarItem item = this.Component as CrumbBarItem;
                if (item != null)
                {
                    foreach (BaseItem node in item.SubItems)
                        GetItemsRecursive(node, c);
                }
                return c;
            }
        }
        private void GetItemsRecursive(BaseItem parent, ArrayList c)
        {
            c.Add(parent);
            foreach (BaseItem node in parent.SubItems)
            {
                c.Add(node);
                GetItemsRecursive(node, c);
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
            CrumbBarItem item = this.Component as CrumbBarItem;
            item.Text = item.Name;
        }

        #endregion
    }
}
