#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors
{
    public class VisualButton : VisualButtonBase
    {
        #region Private Variables

        #endregion

        #region Events
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        public override void PerformLayout(PaintInfo pi)
        {
            Size size = new Size(0, pi.AvailableSize.Height);
            Graphics g = pi.Graphics;

            size.Width += 6; // Border 2 pixels on each side and 1 pixels of padding between border and content

            if (_Text.Length > 0)
            {
                Size textSize = TextDrawing.MeasureString(g, _Text, pi.DefaultFont);
                size.Width += textSize.Width;
                if (_Image != null)
                    size.Width += 4; // Padding between text and image
            }
            
            if (_Image != null)
            {
                size.Width += _Image.Width;
            }

            if (_Text.Length == 0 && _Image == null)
                size.Width += 9;

            this.Size = size;
            base.PerformLayout(pi);
        }

        protected override void OnPaint(PaintInfo p)
        {
            Graphics g = p.Graphics;
            Rectangle r = this.RenderBounds;
            PaintButtonBackground(p);

            Rectangle contentRect = r;
            contentRect.Inflate(-3, -3); // Two pixels border + 1 pixels padding for content
            if (_Text.Length > 0)
            {
                TextDrawing.DrawString(g, _Text, p.DefaultFont, GetIsEnabled(p) ? p.ForeColor : p.DisabledForeColor, contentRect, eTextFormat.Default | eTextFormat.VerticalCenter);
            }

            if (_Image != null)
            {
                Image image = GetIsEnabled(p) ? _Image : GetDisabledImage();
                g.DrawImage(image, new Rectangle(contentRect.Right - _Image.Width, contentRect.Y + (contentRect.Height - _Image.Height) / 2, _Image.Width, _Image.Height));
            }

            base.OnPaint(p);
        }

        protected virtual void PaintButtonBackground(PaintInfo p)
        {
            PaintButtonBackground(p, GetOffice2007StateColorTable(p));
        }

        protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct)
        {
            Graphics g = p.Graphics;
            Rectangle r = this.RenderBounds;
            if (RenderBackground(p))
                Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RectangleShape);
        }

        private bool RenderBackground(PaintInfo p)
        {
            if (RenderDefaultBackground) return true;

            if (!p.MouseOver && !this.IsMouseDown && !this.IsMouseOver && !this.Checked || !this.GetIsEnabled())
                return false;

            return true;
        }

        protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(PaintInfo p)
        {
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                Office2007ColorTable ct = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
                Office2007ButtonItemColorTable buttonColorTable = ct.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
                if (!this.GetIsEnabled(p))
                    return buttonColorTable.Disabled;
                else if (this.IsMouseDown)
                    return buttonColorTable.Pressed;
                else if (this.IsMouseOver)
                    return buttonColorTable.MouseOver;
                else if (this.Checked)
                    return buttonColorTable.Checked;
                else
                    return buttonColorTable.Default;
            }

            return null;
        }

        protected override void OnMouseEnter()
        {
            if (this.GetIsEnabled())
                this.IsMouseOver = true;
            base.OnMouseEnter();
        }

        protected override void OnMouseLeave()
        {
            this.IsMouseOver = false;
            base.OnMouseLeave();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.IsMouseDown = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.IsMouseDown = false;
            base.OnMouseUp(e);
        }

        private bool _IsMouseOver = false;
        /// <summary>
        /// Gets whether mouse is over the control.
        /// </summary>
        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
            internal set
            {
                if (_IsMouseOver != value)
                {
                    _IsMouseOver = value;
                    this.InvalidateRender();
                }
            }
        }

        private bool _IsMouseDown = false;
        /// <summary>
        /// Gets whether mouse is pressed on the control.
        /// </summary>
        public bool IsMouseDown
        {
            get { return _IsMouseDown; }
            internal set
            {
                if (_IsMouseDown != value)
                {
                    _IsMouseDown = value;
                    this.InvalidateRender();
                }
            }
        }

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    this.InvalidateRender();
                }
            }
        }
        

        private string _Text = "";
        /// <summary>
        /// Gets or sets the text displayed on the face of the button.
        /// </summary>
        [DefaultValue("")]
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value == null) value = "";
                if (_Text != value)
                {
                    _Text = value;
                    this.InvalidateArrange();
                }
            }
        }

        private Image GetDisabledImage()
        {
            if (_DisabledImage == null && _Image != null)
            {
                _DisabledImage = ImageHelper.CreateGrayScaleImage(_Image as Bitmap);
            }

            return _DisabledImage;
        }

        private Image _DisabledImage = null;
        private Image _Image = null;
        /// <summary>
        /// Gets or sets the image displayed on the face of the button.
        /// </summary>
        [DefaultValue(null)]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    if (_DisabledImage != null)
                    {
                        _DisabledImage.Dispose();
                        _DisabledImage = null;
                    }
                    this.InvalidateArrange();
                }
            }
        }
        #endregion

    }
}
#endif

