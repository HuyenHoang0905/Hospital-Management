#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DevComponents.Editors
{
    public class VisualGroup : VisualItem
    {
        #region Private Variables
        private VisualItem _MouseDownItem = null;
        private VisualItem _MouseOverItem = null;
        #endregion

        #region Events
        #endregion

        #region Constructor
        public VisualGroup()
            : base()
        {
            _Items = new VisualItemCollection(this);
        }
        #endregion

        #region Internal Implementation

        internal virtual void ProcessItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
        {
            OnItemsCollectionChanged(collectionChangedInfo);
        }

        protected virtual void OnItemsCollectionChanged(CollectionChangedInfo collectionChangedInfo)
        {
            if (collectionChangedInfo.ChangeType == eCollectionChangeType.Adding || collectionChangedInfo.ChangeType == eCollectionChangeType.Removing ||
                collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing)
            {
                if (collectionChangedInfo.Removed != null)
                {
                    foreach (VisualItem item in collectionChangedInfo.Removed)
                        item.Parent = null;
                }
                if (collectionChangedInfo.ChangeType == eCollectionChangeType.Clearing)
                {
                    foreach (VisualItem item in this.Items)
                        item.Parent = null;
                }

                if (collectionChangedInfo.Added != null)
                {
                    foreach (VisualItem item in collectionChangedInfo.Added)
                    {
                        if (item.Parent != null && item.Parent != this)
                        {
                            item.Parent.Items.Remove(item);
                        }
                        item.Parent = this;
                    }
                }
            }
            this.InvalidateArrange();
        }

        private VisualItemCollection _Items;
        public virtual VisualItemCollection Items
        {
            get { return _Items; }
        }

        internal void OnChildItemVisibleChanged(VisualItem visualItem)
        {
            InvalidateArrange();
        }

        private VisualItem _FocusedItem;
        public VisualItem FocusedItem
        {
            get { return _FocusedItem; }
            internal set
            {
                if (_FocusedItem == value) return;
                if (_FocusedItem != null)
                    _FocusedItem.ProcessLostFocus();
                VisualItem oldFocus = _FocusedItem;
                _FocusedItem = value;
                if (_FocusedItem != null)
                    _FocusedItem.ProcessGotFocus();
                OnFocusedItemChanged(oldFocus);
                this.InvalidateArrange();
            }
        }

        protected virtual void OnFocusedItemChanged(VisualItem previousFocus)
        {

        }

        protected override void OnPaint(PaintInfo p)
        {
            if (!IsLayoutValid)
                PerformLayout(p);

            Point renderOffset = p.RenderOffset;
            Point pr = p.RenderOffset;
            pr.Offset(this.RenderBounds.Location);
            p.RenderOffset = pr;

            Graphics g = p.Graphics;
            Region oldClip = null;
            bool clipSet = false;
            oldClip = g.Clip;
            Rectangle renderBounds = this.RenderBounds;
            renderBounds.Offset(renderOffset);
            g.SetClip(renderBounds, CombineMode.Intersect);
            clipSet = true;

            bool parentEnabled = p.ParentEnabled;
            p.ParentEnabled = p.ParentEnabled && this.Enabled;
            bool renderSystemItemsOnly = p.RenderSystemItemsOnly;

            foreach (VisualItem v in _Items)
            {
                if (renderSystemItemsOnly && v.ItemType != eSystemItemType.SystemButton) continue;
                if (_VerticalItemAlignment == eVerticalAlignment.Middle)
                    v.RenderBounds = new Rectangle(p.RenderOffset.X + v.Location.X, p.RenderOffset.Y + v.Location.Y + (this.Size.Height - v.Size.Height) / 2, v.Size.Width, v.Size.Height);
                else if (_VerticalItemAlignment == eVerticalAlignment.Bottom)
                    v.RenderBounds = new Rectangle(p.RenderOffset.X + v.Location.X, p.RenderOffset.Y + v.Location.Y + (this.Size.Height - v.Size.Height), v.Size.Width, v.Size.Height);
                else
                    v.RenderBounds = new Rectangle(p.RenderOffset.X + v.Location.X, p.RenderOffset.Y + v.Location.Y, v.Size.Width, v.Size.Height);
                if (v.RenderBounds.IntersectsWith(renderBounds))
                {
                    p.RenderOffset = Point.Empty;
                    v.ProcessPaint(p);
                    p.RenderOffset = pr;
                }
            }
            p.ParentEnabled = parentEnabled;

            if (clipSet)
            {
                if (oldClip != null)
                    g.Clip = oldClip;
                else
                    g.ResetClip();
            }

            if(oldClip!=null) oldClip.Dispose();

            p.RenderOffset = renderOffset;

            base.OnPaint(p);
        }

        protected override void OnIsRightToLeftChanged()
        {
            foreach (VisualItem vi in _Items)
                vi.IsRightToLeft = this.IsRightToLeft;
            base.OnIsRightToLeftChanged();
        }

        public override void PerformLayout(PaintInfo pi)
        {
            Point p = Point.Empty;
            Size size = Size.Empty;
            Size availableSize = pi.AvailableSize;
            Size containerSize = availableSize;
            bool usesFullWidth = false;

            foreach (VisualItem v in new VisualCollectionEnumerator(_Items, this.IsRightToLeft))
            {
                if (v.Visible)
                {
                    v.Location = p;
                    pi.AvailableSize = new Size(availableSize.Width - p.X, availableSize.Height);
                    v.PerformLayout(pi);
                    if (v.Alignment == eItemAlignment.Right)
                    {
                        v.Location = new Point(containerSize.Width - v.Size.Width, p.Y);
                        containerSize.Width -= v.Size.Width + _HorizontalItemSpacing;
                        usesFullWidth = true;
                    }
                    else
                    {
                        p.X += v.Size.Width + _HorizontalItemSpacing;
                        size.Width += v.Size.Width + _HorizontalItemSpacing;
                    }
                    if (v.Size.Height > size.Height)
                        size.Height = v.Size.Height;
                }
            }
            if (size.Width > 0)
                size.Width -= +_HorizontalItemSpacing;

            if (usesFullWidth && _HorizontalItemAlignment != eHorizontalAlignment.Left)
            {
                if (_HorizontalItemAlignment == eHorizontalAlignment.Right)
                {
                    if (containerSize.Width < availableSize.Width) containerSize.Width--;
                    foreach (VisualItem v in new VisualCollectionEnumerator(_Items, !this.IsRightToLeft))
                    {
                        if (!v.Visible || v.Alignment == eItemAlignment.Right || v is LockUpdateCheckBox) continue;
                        v.Location = new Point(containerSize.Width - v.Size.Width, p.Y);
                        containerSize.Width -= v.Size.Width + _HorizontalItemSpacing;
                    }
                }
                else
                {
                    int offset = (containerSize.Width - size.Width) / 2;
                    foreach (VisualItem v in new VisualCollectionEnumerator(_Items, this.IsRightToLeft))
                    {
                        if (!v.Visible || v.Alignment == eItemAlignment.Right || v is LockUpdateCheckBox) continue;
                        v.Location = new Point(v.Location.X + offset, v.Location.Y);
                    }
                }
            }

            if (usesFullWidth)
                size.Width = availableSize.Width;

            pi.AvailableSize = availableSize;
            this.Size = size;
            this.RenderBounds = new Rectangle(pi.RenderOffset, size);
            base.PerformLayout(pi);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            VisualItem item = GetItemAt(e.X, e.Y);
            if (item != null)
            {
                _MouseDownItem = item;
                _MouseOverItem = item;
                item.ProcessMouseDown(e);
                if (_MouseDownItem == item && CanFocus(item))
                    this.FocusedItem = item;
            }
            else if (item == null && this.IsFocused)
            {
                // Find first available input and focus it instead
                foreach (VisualItem visual in _Items)
                {
                    if (visual is VisualInputBase && CanFocus(visual))
                    {
                        this.FocusedItem = visual;
                        break;
                    }
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.FocusedItem != null)
                this.FocusedItem.ProcessMouseWheel(e);
            base.OnMouseWheel(e);
        }

        protected virtual bool CanFocus(VisualItem v)
        {
            return v != null && v.Visible && (v.Focusable || v is VisualInputGroup) && v.Enabled;
        }

        protected override void OnClick(EventArgs e)
        {
            if (_MouseDownItem != null)
            {
                _MouseDownItem.ProcessClick();
            }
            base.OnClick(EventArgs.Empty);
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (_MouseDownItem != null)
            {
                _MouseDownItem.ProcessMouseClick(e);
            }
            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (_MouseDownItem != null)
            {
                _MouseDownItem.ProcessMouseUp(e);
                _MouseDownItem = null;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (_MouseDownItem != null)
                _MouseDownItem.ProcessMouseMove(e);
            else
            {
                VisualItem item = GetItemAt(e.X, e.Y);
                if (item != _MouseOverItem && _MouseOverItem != null)
                {
                    _MouseOverItem.ProcessMouseLeave();
                    _MouseOverItem = null;
                }

                if (item != null && _MouseOverItem == null)
                {
                    _MouseOverItem = item;
                    _MouseOverItem.ProcessMouseEnter();
                }

                if (_MouseOverItem != null)
                    _MouseOverItem.ProcessMouseMove(e);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave()
        {
            if (_MouseDownItem != null)
                _MouseDownItem.ProcessMouseLeave();
            else
            {
                if (_MouseOverItem != null)
                {
                    _MouseOverItem.ProcessMouseLeave();
                    _MouseOverItem = null;
                }
            }
            base.OnMouseLeave();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (_FocusedItem != null)
                _FocusedItem.ProcessKeyDown(e);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            if (_FocusedItem != null)
                _FocusedItem.ProcessKeyUp(e);
            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (_FocusedItem != null)
                _FocusedItem.ProcessKeyPress(e);
            base.OnKeyPress(e);
        }

        protected override bool OnCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (_FocusedItem != null)
                if (_FocusedItem.ProcessCmdKey(ref msg, keyData)) return true;
            foreach (VisualItem item in this.Items)
            {
                if (item is VisualButtonBase)
                {
                    if (item.ProcessCmdKey(ref msg, keyData)) return true;
                }
            }
            return base.OnCmdKey(ref msg, keyData);
        }

        public VisualItem GetItemAt(int x, int y)
        {
            for (int i = _Items.Count - 1; i >= 0 ; i--)
            {
                VisualItem item = _Items[i];
                if (!item.Visible) continue;

                if (item.RenderBounds.Contains(x, y))
                    return item;
            }

            return null;
        }

        private VisualItem GetFirstVisibleItem()
        {
            foreach (VisualItem item in new VisualCollectionEnumerator(_Items, this.IsRightToLeft))
            {
                if (item.Visible) return item;
            }
            return null;
        }

        protected override void OnGotFocus()
        {
            OnGroupFocused();
            base.OnGotFocus();
        }

        protected virtual void OnGroupFocused()
        {
            if (this.FocusedItem == null)
            {
                VisualItem item = GetFirstVisibleItem();
                if (item != null) this.FocusedItem = item;
            }
        }

        protected override void OnLostFocus()
        {
            this.FocusedItem = null;
            base.OnLostFocus();
        }

        internal virtual void ProcessInputComplete()
        {
            OnInputComplete();
        }

        /// <summary>
        /// Occurs when child item input is complete. Method should be used to forward the input focus onto the different field if desired.
        /// </summary>
        protected virtual void OnInputComplete()
        {
        }

        /// <summary>
        /// Occurs when input stack on the child control has changed.
        /// </summary>
        /// <param name="input">Control on which input has changed</param>
        internal void ProcessInputChanged(VisualInputBase input)
        {
            OnInputChanged(input);
        }

        /// <summary>
        /// Occurs when input stack on the child control has changed.
        /// </summary>
        /// <param name="input">Control on which input has changed</param>
        protected virtual void OnInputChanged(VisualInputBase input)
        {

        }

        private int _HorizontalItemSpacing = 0;
        /// <summary>
        /// Gets or sets the horizontal spacing in pixels between the items.
        /// </summary>
        [DefaultValue(0)]
        public int HorizontalItemSpacing
        {
            get { return _HorizontalItemSpacing; }
            set
            {
                if (_HorizontalItemSpacing != value)
                {
                    _HorizontalItemSpacing = value;
                    this.InvalidateArrange();
                }
            }
        }

        private bool _IsRootVisual = false;
        /// <summary>
        /// Gets or sets whether visual is root visual directly parented to the control.
        /// </summary>
        [DefaultValue(false)]
        public bool IsRootVisual
        {
            get { return _IsRootVisual; }
            set { _IsRootVisual = value; }
        }

        private eVerticalAlignment _VerticalItemAlignment = eVerticalAlignment.Middle;
        /// <summary>
        /// Gets or sets group vertical alignment. Default value is middle.
        /// </summary>
        [DefaultValue(eVerticalAlignment.Middle)]
        public eVerticalAlignment VerticalItemAlignment
        {
            get { return _VerticalItemAlignment; }
            set
            {
                if (_VerticalItemAlignment != value)
                {
                    _VerticalItemAlignment = value;
                    OnArrangeInvalid();
                }
            }
        }

        private eHorizontalAlignment _HorizontalItemAlignment = eHorizontalAlignment.Left;
        /// <summary>
        /// Gets or sets input group horizontal alignment. Default value is left.
        /// </summary>
        [DefaultValue(eHorizontalAlignment.Left)]
        public eHorizontalAlignment HorizontalItemAlignment
        {
            get { return _HorizontalItemAlignment; }
            set
            {
                if (_HorizontalItemAlignment != value)
                {
                    _HorizontalItemAlignment = value;
                    OnArrangeInvalid();
                }
            }
        }
        #endregion

    }
}
#endif

