using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents an item that provides system buttons displayed on form caption.
    /// </summary>
    public class SystemCaptionItem : MDISystemItem
    {
        #region Private Variables
        private bool m_MinimizeVisible = true;
        private bool m_RestoreMaximizeVisible = true;
        private bool m_CloseVisible = true;
        private bool m_HelpVisible = false;
        private bool m_GlassEnabled = false;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets the default size of the system buttons.
        /// </summary>
        /// <returns></returns>
        internal override Size GetButtonSize()
        {
            Size s;
            if (!_CustomButtonSize.IsEmpty)
                return _CustomButtonSize;
            if (_ToolWindowButtons)
            {
                s = System.Windows.Forms.SystemInformation.ToolWindowCaptionButtonSize;
                s.Width += 5;
                return s;
            }

            s = System.Windows.Forms.SystemInformation.CaptionButtonSize;
            if (System.Environment.OSVersion.Version.Major < 6 && this.ContainerControl is RibbonStrip)
                s = new Size(25, 25);
            if (System.Environment.OSVersion.Version.Major >= 6 && s.Height == 19 && this.Parent is CaptionItemContainer)
                s.Height += 3;
            return s;
        }

        private Size _CustomButtonSize = Size.Empty;
        /// <summary>
        /// Gets or sets the custom button size to use instead of system determined size.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size CustomButtonSize
        {
            get { return _CustomButtonSize; }
            set { _CustomButtonSize = value; }
        }

        public override void Paint(ItemPaintArgs pa)
        {
            if (this.SuspendLayout)
                return;

            if (_QueryIconOnPaint && pa.ContainerControl!=null)
            {
                Form parentForm = pa.ContainerControl.FindForm();
                if (parentForm != null)
                {
                    this.SetIcon(parentForm.Icon);
                }
            }

            m_GlassEnabled = pa.GlassEnabled;

            if (pa.Renderer != null)
                pa.Renderer.DrawSystemCaptionItem(new SystemCaptionItemRendererEventArgs(pa.Graphics, this, pa.GlassEnabled));
            else
                base.Paint(pa);
        }

        protected override bool ShowToolTips
        {
            get
            {
                return !m_GlassEnabled;
            }
        }

        /// <summary>
        /// Gets or sets whether Minimize button is visible.
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool MinimizeVisible
        {
            get { return m_MinimizeVisible; }
            set
            {
                m_MinimizeVisible = value;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether Restore/Maximize button is visible.
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool RestoreMaximizeVisible
        {
            get { return m_RestoreMaximizeVisible; }
            set
            {
                m_RestoreMaximizeVisible = value;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether Restore/Maximize button is visible.
        /// </summary>
        [Browsable(false), DefaultValue(true)]
        public bool CloseVisible
        {
            get { return m_CloseVisible; }
            set
            {
                m_CloseVisible = value;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether help button is visible.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool HelpVisible
        {
            get { return m_HelpVisible; }
            set
            {
                m_HelpVisible = value;
                this.OnAppearanceChanged();
            }
        }

        public override void RecalcSize()
        {

            if (this.SuspendLayout)
                return;

            if (this.IsSystemIcon)
            {
                base.RecalcSize();
            }
            else
            {
                Size singleButtonSize = GetButtonSize();
                int buttonCount = 0;
                if (m_MinimizeVisible)
                    buttonCount++;
                if(m_RestoreMaximizeVisible)
                    buttonCount++;
                if (m_CloseVisible)
                    buttonCount++;
                if (m_HelpVisible)
                    buttonCount++;

                if (this.Orientation == eOrientation.Horizontal)
                    m_Rect.Size = new Size(singleButtonSize.Width * buttonCount + buttonCount - 1, singleButtonSize.Height);
                else
                    m_Rect.Size = new Size(singleButtonSize.Width, singleButtonSize.Height * buttonCount + buttonCount - 1);
            }

            m_NeedRecalcSize = false;
        }

        internal override SystemButton GetButton(int x, int y)
        {
            Rectangle r = new Rectangle(this.DisplayRectangle.Location, GetButtonSize());
            //r.Inflate(-1, -2);
            r.Location = this.DisplayRectangle.Location;

            if (this.Orientation == eOrientation.Horizontal)
                r.Offset(0, (this.DisplayRectangle.Height - r.Height) / 2);
            else
                r.Offset((this.WidthInternal - r.Width) / 2, 0);

            if (m_HelpVisible && (!IsRightToLeft || m_CloseVisible && IsRightToLeft))
            {
                if (r.Contains(x, y))
                {
                    if (IsRightToLeft)
                        return SystemButton.Close;
                    else
                        return SystemButton.Help;
                }

                if (this.Orientation == eOrientation.Horizontal)
                    r.Offset(r.Width + 1, 0);
                else
                    r.Offset(0, r.Height + 1);
            }

            if (m_MinimizeVisible && m_HelpVisible || !m_HelpVisible && m_MinimizeVisible && (!IsRightToLeft || m_CloseVisible && IsRightToLeft))
            {
                if (r.Contains(x, y))
                {
                    if (IsRightToLeft)
                        return SystemButton.Close;
                    else
                        return SystemButton.Minimize;
                }

                if (this.Orientation == eOrientation.Horizontal)
                    r.Offset(r.Width + 1, 0);
                else
                    r.Offset(0, r.Height + 1);
            }

            if (m_RestoreMaximizeVisible)
            {
                if (r.Contains(x, y))
                {
                    if (this.RestoreEnabled)
                        return SystemButton.Restore;
                    return SystemButton.Maximize;
                }

                if (this.Orientation == eOrientation.Horizontal)
                    r.Offset(r.Width + 3, 0);
                else
                    r.Offset(0, r.Height + 3);
            }

            if (m_CloseVisible && !IsRightToLeft || m_MinimizeVisible && IsRightToLeft)
            {
                if (r.Contains(x, y))
                {
                    if (IsRightToLeft)
                        return SystemButton.Minimize;
                    else
                        return SystemButton.Close;
                }
            }

            return SystemButton.None;
        }

        public override void Refresh()
        {
            base.Refresh();
            System.Drawing.Rectangle inv = m_Rect;
            Control c = this.ContainerControl as Control;
            if (c != null && IsHandleValid(c))
            {
                const int RDW_INVALIDATE = 0x0001;
                const int RDW_FRAME = 0x0400;
                int height = SystemInformation.Border3DSize.Height + SystemInformation.CaptionHeight;
                NativeFunctions.RECT r = new NativeFunctions.RECT(0, -height, c.Width, height);
                NativeFunctions.RedrawWindow(c.Handle, ref r, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME);
            }
        }

        private bool _QueryIconOnPaint = false;
        /// <summary>
        /// Gets or sets whether Icon is queried when item is painted. Default value is false.
        /// </summary>
        [Browsable(false)]
        public bool QueryIconOnPaint
        {
            get { return _QueryIconOnPaint; }
            set
            {
                _QueryIconOnPaint = value;
            }
        }

        private bool _ToolWindowButtons = false;
        internal bool ToolWindowButtons
        {
            get { return _ToolWindowButtons; }
            set { _ToolWindowButtons = value;}
        }
        #endregion
    }
}
