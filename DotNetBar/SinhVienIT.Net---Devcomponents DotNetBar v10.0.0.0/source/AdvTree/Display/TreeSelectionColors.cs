using System;
using System.Text;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Defines the color table for tree selection.
    /// </summary>
    public class TreeSelectionColors
    {
        /// <summary>
        /// Gets or sets the color table for FullRowSelect selection type.
        /// </summary>
        public SelectionColorTable FullRowSelect = null;
        /// <summary>
        /// Gets or sets the color table for FullRowSelect selection type when tree control is inactive.
        /// </summary>
        public SelectionColorTable FullRowSelectInactive = null;
        /// <summary>
        /// Gets or sets the color table for HighlightCells selection type.
        /// </summary>
        public SelectionColorTable HighlightCells = null;
        /// <summary>
        /// Gets or sets the color table for HighlightCells selection type when tree control is inactive.
        /// </summary>
        public SelectionColorTable HighlightCellsInactive = null;
        /// <summary>
        /// Gets or sets the color table for NodeMarker selection type.
        /// </summary>
        public SelectionColorTable NodeMarker = null;
        /// <summary>
        /// Gets or sets the color table for NodeMarker selection type when tree control is inactive.
        /// </summary>
        public SelectionColorTable NodeMarkerInactive = null;
        /// <summary>
        /// Gets or sets the color table used for node hot-tracking.
        /// </summary>
        public SelectionColorTable NodeHotTracking = null;
    }
}
