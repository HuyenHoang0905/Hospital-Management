using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.WinForms.Drawing
{
    /// <summary>
    /// Defines single color blend point for the multicolor gradient fills.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), TypeConverter(typeof(ColorStopConverter))]
    public class ColorStop
    {
        #region Private Variables
        private Color _Color = Color.Empty;
        private float _Position = 0;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Creates new instance of the class. When defining multicolor gradient blends and using the percentage positions the positions created
        /// must start with 0f and end with 1f.
        /// </summary>
        public ColorStop() { }

        /// <summary>
        /// Creates new instance of the class and initialize it with default values.
        /// </summary>
        public ColorStop(Color color, float position)
        {
            _Color = color;
            _Position = position;
        }

        ///// <summary>
        ///// Creates new instance of the class and initialize it with default values.
        ///// </summary>
        //public ColorStop(int color, float position)
        //{
        //    _Color = ColorScheme.GetColor(color);
        //    _Position = position;
        //}

        /// <summary>
        /// Gets or sets Color to use in multicolor gradient blend at specified position.
        /// </summary>
        [Browsable(true), Description("Indicates the Color to use in multicolor gradient blend at specified position.")]
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                OnColorBlendChanged();
            }
        }
        private bool ShouldSerializeColor()
        {
            return !_Color.IsEmpty;
        }

        /// <summary>
        /// Gets or sets the color position in multicolor gradient blend. Values less or equal to 1 are used as percentage specifing percentages of distance along the gradient line.
        /// Values greater than 1 are used as absolute pixel values of distance along the gradient line.
        /// </summary>
        [Browsable(true), DefaultValue(0f), Description("")]
        public float Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                OnColorBlendChanged();
            }
        }

        private void OnColorBlendChanged()
        {
        }
        #endregion

    }
}
