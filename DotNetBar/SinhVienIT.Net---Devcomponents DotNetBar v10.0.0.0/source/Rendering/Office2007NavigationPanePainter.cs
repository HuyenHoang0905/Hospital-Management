using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2007NavigationPanePainter : NavigationPanePainter, IOffice2007Painter
    {
        #region IOffice2007Painter
        private Office2007ColorTable m_ColorTable = null;

        /// <summary>
        /// Gets or sets color table used by renderer.
        /// </summary>
        public Office2007ColorTable ColorTable
        {
            get { return m_ColorTable; }
            set { m_ColorTable = value; }
        }
        #endregion

        public override void PaintButtonBackground(NavPaneRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            Office2007NavigationPaneColorTable ct = m_ColorTable.NavigationPane;

            using (Brush brush = DisplayHelp.CreateBrush(r, ct.ButtonBackground))
            {
                g.FillRectangle(brush, r);
            }
        }
    }
}
