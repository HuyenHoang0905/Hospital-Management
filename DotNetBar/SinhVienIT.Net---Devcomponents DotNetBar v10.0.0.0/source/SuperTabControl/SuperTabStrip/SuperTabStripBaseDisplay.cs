#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    internal class SuperTabStripBaseDisplay
    {
        #region Constants

        private const int MarkerSize = 10;

        #endregion

        #region Private Variables

        private int _TabHeight;
        private SuperTabStripItem _TabStripItem;

        private SerialContentLayoutManager _ContentManager;
        private SuperTabItemLayoutManager _BlockManager;
        private SuperTabColorTable _DefaultColorTable;

        private Bitmap _CloseButton;
        private Color _CloseButtonColor;

        private Bitmap _MenuButton1;
        private Bitmap _MenuButton2;
        private Color _MenuButtonColor;

        private Bitmap _TopInsertMarker;
        private Bitmap _LeftInsertMarker;
        private Color _InsertMarkerColor;

		#endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public SuperTabStripBaseDisplay(SuperTabStripItem tabStripItem)
        {
            _TabStripItem = tabStripItem;

            _ContentManager = new SerialContentLayoutManager();
            _ContentManager.NextPosition += ContentManagerNextPosition;

            _BlockManager = new SuperTabItemLayoutManager(tabStripItem);

            _DefaultColorTable = SuperTabStyleColorFactory.GetColorTable(tabStripItem.TabStyle, ColorFactory.Empty);
        }

        #region Internal properties

        #region Alignment

        /// <summary>
        /// Gets the TabAlignment
        /// </summary>
        internal eTabStripAlignment Alignment
        {
            get { return (_TabStripItem.TabAlignment); }
        }

        #endregion

        #region DefaultColorTable

        /// <summary>
        /// gets or sets the default color table
        /// </summary>
        internal SuperTabColorTable DefaultColorTable
        {
            get { return (_DefaultColorTable); }

            set
            {
                _DefaultColorTable = value;

                for (int i = 0; i < Tabs.Count; i++)
                {
                    SuperTabItem tab = Tabs[i] as SuperTabItem;

                    if (tab != null)
                        tab.TabItemDisplay.SetDefaultColorTable();
                }
            }
        }

        #endregion

        #region IsRightToLeft

            /// <summary>
            /// Gets if display is RightToLeft
            /// </summary>
            internal bool IsRightToLeft
        {
            get { return (_TabStripItem.IsRightToLeft); }
        }

        #endregion

        #region IsVertical

        /// <summary>
        /// Gets tab vertical orientation
        /// </summary>
        internal bool IsVertical
        {
            get { return (_TabStripItem.IsVertical); }
        }

        #endregion

        #region MinTabStripHeight

        /// <summary>
        /// Gets the minimum TabStrip height
        /// </summary>
        internal virtual int MinTabStripHeight
        {
            get
            {
                if (TabItemsBounds.IsEmpty)
                    return (TabHeight + 6);

                if (Alignment == eTabStripAlignment.Top || Alignment == eTabStripAlignment.Bottom)
                    return (TabItemsBounds.Height + 2);

                return (TabItemsBounds.Width + 2);
            }
        }

        #endregion

        #region ShowFocusRectangle

        /// <summary>
        /// Gets whether we are to show the Focus rectangle
        /// </summary>
        internal bool ShowFocusRectangle
        {
            get { return (_TabStripItem.ShowFocusRectangle); }   // && _TabStripItem.Focused
        }

        #endregion

        #region Tab properties

        #region CloseButtonOnTabs

        /// <summary>
        /// Gets CloseButtonOnTabsVisible
        /// </summary>
        internal bool CloseButtonOnTabs
        {
            get { return (_TabStripItem.CloseButtonOnTabsVisible); }
        }

        #endregion

        #region FixedTabSize

        /// <summary>
        /// Gets FixedTabSize
        /// </summary>
        internal Size FixedTabSize
        {
            get { return (_TabStripItem.FixedTabSize); }
        }

        #endregion

        #region MinTabSize

        /// <summary>
        /// Gets the minimum tab size
        /// </summary>
        internal virtual Size MinTabSize
        {
            get { return (new Size(13, 13)); }
        }

        #endregion

        #region SelectedTab

        /// <summary>
        /// Gets the selected tab
        /// </summary>
        internal SuperTabItem SelectedTab
        {
            get { return (_TabStripItem.SelectedTab); }
        }

        #endregion

        #region SelectedTabFont

        /// <summary>
        /// Gets the selected font
        /// </summary>
        internal Font SelectedTabFont
        {
            get { return (_TabStripItem.SelectedTabFont); }
        }

        #endregion

        #region TabControlBox

        /// <summary>
        /// Gets the tab ControlBox
        /// </summary>
        internal SuperTabControlBox TabControlBox
        {
            get { return (_TabStripItem.ControlBox); }
        }

        #endregion

        #region TabHeight

        /// <summary>
        /// Gets the tab height
        /// </summary>
        internal int TabHeight
        {
            get
            {
                if (_TabHeight <= 0)
                {
                    _TabHeight = (FixedTabSize.Height > 0)
                        ? FixedTabSize.Height : GetTabHeight();
                }

                return (_TabHeight);
            }

            set { _TabHeight = value; }
        }

        #endregion

        #region TabItemsBounds

        /// <summary>
        /// Gets the tab bounds
        /// </summary>
        internal Rectangle TabItemsBounds
        {
            get { return (_TabStripItem.TabItemsBounds); }
            set { _TabStripItem.TabItemsBounds = value; }
        }

        #endregion

        #region Tabs

        /// <summary>
        /// Gets the Tabs collection
        /// </summary>
        internal SubItemsCollection Tabs
        {
            get { return (_TabStripItem.SubItems); }
        }

        #endregion

        #endregion

        #region TabStrip

        /// <summary>
        /// Gets the TabStrip
        /// </summary>
        internal SuperTabStrip TabStrip
        {
            get { return (TabStripItem.TabStrip); }
        }

        #endregion

        #region TabStripItem

        /// <summary>
        /// Gets the TabStripItem
        /// </summary>
        internal SuperTabStripItem TabStripItem
        {
            get { return (_TabStripItem); }
        }

        #endregion

        #region Virtual properties

        /// <summary>
        ///  Gets the SelectedPaddingWidth
        /// </summary>
        internal virtual int SelectedPaddingWidth
        {
            get { return (0); }
        }

        /// <summary>
        /// Gets the TabOverlap
        /// </summary>
        internal virtual int TabOverlap
        {
            get { return (0); }
        }

        /// <summary>
        /// Gets the TabSpacing
        /// </summary>
        internal virtual int TabSpacing
        {
            get { return (0); }
        }

        /// <summary>
        /// Gets the TabOverlapLeft
        /// </summary>
        internal virtual bool TabOverlapLeft
        {
            get { return (false); }
        }

        /// <summary>
        /// Gets the TabLayoutOffset
        /// </summary>
        internal virtual Size TabLayoutOffset
        {
            get { return (Size.Empty); }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Handles ContentManager NextPosition events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContentManagerNextPosition(object sender, LayoutManagerPositionEventArgs e)
        {
            NextBlockPosition(e);
        }

        #endregion

        #region RecalcSize

        /// <summary>
        /// Handles RecalcSize requests
        /// </summary>
        /// <param name="g"></param>
        internal virtual void RecalcSize(Graphics g)
        {
            RecalcLayoutInit(g);
            RecLayoutContent();

            int n = MinTabStripHeight;

            switch (_TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Left:
                case eTabStripAlignment.Right:
                    _TabStripItem.TabStrip.Width = n;

                    _TabStripItem.WidthInternal = n;
                    _TabStripItem.HeightInternal = _TabStripItem.TabStrip.Height;
                    break;

                default:
                    _TabStripItem.TabStrip.Height = n;

                    _TabStripItem.HeightInternal = n;
                    _TabStripItem.WidthInternal = _TabStripItem.TabStrip.Width;
                    break;
            }
        }

        #endregion

        #region RecLayoutContent

        /// <summary>
        /// Handles RecLayoutContent requests
        /// </summary>
        internal virtual void RecLayoutContent()
        {
            if (TabControlBox.MenuBox.AutoHide == true && TabControlBox.Visible == true)
            {
                RecalcLayout(GetTabClientArea(false));

                TabControlBox.MenuBox.Visible = !AllItemsVisible();
            }

            RecalcLayout(GetTabClientArea());
        }

        #endregion

        #region RecalcLayoutInit

        /// <summary>
        /// Initializes the layout engine
        /// </summary>
        /// <param name="g"></param>
        protected virtual void RecalcLayoutInit(Graphics g)
        {
            _BlockManager.Graphics = g;

            g.CompositingMode = g.CompositingMode;

            _BlockManager.FixedTabSize = FixedTabSize;
            _BlockManager.TabLayoutOffset = TabLayoutOffset;

            _ContentManager.RightToLeft = IsRightToLeft;
            _ContentManager.EvenHeight = true;

            switch (Alignment)
            {
                case eTabStripAlignment.Bottom:
                    _ContentManager.ContentAlignment = eContentAlignment.Left;
                    _ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.ContentOrientation = eContentOrientation.Horizontal;
                    break;

                case eTabStripAlignment.Top:
                    _ContentManager.ContentAlignment = eContentAlignment.Left;
                    _ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.BlockLineAlignment = eContentVerticalAlignment.Bottom;
                    _ContentManager.ContentOrientation = eContentOrientation.Horizontal;
                    break;

                case eTabStripAlignment.Left:
                    _ContentManager.ContentAlignment = eContentAlignment.Left;
                    _ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.ContentOrientation = eContentOrientation.Vertical;
                    break;

                case eTabStripAlignment.Right:
                    _ContentManager.ContentAlignment = eContentAlignment.Left;
                    _ContentManager.ContentVerticalAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.BlockLineAlignment = eContentVerticalAlignment.Top;
                    _ContentManager.ContentOrientation = eContentOrientation.Vertical;
                    break;
            }

            switch (_TabStripItem.TabLayoutType)
            {
                case eSuperTabLayoutType.SingleLine:
                    _ContentManager.MultiLine = false;
                    _ContentManager.FitContainer = false;
                    break;

                case eSuperTabLayoutType.SingleLineFit:
                    _ContentManager.MultiLine = false;
                    _ContentManager.FitContainer = true;
                    break;

                case eSuperTabLayoutType.MultiLine:
                    _ContentManager.MultiLine = true;
                    _ContentManager.FitContainer = false;
                    break;

                case eSuperTabLayoutType.MultiLineFit:
                    _ContentManager.MultiLine = true;
                    _ContentManager.FitContainer = true;
                    break;
            }
        }

        #endregion

        #region RecalcLayout

        /// <summary>
        /// Performs the Recalc Layout
        /// </summary>
        /// <param name="r"></param>
        protected virtual void RecalcLayout(Rectangle r)
        {
            if (Tabs.Count > 0)
            {
                IBlock[] blocks = new IBlock[Tabs.Count];

                for (int i = 0; i < Tabs.Count; i++)
                    blocks[i] = Tabs[i];

                if (IsRightToLeft)
                {
                    Rectangle tabArea = r;

                    if (_TabStripItem.ControlBox.Visible)
                        tabArea.X += TabControlBox.Size.Width;

                    TabItemsBounds = _ContentManager.Layout(tabArea, blocks, _BlockManager);
                    RecalcSizeTabControlBox(r, tabArea);
                }
                else
                {
                    TabItemsBounds = _ContentManager.Layout(r, blocks, _BlockManager);
                    RecalcSizeTabControlBox(r, TabItemsBounds);
                }
            }
            else
            {
                TabItemsBounds = Rectangle.Empty;
            }
        }

        #endregion

        #region RecalcSizeTabSystemBox

        /// <summary>
        /// Recalculates the bounds for the TabControlBox
        /// </summary>
        /// <param name="dispRect"></param>
        /// <param name="tabsBounds"></param>
        protected virtual void RecalcSizeTabControlBox(Rectangle dispRect, Rectangle tabsBounds)
        {
            if (_TabStripItem.ControlBox.Visible == true)
            {
                int dx, dy;
                Size boxSize = TabControlBox.Size;

                switch (Alignment)
                {
                    case eTabStripAlignment.Left:
                    case eTabStripAlignment.Right:
                        dx = dispRect.X + (dispRect.Width - boxSize.Width) / 2;
                        dy = _TabStripItem.DisplayRectangle.Bottom - boxSize.Height;
                        break;

                    default:
                        if (this.IsRightToLeft)
                        {
                            dx = tabsBounds.X + (dispRect.Width - boxSize.Width) / 2;
                            dy = _TabStripItem.DisplayRectangle.Bottom - boxSize.Height;
                        }
                        else
                        {
                            dx = _TabStripItem.DisplayRectangle.Right - boxSize.Width;
                            dy = tabsBounds.Y + (dispRect.Height - boxSize.Height) / 2;
                        }
                        break;
                }

                TabControlBox.Bounds =
                    new Rectangle(dx, dy, boxSize.Width, boxSize.Height);
            }
        }

        #endregion

        #region GetTabClientArea

        /// <summary>
        /// Gets the tab client area
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetTabClientArea()
        {
            return (GetTabClientArea(TabControlBox.MenuBox.Visible));
        }

        /// <summary>
        /// Gets the tab client area
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetTabClientArea(bool mbVisible)
        {
            Rectangle r = new Rectangle();
            r.Size = this.TabStripItem.TabStrip.Size;

            if (TabStripItem.ControlBox.Visible == true)
            {
                if (Alignment == eTabStripAlignment.Right || Alignment == eTabStripAlignment.Left)
                {
                    r.Height -= TabStripItem.ControlBox.Size.Height;

                    if (TabStripItem.ControlBox.MenuBox.Visible != mbVisible)
                    {
                        if (mbVisible == true)
                            r.Height -= TabStripItem.ControlBox.MenuBox.HeightInternal;
                        else
                            r.Height += TabStripItem.ControlBox.MenuBox.HeightInternal;
                    }

                    if (r.Height < TabStripItem.ControlBox.Size.Height)
                        r.Height = TabStripItem.ControlBox.Size.Height;
                }
                else
                {
                    r.Width -= TabStripItem.ControlBox.Size.Width;

                    if (TabStripItem.ControlBox.MenuBox.Visible != mbVisible)
                    {
                        if (mbVisible == true)
                            r.Width -= TabStripItem.ControlBox.MenuBox.WidthInternal;
                        else
                            r.Width += TabStripItem.ControlBox.MenuBox.WidthInternal;
                    }

                    if (r.Width < TabStripItem.ControlBox.Size.Width)
                        r.Width = TabStripItem.ControlBox.Size.Width;
                }
            }

            return (r);
        }

        #endregion

        #region GetTabHeight

        /// <summary>
        /// Gets the tab height
        /// </summary>
        /// <returns></returns>
        protected virtual int GetTabHeight()
        {
            if (FixedTabSize.Height > 0)
                return (FixedTabSize.Height);

            int iHeight = (Alignment == eTabStripAlignment.Left ||
                Alignment == eTabStripAlignment.Right) ? 20 : 16;

            if (TabStrip.ImageList != null)
                iHeight = TabStrip.ImageList.ImageSize.Height;

            Font font = (_TabStripItem.SelectedTabFont ?? _TabStripItem.TabFont) ?? TabStrip.Font;

            if (font.Height > iHeight)
                iHeight = font.Height;

            iHeight += 4;

            return (iHeight);
        }

        #endregion

        #region NextBlockPosition

        /// <summary>
        /// Layout manager NextBlockPosition
        /// </summary>
        /// <param name="e"></param>
        protected virtual void NextBlockPosition(LayoutManagerPositionEventArgs e)
        {
        }

        /// <summary>
        /// PromoteTab NextBlockPosition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vItem"></param>
        /// <returns></returns>
        internal virtual Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
        {
            Rectangle r = item.Bounds;

            if (IsVertical == true)
            {
                r.X = vItem.Bounds.X;
                r.Y += item.Bounds.Height;
            }
            else
            {
                r.X += item.Bounds.Width;
                r.Y = vItem.Bounds.Y;
            }

            r.Size = vItem.Size;

            return (r);
        }

        #endregion

        #region AllItemsVisible

        /// <summary>
        /// Determines if all tabs are visible
        /// </summary>
        /// <returns></returns>
        private bool AllItemsVisible()
        {
            foreach (BaseItem item in Tabs)
            {
                if (item.Visible == true && item.Displayed == false)
                    return (false);
            }

            return (true);
        }

        #endregion

        #region Paint

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="p"></param>
        public void Paint(ItemPaintArgs p)
        {
            Rectangle r = _TabStripItem.DisplayRectangle;

            if (r.Width > 0 && r.Height > 0)
            {
                SuperTabColorTable ct = GetColorTable();

                DrawBackground(p, ct);

                _TabStripItem.OnPaintBackground(p.Graphics, ct);

                if (Tabs.Count > 0)
                {
                    // Draw the ControlBox

                    if (_TabStripItem.ControlBox.Visible == true)
                        TabControlBox.Paint(p);

                    DrawStripBorder(p, ct);
                    DrawTabs(p);
                    DrawTabInsertMarker(p, ct);
                }
            }
        }

        #region DrawBackground

        /// <summary>
        /// Draws the background
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ct"></param>
        protected virtual void DrawBackground(ItemPaintArgs p, SuperTabColorTable ct)
        {
            Graphics g = p.Graphics;
            Rectangle r = _TabStripItem.DisplayRectangle;

            int angle = ct.Background.GradientAngle;

            switch (_TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Bottom:
                    angle += 180;
                    r.Y -= 1;
                    break;

                case eTabStripAlignment.Left:
                    angle += -90;
                    break;

                case eTabStripAlignment.Right:
                    angle += 90;
                    r.X -= 1;
                    break;
            }

            using (Brush lbr = ct.Background.GetBrush(r, angle))
            {
                if (lbr != null)
                    g.FillRectangle(lbr, r);
            }
        }

        #endregion

        #region DrawTabs

        /// <summary>
        /// Draws the tabs
        /// </summary>
        /// <param name="p"></param>
        protected virtual void DrawTabs(ItemPaintArgs p)
        {
            // Draw all but the selected tab item

            for (int i = Tabs.Count - 1; i >= 0; i--)
            {
                BaseItem item = Tabs[i];

                if (item != null && item.Displayed == true)
                {
                    SuperTabItem tab = item as SuperTabItem;

                    if (tab == null || tab.IsSelected == false)
                        item.Paint(p);
                }
            }

            // Draw the selected tab

            if (SelectedTab != null && SelectedTab.Displayed == true)
                SelectedTab.Paint(p);
            
            // Lastly draw tab insert markers

            for (int i = Tabs.Count - 1; i >= 0; i--)
            {
                BaseItem item = Tabs[i];

                if (item != null && item.Displayed == true)
                    item.DrawInsertMarker(p.Graphics);
            }
        }

        #endregion

        #region DrawTabInsertMarker

        #region DrawTabInsertMarker

        /// <summary>
        /// Draws the Drag and Drop insert marker
        /// </summary>
        /// <param name="p">ItemPaintArgs</param>
        /// <param name="ct">Color table</param>
        private void DrawTabInsertMarker(ItemPaintArgs p, SuperTabColorTable ct)
        {
            if (_TabStripItem.InsertTab != null)
            {
                if (_InsertMarkerColor != ct.InsertMarker)
                {
                    if (_TopInsertMarker != null)
                    {
                        _TopInsertMarker.Dispose();
                        _TopInsertMarker = null;
                    }

                    if (_LeftInsertMarker != null)
                    {
                        _LeftInsertMarker.Dispose();
                        _LeftInsertMarker = null;
                    }

                    _InsertMarkerColor = ct.InsertMarker;
                }

                switch (_TabStripItem.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                    case eTabStripAlignment.Bottom:
                        DrawTopInsertMarker(p.Graphics);
                        break;

                    case eTabStripAlignment.Left:
                    case eTabStripAlignment.Right:
                        DrawLeftInsertMarker(p.Graphics);
                        break;
                }
            }
        }

        #endregion

        #region DrawTopInsertMarker

        /// <summary>
        /// Draws the top insert marker
        /// </summary>
        /// <param name="g"></param>
        private void DrawTopInsertMarker(Graphics g)
        {
            Rectangle r = GetTopInsertRect();
            Bitmap b = GetTopInsertMarker(g);

            g.DrawImageUnscaled(b, r);
        }

        #region GetTopInsertRect

        private Rectangle GetTopInsertRect()
        {
            BaseItem item = _TabStripItem.InsertTab;
            SuperTabStripBaseDisplay tabDisplay = _TabStripItem.TabDisplay;

            int x1 = item.Bounds.X;
            int x2 = item.Bounds.Right;

            int n = Tabs.IndexOf(item);

            if (n > 0)
            {
                if (IsSameLine(item, Tabs[n - 1]) == true)
                {
                    if (tabDisplay.TabOverlap > 0)
                    {
                        if (tabDisplay.TabOverlapLeft == true)
                            x1 = Tabs[n - 1].Bounds.Right;
                    }
                    else
                    {
                        x1 = (item.Bounds.X + Tabs[n - 1].Bounds.Right) / 2;
                    }
                }
            }

            if (n + 1 < Tabs.Count)
            {
                if (IsSameLine(item, Tabs[n + 1]) == true)
                {
                    if (tabDisplay.TabOverlap > 0)
                    {
                        if (tabDisplay.TabOverlapLeft == false)
                            x2 = Tabs[n + 1].Bounds.X;
                    }
                    else
                    {
                        x2 = (item.Bounds.Right + Tabs[n + 1].Bounds.X) / 2;
                    }
                }
            }

            Rectangle r = item.Bounds;

            r.X = x1;
            r.Width = x2 - x1;
            r.Height = MarkerSize;

            if (_TabStripItem.InsertBefore == true)
            {
                r.X -= MarkerSize / 2;

                if (r.X < 0)
                    r.X = 0;
            }
            else
            {
                r.X = r.Right - MarkerSize / 2;

                if (r.X + MarkerSize > _TabStripItem.Bounds.Right)
                    r.X = _TabStripItem.Bounds.Right - MarkerSize;
            }

            r.Width = MarkerSize;

            return (r);
        }

        #endregion

        #region GetTopInsertMarker

        private Bitmap GetTopInsertMarker(Graphics g)
        {
            if (_TopInsertMarker == null)
                _TopInsertMarker = CreateTopInsertMarker(g);

            return (_TopInsertMarker);
        }

        #endregion

        #region CreateTopInsertMarker

        private Bitmap CreateTopInsertMarker(Graphics g)
        {
            Bitmap bmp = new Bitmap(MarkerSize, MarkerSize, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.SmoothingMode = g.SmoothingMode;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddPolygon(new Point[]
                    {
                        new Point(6, 0),
                        new Point(6, 5),
                        new Point(9, 5),
                        new Point(5, 9),
                        new Point(4, 9),
                        new Point(0, 5),
                        new Point(3, 5),
                        new Point(3, 0),
                        new Point(6, 0),
                    });

                    path.CloseFigure();

                    Color color = Color.FromArgb(200, _InsertMarkerColor);

                    using (SolidBrush br = new SolidBrush(color))
                        gBmp.FillPath(br, path);

                    using (Pen pen = new Pen(color))
                        gBmp.DrawPath(pen, path);
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region DrawLeftInsertMarker

        /// <summary>
        /// Draws the left insert marker
        /// </summary>
        /// <param name="g"></param>
        private void DrawLeftInsertMarker(Graphics g)
        {
            Rectangle r = GetLeftInsertRect();
            Bitmap b = GetLeftInsertMarker(g);

            g.DrawImageUnscaled(b, r);
        }

        #region GetLeftInsertRect

        private Rectangle GetLeftInsertRect()
        {
            BaseItem item = _TabStripItem.InsertTab;
            SuperTabStripBaseDisplay tabDisplay = _TabStripItem.TabDisplay;

            int y1 = item.Bounds.Y;
            int y2 = item.Bounds.Bottom;

            int n = Tabs.IndexOf(item);

            if (n > 0)
            {
                if (IsSameLine(item, Tabs[n - 1]) == true)
                {
                    if (tabDisplay.TabOverlap > 0)
                    {
                        if (tabDisplay.TabOverlapLeft == true)
                            y1 = Tabs[n - 1].Bounds.Bottom;
                    }
                    else
                    {
                        y1 = (item.Bounds.Y + Tabs[n - 1].Bounds.Bottom) / 2;
                    }
                }
            }

            if (n + 1 < Tabs.Count)
            {
                if (IsSameLine(item, Tabs[n + 1]) == true)
                {
                    if (tabDisplay.TabOverlap > 0)
                    {
                        if (tabDisplay.TabOverlapLeft == false)
                            y2 = Tabs[n + 1].Bounds.Y;
                    }
                    else
                    {
                        y2 = (item.Bounds.Bottom + Tabs[n + 1].Bounds.Y) / 2;
                    }
                }
            }

            Rectangle r = item.Bounds;
            r.Y = y1;
            r.Height = y2 - y1;
            r.Width = MarkerSize;

            if (_TabStripItem.InsertBefore == true)
            {
                r.Y -= MarkerSize / 2;

                if (r.Y < 0)
                    r.Y = 0;
            }
            else
            {
                r.Y = r.Bottom - MarkerSize / 2;

                if (r.Y + MarkerSize > _TabStripItem.Bounds.Bottom)
                    r.Y = _TabStripItem.Bounds.Bottom - MarkerSize;
            }

            r.Height = MarkerSize;

            return (r);
        }

        #endregion

        #region GetLeftInsertMarker

        private Bitmap GetLeftInsertMarker(Graphics g)
        {
            if (_LeftInsertMarker == null)
                _LeftInsertMarker = CreateLeftInsertMarker(g);

            return (_LeftInsertMarker);
        }

        #endregion

        #region CreateLeftInsertMarker

        private Bitmap CreateLeftInsertMarker(Graphics g)
        {
            Bitmap bmp = new Bitmap(MarkerSize, MarkerSize, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.SmoothingMode = g.SmoothingMode;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddPolygon(new Point[]
                     {
                         new Point(0, 6),
                         new Point(5, 6),
                         new Point(5, 9),
                         new Point(9, 5),
                         new Point(9, 4),
                         new Point(5, 0),
                         new Point(5, 3),
                         new Point(0, 3),
                         new Point(0, 6),
                     });

                    path.CloseFigure();

                    Color color = Color.FromArgb(200, _InsertMarkerColor);

                    using (SolidBrush br = new SolidBrush(color))
                        gBmp.FillPath(br, path);

                    using (Pen pen = new Pen(color))
                        gBmp.DrawPath(pen, path);
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region IsSameLine

        /// <summary>
        /// Determines if the given items are on the same line
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        private bool IsSameLine(BaseItem item1, BaseItem item2)
        {
            if (_TabStripItem.TabLines == 1)
                return (true);

            switch (_TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    return (item1.Bounds.Bottom == item2.Bounds.Bottom);

                case eTabStripAlignment.Left:
                    return (item1.Bounds.Right == item2.Bounds.Right);

                case eTabStripAlignment.Bottom:
                    return (item1.Bounds.Top == item2.Bounds.Top);

                default:
                    return (item1.Bounds.Left == item2.Bounds.Left);
            }
        }

        #endregion

        #endregion

        #region DrawStripBorder

        /// <summary>
        /// Draws the TabStrip border
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ct"></param>
        private void DrawStripBorder(ItemPaintArgs p, SuperTabColorTable ct)
        {
            Graphics g = p.Graphics;

            Rectangle r = TabStripItem.Bounds;

            Rectangle t = TabStripItem.SelectedTab != null ?
                TabStripItem.SelectedTab.DisplayRectangle : Rectangle.Empty;

            using (Pen iPen = new Pen(ct.InnerBorder))
            {
                using (Pen oPen = new Pen(ct.OuterBorder))
                {
                    int inner, outer;
                    GetBorderLines(out inner, out outer);

                    switch (TabStripItem.TabAlignment)
                    {
                        case eTabStripAlignment.Top:
                        case eTabStripAlignment.Bottom:
                            if (t.X > r.X)
                            {
                                g.DrawLine(iPen, r.X, inner, t.X, inner);
                                g.DrawLine(oPen, r.X, outer, t.X, outer);
                            }

                            if (t.Right < r.Right)
                            {
                                g.DrawLine(iPen, t.Right - 1, inner, r.Right - 1, inner);
                                g.DrawLine(oPen, t.Right, outer, r.Right - 2, outer);
                            }

                            g.DrawLine(oPen, r.X, outer, r.X, inner);
                            g.DrawLine(oPen, r.Right - 1, outer, r.Right - 1, inner);
                            break;

                        case eTabStripAlignment.Left:
                        case eTabStripAlignment.Right:
                            if (t.Y > 0)
                            {
                                g.DrawLine(iPen, inner, r.Y, inner, t.Y + 1);
                                g.DrawLine(oPen, outer, r.Y, outer, t.Y);
                            }

                            if (t.Bottom < r.Bottom)
                            {
                                g.DrawLine(iPen, inner, t.Bottom - 2, inner, r.Bottom - 1);
                                g.DrawLine(oPen, outer, t.Bottom - 1, outer, r.Bottom - 1);
                            }

                            g.DrawLine(oPen, inner, r.Y, outer, r.Y);
                            g.DrawLine(oPen, inner, r.Bottom - 1, outer, r.Bottom - 1);
                            break;
                    }
                }
            }
        }

        #region GetBorderLines

        /// <summary>
        /// Gets the inner and outer border lines
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="outer"></param>
        private void GetBorderLines(out int inner, out int outer)
        {
            Rectangle r = TabStripItem.Bounds;

            switch (TabStripItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    inner = r.Bottom - 1;
                    outer = r.Bottom - 2;
                    break;

                case eTabStripAlignment.Bottom:
                    inner = r.Y;
                    outer = r.Y + 1;
                    break;

                case eTabStripAlignment.Left:
                    inner = r.Right - 1;
                    outer = r.Right - 2;
                    break;

                default:
                    inner = r.X;
                    outer = r.X + 1;
                    break;
            }
        }

        #endregion

        #endregion

        #endregion

        #region SetDefaultColorTable

        /// <summary>
        /// Sets the default color table
        /// </summary>
        internal void SetDefaultColorTable()
        {
            DefaultColorTable =
                SuperTabStyleColorFactory.GetColorTable(TabStrip.TabStyle, ColorFactory.Empty);
        }

        #endregion

        #region GetColorTable

        /// <summary>
        /// Gets the tab color table
        /// </summary>
        /// <returns>Tab color table</returns>
        internal virtual SuperTabColorTable GetColorTable()
        {
            SuperTabColorTable ct = TabStripItem.TabStripColor;
            SuperTabColorTable sct = (SuperTabColorTable)_DefaultColorTable.Clone();
            
            if (ct != null)
            {
                if (ct.Background.IsEmpty == false)
                    sct.Background = ct.Background;

                if (ct.InnerBorder != Color.Empty)
                    sct.InnerBorder = ct.InnerBorder;

                if (ct.OuterBorder != Color.Empty)
                    sct.OuterBorder = ct.OuterBorder;

                GetControlBoxColors(sct.ControlBoxDefault, ct.ControlBoxDefault);
                GetControlBoxColors(sct.ControlBoxMouseOver, ct.ControlBoxMouseOver);
                GetControlBoxColors(sct.ControlBoxPressed, ct.ControlBoxPressed);

                if (ct.InsertMarker != Color.Empty)
                    sct.InsertMarker = ct.InsertMarker;

                if (ct.SelectionMarker != Color.Empty)
                    sct.SelectionMarker = ct.SelectionMarker;
            }

            return (sct);
        }

        private void GetControlBoxColors(
            SuperTabControlBoxStateColorTable sbc, SuperTabControlBoxStateColorTable bc)
        {
            if (bc.Background != Color.Empty)
                sbc.Background = bc.Background;

            if (bc.Border != Color.Empty)
                sbc.Border = bc.Border;

            if (bc.Image != Color.Empty)
                sbc.Image = bc.Image;
        }

        #endregion

        #region GetCloseButton

        /// <summary>
        /// Gets the tab close button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Bitmap GetCloseButton(Graphics g, Color color)
        {
            if (color != _CloseButtonColor && _CloseButton != null)
            {
                _CloseButton.Dispose();
                _CloseButton = null;
            }

            _CloseButtonColor = color;

            if (_CloseButton == null)
                _CloseButton = CreateCloseButton(g);

            return (_CloseButton);
        }

        #region CreateCloseButton

        /// <summary>
        /// Creates the tab close button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public virtual Bitmap CreateCloseButton(Graphics g)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                using (Pen pen = new Pen(_CloseButtonColor, 2))
                {
                    gBmp.DrawLine(pen, new Point(3, 3), new Point(11, 11));
                    gBmp.DrawLine(pen, new Point(11, 3), new Point(3, 11));
                }
            }

            return (bmp);
        }

        #endregion

        #endregion

        #region GetMenuButton1

        /// <summary>
        /// Gets the Menu button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Bitmap GetMenuButton1(Graphics g, Color color)
        {
            if (color != _MenuButtonColor)
            {
                if (_MenuButton1 != null)
                {
                    _MenuButton1.Dispose();
                    _MenuButton1 = null;
                }

                if (_MenuButton2 != null)
                {
                    _MenuButton2.Dispose();
                    _MenuButton2 = null;
                }
            }

            _MenuButtonColor = color;

            if (_MenuButton1 == null)
                _MenuButton1 = CreateMenuButton1(g);

            return (_MenuButton1);
        }


        #region CreateMenuButton1

        /// <summary>
        /// Create the Menu button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public virtual Bitmap CreateMenuButton1(Graphics g)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                using (GraphicsPath path = GetMenuButtonPath1())
                {
                    using (Brush br = new SolidBrush(_MenuButtonColor))
                        gBmp.FillPath(br, path);

                    using (Pen pen = new Pen(_MenuButtonColor))
                        gBmp.DrawPath(pen, path);
                }
            }

            return (bmp);
        }

        #endregion

        #region GetMenuButtonPath1

        /// <summary>
        /// Gets the Menu button path
        /// </summary>
        /// <returns></returns>
        private GraphicsPath GetMenuButtonPath1()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[] 
            {
                new Point(4, 5), 
                new Point(8, 9), 
                new Point(12, 5) 
            });

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #endregion

        #region GetMenuButton2

        /// <summary>
        /// Gets the Menu button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Bitmap GetMenuButton2(Graphics g, Color color)
        {
            if (color != _MenuButtonColor)
            {
                if (_MenuButton1 != null)
                {
                    _MenuButton1.Dispose();
                    _MenuButton1 = null;
                }

                if (_MenuButton2 != null)
                {
                    _MenuButton2.Dispose();
                    _MenuButton2 = null;
                }
            }

            _MenuButtonColor = color;

            if (_MenuButton2 == null)
                _MenuButton2 = CreateMenuButton2(g);

            return (_MenuButton2);
        }

        #region CreateMenuButton2

        /// <summary>
        /// Create the Menu button bitmap
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public virtual Bitmap CreateMenuButton2(Graphics g)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                using (GraphicsPath path = GetMenuButtonPath2())
                {
                    using (Brush br = new SolidBrush(_MenuButtonColor))
                        gBmp.FillPath(br, path);

                    using (Pen pen = new Pen(_MenuButtonColor))
                        gBmp.DrawPath(pen, path);
                }
            }

            return (bmp);
        }

        #endregion

        #region GetMenuButtonPath2

        /// <summary>
        /// Gets the Menu button path
        /// </summary>
        /// <returns></returns>
        private GraphicsPath GetMenuButtonPath2()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddLines(new Point[]
            {
                new Point(3, 4),
                new Point(6, 7),
                new Point(3, 11)
            });

            path.CloseFigure();

            path.AddLines(new Point[]
            {
                new Point(9, 4),
                new Point(12, 7),
                new Point(9, 11)
            });

            path.CloseFigure();

            return (path);
        }

        #endregion

        #endregion
    }
}
#endif