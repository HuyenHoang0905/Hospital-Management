using System;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DesignTimeDte.
	/// </summary>
	public class DesignTimeDte
	{
		public static string GetProjectPath(IServiceProvider service)
		{
			object dte=DesignTimeDte.GetDTE(service);
			//EnvDTE.DTE dte=System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE") as EnvDTE.DTE;
			if(dte!=null)
			{
				System.Array pjs=dte.GetType().InvokeMember("ActiveSolutionProjects",BindingFlags.GetProperty,null,dte, new object []{}) as System.Array;
				if(pjs==null)
				{
					//MessageBox.Show("Failed to get ActiveSolutionProjects");
					return "";
				}
				if(pjs.Length>0)
				{
					object project=pjs.GetValue(0);
					//MessageBox.Show(((EnvDTE.Project)project).Properties.Item("FullPath").Value.ToString());
					object properties=project.GetType().InvokeMember("Properties",BindingFlags.GetProperty,null,project, new object []{});
					if(properties!=null)
					{
						object item=properties.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,properties, new object []{"FullPath"});
						if(item!=null)
						{
							object val=item.GetType().InvokeMember("Value",BindingFlags.GetProperty,null,item, new object []{});
							if(val!=null && val is string)
                                return val.ToString();
						}
						//else
						//	MessageBox.Show("Failed to get item");
					}
					//else
					//	MessageBox.Show("Failed to get properties");
				}
				//else
				//	MessageBox.Show("PJS length is ZERO");
			}
			//else
			//	MessageBox.Show("Failed to get DTE");
			return "";
		}

		public static string GetDefinitionPath(string definitionName, IServiceProvider service)
		{
			// In design-time our document has to be open for definition to be loaded
			//EnvDTE.DTE dte=GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString()) as EnvDTE.DTE;
			object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte==null)
				return "";
			//EnvDTE.Document doc=dte.ActiveDocument;
			object doc=dte.GetType().InvokeMember("ActiveDocument",BindingFlags.GetProperty,null,dte, new object []{});
			if(doc!=null)
			{
				//string docPath=doc.Path;
				string docPath=(string)doc.GetType().InvokeMember("Path",BindingFlags.GetProperty,null,doc, new object []{});
				if(!docPath.EndsWith("\\"))
					docPath+="\\";
				if(System.IO.File.Exists(docPath+definitionName))
					return docPath;
			}
			// Enumerate open documents and try to find our definition in thier path
			object documents=dte.GetType().InvokeMember("Documents",BindingFlags.GetProperty,null,dte, new object []{});
			//foreach(EnvDTE.Document doc1 in dte.Documents)
			int count=(int)documents.GetType().InvokeMember("Count",BindingFlags.GetProperty,null,documents, new object []{});
			for(int i=1;i<=count;i++)
			{
				object doc1=documents.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,documents, new object []{i});
				//string docPath=doc1.Path;
				string docPath=(string)doc1.GetType().InvokeMember("Path",BindingFlags.GetProperty,null,doc1, new object []{});
				if(!docPath.EndsWith("\\"))
					docPath+="\\";
				if(System.IO.File.Exists(docPath+definitionName))
					return docPath;
			}
			// In need just return project path...
			return GetProjectPath(service);
		}

		public static string GetActiveDocumentPath(IServiceProvider service)
		{
			// In design-time our document has to be open for definition to be loaded
			//EnvDTE.DTE dte=GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString()) as EnvDTE.DTE;
			//EnvDTE.Document doc=dte.ActiveDocument;
			object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte==null)
				return "";
			object doc=dte.GetType().InvokeMember("ActiveDocument",BindingFlags.GetProperty,null,dte, new object []{});
			if(doc!=null)
			{
				string docPath=(string)doc.GetType().InvokeMember("Path",BindingFlags.GetProperty,null,doc, new object []{}); //doc.Path;
				if(!docPath.EndsWith("\\"))
					docPath+="\\";
				return docPath;
			}
			// In need just return project path...
			return GetProjectPath(service);
		}

		public static bool AddFileToProject(string filename, IServiceProvider service)
		{
			bool ret=false;
			object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte!=null)
			{
//				System.Array pjs=dte.GetType().InvokeMember("ActiveSolutionProjects",BindingFlags.GetProperty,null,dte, new object []{}) as System.Array;
//				if(pjs==null)
//					return false;
//
//				if(pjs.Length>0)
//				{
//					object project=pjs.GetValue(0);					
//				}
				object itemOperations=dte.GetType().InvokeMember("ItemOperations",BindingFlags.GetProperty,null,dte, new object []{});
				if(itemOperations!=null)
				{
					object projectItem=itemOperations.GetType().InvokeMember("AddExistingItem",BindingFlags.InvokeMethod,null,itemOperations, new object []{filename});
					if(projectItem!=null)
					{
						try
						{
							object props=projectItem.GetType().InvokeMember("Properties",BindingFlags.GetProperty,null,projectItem, new object []{});
							if(props!=null)
							{
								object item=props.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,props, new object []{"BuildAction"});
								if(item!=null)
								{
									item.GetType().InvokeMember("Value",BindingFlags.SetProperty,null,item, new object []{3});
									ret=true;
								}
							}
						}
						catch
						{
							ret=true;
						}
					}
				}
				//EnvDTE.ProjectItem item=dte.ItemOperations.AddExistingItem(filename);
				//item.Properties.Item("BuildAction").Value=3;
