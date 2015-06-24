using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    #region ElementStyleCollectionEditor
    /// <summary>
    /// Support for ElementStyle design-time editor.
    /// </summary>
    public class ElementStyleCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        /// <summary>Creates new instance of the object.</summary>
        public ElementStyleCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(ElementStyle);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(ElementStyle) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            return item;
        }
    }
    #endregion
}
