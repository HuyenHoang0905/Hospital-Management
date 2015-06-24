using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines the Ribbon Quick Access Overflow system item. Used internally by Ribbon control.
    /// </summary>
    public class QatOverflowItem : DisplayMoreItem
    {
        #region Internal Implementation
        public QatOverflowItem()
            : base()
        {
            this.KeyTips = "00";
        }

        /// <summary>
        /// Returns copy of DisplayMoreItem item
        /// </summary>
        public override BaseItem Copy()
        {
            QatOverflowItem objCopy = new QatOverflowItem();
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
		/// Overriden. Draws the item.
		/// </summary>
		/// <param name="g">Target Graphics object.</param>
        public override void Paint(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                renderer.DrawQatOverflowItem(new QatOverflowItemRendererEventArgs(this, p.Graphics));
                return;
            }
            else
            {
                Rendering.QatOverflowPainter painter = PainterFactory.CreateQatOverflowItemPainter(this);
                if (painter != null)
                {
                    painter.Paint(new QatOverflowItemRendererEventArgs(this, p.Graphics));
                    return;
                }
            }

            base.Paint(p);
        }

        /// <summary>
        /// Returns the insertion index for the items removed from overflow popup. Assumes that right-most items are removed first by the layout manager.
        /// </summary>
        /// <returns></returns>
        protected override int GetReInsertIndex()
        {
            int insertPos = m_Parent.SubItems.Count;
            for (int i = insertPos - 1; i >= 0; i--)
            {
                if (m_Parent.SubItems[i] is CustomizeItem || m_Parent.SubItems[i].ItemAlignment == eItemAlignment.Far)
                {
                    insertPos = i;
                }
                else if (m_Parent.SubItems[i].ItemAlignment == eItemAlignment.Near)
                    break;
            }
            return insertPos;
        }

        #endregion
    }
}
