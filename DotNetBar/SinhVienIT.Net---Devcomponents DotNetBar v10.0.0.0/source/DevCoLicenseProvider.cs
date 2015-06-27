using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace DevComponents.DotNetBar
{
	[System.ComponentModel.ToolboxItem(false),System.ComponentModel.DesignTimeVisible(false)]
	public class DevCoLicenseProvider:LicenseProvider
	{
		const string DOTNETBAR="dotnetbar";
		public DevCoLicenseProvider()
		{
		}
		public override License GetLicense(LicenseContext context,Type type,object instance,bool allowExceptions)
		{
			DevCoLicense lic = null;
			Debug.Assert(context!=null, "No context provided!");
			#if !TRIAL
			//if no context is provided, do nothing
			if (context != null) 
			{
				//if this control is in runtime mode
				if (context.UsageMode == LicenseUsageMode.Runtime) 
				{
					//retreive the stored license key
					string key = context.GetSavedLicenseKey(type, null);
					//check if the stored license key is null
					// and call IsKeyValid to make sure its valid
					if (key != null && IsKeyValid2(key, type)) 
					{
						//if the key is valid create a new license
						lic = new DevCoLicense(key);
					}
					else
					{
						try
						{
							System.Reflection.Assembly[] assemblies=System.AppDomain.CurrentDomain.GetAssemblies();
							foreach(System.Reflection.Assembly a in assemblies)
							{
								string codeBase = a.CodeBase;
								codeBase = codeBase.Substring(codeBase.LastIndexOf("/") + 1)+".licenses";
								System.IO.Stream stream=this.GetManifestResourceStream(a,codeBase);
								if(stream==null)
									codeBase=codeBase.Replace(".DLL.",".dll.");
								stream=this.GetManifestResourceStream(a,codeBase);
								if(stream!=null)
								{
									key=DeserializeLicenseKey(stream);
									if (key != null && key!="") //IsKeyValid2(key, type)) 
									{
										lic = new DevCoLicense(key);
										break;
									}
								}
							}
						}
						catch
						{
							lic=new DevCoLicense("");
						}
					}
				}
				else
				{
					string sKey="";
					Microsoft.Win32.RegistryKey regkey=Microsoft.Win32.Registry.LocalMachine;
					regkey=regkey.OpenSubKey("Software\\DevComponents\\Licenses",false);
					if(regkey!=null)
					{
						sKey=regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2").ToString();
					}
					//check if the key is valid
					if (IsKeyValid(sKey, type)) 
					{
						//valid key so create a new License
						lic = new DevCoLicense(Key(type));
					}

					//if we managed to create a license, stuff it into the context
					if (lic != null) 
					{
						context.SetSavedLicenseKey(type, lic.LicenseKey);
					}
				}
			}
			#else
			if (context != null) 
			{
				lic=new DevCoLicense("");
				//if this control is in runtime mode
				if (context.UsageMode == LicenseUsageMode.Runtime) 
				{
					RemindForm frm=new RemindForm();
					frm.ShowDialog();
				}
				else
				{
					context.SetSavedLicenseKey(type, lic.LicenseKey);
				}
			}
			#endif

			if(lic==null && context!=null)
			{
				RemindForm frm=new RemindForm();
				frm.ShowDialog();
				lic=new DevCoLicense("");
			}
			return lic;
			
		}

		private System.IO.Stream GetManifestResourceStream(System.Reflection.Assembly a, string resourceName)
		{
			resourceName=resourceName.ToLower();
			string[] resources=a.GetManifestResourceNames();
			foreach(string name in resources)
			{
				if(name.ToLower()==resourceName)
				{
					return a.GetManifestResourceStream(name);
					//break;
				}
			}
			return null;
		}

		private string DeserializeLicenseKey(System.IO.Stream o)
		{
			System.Runtime.Serialization.IFormatter formatter1;
			object obj1=null;
			object[] array1;
			formatter1 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			try
			{
				obj1 = formatter1.Deserialize(o);
			
				if((obj1 as object[]) == null)
					return null;
				array1 = ((object[]) obj1);
				if((array1[0] as string) == null)
					return null;
				
				System.Collections.Hashtable h=((System.Collections.Hashtable)array1[1]);
				
				//Type t=typeof(DotNetBarManager);
				string s=DOTNETBAR;
				foreach(System.Collections.DictionaryEntry entry in h)
				{
					string key=entry.Key.ToString().ToLower();
					if(key.IndexOf(s)>=0)
						return entry.Value as string;
				}
			}
			catch{}
			return null;
 		}

		#if !TRIAL
		private bool IsKeyValid2(string key, Type type)
		{
			if(key==null || key=="")
				return false;
			
			return (Key(type)==key);
		}

		internal static string Key(Type type)
		{
			Byte[] bi=(new System.Text.UnicodeEncoding()).GetBytes(type.ToString());
			byte[] res;
			SHA256 shaM = new SHA256Managed(); 
			res = shaM.ComputeHash(bi);
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			System.IO.StringWriter sw=new System.IO.StringWriter(sb);
			System.Xml.XmlTextWriter xt=new System.Xml.XmlTextWriter(sw);
			xt.WriteBase64(res,0,(int)res.Length);
			xt.Close();
			return sb.ToString();
		}

		private bool IsKeyValid(string key, Type type)
		{
			if(key==null || key=="")
				return false;
//			Byte[] bi=(new System.Text.UnicodeEncoding()).GetBytes(type.ToString()+System.Windows.Forms.SystemInformation.ComputerName);
//			byte[] res;
//			SHA256 shaM = new SHA256Managed(); 
//			res = shaM.ComputeHash(bi);
//
//			System.Text.StringBuilder sb=new System.Text.StringBuilder();
//			System.IO.StringWriter sw=new System.IO.StringWriter(sb);
//			System.Xml.XmlTextWriter xt=new System.Xml.XmlTextWriter(sw);
//			xt.WriteBase64(res,0,(int)res.Length);
//			xt.Close();
			return ("F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"==key);
		}
		#endif
	}
	internal class DevCoLicense:License 
	{
		private string key;        
		public DevCoLicense(string key) 
		{
			this.key = key;
		}
		public override string LicenseKey 
		{ 
			get 
			{
				return key;
			}
		}
		public override void Dispose() 
		{
		}
	}

}
