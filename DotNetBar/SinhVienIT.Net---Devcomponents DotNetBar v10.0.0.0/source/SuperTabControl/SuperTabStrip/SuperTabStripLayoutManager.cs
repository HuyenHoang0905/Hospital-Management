#if FRAMEWORK20
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    public class SuperTabItemLayoutManager : BlockLayoutManager
    {
        #region Private variables

        private int _SelLine;

        private Size _CloseButtonSize = new Size(16, 16);
        private Size _FixedTabSize = Size.Empty;
        private Size _TabLayoutOffset = Size.Empty;

        private SuperTabStripItem _TabStripItem;
        private Dictionary<BaseItem, Size> _List;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SuperTabItemLayoutManager()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public SuperTabItemLayoutManager(SuperTabStripItem tabStripItem)
        {
            _TabStripItem = tabStripItem;

            _List = new Dictionary<BaseItem, Size>();
        }

        #region Public properties

        /// <summary>
        /// gets or sets the FixedTabSize
        /// </summary>
        public Size FixedTabSize
        {
            get { return _FixedTabSize; }
            set { _FixedTabSize = value; }
        }

        /// <summary>
        /// Gets or sets the TabLayoutOffset
        /// </summary>
        public Size TabLayoutOffset
        {
            get { return _TabLayoutOffset; }
            set { _TabLayoutOffset = value; }
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets the TabDisplay
        /// </summary>
        internal SuperTabStripBaseDisplay TabDisplay
        {
            get { return (_TabStripItem.TabDisplay); }
        }

        #endregion

        #region Layout

        /// <summary>
        /// Resizes the content block and sets it's Bounds property to reflect new size.
        /// </summary>
        /// <param name="block">Content block to resize.</param>
        /// <param name="availableSize"></param>
        public override void Layout(IBlock block, Size availableSize)
        {
            if (block.Visible == true)
            {
                if (block is SuperTabItem)
                    LayoutTab(block as SuperTabItem);

                else if (block is BaseItem)
                    LayoutBaseItem(block as BaseItem);
            }
        }

        #region LayoutTab

        private void LayoutTab(SuperTabItem tab)
        {
            Size size = new Size();
            Rectangle bounds = Rectangle.Empty;

            if (_FixedTabSize.Width <= 0 || _FixedTabSize.Height <= 0 ||
                tab.TextMarkupBody != null)
            {
                size = MeasureTab(tab);
            }

            // If the user has set a FixedTabSize, then apply it

            if (_FixedTabSize.Width > 0)
                size.Width = _FixedTabSize.Width;

            if (_FixedTabSize.Height > 0)
                size.Height = _FixedTabSize.Height;

            // Make sure we keep the tab size within the tab style minimum

            Size minSize = TabDisplay.MinTabSize;

            if (size.Width < minSize.Width)
                size.Width = minSize.Width;

            if (size.Height < minSize.Height)
                size.Height = minSize.Height;

            bounds.Size = (tab.IsVertical == true)
                         ? new Size(size.Height, size.Width) : size;

            // Let the user have one last crack at measuring the tab

            bounds.Size = _TabStripItem.OnMeasureTabItem(tab, bounds.Size, Graphics);

            tab.Bounds = bounds;
            tab.RecalcSize();
        }

        #endregion

        #region MeasureTab

        /// <summary>
        /// Measures the given tab
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        private Size MeasureTab(SuperTabItem tab)
        {
            Size size = new Size(0, 0);

            // Measure the tab Text

            Font font = tab.GetTabFont();

            Size textSize = (tab.TextMarkupBody != null)
                ? MeasureMarkup(tab, Graphics, font) : MeasureText(tab, Graphics, font);

            textSize.Width += _TabLayoutOffset.Width;
            textSize.Height += _TabLayoutOffset.Height;

            bool displayText =
                (tab.IsSelected || _TabStripItem.DisplaySelectedTextOnly == false);

            // Add in our overlap, spacing, and selection padding

            if (_TabStripItem.IsVertical == false || _TabStripItem.HorizontalText == false)
            {
                if (displayText == true)
                    size.Width += (textSize.Width + _TabStripItem.TabHorizontalSpacing);

                if (textSize.Height > size.Height)
                    size.Height = textSize.Height;

                size.Width += (TabDisplay.TabOverlap + TabDisplay.TabSpacing);
            }
            else
            {
                if (displayText == true)
                {
                    if (textSize.Height > size.Height)
                        size.Height = textSize.Height;
                }

                size.Height += (TabDisplay.TabOverlap + TabDisplay.TabSpacing);
                size.Width += (textSize.Width + _TabStripItem.TabHorizontalSpacing + TabDisplay.SelectedPaddingWidth);
            }

            // Add in any image space needed

            CompositeImage tabImage = tab.GetTabImage();

            if (tabImage != null)
            {
                size.Width += (tabImage.Width + _TabStripItem.TabHorizontalSpacing);

                if (size.Height < tabImage.Height + _TabStripItem.TabVerticalSpacing)
                    size.Height = tabImage.Height + _TabStripItem.TabVerticalSpacing;

                int n = tabImage.Height +
                        _TabStripItem.TabDisplay.TabOverlap + _TabStripItem.TabDisplay.TabSpacing + 4;

                if (size.Height < n)
                    size.Height = n;
            }

            // Add in the Close Button

            if (TabDisplay.CloseButtonOnTabs && tab.CloseButtonVisible)
            {
                size.Width += (_CloseButtonSize.Width + _TabStripItem.TabHorizontalSpacing);

                if (size.Height < _CloseButtonSize.Height + _TabStripItem.TabVerticalSpacing)
                    size.Height = _CloseButtonSize.Height + _TabStripItem.TabVerticalSpacing;

                if (size.Height < _CloseButtonSize.Height + 4)
                    size.Height = _CloseButtonSize.Height + 4;
            }

            // Add our default spacing

            size.Width += _TabStripItem.TabHorizontalSpacing * 2;
            size.Height += _TabStripItem.TabVerticalSpacing * 2;

            return (size);
        }

        #endregion

        #region MeasureMarkup

        /// <summary>
        /// Measures the markup text
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="g"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        private Size MeasureMarkup(SuperTabItem tab, Graphics g, Font font)
        {
            TextMarkup.MarkupDrawContext d =
                new TextMarkup.MarkupDrawContext(g, font, Color.Black, false);

            Size size = new Size(5000, 5000);

            tab.TextMarkupBody.InvalidateElementsSize();
            tab.TextMarkupBody.Measure(size, d);

            d.RightToLeft = _TabStripItem.IsRightToLeft;

            tab.TextMarkupBody.Arrange(new
                Rectangle(Point.Empty, tab.TextMarkupBody.Bounds.Size), d);

            size = tab.TextMarkupBody.Bounds.Size;

            return (size);
        }

        #endregion

        #region MeasureText

        /// <summary>
        /// Measures the given text
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="g"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        private Size MeasureText(SuperTabItem tab, Graphics g, Font font)
        {
            string text = string.IsNullOrEmpty(tab.Text) ? "M" : tab.Text;

            Size textSize = tab.IsVertical
               ? TextDrawing.MeasureStringLegacy(g, text, font, Size.Empty, eTextFormat.Default)
               : TextDrawing.MeasureString(g, text, font, 0, eTextFormat.Default);

            return (textSize);
        }

        #endregion

        #region LayoutBaseItem

        private void LayoutBaseItem(BaseItem item)
        {
            Size size = new Size();
            Rectangle bounds = Rectangle.Empty;

            if (_FixedTabSize.Width <= 0 || _FixedTabSize.Height <= 0)
            {
                item.RecalcSize();

                size = item.Bounds.Size;

                size.Width += _TabLayoutOffset.Width;
                size.Height += _TabLayoutOffset.Height;

                size.Width += _TabStripItem.ItemPadding.Horizontal;
                size.Height += _TabStripItem.ItemPadding.Vertical;
            }

            if (_FixedTabSize.Width > 0)
                size.Width = _FixedTabSize.Width;

            if (_FixedTabSize.Height > 0)
                size.Height = _FixedTabSize.Height;

            bounds.Size = size;

            item.Bounds = bounds;

            if (item.Stretch == false)
                _List[item] = size;
        }

        #endregion

        #endregion

        #region FinalizeLayout

        /// <summary>
        /// Finalizes the layout
        /// </summary>
        /// <param name="containerBounds"></param>
        /// <param name="blocksBounds"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public override Rectangle FinalizeLayout(
            Rectangle containerBounds, Rectangle blocksBounds, ArrayList lines)
        {
            if (lines.Count > 1)
            {
                int line = FindSelectedLine(lines);

                ReorderLines(lines, line);
                SetBlockDisplay(containerBounds, lines);
                ResizeMultiLine(lines);
            }
            else
            {
                SetBlockDisplay(containerBounds, lines);
                PromoteSelectedTab(containerBounds, lines);
                ResizeSingleLine(lines);
            }

            _TabStripItem.TabLines = lines.Count;

            _List.Clear();

            return (blocksBounds);
        }

        #endregion

        #region ReorderLines

        /// <summary>
        /// Reorders the lines to keep the selected tab as line 0.
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="line"></param>
        private void ReorderLines(ArrayList lines, int line)
        {
            _SelLine = 0;

            switch (_TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                case eTabStripAlignment.Left:
                    line++;
                    _SelLine = lines.Count - line;
                    break;

                default:
                    _SelLine = line;
                    break;
            }

            if (_SelLine > 0)
                AdjustLines(lines, line);
        }

        #region AdjustLines

        /// <summary>
        /// Adjusts the line layout
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="n"></param>
        private void AdjustLines(ArrayList lines, int n)
        {
            Size[] szs = GetLineSizes(lines);

            Point pt = new Point();

            for (int i = 0; i < lines.Count; i++)
            {
                int t = (i + n) % lines.Count;

                SerialContentLayoutManager.BlockLineInfo info =
                    lines[t] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    AdjustBlocks(info, pt);

                    pt.X += szs[t].Width;
                    pt.Y += szs[t].Height;
                }
            }
        }

        #endregion

        #region GetLineSizes

        /// <summary>
        /// Gets the array od line sizes
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private Size[] GetLineSizes(ArrayList lines)
        {
            Size[] szs = new Size[lines.Count];

            for (int i = 0; i < lines.Count; i++)
            {
                SerialContentLayoutManager.BlockLineInfo info =
                    lines[i] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    IBlock item = info.Blocks[0] as IBlock;

                    if (item != null)
                        szs[i] = item.Bounds.Size;
                }
            }

            return (szs);
        }

        #endregion

        #region AdjustBlocks

        /// <summary>
        /// Adjusts the individual blocks within a given line
        /// </summary>
        /// <param name="info"></param>
        /// <param name="pt"></param>
        private void AdjustBlocks(SerialContentLayoutManager.BlockLineInfo info, Point pt)
        {
            for (int i = 0; i < info.Blocks.Count; i++)
            {
                BaseItem item = info.Blocks[i] as BaseItem;

                if (item != null)
                {
                    switch (_TabStripItem.TabAlignment)
                    {
                        case eTabStripAlignment.Top:
                        case eTabStripAlignment.Bottom:
                            item.TopInternal = pt.Y;
                            break;

                        default:
                            item.LeftInternal = pt.X;
                            break;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region SetBlockDisplay

        #region SetBlockDisplay

        /// <summary>
        /// Sets the Displayed status for the line blocks
        /// </summary>
        /// <param name="containerBounds"></param>
        /// <param name="lines"></param>
        private void SetBlockDisplay(
            Rectangle containerBounds, ArrayList lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                SerialContentLayoutManager.BlockLineInfo info =
                    lines[i] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    bool checkBreaks = true;

                    // Set each item's display state, and
                    // align each item if they aren't being stretched

                    for (int j = 0; j < info.Blocks.Count; j++)
                    {
                        BaseItem item = info.Blocks[j] as BaseItem;

                        if (item != null)
                        {
                            bool displayed = item.Visible;

                            if (info.Blocks.Count > 1 && containerBounds.Contains(item.Bounds) == false)
                            {
                                checkBreaks = false;
                                displayed = false;
                            }

                            item.Displayed = displayed;

                            if (item.Displayed == true)
                                AlignBaseItem(item);
                        }
                    }

                    // Check item breaks, if we have extra
                    // room to spare

                    if (checkBreaks == true)
                    {
                        Size size;
                        int count = GetBreakCount(info, out size);

                        if (count > 0)
                        {
                            if (_TabStripItem.IsVertical)
                                ProcessBreaks(info, count, containerBounds.Height - size.Height, true);
                            else
                                ProcessBreaks(info, count, containerBounds.Width - size.Width, false);
                        }
                    }
                }
            }
        }

        #endregion

        #region AlignBaseItem

        private void AlignBaseItem(BaseItem item)
        {
            Rectangle r = item.Bounds;

            if (item.Stretch == false)
            {
                Size size;

                if (_List.TryGetValue(item, out size) == true)
                {
                    r.Size = size;

                    r.Y = item.Bounds.Y + (item.Bounds.Height - r.Height)/2;

                    switch (item.ItemAlignment)
                    {
                        case eItemAlignment.Center:
                            r.X += (item.Bounds.Width - r.Width)/2;
                            break;

                        case eItemAlignment.Far:
                            r.X = item.Bounds.Right - r.Width;
                            break;
                    }
                }
            }

            // Take user specified padding into account
            // for all non-SuperTabItems only

            if (item is SuperTabItem == false)
            {
                r.X += _TabStripItem.ItemPadding.Left;
                r.Y += _TabStripItem.ItemPadding.Top;

                r.Width -= (_TabStripItem.ItemPadding.Horizontal);
                r.Height -= (_TabStripItem.ItemPadding.Vertical);
            }

            item.Bounds = r;
        }

        #endregion

        #region GetBreakCount

        /// <summary>
        /// Gets the count of BeginBreak groups
        /// </summary>
        /// <param name="info"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private int GetBreakCount(
            SerialContentLayoutManager.BlockLineInfo info, out Size size)
        {
            int cnt = 0;
            size = Size.Empty;

            for (int i = 0; i < info.Blocks.Count; i++)
            {
                BaseItem item = info.Blocks[i] as BaseItem;

                if (item != null)
                {
                    if (item.Displayed == true)
                    {
                        size.Width = item.Bounds.Right;
                        size.Height = item.Bounds.Bottom;

                        if (item.BeginGroup == true)
                            cnt++;
                    }
                }
            }

            return (cnt);
        }

        #endregion

        #region ProcessBreaks

        /// <summary>
        /// Process item BeginBreaks
        /// </summary>
        /// <param name="info"></param>
        /// <param name="count"></param>
        /// <param name="span"></param>
        /// <param name="vertical"></param>
        private void ProcessBreaks(SerialContentLayoutManager.BlockLineInfo info,
            int count, int span, bool vertical)
        {
            int dcnt = 0;
            float delta = 0;
            float n = (float) span / count;

            for (int i = 0; i < info.Blocks.Count; i++)
            {
                BaseItem item = info.Blocks[i] as BaseItem;

                if (item != null && item.Displayed == true)
                {
                    if (item.BeginGroup == true)
                        delta = (++dcnt == count) ? span : delta + n;

                    if (delta > 0)
                    {
                        if (vertical == true)
                            item.TopInternal += (int) delta;
                        else
                            item.LeftInternal += (int) delta;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region PromoteSelectedTab

        /// <summary>
        /// Makes sure the "VisibleTab" is in fact visible.
        /// </summary>
        /// <param name="containerBounds"></param>
        /// <param name="lines"></param>
        private void PromoteSelectedTab(Rectangle containerBounds, ArrayList lines)
        {
            BaseItem vItem = _TabStripItem.VisibleTab;

            if (vItem != null && vItem.Displayed == false)
            {
                SerialContentLayoutManager.BlockLineInfo info =
                    lines[0] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    for (int i = info.Blocks.Count - 1; i >= 0; i--)
                    {
                        BaseItem item = info.Blocks[i] as BaseItem;

                        if (item != null)
                        {
                            Rectangle r = _TabStripItem.TabDisplay.NextBlockPosition(item, vItem);

                            if (containerBounds.Contains(r) == true)
                            {
                                vItem.Bounds = r;
                                vItem.Displayed = true;

                                break;
                            }

                            item.Displayed = false;

                            if (i == 0)
                            {
                                r = item.Bounds;
                                r.Size = vItem.Size;

                                vItem.Bounds = r;
                                vItem.Displayed = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region FindSelectedLine

        /// <summary>
        /// Determines what line th selected tab lies within
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private int FindSelectedLine(ArrayList lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                SerialContentLayoutManager.BlockLineInfo line =
                    lines[i] as SerialContentLayoutManager.BlockLineInfo;

                if (line != null)
                {
                    int iBlock = FindSelectedBlock(line.Blocks);

                    if (iBlock >= 0)
                        return (i);
                }
            }

            return (-1);
        }

        #endregion

        #region FindSelectedBlock

        /// <summary>
        /// Finds the SelectedTab block
        /// </summary>
        /// <param name="blocks"></param>
        /// <returns></returns>
        private int FindSelectedBlock(ArrayList blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                SuperTabItem tab = blocks[i] as SuperTabItem;

                if (tab != null)
                {
                    if (tab == _TabStripItem.SelectedTab)
                        return (i);
                }
            }

            return (-1);
        }

        #endregion

        #region ResizeSingleLine

        /// <summary>
        /// Resizes single line layouts
        /// </summary>
        /// <param name="lines"></param>
        private void ResizeSingleLine(ArrayList lines)
        {
            SerialContentLayoutManager.BlockLineInfo info =
                lines[0] as SerialContentLayoutManager.BlockLineInfo;

            if (info != null)
            {
                switch (_TabStripItem.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                        ResizeSingleLine1(info, 0, _TabLayoutOffset.Height, 0, -_TabLayoutOffset.Height);
                        break;

                    case eTabStripAlignment.Left:
                        ResizeSingleLine1(info, _TabLayoutOffset.Width, 0, -_TabLayoutOffset.Width, 0);
                        break;

                    case eTabStripAlignment.Bottom:
                        ResizeSingleLine2(info, 0, 2, 0, -_TabLayoutOffset.Height);
                        break;

                    case eTabStripAlignment.Right:
                        ResizeSingleLine2(info, 2, 0, -_TabLayoutOffset.Width, 0);
                        break;
                }
            }
        }

        #region ResizeSingleLine1

        private void ResizeSingleLine1(
            SerialContentLayoutManager.BlockLineInfo info, int dx, int dy, int dw, int dh)
        {
            for (int i = 0; i < info.Blocks.Count; i++)
            {
                BaseItem item = info.Blocks[i] as BaseItem;

                if (item != null)
                {
                    SuperTabItem tab = info.Blocks[i] as SuperTabItem;

                    if (tab == null || tab.IsSelected == false)
                    {
                        Rectangle r = item.Bounds;

                        r.X += dx;
                        r.Y += dy;
                        r.Width += dw;
                        r.Height += dh;

                        item.Bounds = r;
                    }
                }
            }
        }

        #endregion

        #region ResizeSingleLine2

        private void ResizeSingleLine2(
            SerialContentLayoutManager.BlockLineInfo info, int dx, int dy, int dw, int dh)
        {
            for (int i = 0; i < info.Blocks.Count; i++)
            {
                BaseItem item = info.Blocks[i] as BaseItem;

                if (item != null)
                {
                    Rectangle r = item.Bounds;
                    SuperTabItem tab = info.Blocks[i] as SuperTabItem;

                    r.X += dx;
                    r.Y += dy;

                    if (tab == null || tab.IsSelected == false)
                    {
                        r.Width += dw;
                        r.Height += dh;
                    }

                    item.Bounds = r;
                }
            }
        }

        #endregion

        #endregion

        #region ResizeMultiLine

        /// <summary>
        /// Resizes multiline layouts
        /// </summary>
        /// <param name="lines"></param>
        private void ResizeMultiLine(ArrayList lines)
        {
            switch (_TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    ResizeMultiLineTop();
                    break;

                case eTabStripAlignment.Left:
                    ResizeMultiLineLeft();
                    break;

                case eTabStripAlignment.Bottom:
                    ResizeMultiLineBottom(lines);
                    break;

                case eTabStripAlignment.Right:
                    ResizeMultiLineRight(lines);
                    break;
            }
        }

        #region ResizeMultiLineTop

        private void ResizeMultiLineTop()
        {
            if (_TabStripItem.SelectedTab != null)
            {
                Rectangle r = _TabStripItem.SelectedTab.Bounds;

                r.Y -= _TabLayoutOffset.Height;
                r.Height += _TabLayoutOffset.Height;

                _TabStripItem.SelectedTab.Bounds = r;
            }
        }

        #endregion

        #region ResizeMultiLineLeft

        private void ResizeMultiLineLeft()
        {
            if (_TabStripItem.SelectedTab != null)
            {
                Rectangle r = _TabStripItem.SelectedTab.Bounds;

                r.X -= _TabLayoutOffset.Width;
                r.Width += _TabLayoutOffset.Width;

                _TabStripItem.SelectedTab.Bounds = r;
            }
        }

        #endregion

        #region ResizeMultiLineBottom

        private void ResizeMultiLineBottom(ArrayList lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                SerialContentLayoutManager.BlockLineInfo info =
                    lines[i] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    for (int j = 0; j < info.Blocks.Count; j++)
                    {
                        BaseItem item = info.Blocks[j] as BaseItem;

                        if (item != null)
                        {
                            Rectangle r = item.Bounds;

                            r.Y += 2;

                            SuperTabItem tab = info.Blocks[j] as SuperTabItem;

                            if (tab != null && tab.IsSelected == true)
                                r.Height += _TabLayoutOffset.Height;

                            item.Bounds = r;
                        }
                    }
                }
            }
        }

        #endregion

        #region ResizeMultiLineRight

        private void ResizeMultiLineRight(ArrayList lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                SerialContentLayoutManager.BlockLineInfo info =
                    lines[i] as SerialContentLayoutManager.BlockLineInfo;

                if (info != null)
                {
                    for (int j = 0; j < info.Blocks.Count; j++)
                    {
                        BaseItem item = info.Blocks[j] as BaseItem;

                        if (item != null)
                        {
                            Rectangle r = item.Bounds;

                            r.X += 2;

                            SuperTabItem tab = info.Blocks[j] as SuperTabItem;

                            if (tab != null && tab.IsSelected == true)
                                r.Width += _TabLayoutOffset.Width;

                            item.Bounds = r;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
#endif