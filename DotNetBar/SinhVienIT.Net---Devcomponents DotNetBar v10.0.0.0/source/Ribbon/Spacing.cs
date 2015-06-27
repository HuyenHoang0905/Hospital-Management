using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Indicates spacing for an user interface element either padding or margins.
    /// </summary>
    [ToolboxItem(false),TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Spacing
    {
        #region Events
        public event EventHandler SpacingChanged;
        #endregion

        #region Private Variables
        private int m_Left = 0;
        private int m_Right = 0;
        private int m_Top = 0;
        private int m_Bottom = 0;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets or sets the amount of the space on the left side.
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates the amount of the space on the left side."), DevCoSerialize()]
        public int Left
        {
            get { return m_Left; }
            set
            {
                m_Left = value;
                OnSpacingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of the space on the right side.
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates the amount of the space on the right side."), DevCoSerialize()]
        public int Right
        {
            get { return m_Right; }
            set
            {
                m_Right = value;
                OnSpacingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of the space on the top.
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates the amount of the space on the top."), DevCoSerialize()]
        public int Top
        {
            get { return m_Top; }
            set
            {
                m_Top = value;
                OnSpacingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of the space on the bottom.
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates the amount of the space on the bottom."), DevCoSerialize()]
        public int Bottom
        {
            get { return m_Bottom; }
            set
            {
                m_Bottom = value;
                OnSpacingChanged();
            }
        }

        private void OnSpacingChanged()
        {
            if (SpacingChanged != null)
                SpacingChanged(this, new EventArgs());
        }

        /// <summary>
        /// Gets total horizontal spacing.
        /// </summary>
        [Browsable(false)]
        public int Horizontal
        {
            get { return Left + Right; }
        }

        /// <summary>
        /// Gets total vertical spacing.
        /// </summary>
        [Browsable(false)]
        public int Vertical
        {
            get { return Top + Bottom; }
        }

        /// <summary>
        /// Gets whether all memebers of class are set to 0.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get { return (m_Left==0 && m_Right==0 && m_Top==0 && m_Bottom==0); }
        }
        #endregion
    }
}
