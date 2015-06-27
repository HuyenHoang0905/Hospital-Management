#if FRAMEWORK20
using System;
using System.Text;
using System.Drawing;

namespace DevComponents.Editors
{
    public class PaintInfo
    {
        private Graphics _Graphics;
        public Graphics Graphics
        {
            get { return _Graphics; }
            set { _Graphics = value; }
        }

        private Point _RenderOffset;
        public Point RenderOffset
        {
            get { return _RenderOffset; }
            set { _RenderOffset = value; }
        }

        private Font _DefaultFont = null;
        public Font DefaultFont
        {
            get { return _DefaultFont; }
            set { _DefaultFont = value; }
        }

        private Color _ForeColor = SystemColors.ControlText;
        public Color ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; }
        }

        private Color _DisabledForeColor = SystemColors.ControlDark;
        public Color DisabledForeColor
        {
            get { return _DisabledForeColor; }
            set { _DisabledForeColor = value; }
        }

        private bool _WatermarkEnabled = false;
        public bool WatermarkEnabled
        {
            get { return _WatermarkEnabled; }
            set { _WatermarkEnabled = value; }
        }

        private Font _WatermarkFont = null;
        public Font WatermarkFont
        {
            get { return _WatermarkFont; }
            set { _WatermarkFont = value; }
        }

        private Color _WatermarkColor = Color.Empty;
        public Color WatermarkColor
        {
            get { return _WatermarkColor; }
            set { _WatermarkColor = value; }
        }

        private Size _AvailableSize;
        /// <summary>
        /// Gets or sets the size available for the item currently being arranged.
        /// </summary>
        public Size AvailableSize
        {
            get { return _AvailableSize; }
            set { _AvailableSize = value; }
        }

        private bool _ParentEnabled = true;
        public bool ParentEnabled
        {
            get { return _ParentEnabled; }
            set
            {
                _ParentEnabled = value;
            }
        }

        private bool _MouseOver = false;
        /// <summary>
        /// Gets or sets whether mouse is over the host control.
        /// </summary>
        public bool MouseOver
        {
            get { return _MouseOver; }
            set { _MouseOver = value; }
        }

        private InputControlColors _Colors;
        /// <summary>
        /// Gets or sets system colors used by the control.
        /// </summary>
        public InputControlColors Colors
        {
            get { return _Colors; }
            set { _Colors = value; }
        }

        private bool _RenderSystemItemsOnly = false;
        public bool RenderSystemItemsOnly
        {
            get { return _RenderSystemItemsOnly; }
            set
            {
                _RenderSystemItemsOnly = value;
            }
        }
        
    }
}
#endif