//				foreach(EnvDTE.Property prop in item.Properties)
//				{
//					MessageBox.Show(prop.Name+"  "+prop.Value);
//				}
			}
			return ret;
		}

        //public static bool DeleteFromProject(string filename, IServiceProvider service)
        //{
        //    try
        //    {
        //        object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
        //        if(dte==null)
        //            return false;

        //        System.Array pjs=dte.GetType().InvokeMember("ActiveSolutionProjects",BindingFlags.GetProperty,null,dte, new object []{}) as System.Array;
        //        if(pjs==null)
        //            return false;

        //        object project=null;
        //        if(pjs.Length>0)
        //            project=pjs.GetValue(0);
        //        if(project==null)
        //            return false;

        //        object pjItems=project.GetType().InvokeMember("ProjectItems",BindingFlags.GetProperty,null,project, new object []{});
        //        if(pjItems==null)
        //            return false;

        //        object pjItem=pjItems.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,pjItems, new object []{filename});
        //        //project.ProjectItems.Item(filename) as EnvDTE.ProjectItem;
        //        if(pjItem==null)
        //            return false;

        //        pjItem.GetType().InvokeMember("Delete",BindingFlags.InvokeMethod,null,pjItem, new object []{});
        //        return true;
        //    }
        //    catch(Exception){}
        //    return false;
        //}

		public static bool ExistInProject(string filename, IServiceProvider service)
		{
//			EnvDTE.DTE dte=GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString()) as EnvDTE.DTE;
//			if(dte==null)
//				return false;
//			Array projects=dte.ActiveSolutionProjects as Array;
			object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte==null)
				return false;

			System.Array projects=dte.GetType().InvokeMember("ActiveSolutionProjects",BindingFlags.GetProperty,null,dte, new object []{}) as System.Array;
			if(projects==null)
				return false;
			foreach(object pj in projects)
			{
				object pji=pj.GetType().InvokeMember("ProjectItems",BindingFlags.GetProperty,null,pj, new object []{});
				int count=(int)pji.GetType().InvokeMember("Count",BindingFlags.GetProperty,null,pji, new object []{});
				//foreach(EnvDTE.ProjectItem pi in pj.ProjectItems)
				for(int i=1;i<=count;i++)
				{
					object pi=pji.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,pji, new object []{i});
					if(CheckProjectItem(pi,filename))
						return true;
				}
			}
			return false;
		}

		private static bool CheckProjectItem(object pi,string filename)
		{
			try
			{
				string name=(string)pi.GetType().InvokeMember("Name",BindingFlags.GetProperty,null,pi, new object []{});
				if(name.ToLower()==filename.ToLower())
					return true;
				short fileCount=(short)pi.GetType().InvokeMember("FileCount",BindingFlags.GetProperty,null,pi, new object []{});
				for(short i=0;i<fileCount;i++)
				{
					//string fn=pi.get_FileNames(i);
					string fn=(string)pi.GetType().InvokeMember("get_FileNames",BindingFlags.GetProperty,null,pi, new object []{i});
					if(fn.ToLower().EndsWith(filename.ToLower()))
						return true;
				}
				object pji=pi.GetType().InvokeMember("ProjectItems",BindingFlags.GetProperty,null,pi, new object []{});
				int count=(int)pji.GetType().InvokeMember("Count",BindingFlags.GetProperty,null,pji, new object []{});
				for(int i=1;i<=count;i++)
				//foreach(EnvDTE.ProjectItem item in pi.ProjectItems)
				{
					object pItem=pji.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,pji, new object []{i});
					if(CheckProjectItem(pItem,filename))
						return true;
				}
			}
			catch(Exception){}
			return false;
		}

		public static bool ExistInProject2(string filename, IServiceProvider service)
		{
			try
			{
				object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
				if(dte==null)
					return false;

				System.Array pjs=dte.GetType().InvokeMember("ActiveSolutionProjects",BindingFlags.GetProperty,null,dte, new object []{}) as System.Array;
				if(pjs==null)
					return false;

				object project=null;
				if(pjs.Length>0)
					project=pjs.GetValue(0);
				if(project==null)
					return false;

				object pjItems=project.GetType().InvokeMember("ProjectItems",BindingFlags.GetProperty,null,project, new object []{});
				if(pjItems==null)
					return false;

				object pjItem=pjItems.GetType().InvokeMember("Item",BindingFlags.InvokeMethod,null,pjItems, new object []{filename});
				if(pjItem==null)
					return false;
			}
			catch(Exception)
			{
				return false;
			}

			return true;
		}

		public static bool CheckOutFile(string filename, IServiceProvider service)
		{
			try
			{
				object dte=DesignTimeDte.GetDTE(service); //GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
				if(dte==null)
					return false;
				object sctrl=dte.GetType().InvokeMember("SourceControl",BindingFlags.GetProperty,null,dte, new object []{});
				if(sctrl==null)
				{
					return false;
				}
				object res=sctrl.GetType().InvokeMember("IsItemUnderSCC",BindingFlags.InvokeMethod,null,sctrl, new object []{filename});
				if(res!=null && res is bool && (bool)res==true)
				{
					res=sctrl.GetType().InvokeMember("CheckOutItem",BindingFlags.InvokeMethod,null,sctrl, new object []{filename});
					if(res!=null && res is bool)
						return (bool)res;
					else
						return false;
				}
				return true;
			}
			catch(Exception){}
            return false;
		}

		public static object GetDTE(IServiceProvider service)
		{
			object dte=GetMSDEVFromGIT("VisualStudio.DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte==null)
				dte=GetMSDEVFromGIT(".DTE",System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
			if(dte==null && service!=null)
			{
				Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly a in loadedAssemblies)
				{
					if (a.FullName.ToLower().StartsWith("envdte"))
					{
						Type t=a.GetType("EnvDTE._DTE", false, true);
						if (t != null)
						{
							dte=service.GetService(t);
							break;
						}
					}
				}
			}
			return dte;
		}

#if FRAMEWORK20
		[DllImport("ole32.dll")]		
		public static extern int GetRunningObjectTable(int reserved, out 
			System.Runtime.InteropServices.ComTypes.IRunningObjectTable prot);
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out System.Runtime.InteropServices.ComTypes.IBindCtx
            ppbc);

        [STAThread]
        public static object GetMSDEVFromGIT(string strProgID, string processId)
        {
            System.Runtime.InteropServices.ComTypes.IRunningObjectTable prot;
            System.Runtime.InteropServices.ComTypes.IEnumMoniker pMonkEnum;
            try
            {
                GetRunningObjectTable(0, out prot);
                prot.EnumRunning(out pMonkEnum);
                pMonkEnum.Reset();          // Churn through enumeration.				
                IntPtr fetched=IntPtr.Zero;
                System.Runtime.InteropServices.ComTypes.IMoniker[] pmon = new System.Runtime.InteropServices.ComTypes.IMoniker[1];
                while (pMonkEnum.Next(1, pmon, fetched) == 0)
                {
                    System.Runtime.InteropServices.ComTypes.IBindCtx pCtx;
                    CreateBindCtx(0, out pCtx);
                    string str;
                    pmon[0].GetDisplayName(pCtx, null, out str);
                    //					#if DEBUG
                    //					System.Windows.Forms.MessageBox.Show(str+"   strProgId="+strProgID+"  processId="+processId);
                    //					#endif
                    if (str.IndexOf(strProgID) > 0 && (str.IndexOf(":" + processId) > 0 || processId == ""))
                    {
                        object objReturnObject;
                        prot.GetObject(pmon[0], out objReturnObject);
                        object ide = (object)objReturnObject;
                        return ide;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
#else
        [DllImport("ole32.dll")]		
		public static extern int GetRunningObjectTable(int reserved, out 
			UCOMIRunningObjectTable prot);	
        [DllImport("ole32.dll")]		
		public static extern int  CreateBindCtx(int reserved, out UCOMIBindCtx 
			ppbc);

         [STAThread]		
		public static object GetMSDEVFromGIT(string strProgID, string processId)		
		{			
			UCOMIRunningObjectTable prot;			
			UCOMIEnumMoniker pMonkEnum;			
			try			
			{				
				GetRunningObjectTable(0,out prot);				
				prot.EnumRunning(out pMonkEnum);				
				pMonkEnum.Reset();          // Churn through enumeration.				
				int fetched;				
				UCOMIMoniker []pmon = new UCOMIMoniker[1];				
				while(pMonkEnum.Next(1, pmon, out fetched) == 0) 				
				{					
					UCOMIBindCtx pCtx;					
					CreateBindCtx(0, out pCtx);					
					string str;					
					pmon[0].GetDisplayName(pCtx,null,out str);
//					#if DEBUG
//					System.Windows.Forms.MessageBox.Show(str+"   strProgId="+strProgID+"  processId="+processId);
//					#endif
					if(str.IndexOf(strProgID)>0 && (str.IndexOf(":"+processId)>0 || processId==""))
					{		
						object objReturnObject;		
						prot.GetObject(pmon[0],out objReturnObject);	
						object ide = (object)objReturnObject;		
						return ide;		
					}		
				}	
			}	
			catch	
			{	
				return null;	
			}
			return null;	
		}
#endif



    }
}
