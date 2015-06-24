using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    /// <summary>
    /// Represents the Gauge control.
    /// </summary>
    [ToolboxItem(true), Designer("DevComponents.Instrumentation.Design.GaugeControlDesigner, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5")]
    public partial class GaugeControl : Control, IDisposable, ISupportInitialize
    {
        #region Events

        #region Gauge events

        [Description("Occurs before any Gauge content is rendered.")]
        public event EventHandler<RenderGaugeContentEventArgs> PreRenderGaugeContent;

        [Description("Occurs after all Gauge content is rendered.")]
        public event EventHandler<RenderGaugeContentEventArgs> PostRenderGaugeContent;

        [Description("Occurs when GaugeItem DisplayTemplate text is needed.")]
        public event EventHandler<GetDisplayTemplateTextEventArgs> GetDisplayTemplateText;

        [Description("Occurs when Dampened Pointer Value needs updated.")]
        internal event EventHandler<EventArgs> DampeningUpdate;

        #endregion

        #region Scale events

        [Description("Occurs before each Scale is rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScale;

        [Description("Occurs after each Scale is rendered.")]
        public event EventHandler<PostRenderScaleEventArgs> PostRenderScale;

        #endregion

        #region Range events

        [Description("Occurs before any Scale Range is rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScaleRanges;

        [Description("Occurs after every Scale Range has been rendered.")]
        public event EventHandler<PostRenderScaleEventArgs> PostRenderScaleRanges;

        [Description("Occurs before each Scale Range is rendered.")]
        public event EventHandler<PreRenderScaleRangeEventArgs> PreRenderScaleRange;

        [Description("Occurs after each Scale Range is rendered.")]
        public event EventHandler<PostRenderScaleRangeEventArgs> PostRenderScaleRange;

        #endregion

        #region Section Events

        [Description("Occurs before any Scale Section is rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScaleSections;

        [Description("Occurs after every Scale Section has been rendered.")]
        public event EventHandler<PostRenderScaleEventArgs> PostRenderScaleSections;

        [Description("Occurs before each Scale Section is rendered.")]
        public event EventHandler<PreRenderScaleSectionEventArgs> PreRenderScaleSection;

        [Description("Occurs after each Scale Section is rendered.")]
        public event EventHandler<PostRenderScaleSectionEventArgs> PostRenderScaleSection;

        #endregion

        #region Pointer Events

        [Description("Occurs before any Scale Pointer is rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScalePointers;

        [Description("Occurs after every Scale Pointer has been rendered.")]
        public event EventHandler<EventArgs> PostRenderScalePointers;

        [Description("Occurs before each Scale Pointer is rendered.")]
        public event EventHandler<PreRenderScalePointerEventArgs> PreRenderScalePointer;

        [Description("Occurs after each Scale Pointer is rendered.")]
        public event EventHandler<PostRenderScalePointerEventArgs> PostRenderScalePointer;

        [Description("Occurs when a Pointer enters a Scale.")]
        public event EventHandler<ScaleEnterEventArgs> ScaleEnter;

        [Description("Occurs when a Pointer leaves a Scale.")]
        public event EventHandler<ScaleLeaveEventArgs> ScaleLeave;

        [Description("Occurs when a Pointer enters a Range.")]
        public event EventHandler<RangeEnterEventArgs> RangeEnter;

        [Description("Occurs when a Pointer leaves a Range.")]
        public event EventHandler<RangeLeaveEventArgs> RangeLeave;

        [Description("Occurs when a Pointer enters a Section.")]
        public event EventHandler<SectionEnterEventArgs> SectionEnter;

        [Description("Occurs when a Pointer leaves a Section.")]
        public event EventHandler<SectionLeaveEventArgs> SectionLeave;

        [Description("Occurs when a Pointer's Value is interactively changing.")]
        public event EventHandler<PointerChangingEventArgs> PointerChanging;

        [Description("Occurs when a Pointer's Value has been interactively changed.")]
        public event EventHandler<PointerChangedEventArgs> PointerChanged;

        [Description("Occurs when a Pointer's Value has been changed.")]
        public event EventHandler<PointerChangedEventArgs> PointerValueChanged;

        [Description("Occurs when a Pointer's GraphicsPath is needed.")]
        public event EventHandler<GetPointerPathEventArgs> GetPointerPath;

        #endregion

        #region TickMark events

        [Description("Occurs before Scale TickMaks are rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScaleTickMarks;

        [Description("Occurs after Scale TickMaks are rendered.")]
        public event EventHandler<PostRenderScaleEventArgs> PostRenderScaleTickMarks;

        #endregion

        #region GaugePin events

        [Description("Occurs before each Scale Min/Max Pin is rendered.")]
        public event EventHandler<PreRenderScaleGaugePinEventArgs> PreRenderScaleGaugePin;

        [Description("Occurs after each Scale Min/Max Pin has been rendered.")]
        public event EventHandler<PostRenderScaleGaugePinEventArgs> PostRenderScaleGaugePin;

        #endregion

        #region TickMarkLabel events

        [Description("Occurs before Scale TickMaks Labels are rendered.")]
        public event EventHandler<PreRenderScaleEventArgs> PreRenderScaleTickMarkLabels;

        [Description("Occurs after Scale TickMaks Labels are rendered.")]
        public event EventHandler<PostRenderScaleEventArgs> PostRenderScaleTickMarkLabels;

        #endregion

        #region CustomLabel events

        [Description("Occurs before each Scale Custom Label is rendered.")]
        public event EventHandler<PreRenderScaleCustomLabelEventArgs> PreRenderScaleCustomLabel;

        [Description("Occurs after each Scale Custom Label is rendered.")]
        public event EventHandler<PostRenderScaleCustomLabelEventArgs> PostRenderScaleCustomLabel;

        #endregion

        #region Indicator events

        [Description("Occurs before each Indicator is rendered.")]
        public event EventHandler<PreRenderIndicatorEventArgs> PreRenderIndicator;

        [Description("Occurs after each Indicator is rendered.")]
        public event EventHandler<PostRenderIndicatorEventArgs> PostRenderIndicator;

        [Description("Occurs before each Indicator Digit is rendered.")]
        public event EventHandler<PreRenderIndicatorDigitEventArgs> PreRenderIndicatorDigit;

        [Description("Occurs after each Indicator Digit is rendered.")]
        public event EventHandler<PostRenderIndicatorDigitEventArgs> PostRenderIndicatorDigit;

        [Description("Occurs when an Indicator's digit segment pattern is needed.")]
        public event EventHandler<GetDigitSegmentsEventArgs> GetDigitSegments;

        #endregion

        #endregion

        #region Private variables

        private GaugeFrame _Frame;

        private GaugeCircularScaleCollection _CircularScales;
        private GaugeLinearScaleCollection _LinearScales;
        private GaugeItemCollection _GaugeItems;

        private ToolTip _ToolTip;
        private bool _ShowToolTips;
        private bool _AntiAlias;

        private GaugeItem _HotItem;

        private bool _MouseDown;
        private bool _InitComplete;
        private bool _NeedRecalcLayout;

        private int _SuspendCount;
        private int _DampCount;
        private bool _DValueChange;
        private BackgroundWorker _DampWorker;

        #endregion

        public GaugeControl()
        {
            // Initialize our control

            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, true);

            _Frame = new GaugeFrame(this);

            _CircularScales = new GaugeCircularScaleCollection();
            _LinearScales = new GaugeLinearScaleCollection();
            _GaugeItems = new GaugeItemCollection();

            _ToolTip = new ToolTip();
            _ShowToolTips = true;
            _AntiAlias = true;

            NeedRecalcLayout = true;

            HookEvents(true);
        }

        #region DefaultSize

        protected override Size DefaultSize
        {
            get { return new Size(250, 250); }
        }

        #endregion

        #region Licensing

#if !TRIAL
        private string _LicenseKey = "";
        private bool _DialogDisplayed;

        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return _LicenseKey; }

            set
            {
                if (Licensing.ValidateLicenseKey(value) == false)
                    _LicenseKey = (!Licensing.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        #endregion

        #region Public properties

        #region AntiAlias

        [Browsable(true), DefaultValue(true), Category("Appearance")]
        [Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return (_AntiAlias); }

            set
            {
                if (_AntiAlias != value)
                {
                    _AntiAlias = value;

                    OnGaugeChanged();
                }
            }
        }

        #endregion

        #region Frame

        /// <summary>
        /// Gets the gauge Frame.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the gauge Frame.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeFrame Frame
        {
            get { return (_Frame); }
        }

        #endregion

        #region GaugeItems

        /// <summary>
        /// Gets the collection og GaugeItems (Text, Image, etc)
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection root Gauge Items contained within the gauge.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeItemCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugeItemCollection GaugeItems
        {
            get { return (_GaugeItems); }
        }

        #endregion

        #region CircularScales

        /// <summary>
        /// Gets the collection of Circular Scales contained within the gauge.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection Circular Scales contained within the gauge.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugeCircularScaleCollection CircularScales
        {
            get { return (_CircularScales); }
        }

        #endregion

        #region LinearScales

        /// <summary>
        /// Gets the collection of Linear Scales contained within the gauge.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection Linear Scales contained within the gauge.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("DevComponents.Instrumentation.Design.GaugeCollectionEditor, DevComponents.Instrumentation.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=76cb4c6eb576bca5", typeof(UITypeEditor))]
        public GaugeLinearScaleCollection LinearScales
        {
            get { return (_LinearScales); }
        }

        #endregion

        #region ShowToolTips

        /// <summary>
        /// Gets or sets whether to display ToolTips
        /// </summary>
        [Browsable(true)]
        [Category("Behavior"), DefaultValue(true)]
        [Description("Determines whether to display ToolTips.")]
        public bool ShowToolTips
        {
            get { return (_ShowToolTips); }
            set { _ShowToolTips = value; }
        }

        #endregion

        #endregion

        #region Internal properties

        #region DValueChange

        internal bool DValueChange
        {
            set { _DValueChange = value; }
        }

        #endregion

        #region HasGetDigitSegments

        internal bool HasGetDigitSegments
        {
            get { return (GetDigitSegments != null); }
        }

        #endregion

        #region InitComplete

        internal bool InitComplete
        {
            get { return (_InitComplete); }
        }

        #endregion

        #region InDesignMode

        internal bool InDesignMode
        {
            get { return (DesignMode); }
        }

        #endregion

        #region IsSuspended

        internal bool IsSuspended
        {
            get { return (_SuspendCount > 0); }
        }

        #endregion

        #region NeedRecalcLayout

        internal bool NeedRecalcLayout
        {
            get { return (_NeedRecalcLayout); }

            set
            {
                _NeedRecalcLayout = value;

                if (_NeedRecalcLayout == true)
                    OnLayoutChanged();
            }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _Frame.GaugeFrameChanged += GaugeFrameGaugeFrameChanged;
                _CircularScales.CollectionChanged += GaugeCircularScaleItemsCollectionChanged;
                _LinearScales.CollectionChanged += GaugeLinearScaleItemsCollectionChanged;
                _GaugeItems.CollectionChanged += GaugeItemsCollectionChanged;
            }
            else
            {
                _Frame.GaugeFrameChanged -= GaugeFrameGaugeFrameChanged;
                _CircularScales.CollectionChanged -= GaugeCircularScaleItemsCollectionChanged;
                _LinearScales.CollectionChanged -= GaugeLinearScaleItemsCollectionChanged;
                _GaugeItems.CollectionChanged -= GaugeItemsCollectionChanged;
            }
        }

        #endregion

        #region Event processing

        #region GaugeFrame_GaugeFrameChanged

        void GaugeFrameGaugeFrameChanged(object sender, EventArgs e)
        {
            OnGaugeChanged();
        }

        #endregion

        #region GaugeItems_CollectionChanged

        void GaugeItemsCollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugeItem item in _GaugeItems)
            {
                if (item is GaugeImage)
                {
                    GaugeImage gi = item as GaugeImage;

                    gi.GaugeControl = this;

                    if (gi.Name == null)
                        gi.Name = GetItemName("Image");
                }
                else if (item is GaugeText)
                {
                    GaugeText gt = item as GaugeText;

                    gt.GaugeControl = this;

                    if (gt.Name == null)
                        gt.Name = GetItemName("Text");
                }
                else if (item is NumericIndicator)
                {
                    NumericIndicator gt = item as NumericIndicator;

                    gt.GaugeControl = this;

                    if (gt.Name == null)
                        gt.Name = GetItemName("NumericIndicator");
                }
                else if (item is StateIndicator)
                {
                    StateIndicator gt = item as StateIndicator;

                    gt.GaugeControl = this;

                    if (gt.Name == null)
                        gt.Name = GetItemName("StateIndicator");
                }

                item.NeedRecalcLayout = true;

                item.GaugeItemChanged -= GaugeGaugeItemChanged;
                item.GaugeItemChanged += GaugeGaugeItemChanged;
            }

            OnGaugeChanged();
        }

        #endregion

        #region Gauge_GaugeItemChanged

        void GaugeGaugeItemChanged(object sender, EventArgs e)
        {
            OnGaugeChanged();
        }

        #endregion

        #region GaugeCircularScaleItems_CollectionChanged

        void GaugeCircularScaleItemsCollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugeCircularScale scale in _CircularScales)
            {
                scale.GaugeControl = this;
                scale.NeedRecalcLayout = true;

                scale.GaugeItemChanged -= GaugeGaugeCircularItemChanged;
                scale.GaugeItemChanged += GaugeGaugeCircularItemChanged;

                scale.UpdateValueData();
            }

            OnGaugeChanged();
        }

        #endregion

        #region GaugeLinearScaleItems_CollectionChanged

        void GaugeLinearScaleItemsCollectionChanged(object sender, EventArgs e)
        {
            foreach (GaugeLinearScale scale in _LinearScales)
            {
                scale.GaugeControl = this;
                scale.NeedRecalcLayout = true;

                scale.GaugeItemChanged -= GaugeGaugeLinearItemChanged;
                scale.GaugeItemChanged += GaugeGaugeLinearItemChanged;
            }

            OnGaugeChanged();
        }

        #endregion

        #region Gauge_GaugeCircularItemChanged

        void GaugeGaugeCircularItemChanged(object sender, EventArgs e)
        {
            GaugeCircularScale scale = sender as GaugeCircularScale;

            if (scale != null)
                Invalidate();
        }

        #endregion

        #region Gauge_GaugeLinearItemChanged

        void GaugeGaugeLinearItemChanged(object sender, EventArgs e)
        {
            GaugeLinearScale scale = sender as GaugeLinearScale;

            if (scale != null)
                Invalidate();
        }

        #endregion

        #region GetItemName

        private string GetItemName(string s)
        {
            for (int i = 1; i < 100; i++)
            {
                string name = s + i;

                if (_GaugeItems[name] == null)
                    return (name);
            }

            return (null);
        }

        #endregion

        #endregion

        #region BeginUpdate

        /// <summary>
        /// Disables any redrawing of the Gauge control. To maintain performance while items
        /// are added one at a time to the control, call the BeginUpdate method. The BeginUpdate
        /// method prevents the control from painting until the
        /// <see cref="EndUpdate">EndUpdate</see> method is called.
        /// </summary>
        public void BeginUpdate()
        {
            _SuspendCount++;
        }

        #endregion

        #region EndUpdate

        /// <summary>
        /// Enables the redrawing of the Gauge control. To maintain performance while items are
        /// added one at a time to the control, call the <see cref="BeginUpdate">BeginUpdate</see>
        /// method. The BeginUpdate method prevents the control from painting until the EndUpdate
        /// method is called.
        /// </summary>
        public void EndUpdate()
        {
            EndUpdate(true);
        }

        internal void EndUpdate(bool refresh)
        {
            if (_SuspendCount > 0)
            {
                _SuspendCount--;

                if (_SuspendCount == 0 && refresh == true)
                    Refresh();
            }
        }

        #endregion

        #region RecalcLayout

        private void RecalcLayout()
        {
            if (_NeedRecalcLayout == true)
                NeedRecalcLayout = false;
        }

        #endregion

        #region Refresh

        public override void Refresh()
        {
            NeedRecalcLayout = true;

            base.Refresh();
        }

        #endregion

        #region OnPaint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_SuspendCount == 0)
            {
                RecalcLayout();

                Graphics g = e.Graphics;
                Region rgn = g.Clip;

                if (_AntiAlias == true)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                }

                PaintGauge(e, rgn);

                g.Clip = rgn;
            }
        }

        #endregion

        #region PaintGauge

        private void PaintGauge(PaintEventArgs e, Region rgn)
        {
            if (_AntiAlias == true)
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            _Frame.OnPaint(e);
#if TRIAL
            if (Licensing.ColorExpAlt())
            {
                e.Graphics.Clip = rgn;

                PaintLicenseText(e.Graphics,
                    "Thank you very much for trying GaugeControl. Unfortunately your trial period has expired. " +
                    "To continue using GaugeControl you should purchase a license at http://www.devcomponents.com",
                    10, Color.Gray, Color.Red, 255);

                return;
            }
#endif
            if (_Frame.Renderer != null)
                _Frame.Renderer.SetBackClipRegion(e);

            OnPreContentRender(e);

            PaintGaugeItems(e, true);
            PaintScaleItems(e);
            PaintGaugeItems(e, false);
            PaintPointerItems(e, false);

            OnPostContentRender(e);

#if TRIAL
            e.Graphics.Clip = rgn;

            PaintLicenseText(e.Graphics,
                "DotNetBar\nGaugeControl\nTrial Version.\nEvaluation only.",
                8, Color.LightGray, Color.Gray, 150);
#else
            if (Licensing.KeyValidated2 != 114)
            {
                e.Graphics.Clip = rgn;

                PaintLicenseText(e.Graphics,
                    "DotNetBar GaugeControl license not found. Please purchase a license at http://www.devcomponents.com",
                    10, Color.Gray, Color.Red, 255);
            }
#endif
        }

        #endregion

        #region PaintLicenseText

        private void PaintLicenseText(Graphics g,
            string text, int fontSize, Color c1, Color c2, int alpha)
        {
            using (StringFormat sf = new StringFormat(StringFormat.GenericDefault))
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                Rectangle r = ClientRectangle;

                using (Font font = new Font(Font.FontFamily, fontSize))
                {
                    using (Brush br = new SolidBrush(Color.FromArgb(alpha, c1)))
                        g.DrawString(text, font, br, r, sf);

                    r.X -= 1;
                    r.Y -= 1;

                    using (Brush br = new SolidBrush(Color.FromArgb(alpha, c2)))
                        g.DrawString(text, font, br, r, sf);
                }
            }
        }

        #endregion

        #region PaintScaleItems

        private void PaintScaleItems(PaintEventArgs e)
        {
            if (_CircularScales != null)
            {
                foreach (GaugeCircularScale scale in _CircularScales)
                {
                    if (scale.Visible == true)
                        scale.OnPaint(e);
                }
            }

            if (_LinearScales != null)
            {
                foreach (GaugeLinearScale scale in _LinearScales)
                {
                    if (scale.Visible == true)
                        scale.OnPaint(e);
                }
            }
        }

        #endregion

        #region PaintGaugeItems

        private void PaintGaugeItems(PaintEventArgs e, bool under)
        {
            if (_GaugeItems != null)
            {
                foreach (GaugeItem item in _GaugeItems)
                {
                    if (item.Visible == true)
                    {
                        if (item is GaugeText)
                        {
                            if (((GaugeText) item).UnderScale == under)
                                item.OnPaint(e);
                        }
                        else if (item is GaugeImage)
                        {
                            if (((GaugeImage) item).UnderScale == under)
                                item.OnPaint(e);
                        }
                        else if (item is GaugeIndicator)
                        {
                            if (((GaugeIndicator)item).UnderScale == under)
                                item.OnPaint(e);
                        }
                    }
                }
            }
        }

        #endregion

        #region PaintPointerItems

        private void PaintPointerItems(PaintEventArgs e, bool under)
        {
            if (_CircularScales != null)
            {
                foreach (GaugeCircularScale scale in _CircularScales)
                {
                    if (scale.Visible == true)
                        scale.PaintPointers(e, under);
                }
            }

            if (_LinearScales != null)
            {
                foreach (GaugeLinearScale scale in _LinearScales)
                {
                    if (scale.Visible == true)
                        scale.PaintPointers(e, under);
                }
            }
        }

        #endregion

        #region OnEvent processing

        #region Gauge events

        #region OnResize

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            NeedRecalcLayout = true;

            _Frame.RecalcLayout();
        }

        #endregion

        #region OnGaugeChanged

        protected void OnGaugeChanged()
        {
            Invalidate();
        }

        #endregion

        #region OnLayoutChanged

        private void OnLayoutChanged()
        {
            _Frame.NeedRecalcLayout = true;

            if (_CircularScales != null)
            {
                foreach (GaugeCircularScale item in _CircularScales)
                    item.NeedRecalcLayout = true;
            }

            if (_LinearScales != null)
            {
                foreach (GaugeLinearScale item in _LinearScales)
                    item.NeedRecalcLayout = true;
            }

            if (_GaugeItems != null)
            {
                foreach (GaugeItem item in _GaugeItems)
                    item.NeedRecalcLayout = true;
            }
        }

        #endregion

        #region OnPreContentRender

        private void OnPreContentRender(PaintEventArgs e)
        {
            if (PreRenderGaugeContent != null)
            {
                RenderGaugeContentEventArgs args =
                    new RenderGaugeContentEventArgs(e.Graphics, _Frame.BackBounds);

                PreRenderGaugeContent(this, args);

                if (args.Cancel == true)
                    return;
            }

            _Frame.PreRenderContent(e);
        }

        #endregion

        #region OnPostContentRender

        private void OnPostContentRender(PaintEventArgs e)
        {
            if (PostRenderGaugeContent != null)
            {
                RenderGaugeContentEventArgs args =
                    new RenderGaugeContentEventArgs(e.Graphics, _Frame.BackBounds);

                PostRenderGaugeContent(this, args);

                if (args.Cancel == true)
                    return;
            }

            _Frame.PostRenderContent(e);
        }

        #endregion

        #endregion

        #region Scale events

        #region OnPreRenderScale

        internal bool OnPreRenderScale(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScale != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScale(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScale

        internal void OnPostRenderScale(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScale != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScale(this, args);
            }
        }

        #endregion

        #endregion

        #region Range events

        #region OnPreRenderScaleRanges

        internal bool OnPreRenderScaleRanges(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScaleRanges != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScaleRanges(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleRanges

        internal void OnPostRenderScaleRanges(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScaleRanges != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScaleRanges(this, args);
            }
        }

        #endregion

        #region OnPreRenderScaleRange

        internal bool OnPreRenderScaleRange(PaintEventArgs e, GaugeRange range)
        {
            if (PreRenderScaleRange != null)
            {
                PreRenderScaleRangeEventArgs args =
                    new PreRenderScaleRangeEventArgs(e.Graphics, range);

                PreRenderScaleRange(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleRange

        internal void OnPostRenderScaleRange(PaintEventArgs e, GaugeRange range)
        {
            if (PostRenderScaleRange != null)
            {
                PostRenderScaleRangeEventArgs args =
                    new PostRenderScaleRangeEventArgs(e.Graphics, range);

                PostRenderScaleRange(this, args);
            }
        }

        #endregion

        #endregion

        #region Section events

        #region OnPreRenderScaleSections

        internal bool OnPreRenderScaleSections(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScaleSections != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScaleSections(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleSections

        internal void OnPostRenderScaleSections(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScaleSections != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScaleSections(this, args);
            }
        }

        #endregion

        #region OnPreRenderScaleSection

        internal bool OnPreRenderScaleSection(PaintEventArgs e, GaugeSection section)
        {
            if (PreRenderScaleSection != null)
            {
                PreRenderScaleSectionEventArgs args =
                    new PreRenderScaleSectionEventArgs(e.Graphics, section);

                PreRenderScaleSection(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleSection

        internal void OnPostRenderScaleSection(PaintEventArgs e, GaugeSection section)
        {
            if (PostRenderScaleSection != null)
            {
                PostRenderScaleSectionEventArgs args =
                    new PostRenderScaleSectionEventArgs(e.Graphics, section);

                PostRenderScaleSection(this, args);
            }
        }

        #endregion

        #endregion

        #region Pointer events

        #region OnPreRenderScalePointers

        internal bool OnPreRenderScalePointers(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScalePointers != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScalePointers(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScalePointers

        internal void OnPostRenderScalePointers(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScalePointers != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScalePointers(this, args);
            }
        }

        #endregion

        #region OnPreRenderScalePointer

        internal bool OnPreRenderScalePointer(PaintEventArgs e, GaugePointer pointer)
        {
            if (PreRenderScalePointer != null)
            {
                PreRenderScalePointerEventArgs args =
                    new PreRenderScalePointerEventArgs(e.Graphics, pointer);

                PreRenderScalePointer(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScalePointer

        internal void OnPostRenderScalePointer(PaintEventArgs e, GaugePointer pointer)
        {
            if (PostRenderScalePointer != null)
            {
                PostRenderScalePointerEventArgs args =
                    new PostRenderScalePointerEventArgs(e.Graphics, pointer);

                PostRenderScalePointer(this, args);
            }
        }

        #endregion

        #region OnScaleEnter

        internal void OnScaleEnter(GaugePointer pointer, GaugeScale scale)
        {
            if (ScaleEnter != null)
                ScaleEnter(this, new ScaleEnterEventArgs(pointer, scale));
        }

        #endregion

        #region OnScaleExit

        internal void OnScaleExit(GaugePointer pointer, GaugeScale scale)
        {
            if (ScaleLeave != null)
                ScaleLeave(this, new ScaleLeaveEventArgs(pointer, scale));
        }

        #endregion

        #region OnSectionEnter

        internal void OnSectionEnter(GaugePointer pointer, GaugeSection section)
        {
            if (SectionEnter != null)
                SectionEnter(this, new SectionEnterEventArgs(pointer, section));
        }

        #endregion

        #region OnSectionExit

        internal void OnSectionExit(GaugePointer pointer, GaugeSection section)
        {
            if (SectionLeave != null)
                SectionLeave(this, new SectionLeaveEventArgs(pointer, section));
        }

        #endregion

        #region OnRangeEnter

        internal void OnRangeEnter(GaugePointer pointer, GaugeRange range)
        {
            if (RangeEnter != null)
                RangeEnter(this, new RangeEnterEventArgs(pointer, range));
        }

        #endregion

        #region OnRangeExit

        internal void OnRangeExit(GaugePointer pointer, GaugeRange range)
        {
            if (RangeLeave != null)
                RangeLeave(this, new RangeLeaveEventArgs(pointer, range));
        }

        #endregion

        #region OnPointerChanging

        internal bool OnPointerChanging(GaugePointer pointer, double oldValue, ref double newValue)
        {
            if (PointerChanging != null)
            {
                PointerChangingEventArgs args =
                    new PointerChangingEventArgs(pointer, oldValue, newValue);

                PointerChanging(this, args);

                newValue = args.NewValue;

                return (args.Cancel);
            }

            return (false);
        }

        #endregion

        #region OnPointerChanged

        internal void OnPointerChanged(GaugePointer pointer, double oldValue, double newValue)
        {
            if (PointerChanged != null)
            {
                PointerChangedEventArgs args =
                    new PointerChangedEventArgs(pointer, oldValue, newValue);

                PointerChanged(this, args);
            }
        }

        #endregion

        #region OnPointerValueChanged

        internal void OnPointerValueChanged(GaugePointer pointer, double oldValue, double newValue)
        {
            if (PointerValueChanged != null)
            {
                PointerChangedEventArgs args =
                    new PointerChangedEventArgs(pointer, oldValue, newValue);

                PointerValueChanged(this, args);
            }
        }

        #endregion

        #region OnGetPointerPath

        internal GraphicsPath OnGetPointerPath(GaugePointer pointer, Rectangle bounds)
        {
            if (GetPointerPath != null)
            {
                GetPointerPathEventArgs e =
                    new GetPointerPathEventArgs(pointer, bounds);

                GetPointerPath(this, e);

                return (e.Path);
            }

            return (null);
        }

        #endregion

        #endregion

        #region TickMark events

        #region OnPreRenderScaleTickMarks

        internal bool OnPreRenderScaleTickMarks(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScaleTickMarks != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScaleTickMarks(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleTickMarks

        internal void OnPostRenderScaleTickMarks(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScaleTickMarks != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScaleTickMarks(this, args);
            }
        }

        #endregion

        #endregion

        #region GaugePin events

        #region OnPreRenderScaleGaugePin

        internal bool OnPreRenderScaleGaugePin(PaintEventArgs e, GaugePin gaugePin)
        {
            if (PreRenderScaleGaugePin != null)
            {
                PreRenderScaleGaugePinEventArgs args =
                    new PreRenderScaleGaugePinEventArgs(e.Graphics, gaugePin);

                PreRenderScaleGaugePin(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleGaugePin

        internal void OnPostRenderScaleGaugePin(PaintEventArgs e, GaugePin gaugePin)
        {
            if (PostRenderScaleGaugePin != null)
            {
                PostRenderScaleGaugePinEventArgs args =
                    new PostRenderScaleGaugePinEventArgs(e.Graphics, gaugePin);

                PostRenderScaleGaugePin(this, args);
            }
        }

        #endregion

        #endregion

        #region TickMarkLabels events

        #region OnPreRenderScaleTickMarkLabels

        internal bool OnPreRenderScaleTickMarkLabels(PaintEventArgs e, GaugeScale scale)
        {
            if (PreRenderScaleTickMarkLabels != null)
            {
                PreRenderScaleEventArgs args =
                    new PreRenderScaleEventArgs(e.Graphics, scale);

                PreRenderScaleTickMarkLabels(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleTickMarkLabels

        internal void OnPostRenderScaleTickMarkLabels(PaintEventArgs e, GaugeScale scale)
        {
            if (PostRenderScaleTickMarkLabels != null)
            {
                PostRenderScaleEventArgs args =
                    new PostRenderScaleEventArgs(e.Graphics, scale);

                PostRenderScaleTickMarkLabels(this, args);
            }
        }

        #endregion

        #endregion

        #region CustomLabel events

        #region OnPreRenderScaleCustomLabel

        internal bool OnPreRenderScaleCustomLabel(PaintEventArgs e, GaugeCustomLabel customLabel)
        {
            if (PreRenderScaleCustomLabel != null)
            {
                PreRenderScaleCustomLabelEventArgs args =
                    new PreRenderScaleCustomLabelEventArgs(e.Graphics, customLabel);

                PreRenderScaleCustomLabel(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderScaleCustomLabel

        internal void OnPostRenderScaleCustomLabel(PaintEventArgs e, GaugeCustomLabel customLabel)
        {
            if (PostRenderScaleCustomLabel != null)
            {
                PostRenderScaleCustomLabelEventArgs args =
                    new PostRenderScaleCustomLabelEventArgs(e.Graphics, customLabel);

                PostRenderScaleCustomLabel(this, args);
            }
        }

        #endregion

        #endregion

        #region GetDisplayTemplateText

        internal string OnGetDisplayTemplateText(
            GaugeItem item, string displayTemplate, string displayFormat)
        {
            if (GetDisplayTemplateText != null)
            {
                GetDisplayTemplateTextEventArgs e =
                    new GetDisplayTemplateTextEventArgs(item, displayTemplate, displayFormat);

                GetDisplayTemplateText(this, e);

                return (e.DisplayText);
            }

            return (displayTemplate);
        }

        #endregion

        #region Indicator events

        #region OnPreRenderIndicator

        internal bool OnPreRenderIndicator(PaintEventArgs e, GaugeIndicator indicator)
        {
            if (PreRenderIndicator != null)
            {
                PreRenderIndicatorEventArgs args =
                    new PreRenderIndicatorEventArgs(e.Graphics, indicator);

                PreRenderIndicator(this, args);

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderIndicator

        internal void OnPostRenderIndicator(PaintEventArgs e, GaugeIndicator indicator)
        {
            if (PostRenderIndicator != null)
            {
                PostRenderIndicatorEventArgs args =
                    new PostRenderIndicatorEventArgs(e.Graphics, indicator);

                PostRenderIndicator(this, args);
            }
        }

        #endregion

        #region OnPreRenderIndicatorDigit

        internal bool OnPreRenderIndicatorDigit(
            PaintEventArgs e, NumericIndicator indicator, NumericElement digit, int index)
        {
            if (PreRenderIndicatorDigit != null)
            {
                PreRenderIndicatorDigitEventArgs args =
                    new PreRenderIndicatorDigitEventArgs(e.Graphics, indicator, digit, index);

                try
                {
                    digit.InRenderCallout = true;

                    PreRenderIndicatorDigit(this, args);
                }
                finally
                {
                    digit.InRenderCallout = false;
                }

                if (args.Cancel == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region OnPostRenderIndicatorDigit

        internal void OnPostRenderIndicatorDigit(
            PaintEventArgs e, NumericIndicator indicator, NumericElement digit, int index)
        {
            if (PostRenderIndicatorDigit != null)
            {
                PostRenderIndicatorDigitEventArgs args =
                    new PostRenderIndicatorDigitEventArgs(e.Graphics, indicator, digit, index);

                try
                {
                    digit.InRenderCallout = true;

                    PostRenderIndicatorDigit(this, args);
                }
                finally
                {
                    digit.InRenderCallout = false;
                }
            }
        }

        #endregion

        #region OnGetDigitSegments

        internal int OnGetDigitSegments(NumericIndicator indicator, char digit, int segments)
        {
            if (GetDigitSegments != null)
            {
                GetDigitSegmentsEventArgs e =
                    new GetDigitSegmentsEventArgs(indicator, digit, segments);

                GetDigitSegments(this, e);

                return (e.Segments);
            }

            return (segments);
        }

        #endregion

        #endregion

        #endregion

        #region Pointer Dampening support

        #region StartDampening

        internal void StartDampening()
        {
            if (_DampWorker == null)
            {
                _DampWorker = new BackgroundWorker();

                _DampWorker.WorkerSupportsCancellation = true;
                _DampWorker.DoWork += DampWorkerDoWork;

                _DampWorker.RunWorkerAsync();
            }

            _DampCount++;
        }

        #endregion

        #region StopDampening

        public void StopDampening()
        {
            if (_DampCount > 0)
            {
                _DampCount--;

                if (_DampCount == 0)
                {
                    if (_DampWorker != null)
                    {
                        BackgroundWorker worker = _DampWorker;
                        _DampWorker = null;

                        worker.CancelAsync();
                        worker.DoWork -= DampWorkerDoWork;
                        worker.Dispose();
                    }
                }
            }
        }

        #endregion

        #region DampWorkerDoWork

        void DampWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            while (!worker.CancellationPending)
            {
                EventHandler<EventArgs> eh = DampeningUpdate;
                if (eh != null)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        BeginUpdate();
                        eh(this, EventArgs.Empty);
                        EndUpdate(_DValueChange);
                    }
                    ));

                    _DValueChange = false;
                }

                Thread.Sleep(20);
            }
        }

        #endregion

        #endregion

        #region Mouse processing

        #region OnMouseMove

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_MouseDown == true)
            {
                if (_HotItem != null)
                    _HotItem.OnMouseMove(e, true);
            }
            else
            {
                GaugeItem item = GetGaugeItemFromPoint(e.Location);

                if (item != null)
                {
                    if (_HotItem != item)
                    {
                        if (_HotItem != null)
                            _HotItem.OnMouseLeave();

                        Cursor = Cursors.Default;

                        item.OnMouseEnter();

                        if (_ShowToolTips == true)
                            _ToolTip.SetToolTip(this, item.GetitemTemplateText(this));
                    }

                    item.OnMouseMove(e, false);
                }
                else
                {
                    if (_HotItem != null)
                    {
                        _HotItem.OnMouseLeave();

                        _ToolTip.SetToolTip(this, "");
                    }

                    Cursor = Cursors.Default;
                }

                _HotItem = item;
            }
        }

        #endregion

        #region OnMouseDown

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _MouseDown = true;

            if (_HotItem != null)
                _HotItem.OnMouseDown(e);

#if !TRIAL
            if (Licensing.KeyValidated2 != 114 && _DialogDisplayed == false)
            {
                RemindForm f = new RemindForm();

                f.ShowDialog();
                f.Dispose();

                _DialogDisplayed = true;
            }
#endif
        }

        #endregion

        #region OnMouseUp

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_MouseDown == true)
            {
                _MouseDown = false;

                if (_HotItem != null)
                {
                    if (_ShowToolTips == true)
                        _ToolTip.SetToolTip(this, _HotItem.GetitemTemplateText(this));

                    _HotItem.OnMouseUp(e);
                }
            }
        }

        #endregion

        #endregion

        #region GetAbsPoint

        internal Point GetAbsPoint(PointF ptf, bool minSize)
        {
            int width = Width;
            int height = Height;

            Point pt = Point.Empty;

            if (minSize == true)
            {
                if (width > height)
                {
                    pt.X = (width - height) / 2;
                    width = height;
                }
                else
                {
                    pt.Y = (height - width) / 2;
                    height = width;
                }
            }

            pt.X += (int)(width * ptf.X);
            pt.Y += (int)(height * ptf.Y);

            return (pt);
        }

        #endregion

        #region GetAbsSize

        internal Size GetAbsSize(SizeF sf, bool minSize)
        {
            if (NeedRecalcLayout == true)
                throw new Exception();

            int width = Width;
            int height = Height;

            if (minSize == true)
            {
                if (width > height)
                    width = height;
                else
                    height = width;
            }

            Size size = new Size((int)(width * sf.Width), (int)(height * sf.Height));

            if (size.Width <= 0)
                size.Width = 1;

            if (size.Height <= 0)
                size.Height = 1;

            return (size);
        }

        #endregion

        #region GetPointerValue

        /// <summary>
        /// Gets the named Pointer Value.
        /// If the Pointer is not defined, an exception is thrown.
        /// </summary>
        /// <param name="pointerName">Pointer Name</param>
        /// <returns>Value</returns>
        public double GetPointerValue(string pointerName)
        {
            GaugePointer pointer = GetPointer(pointerName);

            if (pointer != null)
                return (pointer.Value);

            throw new Exception("Pointer (" + pointerName + ") is not defined.");
        }

        /// <summary>
        /// Gets the named Scale:Pointer Value.
        /// If either the Scale or the Pointer is not defined,
        /// an exception is thrown.
        /// </summary>
        /// <param name="scaleName">Scale Name</param>
        /// <param name="pointerName">Pointer Name</param>
        /// <returns></returns>
        public double GetPointerValue(string scaleName, string pointerName)
        {
            GaugePointer pointer = GetPointer(scaleName, pointerName);

            if (pointer != null)
                return (pointer.Value);

            throw new Exception("Scale/Pointer (" + scaleName + "/" + pointerName + ") is not defined.");
        }

        #endregion

        #region SetPointerValue

        #region SetPointerValue (pointerName, value[, dampen])

        /// <summary>
        /// Sets the named Pointer Value to the given value.
        /// An exception is thrown if the Pointer is not defined.
        /// </summary>
        /// <param name="pointerName">Pointer Name</param>
        /// <param name="value">Value to set</param>
        public void SetPointerValue(string pointerName, double value)
        {
            SetPointerValue(pointerName, value, true);
        }

        /// <summary>
        /// Sets the named Pointer Value to the given value.
        /// An exception is thrown if the Pointer is not defined.
        /// </summary>
        /// <param name="pointerName">Pointer Name</param>
        /// <param name="value">Value to set</param>
        /// <param name="dampen">Indicates whether to dampen the resultant output</param>
        public void SetPointerValue(string pointerName, double value, bool dampen)
        {
            GaugePointer pointer = GetPointer(pointerName);

            if (pointer != null)
            {
                if (dampen == true)
                    pointer.Value = value;
                else
                    pointer.ValueEx = value;
            }
            else
            {
                throw new Exception("Pointer (" + pointerName + ") is not defined.");
            }
        }

        #endregion

        #region SetPointerValue (scaleName, pointerName[, dampen])

        /// <summary>
        /// Sets the named Scale:Pointer Value to the given value.
        /// An exception is thrown if the Scale or Pointer is not defined.
        /// </summary>
        /// <param name="scaleName">Scale name</param>
        /// <param name="pointerName">Pointer name</param>
        /// <param name="value">Value to set</param>
        public void SetPointerValue(string scaleName, string pointerName, double value)
        {
            SetPointerValue(scaleName, pointerName, value, true);
        }

        /// <summary>
        /// Sets the named Scale:Pointer Value to the given value.
        /// An exception is thrown if the Scale or Pointer is not defined.
        /// </summary>
        /// <param name="scaleName">Scale name</param>
        /// <param name="pointerName">Pointer name</param>
        /// <param name="value">Value to set</param>
        /// <param name="dampen">Indicates whether to dampen the resultant output</param>
        public void SetPointerValue(string scaleName, string pointerName, double value, bool dampen)
        {
            GaugePointer pointer = GetPointer(scaleName, pointerName);

            if (pointer != null)
            {
                if (dampen == true)
                    pointer.Value = value;
                else
                    pointer.ValueEx = value;
            }
            else
            {
                throw new Exception("Scale/Pointer (" + scaleName + "/" + pointerName + ") is not defined.");
            }
        }

        #endregion

        #endregion

        #region GetPointer

        #region GetPointer (pointerName)

        /// <summary>
        /// Gets the named Pointer.
        /// </summary>
        /// <param name="pointerName">Pointer name</param>
        /// <returns>Pointer, or null</returns>
        public GaugePointer GetPointer(string pointerName)
        {
            if (_CircularScales != null)
            {
                foreach (GaugeCircularScale scale in _CircularScales)
                {
                    GaugePointer pointer = scale.Pointers[pointerName];

                    if (pointer != null)
                        return (pointer);
                }
            }

            if (_LinearScales != null)
            {
                foreach (GaugeLinearScale scale in _LinearScales)
                {
                    GaugePointer pointer = scale.Pointers[pointerName];

                    if (pointer != null)
                        return (pointer);
                }
            }

            return (null);
        }

        #endregion

        #region GetPointer (scaleName, pointerName)

        /// <summary>
        /// Gets the named Scale:Pointer.
        /// </summary>
        /// <param name="scaleName">Scale name</param>
        /// <param name="pointerName">Pointer name</param>
        /// <returns>Pointer, or null</returns>
        public GaugePointer GetPointer(string scaleName, string pointerName)
        {
            if (_CircularScales != null)
            {
                GaugeScale scale = _CircularScales[scaleName];

                if (scale != null)
                {
                    GaugePointer pointer = scale.Pointers[pointerName];

                    if (pointer != null)
                        return (pointer);
                }
            }

            if (_LinearScales != null)
            {
                GaugeScale scale = _LinearScales[scaleName];

                if (scale != null)
                {
                    GaugePointer pointer = scale.Pointers[pointerName];

                    if (pointer != null)
                        return (pointer);
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #region GetGaugeItemFromPoint

        /// <summary>
        /// Gets the GaugeItem from the given Point
        /// </summary>
        /// <param name="pt">Point</param>
        /// <returns>GaugeItem, or null</returns>
        public GaugeItem GetGaugeItemFromPoint(Point pt)
        {
            GaugeItem item;

            if ((item = FindCircularPointer(pt)) != null)
                return (item);

            if ((item = FindLinearPointer(pt)) != null)
                return (item);

            if ((item = FindCircularItem(pt)) != null)
                return (item);

            if ((item = FindLinearItem(pt)) != null)
                return (item);

            if ((item = FindGaugeItem(pt)) != null)
                return (item);

            return (null);
        }

        #region FindCircularPointer

        private GaugeItem FindCircularPointer(Point pt)
        {
            if (_CircularScales != null)
            {
                for (int i = _CircularScales.Count - 1; i >= 0; i--)
                {
                    GaugeCircularScale scale = _CircularScales[i];

                    if (scale.Visible == true)
                    {
                        GaugeItem item = scale.FindPointerItem(pt);

                        if (item != null)
                            return (item);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindLinearPointer

        private GaugeItem FindLinearPointer(Point pt)
        {
            if (_LinearScales != null)
            {
                for (int i = _LinearScales.Count - 1; i >= 0; i--)
                {
                    GaugeLinearScale scale = _LinearScales[i];

                    if (scale.Visible == true)
                    {
                        GaugeItem item = scale.FindPointerItem(pt);

                        if (item != null)
                            return (item);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindCircularItem

        private GaugeItem FindCircularItem(Point pt)
        {
            if (_CircularScales != null)
            {
                for (int i = _CircularScales.Count - 1; i >= 0; i--)
                {
                    GaugeCircularScale scale = _CircularScales[i];

                    if (scale.Visible == true)
                    {
                        GaugeItem item = scale.FindItem(pt);

                        if (item != null)
                            return (item);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindLinearItem

        private GaugeItem FindLinearItem(Point pt)
        {
            if (_LinearScales != null)
            {
                for (int i = _LinearScales.Count - 1; i >= 0; i--)
                {
                    GaugeLinearScale scale = _LinearScales[i];

                    if (scale.Visible == true)
                    {
                        GaugeItem item = scale.FindItem(pt);

                        if (item != null)
                            return (item);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region FindGaugeItem

        private GaugeItem FindGaugeItem(Point pt)
        {
            if (_GaugeItems != null)
            {
                for (int i = _GaugeItems.Count - 1; i >= 0; i--)
                {
                    GaugeItem item = _GaugeItems[i];

                    if (item.Visible == true)
                    {
                        if (item is GaugeText)
                        {
                            if (((GaugeText) item).Contains(pt))
                                return (item);
                        }

                        if (item is GaugeImage)
                        {
                            if (((GaugeImage) item).Contains(pt))
                                return (item);
                        }

                        if (item is GaugeIndicator)
                        {
                            if (((GaugeIndicator)item).Contains(pt))
                                return (item);
                        }
                    }
                }
            }

            return (null);
        }

        #endregion

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            HookEvents(false);

            Dispose();
        }

        #endregion

        #region ISupportInitialize Members

        public void BeginInit()
        {
            _InitComplete = false;
        }

        public void EndInit()
        {
            if (_CircularScales != null)
            {
                foreach (GaugeCircularScale scale in _CircularScales)
                {
                    if (scale.Visible == true)
                        scale.UpdateValueData();
                }
            }

            if (_LinearScales != null)
            {
                foreach (GaugeLinearScale scale in _LinearScales)
                {
                    if (scale.Visible == true)
                        scale.UpdateValueData();
                }
            }

            _InitComplete = true;
        }

        #endregion
    }

    #region RenderGaugeContentEventArgs

    /// <summary>
    /// RenderGaugeContentEventArgs
    /// </summary>
    public class RenderGaugeContentEventArgs : CancelEventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private Rectangle _Bounds;

        #endregion

        public RenderGaugeContentEventArgs(Graphics graphics, Rectangle bounds)
        {
            _Graphics = graphics;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// Gauge Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        #endregion
    }

    #endregion

    #region PreRenderScaleEventArgs

    /// <summary>
    /// PreRenderScaleEventArgs
    /// </summary>
    public class PreRenderScaleEventArgs : PostRenderScaleEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScaleEventArgs(Graphics graphics, GaugeScale scale)
            : base(graphics, scale)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScaleEventArgs

    /// <summary>
    /// PostRenderScaleEventArgs
    /// </summary>
    public class PostRenderScaleEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GaugeScale _Scale;

        #endregion

        public PostRenderScaleEventArgs(Graphics graphics, GaugeScale scale)
        {
            _Graphics = graphics;
            _Scale = scale;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugeScale
        /// </summary>
        public GaugeScale Scale
        {
            get { return (_Scale); }
        }

        #endregion
    }

    #endregion

    #region PreRenderScaleRangeEventArgs

    /// <summary>
    /// PreRenderScaleRangeEventArgs
    /// </summary>
    public class PreRenderScaleRangeEventArgs : PostRenderScaleRangeEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScaleRangeEventArgs(Graphics graphics, GaugeRange range)
            : base(graphics, range)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScaleRangeEventArgs

    /// <summary>
    /// PostRenderScaleRangeEventArgs
    /// </summary>
    public class PostRenderScaleRangeEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;

        private GaugeRange _Range;
        private Rectangle _Bounds;

        private float _StartAngle;
        private float _SweepAngle;

        #endregion
        
        public PostRenderScaleRangeEventArgs(Graphics graphics, GaugeRange range)
        {
            _Graphics = graphics;

            _Range = range;
            _Bounds = range.Bounds;

            _StartAngle = range.StartAngle;
            _SweepAngle = range.SweepAngle;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugeRange
        /// </summary>
        public GaugeRange Range
        {
            get { return (_Range); }
        }

        /// <summary>
        /// GaugeRange Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Range StartAngle
        /// </summary>
        public float StartAngle
        {
            get { return (_StartAngle); }
        }

        /// <summary>
        /// Range SweepAngle
        /// </summary>
        public float SweepAngle
        {
            get { return (_SweepAngle); }
        }

        #endregion

    }

    #endregion

    #region PreRenderScaleSectionEventArgs

    /// <summary>
    /// PreRenderScaleSectionEventArgs
    /// </summary>
    public class PreRenderScaleSectionEventArgs : PostRenderScaleSectionEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScaleSectionEventArgs(Graphics graphics, GaugeSection section)
            : base(graphics, section)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScaleSectionEventArgs

    /// <summary>
    /// PostRenderScaleSectionEventArgs
    /// </summary>
    public class PostRenderScaleSectionEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;

        private GaugeSection _Section;
        private Rectangle _Bounds;

        private float _StartAngle;
        private float _SweepAngle;

        #endregion

        public PostRenderScaleSectionEventArgs(Graphics graphics, GaugeSection section)
        {
            _Graphics = graphics;

            _Section = section;
            _Bounds = section.Bounds;

            _StartAngle = section.StartAngle;
            _SweepAngle = section.SweepAngle;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugeSection
        /// </summary>
        public GaugeSection Section
        {
            get { return (_Section); }
        }

        /// <summary>
        /// Section Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// Section StartAngle
        /// </summary>
        public float StartAngle
        {
            get { return (_StartAngle); }
        }

        /// <summary>
        /// Section SweepAngle
        /// </summary>
        public float SweepAngle
        {
            get { return (_SweepAngle); }
        }

        #endregion
    }

    #endregion

    #region PreRenderScalePointerEventArgs

    /// <summary>
    /// PreRenderScalePointerEventArgs
    /// </summary>
    public class PreRenderScalePointerEventArgs : PostRenderScalePointerEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScalePointerEventArgs(Graphics graphics, GaugePointer pointer)
            : base(graphics, pointer)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScalePointerEventArgs

    /// <summary>
    /// PostRenderScalePointerEventArgs
    /// </summary>
    public class PostRenderScalePointerEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GaugePointer _Pointer;

        #endregion
        
        public PostRenderScalePointerEventArgs(Graphics graphics, GaugePointer pointer)
        {
            _Graphics = graphics;

            _Pointer = pointer;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        #endregion
    }

    #endregion

    #region PreRenderScaleGaugePinEventArgs

    /// <summary>
    /// PreRenderScaleGaugePinEventArgs
    /// </summary>
    public class PreRenderScaleGaugePinEventArgs : PostRenderScaleGaugePinEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScaleGaugePinEventArgs(Graphics graphics, GaugePin gaugePin)
            : base(graphics, gaugePin)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScaleGaugePinEventArgs

    /// <summary>
    /// PostRenderScaleGaugePinEventArgs
    /// </summary>
    public class PostRenderScaleGaugePinEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GaugePin _GaugePin;

        #endregion
        
        public PostRenderScaleGaugePinEventArgs(Graphics graphics, GaugePin gaugePin)
        {
            _Graphics = graphics;
            _GaugePin = gaugePin;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugePin
        /// </summary>
        public GaugePin GaugePin
        {
            get { return (_GaugePin); }
        }

        #endregion
    }

    #endregion

    #region PreRenderScaleCustomLabelEventArgs

    /// <summary>
    /// PreRenderScaleCustomLabelEventArgs
    /// </summary>
    public class PreRenderScaleCustomLabelEventArgs : PostRenderScaleCustomLabelEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderScaleCustomLabelEventArgs(Graphics graphics, GaugeCustomLabel customLabel)
            : base(graphics, customLabel)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderScaleCustomLabelEventArgs

    /// <summary>
    /// PostRenderScaleCustomLabelEventArgs
    /// </summary>
    public class PostRenderScaleCustomLabelEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GaugeCustomLabel _CustomLabel;

        #endregion

        public PostRenderScaleCustomLabelEventArgs(Graphics graphics, GaugeCustomLabel customLabel)
        {
            _Graphics = graphics;
            _CustomLabel = customLabel;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// CustomLabel
        /// </summary>
        public GaugeCustomLabel CustomLabel
        {
            get { return (_CustomLabel); }
        }

        #endregion
    }

    #endregion

    #region PreRenderIndicatorEventArgs

    /// <summary>
    /// PreRenderIndicatorEventArgs
    /// </summary>
    public class PreRenderIndicatorEventArgs : PostRenderIndicatorEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderIndicatorEventArgs(Graphics graphics, GaugeIndicator indicator)
            : base(graphics, indicator)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderIndicatorEventArgs

    /// <summary>
    /// PostRenderIndicatorEventArgs
    /// </summary>
    public class PostRenderIndicatorEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private GaugeIndicator _Indicator;

        #endregion

        public PostRenderIndicatorEventArgs(Graphics graphics, GaugeIndicator indicator)
        {
            _Graphics = graphics;
            _Indicator = indicator;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugeIndicator
        /// </summary>
        public GaugeIndicator Indicator
        {
            get { return (_Indicator); }
        }

        #endregion
    }

    #endregion

    #region PreRenderIndicatorDigitEventArgs

    /// <summary>
    /// PreRenderIndicatorDigitEventArgs
    /// </summary>
    public class PreRenderIndicatorDigitEventArgs : PostRenderIndicatorDigitEventArgs
    {
        #region Private variables

        private bool _Cancel;

        #endregion

        public PreRenderIndicatorDigitEventArgs(
            Graphics graphics, NumericIndicator indicator, NumericElement digit, int index)
            : base(graphics, indicator, digit, index)
        {
        }

        #region Public properties

        /// <summary>
        /// Cancel
        /// </summary>
        public bool Cancel
        {
            get { return (_Cancel); }
            set { _Cancel = value; }
        }

        #endregion
    }

    #endregion

    #region PostRenderIndicatorEventArgs

    /// <summary>
    /// PostRenderIndicatorDigitEventArgs
    /// </summary>
    public class PostRenderIndicatorDigitEventArgs : EventArgs
    {
        #region Private variables

        private Graphics _Graphics;
        private NumericIndicator _Indicator;
        private NumericElement _Digit;
        private int _Index;

        #endregion

        public PostRenderIndicatorDigitEventArgs(
            Graphics graphics, NumericIndicator indicator, NumericElement digit, int index)
        {
            _Graphics = graphics;
            _Indicator = indicator;
            _Digit = digit;
            _Index = index;
        }

        #region Public properties

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics Graphics
        {
            get { return (_Graphics); }
        }

        /// <summary>
        /// GaugeIndicator
        /// </summary>
        public NumericIndicator Indicator
        {
            get { return (_Indicator); }
        }

        /// <summary>
        /// NumericElement / digit
        /// </summary>
        public NumericElement Digit
        {
            get { return (_Digit); }
        }

        /// <summary>
        /// NumericElement / digit index (from left to right)
        /// </summary>
        public int Index
        {
            get { return (_Index); }
        }

        #endregion
    }

    #endregion

    #region ScaleEnterEventArgs

    /// <summary>
    /// ScaleEnterEventArgs
    /// </summary>
    public class ScaleEnterEventArgs : EventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;
        private GaugeScale _Scale;

        #endregion

        public ScaleEnterEventArgs(GaugePointer pointer, GaugeScale scale)
        {
            _Pointer = pointer;
            _Scale = scale;
        }

        #region Public properties

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        /// <summary>
        /// GaugeScale
        /// </summary>
        public GaugeScale Scale
        {
            get { return (_Scale); }
        }

        #endregion
    }

    #endregion

    #region ScaleLeaveEventArgs

    /// <summary>
    /// ScaleLeaveEventArgs
    /// </summary>
    public class ScaleLeaveEventArgs : ScaleEnterEventArgs
    {
        public ScaleLeaveEventArgs(GaugePointer pointer, GaugeScale scale)
            : base(pointer, scale)
        {
        }
    }

    #endregion

    #region SectionEnterEventArgs

    /// <summary>
    /// SectionEnterEventArgs
    /// </summary>
    public class SectionEnterEventArgs : EventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;
        private GaugeSection _Section;

        #endregion

        public SectionEnterEventArgs(GaugePointer pointer, GaugeSection section)
        {
            _Pointer = pointer;
            _Section = section;
        }

        #region Public properties

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        /// <summary>
        /// GaugeSection
        /// </summary>
        public GaugeSection Section
        {
            get { return (_Section); }
        }

        #endregion
    }

    #endregion

    #region SectionLeaveEventArgs

    /// <summary>
    /// SectionLeaveEventArgs
    /// </summary>
    public class SectionLeaveEventArgs : SectionEnterEventArgs
    {
        public SectionLeaveEventArgs(GaugePointer pointer, GaugeSection section)
            : base(pointer, section)
        {
        }
    }

    #endregion

    #region RangeEnterEventArgs

    /// <summary>
    /// RangeEnterEventArgs
    /// </summary>
    public class RangeEnterEventArgs : EventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;
        private GaugeRange _Range;

        #endregion

        public RangeEnterEventArgs(GaugePointer pointer, GaugeRange range)
        {
            _Pointer = pointer;
            _Range = range;
        }

        #region Public properties

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        /// <summary>
        /// GaugeRange
        /// </summary>
        public GaugeRange Range
        {
            get { return (_Range); }
        }

        #endregion
    }

    #endregion

    #region RangeLeaveEventArgs

    /// <summary>
    /// RangeLeaveEventArgs
    /// </summary>
    public class RangeLeaveEventArgs : RangeEnterEventArgs
    {
        public RangeLeaveEventArgs(GaugePointer pointer, GaugeRange range)
            : base(pointer, range)
        {
        }
    }

    #endregion

    #region PointerChangingEventArgs

    /// <summary>
    /// PointerChangingEventArgs
    /// </summary>
    public class PointerChangingEventArgs : CancelEventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;

        private double _OldValue;
        private double _NewValue;

        #endregion

        public PointerChangingEventArgs(
            GaugePointer pointer, double oldValue, double newValue)
        {
            _Pointer = pointer;

            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        /// <summary>
        /// OldValue
        /// </summary>
        public double OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// NewValue
        /// </summary>
        public double NewValue
        {
            get { return (_NewValue); }
            set { _NewValue = value; }
        }

        #endregion
    }

    #endregion

    #region PointerChangedEventArgs

    /// <summary>
    /// PointerChangedEventArgs
    /// </summary>
    public class PointerChangedEventArgs : EventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;

        private double _OldValue;
        private double _NewValue;

        #endregion

        public PointerChangedEventArgs(
            GaugePointer pointer, double oldValue, double newValue)
        {
            _Pointer = pointer;

            _OldValue = oldValue;
            _NewValue = newValue;
        }

        #region Public properties

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        /// <summary>
        /// OldValue
        /// </summary>
        public double OldValue
        {
            get { return (_OldValue); }
        }

        /// <summary>
        /// NewValue
        /// </summary>
        public double NewValue
        {
            get { return (_NewValue); }
        }

        #endregion
    }

    #endregion

    #region GetPointerPathEventArgs

    /// <summary>
    /// GetPointerPathEventArgs
    /// </summary>
    public class GetPointerPathEventArgs : EventArgs
    {
        #region Private variables

        private GaugePointer _Pointer;
        private GraphicsPath _Path;
        private Rectangle _Bounds;

        #endregion

        public GetPointerPathEventArgs(
            GaugePointer pointer, Rectangle bounds)
        {
            _Pointer = pointer;
            _Bounds = bounds;
        }

        #region Public properties

        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
        }

        /// <summary>
        /// GraphicsPath
        /// </summary>
        public GraphicsPath Path
        {
            get { return (_Path); }
            set { _Path = value; }
        }

        /// <summary>
        /// GaugePointer
        /// </summary>
        public GaugePointer Pointer
        {
            get { return (_Pointer); }
        }

        #endregion
    }

    #endregion

    #region GetDisplayTemplateTextEventArgs

    /// <summary>
    /// GetDisplayTemplateTextEventArgs
    /// </summary>
    public class GetDisplayTemplateTextEventArgs : EventArgs
    {
        #region Private variables

        private GaugeItem _GaugeItem;

        private string _DisplayText;
        private string _DisplayTemplate;
        private string _DisplayFormat;

        #endregion

        public GetDisplayTemplateTextEventArgs(GaugeItem gaugeItem,
            string displayTemplate, string displayFormat)
        {
            _GaugeItem = gaugeItem;

            _DisplayText = displayTemplate;
            _DisplayTemplate = displayTemplate;
            _DisplayFormat = displayFormat;
        }

        #region Public properties

        /// <summary>
        /// Gets the GaugeItem
        /// </summary>
        public GaugeItem GaugeItem
        {
            get { return (_GaugeItem); }
        }

        /// <summary>
        /// Gets or sets the Display Text for the given DisplayTemplate
        /// </summary>
        public string DisplayText
        {
            get { return (_DisplayText); }
            set { _DisplayText = value; }
        }

        /// <summary>
        /// Gets the Display Template
        /// </summary>
        public string DisplayTemplate
        {
            get { return (_DisplayTemplate); }
        }

        /// <summary>
        /// Gets the Display Format for the given DisplayTemplate
        /// </summary>
        public string DisplayFormat
        {
            get { return (_DisplayFormat); }
        }

        #endregion
    }

    #endregion

    #region GetDigitSegmentsEventArgs

    /// <summary>
    /// GetDigitSegmentsEventArgs
    /// </summary>
    public class GetDigitSegmentsEventArgs : EventArgs
    {
        #region Private variables

        private NumericIndicator _Indicator;
        private char _Digit;
        private int _Segments;

        #endregion

        public GetDigitSegmentsEventArgs(
            NumericIndicator indicator, char digit, int segments)
        {
            _Indicator = indicator;
            _Digit = digit;
            _Segments = segments;
        }

        #region Public properties

        /// <summary>
        /// Digit
        /// </summary>
        public char Digit
        {
            get { return (_Digit); }
        }

        /// <summary>
        /// NumericIndicator
        /// </summary>
        public NumericIndicator Indicator
        {
            get { return (_Indicator); }
        }

        /// <summary>
        /// Segments pattern
        /// </summary>
        public int Segments
        {
            get { return (_Segments); }
            set { _Segments = value; }
        }

        #endregion
    }

    #endregion

}
