#if FRAMEWORK20
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
    public class VS2008DockSuperTabItem : SuperTabItemBaseDisplay
    {
        public VS2008DockSuperTabItem(SuperTabItem tabItem)
            : base(tabItem)
        {
        }

        #region TabItemPath

        /// <summary>
        /// Creates the tab item GraphicsPath
        /// </summary>
        /// <returns>Tab path</returns>
        internal override GraphicsPath TabItemPath()
        {
            // Check to see if the user is supplying a path

            GraphicsPath path = base.TabItemPath();

            if (path != null)
                return (path);

            // Supply the default path based upon
            // the current tab alignment

            switch (TabItem.TabAlignment)
            {
                case eTabStripAlignment.Top:
                    return (TopTabPath());

                case eTabStripAlignment.Bottom:
                    return (BottomTabPath());

                case eTabStripAlignment.Left:
                    return (LeftTabPath());

                default:
                    return (RightTabPath());
            }
        }

        #region TopTabPath

        /// <summary>
        /// Create the Top tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath TopTabPath()
        {         
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Height += 2;

            // Create the path

            path.AddLines(new Point[]
            {
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
            });

            return (path);
        }

        #endregion

        #region BottomTabPath

        /// <summary>
        /// Creates the Bottom tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath BottomTabPath()
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border
            
            if (TabItem.IsSelected == true)
            {
                r.Y -= 2;
                r.Height += 2;
            }

            // Create the path
            
            path.AddLines(new Point[]
            {
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
            });

            return (path);
        }

        #endregion

        #region LeftTabPath

        /// <summary>
        /// Creates the Left tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath LeftTabPath()
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
                r.Width += 2;

            // Create the tab path

            path.AddLines(new Point[]
            {
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
            });

            return (path);
        }

        #endregion

        #region RightTabPath

        /// <summary>
        /// Create the Right tab path
        /// </summary>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath RightTabPath()
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = TabItem.DisplayRectangle;
            r.Width -= 1;
            r.Height -= 1;

            // Allow for the TabStrip border

            if (TabItem.IsSelected == true)
            {
                r.X -= 2;
                r.Width += 2;
            }

            // Create the tab path
            
            path.AddLines(new Point[]
            {
                new Point(r.X, r.Top),
                new Point(r.Right, r.Top),
                new Point(r.Right, r.Bottom),
                new Point(r.X, r.Bottom), 
            });

            return (path);
        }

        #endregion

        #endregion

        #region ApplyPredefinedColor

        /// <summary>
        /// Applies the predefined tab color to the color table
        /// </summary>
        /// <param name="tabState"></param>
        /// <param name="sct"></param>
        internal override void ApplyPredefinedColor(eTabState tabState, SuperTabItemStateColorTable sct)
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
                        ict.InnerBorder = Color.Empty;
                        break;
                }

                sct.InnerBorder = ict.InnerBorder;
                sct.OuterBorder = ict.OuterBorder;
                sct.Background = ict.Background;
            }
        }

        #endregion

        #region ApplyPredefinedColor

        /// <summary>
        /// Applies the predefined color to the panel color table
        /// </summary>
        /// <param name="pct"></param>
        internal override void ApplyPredefinedColor(SuperTabPanelItemColorTable pct)
        {
            if (TabItem.PredefinedColor != eTabItemColor.Default)
            {
                SuperTabPanelItemColorTable ct =
                    SuperTabStyleColorFactory.GetPredefinedPanelColors(TabItem.PredefinedColor, ColorFactory.Empty);

                pct.Background = ct.Background;
                pct.InnerBorder = pct.Background.Colors[pct.Background.Colors.Length - 1];
                pct.OuterBorder = ct.OuterBorder;
            }
        }

        #endregion

    }
}
#endif