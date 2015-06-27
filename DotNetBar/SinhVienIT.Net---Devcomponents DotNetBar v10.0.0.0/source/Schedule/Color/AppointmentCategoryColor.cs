#if FRAMEWORK20
using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.Schedule
{
    public class AppointmentCategoryColor
    {
        #region Events

        /// <summary>
        /// Occurs when AppointmentCategoryColorCollection has changed
        /// </summary>
        [Description("Occurs when the AppointmentCategoryColorCollection has changed.")]
        public event EventHandler<EventArgs> AppointmentCategoryColorChanged;

        #endregion

        #region Private variables

        private string _ColorName;
        private Color _TextColor;
        private Color _BorderColor;
        private ColorDef _BackColor;

        #endregion

        /// <summary>
        /// AppointmentCategoryColor
        /// </summary>
        /// <param name="colorName">Color name</param>
        /// <param name="textColor">Text Color</param>
        /// <param name="borderColor">Border Color</param>
        /// <param name="backColor">Background Color</param>
        public AppointmentCategoryColor(string colorName,
            Color textColor, Color borderColor, ColorDef backColor)
        {
            _ColorName = colorName;

            _TextColor = textColor;
            _BorderColor = borderColor;
            _BackColor = backColor;
        }

        /// <summary>
        /// AppointmentCategoryColor
        /// </summary>
        /// <param name="colorName">Color name</param>
        public AppointmentCategoryColor(string colorName)
            : this(colorName, Color.Black, Color.Black, new ColorDef(Color.White))
        {
        }

        #region Public properties

        #region ColorName

        /// <summary>
        /// Color name
        /// </summary>
        public string ColorName
        {
            get { return (_ColorName); }
            internal set { _ColorName = value; }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Text Color
        /// </summary>
        public Color TextColor
        {
            get { return (_TextColor); }

            set
            {
                if (_TextColor != value)
                {
                    _TextColor = value;

                    OnAppointmentCategoryColorChanged();
                }
            }
        }

        #endregion

        #region BorderColor

        /// <summary>
        /// Border Color
        /// </summary>
        public Color BorderColor
        {
            get { return (_BorderColor); }

            set
            {
                if (_BorderColor != value)
                {
                    _BorderColor = value;

                    OnAppointmentCategoryColorChanged();
                }
            }
        }

        #endregion

        #region BackColor

        /// <summary>
        /// Background Color
        /// </summary>
        public ColorDef BackColor
        {
            get { return (_BackColor); }

            set
            {
                if (_BackColor != value)
                {
                    _BackColor = value;

                    OnAppointmentCategoryColorChanged();
                }
            }
        }

        #endregion

        #endregion

        #region OnAppointmentCategoryColorChanged

        private void OnAppointmentCategoryColorChanged()
        {
            if (AppointmentCategoryColorChanged != null)
                AppointmentCategoryColorChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
#endif