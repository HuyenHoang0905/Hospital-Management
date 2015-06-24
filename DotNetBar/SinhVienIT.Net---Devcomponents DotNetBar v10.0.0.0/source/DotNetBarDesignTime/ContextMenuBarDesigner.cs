using System;
using System.Text;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design
{
	/// <summary>
	/// Represents designer for the ContextMenuBar control.
	/// </summary>
    public class ContextMenuBarDesigner : BarDesigner
    {
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            string[] remove = new string[] {
                "AccessibleDescription", "AccessibleName", "AccessibleRole",
                "AlwaysDisplayDockTab", "AlwaysDisplayKeyAccelerators", "AutoCreateCaptionMenu",
                "AutoHide", "AutoHideAnimationTime", "AutoSyncBarCaption",
                "BackColor", "BackgroundImage", "BackgroundImageAlpha","BackgroundImageLayout",
                "BarType", "CanDockBottom", "CanDockTop","CanDockLeft", "CanDockRight", "CanDockTab",
                "CanHide", "CanReorderTabs", "CanUndock",
                "DisplayMoreItemsOnMenu", "DockedBorderStyle", "DockOrientation",
                "DockTabAlignment", "Enabled", "EqualButtonSize",
                "FadeEffect", "GrabHandleStyle", "ImageSize",
                "ImagesLarge", "ImagesMedium", "ItemSpacing",
                "LayoutType", "MenuBar", "PaddingBottom","PaddingTop","PaddingLeft","PaddingRight",
                "RoundCorners", "SaveLayoutChanges", "SingleLineColor", "SelectedDockTab",
                "TabNavigation", "ThemeAware",
                "WrapItemsDock"
            };
            foreach(string prop in remove)
                properties.Remove(prop);
        }

        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = null;
                verbs = new DesignerVerb[]
					{
						new DesignerVerb("Add Context Menu", new EventHandler(CreateButton))
						};
                return new DesignerVerbCollection(verbs);
            }
        }

        protected override void OnitemCreated(BaseItem item)
        {
            if (item is ButtonItem) ((ButtonItem)item).AutoExpandOnClick = true;
            base.OnitemCreated(item);
        }
    }
}
