using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for RenderRibbonTabGroup event.
    /// </summary>
    public class RibbonTabGroupRendererEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets or sets RibbonTabItemGroup being rendered.
        /// </summary>
        public RibbonTabItemGroup RibbonTabItemGroup = null;
        /// <summary>
        /// Gets or sets the bounds of the tab group. Bounds specified here are bounds of the tab group title. GroupBounds contains the bounds
        /// that include all tabs that belong to the tab group.
        /// </summary>
        public Rectangle Bounds = Rectangle.Empty;
        /// <summary>
        /// Gets or sets the font that should be used to render group text.
        /// </summary>
        public Font GroupFont = null;
        /// <summary>
        /// Gets or sets group bounds including the tabs that belong to the group.
        /// </summary>
        public Rectangle GroupBounds = Rectangle.Empty;
        /// <summary>
        /// Gets or sets the effective style for the group.
        /// </summary>
        public eDotNetBarStyle EffectiveStyle = eDotNetBarStyle.Office2007;

        /// <summary>
        /// Gets whether Windows Vista glass is enabled.
        /// </summary>
        public ItemPaintArgs ItemPaintArgs = null;

        public RibbonTabGroupRendererEventArgs(Graphics g, RibbonTabItemGroup group, Rectangle bounds, Rectangle groupBounds, Font font, ItemPaintArgs pa, eDotNetBarStyle effectiveStyle)
        {
            this.Graphics = g;
            this.RibbonTabItemGroup = group;
            this.Bounds = bounds;
            this.GroupBounds = groupBounds;
            this.GroupFont = font;
            this.ItemPaintArgs = pa;
            this.EffectiveStyle = effectiveStyle;
        }
    }
}
