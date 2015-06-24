using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2007SideBarPainter : SideBarPainter, IOffice2007Painter
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

        #region Internal Implementation
        public override void PaintSideBar(SideBarRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            SideBar sb = e.SideBar;
            Office2007SideBarColorTable ct = m_ColorTable.SideBar;
            LinearGradientColorTable background = ct.Background;
            //if (sb.BackColor != SystemColors.Control)
            //{
            //    background.Start = sb.BackColor;
            //    background.End = Color.Empty;
            //}

            Rectangle r = new Rectangle(Point.Empty, sb.Size);
            DisplayHelp.FillRectangle(g, r, background);

            Color border = ct.Border;

            DisplayHelp.DrawRectangle(g, border, r);

            base.PaintSideBar(e);
        }

        public override void PaintSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
        {
            Graphics g = e.Graphics;
            SideBarPanelItem sb = e.SideBarPanelItem;
            Office2007SideBarColorTable ct = m_ColorTable.SideBar;
            GradientColorTable stateColorTable = sb.Expanded ? ct.SideBarPanelItemExpanded : ct.SideBarPanelItemDefault;
            if (sb.IsMouseDown)
                stateColorTable = ct.SideBarPanelItemPressed;
            else if (sb.IsMouseOver)
                stateColorTable = ct.SideBarPanelItemMouseOver;

            if (!sb.BackgroundStyle.BackColor1.IsEmpty || sb.BackgroundStyle.BackgroundImage != null)
                sb.BackgroundStyle.Paint(g, sb.DisplayRectangle);

            Rectangle r = sb.PanelRect;
            Rectangle bounds = sb.DisplayRectangle;
            r.Offset(bounds.Location);

            if (stateColorTable != null)
            {
                using (Brush brush = DisplayHelp.CreateBrush(r, stateColorTable))
                {
                    g.FillRectangle(brush, r);
                }
            }

            if (sb.Expanded)
            {
                DisplayHelp.DrawLine(g, r.X, r.Bottom, r.Right, r.Bottom, ct.Border, 1);
            }

            DisplayHelp.DrawLine(g, bounds.X, bounds.Bottom, bounds.Right, bounds.Bottom, ct.Border, 1);
        
            Font font = sb.GetFont();
            CompositeImage image = sb.GetImage();

            if (image != null)
            {
                r.X += 2;
                r.Width -= 2;
                image.DrawImage(g, new Rectangle(r.X, r.Y + (r.Height - image.Height) / 2, image.Width, image.Height));
                r.X += (image.Width + 4);
                r.Width -= (image.Width + 8);
            }

            Rectangle rText = Rectangle.Empty;
            if (sb.Text.Length > 0)
            {
                TextDrawing.DrawString(g, sb.Text, font, ct.SideBarPanelItemText, r, sb.GetStringFormat());
                rText = r;
            }

            if (sb.Focused)
            {
                if (sb.DesignMode)
                {
                    Rectangle rFocus = sb.PanelRect;
                    rFocus.Offset(sb.DisplayRectangle.Location);
                    rFocus.Inflate(-2, -2);
                    DesignTime.DrawDesignTimeSelection(g, rFocus, e.ItemPaintArgs.Colors.ItemDesignTimeBorder);
                }
                else if (!rText.IsEmpty)
                {
                    System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, rText);
                }
            }

            base.PaintSideBarPanelItem(e);
        }
        #endregion
    }
}
