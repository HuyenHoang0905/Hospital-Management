using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    internal class Office2007KeyTipsPainter : KeyTipsPainter, IOffice2007Painter
    {
        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null; //new OfficeColorTable();

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }
        #endregion

        #region Paint KeyTips
        public override void PaintKeyTips(KeyTipsRendererEventArgs e)
        {
            Rectangle r = e.Bounds;
            Color borderColor = m_ColorTable.KeyTips.KeyTipBorder;
            Color textColor = m_ColorTable.KeyTips.KeyTipText;
            Color backColor = m_ColorTable.KeyTips.KeyTipBackground;

            if (e.ReferenceObject is BaseItem && !((BaseItem)e.ReferenceObject).Enabled)
            {
                int alpha = 128;
                backColor = Color.FromArgb(alpha, backColor);
                textColor = Color.FromArgb(alpha, textColor);
                borderColor = Color.FromArgb(alpha, borderColor);
            }

            Graphics g = e.Graphics;
            string keyTip = e.KeyTip;
            Font font = e.Font;

            using (SolidBrush brush = new SolidBrush(backColor))
                DisplayHelp.FillRoundedRectangle(g, brush, r, 2);
            using (Pen pen = new Pen(textColor, 1))
                DisplayHelp.DrawRoundedRectangle(g, pen, r, 2);
            TextDrawing.DrawString(g, keyTip, font, textColor, r, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
        }
        #endregion
    }
}
