using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(CircularProgress), "Controls.CircularProgress.ico"), ToolboxItem(true), DefaultEvent("ValueChanged")]
    public class CircularProgress : BaseItemControl
    {
        #region Constructor
        private CircularProgressItem _Item = null;
        /// <summary>
        /// Initializes a new instance of the CircularProgress class.
        /// </summary>
        public CircularProgress()
        {
            _Item = new CircularProgressItem();
            _Item.ValueChanged += new EventHandler(ItemValueChanged);
            _Item.TextVisible = false;
            this.HostItem = _Item;
        }
        #endregion

        #region Implementation
        protected override void Dispose(bool disposing)
        {
            _Item.Dispose();
            base.Dispose(disposing);
        }
        protected override void RecalcSize()
        {
            _Item.Diameter = Math.Min(this.Width, this.Height);
            base.RecalcSize();
        }
        private void ItemValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(e);
        }
        /// <summary>
        /// Occurs when Value property has changed.
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises ValueChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Gets or sets the circular progress bar type.
        /// </summary>
        [DefaultValue(eCircularProgressType.Line), Category("Appearance"), Description("Indicates circular progress bar type.")]
        public eCircularProgressType ProgressBarType
        {
            get { return _Item.ProgressBarType; }
            set
            {
                _Item.ProgressBarType = value;
            }
        }
        /// <summary>
        /// Gets or sets the maximum value of the progress bar.
        /// </summary>
        [Description("Indicates maximum value of the progress bar."), Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get { return _Item.Maximum; }
            set
            {
                _Item.Maximum = value;
            }
        }
        /// <summary>
        /// Gets or sets the minimum value of the progress bar.
        /// </summary>
        [Description("Indicates minimum value of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get { return _Item.Minimum; }
            set
            {
                _Item.Minimum = value;
            }
        }
        /// <summary>
        /// Gets or sets the color of the progress percentage text.
        /// </summary>
        [Category("Appearance"), Description("Indicates color of progress percentage text")]
        public Color ProgressTextColor
        {
            get { return _Item.ProgressTextColor; }
            set { _Item.ProgressTextColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProgressTextColor()
        {
            return _Item.ShouldSerializeProgressTextColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetProgressTextColor()
        {
            _Item.ResetProgressTextColor();
        }

        /// <summary>
        /// Gets or sets whether text that displays the progress bar completion percentage text is visible. Default value is false.
        /// </summary>
        [DefaultValue(false), Category("Appearance"), Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
        public bool ProgressTextVisible
        {
            get { return _Item.ProgressTextVisible; }
            set
            {
                _Item.ProgressTextVisible = value;
            }
        }
        /// <summary>
        /// Gets or sets the text displayed on top of the circular progress bar.
        /// </summary>
        [DefaultValue(""), Category("Appearance"), Description("Indicates text displayed on top of the circular progress bar.")]
        public string ProgressText
        {
            get { return _Item.ProgressText; }
            set
            {
                _Item.ProgressText = value;
            }
        }
        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        [Description("Indicates current value of the progress bar."), Category("Behavior"), DefaultValue(0)]
        public int Value
        {
            get { return _Item.Value; }
            set
            {
                _Item.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets whether endless type progress bar is running.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IsRunning
        {
            get { return _Item.IsRunning; }
            set
            {
                _Item.IsRunning = value;
            }
        }
        /// <summary>
        /// Gets or sets the color of the color of progress indicator.
        /// </summary>
        [Category("Columns"), Description("Indicates color of progress indicator.")]
        public Color ProgressColor
        {
            get { return _Item.ProgressColor; }
            set { _Item.ProgressColor = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProgressColor()
        {
            return _Item.ShouldSerializeProgressColor();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetProgressColor()
        {
            _Item.ResetProgressColor();
        }

        ///// <summary>
        ///// Gets or sets circular progress indicator diameter in pixels.
        ///// </summary>
        //[DefaultValue(24), Category("Appearance"), Description("Indicates circular progress indicator diameter in pixels.")]
        //public int Diameter
        //{
        //    get { return _Item.Diameter; }
        //    set
        //    {
        //        _Item.Diameter = value;
        //        this.RecalcLayout();
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the text position in relation to the circular progress indicator.
        ///// </summary>
        //[DefaultValue(eTextPosition.Left), Category("Appearance"), Description("Indicatesd text position in relation to the circular progress indicator.")]
        //public eTextPosition TextPosition
        //{
        //    get { return _Item.TextPosition; }
        //    set
        //    {
        //        _Item.TextPosition = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets whether text/label displayed next to the item is visible. 
        ///// </summary>
        //[DefaultValue(true), Category("Appearance"), Description("Indicates whether caption/label set using Text property is visible.")]
        //public bool TextVisible
        //{
        //    get { return _Item.TextVisible; }
        //    set
        //    {
        //        _Item.TextVisible = value;
        //    }
        //}
        ///// <summary>
        ///// Gets or sets the suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.
        ///// </summary>
        //[DefaultValue(0), Category("Appearance"), Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
        //public int TextWidth
        //{
        //    get { return _Item.TextWidth; }
        //    set
        //    {
        //        _Item.TextWidth = value;
        //    }
        //}

        ///// <summary>
        ///// Gets or sets text padding.
        ///// </summary>
        //[Browsable(true), Category("Appearance"), Description("Gets or sets text padding."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public Padding TextPadding
        //{
        //    get { return _Item.TextPadding; }
        //}
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeTextPadding()
        //{
        //    return _Item.ShouldSerializeTextPadding();
        //}
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetTextPadding()
        //{
        //    _Item.ResetTextPadding();
        //}

        //protected override void OnForeColorChanged(EventArgs e)
        //{
        //    _Item.TextColor = this.ForeColor;
        //    base.OnForeColorChanged(e);
        //}

        /// <summary>
        /// Gets or sets the color of the pie progress bar dark border. 
        /// </summary>
        [Category("Pie"), Description("Indicates color of pie progress bar dark border.")]
        public Color PieBorderDark
        {
            get { return _Item.PieBorderDark; }
            set { _Item.PieBorderDark = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePieBorderDark()
        {
            return _Item.ShouldSerializePieBorderDark();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPieBorderDark()
        {
            _Item.ResetPieBorderDark();
        }
        /// <summary>
        /// Gets or sets the color of the pie progress bar light border. 
        /// </summary>
        [Category("Pie"), Description("Indicates color of pie progress bar light border. ")]
        public Color PieBorderLight
        {
            get { return _Item.PieBorderLight; }
            set { _Item.PieBorderLight = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePieBorderLight()
        {
            return _Item.ShouldSerializePieBorderLight();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetPieBorderLight()
        {
            _Item.ResetPieBorderLight();
        }

        /// <summary>
        /// Gets or sets the color of the spoke progress bar dark border.
        /// </summary>
        [Category("Spoke"), Description("Indicates color of spoke progress bar dark border.")]
        public Color SpokeBorderDark
        {
            get { return _Item.SpokeBorderDark; }
            set { _Item.SpokeBorderDark = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpokeBorderDark()
        {
            return _Item.ShouldSerializeSpokeBorderDark();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSpokeBorderDark()
        {
            _Item.ResetSpokeBorderDark();
        }
        /// <summary>
        /// Gets or sets the color of the spoke progress bar light border.
        /// </summary>
        [Category("Spoke"), Description("Indicates color of spoke progress bar light border..")]
        public Color SpokeBorderLight
        {
            get { return _Item.SpokeBorderLight; }
            set { _Item.SpokeBorderLight = value; }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpokeBorderLight()
        {
            return _Item.ShouldSerializeSpokeBorderLight();
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSpokeBorderLight()
        {
            _Item.ResetSpokeBorderLight();
        }

        /// <summary>
        /// Gets or sets format string for progress value.
        /// </summary>
        [DefaultValue("{0}%"), Category("Appearance"), Description("Indicates format string for progress value.")]
        public string ProgressTextFormat
        {
            get { return _Item.ProgressTextFormat; }
            set
            {
                _Item.ProgressTextFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the animation speed for endless running progress. Lower number means faster running.
        /// </summary>
        [DefaultValue(100), Description("Indicates the animation speed for endless running progress. Lower number means faster running."), Category("Behavior")]
        public int AnimationSpeed
        {
            get
            {
                return _Item.AnimationSpeed;
            }
            set
            {
            	_Item.AnimationSpeed = value;
            }
        }
        #endregion

        #region Property-Hiding
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        #endregion
    }
}
