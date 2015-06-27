using System;
using System.Text;

namespace DevComponents.DotNetBar
{
    internal class BarBackgroundPainter
    {
        #region Docked Background
        /// <summary>
        /// Paints background of docked bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public virtual void PaintDockedBackground(ToolbarRendererEventArgs e)
        {
        }
        #endregion

        #region Floating Background
        /// <summary>
        /// Paints background of floating bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public virtual void PaintFloatingBackground(ToolbarRendererEventArgs e)
        {
        }
        #endregion

        #region Popup Background
        /// <summary>
        /// Paints background of popup bar.
        /// </summary>
        /// <param name="e">Context information</param>
        public virtual void PaintPopupBackground(ToolbarRendererEventArgs e)
        {
        }
        #endregion
    }
}
