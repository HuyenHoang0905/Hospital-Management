using System;
using System.Text;
using System.Drawing;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class Heading: ContainerElement
    {
        #region Private Variables
        private int m_Level = 1;
        private Font m_OldFont = null;
        #endregion

        #region Internal Implementation
        public Heading() { }
        public Heading(int level)
        {
            m_Level = level;
        }

        public override void Measure(Size availableSize, MarkupDrawContext d)
        {
            SetFont(d);
            base.Measure(availableSize, d);
            if (m_OldFont != null)
                d.CurrentFont = m_OldFont;
        }

        public override void Render(MarkupDrawContext d)
        {
            SetFont(d);
            base.Render(d);
            if (m_OldFont != null)
                d.CurrentFont = m_OldFont;
        }

        protected virtual void SetFont(MarkupDrawContext d)
        {
            Font font = d.CurrentFont;
            try
            {
                float size = d.CurrentFont.SizeInPoints;
                if (m_Level == 1)
                {
                    size += 12;
                }
                else if (m_Level == 2)
                {
                    size += 8;
                }
                else if (m_Level == 3)
                {
                    size += 6;
                }
                else if (m_Level == 4)
                {
                    size += 4;
                }
                else if (m_Level == 5)
                {
                    size += 2;
                }
                else if (m_Level == 6)
                {
                    size += 1;
                }

                d.CurrentFont = new Font(d.CurrentFont.FontFamily, size, FontStyle.Bold);
            }
            catch
            {
                font = null;
            }

            if (font != null)
                m_OldFont = font;
        }

        /// <summary>
        /// Gets or sets heading level. Values from 1 to 6 are valid. Default is 1.
        /// </summary>
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        #endregion
    }
}
