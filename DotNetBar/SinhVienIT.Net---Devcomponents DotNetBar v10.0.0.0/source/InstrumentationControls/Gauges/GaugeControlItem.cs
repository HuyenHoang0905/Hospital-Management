using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    public class GaugeControlItem
    {
        #region Private variables

        private GaugeControl _GaugeControl;
        private GaugeControlItem _Parent;

        private GaugeFrame _GaugeFrame;

        private GaugeScaleCollection _GaugeScaleItems;
        private GaugeImageCollection _GaugeImageItems;
        private GaugeTextCollection _GaugeTextItems;

        private int _SuspendCount;
        private bool _NeedRecalcLayout;

        private float _Height;
        private float _Width;
        private PointF _Location;

        private PointF _PivotPoint;
        private float _Radius;

        private Rectangle _AbsBounds;

        private bool _Visible;

        #endregion

        public GaugeControlItem()
        {
            _GaugeFrame = new GaugeFrame(this);
            _GaugeScaleItems = new GaugeScaleCollection();

            _Width = 1;
            _Height = 1;

            _PivotPoint = new PointF(.5f, .5f);
            _Radius = .5f;

            NeedRecalcLayout = true;

            HookEvents(true);
        }

        #region Public properties

        #region GaugeFrame

        /// <summary>
        /// Gets the Gauge Frame.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the Gauge Frame.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeFrame GaugeFrame
        {
            get { return (_GaugeFrame); }
        }

        #endregion

        #region GaugeScaleItems

        /// <summary>
        /// Gets the collection GaugeScaleItems contained within the gauge.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates the collection GaugeScaleItems contained within the gauge.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GaugeScaleCollection GaugeScaleItems
        {
            get { return (_GaugeScaleItems); }
        }

        #endregion

        #region Location

        public PointF Location
        {
            get { return (_Location); }

            set
            {
                if (_Location != value)
                {
                    _Location = value;

                    OnGaugeChanged();
                }
            }
        }

        #endregion

        #region Height

        public float Height
        {
            get { return (_Height); }

            set
            {
                if (_Height != value)
                {
                    _Height = value;

                    OnResize(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region PivotPoint

        public PointF PivotPoint
        {
            get { return (_PivotPoint); }

            set
            {
                if (_PivotPoint.Equals(value) == false)
                {
                    _PivotPoint = value;

                    OnGaugeChanged();
                }
            }
        }

        #endregion

        #region Radius

        public float Radius
        {
            get { return (_Radius); }

            set
            {
                if (_Radius != value)
                {
                    _Radius = value;

                    OnGaugeChanged();
                }
            }
        }

        #endregion

        #region Visible

        public bool Visible
        {
            get { return (_Visible); }

            set
            {
                if (_Visible != value)
                {
                    _Visible = value;

                    Refresh();
                }
            }
        }

        #endregion

        #region Width

        public float Width
        {
            get { return (_Width); }

            set
            {
                if (_Width != value)
                {
                    _Width = value;

                    OnResize(EventArgs.Empty);
                }
            }
        }

        #endregion

        #endregion

        #region Internal properties

        #region AbsBounds

        internal Rectangle AbsBounds
        {
            get { return (_AbsBounds); }
        }

        #endregion

        #region AbsHeight

        internal int AbsHeight
        {
            get { return (_AbsBounds.Height); }
        }

        #endregion

        #region AbsWidth

        internal int AbsWidth
        {
            get { return (_AbsBounds.Width); }
        }

        #endregion

        #region GaugeControl

        internal GaugeControl GaugeControl
        {
            get { return (_GaugeControl);  }
            set { _GaugeControl = value; }
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

        #region Parent

        internal GaugeControlItem Parent
        {
            get { return (_Parent); }
            set { _Parent = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _GaugeFrame.GaugeFrameChanged += GaugeFrame_GaugeFrameChanged;
            }
            else
            {
                _GaugeFrame.GaugeFrameChanged -= GaugeFrame_GaugeFrameChanged;
            }
        }

        #endregion

        #region Event processing

        void GaugeFrame_GaugeFrameChanged(object sender, EventArgs e)
        {
            OnGaugeChanged();
        }

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
            if (_SuspendCount > 0)
                _SuspendCount--;

            if (_SuspendCount == 0)
                Refresh();
        }

        #endregion

        #region Refresh

        private void Refresh()
        {
            if (_GaugeControl != null)
                _GaugeControl.Invalidate(_GaugeFrame.Bounds);
        }

        #endregion

        #region RecalcLayout

        private void RecalcLayout()
        {
            if (_NeedRecalcLayout == true)
            {
                NeedRecalcLayout = false;

                _AbsBounds = GetItemBounds();
            }
        }

        #endregion

        #region GetItemBounds

        private Rectangle GetItemBounds()
        {
            Rectangle r;

            if (_Parent != null)
            {
                if (_Parent.NeedRecalcLayout == true)
                    throw new Exception();

                r = _Parent.AbsBounds;
            }
            else
            {
                if (_GaugeControl == null)
                    throw new Exception();

                r = _GaugeControl.Bounds;
            }

            if (_GaugeFrame.Style == GaugeFrameStyle.Circular)
            {
                int n = Math.Min(r.Width, r.Height);

                int x = (r.Width - n) / 2;
                int y = (r.Height - n) / 2;

                n = Math.Max(n, 1);

                r = new Rectangle(x, y, n, n);
            }
            else
            {
                r.X = (int)(r.Width * _Location.X);
                r.Y = (int)(r.Height * _Location.Y);

                r.Width = (int)(r.Width * _Width);
                r.Height = (int)(r.Height * _Height);
            }


            return (r);
        }

        #endregion

        #region OnLayoutChanged

        private void OnLayoutChanged()
        {
            GaugeFrame.NeedRecalcLayout = true;

            if (_GaugeScaleItems != null)
            {
                foreach (GaugeScale item in _GaugeScaleItems)
                    item.NeedRecalcLayout = true;
            }

            if (_GaugeImageItems != null)
            {
                foreach (GaugeImage item in _GaugeImageItems)
                    item.NeedRecalcLayout = true;
            }

            if (_GaugeTextItems != null)
            {
                foreach (GaugeText item in _GaugeTextItems)
                    item.NeedRecalcLayout = true;
            }
        }

        #endregion

        #region OnPaint

        public void OnPaint(PaintEventArgs e)
        {
            RecalcLayout();

            GaugeFrame.OnPaint(e);

            PaintScaleItems(e);
            PaintImageItems(e);
            PaintTextItems(e);
        }

        #region PaintScaleItems

        private void PaintScaleItems(PaintEventArgs e)
        {
            if (_GaugeScaleItems != null)
            {
                foreach (GaugeScale scale in _GaugeScaleItems)
                {
                    if (scale.Visible == true)
                        scale.OnPaint(e);
                }
            }
        }

        #endregion

        #region PaintImageItems

        private void PaintImageItems(PaintEventArgs e)
        {
            if (_GaugeImageItems != null)
            {
                foreach (GaugeImage scale in _GaugeImageItems)
                {
                    if (scale.Visible == true)
                        scale.OnPaint(e);
                }
            }
        }

        #endregion

        #region PaintTextItems

        private void PaintTextItems(PaintEventArgs e)
        {
            if (_GaugeTextItems != null)
            {
                foreach (GaugeText scale in _GaugeTextItems)
                {
                    if (scale.Visible == true)
                        scale.OnPaint(e);
                }
            }
        }

        #endregion

        #region GetAbsPoint

        internal Point GetAbsPoint(PointF ptf)
        {
            if (NeedRecalcLayout == true)
                throw new Exception();

            return (new Point((int) (AbsWidth*ptf.X), (int) (AbsHeight*ptf.Y)));
        }

        #endregion

        #region GetAbsSize

        internal Size GetAbsSize(SizeF sf, bool useMin)
        {
            if (NeedRecalcLayout == true)
                throw new Exception();

            int width = AbsWidth;
            int height = AbsHeight;

            if (useMin == true)
            {
                if (width > height)
                    width = height;
                else
                    height = width;
            }

            return (new Size((int)(width * sf.Width), (int)(height * sf.Height)));
        }

        #endregion

        #endregion

        #region OnResize

        public void OnResize(EventArgs e)
        {
            NeedRecalcLayout = true;
        }

        #endregion

        #region OnGaugeChanged

        protected void OnGaugeChanged()
        {
            Refresh();
        }

        #endregion
    }
}

