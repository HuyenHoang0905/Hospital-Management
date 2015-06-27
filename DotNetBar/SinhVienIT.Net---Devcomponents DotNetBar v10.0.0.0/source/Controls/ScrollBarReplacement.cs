#if FRAMEWORK20
using System;
using System.Text;
using DevComponents.DotNetBar.ScrollBar;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls
{
    internal class ScrollBarReplacement:IDisposable
    {
        #region Private Variables
        private ScrollBarCore m_ScrollBarCore = null;
        private System.Windows.Forms.ScrollBar m_ParentScrollBar = null;
        private IScrollBarExtender m_ParentScrollBarWndProc = null;
        private bool m_IsVScrollBar = false;
        #endregion

        #region Constructor
        public ScrollBarReplacement(System.Windows.Forms.ScrollBar sb)
        {
            m_ParentScrollBar = sb;
            m_ParentScrollBarWndProc = (IScrollBarExtender)m_ParentScrollBar;
            m_IsVScrollBar = m_ParentScrollBar is VScrollBar;

            m_ScrollBarCore = new ScrollBarCore(m_ParentScrollBar, false);
            m_ScrollBarCore.ValueChanged += new EventHandler(ScrollBarCore_ValueChanged);
            if (m_ParentScrollBar is HScrollBar)
                m_ScrollBarCore.Orientation = eOrientation.Horizontal;
            else
                m_ScrollBarCore.Orientation = eOrientation.Vertical;
            m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
            m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
            m_ScrollBarCore.Value = m_ParentScrollBar.Value;
            m_ScrollBarCore.Enabled = m_ParentScrollBar.Enabled;
            m_ParentScrollBar.EnabledChanged += new EventHandler(ParentScrollBar_EnabledChanged);
        }
        #endregion

        #region Internal Implementation
        private void ParentScrollBar_EnabledChanged(object sender, EventArgs e)
        {
            m_ScrollBarCore.Enabled = m_ParentScrollBar.Enabled;
        }

        internal void OnHandleCreated()
        {
            UpdateScrollValues();
        }

        internal void UpdateScrollValues()
        {
            Rectangle r = new Rectangle(0, 0, m_ParentScrollBar.Width, m_ParentScrollBar.Height);

            if (m_ScrollBarCore.Minimum != m_ParentScrollBar.Minimum)
                m_ScrollBarCore.Minimum = m_ParentScrollBar.Minimum;
            if (m_ScrollBarCore.Maximum != m_ParentScrollBar.Maximum)
                m_ScrollBarCore.Maximum = m_ParentScrollBar.Maximum;
            if (m_ScrollBarCore.SmallChange != m_ParentScrollBar.SmallChange)
                m_ScrollBarCore.SmallChange = m_ParentScrollBar.SmallChange;
            if (m_ScrollBarCore.LargeChange != m_ParentScrollBar.LargeChange)
                m_ScrollBarCore.LargeChange = m_ParentScrollBar.LargeChange;
            if (m_ScrollBarCore.Value != m_ParentScrollBar.Value)
                m_ScrollBarCore.Value = m_ParentScrollBar.Value;
            if (r != m_ScrollBarCore.DisplayRectangle)
                m_ScrollBarCore.DisplayRectangle = r;
        }

        private bool IsVScroll
        {
            get
            {
                return m_IsVScrollBar;
            }
        }

        internal void OnMouseEnter(EventArgs e)
        {
            if (m_ParentScrollBar.Capture)
                m_ParentScrollBar.Capture = false;
        }

        internal void OnMouseLeave(EventArgs e)
        {
            m_ScrollBarCore.MouseLeave();
        }

        internal void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            m_ScrollBarCore.MouseMove(e);
        }

        internal void NotifyInvalidate(Rectangle invalidatedArea)
        {
            //UpdateScrollValues();
            m_ScrollBarCore.DisposeCashedView();
        }

        internal void OnMouseDown(MouseEventArgs e)
        {
            m_ScrollBarCore.MouseDown(e);
        }

        internal void OnMouseUp(MouseEventArgs e)
        {
            m_ScrollBarCore.MouseUp(e);
        }

        private void ScrollBarCore_ValueChanged(object sender, EventArgs e)
        {
            SetValue(m_ScrollBarCore.Value);
        }

        private void SetValue(int v)
        {
            //Console.WriteLine(v + "   " + m_ParentScrollBar.LargeChange +"    "+m_ParentScrollBar.Maximum);
            if (m_ParentScrollBar.Value == v && m_ParentScrollBar.Value != m_ScrollBarCore.GetMaximumValue()) return;

            ScrollEventType t = ScrollEventType.SmallIncrement;
            if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.ThumbDecrease)
                t = ScrollEventType.SmallDecrement;
            else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Track)
                t = ScrollEventType.ThumbTrack;
            else if (m_ScrollBarCore.MouseOverPart == ScrollBarCore.eScrollPart.Control)
            {
                if (v > m_ParentScrollBar.Value)
                    t = ScrollEventType.LargeIncrement;
                else
                    t = ScrollEventType.LargeDecrement;
            }
            if (t == ScrollEventType.SmallIncrement && m_ParentScrollBar.Value == v && m_ParentScrollBar.Value == m_ScrollBarCore.GetMaximumValue())
            {
                t = ScrollEventType.Last;
            }
            m_ParentScrollBarWndProc.SetValue(v, t);

            //if (t == ScrollEventType.ThumbTrack && m_ParentScrollBar.Parent is DataGridView)
            //    m_ParentScrollBar.Parent.Refresh();
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            
        }
        #endregion

        #region Rendering
        internal void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UpdateScrollValues();
            //using (BufferedBitmap bmp = new BufferedBitmap(g, new Rectangle(0, 0, m_ParentScrollBar.Width, m_ParentScrollBar.Height)))
            //{
                m_ScrollBarCore.Paint(GetItemPaintArgs(g));
                //bmp.Render(g);
            //}
        }

        private ItemPaintArgs GetItemPaintArgs(Graphics g)
        {
            ItemPaintArgs pa = new ItemPaintArgs(m_ParentScrollBar as IOwner, m_ParentScrollBar, g, GetColorScheme());
            pa.Renderer = this.GetRenderer();
            pa.DesignerSelection = false;
            pa.GlassEnabled = false;
            return pa;
        }

        private ColorScheme m_ColorScheme = null;
        /// <summary>
        /// Returns the color scheme used by control. Color scheme for Office2007 style will be retrived from the current renderer instead of
        /// local color scheme referenced by ColorScheme property.
        /// </summary>
        /// <returns>An instance of ColorScheme object.</returns>
        protected virtual ColorScheme GetColorScheme()
        {
            BaseRenderer r = GetRenderer();
            if (r is Office2007Renderer)
                return ((Office2007Renderer)r).ColorTable.LegacyColors;
            if (m_ColorScheme == null)
                m_ColorScheme = new ColorScheme(eDotNetBarStyle.Office2007);
            return m_ColorScheme;
        }

        private Rendering.BaseRenderer m_DefaultRenderer = null;
        private Rendering.BaseRenderer m_Renderer = null;
        private eRenderMode m_RenderMode = eRenderMode.Global;
        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public virtual Rendering.BaseRenderer GetRenderer()
        {
            if (m_RenderMode == eRenderMode.Global && Rendering.GlobalManager.Renderer != null)
                return Rendering.GlobalManager.Renderer;
            else if (m_RenderMode == eRenderMode.Custom && m_Renderer != null)
                return m_Renderer;

            if (m_DefaultRenderer == null)
                m_DefaultRenderer = new Rendering.Office2007Renderer();

            return m_DefaultRenderer;
        }

        /// <summary>
        /// Gets or sets the redering mode used by control. Default value is eRenderMode.Global which means that static GlobalManager.Renderer is used. If set to Custom then Renderer property must
        /// also be set to the custom renderer that will be used.
        /// </summary>
        public eRenderMode RenderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;
                    m_ParentScrollBar.Invalidate(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom renderer used by the items on this control. RenderMode property must also be set to eRenderMode.Custom in order renderer
        /// specified here to be used.
        /// </summary>
        public DevComponents.DotNetBar.Rendering.BaseRenderer Renderer
        {
            get
            {
                return m_Renderer;
            }
            set { m_Renderer = value; }
        }

        internal ScrollBarCore ScrollBarCore
        {
            get { return m_ScrollBarCore; }
        }

        public bool AppStyleScrollBar
        {
            get { return m_ScrollBarCore.IsAppScrollBarStyle; }
            set { m_ScrollBarCore.IsAppScrollBarStyle = value; }
        }
        #endregion

        #region IScrollBarExtender
        internal interface IScrollBarExtender
        {
            void SetValue(int newValue, ScrollEventType type);
        }
        #endregion
    }
}
#endif