using System;
using System.Text;
using System.Drawing;
using System.Xml;

#if TREEGX
using DevComponents.Tree;
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class FontElement : FontChangeElement
    {
        #region Private Variables
        private Color m_ForeColor = Color.Empty;
        private Color m_OldForeColor = Color.Empty;
        private int m_Size = 0;
        private bool m_RelativeSize = false;
        private string m_Face = "";
        private string m_SystemColorName = "";
        #endregion

        #region Internal Implementation
        protected override void SetFont(MarkupDrawContext d)
        {
            Font font = d.CurrentFont;
            try
            {
                if (m_Face != "" || m_Size != 0 && m_RelativeSize || m_Size>4 && !m_RelativeSize)
                {
                    if (m_Face != "")
                        d.CurrentFont = new Font(m_Face, ((m_RelativeSize || m_Size == 0)?font.SizeInPoints + m_Size:m_Size), font.Style);
                    else
                        d.CurrentFont = new Font(font.FontFamily, ((m_RelativeSize || m_Size == 0)? font.SizeInPoints + m_Size : m_Size), font.Style);
                }
                else
                    font = null;
            }
            catch
            {
                font = null;
            }

            if (font != null)
                m_OldFont = font;

            if (!d.IgnoreFormattingColors)
            {
                if (!m_ForeColor.IsEmpty)
                {
                    m_OldForeColor = d.CurrentForeColor;
                    d.CurrentForeColor = m_ForeColor;
                }
                else if (m_SystemColorName != "")
                {
#if DOTNETBAR
                    if (Rendering.GlobalManager.Renderer is Rendering.Office2007Renderer)
                    {
                        m_OldForeColor = d.CurrentForeColor;
                        d.CurrentForeColor = ((Rendering.Office2007Renderer)Rendering.GlobalManager.Renderer).ColorTable.Form.Active.CaptionTextExtra;
                    }
#endif
                }
            }
        }

        public override void RenderEnd(MarkupDrawContext d)
        {
            RestoreForeColor(d);

            base.RenderEnd(d);
        }

        public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
        {
            RestoreForeColor(d);
            base.MeasureEnd(availableSize, d);
        }

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

        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public string Face
        {
            get { return m_Face; }
            set { m_Face = value; }
        }

        private Color GetColorFromName(string name)
        {
            string s = name.ToLower();
            m_SystemColorName = "";
            if (s == "syscaptiontextextra")
            {
                m_SystemColorName = s;
                return Color.Empty;
            }

            return Color.FromName(name);
        }

        public override void ReadAttributes(XmlTextReader reader)
        {
            m_RelativeSize = false;
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "color")
                {
                    try
                    {
                        string s = reader.Value;
                        if (s.StartsWith("#"))
                        {
                            if (s.Length == 7)
                                m_ForeColor = ColorScheme.GetColor(s.Substring(1));
                        }
                        else
                        {
                            m_ForeColor = GetColorFromName(s);
                        }
                    }
                    catch
                    {
                        m_ForeColor = Color.Empty;
                    }
                }
                else if (reader.Name.ToLower() == "size")
                {
                    string s = reader.Value;
                    if (s.StartsWith("+"))
                    {
                        try
                        {
                            m_Size = Int32.Parse(s.Substring(1));
                            m_RelativeSize = true;
                        }
                        catch
                        {
                            m_Size = 0;
                        }
                    }
                    else
                    {
                        if (s.StartsWith("-"))
                            m_RelativeSize = true;
                        try
                        {
                            m_Size = Int32.Parse(s);
                        }
                        catch
                        {
                            m_Size = 0;
                        }
                    }
                }
                else if (reader.Name.ToLower() == "face")
                {
                    m_Face = reader.Value;
                }
            }
        }
        #endregion
    }
}
