using System;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class HyperLink : MarkupElement, IActiveMarkupElement
    {
        #region Private Variables
        private Color m_ForeColor = Color.Empty;
        private Color m_OldForeColor = Color.Empty;
        private string m_HRef = "";
        private string m_Name = "";
        private Cursor m_OldCursor = null;
        private bool m_IsMouseOver = false;
        private bool m_Visited = false;
        #endregion

        #region Internal Implementation
        public override void Measure(Size availableSize, MarkupDrawContext d)
        {
            this.Bounds = Rectangle.Empty;
            SetForeColor(d);
        }

        public override void Render(MarkupDrawContext d)
        {
            d.HyperLink = true;
            d.HyperlinkStyle = GetHyperlinkStyle();
            if (!d.HyperlinkStyle.BackColor.IsEmpty)
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    MarkupElementCollection col = this.Parent.Elements;
                    int start = col.IndexOf(this) + 1;
                    for (int i = start; i < col.Count; i++)
                    {
                        MarkupElement elem = col[i];
                        if (!elem.Visible) continue;
                        if (elem is EndMarkupElement && ((EndMarkupElement)elem).StartElement == this)
                            break;
                        gp.AddRectangle(elem.RenderBounds);
                    }

                    using (SolidBrush brush = new SolidBrush(d.HyperlinkStyle.BackColor))
                        d.Graphics.FillPath(brush, gp);
                }
            }
            SetForeColor(d);
        }

        private HyperlinkStyle GetHyperlinkStyle()
        {
            if (m_IsMouseOver && MarkupSettings.MouseOverHyperlink.IsChanged)
                return MarkupSettings.MouseOverHyperlink;
            else if (m_Visited && MarkupSettings.VisitedHyperlink.IsChanged)
                return MarkupSettings.VisitedHyperlink;

            return MarkupSettings.NormalHyperlink;
        }

        protected virtual void SetForeColor(MarkupDrawContext d)
        {
            Color c = Color.Empty;
            HyperlinkStyle style = GetHyperlinkStyle();
            if (style != null && !style.TextColor.IsEmpty)
                c = style.TextColor;

            if (!m_ForeColor.IsEmpty)
                c = m_ForeColor;
            if (!c.IsEmpty)
            {
                m_OldForeColor = d.CurrentForeColor;
                d.CurrentForeColor = c;
            }
        }

        public override void RenderEnd(MarkupDrawContext d)
        {
            RestoreForeColor(d);
            d.HyperLink = false;
            d.HyperlinkStyle = null;
            base.RenderEnd(d);
        }

        public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
        {
            RestoreForeColor(d);
            base.MeasureEnd(availableSize, d);
        }

        protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d) { }

        protected virtual void RestoreForeColor(MarkupDrawContext d)
        {
            if (!m_OldForeColor.IsEmpty)
                d.CurrentForeColor = m_OldForeColor;
            m_OldForeColor = Color.Empty;
        }

        public Color ForeColor
        {
            get { return m_ForeColor; }
            set { m_ForeColor = value; }
        }

        public string HRef
        {
            get { return m_HRef; }
            set { m_HRef = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public override void ReadAttributes(XmlTextReader reader)
        {
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "href")
                {
                    m_HRef = reader.Value;
                }
                else if (reader.Name.ToLower() == "name")
                {
                    m_Name = reader.Value;
                }
            }
        }

        /// <summary>
        /// Returns whether hyper-link contains specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool HitTest(int x, int y)
        {
            if (this.Parent == null)
                return false;

            MarkupElementCollection col = this.Parent.Elements;
            int start = col.IndexOf(this)+1;
            for (int i = start; i < col.Count; i++)
            {
                MarkupElement el = col[i];
                if (el is EndMarkupElement && ((EndMarkupElement)el).StartElement == this)
                    break;

                if (col[i].RenderBounds.Contains(x, y))
                    return true;

            }

            return false;
        }

        public void MouseEnter(Control parent)
        {
            m_OldCursor = parent.Cursor;
            parent.Cursor = Cursors.Hand;
            m_IsMouseOver = true;
            if (MarkupSettings.MouseOverHyperlink.IsChanged)
            {
                InvalidateElements(parent);
            }
        }

        public void MouseLeave(Control parent)
        {
            if (m_OldCursor != null && parent!=null)
                parent.Cursor = m_OldCursor;
            m_OldCursor = null;
            m_IsMouseOver = false;
            if (MarkupSettings.MouseOverHyperlink.IsChanged)
            {
                InvalidateElements(parent);
            }
        }

        public void MouseDown(Control parent, MouseEventArgs e) { }
        public void MouseUp(Control parent, MouseEventArgs e) { }
        public void Click(Control parent)
        {
            m_Visited = true;
            if (MarkupSettings.VisitedHyperlink.IsChanged)
            {
                InvalidateElements(parent);
            }
        }

        private void InvalidateElements(Control parent)
        {
            if (this.Parent == null) return;
            MarkupElementCollection col = this.Parent.Elements;
            int start = col.IndexOf(this) + 1;
            for (int i = start; i < col.Count; i++)
            {
                MarkupElement elem = col[i];
                if (!elem.Visible) continue;
                if (elem is EndMarkupElement && ((EndMarkupElement)elem).StartElement == this)
                    break;
                parent.Invalidate(elem.RenderBounds);
            }
        }
        #endregion
    }
}
