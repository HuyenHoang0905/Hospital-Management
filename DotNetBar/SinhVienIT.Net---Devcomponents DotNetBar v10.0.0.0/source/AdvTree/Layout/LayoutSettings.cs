using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevComponents.AdvTree.Layout
{
    internal class LayoutSettings
    {
        #region Internal Implementation
        private int _NodeVerticalSpacing = 3;
        /// <summary>
        /// Gets or sets the vertical spacing between nodes in pixels.
        /// </summary>
        public virtual int NodeVerticalSpacing
        {
            get { return _NodeVerticalSpacing; }
            set { _NodeVerticalSpacing = value; }
        }

        private int _NodeHorizontalSpacing = 4;
        /// <summary>
        /// Gets or sets the horizontal spacing between nodes in pixels.
        /// </summary>
        public virtual int NodeHorizontalSpacing
        {
            get { return _NodeHorizontalSpacing; }
            set { _NodeHorizontalSpacing = value; }
        }

        private int _CellHorizontalSpacing = 5;
        /// <summary>
        /// Returns horizontal spacing between cells in a node
        /// </summary>
        public int CellHorizontalSpacing
        {
            get { return _CellHorizontalSpacing; }
            set
            {
                _CellHorizontalSpacing = value;
            }
        }

        private int _ExpandAreaWidth = 24;
        /// <summary>
        /// Returns width of the expand button area. Default is 24 pixels.
        /// </summary>
        public virtual int ExpandAreaWidth
        {
            get { return _ExpandAreaWidth; }
            set
            {
                _ExpandAreaWidth = value;
            }
        }

        protected Size _ExpandPartSize = new Size(8, 8);
        /// <summary>
        /// Gets or sets the size of the expand part that is expanding/collapsing the node. Default value is 8,8.
        /// </summary>
        public System.Drawing.Size ExpandPartSize
        {
            get { return _ExpandPartSize; }
            set { _ExpandPartSize = value; }
        }

        private int _CommandAreaWidth = 10;
        /// <summary>
        /// Gets or sets width of command button area. Default is 8 pixels.
        /// </summary>
        public virtual int CommandAreaWidth
        {
            get { return _CommandAreaWidth; }
            set { _CommandAreaWidth = value; }
        }
        #endregion
    }
}
