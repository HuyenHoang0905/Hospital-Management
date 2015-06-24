using System.Collections;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    public class SuperTabItemDesigner : BaseItemDesigner
    {
        #region GetTabItem

        protected virtual SuperTabItem GetTabItem()
        {
            return (Component as SuperTabItem);
        }

        #endregion

        #region Verbs

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[] { };

                return (new DesignerVerbCollection(verbs));
            }
        }

        #endregion

        #region AssociatedComponents

        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList c = new ArrayList(base.AssociatedComponents);

                SuperTabItem tab = GetTabItem();

                if (tab != null)
                {
                    if (tab.AttachedControl != null)
                        c.Add(tab.AttachedControl);
                }

                return (c);
            }
        }

        #endregion

        #region ComponentChangeComponentAdding

        protected override void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
        {
        }

        #endregion
    }
}
