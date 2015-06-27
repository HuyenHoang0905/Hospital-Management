using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

#if AdvTree
namespace DevComponents.Tree
#elif DOTNETBAR
namespace DevComponents.DotNetBar
#endif
{
    /// <summary>
    /// Defines single color blend point for the multicolor gradient fills.
    /// </summary>
    [ToolboxItem(false),DesignTimeVisible(false),TypeConverter(typeof(BackgroundColorBlendConverter))]
    public class BackgroundColorBlend
    {
        #region Private Variables
        private Color m_Color = Color.Empty;
        private float m_Position = 0;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Creates new instance of the class. When defining multicolor gradinet blends and using the percentage positions the positions created
        /// must start with 0f and end with 1f.
        /// </summary>
        public BackgroundColorBlend() {}

        /// <summary>
        /// Creates new instance of the class and initialize it with default values.
        /// </summary>
        public BackgroundColorBlend(Color color, float position)
        {
            m_Color=color;
            m_Position=position;
        }

        /// <summary>
        /// Creates new instance of the class and initialize it with default values.
        /// </summary>
        public BackgroundColorBlend(int color, float position)
        {
            m_Color = ColorScheme.GetColor(color);
            m_Position = position;
        }

        /// <summary>
        /// Gets or sets Color to use in multicolor gradient blend at specified position.
        /// </summary>
        [Browsable(true), Description("Indicates the Color to use in multicolor gradient blend at specified position.")]
        public Color Color
        {
            get { return m_Color; }
            set
            {
                m_Color = value;
                OnColorBlendChanged();
            }
        }
        private bool ShouldSerializeColor()
        {
            return !m_Color.IsEmpty;
        }

        /// <summary>
        /// Gets or sets the color position in multicolor gradient blend. Values less or equal to 1 are used as percentage specifing percentages of distance along the gradient line.
        /// Values greater than 1 are used as absolute pixel values of distance along the gradient line.
        /// </summary>
        [Browsable(true), DefaultValue(0f), Description("")]
        public float Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                OnColorBlendChanged();
            }
        }

        private void OnColorBlendChanged()
        {
        }
        #endregion

    }
}
