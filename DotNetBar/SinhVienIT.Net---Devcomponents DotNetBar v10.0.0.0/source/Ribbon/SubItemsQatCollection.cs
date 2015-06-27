using System;
using System.Text;

namespace DevComponents.DotNetBar.Ribbon
{
    internal class SubItemsQatCollection : SubItemsCollection
    {
        private Ribbon.QatToolbar m_QatToolbar = null;
        public SubItemsQatCollection(Ribbon.QatToolbar qatToolbar):base(null)
        {
            m_QatToolbar = qatToolbar;
        }

        protected override void OnInsert(int index, object value) {}

        protected override void OnInsertComplete(int index, object value)
        {
            if (!m_IgnoreEvents)
            {
                if(m_QatToolbar.Items.Count<=index)
                    m_QatToolbar.Items.Add(value as BaseItem);
                else
                    m_QatToolbar.Items.Insert(index, value as BaseItem);
            }
        }

        protected override void RemoveInternal(int index, object value)
        {
            // Raise event before item is actualy removed so the item is able to clean its state
            // See override in PopupItem
            if (!m_IgnoreEvents)
            {
                m_QatToolbar.Items.Remove(value as BaseItem);
            }
        }

        protected override void RemoveCompleteInternal(int index, object value)
        {
        }
    }
}
