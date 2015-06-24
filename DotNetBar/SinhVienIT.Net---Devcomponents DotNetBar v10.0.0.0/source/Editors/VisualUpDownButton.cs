#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    /// <summary>
    /// Represents the up down button which allows change of the value in currently focused input control.
    /// </summary>
    public class VisualUpDownButton : VisualButtonBase
    {
        #region Private Variables
        private bool _MouseOverButtonUp = false;
        private bool _MouseOverButtonDown = false;
        private bool _MouseDownButtonUp = false;
        private bool _MouseDownButtonDown = false;
        private Rectangle _DownButtonRectangle = Rectangle.Empty;
        private Rectangle _UpButtonRectangle = Rectangle.Empty;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Up part of the button has been clicked.
        /// </summary>
        public event EventHandler UpClick;
        /// <summary>
        /// Occurs when Down part of the button has been clicked.
        /// </summary>
        public event EventHandler DownClick;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the VisualUpDownButton class.
        /// </summary>
        public VisualUpDownButton()
        {
            this.Focusable = false;
            this.ClickAutoRepeat = true;
        }
        #endregion

        #region Internal Implementation
        public override void PerformLayout(PaintInfo pi)
        {
            int height = pi.AvailableSize.Height;
            if (height % 2 != 0)
                height++;
            Size size = new Size(_ButtonWidth, height);

            if (_UpImage != null && _DownImage != null)
                size.Width = Math.Max(_UpImage.Width, _DownImage.Width);

            this.Size = size;
            base.PerformLayout(pi);
        }

        protected override void OnPaint(PaintInfo p)
        {
            Graphics g = p.Graphics;
            Rectangle r = this.RenderBounds;
            if (r.Width <= 0 || r.Height <= 0)
                return;

            Rectangle buttonRect = new Rectangle(r.X, r.Y, r.Width, r.Height / 2);
            if (_UpImage != null)
                g.DrawImage(_UpImage, buttonRect.Location);
            else
            {
                Office2007ButtonItemStateColorTable ct = GetOffice2007StateColorTableButtonUp(p);
                PaintButtonBackground(p, ct, buttonRect);
                using (SolidBrush brush = new SolidBrush(ct.Text))
                    p.Graphics.FillPolygon(brush, Office2007ButtonItemPainter.GetExpandPolygon(buttonRect, ePopupSide.Top));
            }
            _UpButtonRectangle = buttonRect;


            buttonRect = new Rectangle(r.X, buttonRect.Bottom, r.Width, r.Height - buttonRect.Height);
            if (_DownImage != null)
                g.DrawImage(_DownImage, buttonRect.Location);
            else
            {
                Office2007ButtonItemStateColorTable ct = GetOffice2007StateColorTableButtonDown(p);
                PaintButtonBackground(p, ct, buttonRect);
                using (SolidBrush brush = new SolidBrush(ct.Text))
                    p.Graphics.FillPolygon(brush, Office2007ButtonItemPainter.GetExpandPolygon(buttonRect, ePopupSide.Bottom));
            }
            _DownButtonRectangle = buttonRect;

            base.OnPaint(p);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.GetIsEnabled())
            {
                if (_DownButtonRectangle.Contains(e.X, e.Y))
                {
                    this.MouseOverButtonDown = true;
                    this.MouseOverButtonUp = false;
                }
                else if (_UpButtonRectangle.Contains(e.X, e.Y))
                {
                    this.MouseOverButtonUp = true;
                    this.MouseOverButtonDown = false;
                }
                else
                {
                    this.MouseOverButtonUp = false;
                    this.MouseOverButtonDown = false;
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.GetIsEnabled())
            {
                if (this.MouseOverButtonUp)
                    this.MouseDownButtonUp = true;
                else if (this.MouseOverButtonDown)
                    this.MouseDownButtonDown = true;

                ExecuteClickAction();
            }
            base.OnMouseDown(e);
        }

        private void ExecuteClickAction()
        {
            if (this.MouseOverButtonUp)
            {
                if (_AutoChange != eUpDownButtonAutoChange.None || _AutoChangeItem != null)
                    UpClickFocusedInputValue();
                OnUpClick(new EventArgs());
            }
            else if (this.MouseOverButtonDown)
            {
                if (_AutoChange != eUpDownButtonAutoChange.None || _AutoChangeItem != null)
                    DownClickFocusedInputValue();
                OnDownClick(new EventArgs());
            }
        }

        protected override void OnClickAutoRepeat()
        {
            ExecuteClickAction();
            base.OnClickAutoRepeat();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            this.MouseDownButtonDown = false;
            this.MouseDownButtonUp = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave()
        {
            this.MouseOverButtonDown = false;
            this.MouseOverButtonUp = false;
            base.OnMouseLeave();
        }

        private void DownClickFocusedInputValue()
        {
            VisualItem focusedItem = GetAutoChangeItem();
            if (focusedItem == null) return;

            VisualInputGroup parent = null;
            if (focusedItem.Parent is VisualInputGroup && !((VisualInputGroup)this.Parent).IsUserInput)
            {
                parent = (VisualInputGroup)this.Parent;
                parent.IsUserInput = true;
            }

            if (focusedItem is VisualNumericInput)
                ((VisualNumericInput)focusedItem).DecreaseValue();
            else if (focusedItem is VisualListInput)
                ((VisualListInput)focusedItem).SelectNext();

            if (parent != null)
                parent.IsUserInput = false;
        }

        private void UpClickFocusedInputValue()
        {
            VisualItem focusedItem = GetAutoChangeItem();
            if (focusedItem == null) return;

            VisualInputGroup parent=null;
            if (focusedItem.Parent is VisualInputGroup && !((VisualInputGroup)this.Parent).IsUserInput)
            {
                parent = (VisualInputGroup)this.Parent;
                parent.IsUserInput = true;
            }

            if (focusedItem is VisualNumericInput)
                ((VisualNumericInput)focusedItem).IncreaseValue();
            else if (focusedItem is VisualListInput)
                ((VisualListInput)focusedItem).SelectPrevious();

            if (parent != null)
                parent.IsUserInput = false;
        }

        private VisualItem GetAutoChangeItem()
        {
            if (_AutoChangeItem != null) return _AutoChangeItem;

            if (_AutoChange == eUpDownButtonAutoChange.None || this.Parent == null || !(this.Parent is VisualGroup))
                return null;
            VisualGroup group = this.Parent as VisualGroup;
            if (_AutoChange == eUpDownButtonAutoChange.FocusedItem)
            {
                if (group.FocusedItem is VisualInputGroup)
                {
                    VisualInputGroup g = group.FocusedItem as VisualInputGroup;
                    while (g.FocusedItem is VisualInputGroup)
                        g = g.FocusedItem as VisualInputGroup;
                    return g.FocusedItem;
                }
                if (!(group.FocusedItem is VisualInputBase))
                {
                    foreach (VisualItem item in group.Items)
                    {
                        if (item is VisualInputBase && item.Enabled && item.Visible && item.Focusable)
                            return item;
                    }
                }

                return group.FocusedItem;
            }
            int start = group.Items.IndexOf(this);
            for (int i = start; i >= 0; i--)
            {
                VisualItem item = group.Items[i];
                if (item is VisualInputBase && item.Visible)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Raises the UpClick event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnUpClick(EventArgs e)
        {
            if (UpClick != null)
                UpClick(this, e);
        }

        /// <summary>
        /// Raises the DownClick event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnDownClick(EventArgs e)
        {
            if (DownClick != null)
                DownClick(this, e);
        }

        private Image _UpImage = null;
        /// <summary>
        /// Gets or sets the image displayed on the face of the button.
        /// </summary>
        [DefaultValue(null)]
        public Image UpImage
        {
            get { return _UpImage; }
            set
            {
                if (_UpImage != value)
                {
                    _UpImage = value;
                    this.InvalidateArrange();
                }
            }
        }

        private Image _DownImage = null;
        /// <summary>
        /// Gets or sets the image displayed on the face of the button.
        /// </summary>
        [DefaultValue(null)]
        public Image DownImage
        {
            get { return _DownImage; }
            set
            {
                if (_DownImage != value)
                {
                    _DownImage = value;
                    this.InvalidateArrange();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MouseOverButtonUp
        {
            get { return _MouseOverButtonUp; }
            set
            {
                if (_MouseOverButtonUp != value)
                {
                    _MouseOverButtonUp = value;
                    this.InvalidateRender();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MouseOverButtonDown
        {
            get { return _MouseOverButtonDown; }
            set
            {
                if (_MouseOverButtonDown != value)
                {
                    _MouseOverButtonDown = value;
                    this.InvalidateRender();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MouseDownButtonDown
        {
            get { return _MouseDownButtonDown; }
            set
            {
                if (_MouseDownButtonDown != value)
                {
                    _MouseDownButtonDown = value;
                    this.InvalidateRender();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MouseDownButtonUp
        {
            get { return _MouseDownButtonUp; }
            set
            {
                if (_MouseDownButtonUp != value)
                {
                    _MouseDownButtonUp = value;
                    this.InvalidateRender();
                }
            }
        }

        private int _ButtonWidth = 15;
        /// <summary>
        /// Gets or sets the default width of the buttons.
        /// </summary>
        [DefaultValue(15)]
        public int ButtonWidth
        {
            get { return _ButtonWidth; }
            set
            {
                if (_ButtonWidth != value)
                {
                    _ButtonWidth = value;
                    this.InvalidateArrange();
                }
            }
        }

        private eUpDownButtonAutoChange _AutoChange = eUpDownButtonAutoChange.None;
        /// <summary>
        /// Gets or sets whether control automatically tries to increase/decrease the value of the item that has input focus in the same
        /// parent group as the button. Default value is false.
        /// </summary>
        [DefaultValue(eUpDownButtonAutoChange.None)]
        public eUpDownButtonAutoChange AutoChange
        {
            get { return _AutoChange; }
            set
            {
                _AutoChange = value;
            }
        }

        private VisualItem _AutoChangeItem;
        /// <summary>
        /// Gets or sets the item that is automatically changed when buttons are pressed.
        /// </summary>
        [DefaultValue(null)]
        public VisualItem AutoChangeItem
        {
            get { return _AutoChangeItem; }
            set { _AutoChangeItem = value; }
        }

        private bool RenderBackground(PaintInfo p)
        {
            if (RenderDefaultBackground) return true;

            if (!p.MouseOver && !(this.MouseDownButtonUp || this.MouseDownButtonDown) && !(this.MouseOverButtonUp || this.MouseOverButtonDown) || !this.GetIsEnabled())
                return false;

            return true;
        }

        protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct, Rectangle r)
        {
            Graphics g = p.Graphics;
            if(RenderBackground(p))
                Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RectangleShape);
        }

        protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTableButtonUp(PaintInfo p)
        {
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                Office2007ButtonItemColorTable buttonColorTable = ct.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
                if (!this.GetIsEnabled(p) || !_UpEnabled)
                    return buttonColorTable.Disabled;
                else if (this.MouseDownButtonUp)
                    return buttonColorTable.Pressed;
                else if (this.MouseOverButtonUp)
                    return buttonColorTable.MouseOver;
                else
                    return buttonColorTable.Default;
            }

            return null;
        }

        protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTableButtonDown(PaintInfo p)
        {
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                Office2007ButtonItemColorTable buttonColorTable = ct.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
                if (!this.GetIsEnabled(p) || !_DownEnabled)
                    return buttonColorTable.Disabled;
                else if (this.MouseDownButtonDown)
                    return buttonColorTable.Pressed;
                else if (this.MouseOverButtonDown)
                    return buttonColorTable.MouseOver;
                else
                    return buttonColorTable.Default;
            }

            return null;
        }

        private bool _UpEnabled = true;
        [DefaultValue(true)]
        public bool UpEnabled
        {
            get { return _UpEnabled; }
            set
            {
                if (_UpEnabled != value)
                {
                    _UpEnabled = value;
                    this.InvalidateRender();
                }
            }
        }

        private bool _DownEnabled = true;
        [DefaultValue(true)]
        public bool DownEnabled
        {
            get { return _DownEnabled; }
            set
            {
                if (_DownEnabled != value)
                {
                    _DownEnabled = value;
                    this.InvalidateRender();
                }
            }
        }
        #endregion

    }
}
#endif

