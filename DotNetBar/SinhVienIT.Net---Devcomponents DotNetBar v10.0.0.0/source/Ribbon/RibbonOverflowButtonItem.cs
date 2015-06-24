using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents ribbon overflow button.
    /// </summary>
    internal class RibbonOverflowButtonItem : ButtonItem
    {
        /// <summary>
        /// Gets or sets the ribbon bar control overflow button is displayed on.
        /// </summary>
        public RibbonBar RibbonBar = null;

        protected override void OnCommandChanged()
        {
        }

        protected override void OnExternalSizeChange()
        {
            base.OnExternalSizeChange();
            AdjustSubItemsRect();
        }

        public override void RecalcSize()
        {
            base.RecalcSize();
            AdjustSubItemsRect();
        }
        private void AdjustSubItemsRect()
        {
            Rectangle r = this.SubItemsRect;
            if (!r.IsEmpty)
            {
                r.Y += 8;
                r.Height -= 4;
                this.SubItemsRect = r;
            }
        }
    }
}
