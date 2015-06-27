using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents canvas for KeyTips
    /// </summary>
    internal class KeyTipsCanvasControl : Control
    {
        #region Private variables
        private Control m_ParentControl=null;
        private IKeyTipsRenderer m_Renderer=null;
        #endregion

        #region Internal Implementation
        public KeyTipsCanvasControl(IKeyTipsRenderer renderer)
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ContainerControl, false);
            this.SetStyle(ControlStyles.Selectable, false);
            this.BackColor = Color.Transparent;
            m_Renderer = renderer;
        }

        protected override void Dispose(bool disposing)
        {
            if (m_ParentControl != null)
            {
                m_ParentControl.Resize -= new EventHandler(ParentControlResize);
                m_ParentControl = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_Renderer != null)
            {
                m_Renderer.PaintKeyTips(e.Graphics);
            }
            base.OnPaint(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (m_ParentControl != null)
                m_ParentControl.Resize -= new EventHandler(ParentControlResize);
            
            m_ParentControl = this.Parent;
            
            if(m_ParentControl!=null)
                m_ParentControl.Resize += new EventHandler(ParentControlResize);

            base.OnParentChanged(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        void ParentControlResize(object sender, EventArgs e)
        {
            if (m_ParentControl == null)
                return;

            this.Bounds = new Rectangle(0, 0, m_ParentControl.Width, m_ParentControl.Height);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TRANSPARENT = 0x020;
                CreateParams p = base.CreateParams;
                p.ExStyle = (p.ExStyle | WS_EX_TRANSPARENT);
                return p;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.WM_NCHITTEST)
            {
                m.Result = new IntPtr(-1);
                return;
            }
            base.WndProc(ref m);
        }
        #endregion
    }

    internal interface IKeyTipsRenderer
    {
        void PaintKeyTips(Graphics g);
    }

    public interface IKeyTipsControl
    {
        bool ProcessMnemonicEx(char charCode);
        bool ShowKeyTips { get;set;}
        string KeyTipsKeysStack { get; set; }
    }
}
