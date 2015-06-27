using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Defines the Tree color table.
    /// </summary>
    [ToolboxItem(false)]
    public class TreeColorTable : Component
    {
        #region Internal Implementation
        /// <summary>
        /// Gets or sets the color table used for the node selection display.
        /// </summary>
        public TreeSelectionColors Selection = new TreeSelectionColors();
        /// <summary>
        /// Gets or sets the color for node drag &amp; drop marker.
        /// </summary>
        public Color DragDropMarker = Color.Black;
        /// <summary>
        /// Gets or sets the color of tree expand button type of rectangle.
        /// </summary>
        public TreeExpandColorTable ExpandRectangle = new TreeExpandColorTable();
        /// <summary>
        /// Gets or sets the color of tree expand button type of Ellipse.
        /// </summary>
        public TreeExpandColorTable ExpandEllipse = new TreeExpandColorTable();
        /// <summary>
        /// Gets or sets the color of tree expand button type of Triangle.
        /// </summary>
        public TreeExpandColorTable ExpandTriangle = new TreeExpandColorTable();
        /// <summary>
        /// Gets or sets the color for tree grid lines.
        /// </summary>
        public Color GridLines = Color.Empty;
        /// <summary>
        /// Gets or sets the color of the column sort indicator which is rendered on columns when sorted.
        /// </summary>
        public Color ColumnSortIndicatorColor = Color.Gray;
        #endregion
    }
}
