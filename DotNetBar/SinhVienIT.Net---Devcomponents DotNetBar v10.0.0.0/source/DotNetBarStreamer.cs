using System;
using System.Runtime.Serialization;
using System.Xml;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for DotNetBarStreamer.
	/// </summary>
	[Serializable()]
	public sealed class DotNetBarStreamer:IDisposable,ISerializable
	{
		private XmlDocument m_XmlDoc=null;
		private DotNetBarManager m_Owner=null;

		private DotNetBarStreamer(){}
		private DotNetBarStreamer(SerializationInfo info, StreamingContext c)
		{
			SerializationInfoEnumerator en=info.GetEnumerator();
			while(en.MoveNext())
			{
				//System.Windows.Forms.MessageBox.Show("Read stream: "+en.Name);
				if(en.Name=="dotnetbardata")
				{
					string s=(string)en.Value;
					m_XmlDoc=new XmlDocument();
                    m_XmlDoc.LoadXml(s);
					break;
				}
			}
		}
		public DotNetBarStreamer(DotNetBarManager owner)
		{
			m_Owner=owner;
		}

		void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
		{
			/*if(m_Owner==null)
			{
				if(m_XmlDoc!=null)
                    System.Windows.Forms.MessageBox.Show("XML DOC IS not NULL !!!!");
				else
					System.Windows.Forms.MessageBox.Show("Serializing: NULL");
			}
			else
				System.Windows.Forms.MessageBox.Show("Serializing VALUE");*/
			if(m_Owner==null && m_XmlDoc==null)
				return;

			// Very important to set the type to string, if type is not set then
			// complete assembly version is written to the file, probably becouse type is
			// binary and when new version is compiled the old stream could not be loaded again.
			string s=info.AssemblyName;
			string[] sarr=s.Split(',');
			s="";
			for(int i=0;i<sarr.Length;i++)
			{
				if(!((string)sarr.GetValue(i)).ToLower().Trim().StartsWith("version"))
				{
					if(s!="") s+=",";
					s+=sarr.GetValue(i);
				}
			}
			info.AssemblyName=s;
			//System.Reflection.AssemblyName name=System.Reflection.Assembly.GetExecutingAssembly().GetName();
			
			if(m_Owner!=null)
			{
				XmlDocument xml=new XmlDocument();
				m_Owner.SaveDefinition(xml);
				
				info.AddValue("dotnetbardata",xml.OuterXml);
			}
			else
				info.AddValue("dotnetbardata",m_XmlDoc.OuterXml);
		}

		internal XmlDocument Data
		{
			get
			{
				return m_XmlDoc;
			}
		}

		void IDisposable.Dispose()
		{
			m_Owner=null;
		}
	}
}
