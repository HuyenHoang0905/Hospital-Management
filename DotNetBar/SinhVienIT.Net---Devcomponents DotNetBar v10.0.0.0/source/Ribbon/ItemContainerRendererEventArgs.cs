using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides data for rendering ItemContainer.
    /// </summary>
    public class ItemContainerRendererEventArgs:EventArgs
    {
        /// <summary>
        /// Gets or sets Graphics object group is rendered on.
        /// </summary>
        public Graphics Graphics = null;
        /// <summary>
        /// Gets the reference to ItemContainer instance being rendered.
        /// </summary>
        public ItemContainer ItemContainer = null;

        internal ItemPaintArgs ItemPaintArgs = null;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="g">Reference to graphics object.</param>
        /// <param name="container">Reference to ItemContainer object.</param>
        public ItemContainerRendererEventArgs(Graphics g, ItemContainer container)
        {
            this.Graphics = g;
            this.ItemContainer = container;
        }
    }

    /// <summary>
    /// Provides data for the item separator rendering inside of the ItemContainer.
    /// </summary>
    public class ItemContainerSeparatorRendererEventArgs : ItemContainerRendererEventArgs
    {
        /// <summary>
        /// Gets or sets the reference to the item separator is being rendered for.
        /// </summary>
        public BaseItem Item = null;

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public ItemContainerSeparatorRendererEventArgs(Graphics g, ItemContainer container, BaseItem item): base(g, container)
        {
            this.Item = item;
        }
    }
}
