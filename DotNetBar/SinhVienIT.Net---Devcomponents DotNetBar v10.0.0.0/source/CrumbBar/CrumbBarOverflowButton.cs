using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    public class CrumbBarOverflowButton : ButtonItem
    {
        #region Internal Implementation
        protected override void RenderButton(ItemPaintArgs p)
        {
            if (!p.IsOnMenu)
            {
                Rendering.BaseRenderer renderer = p.Renderer;
                if (renderer != null)
                {
                    p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
                    p.ButtonItemRendererEventArgs.ButtonItem = this;
                    p.ButtonItemRendererEventArgs.ItemPaintArgs = p;
                    renderer.DrawCrumbBarOverflowItem(p.ButtonItemRendererEventArgs);
                    return;
                }
            }
            base.RenderButton(p);
        }

        public override void RecalcSize()
        {
            m_Rect.Width = 16;
            m_Rect.Height = 11;
            m_NeedRecalcSize = false;
        }

        public override void InternalMouseEnter()
        {
            CrumbBarViewContainer parent = this.Parent as CrumbBarViewContainer;
            if (parent != null && parent.Expanded)
            {
                parent.Expanded = false;
                this.Expanded = true;
            }
            base.InternalMouseEnter();
        }

        /// <summary>
		/// Creates new instance of BaseItem.
		/// </summary>
		public CrumbBarOverflowButton():this("","") {}
		/// <summary>
		/// Creates new instance of BaseItem and assigns item name.
		/// </summary>
		/// <param name="sItemName">Item name.</param>
        public CrumbBarOverflowButton(string sItemName) : this(sItemName, "") { }
        /// <summary>
		/// Creates new instance of BaseItem and assigns item name and item text.
		/// </summary>
		/// <param name="itemName">Item Name</param>
		/// <param name="itemText">Item Text</param>
        public CrumbBarOverflowButton(string itemName, string itemText)
            : base(itemName, itemText)
        {
            this.AutoExpandOnClick = true;
            this.ShowSubItems = false;
        }

        /// <summary>
        /// Returns copy of ExplorerBarContainerItem item
        /// </summary>
        public override BaseItem Copy()
        {
            CrumbBarOverflowButton objCopy = new CrumbBarOverflowButton();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        protected override void CopyToItem(BaseItem copy)
        {
            CrumbBarOverflowButton objCopy = copy as CrumbBarOverflowButton;
            base.CopyToItem(objCopy);
        }
        #endregion
    }
}
