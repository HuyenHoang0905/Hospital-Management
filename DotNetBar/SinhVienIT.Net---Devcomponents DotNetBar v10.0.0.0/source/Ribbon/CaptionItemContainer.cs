using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines container for ribbon caption layout and quick access toolbar customization and overflow.
    /// </summary>
    internal class CaptionItemContainer : GenericItemContainer
    {
        #region Private Variables
        private int m_MinTitleSize = 40;
        private int m_MoreItemsLeft = 0;
        private CustomizeItem m_CustomizeItem = null;
        private int m_MaxItemHeight = 0;
        #endregion

        #region Constructor
        public CaptionItemContainer():base()
        {
            this.FirstItemSpacing = 3;
            this.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
        }
        #endregion

        #region Layout
        public override void RecalcSize()
        {
            if (m_MoreItems != null && m_MoreItems.Expanded)
                m_MoreItems.Expanded = false;

            m_RecalculatingSize = true;
            int width = m_MinTitleSize;
            if (this.ContainerControl is Ribbon.QatToolbar)
                width = 0;
            int count = this.SubItems.Count;
            int firstFarIndex = -1;
            int maxHeight = 0;
            int maxItemHeight = 0;
            int moreItemsHeight = 22;
            bool hasQatCustomizeItem = false;
            for (int i = 0; i < count; i++)
            {
                BaseItem item = this.SubItems[i];
                if (!item.Visible)
                    continue;
                item.RecalcSize();
                item.Displayed = true;
                width += (item.WidthInternal + (i==0?this.FirstItemSpacing:m_ItemSpacing));
                
                if (item.ItemAlignment == eItemAlignment.Far && firstFarIndex == -1)
                    firstFarIndex = i;
                if (firstFarIndex == -1)
                    moreItemsHeight = item.HeightInternal;

                if (item.HeightInternal > maxHeight)
                    maxHeight = item.HeightInternal;
                if (item.HeightInternal > maxItemHeight && !(item is Office2007StartButton || item is SystemCaptionItem))
                    maxItemHeight = item.HeightInternal;

                if (item is QatCustomizeItem || item is QatOverflowItem)
                    hasQatCustomizeItem = true;
            }

            if (hasQatCustomizeItem)
                width += maxHeight / 2;

            m_MaxItemHeight = maxItemHeight;
            if (width < this.WidthInternal)
            {
                m_RecalculatingSize = false;
                base.RecalcSize();
                return;
            }
            if (firstFarIndex == -1)
                firstFarIndex = this.SubItems.Count;
            for (int i = firstFarIndex - 1; i >= 0; i--)
            {
                BaseItem item = this.SubItems[i];
                if (!item.Visible)
                    continue;
                width -= item.WidthInternal;
                item.Displayed = false;
                if (width + DisplayMoreItem.FixedSize + maxHeight/2 < this.WidthInternal)
                    break;
            }

            int x = this.LeftInternal + m_PaddingLeft;
            int y = this.TopInternal + m_PaddingTop;
            int loopTo = count;
            if (firstFarIndex >= 0)
                loopTo = firstFarIndex - 1;
            bool oneNearVisible = false;
            for (int i = 0; i < loopTo; i++)
            {
                BaseItem item = this.SubItems[i];
                if (!item.Visible || !item.Displayed)
                    continue;
                oneNearVisible = true;
                Rectangle rb = new Rectangle(x, y, item.WidthInternal, item.HeightInternal);
                if (!(item is Office2007StartButton || item is SystemCaptionItem))
                    rb.Y += (maxItemHeight - item.HeightInternal) / 2;
                item.Bounds = rb;
                x += (item.WidthInternal + (i == 0 ? this.FirstItemSpacing : m_ItemSpacing));
            }
            m_MoreItemsLeft = x + maxHeight/2;

            if (firstFarIndex >= 0)
            {
                x = this.DisplayRectangle.Right - m_PaddingRight;
                for (int i = count - 1; i >= firstFarIndex; i--)
                {
                    BaseItem item = this.SubItems[i];
                    if (!item.Visible || !item.Displayed)
                        continue;
                    x -= item.WidthInternal;
                    item.Bounds = new Rectangle(x, y, item.WidthInternal, item.HeightInternal);
                    x -= m_ItemSpacing;
                }
            }

            m_Rect.Height = maxHeight + m_PaddingTop + m_PaddingBottom;
            if (oneNearVisible)
            {
                // Add Display More item...
                CreateMoreItemsButton(this.IsRightToLeft);

                m_MoreItems.HeightInternal = moreItemsHeight;
                m_MoreItems.RecalcSize();
            }
            else if(m_MoreItems!=null)
            {
                // Clean up, we don't need this anymore
                m_MoreItems.Dispose();
                m_MoreItems = null;
            }

            m_NeedRecalcSize = false;
            m_RecalculatingSize = false;
        }

        protected override int GetItemLayoutWidth(BaseItem objItem)
        {
            if (objItem is QatCustomizeItem || objItem is QatOverflowItem)
                return objItem.WidthInternal + this.HeightInternal / 2;
            return base.GetItemLayoutWidth(objItem);
        }

        protected override int GetItemLayoutX(BaseItem objItem, int iX)
        {
            if (objItem is QatCustomizeItem || objItem is QatOverflowItem)
                return iX + this.HeightInternal / 2;
            return base.GetItemLayoutX(objItem, iX);
        }

        protected override int GetItemLayoutY(BaseItem objItem, int iY)
        {
            if (!(objItem is Office2007StartButton || objItem is SystemCaptionItem))
                return iY + (m_MaxItemHeight - objItem.HeightInternal) / 2;
            return base.GetItemLayoutY(objItem, iY);
        }

        internal int MaxItemHeight
        {
            get
            {
                return m_MaxItemHeight;
            }
        }

        protected override Point GetMoreItemsLocation(bool isRightToLeft)
        {
            if (m_MoreItems == null)
                return Point.Empty;
            Point p = Point.Empty;
            if (m_Orientation == eOrientation.Vertical)
            {
                p.X = m_Rect.Left + m_PaddingLeft;
                p.Y = m_MoreItemsLeft;
            }
            else
            {
                if (isRightToLeft)
                    p.X = m_Rect.X + m_PaddingLeft;
                else
                    p.X = m_MoreItemsLeft;
                p.Y = m_Rect.Top + m_PaddingTop;
            }

            return p;
        }

        protected override void CreateMoreItemsButton(bool isRightToLeft)
        {
            if (m_MoreItems == null)
            {
                m_MoreItems = new QatOverflowItem();
                m_MoreItems.Style = m_Style;
                m_MoreItems.SetParent(this);
                m_MoreItems.ThemeAware = this.ThemeAware;
            }
            if (this.MoreItemsOnMenu)
                m_MoreItems.PopupType = ePopupType.Menu;
            else
                m_MoreItems.PopupType = ePopupType.ToolBar;
            m_MoreItems.Orientation = m_Orientation;
            m_MoreItems.Displayed = true;

            if (m_Orientation == eOrientation.Vertical)
            {
                m_MoreItems.WidthInternal = m_Rect.Width - (m_PaddingLeft + m_PaddingRight);
                m_MoreItems.RecalcSize();
            }
            else
            {
                m_MoreItems.HeightInternal = m_Rect.Height - (m_PaddingTop + m_PaddingBottom);
                m_MoreItems.RecalcSize();
            }

            Point loc = GetMoreItemsLocation(isRightToLeft);
            m_MoreItems.LeftInternal = loc.X;
            m_MoreItems.TopInternal = loc.Y;
        }
        #endregion

        #region Internal Implementation
        protected internal override void OnItemAdded(BaseItem objItem)
        {
            base.OnItemAdded(objItem);

            BaseItem item = objItem;
            if (item is CustomizeItem)
                m_CustomizeItem = (CustomizeItem)item;
            else if (m_CustomizeItem != null)
            {
                bool added = false;
                if(this.SubItems.Contains(m_CustomizeItem))
                    this.SubItems._Remove(m_CustomizeItem);
                for (int i = this.SubItems.Count - 1; i > 0; i--)
                {
                    if (this.SubItems[i].ItemAlignment == eItemAlignment.Near && !(this.SubItems[i] is SystemCaptionItem))
                    {
                        this.SubItems._Add(m_CustomizeItem, i+1);
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    if (this.SubItems.Count > 0 && this.SubItems[this.SubItems.Count - 1] is SystemCaptionItem)
                        this.SubItems._Add(m_CustomizeItem, this.SubItems.Count - 2);
                    else
                        this.SubItems._Add(m_CustomizeItem);
                }
            }
        }

        protected internal override void OnAfterItemRemoved(BaseItem objItem)
        {
            if (objItem == m_CustomizeItem)
                m_CustomizeItem = null;
            base.OnAfterItemRemoved(objItem);
        }
        #endregion
    }
}
