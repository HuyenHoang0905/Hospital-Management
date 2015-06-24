using System;
using System.Collections;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Designer for ExplorerBarGroupItem object.
	/// </summary>
	public class ExplorerBarGroupItemDesigner:BaseItemDesigner
	{
		protected override void BeforeNewItemAdded(BaseItem item)
		{
			base.BeforeNewItemAdded(item);
			if(item is ButtonItem)
				ExplorerBarGroupItem.SetDesignTimeDefaults((ButtonItem)item,((ExplorerBarGroupItem)this.Component).StockStyle);
		}

		protected override void SetDesignTimeDefaults()
		{
			if (this.Component is ExplorerBarGroupItem)
			{
				ExplorerBarGroupItem e = this.Component as ExplorerBarGroupItem;
				e.SetDefaultAppearance();
                e.StockStyle = eExplorerBarStockStyle.SystemColors;
			}
		}

		public override System.Collections.ICollection AssociatedComponents
		{
			get
			{
				System.Collections.ArrayList c=new System.Collections.ArrayList(base.AssociatedComponents);
				BaseItem container=this.Component as BaseItem;
				if(container!=null)
				{
					foreach(BaseItem item in container.SubItems)
						c.Add(item);
				}
				return c;
			}
		}
	}
}
