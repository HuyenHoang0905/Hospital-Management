using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using DevComponents.DotNetBar.Design;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DesignerSupport.
	/// </summary>
	internal class DesignerSupport
	{
		public static ItemContainer CreateItemContainer(IDesignerServices designer, BaseItem parent, eOrientation containerOrientation)
		{
			IDesignerHost dh=designer.GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService cc=designer.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			
			if(dh==null || parent==null || cc==null)
				return null;

			ItemContainer c=null;

			DesignerTransaction trans=dh.CreateTransaction("New DotNetBar Item Container");
			try
			{
				c=dh.CreateComponent(typeof(ItemContainer)) as ItemContainer;
				TypeDescriptor.GetProperties(c)["LayoutOrientation"].SetValue(c,containerOrientation);
				cc.OnComponentChanging(parent,TypeDescriptor.GetProperties(c)["SubItems"]);
				parent.SubItems.Add(c);
				cc.OnComponentChanged(parent,TypeDescriptor.GetProperties(c)["SubItems"],null,null);
			}
			finally
			{
				if(!trans.Canceled)
					trans.Commit();
			}

			return c;
        }

        public static RibbonTabItemGroup CreateRibbonTabItemGroup(RibbonStrip strip, IServiceProvider provider)
		{
			IDesignerHost dh=provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService cc=provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			if(dh==null || cc==null) return null;
            
			DesignerTransaction trans=dh.CreateTransaction("New RibbonTabItemGroup");
			RibbonTabItemGroup group=null;
			try
			{
				group=dh.CreateComponent(typeof(RibbonTabItemGroup)) as RibbonTabItemGroup;
				cc.OnComponentChanging(strip,TypeDescriptor.GetProperties(strip)["TabGroups"]);
				strip.TabGroups.Add(group);				
				cc.OnComponentChanged(strip,TypeDescriptor.GetProperties(strip)["TabGroups"],null,null);
				SetDefaults(group);
			}
			catch
			{
				trans.Cancel();
				throw;
			}
			finally
			{
				if(!trans.Canceled)
					trans.Commit();
			}
			return group;
		}

        public static GalleryGroup CreateGalleryGroup(GalleryContainer gallery, IServiceProvider provider)
        {
            IDesignerHost dh = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IComponentChangeService cc = provider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (dh == null || cc == null) return null;

            DesignerTransaction trans = dh.CreateTransaction("New GalleryGroup");
            GalleryGroup group = null;
            try
            {
                group = dh.CreateComponent(typeof(GalleryGroup)) as GalleryGroup;

                cc.OnComponentChanging(gallery, TypeDescriptor.GetProperties(gallery)["GalleryGroups"]);
                gallery.GalleryGroups.Add(group);
                cc.OnComponentChanged(gallery, TypeDescriptor.GetProperties(gallery)["GalleryGroups"], null, null);

                group.Text = group.Name;
            }
            catch
            {
                trans.Cancel();
                throw;
            }
            finally
            {
                if (!trans.Canceled)
                    trans.Commit();
            }
            return group;
        }
        

		public static void SetDefaults(RibbonTabItemGroup group)
		{
			TypeDescriptor.GetProperties(group)["GroupTitle"].SetValue(group,"New Group");
			TypeDescriptor.GetProperties(group.Style)["Border"].SetValue(group.Style,eStyleBorderType.Solid);
			TypeDescriptor.GetProperties(group.Style)["BorderColor"].SetValue(group.Style,ColorScheme.GetColor("9A3A3B"));
			TypeDescriptor.GetProperties(group.Style)["CornerType"].SetValue(group.Style,eCornerType.Square);
            TypeDescriptor.GetProperties(group.Style)["BackColor"].SetValue(group.Style, ColorScheme.GetColor("AE6D94"));
            TypeDescriptor.GetProperties(group.Style)["BackColor2"].SetValue(group.Style, ColorScheme.GetColor("90487B"));
			TypeDescriptor.GetProperties(group.Style)["BackColorGradientAngle"].SetValue(group.Style,90);
			TypeDescriptor.GetProperties(group.Style)["BorderWidth"].SetValue(group.Style,1);
			TypeDescriptor.GetProperties(group.Style)["TextColor"].SetValue(group.Style,Color.White);
            TypeDescriptor.GetProperties(group.Style)["TextShadowColor"].SetValue(group.Style, Color.Black);
            TypeDescriptor.GetProperties(group.Style)["TextShadowOffset"].SetValue(group.Style, new Point(1,1));
			TypeDescriptor.GetProperties(group.Style)["TextAlignment"].SetValue(group.Style,eStyleTextAlignment.Center);
			TypeDescriptor.GetProperties(group.Style)["TextLineAlignment"].SetValue(group.Style,eStyleTextAlignment.Near);
		}
	}
}
