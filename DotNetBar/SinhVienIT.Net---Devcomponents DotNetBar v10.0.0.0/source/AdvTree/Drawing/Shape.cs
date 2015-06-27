using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.WinForms.Drawing
{
    /// <summary>
    /// Defines a visual shape.
    /// </summary>
    internal abstract class Shape
    {
        #region Internal Implementation
        /// <summary>
        /// Renders shape on canvas.
        /// </summary>
        /// <param name="g">Target graphics to render shape on.</param>
        /// <param name="bounds">Shape bounds.</param>
        public abstract void Paint(Graphics g, Rectangle bounds);

        private Shape _Content = null;
        /// <summary>
        /// Gets or sets the single piece of content inside of the shape.
        /// </summary>
        [DefaultValue(null)]
        public Shape Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        private bool _ClipToBounds = false;
        /// <summary>
        /// Gets or sets whether to clip the Content of this shape. Default value is false.
        /// </summary>
        [DefaultValue(false)]
        public bool ClipToBounds
        {
            get { return _ClipToBounds; }
            set
            {
                _ClipToBounds = value;
            }
        }
        
        #endregion
    }
}
