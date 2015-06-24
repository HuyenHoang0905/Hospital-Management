using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.Tree
{
    #region Padding Class

    /// <summary>
    /// Represents class that holds padding information for user interface elements.
    /// </summary>
    [TypeConverter(typeof(PaddingConvertor))]
    public class Padding : INotifyPropertyChanged
    {
        #region Private variables

        /// <summary>
        /// Gets or sets padding on left side. Default value is 0
        /// </summary>
        private int _Left;

        /// <summary>
        /// Gets or sets padding on right side. Default value is 0
        /// </summary>
        private int _Right;

        /// <summary>
        /// Gets or sets padding on top side. Default value is 0
        /// </summary>
        private int _Top;

        /// <summary>
        /// Gets or sets padding on bottom side. Default value is 0
        /// </summary>
        private int _Bottom;

        #endregion

        /// <summary>
        /// Creates new instance of the class and initializes it.
        /// </summary>
        /// <param name="left">Left padding</param>
        /// <param name="right">Right padding</param>
        /// <param name="top">Top padding</param>
        /// <param name="bottom">Bottom padding</param>
        public Padding(int left, int right, int top, int bottom)
        {
            _Left = left;
            _Right = right;
            _Top = top;
            _Bottom = bottom;
        }

        #region Public properties

        /// <summary>
        /// Gets amount of Top padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Top padding.")]
        [NotifyParentProperty(true)]
        public int Top
        {
            get { return (_Top); }
            set { _Top = value; OnPropertyChanged(new PropertyChangedEventArgs("Top")); }
        }

        /// <summary>
        /// Gets amount of Left padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Left padding.")]
        [NotifyParentProperty(true)]
        public int Left
        {
            get { return (_Left); }
            set { _Left = value; OnPropertyChanged(new PropertyChangedEventArgs("Left")); }
        }

        /// <summary>
        /// Gets amount of Bottom padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Bottom padding.")]
        [NotifyParentProperty(true)]
        public int Bottom
        {
            get { return (_Bottom); }
            set { _Bottom = value; OnPropertyChanged(new PropertyChangedEventArgs("Bottom")); }
        }

        /// <summary>
        /// Gets amount of Right padding
        /// </summary>
        [Browsable(true), DefaultValue(0)]
        [Description("Indicates the amount of Right padding.")]
        [NotifyParentProperty(true)]
        public int Right
        {
            get { return (_Right); }
            set { _Right = value; OnPropertyChanged(new PropertyChangedEventArgs("Right")); }
        }

        /// <summary>
        /// Gets amount of horizontal padding (Left+Right)
        /// </summary>
        [Browsable(false)]
        public int Horizontal
        {
            get { return (_Left + _Right); }
        }

        /// <summary>
        /// Gets amount of vertical padding (Top+Bottom)
        /// </summary>
        [Browsable(false)]
        public int Vertical
        {
            get { return (_Top + _Bottom); }
        }

        /// <summary>
        /// Gets whether Padding is empty.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get { return (_Left == 0 && _Right == 0 && _Top == 0 && _Bottom == 0); }
        }

        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eh = PropertyChanged;
            if (eh != null) eh(this, e);
        }
        #endregion
    }

    #endregion

    #region PaddingConvertor

    public class PaddingConvertor : ExpandableObjectConverter
    {
        public override bool CanConvertTo(
            ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (true);

            return (base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Padding pad = value as Padding;

                if (pad != null)
                {
                    return (String.Format("{0:D}, {1:D}, {2:D}, {3:D}",
                        pad.Bottom, pad.Left, pad.Right, pad.Top));
                }
            }

            return (base.ConvertTo(context, culture, value, destinationType));
        }

        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return (true);

            return (base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] values = ((string)value).Split(',');

                if (values.Length != 4)
                    throw new ArgumentException("Invalid value to convert.");

                try
                {
                    int bottom = int.Parse(values[0]);
                    int left = int.Parse(values[1]);
                    int right = int.Parse(values[2]);
                    int top = int.Parse(values[3]);

                    Padding pad = new Padding(left, right, top, bottom);

                    return (pad);
                }
                catch (Exception exp)
                {
                    throw new ArgumentException("Invalid value to convert.");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    #endregion
}
