using System;
using System.Drawing;
using System.Text;


namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the painter for ColorItem in Office 2007 style
    /// </summary>
    internal class Office2007ColorItemPainter : ColorItemPainter, IOffice2007Painter
    {
        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null; //new Office2007ColorTable();

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }
        #endregion

        #region Internal Implementation
        public override void PaintColorItem(ColorItemRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            ColorItem item = e.ColorItem;
            Rectangle r = item.Bounds;
            Color borderColor = m_ColorTable.ColorItem.Border;
            Color outerHotColor = m_ColorTable.ColorItem.MouseOverOuterBorder;
            Color innerHotColor = m_ColorTable.ColorItem.MouseOverInnerBorder;

            System.Drawing.Drawing2D.SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            Region clip = g.Clip;
            g.SetClip(r);
            Color selectedColor = Color.Empty;
            ColorPickerDropDown cpd = null;
            BaseItem parent = item.Parent;
            while (parent != null)
            {
                if (parent is ColorPickerDropDown)
                {
                    cpd = parent as ColorPickerDropDown;
                    break;
                }
                parent = parent.Parent;
            }
            if (cpd != null)
                selectedColor = cpd.SelectedColor;
            try
            {
                if (!item.Color.IsEmpty)
                {
                    Rectangle fill = r;
                    fill.Inflate(1, 1);
                    DisplayHelp.FillRectangle(g, fill, item.Color, Color.Empty);
                }

                if (item.IsMouseOver || selectedColor == item.Color)
                {
                    Rectangle inner = r;
                    inner.Inflate(-1, -1);
                    DisplayHelp.DrawRectangle(g, innerHotColor, inner);
                    DisplayHelp.DrawRectangle(g, outerHotColor, r);
                }
                else
                {
                    eColorItemBorder border = item.Border;
                    if (border == eColorItemBorder.All)
                        DisplayHelp.DrawRectangle(g, borderColor, r);
                    else if (border != eColorItemBorder.None)
                    {
                        using (Pen pen = new Pen(borderColor))
                        {
                            if ((border & eColorItemBorder.Left) == eColorItemBorder.Left)
                                g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom - 1);
                            if ((border & eColorItemBorder.Right) == eColorItemBorder.Right)
                                g.DrawLine(pen, r.Right - 1, r.Y, r.Right - 1, r.Bottom - 1);
                            if ((border & eColorItemBorder.Top) == eColorItemBorder.Top)
                                g.DrawLine(pen, r.X, r.Y, r.Right - 1, r.Y);
                            if ((border & eColorItemBorder.Bottom) == eColorItemBorder.Bottom)
                                g.DrawLine(pen, r.X, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                        }
                    }
                }
            }
            finally
            {
                if (clip != null)
                    g.Clip = clip;
                else
                    g.ResetClip();
            }
            g.SmoothingMode = sm;
        }
        #endregion
    }
}
