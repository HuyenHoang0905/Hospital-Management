using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Defines the colors for the SuperTabControlBox states
    /// </summary>
    [TypeConverter(typeof(SuperTabControlBoxStateColorTableConvertor))]
    public class SuperTabControlBoxStateColorTable : ICloneable
    {
        #region Events

        /// <summary>
        /// Event raised when the SuperTabControlBoxStateColorTable is changed
        /// </summary>
        [Description("Event raised when the SuperTabControlBoxStateColorTable is changed")]
        public event EventHandler<EventArgs> ColorTableChanged;

        #endregion

        #region Private variables

        private Color _Background;
        private Color _Border = Color.Empty;
        private Color _Image = Color.Empty;

        #endregion

        #region Public properties

        #region Background

        /// <summary>
        /// Gets or sets the colors for the background.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        public Color Background
        {
            get { return (_Background); }

            set
            {
                if (_Background != value)
                {
                    _Background = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackground()
        {
            return (_Background != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBackground()
        {
            _Background = Color.Empty;
        }

        #endregion

        #region Border

        /// <summary>
        /// Gets or sets the colors for the border.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        public Color Border
        {
            get { return (_Border); }

            set
            {
                if (_Border != value)
                {
                    _Border = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorder()
        {
            return (_Border != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBorder()
        {
            _Border = Color.Empty;
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets or sets the colors for the drawn image.
        /// </summary>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        public Color Image
        {
            get { return (_Image); }

            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    OnColorTableChanged();
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImage()
        {
            return (_Image != Color.Empty);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetImage()
        {
            _Image = Color.Empty;
        }

        #endregion

        #region IsEmpty

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                if (_Background.IsEmpty == false)
                    return (false);

                if (_Border != Color.Empty)
                    return (false);

                if (_Image != Color.Empty)
                    return (false);

                return (true);
            }
        }

        #endregion

        #endregion

        #region OnColorTableChanged

        private void OnColorTableChanged()
        {
            if (ColorTableChanged != null)
                ColorTableChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            SuperTabControlBoxStateColorTable sct = new SuperTabControlBoxStateColorTable();

            sct.Background = Background;
            sct.Border = Border;
            sct.Image = Image;

            return (sct);
        }

        #endregion
    }

    #region SuperTabItemStateColorTableConvertor

    public class SuperTabControlBoxStateColorTableConvertor : ExpandableObjectConverter
    {
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                SuperTabControlBoxStateColorTable sct = value as SuperTabControlBoxStateColorTable;

                if (sct != null)
                {
                    ColorConverter cvt = new ColorConverter();

                    if (sct.Background.IsEmpty == false)
                        return (cvt.ConvertToString(sct.Background));

                    if (sct.Border.IsEmpty == false)
                        return (cvt.ConvertToString(sct.Border));

                    if (sct.Image.IsEmpty == false)
                        return (cvt.ConvertToString(sct.Image));

                    return (String.Empty);
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }
    }

    #endregion
}
