using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Layout
{
    internal class CellTileLayout : CellLayout
    {
        /// <summary>
        /// Initializes a new instance of the CellTileLayout class.
        /// </summary>
        public CellTileLayout(LayoutSettings layoutSettings) : base(layoutSettings)
        {
            
        }
        public override Size LayoutCells(NodeLayoutContextInfo layoutInfo, int x, int y)
        {
            eCellLayout layout = layoutInfo.CellLayout;
            if (layoutInfo.ContextNode.CellLayout != layoutInfo.CellLayout && layoutInfo.ContextNode.CellLayout != eCellLayout.Default)
                layout = layoutInfo.ContextNode.CellLayout;
            if (layout == eCellLayout.Default && !(layoutInfo.ContextNode.HasChildNodes && layoutInfo.IsViewGroupping) && layoutInfo.ContextNode.ImageAlignment == eCellPartAlignment.Default)
            {
                return TileLayout(layoutInfo, x, y);
            }
            else
                return base.LayoutCells(layoutInfo, x, y);
            
        }
        private Size TileLayout(NodeLayoutContextInfo layoutInfo, int x, int y)
        {
            Node node = layoutInfo.ContextNode;
            int height = 0, width = 0, realHeight = 0;
            //eHorizontalAlign align = eHorizontalAlign.Left;
            Size tileSize = layoutInfo.TileSize;
            int iVisibleCells = 0;
            int cellCount = node.Cells.Count;
            bool isVerticalOverflow = false;
            for (int i = 0; i < cellCount; i++)
            {
                Cell cell = node.Cells[i];

                bool bCellVisible = isVerticalOverflow ? false : true;

                // Setup cell layout helper class
                LayoutCellInfo cellLayout = this.GetLayoutCellInfo();
                cellLayout.Top = y;
                cellLayout.Left = x;
                cellLayout.CellWidth = tileSize.Width;
                cellLayout.ContextCell = cell;
                cellLayout.Graphics = layoutInfo.Graphics;
                cellLayout.LeftToRight = layoutInfo.LeftToRight;
                cellLayout.Font = layoutInfo.DefaultFont;
                cellLayout.View = layoutInfo.View;
                cellLayout.CellIndex = i;
                if (cell.Layout != eCellPartLayout.Default)
                    cellLayout.VerticalPartAlignment = (cell.Layout == eCellPartLayout.Vertical);
                else if (layoutInfo.CellPartLayout != eCellPartLayout.Default)
                    cellLayout.VerticalPartAlignment = (layoutInfo.CellPartLayout == eCellPartLayout.Vertical);

                // Prepare union style
                if (cell.StyleNormal != null)
                    cellLayout.LayoutStyle = cell.StyleNormal;
                else
                    cellLayout.LayoutStyle = layoutInfo.DefaultCellStyle;

                this.LayoutSingleTileCell(cellLayout);
                if (bCellVisible && y + cell.BoundsRelative.Height > tileSize.Height && i > 0)
                {
                    isVerticalOverflow = true;
                    bCellVisible = false;
                }
                cell.SetVisible(bCellVisible);

                if (bCellVisible)
                {
                    iVisibleCells++;
                    y += cell.BoundsRelative.Height;
                    height += cell.BoundsRelative.Height;
                    if (cell.BoundsRelative.Height > 0)
                    {
                        y += this.CellVerticalSpacing;
                        height += this.CellVerticalSpacing;
                    }
                    if (cell.BoundsRelative.Width > width)
                        width = cell.BoundsRelative.Width;
                    if (i == 0)
                    {
                        realHeight += cell.BoundsRelative.Height;
                        if (cell.BoundsRelative.Height > 0)
                            realHeight += this.CellVerticalSpacing;
                    }
                    else
                        realHeight = Math.Max(realHeight, height);

                    // Align other cells under the text of the first cell and to the right of the image, if any
                    if (i == 0 && !cell.Images.LargestImageSize.IsEmpty && cellCount > 1)
                    {
                        if (cell.TextContentBounds.IsEmpty)
                            x += cell.Images.LargestImageSize.Width + this.ImageTextSpacing;
                        else
                            x += (cell.TextContentBounds.X - x);
                        tileSize.Width = cell.TextContentBounds.Width;
                        height -= cell.BoundsRelative.Height;
                        height += cell.TextContentBounds.Height;
                        y -= cell.BoundsRelative.Height;
                        y += cell.TextContentBounds.Height;
                    }
                }
            }

            // Take last added spacing off
            y -= this.CellVerticalSpacing;
            height -= this.CellVerticalSpacing;
            realHeight -= this.CellVerticalSpacing;

            if (node.Cells[0].BoundsRelative.Height > height && !node.Cells[0].Images.LargestImageSize.IsEmpty)
            {
                int textOffset = ((realHeight - height) / iVisibleCells) / 2;
                if (textOffset > 0)
                {
                    foreach (Cell cell in node.Cells)
                    {
                        if (!cell.IsVisible) continue;
                        cell.TextContentBounds = new Rectangle(cell.TextContentBounds.X, cell.TextContentBounds.Y + textOffset, cell.TextContentBounds.Width, cell.TextContentBounds.Height);
                    }
                }
            }
            else if(iVisibleCells == 1)
            {
                Rectangle rtc = node.Cells[0].TextContentBounds;
                node.Cells[0].TextContentBounds = new Rectangle(rtc.X, rtc.Y, rtc.Width, node.Cells[0].BoundsRelative.Height);
            }

            // Additional pass needed if horizontal alignment is other than left and there is more than one cell visible
            //if (align != eHorizontalAlign.Left && iVisibleCells > 1)
            //{
            //    foreach (Cell cell in node.Cells)
            //    {
            //        if (!cell.IsVisible)
            //            continue;
            //        if (align == eHorizontalAlign.Center)
            //            this.Offset(cell, (width - cell.BoundsRelative.Width) / 2, 0);
            //        else // Right aligned cells
            //            this.Offset(cell, width - cell.BoundsRelative.Width, 0);
            //    }
            //}
            if (width < layoutInfo.TileSize.Width)
                width = layoutInfo.TileSize.Width;
            if (realHeight < layoutInfo.TileSize.Height)
                realHeight = layoutInfo.TileSize.Height;
            return new Size(width, realHeight);
        }

        protected virtual void LayoutSingleTileCell(LayoutCellInfo info)
        {
            Size textSize = Size.Empty;
            Font font = info.Font;
            int height = 0;
            if (info.LayoutStyle.Font != null)
                font = info.LayoutStyle.Font;

            info.ContextCell.OnLayoutCell();

            if (info.ContextCell.Images.LargestImageSize.IsEmpty && HasImage(info.ContextCell))
                info.ContextCell.Images.RefreshLargestImageSize();

            if (info.ContextCell.HostedControl != null)
            {
                Size controlSize = info.ContextCell.HostedControl.Size;
                if (!info.ContextCell.HostedControlSize.IsEmpty)
                    controlSize = info.ContextCell.HostedControlSize;
                if (info.CellWidth == 0)
                    textSize = new Size(controlSize.Width, controlSize.Height);
                else
                {
                    int availTextWidth = info.CellWidth -
                                       ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
                    textSize = new Size(availTextWidth, controlSize.Height);
                }
            }
            else if (info.ContextCell.HostedItem != null)
            {
                if (info.CellWidth != 0) info.ContextCell.HostedItem.WidthInternal = info.CellWidth;
                info.ContextCell.HostedItem.RecalcSize();

                Size controlSize = info.ContextCell.HostedItem.Size;
                if (info.CellWidth == 0)
                    textSize = new Size(controlSize.Width, controlSize.Height);
                else
                {
                    int availTextWidth = info.CellWidth -
                                       ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
                    textSize = new Size(availTextWidth, controlSize.Height);
                    info.ContextCell.HostedItem.WidthInternal = availTextWidth;
                }
            }
            else
            {
                // Calculate Text Width and Height
                if (info.CellWidth == 0)
                {
                    if (info.ContextCell.TextMarkupBody == null)
                    {
                        string text = info.ContextCell.DisplayText;
                        if (text != "")
                        {
                            if (info.LayoutStyle.WordWrap && info.LayoutStyle.MaximumWidth > 0)
                                textSize = TextDrawing.MeasureString(info.Graphics, text, font, info.LayoutStyle.MaximumWidth);
                            else if (info.ContextCell.Parent != null && info.ContextCell.Parent.Style != null && info.ContextCell.Parent.Style.WordWrap && info.ContextCell.Parent.Style.MaximumWidth > 0)
                                textSize = TextDrawing.MeasureString(info.Graphics, text, font, info.ContextCell.Parent.Style.MaximumWidth);
                            else
                                textSize = TextDrawing.MeasureString(info.Graphics, text, font, 0, eTextFormat.Left | eTextFormat.LeftAndRightPadding | eTextFormat.GlyphOverhangPadding | eTextFormat.NoPrefix);
#if (FRAMEWORK20)
                            if (!BarFunctions.IsVista && BarUtilities.UseTextRenderer) textSize.Width += 4;
#endif
                        }
                        else if (info.ContextCell.Images.LargestImageSize.IsEmpty && !info.ContextCell.CheckBoxVisible)
                        {
                            textSize = new Size(5, font.Height);
                        }
                    }
                    else
                    {
                        Size availSize = new Size(1600, 1);
                        if (info.LayoutStyle.WordWrap && info.LayoutStyle.MaximumWidth > 0)
                            availSize.Width = info.LayoutStyle.MaximumWidth;
                        else if (info.ContextCell.Parent != null && info.ContextCell.Parent.Style != null && info.ContextCell.Parent.Style.WordWrap && info.ContextCell.Parent.Style.MaximumWidth > 0)
                            availSize.Width = info.ContextCell.Parent.Style.MaximumWidth;

                        DevComponents.DotNetBar.TextMarkup.MarkupDrawContext d = new DevComponents.DotNetBar.TextMarkup.MarkupDrawContext(info.Graphics, font, Color.Empty, false);
                        info.ContextCell.TextMarkupBody.Measure(availSize, d);
                        availSize = info.ContextCell.TextMarkupBody.Bounds.Size;
                        d.RightToLeft = !info.LeftToRight;
                        info.ContextCell.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

                        textSize = info.ContextCell.TextMarkupBody.Bounds.Size;
                    }
                }
                else
                {
                    int availTextWidth = info.CellWidth -
                                       ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);

                    availTextWidth -= info.ContextCell.Images.LargestImageSize.Width +
                        (info.ContextCell.Images.LargestImageSize.Width > 0 ? ImageTextSpacing * 2 : 0);

                    if (info.ContextCell.CheckBoxVisible)
                        availTextWidth -= CheckBoxSize.Width + ImageTextSpacing * 2;

                    int cellHeight = font.Height;

                    if (info.LayoutStyle.WordWrap || info.ContextCell.TextMarkupBody != null)
                    {
                        cellHeight = info.LayoutStyle.MaximumHeight - info.LayoutStyle.MarginTop -
                                   info.LayoutStyle.MarginBottom - info.LayoutStyle.PaddingTop - info.LayoutStyle.PaddingBottom;

                        if (info.ContextCell.TextMarkupBody == null)
                        {
                            if (availTextWidth > 0)
                            {
                                if (cellHeight > 0)
                                    textSize = TextDrawing.MeasureString(info.Graphics, info.ContextCell.DisplayText, font, new Size(availTextWidth, cellHeight), info.LayoutStyle.TextFormat);
                                else
                                    textSize = TextDrawing.MeasureString(info.Graphics, info.ContextCell.DisplayText, font, availTextWidth, info.LayoutStyle.TextFormat);
                            }
                        }
                        else
                        {
                            Size availSize = new Size(availTextWidth, 1);
                            DevComponents.DotNetBar.TextMarkup.MarkupDrawContext d = new DevComponents.DotNetBar.TextMarkup.MarkupDrawContext(info.Graphics, font, Color.Empty, false);
                            info.ContextCell.TextMarkupBody.Measure(availSize, d);
                            availSize = info.ContextCell.TextMarkupBody.Bounds.Size;
                            availSize.Width = availTextWidth;
                            d.RightToLeft = !info.LeftToRight;
                            info.ContextCell.TextMarkupBody.Arrange(new Rectangle(0, 0, availSize.Width, availSize.Height), d);

                            textSize = info.ContextCell.TextMarkupBody.Bounds.Size;
                        }
                    }
                    else
                        textSize = new Size(availTextWidth, cellHeight);
                }
            }

            if (info.LayoutStyle.WordWrap)
                info.ContextCell.WordWrap = true;
            else
                info.ContextCell.WordWrap = false;

            height = (int)Math.Max(height, Math.Ceiling((double)textSize.Height));
            if (info.VerticalPartAlignment)
            {
                if (info.ContextCell.Images.LargestImageSize.Height > 0)
                    height += info.ContextCell.Images.LargestImageSize.Height + this.ImageTextSpacing;
                if (info.ContextCell.CheckBoxVisible)
                    height += CheckBoxSize.Height + this.ImageCheckBoxSpacing;
            }
            else
            {
                if (info.ContextCell.Images.LargestImageSize.Height > height)
                    height = info.ContextCell.Images.LargestImageSize.Height;
                if (info.ContextCell.CheckBoxVisible && CheckBoxSize.Height > height)
                    height = CheckBoxSize.Height;
            }

            Rectangle r = new Rectangle(info.Left + ElementStyleLayout.LeftWhiteSpace(info.LayoutStyle),
                                      info.Top + ElementStyleLayout.TopWhiteSpace(info.LayoutStyle)
                                      , info.CellWidth, height);

            if (r.Width == 0)
            {
                if (info.VerticalPartAlignment)
                {
                    r.Width = (int)Math.Ceiling((double)textSize.Width);
                    if (info.ContextCell.Images.LargestImageSize.Width > r.Width)
                        r.Width = (info.ContextCell.Images.LargestImageSize.Width + this.ImageTextSpacing);
                    if (info.ContextCell.CheckBoxVisible && CheckBoxSize.Width > r.Width)
                        r.Width += (CheckBoxSize.Width + this.ImageTextSpacing);
                }
                else
                {
                    r.Width = (int)Math.Ceiling((double)textSize.Width);
                    if (info.ContextCell.Images.LargestImageSize.Width > 0)
                        r.Width += (info.ContextCell.Images.LargestImageSize.Width + this.ImageTextSpacing);
                    if (info.ContextCell.CheckBoxVisible)
                        r.Width += (CheckBoxSize.Width + this.ImageTextSpacing);
                }
            }

            // Now that we have cell bounds store them
            Rectangle rCellBounds = new Rectangle(info.Left, info.Top, info.CellWidth, r.Height + info.LayoutStyle.MarginTop + info.LayoutStyle.MarginBottom + info.LayoutStyle.PaddingTop + info.LayoutStyle.PaddingBottom);
            if (rCellBounds.Width == 0)
                rCellBounds.Width = r.Width + ElementStyleLayout.HorizontalStyleWhiteSpace(info.LayoutStyle);
            info.ContextCell.SetBounds(rCellBounds);

            // Set position of the check box
            if (info.ContextCell.CheckBoxVisible && rCellBounds.Width >= this.CheckBoxSize.Width)
            {
                eVerticalAlign va = GetCheckBoxVerticalAlign(info.ContextCell.CheckBoxAlignment, info.View);
                eHorizontalAlign ha = GetCheckBoxHorizontalAlign(info.ContextCell.CheckBoxAlignment, info.LeftToRight, info.View);
                if (info.VerticalPartAlignment)
                    info.ContextCell.SetCheckBoxBounds(AlignContentVertical(this.CheckBoxSize, ref r, ha, va, this.ImageTextSpacing));
                else
                    info.ContextCell.SetCheckBoxBounds(AlignContent(this.CheckBoxSize, ref r, ha, va, this.ImageTextSpacing));
            }
            else
                info.ContextCell.SetCheckBoxBounds(Rectangle.Empty);

            // Set Position of the image
            if (!info.ContextCell.Images.LargestImageSize.IsEmpty && rCellBounds.Width >= info.ContextCell.Images.LargestImageSize.Width)
            {
                eVerticalAlign va = GetVerticalAlign(info.ContextCell.ImageAlignment, info.View);
                eHorizontalAlign ha = GetHorizontalAlign(info.ContextCell.ImageAlignment, info.LeftToRight, info.View);
                if (info.VerticalPartAlignment)
                    info.ContextCell.SetImageBounds(AlignContentVertical(info.ContextCell.Images.LargestImageSize, ref r, ha, va, this.ImageTextSpacing));
                else
                    info.ContextCell.SetImageBounds(AlignContent(info.ContextCell.Images.LargestImageSize, ref r, ha, va, this.ImageTextSpacing));
            }
            else
                info.ContextCell.SetImageBounds(Rectangle.Empty);

            // Set position of the text
            //info.ContextCell.SetTextBounds(Rectangle.Empty);
            if (!textSize.IsEmpty)
            {
                if (info.CellWidth > 0)
                    r.Width -= 2;
                if (info.View == eView.Tile && info.CellIndex == 0)
                    info.ContextCell.TextContentBounds = new Rectangle(r.X, r.Y, textSize.Width, textSize.Height + 1);
                else
                    info.ContextCell.TextContentBounds = r;
            }
            else
                info.ContextCell.TextContentBounds = Rectangle.Empty;

        }
    }
}
