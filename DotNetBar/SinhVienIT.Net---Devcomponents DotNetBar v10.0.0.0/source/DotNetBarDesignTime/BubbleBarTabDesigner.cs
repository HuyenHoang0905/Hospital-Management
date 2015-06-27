using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
    #region BubbleBarTabDesigner
    /// <summary>
    /// Represents designer for BaseItem objects and derived classes.
    /// </summary>
    public class BubbleBarTabDesigner : ComponentDesigner
    {
        /// <summary>
        /// Creates new instance of the class.
        /// </summary>
        public BubbleBarTabDesigner()
        {
        }

        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList components = new ArrayList();
                BubbleBarTab parent = this.Component as BubbleBarTab;
                if (parent == null)
                    return base.AssociatedComponents;
                parent.Buttons.CopyTo(components);
                return components;
            }
        }
    }
    #endregion
}
