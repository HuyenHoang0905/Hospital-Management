#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors
{
    /// <summary>
    /// Describes input button settings.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class InputButtonSettings : IComparable
    {
        #region Private Variables
        private IInputButtonControl _Parent = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the InputButtonSettings class.
        /// </summary>
        public InputButtonSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the InputButtonSettings class.
        /// </summary>
        /// <param name="parent"></param>
        public InputButtonSettings(IInputButtonControl parent)
        {
            _Parent = parent;
        }
        #endregion

        #region Internal Implementation
        private bool _Visible = false;
        /// <summary>
        /// Gets or sets whether button is visible.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether button is visible.")]
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    OnVisibleChanged();
                }
            }
        }

        private void OnVisibleChanged()
        {
            NotifyParent();
        }

        private bool _Enabled = true;
        /// <summary>
        /// Gets or sets whether button is enabled.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether button is enabled.")]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                    NotifyParent();
                }
            }
        }

        private int _DisplayPosition = 0;
        /// <summary>
        /// Gets or sets the display position index of the button. Buttons are ordered from left to right with button with lowest index appearing as left-most button.
        /// </summary>
        [DefaultValue(0), Description("Indicates display position index of the button."), Localizable(true)]
        public int DisplayPosition
        {
            get { return _DisplayPosition; }
            set
            {
                if (_DisplayPosition != value)
                {
                    _DisplayPosition = value;
                    OnDisplayPositionChanged();
                }
            }
        }

        private void OnDisplayPositionChanged()
        {
            NotifyParent();
        }

        private Image _Image = null;
        /// <summary>
        /// Gets or sets the image displayed on the face of the button.
        /// </summary>
        [DefaultValue(null), Description("Indicates image displayed on the face of the button."), Localizable(true)]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    OnImageChanged();
                }
            }
        }

        private void OnImageChanged()
        {
            NotifyParent();
        }

        private string _Text = "";
        /// <summary>
        /// Gets or sets the text displayed on the input button face.
        /// </summary>
        [DefaultValue(""), Description("Input text displayed on the input button face."), Localizable(true)]
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value == null) value = "";
                if (_Text != value)
                {
                    _Text = value;
                    OnTextChanged();
                }
            }
        }

        private void OnTextChanged()
        {
            NotifyParent();
        }

        private bool _Checked = false;
        [DefaultValue(false), Description("Gets or sets whether button is checked.")]
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked!=value)
                {
                    _Checked = value;
                    OnCheckedChanged();
                }
                
            }
        }

        private void OnCheckedChanged()
        {
            NotifyParent();
        }
        

        private void NotifyParent()
        {
            if (_Parent != null)
                _Parent.InputButtonSettingsChanged(this);
        }

        private VisualItem _ItemReference = null;
        /// <summary>
        /// Gets or sets the visual item button references for its action.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VisualItem ItemReference
        {
            get { return _ItemReference; }
            set { _ItemReference = value; }
        }

        private eShortcut _Shortcut = eShortcut.None;
        /// <summary>
        /// Gets or sets the shortcut key which when pressed triggers button click event or its default function.
        /// </summary>
        [DefaultValue(eShortcut.None), Description("Indicates shortcut key which when pressed triggers button click event or its default function.")]
        public eShortcut Shortcut
        {
            get { return _Shortcut; }
            set 
            {
                if (_Shortcut != value)
                {
                    _Shortcut = value;
                    NotifyParent();
                }
            }
        }
        #endregion

        /// <summary>
        /// Copies properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New InputButtonSettings instance</param>
        internal void CopyToItem(InputButtonSettings copy)
        {
            copy.Visible = Visible;
            copy.Enabled = Enabled;
            copy.DisplayPosition = DisplayPosition;
            copy.Image = Image;
            copy.Text = Text;
            copy.Checked = Checked;
            copy.Shortcut = Shortcut;
        }

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (obj is InputButtonSettings)
            {
                int pos = ((InputButtonSettings)obj).DisplayPosition - this.DisplayPosition;
                if (pos == 0)
                {
                    if (obj != this) pos = -1;
                }
                return pos;
            }
            return 0;
        }
        #endregion
    }

    public interface IInputButtonControl
    {
        void InputButtonSettingsChanged(InputButtonSettings button);
    }
}
#endif

