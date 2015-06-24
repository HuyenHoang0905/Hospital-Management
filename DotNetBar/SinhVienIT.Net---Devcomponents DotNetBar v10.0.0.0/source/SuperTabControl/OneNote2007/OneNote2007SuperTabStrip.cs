#if FRAMEWORK20
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    internal class OneNote2007SuperTabStrip : SuperTabStripBaseDisplay
    {
        #region Constants

        private const int HTabSpacing = 5;
        private const int HTabOverLap = 10;

        private const int VTabSpacing = 7;
        private const int VTabOverLap = 10;

        #endregion

        /// <summary>
        /// OneNote2007 SuperTabStripBaseDisplay
        /// </summary>
        /// <param name="tabStripItem">Associated TabStripItem</param>
        public OneNote2007SuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
        }

        #region Internal properties

        #region MinTabSize

        /// <summary>
        /// Returns the Minimum tab size for this style
        /// </summary>
        internal override Size MinTabSize
        {
            get
            {
                if (TabStripItem.IsVertical == true)
                    return (new Size(28, 30));

                return (new Size(28, 16));
            }
        }

        #endregion

        #region TabOverlap

        /// <summary>
        /// Tab Overlap
        /// </summary>
        internal override int TabOverlap
        {
            get { return (TabStripItem.IsVertical ? VTabOverLap : HTabOverLap); }
        }

        #endregion

        #region TabSpacing

        /// <summary>
        /// Tab Spacing
        /// </summary>
        internal override int TabSpacing
        {
            get
            {
                switch (TabStripItem.TabAlignment)
                {
                    case eTabStripAlignment.Top:
                    case eTabStripAlignment.Bottom:
                        return (HTabSpacing);

                    case eTabStripAlignment.Right:
                        return (TabStripItem.HorizontalText == false ? HTabSpacing : VTabSpacing);

                    default:
                        return (VTabSpacing);
                }
            }
        }

        #endregion

        #endregion

        #region NextBlockPosition

        /// <summary>
        /// Gets the next layout block position
        /// </summary>
        /// <param name="e">LayoutManagerPositionEventArgs</param>
        protected override void NextBlockPosition(LayoutManagerPositionEventArgs e)
        {
            int n = Tabs.IndexOf((BaseItem)e.Block);

            if (n >= 0 && n + 1 < Tabs.Count)
            {
                SuperTabItem tab1 = Tabs[n] as SuperTabItem;

                if (tab1 != null)
                {
                    SuperTabItem tab2 = Tabs[n + 1] as SuperTabItem;

                    if (tab2 != null)
                    {
                        e.NextPosition = e.CurrentPosition;

                        if (TabStripItem.IsVertical == true)
                            e.NextPosition.Y += (e.Block.Bounds.Height - VTabOverLap);
                        else
                            e.NextPosition.X += (e.Block.Bounds.Width - HTabOverLap);

                        e.Cancel = true;
                    }
                }
            }
        }

        #endregion

        #region NextBlockPosition

        /// <summary>
        /// Gets the next block position when attempting
        /// to make a specific tab visible
        /// </summary>
        /// <param name="item">Potential item to replace</param>
        /// <param name="vItem">View item being placed</param>
        /// <returns>Block Rectangle</returns>
        internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
        {
            Rectangle r = base.NextBlockPosition(item, vItem);

            if (item is SuperTabItem && vItem is SuperTabItem)
            {
                if (TabStripItem.IsVertical == true)
                    r.Y -= VTabOverLap;
                else
                    r.X -= HTabOverLap;
            }

            return (r);
        }

        #endregion
    }
}
#endif