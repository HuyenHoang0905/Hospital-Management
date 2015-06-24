using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents internal CrumbBar view container.
    /// </summary>
    internal class CrumbBarViewContainer : ImageItem
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the CrumbBarViewContainer class.
        /// </summary>
        public CrumbBarViewContainer()
            : base()
        {
            LabelItem imageLabel = new LabelItem("sys_crumbbarimagelabel");
            this.SubItems.Add(imageLabel);

            CrumbBarOverflowButton overflowButton = new CrumbBarOverflowButton("sys_crumbbaroverflowbutton");
            this.SubItems.Add(overflowButton);
            m_IsContainer = true;
        }

        /// <summary>
        /// Recalculates the size of the item
        /// </summary>
        public override void RecalcSize()
        {
            if (_InternalRemove) return;

            Point pos = m_Rect.Location;
            Size containerSize = m_Rect.Size;
            _CalculatedHeight = 0;

            if (m_SubItems == null)
                return;

            RestoreOverflowItems();
            ButtonItem overflowButton = this.OverflowButton;
            LabelItem imageLabel = this.ImageLabel;

            // Overflow button is hidden by default
            overflowButton.Visible = false;

            Rectangle itemsRect = m_Rect;

            // Label may be hidden if there is no image assigned to current item
            if (imageLabel.Visible)
            {
                imageLabel.RecalcSize();
                if (imageLabel.HeightInternal > _CalculatedHeight) _CalculatedHeight = imageLabel.HeightInternal;
                imageLabel.HeightInternal = containerSize.Height;
                itemsRect.X += imageLabel.WidthInternal;
                itemsRect.Width -= imageLabel.WidthInternal;
                imageLabel.Displayed = true;
            }
            Point itemsPos = itemsRect.Location;

            bool overflowState = false;
            BaseItem[] items = new BaseItem[m_SubItems.Count - 2];
            if(m_SubItems.Count>2)
                m_SubItems.CopyToFromIndex(items, 2);

            for (int i = items.Length - 1; i >= 0; i--)
            {
                BaseItem item = items[i];
                if (!item.Visible)
                {
                    item.Displayed = false;
                    continue;
                }

                if (!overflowState)
                {
                    item.LeftInternal = itemsPos.X;
                    item.TopInternal = itemsPos.Y;
                    item.HeightInternal = itemsRect.Height;
                    item.RecalcSize();
                    if (item.HeightInternal > _CalculatedHeight) _CalculatedHeight = item.HeightInternal;
                    item.HeightInternal = itemsRect.Height;
                    if (itemsPos.X + item.WidthInternal > itemsRect.Right || itemsPos.X + item.WidthInternal + 16 > itemsRect.Right && i > 0)
                    {
                        // Overflow mode
                        overflowState = true;
                    }
                    else
                    {
                        itemsPos.X += item.WidthInternal;
                        item.Displayed = true;
                    }
                }

                if (overflowState)
                {
                    _InternalRemove = true;
                    m_SubItems.Remove(item);
                    overflowButton.SubItems.Insert(0, item);
                    _InternalRemove = false;
                }
            }

            // Now position the items left inside
            if (overflowState)
            {
                overflowButton.Visible = true;
                overflowButton.Displayed = true;
                overflowButton.HeightInternal = itemsRect.Height;
                overflowButton.RecalcSize();
                overflowButton.LeftInternal = itemsRect.X;
                overflowButton.TopInternal = itemsRect.Y;
                overflowButton.HeightInternal = itemsRect.Height;
                itemsRect.X += overflowButton.WidthInternal;
                itemsRect.Width -= overflowButton.WidthInternal;
            }

            itemsPos = itemsRect.Location;
            for (int i = 2; i < m_SubItems.Count; i++)
            {
                BaseItem item = m_SubItems[i];
                if (!item.Visible)
                    continue;
                if (item.WidthInternal + itemsPos.X > itemsRect.Right)
                {
                    item.Displayed = false;
                    itemsPos.X = itemsRect.Right;
                    continue;
                }

                item.LeftInternal = itemsPos.X;
                itemsPos.X += item.WidthInternal;
            }

            base.RecalcSize();
        }

        private int _CalculatedHeight = 0;
        internal int CalculatedHeight
        {
            get { return _CalculatedHeight; }
        }

        internal void RestoreOverflowItems()
        {
            ButtonItem overflowButton = this.OverflowButton;
            if (overflowButton.SubItems.Count == 0) return;

            BaseItem[] overflowItems = new BaseItem[overflowButton.SubItems.Count];
            overflowButton.SubItems.CopyTo(overflowItems, 0);
            overflowButton.SubItems.Clear();

            for (int i = 0; i < overflowItems.Length; i++)
            {
                m_SubItems.Insert(i + 2, overflowItems[i]);
            }
        }

        internal LabelItem ImageLabel
        {
            get
            {
                return m_SubItems[0] as LabelItem;
            }
        }

        internal CrumbBarOverflowButton OverflowButton
        {
            get
            {
                return (CrumbBarOverflowButton)m_SubItems[1];
            }
        }

        private bool _InternalRemove = false;
        internal void ClearViewItems()
        {
            _InternalRemove = true;
            while (m_SubItems.Count > 2)
                m_SubItems.RemoveAt(m_SubItems.Count - 1);
            _InternalRemove = false;
        }

        /// <summary>
        /// Paints this base container
        /// </summary>
        public override void Paint(ItemPaintArgs pa)
        {
            if (this.SuspendLayout)
                return;
            System.Drawing.Graphics g = pa.Graphics;
            if (m_NeedRecalcSize)
                RecalcSize();

            if (m_SubItems == null)
                return;

            foreach (BaseItem item in m_SubItems)
            {
                if (item.Visible && item.Displayed)
                {
                    item.Paint(pa);
                }
            }
        }

        /// <summary>
        /// Returns copy of ExplorerBarContainerItem item
        /// </summary>
        public override BaseItem Copy()
        {
            CrumbBarViewContainer objCopy = new CrumbBarViewContainer();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        protected override void CopyToItem(BaseItem copy)
        {
            CrumbBarViewContainer objCopy = copy as CrumbBarViewContainer;
            base.CopyToItem(objCopy);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override bool Expanded
        {
            get
            {
                return m_Expanded;
            }
            set
            {
                base.Expanded = value;
                if (!value)
                    BaseItem.CollapseSubItems(this);
            }
        }

        /// <summary>
        /// Occurs when sub item expanded state has changed.
        /// </summary>
        /// <param name="item">Sub item affected.</param>
        protected internal override void OnSubItemExpandChange(BaseItem item)
        {
            base.OnSubItemExpandChange(item);
            if (item.Expanded)
                this.Expanded = true;
            else
                base.Expanded = false;
        }
        #endregion
    }
}
