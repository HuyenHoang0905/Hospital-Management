using System;

namespace DevComponents.DotNetBar.Design
{
	public class SideBarPanelItemDesigner:BaseItemDesigner
	{
		protected override void BeforeNewItemAdded(BaseItem item)
		{
			base.BeforeNewItemAdded(item);
			if(item is ButtonItem)
			{
				ButtonItem button=item as ButtonItem;
					
				SideBarPanelItem panel=this.Component as SideBarPanelItem;
				if(panel.Appearance==eSideBarAppearance.Flat)
				{
					button.ImagePosition=eImagePosition.Right;
					button.ButtonStyle=eButtonStyle.ImageAndText;
				}
				else
				{
					button.ImagePosition=eImagePosition.Top;
				}
			}
		}
	}
}
