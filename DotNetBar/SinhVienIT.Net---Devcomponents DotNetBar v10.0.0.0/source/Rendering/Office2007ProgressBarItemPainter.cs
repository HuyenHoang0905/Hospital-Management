using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Rendering
{
    internal class Office2007ProgressBarItemPainter : ProgressBarItemPainter, IOffice2007Painter
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
        public override void Paint(ProgressBarItemRenderEventArgs e)
        {
            Rectangle r = e.ProgressBarItem.DisplayRectangle;
            if (r.Width <= 0 || r.Height <= 0)
                return;

            ProgressBarItem item = e.ProgressBarItem;
            Office2007ProgressBarColorTable ct = m_ColorTable.ProgressBarItem;
            if(item.ColorTable == eProgressBarItemColor.Paused)
                ct = m_ColorTable.ProgressBarItemPaused;
            else if (item.ColorTable == eProgressBarItemColor.Error)
                ct = m_ColorTable.ProgressBarItemError;
            
            Graphics g = e.Graphics;

            DisplayHelp.DrawRoundedRectangle(g, ct.OuterBorder, r, 2);
            r.Inflate(-1, -1);
            
            Brush brush = DisplayHelp.CreateBrush(r, ct.BackgroundColors);
            if (brush != null)
            {
                g.FillRectangle(brush, r);
                brush.Dispose();
            }
            DisplayHelp.DrawRoundedRectangle(g, ct.InnerBorder, r, 2);
            r.Inflate(-1, -1);

            Region oldClip = g.Clip;
            try
            {
                g.SetClip(r, CombineMode.Intersect);

                Rectangle chunkRect = r;
                if (item.ProgressType == eProgressItemType.Marquee)
                {
                    // Horizontal
                    if (item.Orientation == eOrientation.Horizontal)
                    {
                        chunkRect.Width = (int)(chunkRect.Width * .3);
                        chunkRect.X += r.Width * item.MarqueeValue / 100 - (int)(chunkRect.Width / 2);
                    }
                    // Vertical
                    else if (item.Orientation == eOrientation.Vertical)
                    {
                        chunkRect.Height = (int)(chunkRect.Height * .3);
                        chunkRect.Y += r.Height * item.MarqueeValue / 100 - (int)(chunkRect.Height / 2);
                    }
                }
                else
                {
                    // Horizontal
                    if (item.Orientation == eOrientation.Horizontal)
                        chunkRect.Width = (int)(chunkRect.Width * ((float)(item.Value - item.Minimum) / (float)(item.Maximum - item.Minimum)));
                    // Vertical
                    else if (item.Orientation == eOrientation.Vertical)
                    {
                        int height = (int)(chunkRect.Height * ((float)(item.Value - item.Minimum) / (float)(item.Maximum - item.Minimum)));
                        chunkRect.Y = chunkRect.Bottom - height;
                        chunkRect.Height = height;
                    }
                }
                if (chunkRect.Width <= 0 || chunkRect.Height<=0) return;
                
                brush = DisplayHelp.CreateBrush(chunkRect, ct.Chunk);
                if (brush != null)
                {
                    g.FillRectangle(brush, chunkRect);
                    brush.Dispose();
                }
                GradientColorTable overlay = ct.ChunkOverlay;
                if (item.Orientation == eOrientation.Horizontal)
                    overlay.LinearGradientAngle = 90;
                else
                    overlay.LinearGradientAngle = 0;
                brush = DisplayHelp.CreateBrush(chunkRect, overlay);
                if (brush != null)
                {
                    g.FillRectangle(brush, chunkRect);

                    if (item.ProgressType == eProgressItemType.Marquee)
                    {
                        // Horizontal
                        if (item.Orientation == eOrientation.Horizontal && chunkRect.Right > r.Right + 4)
                            chunkRect = new Rectangle(r.X, r.Y, chunkRect.Right - r.Right - 4, r.Height);
                        // Vertical
                        else if (item.Orientation == eOrientation.Vertical && chunkRect.Bottom > r.Bottom + 4)
                            chunkRect = new Rectangle(r.X, r.Y, r.Height, chunkRect.Bottom - r.Bottom - 4);
                        g.FillRectangle(brush, chunkRect);
                    }

                    brush.Dispose();
                }
                // Horizontal
                if (item.Orientation == eOrientation.Horizontal && chunkRect.Right + 4 <= r.Right)
                {
                    chunkRect.X = chunkRect.Right;
                    chunkRect.Width = 4;
                    brush = DisplayHelp.CreateBrush(chunkRect, ct.ChunkShadow);
                    if (brush != null)
                    {
                        g.FillRectangle(brush, chunkRect);
                        brush.Dispose();
                    }
                }
                // Vertical
                else if (item.Orientation == eOrientation.Vertical && chunkRect.Y - 3 >= r.Y)
                {
                    chunkRect.Y = chunkRect.Y - 3;
                    chunkRect.Height = 3;
                    brush = DisplayHelp.CreateBrush(chunkRect, ct.ChunkShadow.Colors, ct.ChunkShadow.LinearGradientAngle-90, ct.ChunkShadow.GradientType);
                    if (brush != null)
                    {
                        g.FillRectangle(brush, chunkRect);
                        brush.Dispose();
                    }
                }
            }
            finally
            {
                g.Clip = oldClip;
            }
        }

        #endregion
    }
}
