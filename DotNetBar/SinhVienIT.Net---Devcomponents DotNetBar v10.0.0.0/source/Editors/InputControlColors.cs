#if FRAMEWORK20
using System;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.Editors
{
    /// <summary>
    /// Defines the system colors used by the input controls.
    /// </summary>
   [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class InputControlColors
    {
        /// <summary>
        /// Occurs when color has changed.
        /// </summary>
        public event EventHandler ColorChanged;

        private Color _Highlight = Color.Empty;
        /// <summary>
        /// Gets or sets the background color of input item part when part has input focus. Default value is Color.Empty which indicates that system Highlight color is used.
        /// </summary>
        public Color Highlight
        {
            get { return _Highlight; }
            set
            {
                if (_Highlight != value)
                {
                    _Highlight = value;
                    OnColorChanged();
                }
            }
        }
        /// <summary>
        /// Resets property to its default value. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHighlight()
        {
            Highlight = Color.Empty;
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHighlight()
        {
            return !Highlight.IsEmpty;
        }



        private Color _HighlightText = Color.Empty;
        /// <summary>
        /// Gets or sets the text color of input item part when part has input focus. Default value is Color.Empty which indicates that system HighlightText color is used.
        /// </summary>
        public Color HighlightText
        {
            get { return _HighlightText; }
            set
            {
                if (_HighlightText != value)
                {
                    _HighlightText = value;
                    OnColorChanged();
                }
            }
        }
        /// <summary>
        /// Resets property to its default value. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHighlightText()
        {
            HighlightText = Color.Empty;
        }
        /// <summary>
        /// Returns whether property should be serialized. Provided for design-time support.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHighlightText()
        {
            return !HighlightText.IsEmpty;
        }

        private void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new EventArgs());
        }
    }
}
#endif