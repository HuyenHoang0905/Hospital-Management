#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DevComponents.AdvTree;
using System.Collections;

namespace DevComponents.DotNetBar
{
    internal class PropertyParser
    {
        private const string STR_Misc = "Misc.";

        #region Constructors
        private object _SelectedObject = null;
        private object[] _SelectedObjects = null;
        private TypeConverter _Converter = null;
        private Attribute[] _AttributeFilter = null;
        private PropertyNodeFactory _PropertyNodeFactory = null;
        private List<string> _IgnoredProperties = null;
        private List<string> _IgnoredCategories = null;
        private IPropertyGridLocalizer _Localizer = null;
        private PropertySettingsCollection _PropertySettings = null;
        private ITypeDescriptorContext _TypeDescriptorContext = null;
        /// <summary>
        /// Initializes a new instance of the PropertyParser class.
        /// </summary>
        /// <param name="selectedObject"></param>
        public PropertyParser(object selectedObject,
            Attribute[] attributeFilter,
            PropertyNodeFactory nodeFactory,
            List<string> ignoredProperties,
            List<string> ignoredCategories,
            IPropertyGridLocalizer localizer,
            PropertySettingsCollection propertySettings)
        {
            _SelectedObject = selectedObject;
            _AttributeFilter = attributeFilter;
            _PropertyNodeFactory = nodeFactory;
            _IgnoredProperties = ignoredProperties;
            _IgnoredCategories = ignoredCategories;
            _Localizer = localizer;
            _PropertySettings = propertySettings;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyParser class.
        /// </summary>
        /// <param name="selectedObject"></param>
        public PropertyParser(
            ITypeDescriptorContext context, 
            object selectedObject,
            Attribute[] attributeFilter,
            PropertyNodeFactory nodeFactory,
            List<string> ignoredProperties,
            List<string> ignoredCategories,
            IPropertyGridLocalizer localizer,
            PropertySettingsCollection propertySettings)
        {
            _TypeDescriptorContext = context;
            _SelectedObject = selectedObject;
            _AttributeFilter = attributeFilter;
            _PropertyNodeFactory = nodeFactory;
            _IgnoredProperties = ignoredProperties;
            _IgnoredCategories = ignoredCategories;
            _Localizer = localizer;
            _PropertySettings = propertySettings;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyParser class.
        /// </summary>
        /// <param name="selectedObject"></param>
        public PropertyParser(object[] selectedObjects,
            Attribute[] attributeFilter,
            PropertyNodeFactory nodeFactory,
            List<string> ignoredProperties,
            List<string> ignoredCategories,
            IPropertyGridLocalizer localizer,
            PropertySettingsCollection propertySettings)
        {
            _SelectedObjects = selectedObjects;
            _AttributeFilter = attributeFilter;
            _PropertyNodeFactory = nodeFactory;
            _IgnoredProperties = ignoredProperties;
            _IgnoredCategories = ignoredCategories;
            _Localizer = localizer;
            _PropertySettings = propertySettings;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyParser class.
        /// </summary>
        /// <param name="selectedObject"></param>
        public PropertyParser(
            ITypeDescriptorContext context,
            object[] selectedObjects,
            Attribute[] attributeFilter,
            PropertyNodeFactory nodeFactory,
            List<string> ignoredProperties,
            List<string> ignoredCategories,
            IPropertyGridLocalizer localizer,
            PropertySettingsCollection propertySettings)
        {
            _TypeDescriptorContext = context;
            _SelectedObjects = selectedObjects;
            _AttributeFilter = attributeFilter;
            _PropertyNodeFactory = nodeFactory;
            _IgnoredProperties = ignoredProperties;
            _IgnoredCategories = ignoredCategories;
            _Localizer = localizer;
            _PropertySettings = propertySettings;
        }
        #endregion

        #region Internal Implementation
        internal void Parse(NodeCollection nodeCollection, ePropertySort propertySort, SuperTooltip helpTooltip)
        {
            PropertyDescriptorCollection propertyCollection = GetProperties();
            bool hasIgnoredProperties = _IgnoredProperties.Count > 0;
            bool hasIgnoredCategories = _IgnoredCategories.Count > 0;

            if (propertySort == ePropertySort.Categorized || propertySort == ePropertySort.CategorizedAlphabetical)
            {
                Dictionary<string, Node> table = new Dictionary<string, Node>();
                foreach (PropertyDescriptor item in propertyCollection)
                {
                    if (item == null || hasIgnoredProperties && _IgnoredProperties.Contains(item.Name)) continue;

                    string category = GetCategory(item);
                    PropertySettings settings = _PropertySettings[item, item.Name];
                    if (settings != null && settings.Category != null)
                        category = settings.Category;
                        
                    if (hasIgnoredCategories && !string.IsNullOrEmpty(category) && _IgnoredCategories.Contains(category)) continue;

                    Node categoryNode = null;
                    if (!table.TryGetValue(category, out categoryNode))
                    {
                        categoryNode = _PropertyNodeFactory.CreateCategoryNode(category, _Localizer);
                        nodeCollection.Add(categoryNode);
                        table.Add(category, categoryNode);
                    }
                    PropertyNode node = _PropertyNodeFactory.CreatePropertyNode(item, _SelectedObject ?? _SelectedObjects, _Localizer, settings, _SelectedObjects != null);
                    categoryNode.Nodes.Add(node);
                    node.OnLoaded();
                    if (_HelpType == ePropertyGridHelpType.SuperTooltip && helpTooltip != null)
                        node.SetHelpTooltip(helpTooltip);
                    node.UpdateDisplayedValue();
                }

                if (propertySort == ePropertySort.CategorizedAlphabetical)
                {
                    foreach (Node item in table.Values)
                    {
                        item.Nodes.Sort();
                    }

                    nodeCollection.Sort();
                }
            }
            else
            {
                foreach (PropertyDescriptor item in propertyCollection)
                {
                    if (item == null || _IgnoredProperties.Contains(item.Name)) continue;

                    PropertyNode node = _PropertyNodeFactory.CreatePropertyNode(item, _SelectedObject ?? _SelectedObjects, _Localizer, _PropertySettings, _SelectedObjects != null);
                    nodeCollection.Add(node);
                    node.OnLoaded();
                    if (_HelpType == ePropertyGridHelpType.SuperTooltip && helpTooltip != null)
                        node.SetHelpTooltip(helpTooltip);
                    node.UpdateDisplayedValue();
                }
                if (propertySort == ePropertySort.Alphabetical)
                    nodeCollection.Sort();
            }
        }

        internal static bool GetReadOnlyAttribute(PropertyDescriptor item, object instance)
        {
            bool readOnly = TypeDescriptor.GetAttributes(instance).Contains(InheritanceAttribute.InheritedReadOnly);
            // if (item.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes))
            if (TypeDescriptor.GetAttributes(instance).Contains(ReadOnlyAttribute.Yes))
            {
                readOnly |= true;
            }
            //if(_Converter!=null && _Converter)
            
            ReadOnlyAttribute readOnlyAttribute = item.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;
            if (!readOnly && readOnlyAttribute != null && readOnlyAttribute.IsReadOnly && !readOnlyAttribute.IsDefaultAttribute())
                readOnly |= true;

            //if (item.Attributes[typeof(ImmutableObjectAttribute)].Equals(ImmutableObjectAttribute.Yes))
            //    readOnly |= true;

            return readOnly; // item.IsReadOnly;
        }

        private static CategoryAttribute _CategoryAttribute = new CategoryAttribute();
        private string GetCategory(PropertyDescriptor item)
        {
            string s = item.Category;
            if (!string.IsNullOrEmpty(s)) return s;
            
            CategoryAttribute cat = item.Attributes[_CategoryAttribute.GetType()] as CategoryAttribute;
            if (cat != null && !string.IsNullOrEmpty(cat.Category)) return cat.Category;
            
            return STR_Misc;
        }

        private TypeConverter GetConverter(object component)
        {
            if (_Converter != null && _SelectedObject != null) return _Converter;

            TypeConverter converter = TypeDescriptor.GetConverter(component);
            if(converter.GetPropertiesSupported())
                return converter;
            return null;
        }
        private PropertyDescriptorCollection GetProperties()
        {
            Attribute[] attributes = _AttributeFilter;

            if (_SelectedObject != null)
            {
                object component = _SelectedObject;
                return GetProperties(_TypeDescriptorContext, attributes, component, GetConverter(component));
            }

            int selectedObjectsCount = _SelectedObjects.Length;
            List<PropertyDescriptor> mergedProperties = new List<PropertyDescriptor>();

            List<PropertyDescriptorCollection> allProperties = new List<PropertyDescriptorCollection>(selectedObjectsCount);
            foreach (object selectedObject in _SelectedObjects)
            {
                allProperties.Add(GetProperties(_TypeDescriptorContext, attributes, selectedObject, GetConverter(selectedObject)));
            }

            PropertyDescriptorCollection col1 = allProperties[0];

            for (int i = 0; i < col1.Count; i++)
            {
                PropertyDescriptor propDesc = col1[i];
                bool mergable = propDesc.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute();
                if (!mergable) continue;

                int childPropertyOccurrenceCount = 1;
                for (int j = 1; j < allProperties.Count; j++)
                {
                    PropertyDescriptorCollection col2 = allProperties[j];
                    foreach (PropertyDescriptor propDesc2 in col2)
                    {
                        if (ArePropertiesSame(propDesc2, propDesc) && propDesc.Attributes[typeof(MergablePropertyAttribute)].IsDefaultAttribute())
                        {
                            childPropertyOccurrenceCount++;
                            break;
                        }
                    }
                }

                if (childPropertyOccurrenceCount == selectedObjectsCount)
                {
                    mergedProperties.Add(propDesc);
                }
            }

            return new PropertyDescriptorCollection(mergedProperties.ToArray());
        }

        private bool ArePropertiesSame(PropertyDescriptor propDesc2, PropertyDescriptor propDesc)
        {
            return propDesc2.Name == propDesc.Name && propDesc2.PropertyType == propDesc.PropertyType;
        }
        public static PropertyDescriptor GetProperty(string propertyName, object component, TypeConverter converter)
        {
            PropertyDescriptorCollection col = GetProperties(null, component, converter);
            foreach (PropertyDescriptor item in col)
            {
                if (item.Name == propertyName)
                    return item;
            }
            return null;
        }
        
        public static PropertyDescriptorCollection GetProperties(Attribute[] attributes, object component, TypeConverter converter)
        {
            return GetProperties(null, attributes, component, converter);
        }

         public static PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, Attribute[] attributes, object component, TypeConverter converter)
        {
            if (component == null) return null;

            if (component is ICustomTypeDescriptor)
            {
                ICustomTypeDescriptor descriptor = (ICustomTypeDescriptor)component;
                if (attributes != null)
                    return descriptor.GetProperties(attributes);
                return descriptor.GetProperties();
            }

            // Note that this assumes that converter supports GetProperties!
            if (converter != null)
            {
                return converter.GetProperties(context, component, attributes);
            }

            Type type = component.GetType();
            
            if (attributes != null)
                return TypeDescriptor.GetProperties(component, attributes);

            return TypeDescriptor.GetProperties(component);
        }

        private ePropertyGridHelpType _HelpType = ePropertyGridHelpType.HelpHidden;
        public ePropertyGridHelpType HelpType
        {
            get { return _HelpType; }
            set { _HelpType = value; }
        }

        public TypeConverter TypeConverter
        {
            get { return _Converter; }
            set { _Converter = value; }
        }
        #endregion



    }
}
#endif