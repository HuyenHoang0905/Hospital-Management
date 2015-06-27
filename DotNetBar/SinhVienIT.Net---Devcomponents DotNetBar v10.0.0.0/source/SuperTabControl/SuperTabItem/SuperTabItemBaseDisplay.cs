#if FRAMEWORK20
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class SuperTabItemBaseDisplay : IDisposable
    {
        #region Private variables

        private SuperTabItem _TabItem;

        private Bitmap _CloseButtonBitmap;
        private Bitmap _CloseButtonHotBitmap;
        private Bitmap _CloseButtonPressedBitmap;
        private Color _CloseMarkerColor;

        private SuperTabItemColorTable _DefaultTabColorTable;
        private SuperTabPanelColorTable _DefaultPanelColorTable;

        private bool _CurrentlyAnimating;
        private Image _AnimateImage;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabItem">Associated SuperTabItem</param>
        public SuperTabItemBaseDisplay(SuperTabItem tabItem)
        {
            _TabItem = tabItem;

            SetDefaultColorTable();
        }

        #region Internal properties

        /// <summary>
        /// Gets or sets the default item ColorTable
        /// </summary>
        internal SuperTabItemColorTable DefaultItemColorTable
        {
            get { return (_DefaultTabColorTable); }
            set { _DefaultTabColorTable = value; }
        }

        #region TabItem

        internal SuperTabItem TabItem
        {
            get { return (_TabItem); }
        }

        #endregion

        #region TabStripItem

        internal SuperTabStripItem TabStripItem
        {
            get { return (_TabItem.TabStripItem); }
        }

        #endregion

        #region TabDisplay

        internal SuperTabStripBaseDisplay TabDisplay
        {
            get { return (_TabItem.TabStripItem.TabDisplay); }
        }

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Paint processing
        /// </summary>
        /// <param name="p"></param>
        public virtual void Paint(ItemPaintArgs p)
        {
            if (TabItem.Visible == true)
            {
                Graphics g = p.Graphics;

                if (TabStripItem.OnPreRenderTabItem(TabItem, g) == false)
                {
                    SuperTabItemStateColorTable colors = GetTabColorTable();

                    using (GraphicsPath path = TabItemPath())
                    {
                        DrawTabItemBackground(g, path, colors);

                        DrawTabContent(g, colors);
                        DrawTabBorder(g, path, colors);
                    }

                    TabStripItem.OnPostRenderTabItem(TabItem, g);
                }
            }
        }

        #endregion

        #region GetContentRectangle

        /// <summary>
        /// Gets the default tab ContentRectangle
        /// </summary>
        /// <returns>Rectangle</returns>
        internal virtual Rectangle ContentRectangle()
        {
            return (_TabItem.DisplayRectangle);
        }

        /// <summary>
        /// Gets the tab ContentRectangle
        /// </summary>
        /// <returns>Rectangle</returns>
        internal Rectangle GetContentRectangle()
        {
            Rectangle r = TabStripItem.OnGetTabItemContentRectangle(_TabItem);

            return (r);
        }

        #endregion

        #region TabItemPath

        /// <summary>
        /// Gets the tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        internal virtual GraphicsPath TabItemPath()
        {
            GraphicsPath path = TabStripItem.OnGetTabItemPath(_TabItem);

            return (path);
        }

        #endregion

        #region SetDefaultColorTable

        /// <summary>
        /// Sets the default color tables
        /// </summary>
        internal void SetDefaultColorTable()
        {
            _DefaultTabColorTable =
                SuperTabStyleColorFactory.GetItemColorTable(TabItem.TabStyle, ColorFactory.Empty);

            _DefaultPanelColorTable =
                SuperTabStyleColorFactory.GetPanelColorTable(TabItem.TabStyle, ColorFactory.Empty);
        }

        #endregion

        #region GetColorTable

        /// <summary>
        /// Gets the tab ColorTable
        /// </summary>
        /// <returns>ColorTable</returns>
        internal virtual SuperTabItemStateColorTable GetTabColorTable()
        {
            eTabState tabState = GetTabState();

            return (GetTabColorTable(tabState));
        }

        /// <summary>
        /// Gets the tab state
        /// </summary>
        /// <returns>eTabState</returns>
        private eTabState GetTabState()
        {
            if (TabItem == TabStripItem.HotTab)
                return (TabItem.IsSelected ? eTabState.SelectedMouseOver : eTabState.MouseOver);

            return (TabItem.IsSelected ? eTabState.Selected : eTabState.Default);
        }

        #region GetTabColorTable

        internal virtual SuperTabItemStateColorTable GetTabColorTable(eTabState tabState)
        {
            SuperTabItemStateColorTable sct = GetStateColorTable(tabState, _DefaultTabColorTable.Default);
            SuperTabItemStateColorTable tct = GetStateColorTable(tabState, TabItem.TabColor.Default);

            sct = (SuperTabItemStateColorTable)sct.Clone();
            tct = (SuperTabItemStateColorTable)tct.Clone();

            switch (TabItem.TabAlignment)
            {
                case eTabStripAlignment.Left:
                    ApplyTabColors(sct, GetStateColorTable(tabState, _DefaultTabColorTable.Left));
                    ApplyTabColors(tct, GetStateColorTable(tabState, TabItem.TabColor.Left));
                    break;

                case eTabStripAlignment.Bottom:
                    ApplyTabColors(sct, GetStateColorTable(tabState, _DefaultTabColorTable.Bottom));
                    ApplyTabColors(tct, GetStateColorTable(tabState, TabItem.TabColor.Bottom));
                    break;

                case eTabStripAlignment.Right:
                    ApplyTabColors(sct, GetStateColorTable(tabState, _DefaultTabColorTable.Right));
                    ApplyTabColors(tct, GetStateColorTable(tabState, TabItem.TabColor.Right));
                    break;
            }

            ApplyPredefinedColor(tabState, sct);
            ApplyTabColors(sct, tct);

            if (sct.SelectionMarker.IsEmpty == true &&
                (tabState == eTabState.Selected || tabState == eTabState.SelectedMouseOver))
            {
                SuperTabColorTable ct = TabDisplay.GetColorTable();

                sct.SelectionMarker = ct.SelectionMarker;
            }

            return (sct);
        }

        #endregion

        #region GetStateColorTable

        private SuperTabItemStateColorTable GetStateColorTable(
            eTabState tabState, SuperTabColorStates cs)
        {
            switch (tabState)
            {
                case eTabState.MouseOver:
                    return (cs.MouseOver);

                case eTabState.Selected:
                    return (cs.Selected);

                case eTabState.SelectedMouseOver:
                    return (cs.SelectedMouseOver);

                default:
                    return (cs.Normal);
            }
        }

        #endregion

        #region ApplyPredefinedColor

        internal virtual void ApplyPredefinedColor(eTabState tabState, SuperTabItemStateColorTable sct)
        {
            if (TabItem.PredefinedColor != eTabItemColor.Default)
            {
                SuperTabItemColorTable ct =
                    SuperTabStyleColorFactory.GetPredefinedTabColors(TabItem.PredefinedColor, ColorFactory.Empty);

                SuperTabItemStateColorTable ict = ct.Default.Normal;

                switch (tabState)
                {
                    case eTabState.SelectedMouseOver:
                        ict = ct.Default.SelectedMouseOver;
                        break;

                    case eTabState.MouseOver:
                        ict = ct.Default.MouseOver;
                        break;

                    case eTabState.Selected:
                        ict = ct.Default.Selected;
                        break;
                }

                sct.Text = ict.Text;
                sct.InnerBorder = ict.InnerBorder;
                sct.OuterBorder = ict.OuterBorder;
                sct.Background = ict.Background;
                sct.CloseMarker = ict.CloseMarker;
                sct.SelectionMarker = ict.SelectionMarker;
            }
        }

        #endregion

        #region ApplyTabColors

        private void ApplyTabColors(
            SuperTabItemStateColorTable sct, SuperTabItemStateColorTable tct)
        {
            if (tct.Background.IsEmpty == false)
            {
                if (tct.Background.IsEmpty == false)
                {
                    if (tct.Background.Colors != null)
                        sct.Background.Colors = tct.Background.Colors;

                    if (tct.Background.Positions != null)
                        sct.Background.Positions = tct.Background.Positions;

                    if (tct.Background.AdaptiveGradient.HasValue)
                        sct.Background.AdaptiveGradient = tct.Background.AdaptiveGradient;
                }

                sct.Background.GradientAngle = tct.Background.GradientAngle;
            }

            if (tct.InnerBorder != Color.Empty)
                sct.InnerBorder = tct.InnerBorder;

            if (tct.OuterBorder != Color.Empty)
                sct.OuterBorder = tct.OuterBorder;

            if (tct.Text != Color.Empty)
                sct.Text = tct.Text;

            if (tct.CloseMarker != Color.Empty)
                sct.CloseMarker = tct.CloseMarker;

            if (tct.SelectionMarker != Color.Empty)
                sct.SelectionMarker = tct.SelectionMarker;
        }

        #endregion

        #endregion

        #region GetPanelColorTable

        /// <summary>
        /// Gets the tab panel Color Table
        /// </summary>
        /// <returns>Color Table</returns>
        internal virtual SuperTabPanelItemColorTable GetPanelColorTable()
        {
            SuperTabPanelItemColorTable sct =
                (SuperTabPanelItemColorTable)_DefaultPanelColorTable.Default.Clone();

            SuperTabControlPanel cp = TabItem.AttachedControl as SuperTabControlPanel;

            if (cp != null)
            {
                SuperTabPanelItemColorTable tct =
                    (SuperTabPanelItemColorTable) cp.PanelColor.Default.Clone();

                switch (TabItem.TabAlignment)
                {
                    case eTabStripAlignment.Left:
                        ApplyPanelColors(sct, _DefaultPanelColorTable.Left);
                        ApplyPanelColors(tct, cp.PanelColor.Left);
                        break;

                    case eTabStripAlignment.Bottom:
                        ApplyPanelColors(sct, _DefaultPanelColorTable.Bottom);
                        ApplyPanelColors(tct, cp.PanelColor.Bottom);
                        break;

                    case eTabStripAlignment.Right:
                        ApplyPanelColors(sct, _DefaultPanelColorTable.Right);
                        ApplyPanelColors(tct, cp.PanelColor.Right);
                        break;
                }

                ApplyPredefinedColor(sct);
                ApplyPanelColors(sct, tct);
            }

            return (sct);
        }

        #region ApplyPredefinedColor

        internal virtual void ApplyPredefinedColor(SuperTabPanelItemColorTable pct)
        {
            if (TabItem.PredefinedColor != eTabItemColor.Default)
            {
                SuperTabPanelItemColorTable ct =
                    SuperTabStyleColorFactory.GetPredefinedPanelColors(TabItem.PredefinedColor, ColorFactory.Empty);

                pct.InnerBorder = ct.InnerBorder;
                pct.OuterBorder = ct.OuterBorder;

                pct.Background = ct.Background;
            }
        }

        #endregion

        #region ApplyPanelColors

        private void ApplyPanelColors(SuperTabPanelItemColorTable sct, SuperTabPanelItemColorTable tct)
        {
            if (tct.Background.IsEmpty == false)
            {
                if (tct.Background.Colors != null)
                    sct.Background.Colors = tct.Background.Colors;

                if (tct.Background.Positions != null)
                    sct.Background.Positions = tct.Background.Positions;

                if (tct.Background.AdaptiveGradient.HasValue)
                    sct.Background.AdaptiveGradient = tct.Background.AdaptiveGradient;

                sct.Background.GradientAngle = tct.Background.GradientAngle;
            }

            if (tct.InnerBorder.IsEmpty == false)
                sct.InnerBorder = tct.InnerBorder;

            if (tct.OuterBorder.IsEmpty == false)
                sct.OuterBorder = tct.OuterBorder;
        }

        #endregion

        #endregion

        #region DrawTabItemBackground

        /// <summary>
        /// Draws the tab background
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="path">Tab path</param>
        /// <param name="tabColors">StateColorTable</param>
        protected virtual void DrawTabItemBackground(
            Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors)
        {
            Rectangle r = Rectangle.Round(path.GetBounds());

            if (r.Width > 0 && r.Height > 0)
            {
                if (tabColors.Background.Colors != null)
                {
                    if (tabColors.Background.Colors.Length == 1)
                        DrawTabItemSolidBackground(g, path, tabColors, r);
                    else
                        DrawTabItemGradientBackground(g, path, tabColors, r);
                }
            }
        }

        #region DrawTabItemSolidBackground

        internal void DrawTabItemSolidBackground(
            Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
        {
            using (Brush br = tabColors.Background.GetBrush(r))
            {
                if (br != null)
                {
                    g.FillPath(br, path);

                    using (Pen pen = new Pen(tabColors.Background.Colors[0]))
                    {
                        int n = (_TabItem.IsSelected == true) ? 3 : 1;

                        for (int i = 0; i < n; i++)
                        {
                            switch (_TabItem.TabAlignment)
                            {
                                case eTabStripAlignment.Top:
                                    g.DrawLine(pen, r.X, r.Bottom + i, r.Right, r.Bottom + i);
                                    break;

                                case eTabStripAlignment.Left:
                                    g.DrawLine(pen, r.Right + i, r.Top, r.Right + i, r.Bottom);
                                    break;

                                case eTabStripAlignment.Bottom:
                                    g.DrawLine(pen, r.X, r.Top - i, r.Right, r.Top - i);
                                    break;

                                case eTabStripAlignment.Right:
                                    g.DrawLine(pen, r.X - i, r.Top, r.X - i, r.Bottom);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region DrawTabItemGradientBackground

        internal void DrawTabItemGradientBackground(
            Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
        {
            int angle = tabColors.Background.GradientAngle;

            if (tabColors.Background.AdaptiveGradient != false)
            {
                switch (_TabItem.TabAlignment)
                {
                    case eTabStripAlignment.Left:
                        angle -= 90;
                        break;

                    case eTabStripAlignment.Bottom:
                        angle += 180;
                        break;

                    case eTabStripAlignment.Right:
                        angle += 90;
                        break;
                }
            }

            using (LinearGradientBrush lbr =
                (LinearGradientBrush)tabColors.Background.GetBrush(r, angle))
            {
                if (lbr != null)
                {
                    lbr.WrapMode = WrapMode.TileFlipXY;

                    g.FillPath(lbr, path);

                    SmoothingMode sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.None;

                    int n = (_TabItem.IsSelected == true) ? 3 : 1;

                    switch (_TabItem.TabAlignment)
                    {
                        case eTabStripAlignment.Top:
                            g.FillRectangle(lbr, r.X, r.Bottom, r.Width, n);
                            break;

                        case eTabStripAlignment.Left:
                            g.FillRectangle(lbr, r.Right, r.Top, n, r.Height);
                            break;

                        case eTabStripAlignment.Bottom:
                            g.FillRectangle(lbr, r.X, r.Top - n + 1, r.Width, n);
                            break;

                        case eTabStripAlignment.Right:
                            g.FillRectangle(lbr, r.X - n + 1, r.Top, n, r.Height);
                            break;
                    }

                    g.SmoothingMode = sm;
                }
            }
        }

        #endregion

        #endregion

        #region DrawTabBorder

        /// <summary>
        /// Draws the tab border
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="path">Tab path</param>
        /// <param name="ct">Color table</param>
        protected virtual void DrawTabBorder(
            Graphics g, GraphicsPath path, SuperTabItemStateColorTable ct)
        {
            using (Pen pen = new Pen(ct.InnerBorder, 2))
            {
                Region rgn = g.Clip;
                g.SetClip(path);

                g.DrawPath(pen, path);
                g.DrawPath(pen, path);
                g.DrawPath(pen, path);
                g.DrawPath(pen, path);

                g.Clip = rgn;
            }

            using (Pen pen = new Pen(ct.OuterBorder))
                g.DrawPath(pen, path);
        }

        #endregion

        #region DrawTabContent

        /// <summary>
        /// Draws the tab contents
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="colors">State color table</param>
        protected virtual void DrawTabContent(Graphics g, SuperTabItemStateColorTable colors)
        {
            // Draw the close button

            if (TabStripItem.CloseButtonOnTabsVisible && TabItem.CloseButtonVisible)
            {
                if (TabStripItem.CloseButtonOnTabsAlwaysDisplayed == true || TabItem.IsMouseOver == true)
                    DrawCloseButton(g, colors);
            }

            // Draw image and text

            DrawTabImage(g);
            DrawTabText(g, colors);
        }

        #region DrawCloseButton

        /// <summary>
        /// Draws the tab close button
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="colors">State color table</param>
        protected virtual void DrawCloseButton(Graphics g, SuperTabItemStateColorTable colors)
        {
            Rectangle r = TabItem.CloseButtonBounds;

            if (TabItem.CloseButtonMouseOver == true || TabItem.CloseButtonPressed == true)
            {
                if (TabItem.CloseButtonMouseOver == true && TabItem.CloseButtonPressed == true)
                {
                    if (TabStripItem.TabCloseButtonPressed != null)
                        g.DrawImageUnscaled(TabStripItem.TabCloseButtonPressed, r);
                    else
                        g.DrawImageUnscaled(GetCloseButtonPressedBitmap(g), r);
                }
                else
                {
                    if (TabStripItem.TabCloseButtonHot != null)
                        g.DrawImageUnscaled(TabStripItem.TabCloseButtonHot, r);
                    else
                        g.DrawImageUnscaled(GetCloseButtonHotBitmap(g), r);
                }
            }
            else
            {
                if (TabStripItem.TabCloseButtonNormal != null)
                    g.DrawImageUnscaled(TabStripItem.TabCloseButtonNormal, r);
                else
                    g.DrawImageUnscaled(GetCloseButtonBitmap(g, colors), r);
            }
        }

        #endregion

        #region DrawTabImage

        /// <summary>
        /// Draws the tab image
        /// </summary>
        /// <param name="g">Graphics</param>
        protected virtual void DrawTabImage(Graphics g)
        {
            CompositeImage image = TabItem.GetTabImage();

            if (image != null)
            {
                if (_CurrentlyAnimating == true)
                {
                    if (TabItem.CanAnimateImage == false)
                        StopImageAnimation();
                }

                if (TabItem.CanAnimateImage == true)
                {
                    if (_CurrentlyAnimating == false)
                        StartImageAnimation(image.Image);

                    ImageAnimator.UpdateFrames();
                }

                Rectangle r = SetTransform(g, TabItem.ImageBounds);

                image.DrawImage(g, r);

                ResetTransform(g);
            }
        }

        #region StartImageAnimation

        internal void StartImageAnimation(Image image)
        {
            _CurrentlyAnimating = true;
            _AnimateImage = image;

            ImageAnimator.Animate(image, OnFrameChanged);
        }

        #endregion

        #region StopImageAnimation

        internal void StopImageAnimation()
        {
            if (_CurrentlyAnimating == true)
            {
                _CurrentlyAnimating = false;

                ImageAnimator.StopAnimate(_AnimateImage, OnFrameChanged);

                FrameDimension frameDimensions =
                    new FrameDimension(_AnimateImage.FrameDimensionsList[0]);

                _AnimateImage.SelectActiveFrame(frameDimensions, 0);
            }
        }

        #endregion

        #region OnFrameChanged

        private void OnFrameChanged(object o, EventArgs e)
        {
            if (_CurrentlyAnimating == true &&
                TabItem.Visible == true && TabItem.Parent != null)
            {
                TabItem.Refresh();
            }
            else
            {
                _CurrentlyAnimating = false;

                ImageAnimator.StopAnimate((Image)o, OnFrameChanged);
            }
        }

        #endregion

        #endregion

        #region DrawTabText

        /// <summary>
        /// Draws the tab text
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colors"></param>
        protected virtual void DrawTabText(Graphics g, SuperTabItemStateColorTable colors)
        {
            if (TabStripItem.DisplaySelectedTextOnly == false || TabItem.IsSelected == true)
            {
                Rectangle r = TabItem.TextBounds;
                Font font = TabItem.GetTabFont();
                
                if (TabItem.IsVertical == false)
                {
                    if (TabItem.TextMarkupBody != null)
                    {
                        r = AlignText(r);

                        TextMarkup.MarkupDrawContext d =
                            new TextMarkup.MarkupDrawContext(g, font, colors.Text, TabItem.IsRightToLeft);

                        TabItem.TextMarkupBody.Arrange(new
                            Rectangle(r.Location, TabItem.TextMarkupBody.Bounds.Size), d);

                        RenderMarkup(g, r, d);
                    }
                    else
                    {
                        eTextFormat strFormat;

                        r = AlignText(r, out strFormat);

                        TextDrawing.DrawString(g, TabItem.Text, font, colors.Text, r, strFormat);
                    }
                }
                else
                {
                    if (TabItem.TextMarkupBody != null)
                    {
                        Rectangle t = TabItem.TextMarkupBody.Bounds;

                        r = SetTransform(g, r);
                        r = AlignText(r);

                        TextMarkup.MarkupDrawContext d =
                            new TextMarkup.MarkupDrawContext(g, font, colors.Text, TabItem.IsRightToLeft);

                        TabItem.TextMarkupBody.Bounds = r;
                        RenderMarkup(g, r, d);
                        TabItem.TextMarkupBody.Bounds = t;
                    }
                    else
                    {
                        eTextFormat strFormat;

                        r = SetTransform(g, r);
                        r = AlignText(r, out strFormat);

                        TextDrawing.DrawStringLegacy(g, TabItem.Text, font, colors.Text, r, strFormat);
                    }

                    ResetTransform(g);
                }

                if (TabDisplay.ShowFocusRectangle && TabItem.IsSelected)
                    ControlPaint.DrawFocusRectangle(g, GetFocusRectangle(TabItem.DisplayRectangle));
            }
        }

        #region AlignText

        private Rectangle AlignText(Rectangle r, out eTextFormat strFormat)
        {
            strFormat = eTextFormat.Default | eTextFormat.SingleLine |
                eTextFormat.EndEllipsis | eTextFormat.VerticalCenter;

            eItemAlignment alignment = TabItem.TextAlignment.HasValue ?
                TabItem.TextAlignment.Value : TabItem.TabStripItem.TextAlignment;

            switch (alignment)
            {
                case eItemAlignment.Center:
                    strFormat |= eTextFormat.HorizontalCenter;

                    if (TabItem.IsVertical == true)
                    {
                        r.X -= TabStripItem.TabHorizontalSpacing;
                        r.Width += TabStripItem.TabHorizontalSpacing;
                    }
                    break;

                case eItemAlignment.Far:
                    strFormat |= eTextFormat.Right;

                    if (TabItem.IsVertical == true)
                    {
                        r.X -= 3;
                        r.Width -= 3;
                    }
                    break;
            }

            return (r);
        }

        private Rectangle AlignText(Rectangle r)
        {
            if (r.Height > TabItem.TextMarkupBody.Bounds.Height)
                r.Y += (r.Height - TabItem.TextMarkupBody.Bounds.Height) / 2;

            if (r.Width > TabItem.TextMarkupBody.Bounds.Width)
            {
                eItemAlignment alignment = TabItem.TextAlignment.HasValue ?
                    TabItem.TextAlignment.Value : TabItem.TabStripItem.TextAlignment;

                switch (alignment)
                {
                    case eItemAlignment.Center:
                        r.X += (r.Width - TabItem.TextMarkupBody.Bounds.Width) / 2;
                        break;

                    case eItemAlignment.Far:
                        r.X += (r.Width - TabItem.TextMarkupBody.Bounds.Width);
                        break;
                }
            }

            return (r);
        }

        #endregion

        #region RenderMarkup

        private void RenderMarkup(Graphics g,
            Rectangle r, TextMarkup.MarkupDrawContext d)
        {
            Region oldClip = g.Clip;
            Rectangle clipRect = r;

            g.SetClip(clipRect, CombineMode.Replace);

            TabItem.TextMarkupBody.Render(d);

            g.Clip = oldClip;
        }

        #endregion

        #endregion

        #endregion

        #region SetTransform

        /// <summary>
        /// Sets the GraphicsTransform for the given alignment
        /// and Horizontal text setting
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="r">Rectangle to transform</param>
        /// <returns>Transformed rectangle</returns>
        private Rectangle SetTransform(Graphics g, Rectangle r)
        {
            if (TabItem.IsVertical == true)
            {
                if (TabDisplay.Alignment == eTabStripAlignment.Left)
                {
                    g.RotateTransform(-90);
                    r = new Rectangle(-r.Bottom, r.X, r.Height, r.Width);
                }
                else
                {
                    g.RotateTransform(90);
                    r = new Rectangle(r.Top, -r.Right, r.Height, r.Width);
                }
            }

            return (r);
        }

        #endregion

        #region ResetTransform

        /// <summary>
        /// Resets out transform
        /// </summary>
        /// <param name="g">Graphics</param>
        private void ResetTransform(Graphics g)
        {
            if (TabItem.IsVertical == true)
                g.ResetTransform();
        }

        #endregion

        #region GetFocusRectangle

        /// <summary>
        /// Gets the focus rectangle
        /// </summary>
        /// <param name="rText"></param>
        /// <returns></returns>
        protected virtual Rectangle GetFocusRectangle(Rectangle rText)
        {
            rText.Inflate(-2, -2);

            return (rText);
        }

        #endregion

        #region GetCloseButtonBitmap

        /// <summary>
        /// Gets the tab close button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="colors">State color table</param>
        /// <returns>Button bitmap</returns>
        public Bitmap GetCloseButtonBitmap(Graphics g, SuperTabItemStateColorTable colors)
        {
            if (_CloseMarkerColor != colors.CloseMarker || _CloseButtonBitmap == null)
            {
                if (_CloseButtonBitmap != null)
                    _CloseButtonBitmap.Dispose();

                _CloseButtonBitmap = CreateCloseButtonBitmap(g, colors);

                _CloseMarkerColor = colors.CloseMarker;
            }

            return (_CloseButtonBitmap);
        }

        #endregion

        #region CreateCloseButtonBitmap

        /// <summary>
        /// Creates the close button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <param name="colors">State color table</param>
        /// <returns>Close bitmap</returns>
        public virtual Bitmap CreateCloseButtonBitmap(Graphics g, SuperTabItemStateColorTable colors)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                using (Pen pen = new Pen(colors.CloseMarker))
                {
                    gBmp.DrawLine(pen, new Point(4, 4), new Point(10, 9));
                    gBmp.DrawLine(pen, new Point(10, 4), new Point(4, 9));
                }
            }

            return (bmp);
        }

        #endregion

        #region GetCloseButtonHotBitmap

        /// <summary>
        /// Gets the hot close button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <returns>Close bitmap</returns>
        public Bitmap GetCloseButtonHotBitmap(Graphics g)
        {
            if (_CloseButtonHotBitmap == null)
                _CloseButtonHotBitmap = CreateCloseButtonHotBitmap(g);

            return (_CloseButtonHotBitmap);
        }

        #endregion

        #region CreateCloseButtonHotBitmap

        /// <summary>
        /// Creates the hot button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <returns>Close bitmap</returns>
        public virtual Bitmap CreateCloseButtonHotBitmap(Graphics g)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                Rectangle r = new Rectangle(0, 0, 15, 15);

                using (GraphicsPath path = GetClosePath(r, 4))
                {
                    using (LinearGradientBrush lbr = new
                        LinearGradientBrush(r, Color.White, Color.Pink, 90))
                    {
                        gBmp.FillPath(lbr, path);
                    }

                    r.Inflate(-1, -1);

                    using (GraphicsPath path2 = GetClosePath(r, 6))
                    {
                        using (LinearGradientBrush lbr = new
                            LinearGradientBrush(r, Color.Pink, Color.Crimson, 90))
                        {
                            gBmp.FillPath(lbr, path2);
                        }
                    }

                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        gBmp.DrawLine(pen, new Point(4, 4), new Point(10, 10));
                        gBmp.DrawLine(pen, new Point(10, 4), new Point(4, 10));
                    }

                    gBmp.DrawPath(Pens.DarkRed, path);
                }
            }

            return (bmp);
        }

        #endregion

        #region GetCloseButtonPressedBitmap

        /// <summary>
        /// Gets the pressed close button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <returns>Close bitmap</returns>
        public Bitmap GetCloseButtonPressedBitmap(Graphics g)
        {
            if (_CloseButtonPressedBitmap == null)
                _CloseButtonPressedBitmap = CreateCloseButtonPressedBitmap(g);

            return (_CloseButtonPressedBitmap);
        }

        #endregion

        #region CreateCloseButtonPressedBitmap

        /// <summary>
        /// Creates the pressed button bitmap
        /// </summary>
        /// <param name="g">Graphics</param>
        /// <returns>Close bitmap</returns>
        public virtual Bitmap CreateCloseButtonPressedBitmap(Graphics g)
        {
            Bitmap bmp = new Bitmap(16, 16, g);

            using (Graphics gBmp = Graphics.FromImage(bmp))
            {
                gBmp.CompositingQuality = CompositingQuality.HighQuality;
                gBmp.SmoothingMode = g.SmoothingMode;

                Rectangle r = new Rectangle(0, 0, 15, 15);

                using (GraphicsPath path = GetClosePath(r, 4))
                {
                    using (LinearGradientBrush lbr = new
                        LinearGradientBrush(r, Color.LightGray, Color.Red, 90))
                    {
                        gBmp.FillPath(lbr, path);
                    }

                    r.Inflate(-1, -1);

                    using (GraphicsPath path2 = GetClosePath(r, 6))
                    {
                        using (LinearGradientBrush lbr = new
                            LinearGradientBrush(r, Color.Red, Color.Maroon, 90))
                        {
                            gBmp.FillPath(lbr, path2);
                        }
                    }

                    using (Pen pen = new Pen(Color.LightGray, 2))
                    {
                        gBmp.DrawLine(pen, new Point(4, 4), new Point(10, 10));
                        gBmp.DrawLine(pen, new Point(10, 4), new Point(4, 10));
                    }

                    gBmp.DrawPath(Pens.DarkRed, path);
                }
            }

            return (bmp);
        }

        #endregion

        #region GetClosePath

        /// <summary>
        /// Gets the close button path
        /// </summary>
        /// <param name="r">Close rectangle</param>
        /// <param name="rad">Corner radius</param>
        /// <returns></returns>
        private GraphicsPath GetClosePath(Rectangle r, int rad)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle ar = new
                Rectangle(r.Right - rad, r.Bottom - rad, rad, rad);

            path.AddArc(ar, 0, 90);

            ar.X = r.X;
            path.AddArc(ar, 90, 90);

            ar.Y = r.Y;
            path.AddArc(ar, 180, 90);

            ar.X = r.Right - rad;
            path.AddArc(ar, 270, 90);

            path.CloseAllFigures();

            return (path);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            StopImageAnimation();
        }

        #endregion
    }
}
#endif