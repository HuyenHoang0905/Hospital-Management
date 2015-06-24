#if FRAMEWORK20
using System;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Provides data for DayLabel painting events.
    /// </summary>
    public class DayPaintEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the graphics canvas for rendering.
        /// </summary>
        public readonly Graphics Graphics;

        /// <summary>
        /// Gets or sets which parts of the item will be drawn by the system. You can set this to None to completely disable system rendering.
        /// </summary>
        public eDayPaintParts RenderParts = eDayPaintParts.All;

        internal DayLabel _Item = null;

        internal ItemPaintArgs _ItemPaintArgs = null;
        /// <summary>
        /// Initializes a new instance of the DayPaintEventArgs class.
        /// </summary>
        /// <param name="graphics">Reference to Graphics canvas.</param>
        /// <param name="item">Reference to item being rendered.</param>
        public DayPaintEventArgs(ItemPaintArgs p, DayLabel item)
        {
            Graphics = p.Graphics;
            _ItemPaintArgs = p;
            _Item = item;
        }

        /// <summary>
        /// Renders the background of the item.
        /// </summary>
        public void PaintBackground()
        {
            _Item.PaintBackground(_ItemPaintArgs);
        }

        /// <summary>
        /// Renders the item text.
        /// </summary>
        public void PaintText()
        {
            _Item.PaintText(_ItemPaintArgs, null, Color.Empty, _Item.TextAlign);
        }

        /// <summary>
        /// Renders the item text.
        /// </summary>
        public void PaintText(Color textColor)
        {
            _Item.PaintText(_ItemPaintArgs, null, textColor, _Item.TextAlign);
        }

        /// <summary>
        /// Renders the item text.
        /// </summary>
        public void PaintText(Color textColor, eLabelPartAlignment textAlign)
        {
            _Item.PaintText(_ItemPaintArgs, null, textColor, textAlign);
        }

        /// <summary>
        /// Renders the item text.
        /// </summary>
        public void PaintText(Color textColor, Font textFont)
        {
            _Item.PaintText(_ItemPaintArgs, textFont, textColor, _Item.TextAlign);
        }

        /// <summary>
        /// Renders the item text.
        /// </summary>
        public void PaintText(Color textColor, Font textFont, eLabelPartAlignment textAlign)
        {
            _Item.PaintText(_ItemPaintArgs, textFont, textColor, textAlign);
        }

        /// <summary>
        /// Renders items image.
        /// </summary>
        public void PaintImage()
        {
            _Item.PaintImage(_ItemPaintArgs, _Item.Image, _Item.ImageAlign);
        }

        /// <summary>
        /// Renders items image.
        /// </summary>
        public void PaintImage(eLabelPartAlignment imageAlign)
        {
            _Item.PaintImage(_ItemPaintArgs, _Item.Image, imageAlign);
        }
    }

    /// <summary>
    /// Defines delegate for DayLabel painting events.
    /// </summary>
    /// <param name="sender">Source of the event.</param>
    /// <param name="e">Provides event data.</param>
    public delegate void DayPaintEventHandler(object sender, DayPaintEventArgs e);

    /// <summary>
    /// Specifies the parts of DayLabel control. Members of this enum are intended to be used as flags (combined).
    /// </summary>
    [Flags()]
    public enum eDayPaintParts
    {
        /// <summary>
        /// Specifies no part.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the label background.
        /// </summary>
        Background = 1,
        /// <summary>
        /// Specifies the label text.
        /// </summary>
        Text = 2,
        /// <summary>
        /// Specifies the label image.
        /// </summary>
        Image = 4,
        /// <summary>
        /// Specifies all parts.
        /// </summary>
        All = Background | Text | Image
    }
}
#endif

