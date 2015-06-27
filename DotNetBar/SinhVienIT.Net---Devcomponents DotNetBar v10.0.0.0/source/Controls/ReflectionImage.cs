using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Controls
{
    /// <summary>
    /// Represents an image control with built-in reflection.
    /// </summary>
    [ToolboxBitmap(typeof(ReflectionImage), "Controls.ReflectionImage.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.ReflectionImageDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class ReflectionImage : ControlWithBackgroundStyle
    {
        #region Private Variables
        private Bitmap _ReflectionBitmap = null;
        private Bitmap _DisabledBitmap = null;
        #endregion

        #region Events
        /// <summary>
        /// Initializes a new instance of the ReflectionImage class.
        /// </summary>
        public ReflectionImage()
        {
        }
        #endregion

        #region Constructor
        #endregion

        #region Internal Implementation
        protected override void Dispose(bool disposing)
        {
            DisposeReflectionImage();
            DisposeDisabledImage();
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _Image);
            }
            base.Dispose(disposing);
        }
        private Image _Image = null;
        /// <summary>
        /// Gets or sets the image displayed on the control.
        /// </summary>
        [DefaultValue(null), Description("Indicates image displayed on the control."), Category("Appearance")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    OnImageChanged();
                }
            }
        }

        private void OnImageChanged()
        {
            DisposeDisabledImage();
            CreateReflectionImage();
        }

        private void CreateReflectionImage()
        {
            DisposeReflectionImage();
            Image image = GetImage();
            if (image != null)
            {
                _ReflectionBitmap = ImageHelper.CreateReflectionImage(image);
            }
            this.Invalidate();
        }

        private Image GetImage()
        {
            if (this.Enabled) return _Image;
            if (_DisabledBitmap == null)
            {
                _DisabledBitmap = ImageHelper.CreateGrayScaleImage(_Image);
            }
            return _DisabledBitmap;
        }

        private void DisposeReflectionImage()
        {
            if (_ReflectionBitmap != null)
            {
                _ReflectionBitmap.Dispose();
                _ReflectionBitmap = null;
            }
        }

        private void DisposeDisabledImage()
        {
            if (_DisabledBitmap != null)
            {
                _DisabledBitmap.Dispose();
                _DisabledBitmap = null;
            }
        }

        protected override void PaintContent(PaintEventArgs e)
        {
            Image image = GetImage();
            if (image == null) return;

            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;
            if (AntiAlias)
                g.SmoothingMode = SmoothingMode.HighQuality;

            ElementStyle style = GetBackgroundStyle();
            Rectangle r = GetContentRectangle();
            Point imageLocation = r.Location;

            if (style.TextAlignment == eStyleTextAlignment.Center && r.Width > image.Width)
                imageLocation.X += (r.Width - image.Width) / 2;
            else if (style.TextAlignment == eStyleTextAlignment.Far && r.Width > image.Width)
                imageLocation.X += (r.Width - image.Width);
            float reflectionFactor = .52f;
            if (style.TextLineAlignment == eStyleTextAlignment.Center && r.Height > (image.Height + image.Height * reflectionFactor))
                imageLocation.Y += (int)((r.Height - (image.Height + image.Height * reflectionFactor)) / 2);
            else if (style.TextLineAlignment == eStyleTextAlignment.Far && r.Height > (image.Height + image.Height * reflectionFactor))
                imageLocation.Y += (int)(r.Height - (image.Height + image.Height * reflectionFactor));

            g.DrawImage(image, imageLocation.X, imageLocation.Y, image.Width, image.Height);

            if (_ReflectionBitmap != null && _ReflectionEnabled)
            {
                imageLocation.Y += image.Height;
                g.DrawImage(_ReflectionBitmap, imageLocation);
            }

            if (AntiAlias)
                g.SmoothingMode = sm;
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(128, 128);
            }
        }

        private bool _ReflectionEnabled = true;
        /// <summary>
        /// Gets or sets whether reflection effect is enabled. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether reflection effect is enabled.")]
        public bool ReflectionEnabled
        {
            get { return _ReflectionEnabled; }
            set
            {
                if (_ReflectionEnabled != value)
                {
                    _ReflectionEnabled = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            CreateReflectionImage();
            base.OnEnabledChanged(e);
        }
        #endregion

        #region Property Hiding
        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }
        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

#if FRAMEWORK20
		[Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        [Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }
#endif
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        #endregion

    }
}
