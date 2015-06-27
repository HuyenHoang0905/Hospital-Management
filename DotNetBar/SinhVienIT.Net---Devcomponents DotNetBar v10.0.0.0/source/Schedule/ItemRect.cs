#if FRAMEWORK20
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    /// <summary>
    /// DayRect array management class
    /// </summary>
    public class ItemRects
    {
        #region Private variables

        private ItemRect[] _ItemRects;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseItem"></param>
        /// <param name="length">Rectangle array length</param>
        public ItemRects(BaseItem baseItem, int length)
        {
            _ItemRects = new ItemRect[length];

            for (int i = 0; i < length; i++)
                _ItemRects[i] = new ItemRect(baseItem);
        }

        #region Public properties

        /// <summary>
        /// Gets the Rectangle array
        /// </summary>
        public ItemRect[] Rects
        {
            get { return (_ItemRects); }
        }

        /// <summary>
        /// Gets  and sets a specific array Rectangle
        /// </summary>
        /// <param name="index">Rectangle index to get</param>
        /// <returns>Rectangle</returns>
        public ItemRect this[int index]
        {
            get { return (_ItemRects[index]); }
            set { _ItemRects[index] = value; }
        }

        #endregion
    }

    /// <summary>
    /// Simple DayRect class
    /// </summary>
    public class ItemRect
    {
        #region Private variables

        private BaseItem _BaseItem;     // BaseItem
        private Rectangle _Bounds;      // Bounds
        private bool _IsSelected;       // Rect selection

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseItem">BaseItem</param>
        public ItemRect(BaseItem baseItem)
        {
            _BaseItem = baseItem;
        }

        #region Public properties

        /// <summary>
        /// Gets and sets the bounding rect
        /// </summary>
        public Rectangle Bounds
        {
            get { return (_Bounds); }
            set { _Bounds = value; }
        }

        /// <summary>
        /// Gets and sets the rect selection status
        /// </summary>
        public bool IsSelected
        {
            get { return (_IsSelected); }

            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    Invalidate();
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Invalidates the given rectangle
        /// </summary>
        public void Invalidate()
        {
            if (_BaseItem != null)
            {
                Control c = (Control)_BaseItem.GetContainerControl(true);

                if (c != null)
                    c.Invalidate(_Bounds);
            }
        }

        #endregion
    }
}
#endif

