#if FRAMEWORK20
using System.Drawing;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar
{
    internal class WinMediaPlayer12SuperTabStrip : SuperTabStripBaseDisplay
    {
        #region Constants

        private const int HvTabOverLap = -3;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabStripItem">Associated SuperTabStripItem</param>
        public WinMediaPlayer12SuperTabStrip(SuperTabStripItem tabStripItem)
            : base(tabStripItem)
        {
            tabStripItem.ControlBox.MenuBox.Style = eDotNetBarStyle.Office2010;
        }

        #region NextBlockPosition (LayoutManager)

        /// <summary>
        /// Gets the Layout manager NextBlockPosition
        /// </summary>
        /// <param name="e"></param>
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
                            e.NextPosition.Y += (e.Block.Bounds.Height - HvTabOverLap);
                        else
                            e.NextPosition.X += (e.Block.Bounds.Width - HvTabOverLap);

                        e.Cancel = true;
                    }
                }
            }
        }

        #endregion

        #region NextBlockPosition (PromoteSelTab)

        /// <summary>
        /// Gets the "PromoteSelTab" NextBlockPosition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="vItem"></param>
        /// <returns></returns>
        internal override Rectangle NextBlockPosition(BaseItem item, BaseItem vItem)
        {
            Rectangle r = base.NextBlockPosition(item, vItem);

            if (item is SuperTabItem && vItem is SuperTabItem)
            {
                if (TabStripItem.IsVertical == true)
                    r.Y -= HvTabOverLap;
                else
                    r.X -= HvTabOverLap;
            }

            return (r);
        }

        #endregion
    }
}
#endif