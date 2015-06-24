#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AdvPropertyGridAppearance : Component, INotifyPropertyChanged
    {
        #region Internal Implementation
        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _PropertyValueErrorImage);
            }
            base.Dispose(disposing);
        }
        private Color _ErrorHighlightColor = ColorScheme.GetColor(0xD99694);
        /// <summary>
        /// Gets or sets the color of the node highlight when error has occurred while setting property value. 
        /// </summary>
        [Category("Colors"), Description("Indicates color of the node highlight when error has occurred while setting property value.")]
        public Color ErrorHighlightColor
        {
            get { return _ErrorHighlightColor; }
            set
            {
                _ErrorHighlightColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorHighlightColor"));
            }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeErrorHighlightColor()
        {
            return !_ErrorHighlightColor.Equals(ColorScheme.GetColor(0xD99694));
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetErrorHighlightColor()
        {
            this.ErrorHighlightColor = ColorScheme.GetColor(0xD99694);
        }

        private Color _SuccessHighlightColor = ColorScheme.GetColor(0x9BBB59);
        /// <summary>
        /// Gets or sets the color of the node highlight when property update was successful.
        /// </summary>
        [Category("Colors"), Description("Indicates color of node highlight when property update was successful.")]
        public Color SuccessHighlightColor
        {
            get { return _SuccessHighlightColor; }
            set
            {
                _SuccessHighlightColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessHighlightColor"));
            }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSuccessHighlightColor()
        {
            return !_SuccessHighlightColor.Equals(ColorScheme.GetColor(0x9BBB59));
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSuccessHighlightColor()
        {
            this.SuccessHighlightColor = ColorScheme.GetColor(0x9BBB59);
        }

        private ElementStyle _DefaultPropertyStyle = null;
        /// <summary>
        /// Gets or sets default style for property node.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Gets or sets default style for property node.")]
        public ElementStyle DefaultPropertyStyle
        {
            get { return _DefaultPropertyStyle; }
            set
            {
                if (_DefaultPropertyStyle != value)
                {
                    if (_DefaultPropertyStyle != null)
                        _DefaultPropertyStyle.StyleChanged -= new EventHandler(this.DefaultPropertyStyleChanged);
                    if (value != null)
                        value.StyleChanged += new EventHandler(DefaultPropertyStyleChanged);
                    _DefaultPropertyStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DefaultPropertyStyle"));
                }
            }
        }
        private void DefaultPropertyStyleChanged(object sender, EventArgs e)
        {
            
        }

        private ElementStyle _ReadOnlyPropertyStyle = null;
        /// <summary>
        /// Gets or sets style for property node when in read-only state.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Gets or sets style for property node when in read-only state.")]
        public ElementStyle ReadOnlyPropertyStyle
        {
            get { return _ReadOnlyPropertyStyle; }
            set
            {
                if (_ReadOnlyPropertyStyle != value)
                {
                    if (_ReadOnlyPropertyStyle != null)
                        _ReadOnlyPropertyStyle.StyleChanged -= this.ReadOnlyPropertyStyleChanged;
                    if (value != null)
                        value.StyleChanged += ReadOnlyPropertyStyleChanged;
                    _ReadOnlyPropertyStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyPropertyStyle"));
                }
            }
        }
        private void ReadOnlyPropertyStyleChanged(object sender, EventArgs e)
        {

        }

        private ElementStyle _ValueChangedPropertyStyle = null;
        /// <summary>
        /// Gets or sets style for property node when in read-only state.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Gets or sets style for property node when in read-only state.")]
        public ElementStyle ValueChangedPropertyStyle
        {
            get { return _ValueChangedPropertyStyle; }
            set
            {
                if (_ValueChangedPropertyStyle != value)
                {
                    if (_ValueChangedPropertyStyle != null)
                        _ValueChangedPropertyStyle.StyleChanged -= this.ValueChangedPropertyStyleChanged;
                    if (value != null)
                        value.StyleChanged += ValueChangedPropertyStyleChanged;
                    _ValueChangedPropertyStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ValueChangedPropertyStyle"));
                }
            }
        }
        private void ValueChangedPropertyStyleChanged(object sender, EventArgs e)
        {

        }

        private ElementStyle _CategoryStyle = null;
        /// <summary>
        /// Gets or sets style for property node when in read-only state.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Gets or sets style for property node when in read-only state.")]
        public ElementStyle CategoryStyle
        {
            get { return _CategoryStyle; }
            set
            {
                if (_CategoryStyle != value)
                {
                    if (_CategoryStyle != null)
                        _CategoryStyle.StyleChanged -= this.CategoryStyleChanged;
                    if (value != null)
                        value.StyleChanged += CategoryStyleChanged;
                    _CategoryStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CategoryStyle"));
                }
            }
        }
        private void CategoryStyleChanged(object sender, EventArgs e)
        {

        }

        private Image _PropertyValueErrorImage = null;
        /// <summary>
        /// Gets or sets the image that is displayed in property name cell when property value has failed the validation.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indicates image that is displayed in property name cell when property value has failed the validation.")]
        public Image PropertyValueErrorImage
        {
            get { return _PropertyValueErrorImage; }
            set
            {
                if (_PropertyValueErrorImage == value) return;
                _PropertyValueErrorImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PropertyValueErrorImage"));
            }
        }
        #endregion

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
#endif