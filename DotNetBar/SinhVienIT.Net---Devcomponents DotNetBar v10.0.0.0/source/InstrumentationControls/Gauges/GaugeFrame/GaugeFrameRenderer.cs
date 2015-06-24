using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.Instrumentation
{
    internal abstract class GaugeFrameRenderer
    {
        #region Private variables

        private GaugeFrame _GaugeFrame;

        #endregion

        protected GaugeFrameRenderer(GaugeFrame gaugeFrame)
        {
            _GaugeFrame = gaugeFrame;
        }

        #region Abstract methods

        internal abstract void SetBackClipRegion(PaintEventArgs e);
        internal abstract void PreRenderContent(PaintEventArgs e);
        internal abstract void PostRenderContent(PaintEventArgs e);

        protected abstract void RenderFrameByAngle(PaintEventArgs e, Rectangle r);
        protected abstract void RenderFrameByCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderFrameByHorizontalCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderFrameByVerticalCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderFrameByNone(PaintEventArgs e, Rectangle r);

        protected abstract void RenderBackByAngle(PaintEventArgs e, Rectangle r);
        protected abstract void RenderBackByCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderBackByHorizontalCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderBackByVerticalCenter(PaintEventArgs e, Rectangle r);
        protected abstract void RenderBackByNone(PaintEventArgs e, Rectangle r);

        #endregion

        #region Protected properties

        protected GaugeFrame GaugeFrame
        {
            get { return (_GaugeFrame); }
        }

        #endregion

        #region SetFrameRegion

        internal virtual void SetFrameRegion()
        {
            if (GaugeFrame.GaugeControl != null)
                GaugeFrame.GaugeControl.Region = null;
        }

        #endregion

        #region RenderFrame

        internal void RenderFrame(PaintEventArgs e)
        {
            int inside = _GaugeFrame.AbsBevelInside;
            int outside = _GaugeFrame.AbsBevelOutside;

            Rectangle r = _GaugeFrame.Bounds;
            r.Inflate(-outside, -outside);

            if (r.Width > 0 && r.Height > 0)
            {
                switch (_GaugeFrame.FrameColor.GradientFillType)
                {
                    case GradientFillType.Auto:
                    case GradientFillType.Angle:
                    case GradientFillType.StartToEnd:
                        RenderFrameByAngle(e, r);
                        break;

                    case GradientFillType.Center:
                        RenderFrameByCenter(e, r);
                        break;

                    case GradientFillType.HorizontalCenter:
                        RenderFrameByHorizontalCenter(e, r);
                        break;

                    case GradientFillType.VerticalCenter:
                        RenderFrameByVerticalCenter(e, r);
                        break;

                    default:
                        RenderFrameByNone(e, r);
                        break;
                }

                r.Inflate(-inside, -inside);

                if (r.Width > 0 && r.Height > 0)
                {
                    switch (_GaugeFrame.BackColor.GradientFillType)
                    {
                        case GradientFillType.Auto:
                        case GradientFillType.Angle:
                        case GradientFillType.StartToEnd:
                            RenderBackByAngle(e, r);
                            break;

                        case GradientFillType.Center:
                            RenderBackByCenter(e, r);
                            break;

                        case GradientFillType.HorizontalCenter:
                            RenderBackByHorizontalCenter(e, r);
                            break;

                        case GradientFillType.VerticalCenter:
                            RenderBackByVerticalCenter(e, r);
                            break;

                        default:
                            RenderBackByNone(e, r);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
