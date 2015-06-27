using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.WinForms.Drawing
{
    [ToolboxItem(false)]
    public abstract class Fill : Component
    {
        #region Internal Implementation
        /// <summary>
        /// Creates the brush for fill.
        /// </summary>
        /// <param name="bounds">Bounds for the brush</param>
        /// <returns>Returns brush or null if brush cannot be created for given bounds or colors are not set. It is responsibility of caller to Dispose the brush.</returns>
        public abstract Brush CreateBrush(Rectangle bounds);

        /// <summary>
        /// Creates a pen based on fill parameters.
        /// </summary>
        /// <param name="width">Width of the pen to create</param>
        /// <returns>new instance of pen or null if pen cannot be created.</returns>
        public abstract Pen CreatePen(int width);
        #endregion
    }
}
