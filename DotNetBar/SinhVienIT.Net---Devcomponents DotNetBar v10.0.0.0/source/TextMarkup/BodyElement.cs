using System;
using System.Text;
using System.Windows.Forms;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class BodyElement : ContainerElement
    {
        #region Private Variables
        private MarkupElementCollection m_ActiveElements=null;
        private IActiveMarkupElement m_MouseOverElement = null;
        internal bool HasExpandElement = false;
        internal string PlainText = "";
        #endregion

        #region Events
        public event EventHandler HyperLinkClick;
        #endregion

        #region Internal Implementation
        public BodyElement()
        {
            m_ActiveElements = new MarkupElementCollection(null);
        }

        public MarkupElementCollection ActiveElements
        {
            get { return m_ActiveElements; }
        }

        public void MouseLeave(Control parent)
        {
            if (m_MouseOverElement != null)
                m_MouseOverElement.MouseLeave(parent);
            m_MouseOverElement = null;
        }

        public void MouseMove(Control parent, MouseEventArgs e)
        {
            if (m_MouseOverElement != null && m_MouseOverElement.HitTest(e.X, e.Y))
                return;
            if (m_MouseOverElement != null)
                m_MouseOverElement.MouseLeave(parent);
            
            m_MouseOverElement = null;

            foreach(IActiveMarkupElement el in m_ActiveElements)
            {
                if (el.HitTest(e.X, e.Y))
                {
                    m_MouseOverElement = el;
                    m_MouseOverElement.MouseEnter(parent);
                }
            }
        }

        public void MouseDown(Control parent, MouseEventArgs e)
        {
            if (m_MouseOverElement != null)
                m_MouseOverElement.MouseDown(parent, e);
        }

        public void MouseUp(Control parent, MouseEventArgs e)
        {
            if (m_MouseOverElement != null)
                m_MouseOverElement.MouseUp(parent, e);
        }

        public void Click(Control parent)
        {
            if (m_MouseOverElement != null)
            {
                m_MouseOverElement.Click(parent);
                if (m_MouseOverElement is HyperLink)
                {
                    if (HyperLinkClick != null)
                        HyperLinkClick(m_MouseOverElement, new EventArgs());
                }
            }
        }
        #endregion
    }
}
