using System;
using System.Text;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents default designer for RatingItem
    /// </summary>
    public class RatingItemDesigner : BaseItemDesigner
    {
        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection();
            }
        }
    }
}
