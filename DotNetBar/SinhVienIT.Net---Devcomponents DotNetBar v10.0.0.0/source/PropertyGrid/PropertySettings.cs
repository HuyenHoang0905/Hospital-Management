#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using DevComponents.AdvTree;
using System.Drawing.Design;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Describes the property settings applied to the property on AdvPropertyGrid control.
    /// </summary>
    [ToolboxItem(false)]
    public class PropertySettings : Component, INotifyPropertyChanged
    {
        #region Events
        /// <summary>
        /// Occurs when property value is being converted to text representation for display. You can handle this event
        /// to provide custom conversion for property values. You must set IsConverted=true on event arguments to indicate that you have performed value conversion.
        /// </summary>
        [Description("Occurs when property value is being converted to text representation for display.")]
        public event ConvertValueEventHandler ConvertPropertyValueToString;

        /// <summary>
        /// Occurs when text entered by user is being converted to typed property value to be assigned to the property. You can handle this event
        /// to provide custom conversion for property values. You must set IsConverted=true on event arguments to indicate that you have performed value conversion.
        /// </summary>
        [Description("Occurs when text entered by user is being converted to typed property value to be assigned to the property.")]
        public event ConvertValueEventHandler ConvertFromStringToPropertyValue;

        /// <summary>
        /// Occurs when property looks for the list of valid values for the property to show on either drop-down or use
        /// in auto-complete list. For example when property type is enum the internal implementation will convert available enum
        /// values into string list and use on drop-down and in auto-complete list. You can use this event to provide
        /// custom value list in combination with ConvertPropertyValueToString, ConvertFromStringToPropertyValue events.
        /// You must set IsListValid=true on event arguments to indicate that list your provided should be used.
        /// </summary>
        [Description("Occurs when property looks for the list of valid values for the property to show on either drop-down or use in auto-complete list.")]
        public event PropertyValueListEventHandler ProvidePropertyValueList;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PropertySettings class.
        /// </summary>
        public PropertySettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertySettings class.
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertySettings(string propertyName)
        {
            _PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySettings class.
        /// </summary>
        /// <param name="propertyDescriptor"></param>
        public PropertySettings(PropertyDescriptor propertyDescriptor)
        {
            _PropertyDescriptor = propertyDescriptor;
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _Image);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Internal Implementation
        private ElementStyle _ReadOnlyStyle = null;
        /// <summary>
        /// Gets or sets read-only property style.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Indicates read-only property style.")]
        public ElementStyle ReadOnlyStyle
        {
            get { return _ReadOnlyStyle; }
            set
            {
                if (_ReadOnlyStyle != value)
                {
                    if (_ReadOnlyStyle != null)
                        _ReadOnlyStyle.StyleChanged -= ReadOnlyStyleChanged;
                    if (value != null)
                        value.StyleChanged += ReadOnlyStyleChanged;
                    _ReadOnlyStyle = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyStyle"));
                }
            }
        }
        private void ReadOnlyStyleChanged(object sender, EventArgs e)
        {

        }
        private bool _ReadOnly = false;
        /// <summary>
        /// Gets or sets whether property is in read-only state. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether property is in read-only state."), Category("Behavior")]
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                if (value != _ReadOnly)
                {
                    bool oldValue = _ReadOnly;
                    _ReadOnly = value;
                    OnReadOnlyChanged(oldValue, value);
                }
            }
        }
        private void OnReadOnlyChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ReadOnly"));
        }

        private SuperTooltipInfo _ReadOnlyTooltip;
        /// <summary>
        /// Gets or sets the SuperTooltip that is displayed when property is in read-only state (ReadOnly=true).
        /// </summary>
        [DefaultValue(null), Description("Indicates SuperTooltip that is displayed when property is in read-only state (ReadOnly=true)"), Category("Behavior")]
        public SuperTooltipInfo ReadOnlyTooltip
        {
            get { return _ReadOnlyTooltip; }
            set
            {
                if (value != _ReadOnlyTooltip)
                {
                    SuperTooltipInfo oldValue = _ReadOnlyTooltip;
                    _ReadOnlyTooltip = value;
                    OnReadOnlyTooltipChanged(oldValue, value);
                }
            }
        }
        private void OnReadOnlyTooltipChanged(SuperTooltipInfo oldValue, SuperTooltipInfo newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ReadOnlyTooltip"));
            
        }

        /// <summary>
        /// Raises the ConvertPropertyValueToString event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnConvertPropertyValueToString(ConvertValueEventArgs e)
        {
            ConvertValueEventHandler handler = ConvertPropertyValueToString;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeConvertPropertyValueToString(ConvertValueEventArgs e)
        {
            OnConvertPropertyValueToString(e);
        }
        internal bool HasConvertPropertyValueToStringHandler
        {
            get
            {
                return ConvertPropertyValueToString != null;
            }
        }

        /// <summary>
        /// Raises the ConvertFromStringToPropertyValue event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnConvertFromStringToPropertyValue(ConvertValueEventArgs e)
        {
            ConvertValueEventHandler handler = ConvertFromStringToPropertyValue;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeConvertFromStringToPropertyValue(ConvertValueEventArgs e)
        {
            OnConvertFromStringToPropertyValue(e);
        }
        internal bool HasConvertFromStringToPropertyValueHandler
        {
            get
            {
                return ConvertFromStringToPropertyValue != null;
            }
        }

        /// <summary>
        /// Raises the ProvidePropertyValueList event.
        /// </summary>
        /// <param name="e">Provides event data</param>
        protected virtual void OnProvidePropertyValueList(PropertyValueListEventArgs e)
        {
            PropertyValueListEventHandler handler = ProvidePropertyValueList;
            if (handler != null)
                handler(this, e);
        }
        internal void InvokeProvidePropertyValueList(PropertyValueListEventArgs e)
        {
            OnProvidePropertyValueList(e);
        }
        internal bool HasProvidePropertyValueListHandler
        {
            get
            {
                return ProvidePropertyValueList != null;
            }
        }

        private SuperTooltipInfo _Tooltip = null;
        /// <summary>
        /// Gets or sets the SuperTooltip that is assigned to the property.
        /// </summary>
        [DefaultValue(null), Localizable(true), Category("Appearance"), Description("Indicates SuperTooltip that is assigned to the property.")]
        public SuperTooltipInfo Tooltip
        {
            get { return _Tooltip; }
            set
            {
                if (value != _Tooltip)
                {
                    SuperTooltipInfo oldValue = _Tooltip;
                    _Tooltip = value;
                    OnTooltipChanged(oldValue, value);
                }
            }
        }
        private void OnTooltipChanged(SuperTooltipInfo oldValue, SuperTooltipInfo newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Tooltip"));
        }

        private Image _Image = null;
        /// <summary>
        /// Gets or sets the image that is displayed next to the property name in the grid.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Localizable(true), Description("Indicates image that is displayed next to the property name in the grid.")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    Image oldValue = _Image;
                    _Image = value;
                    OnImageChanged(oldValue, value);
                }
            }
        }
        private void OnImageChanged(Image oldValue, Image newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Image"));   
        }

        private eCellPartAlignment _ImageAlignment = eCellPartAlignment.NearCenter;
        /// <summary>
        /// Gets or sets the image alignment in relation to the property name inside of the grid cell.
        /// </summary>
        [DefaultValue(eCellPartAlignment.NearCenter), Category("Appearance"), Description("Indicates image alignment in relation to the property name inside of the grid cell.")]
        public eCellPartAlignment ImageAlignment
        {
            get { return _ImageAlignment; }
            set
            {
                if (value != _ImageAlignment)
                {
                    eCellPartAlignment oldValue = _ImageAlignment;
                    _ImageAlignment = value;
                    OnImageAlignmentChanged(oldValue, value);
                }
            }
        }

        private void OnImageAlignmentChanged(eCellPartAlignment oldValue, eCellPartAlignment newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ImageAlignment"));
        }

        private string _Category = null;
        /// <summary>
        /// Gets or sets the custom category node will be placed in instead of category specified by Category attribute.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Localizable(true), Description("Indicates custom category node will be placed in instead of category specified by Category attribute.")]
        public string Category
        {
            get { return _Category; }
            set
            {
                if (value != _Category)
                {
                    string oldValue = _Category;
                    _Category = value;
                    OnCategoryChanged(oldValue, value);
                }
            }
        }

        private void OnCategoryChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Category"));
        }

        private string _DisplayName = null;
        /// <summary>
        /// Gets or sets the custom property name that is displayed in the grid instead of the real property name.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Localizable(true), Description("Indicates custom property name that is displayed in the grid instead of the real property name.")]
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (value != _DisplayName)
                {
                    string oldValue = _DisplayName;
                    _DisplayName = value;
                    OnDisplayNameChanged(oldValue, value);
                }
            }
        }
        private void OnDisplayNameChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
        }

        private string _PropertyName = "";
        /// <summary>
        /// Gets or sets the name of the property that is managed by this informational object. If you set PropertyDescriptor than that is the value 
        /// that will be used by the control regardless of PropertyName value. 
        /// If you set PropertyName but not PropertyDescriptor then all properties with given name
        /// regardless of type will use these settings.
        /// </summary>
        [DefaultValue("Indicates the name of the properties that settings are applied to.")]
        public string PropertyName
        {
            get
            {
                if (_PropertyDescriptor != null) return _PropertyDescriptor.Name;
                return _PropertyName;
            }
            set
            {
                if (_PropertyName == value) return;
                _PropertyName = value;
                OnPropertyNameChanged();
            }
        }
        private void OnPropertyNameChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("PropertyName"));
        }

        private PropertyDescriptor _PropertyDescriptor = null;
        /// <summary>
        /// Gets or sets property descriptor that identifies the property settings apply to. If you set PropertyDescriptor than that is the value 
        /// that will be used by the control regardless of PropertyName value. 
        /// If you set PropertyName but not PropertyDescriptor then all properties with given name
        /// regardless of type will use these settings.
        /// </summary>
        [DefaultValue(null), Description("Indicates property descriptor that identifies the property that uses these settings.")]
        public PropertyDescriptor PropertyDescriptor
        {
            get { return _PropertyDescriptor; }
            set
            {
                if (_PropertyDescriptor != value)
                {
                    _PropertyDescriptor = value;
                    OnPropertyDescriptorChanged();
                }
            }
        }
        private void OnPropertyDescriptorChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("PropertyDescriptor"));
        }

        private ElementStyle _Style = null;
        /// <summary>
        /// Gets or sets default property style.
        /// </summary>
        /// <value>
        /// Default value is null.
        /// </value>
        [DefaultValue(null), /*Editor("DevComponents.AdvTree.Design.ElementStyleTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),*/ Description("Indicates default property style.")]
        public ElementStyle Style
        {
            get { return _Style; }
            set
            {
                if (_Style != value)
                {
                    if (_Style != null)
                        _Style.StyleChanged -= StyleChanged;
                    if (value != null)
                        value.StyleChanged += StyleChanged;
                    _Style = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Style"));
                }
            }
        }
        private void StyleChanged(object sender, EventArgs e)
        {

        }

        private UITypeEditor _UITypeEditor = null;
        /// <summary>
        /// Gets or sets the UITypeEditor used to edit the property.
        /// </summary>
        [DefaultValue(null), Category("Behavior"), Browsable(false)]
        public UITypeEditor UITypeEditor
        {
            get { return _UITypeEditor; }
            set
            {
                if (value != _UITypeEditor)
                {
                    UITypeEditor oldValue = _UITypeEditor;
                    _UITypeEditor = value;
                    OnUITypeEditorChanged(oldValue, value);
                }
            }
        }
        private void OnUITypeEditorChanged(UITypeEditor oldValue, UITypeEditor newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("UITypeEditor"));   
        }

        private PropertyValueEditor _ValueEditor = null;
        /// <summary>
        /// Gets or sets the custom in-line property value editor. Property value editor is created by inheriting from PropertyValueEditor class.
        /// </summary>
        [DefaultValue(null), Browsable(false)]
        public PropertyValueEditor ValueEditor
        {
            get { return _ValueEditor; }
            set
            {
                if (value != _ValueEditor)
                {
                    PropertyValueEditor oldValue = _ValueEditor;
                    _ValueEditor = value;
                    OnValueEditorChanged(oldValue, value);
                }
            }
        }
        private void OnValueEditorChanged(PropertyValueEditor oldValue, PropertyValueEditor newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("ValueEditor"));
            
        }

        private bool _Visible = true;
        /// <summary>
        /// Gets or sets whether node is visible in property grid. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether node is visible in property grid.")]
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                if (value != _Visible)
                {
                    bool oldValue = _Visible;
                    _Visible = value;
                    OnVisibleChanged(oldValue, value);
                }
            }
        }
        private void OnVisibleChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            
        }

        private ePropertyNodeType _PropertyNodeType = ePropertyNodeType.Default;
        /// <summary>
        /// Gets or sets the property node type that is used to edit this property value. Note that content of property grid would need to be
        /// reloaded to apply any changes to this property.
        /// </summary>
        [DefaultValue(ePropertyNodeType.Default), Description("Indicates property node type that is used to edit this property value.")]
        public ePropertyNodeType PropertyNodeType
        {
            get { return _PropertyNodeType; }
            set
            {
                if (value != _PropertyNodeType)
                {
                    ePropertyNodeType oldValue = _PropertyNodeType;
                    _PropertyNodeType = value;
                    OnPropertyNodeTypeChanged(oldValue, value);
                }
            }
        }
        private void OnPropertyNodeTypeChanged(ePropertyNodeType oldValue, ePropertyNodeType newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("PropertyNodeType"));
        }

        private string _Description = null;
        /// <summary>
        /// Gets or sets the property description that overrides description specified on property through DescriptionAttribute. 
        /// Default value is null.
        /// </summary>
        [DefaultValue(null), Description("Indicates property description that overrides description specified on property through DescriptionAttribute. "), Localizable(true)]
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    string oldValue = _Description;
                    _Description = value;
                    OnDescriptionChanged(oldValue, value);
                }
            }
        }
        private void OnDescriptionChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Description"));   
        }
        

        internal static string NamePropertyName = "PropertyName";
        internal static string NamePropertyDescriptor = "PropertyDescriptor";
        internal static string NameVisible = "Visible";
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raises PropertyChanged event.
        /// </summary>
        /// <param name="e">Event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Occurs when property value on the object changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    /// <summary>
    /// Defines the type of the property node in grid that is assigned to property.
    /// </summary>
    public enum ePropertyNodeType
    {
        /// <summary>
        /// Specifies default node type for the property.
        /// </summary>
        Default,
        /// <summary>
        /// Specifies the node type that is constructed as group of option buttons that represent all values for the property. Works best
        /// for smaller set of Enum values.
        /// </summary>
        RadioButtonList
    }
}
#endif