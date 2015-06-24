#if FRAMEWORK20
using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.Editors
{
    public class VisualItem
    {
        #region Private Variables
        #endregion

        #region Events
        /// <summary>
        /// Occurs when item arrange becomes invalid.
        /// </summary>
        public event EventHandler ArrangeInvalid;

        /// <summary>
        /// Occurs when item appearance becomes invalid and items needs to be repainted.
        /// </summary>
        public event EventHandler RenderInvalid;

        /// <summary>
        /// Occurs when item is clicked.
        /// </summary>
        public event EventHandler Click;
        /// <summary>
        /// Occurs when item is clicked using mouse.
        /// </summary>
        public event MouseEventHandler MouseClick;
        /// <summary>
        /// Occurs when mouse button is pressed over the item.
        /// </summary>
        public event MouseEventHandler MouseDown;
        /// <summary>
        /// Occurs when mouse button is pressed over the item.
        /// </summary>
        public event MouseEventHandler MouseUp;
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation

        internal virtual void ProcessMouseEnter()
        {
            if (this.GetIsEnabled())
                OnMouseEnter();
        }

        protected virtual void OnMouseEnter()
        {

        }

        internal virtual void ProcessMouseLeave()
        {
            if (this.GetIsEnabled())
                OnMouseLeave();
        }

        protected virtual void OnMouseLeave()
        {

        }

        internal virtual void ProcessMouseMove(MouseEventArgs e)
        {
            if (this.GetIsEnabled())
                OnMouseMove(e);
        }

        protected virtual void OnMouseMove(MouseEventArgs e)
        {

        }

        internal virtual void ProcessMouseWheel(MouseEventArgs e)
        {
            if (this.GetIsEnabled())
                OnMouseWheel(e);
        }

        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
        }

        internal virtual void ProcessMouseDown(MouseEventArgs e)
        {
            if (this.GetIsEnabled())
                OnMouseDown(e);
        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        internal virtual void ProcessMouseUp(MouseEventArgs e)
        {
            if (this.GetIsEnabled())
                OnMouseUp(e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }

        internal virtual void ProcessClick()
        {
            if (this.GetIsEnabled())
                OnClick(EventArgs.Empty);
        }

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        internal virtual void ProcessMouseClick(MouseEventArgs e)
        {
            if (this.GetIsEnabled())
                OnMouseClick(e);
        }

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (MouseClick != null)
                MouseClick(this, e);
        }

        internal virtual void ProcessKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        protected virtual void OnKeyDown(KeyEventArgs e)
        {

        }

        internal virtual void ProcessKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {

        }

        internal virtual void ProcessKeyPress(KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {

        }

        internal virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return OnCmdKey(ref msg, keyData);
        }

        protected virtual bool OnCmdKey(ref Message msg, Keys keyData)
        {
            return false;
        }

        internal virtual void ProcessGotFocus()
        {
            OnGotFocus();
        }

        protected virtual void OnGotFocus()
        {
            _IsFocused = true;
            OnFocusChanged();
        }

        internal virtual void ProcessLostFocus()
        {
            OnLostFocus();
        }

        protected virtual void OnLostFocus()
        {
            _IsFocused = false;
            OnFocusChanged();
        }

        protected virtual void OnFocusChanged()
        {

        }

        internal virtual void ProcessPaint(PaintInfo p)
        {
            if (!IsRendered) return;
            OnPaint(p);
        }

        protected virtual void OnPaint(PaintInfo p)
        {
        }

        private bool _Focusable = false;
        public bool Focusable
        {
            get { return _Focusable; }
            set
            {
                if (_Focusable != value)
                {
                    _Focusable = value;
                    OnFocusableChanged();
                }
            }
        }

        protected virtual void OnFocusableChanged()
        {

        }

        private bool _Enabled = true;
        /// <summary>
        /// Gets or sets whether item is Enabled.
        /// </summary>
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                    OnRenderInvalid();
                }
            }
        }

        protected virtual bool GetIsEnabled()
        {
            if (!_Enabled) return _Enabled;
            VisualItem p = this.Parent;
            while (p != null)
            {
                if (!p.Enabled) return false;
                p = p.Parent;
            }

            return _Enabled;
        }

        protected virtual bool GetIsEnabled(PaintInfo p)
        {
            return p.ParentEnabled && _Enabled;
        }

        private bool _Visible = true;
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    OnVisibleChanged();
                }
            }
        }

        protected virtual void OnVisibleChanged()
        {
            if (_Parent != null)
            {
                _Parent.OnChildItemVisibleChanged(this);
            }
        }

        private bool _IsRendered = true;
        /// <summary>
        /// Gets or sets whether visual is rendered. Default value is true.
        /// </summary>
        public virtual bool IsRendered
        {
            get { return _IsRendered; }
            set
            {
                _IsRendered = value;
            }
        }

        public virtual void InvalidateArrange()
        {
            _RenderBounds = Rectangle.Empty;
            _Location = Point.Empty;
            _IsLayoutValid = false;
            if (_Parent != null)
                _Parent.InvalidateArrange();
            OnArrangeInvalid();
        }

        public virtual void InvalidateRender()
        {
            if (_Parent != null)
                _Parent.InvalidateRender();
            OnRenderInvalid();
        }

        /// <summary>
        /// Raises the RenderInvalid event.
        /// </summary>
        protected virtual void OnRenderInvalid()
        {
            if (RenderInvalid != null)
                RenderInvalid(this, new EventArgs());
        }

        /// <summary>
        /// Raises the ArrangeInvalid event.
        /// </summary>
        protected virtual void OnArrangeInvalid()
        {
            if (ArrangeInvalid != null)
                ArrangeInvalid(this, new EventArgs());
        }

        private VisualGroup _Parent;
        /// <summary>
        /// Gets the parent of the item.
        /// </summary>
        public VisualGroup Parent
        {
            get { return _Parent; }
            internal set
            {
                if (_Parent != value)
                {
                    _Parent = value;
                    OnParentChanged();
                }
            }
        }

        protected virtual void OnParentChanged()
        {

        }

        private Size _ItemSize = Size.Empty;
        public Size Size
        {
            get { return _ItemSize; }
            internal set
            {
                _ItemSize = value;
            }
        }

        private Rectangle _RenderBounds = Rectangle.Empty;
        public Rectangle RenderBounds
        {
            get { return _RenderBounds; }
            internal set
            {
                _RenderBounds = value;
            }
        }

        private Point _Location;
        /// <summary>
        /// Gets the relative location of the element inside of its parent item.
        /// </summary>
        public Point Location
        {
            get { return _Location; }
            internal set { _Location = value; }
        }

        private bool _IsRightToLeft;
        public bool IsRightToLeft
        {
            get { return _IsRightToLeft; }
            set
            {
                if (_IsRightToLeft != value)
                {
                    _IsRightToLeft = value;
                    OnIsRightToLeftChanged();
                }
            }
        }

        protected virtual void OnIsRightToLeftChanged()
        {
            InvalidateArrange();
        }

        private bool _IsLayoutValid = false;
        public virtual bool IsLayoutValid
        {
            get { return _IsLayoutValid; }
        }

        public virtual void PerformLayout(PaintInfo pi) { _IsLayoutValid = true; }

        private bool _IsFocused = false;
        public bool IsFocused
        {
            get { return _IsFocused; }
        }

        private eItemAlignment _Alignment = eItemAlignment.Left;
        /// <summary>
        /// Gets or sets the item horizontal alignment inside of the parent group. Default value is left.
        /// </summary>
        [DefaultValue(eItemAlignment.Left)]
        public eItemAlignment Alignment
        {
            get { return _Alignment; }
            set
            {
                if (_Alignment != value)
                {
                    _Alignment = value;
                    this.InvalidateArrange();
                }
            }
        }

        private eSystemItemType _ItemType = eSystemItemType.Default;
        internal eSystemItemType ItemType
        {
            get { return _ItemType; }
            set
            {
                _ItemType = value;
            }
        }
        #endregion

    }
}
#endif

