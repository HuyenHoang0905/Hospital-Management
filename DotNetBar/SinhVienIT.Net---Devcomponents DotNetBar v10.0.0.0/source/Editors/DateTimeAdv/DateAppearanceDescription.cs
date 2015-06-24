#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Defines the visual marking applied to dates through month calendar control.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class DateAppearanceDescription
    {
        #region Private Variables
        private BaseItem _Parent = null;
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the DateAppearanceDescription class.
        /// </summary>
        public DateAppearanceDescription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DateAppearanceDescription class.
        /// </summary>
        /// <param name="parent"></param>
        public DateAppearanceDescription(BaseItem parent)
        {
            _Parent = parent;
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Applies all settings from this object to specified object.
        /// </summary>
        /// <param name="desc">Reference to object.</param>
        public void ApplyTo(DateAppearanceDescription desc)
        {
            desc.BackColor = this.BackColor;
            desc.BackColor2 = this.BackColor2;
            desc.BackColorGradientAngle = this.BackColorGradientAngle;
            desc.BorderColor = this.BorderColor;
            desc.IsBold = this.IsBold;
            desc.Selectable = this.Selectable;
            desc.TextColor = this.TextColor;
        }

        private void Refresh()
        {
            if (_Parent != null)
                _Parent.Refresh();
        }

        private bool _IsBold = false;
        /// <summary>
        /// Gets or sets whether text is drawn using bold font.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether text is drawn using bold font.")]
        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                if (_IsBold != value)
                {
                    _IsBold = value;
                    this.Refresh();
                }
            }
        }

        private Color _BackColor = Color.Empty;
        /// <summary>
        /// Gets or sets the background color for the marked day.
        /// </summary>
        [Category("Colors"), Description("Indicates background color for the marked day.")]
        public Color BackColor
        {
            get { return _BackColor; }
            set
            {
                _BackColor = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return !BackColor.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackColor()
        {
            BackColor = Color.Empty;
        }

        private Color _BackColor2 = Color.Empty;
        /// <summary>
        /// Gets or sets the background target gradient color for the marked date.
        /// </summary>
        [Category("Colors"), Description("Indicates background target gradient color for the marked date.")]
        public Color BackColor2
        {
            get { return _BackColor2; }
            set
            {
                _BackColor2 = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor2()
        {
            return !BackColor2.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackColor2()
        {
            BackColor2 = Color.Empty;
        }

        private int _BackColorGradientAngle = 90;
        /// <summary>
        /// Gets or sets the background gradient fill angle. Default value is 90.
        /// </summary>
        [DefaultValue(90), Description("Indicates background gradient fill angle.")]
        public int BackColorGradientAngle
        {
            get { return _BackColorGradientAngle; }
            set
            {
                if (_BackColorGradientAngle != value)
                {
                    _BackColorGradientAngle = value;
                    this.Refresh();
                }
            }
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color for the marked date.
        /// </summary>
        [Category("Colors"), Description("Indicates text color for the marked date.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !TextColor.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            TextColor = Color.Empty;
        }

        /// <summary>
        /// Gets whether any of the appearance values have been changed.
        /// </summary>
        [Browsable(false)]
        public bool IsCustomized
        {
            get
            {
                return !_BackColor.IsEmpty || !_BackColor2.IsEmpty || !_TextColor.IsEmpty || _IsBold || !_BorderColor.IsEmpty;
            }
        }

        internal BaseItem Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != value)
                {
                    _Parent = value;
                }
            }
        }

        private Color _BorderColor = Color.Empty;
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        [Category("Colors"), Description("Indicates borderColor color.")]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set
            {
                _BorderColor = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColor()
        {
            return !BorderColor.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBorderColor()
        {
            BorderColor = Color.Empty;
        }

        private bool _Selectable = true;
        /// <summary>
        /// Gets or sets whether day marked is selectable by end user. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether day marked is selectable by end user.")]
        public bool Selectable
        {
            get { return _Selectable; }
            set
            {
                if (_Selectable != value)
                {
                    _Selectable = value;
                }
            }
        }
        #endregion

    }
}
#endif

