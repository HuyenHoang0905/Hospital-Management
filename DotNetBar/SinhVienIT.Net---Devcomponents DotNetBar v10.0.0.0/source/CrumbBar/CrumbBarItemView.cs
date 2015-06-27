using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents a view of CrumbBarItem displayed inside of CrumbBar control.
    /// </summary>
    public class CrumbBarItemView : ButtonItem
    {
        #region Internal Implementation
        /// <summary>
        /// Initializes a new instance of the CrumbBarItemView class.
        /// </summary>
        public CrumbBarItemView():base()
        {
            this.SubItemsExpandWidth = 15;
            this.HorizontalPadding = 2;
        }

        private CrumbBarItem _AttachedItem;
        /// <summary>
        /// Gets the item attached to the view.
        /// </summary>
        public CrumbBarItem AttachedItem
        {
            get { return _AttachedItem; }
            internal set
            {
                if (_AttachedItem != null)
                {
                    _AttachedItem.TextChanged -= new EventHandler(AttachedItemTextChanged);
                    _AttachedItem.SubItemsChanged -= new System.ComponentModel.CollectionChangeEventHandler(AttachedItemSubItemsChanged);
                    _AttachedItem.PopupClose -= new EventHandler(AttachedItemPopupClose);
                }
                _AttachedItem = value;

                if (_AttachedItem != null)
                {
                    this.Text = _AttachedItem.Text;
                    this.Cursor = _AttachedItem.Cursor;
                    this.ForeColor = _AttachedItem.ForeColor;
                    this.HotForeColor = _AttachedItem.HotForeColor;
                    this.Tooltip = _AttachedItem.Tooltip;

                    _AttachedItem.TextChanged += new EventHandler(AttachedItemTextChanged);
                    _AttachedItem.SubItemsChanged += new System.ComponentModel.CollectionChangeEventHandler(AttachedItemSubItemsChanged);
                    _AttachedItem.PopupClose += new EventHandler(AttachedItemPopupClose);
                    if (AnyVisibleItems)
                        this.PopupType = ePopupType.Container;
                    else
                        this.PopupType = ePopupType.Menu;
                }
                else
                    this.PopupType = ePopupType.Menu;
            }
        }

        private DateTime _PopupCloseTime = DateTime.MinValue;
        private void AttachedItemPopupClose(object sender, EventArgs e)
        {
            _Expanded = false;
            CrumbBarViewContainer c = this.Parent as CrumbBarViewContainer;
            if (c != null)
                c.OnSubItemExpandChange(this);

            _PopupCloseTime = DateTime.Now;
            this.Refresh();
        }

        public override void InternalMouseDown(MouseEventArgs objArg)
        {
            if (_PopupCloseTime != DateTime.MinValue && DateTime.Now.Subtract(_PopupCloseTime).TotalMilliseconds < 200)
            {
                _PopupCloseTime = DateTime.MinValue;
                return;
            }
            base.InternalMouseDown(objArg);
        }

        public override void InternalMouseEnter()
        {
            CrumbBarViewContainer parent = this.Parent as CrumbBarViewContainer;
            if (parent != null && parent.Expanded && AnyVisibleItems)
            {
                parent.Expanded = false;
                this.Expanded = true;
            }
            base.InternalMouseEnter();
        }

        private bool AnyVisibleItems
        {
            get
            {
                if (_AttachedItem != null)
                {
                    foreach (BaseItem item in _AttachedItem.SubItems)
                    {
                        if (item.Visible) return true;
                    }
                }
                return false;
            }
        }

        private void AttachedItemSubItemsChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            UpdatePopupType();
        }

        private void UpdatePopupType()
        {
            if (AnyVisibleItems && !(this.Parent is CrumbBarOverflowButton))
                this.PopupType = ePopupType.Container;
            else
                this.PopupType = ePopupType.Menu;
        }

        protected override void OnParentChanged()
        {
            UpdatePopupType();
            base.OnParentChanged();
        }

        void AttachedItemTextChanged(object sender, EventArgs e)
        {
            this.Text = _AttachedItem.Text;
        }

        protected override void Dispose(bool disposing)
        {
            this.AttachedItem = null;
            base.Dispose(disposing);
        }

        internal static object CreateViewForItem(CrumbBarItem item)
        {
            CrumbBarItemView view = new CrumbBarItemView();
            view.AttachedItem = item;
            return view;
        }

        private bool _Expanded = false;
        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded or not. For Popup items this would indicate whether the item is popped up or not.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public override bool Expanded
        {
            get
            {
                return _Expanded;
            }
            set
            {
                if (_Expanded != value)
                {
                    _Expanded = value;
                    InternalExpandedChanged();
                }
            }
        }

        private void InternalExpandedChanged()
        {
            if (!_Expanded)
            {
                _AttachedItem.Expanded = false;
            }
            else
            {
                Control parent=this.ContainerControl as Control;
                if(parent==null) return;
                Point p = new Point(this.SubItemsRect.X - 10, this.SubItemsRect.Bottom - 1);
                p.Offset(this.DisplayRectangle.Location.X, this.DisplayRectangle.Location.Y);
                p = parent.PointToScreen(p);
                _AttachedItem.PopupMenu(p);
            }

            CrumbBarViewContainer c = this.Parent as CrumbBarViewContainer;
            if (c != null)
                c.OnSubItemExpandChange(this);
        }

        protected override void OnClick()
        {
            CrumbBar bar = this.ContainerControl as CrumbBar;
            if (bar == null) bar = this.GetOwner() as CrumbBar;

            base.OnClick();

            if (bar != null)
            {
                bar.SelectedItem = _AttachedItem;
            }
        }

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
                    renderer.DrawCrumbBarItemView(p.ButtonItemRendererEventArgs);
                    return;
                }
            }
            base.RenderButton(p);
        }

        /// <summary>
        /// Returns copy of ExplorerBarContainerItem item
        /// </summary>
        public override BaseItem Copy()
        {
            CrumbBarItemView objCopy = new CrumbBarItemView();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        protected override void CopyToItem(BaseItem copy)
        {
            CrumbBarItemView objCopy = copy as CrumbBarItemView;
            base.CopyToItem(objCopy);
        }
        #endregion

    }
}
