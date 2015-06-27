using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    #region BubbleButtonCollectionEditor
    /// <summary>
    /// Support for BubbleBarTab tabs design-time editor.
    /// </summary>
    public class BubbleButtonCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        public BubbleButtonCollectionEditor(Type type)
            : base(type)
        {
        }
        protected override Type CreateCollectionItemType()
        {
            return typeof(BubbleButton);
        }
        protected override Type[] CreateNewItemTypes()
        {
            return new Type[] { typeof(BubbleButton) };
        }
        protected override object CreateInstance(Type itemType)
        {
            object item = base.CreateInstance(itemType);
            if (item is BubbleButton)
            {
                BubbleButton button = item as BubbleButton;
                button.Image = Helpers.LoadBitmap("SystemImages.Note24.png");
                button.ImageLarge = Helpers.LoadBitmap("SystemImages.Note64.png");
            }
            return item;
        }
    }
    #endregion
}
