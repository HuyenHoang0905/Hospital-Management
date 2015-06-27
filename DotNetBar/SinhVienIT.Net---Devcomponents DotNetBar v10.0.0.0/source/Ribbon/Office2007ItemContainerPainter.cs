using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    internal class Office2007ItemContainerPainter : ItemContainerPainter, IOffice2007Painter
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

        #region PaintBackground
        public override void PaintBackground(ItemContainerRendererEventArgs e)
        {
            if (!BarFunctions.IsOffice2007Style(e.ItemContainer.EffectiveStyle))
                return;

            ItemContainer container = e.ItemContainer;
            Office2007ItemGroupColorTable gt = m_ColorTable.ItemGroup;

            if (container.SubItems.Count == 0 || gt == null)
                return;

            Rectangle bounds = container.DisplayRectangle;

            if (!container.BeginGroup || container.IsOnMenu && !(container.Parent is ItemContainer))
            {
                if (container.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key && e.ItemPaintArgs != null &&
                e.ItemPaintArgs.ContainerControl is Bar && ((Bar)e.ItemPaintArgs.ContainerControl).BarType == eBarType.StatusBar)
                {                    
                    bounds.Y = 1;
                    bounds.Height = e.ItemPaintArgs.ContainerControl.Height - 1;
                    if (container.Parent.SubItems.IndexOf(container) == container.Parent.SubItems.Count - 1)
                    {
                        if (e.ItemPaintArgs.RightToLeft)
                        {
                            bounds.Width += bounds.X;
                            bounds.X = 0;
                        }
                        else
                        {
                            bounds.Width += e.ItemPaintArgs.ContainerControl.Width - bounds.Right;
                        }
                    }
                }
                Region oldClip = e.Graphics.Clip;
                e.Graphics.SetClip(bounds);
                ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(container.BackgroundStyle, e.Graphics, bounds));
                e.Graphics.Clip = oldClip;
                if (oldClip != null) oldClip.Dispose();
                return;
            }

            bounds.Width -= container.BackgroundStyle.MarginHorizontal;
            bounds.Height -= container.BackgroundStyle.MarginVertical;
            bounds.X += container.BackgroundStyle.MarginLeft;
            bounds.Y += container.BackgroundStyle.MarginTop;
            //bounds.Width--;
            //bounds.Height--;

            Graphics g = e.Graphics;
            int cornerSize = 2;

            
            //GraphicsPath path = DisplayHelp.GetRoundedRectanglePath(bounds, 2);

            //using (SolidBrush brush = new SolidBrush(ColorScheme.GetColor("FEFFFF")))
            //    g.FillPath(brush, path);

            // Draw border
            if (gt.OuterBorder != null && !gt.OuterBorder.IsEmpty)
                DisplayHelp.DrawRoundGradientRectangle(g, bounds, gt.OuterBorder, 1, cornerSize);

            if (gt.InnerBorder != null && !gt.InnerBorder.IsEmpty)
            {
                bounds.Inflate(-1, -1);
                DisplayHelp.DrawRoundGradientRectangle(g, bounds, gt.InnerBorder, 1, cornerSize);
            }

            float topPart = .4f;

            Rectangle r = bounds;
            r.Inflate(-1, -1);

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            if(gt.TopBackground!=null && !gt.TopBackground.IsEmpty)
                DisplayHelp.FillRectangle(g, r, gt.TopBackground);
            int topHeight = (int)(r.Height * topPart);
            r.Height -= topHeight;
            r.Y += topHeight;

            if (gt.BottomBackground != null && !gt.BottomBackground.IsEmpty)
                DisplayHelp.FillRectangle(g, r, gt.BottomBackground);
            g.SmoothingMode = sm;
            bool isVertical = container.LayoutOrientation == eOrientation.Vertical;
            using (Pen penLight = new Pen(gt.ItemGroupDividerLight, 1))
            {
                using (Pen penDark = new Pen(gt.ItemGroupDividerDark))
                {
                    int c = container.SubItems.Count - 1;
                    for (int i = 0; i <= c; i++)
                    {
                        BaseItem item = container.SubItems[i];
                        if (!item.Visible || item.DisplayRectangle.Right == container.DisplayRectangle.Right || i == c)
                            continue;
                        if (isVertical)
                        {
                            g.DrawLine(penDark, container.DisplayRectangle.X + 1, item.DisplayRectangle.Bottom - 1, container.DisplayRectangle.Right - 1, item.DisplayRectangle.Bottom - 1);
                            g.DrawLine(penLight, container.DisplayRectangle.X + 1, item.DisplayRectangle.Bottom, container.DisplayRectangle.Right, item.DisplayRectangle.Bottom);
                        }
                        else
                        {
                            g.DrawLine(penDark, item.DisplayRectangle.Right - 1, item.DisplayRectangle.Y + 1, item.DisplayRectangle.Right - 1, item.DisplayRectangle.Bottom - 2);
                            g.DrawLine(penLight, item.DisplayRectangle.Right, item.DisplayRectangle.Y + 1, item.DisplayRectangle.Right, item.DisplayRectangle.Bottom - 2);
                        }
                    }
                }
            }

            //path.Dispose();
        }
        #endregion

        #region Paint Separator
        public override void PaintItemSeparator(ItemContainerSeparatorRendererEventArgs e)
        {
            if (e.Item is ItemContainer)
                return;

            Graphics g = e.Graphics;
            BaseItem item = e.Item;
            Size imageSize = Size.Empty;
            if (item is ImageItem)
                imageSize = ((ImageItem)item).ImageSize;

            ItemContainer container = e.ItemContainer;
            Color color1 = m_ColorTable.LegacyColors.ItemSeparator;
            Color color2 = m_ColorTable.LegacyColors.ItemSeparatorShade;

            Point start = Point.Empty, end = Point.Empty;
            Point start2 = Point.Empty, end2 = Point.Empty;

            if (container.LayoutOrientation == eOrientation.Horizontal)
            {
                start = new Point(item.DisplayRectangle.X - 2, item.DisplayRectangle.Y + 3);
                end = new Point(start.X, item.DisplayRectangle.Bottom - 4);
                start2 = new Point(start.X + 1, start.Y);
                end2 = new Point(end.X + 1, end.Y);
            }
            else
            {
                if (item.IsOnMenu)
                {
                    start = new Point(item.DisplayRectangle.X + imageSize.Width, item.DisplayRectangle.Y - 2);
                    end = new Point(item.DisplayRectangle.Right-1, start.Y);
                    start2 = new Point(start.X, start.Y + 1);
                    end2 = new Point(end.X, end.Y + 1);
                }
                else
                {
                    start = new Point(item.DisplayRectangle.X + 3, item.DisplayRectangle.Y - 2);
                    end = new Point(item.DisplayRectangle.Right - 4, start.Y);
                    start2 = new Point(start.X, start.Y + 1);
                    end2 = new Point(end.X, end.Y + 1);
                }
            }

            DisplayHelp.DrawLine(g, start, end, color1, 1);
            DisplayHelp.DrawLine(g, start2, end2, color2, 1);
        }
        #endregion
    }
}
