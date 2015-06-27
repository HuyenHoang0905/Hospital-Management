#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.AdvTree;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    internal class PropertyNodeFactory
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PropertyNodeFactory class.
        /// </summary>
        /// <param name="categoryStyle"></param>
        public PropertyNodeFactory(IPropertyElementStyleProvider styleProvider)
        {
            _StyleProvider = styleProvider;
        }
        #endregion

        #region Internal Implementation
        public PropertyNode CreatePropertyNode(PropertyDescriptor item, object targetComponent, IPropertyGridLocalizer localizer, PropertySettingsCollection propertySettings, bool isMultiObject)
        {
            PropertySettings settings = propertySettings[item, item.Name];
            return CreatePropertyNode(item, targetComponent, localizer, settings, isMultiObject);
        }
        public PropertyNode CreatePropertyNode(PropertyDescriptor item, object targetComponent, IPropertyGridLocalizer localizer, PropertySettings settings, bool isMultiObject)
        {
            PropertyNode node = null;

            if (item.PropertyType == typeof(Color))
                node = new PropertyNodeColor(item);
            else if (item.PropertyType == typeof(bool))
                node = new PropertyCheckBoxNode(item);
            else if (settings != null && settings.PropertyNodeType == ePropertyNodeType.RadioButtonList)
                node = new PropertyOptionListNode(item);
            else
                node = new PropertyNode(item);

            node.IsMultiObject = isMultiObject;
            node.NodesIndent = _ChildPropertiesIndent;
            node.ImageAlignment = eCellPartAlignment.FarCenter;
            string name = localizer.GetPropertyName(item.Name);
            if (string.IsNullOrEmpty(name)) name = item.DisplayName;
            node.Text = name;
            node.TargetComponent = targetComponent;
            Cell cell = new Cell();
            cell.Selectable = false;
            node.Cells.Add(cell);
            
            node.PropertySettings = settings;

            return node;
        }
        public Node CreateCategoryNode(string category, IPropertyGridLocalizer localizer)
        {
            PropertyCategoryNode node = new PropertyCategoryNode();
            if (_StyleProvider != null)
                node.Style = _StyleProvider.CategoryStyle;
            string name = localizer.GetCategoryName(category);
            if (string.IsNullOrEmpty(name)) name = category;
            node.Text = name;
            node.Expanded = true;
            node.FullRowBackground = true;
            node.Selectable = true;
            node.CellNavigationEnabled = false;
            return node;
        }

        private int _ChildPropertiesIndent = 9;
        public int ChildPropertiesIndent
        {
            get { return _ChildPropertiesIndent; }
            set
            {
                _ChildPropertiesIndent = value;
            }
        }

        private IPropertyElementStyleProvider _StyleProvider;
        public IPropertyElementStyleProvider StyleProvider
        {
            get { return _StyleProvider; }
            set { _StyleProvider = value; }
        }
        #endregion


    }
}
#endif