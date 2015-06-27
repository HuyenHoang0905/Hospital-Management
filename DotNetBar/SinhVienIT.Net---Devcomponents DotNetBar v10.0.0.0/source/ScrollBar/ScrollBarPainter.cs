using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.ScrollBar
{
    internal class ScrollBarPainter
    {
        #region Internal Implementation
        public virtual void PaintThumb(Graphics g, System.Drawing.Rectangle bounds, eScrollThumbPosition position, eScrollBarState state)
        {
            
        }

        public virtual void PaintTrackVertical(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state)
        {
        }

        public virtual void PaintTrackHorizontal(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state)
        {
        }

        public virtual void PaintBackground(Graphics g, System.Drawing.Rectangle bounds, eScrollBarState state, bool horizontal, bool sideBorderOnly, bool rtl)
        {
        }
        #endregion
    }
    
    internal enum eScrollThumbPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
    internal enum eScrollBarState
    {
        Normal,
        PartMouseOver,
        ControlMouseOver,
        Pressed,
        Disabled
    }
}
