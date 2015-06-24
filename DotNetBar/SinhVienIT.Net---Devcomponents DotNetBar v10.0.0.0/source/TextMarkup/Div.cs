using System;
using System.Text;
using System.Drawing;
using System.Xml;

#if AdvTree
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
using DevComponents.UI.ContentManager;
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class Div : ContainerElement
    {
        #region Private Variables
        private eParagraphAlignment m_Align = eParagraphAlignment.Left;
        private eParagraphVerticalAlignment m_VAlign = eParagraphVerticalAlignment.Top;
        private int m_Width = 0;
        private Padding m_Padding = new Padding(0, 0, 0, 0);
        #endregion

        #region Internal Implementation
        public eParagraphAlignment Align
        {
            get { return m_Align; }
            set { m_Align = value; }
        }

        protected override SerialContentLayoutManager GetLayoutManager(bool multiLine)
        {
            SerialContentLayoutManager sm = base.GetLayoutManager(multiLine);
            if (m_Align == eParagraphAlignment.Left)
                sm.ContentAlignment = eContentAlignment.Left;
            else if (m_Align == eParagraphAlignment.Right)
                sm.ContentAlignment = eContentAlignment.Right;
            else if (m_Align == eParagraphAlignment.Center)
                sm.ContentAlignment = eContentAlignment.Center;

            if (m_VAlign != eParagraphVerticalAlignment.Top)
            {
                sm.EvenHeight = false;
                sm.BlockLineAlignment = (m_VAlign == eParagraphVerticalAlignment.Bottom ? eContentVerticalAlignment.Bottom : eContentVerticalAlignment.Middle);
            }

            return sm;
        }

        protected override Point GetContainerOffset()
        {
            if (m_Padding.Left == 0 && m_Padding.Top == 0)
                return base.GetContainerOffset();
            
            Point p = base.GetContainerOffset();
            p.X += m_Padding.Left;
            p.Y += m_Padding.Top;
            return p;
        }

        protected override void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
        {
            Rectangle r = bounds;
            if (m_Width > 0)
                r.Width = m_Width;
            if (m_Padding.IsEmpty)
            {
                base.ArrangeInternal(r, d);
                if (m_Width > 0)
                    this.Bounds = new Rectangle(this.Bounds.X, this.Bounds.Y, m_Width, this.Bounds.Height + m_Padding.Bottom);
            }
            else
            {
                r.X += m_Padding.Left;
                r.Y += m_Padding.Top;
                base.ArrangeInternal(r, d);

                r = new Rectangle(bounds.X, bounds.Y, this.Bounds.Width + m_Padding.Horizontal, this.Bounds.Height + m_Padding.Vertical);
                if (m_Width > 0)
                    r.Width = m_Width;
                
                this.Bounds = r;
            }
        }

        public override void ReadAttributes(XmlTextReader reader)
        {
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "align")
                {
                    string s = reader.Value.ToLower();
                    if (s == "left")
                        m_Align = eParagraphAlignment.Left;
                    else if (s == "right")
                        m_Align = eParagraphAlignment.Right;
                    else if (s == "center")
                        m_Align = eParagraphAlignment.Center;
                }
                else if (reader.Name.ToLower() == "valign")
                {
                    string s = reader.Value.ToLower();
                    if (s == "top")
                        m_VAlign = eParagraphVerticalAlignment.Top;
                    else if (s == "middle")
                        m_VAlign = eParagraphVerticalAlignment.Middle;
                    else if (s == "bottom")
                        m_VAlign = eParagraphVerticalAlignment.Bottom;
                }
                else if (reader.Name.ToLower() == "width")
                {
                    try
                    {
                        m_Width = Int32.Parse(reader.Value);
                    }
                    catch
                    {
                        m_Width = 0;
                    }
                }
                else if (reader.Name.ToLower() == "padding")
                {
                    try
                    {
                        string[] values = reader.Value.Split(',');
                        if (values.Length > 0)
                            m_Padding.Left = Int32.Parse(values[0]);
                        if (values.Length > 1)
                            m_Padding.Right = Int32.Parse(values[1]);
                        if (values.Length > 2)
                            m_Padding.Top = Int32.Parse(values[2]);
                        if (values.Length > 3)
                            m_Padding.Bottom = Int32.Parse(values[3]);
                    }
                    catch
                    {
                        m_Padding = new Padding(0,0,0,0);
                    }
                }
            }
        }
        #endregion
    }

    #region eParagraphAlignment
    /// <summary>
    /// Indicates paragraph content alignment
    /// </summary>
    internal enum eParagraphAlignment
    {
        Left,
        Right,
        Center
    }

    /// <summary>
    /// Indicates paragraph content alignment
    /// </summary>
    internal enum eParagraphVerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }
    #endregion
}
