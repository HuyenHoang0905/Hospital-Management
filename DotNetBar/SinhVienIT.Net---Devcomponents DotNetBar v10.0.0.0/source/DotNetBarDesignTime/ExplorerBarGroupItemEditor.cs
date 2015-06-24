using System;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Collection editor for ExplorerGroupItem.
	/// </summary>
	public class ExplorerBarGroupItemEditor:System.ComponentModel.Design.CollectionEditor
	{
		public ExplorerBarGroupItemEditor(Type type):base(type)
		{
		}
		protected override Type CreateCollectionItemType()
		{
			return typeof(ButtonItem);
		}
		protected override object CreateInstance(Type itemType)
		{
			object item=base.CreateInstance(itemType);
			if(item is ButtonItem)
			{
				ButtonItem button=item as ButtonItem;
				ExplorerBarGroupItem.SetDesignTimeDefaults(button,eExplorerBarStockStyle.Custom);
			}
			return item;
		}
	}
}
