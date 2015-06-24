using System;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;

#if DOTNETBAR
namespace DevComponents.DotNetBar
#else
namespace DevComponents.Tree
#endif
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public class DevCoSerialize : Attribute
    {
        public DevCoSerialize(){}
    }

	/// <summary>
	/// Represents class that can serialize compatible marked properties.
	/// </summary>
    public class ElementSerializer
    {
        private static string XmlImageName="image";
        private static string XmlIconName="icon";
        private static string XmlAttributeName="name";
//    	private static string XmlCollectionName="collection";
//    	private static string XmlTypeName="type";
//    	private static string XmlObjectName="object";

        public static void Serialize(object element, XmlElement xmlElem)
        {
            PropertyInfo[] propInfo = element.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in propInfo)
            {
                if (prop.GetCustomAttributes(typeof(DevCoSerialize), true).Length > 0)
                {
                	// Check whether property needs to be serialized
                	object[] attributesDefaultValue=prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                	if (attributesDefaultValue.Length > 0)
                	{
                		DefaultValueAttribute att=attributesDefaultValue[0] as DefaultValueAttribute;
                		object propertyValue = prop.GetValue(element, null);
                		if(propertyValue==att.Value)
                			continue;
                		if(propertyValue!=null && att.Value!=null && propertyValue.Equals(att.Value))
                			continue;
                	}
                	else if(element.GetType().GetMethod("ShouldSerialize" + prop.Name,BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!=null)
                	{
                		MethodInfo mi=element.GetType().GetMethod("ShouldSerialize" + prop.Name,BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                		object result=mi.Invoke(element,null);
                		if(result is bool && !((bool)result))
                			continue;
                	}
                	
                    string attributeName=XmlConvert.EncodeName(prop.Name);
                    Type t=prop.PropertyType;
                    if (t.IsPrimitive || t == typeof(string) || t.IsEnum)
                    {
                        if (t == typeof(string))
                            xmlElem.SetAttribute(attributeName, prop.GetValue(element, null).ToString());
                        else if (t.IsEnum)
                        {
                            xmlElem.SetAttribute(attributeName, Enum.Format(t, prop.GetValue(element, null), "g"));
                        }
                        else if (t == typeof(bool))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((bool)prop.GetValue(element, null)));
                        else if (t == typeof(Int32))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((Int32)prop.GetValue(element, null)));
                        else if (t == typeof(double))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((double)prop.GetValue(element, null)));
                        else if (t == typeof(float))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((float)prop.GetValue(element, null)));
                        else if (t == typeof(Int16))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((Int16)prop.GetValue(element, null)));
                        else if (t == typeof(Int64))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((Int64)prop.GetValue(element, null)));
                        else if (t == typeof(byte))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((byte)prop.GetValue(element, null)));
                        else if (t == typeof(char))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((char)prop.GetValue(element, null)));
                        else if (t == typeof(decimal))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((decimal)prop.GetValue(element, null)));
                        else if (t == typeof(Guid))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((Guid)prop.GetValue(element, null)));
                        else if (t == typeof(sbyte))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((sbyte)prop.GetValue(element, null)));
                        else if (t == typeof(TimeSpan))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((TimeSpan)prop.GetValue(element, null)));
                        else if (t == typeof(ushort))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((ushort)prop.GetValue(element, null)));
                        else if (t == typeof(uint))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((uint)prop.GetValue(element, null)));
                        else if (t == typeof(ulong))
                            xmlElem.SetAttribute(attributeName, XmlConvert.ToString((ulong)prop.GetValue(element, null)));
                        else
                            throw new ApplicationException("Unsupported serialization type '" + t.ToString() + "' on property '" + attributeName + "'");
                    }
                    else if (t == typeof(Color))
                        xmlElem.SetAttribute(attributeName, ColorToString((Color)prop.GetValue(element, null)));
                    else if (t == typeof(Image))
                    {
                        XmlElement imageXmlElement = xmlElem.OwnerDocument.CreateElement(XmlImageName);
                        imageXmlElement.SetAttribute(XmlAttributeName, attributeName);
                        xmlElem.AppendChild(imageXmlElement);
                        SerializeImage((Image)prop.GetValue(element, null), imageXmlElement);
                    }
                    else if (t == typeof(Icon))
                    {
                        XmlElement imageXmlElement = xmlElem.OwnerDocument.CreateElement(XmlIconName);
                        imageXmlElement.SetAttribute(XmlAttributeName, attributeName);
                        xmlElem.AppendChild(imageXmlElement);
                        SerializeIcon((Icon)prop.GetValue(element, null), imageXmlElement);
                    }
                    else if (t == typeof(Font))
                    {
                        xmlElem.SetAttribute(attributeName, FontToString((Font)prop.GetValue(element, null)));
                    }
                    else
                        throw new ApplicationException("Unsupported serialization type '" + t.ToString() + "'" + "' on property '" + attributeName + "'");
                }
            }
        }

        private static string ColorToString(Color clr)
        {
            if (clr.IsSystemColor || clr.IsNamedColor || clr.IsKnownColor)
                return ("." + clr.Name);
            else
                return clr.ToArgb().ToString();
        }

        private static Color ColorFromString(string sclr)
        {
            if (sclr == "")
                return Color.Empty;
            if (sclr[0] == '.')
                return Color.FromName(sclr.Substring(1));
            else
                return Color.FromArgb(System.Xml.XmlConvert.ToInt32(sclr));
        }

        private static string FontToString(Font font)
        {
            if (font == null)
                return "";

            string s = font.Name + "," + XmlConvert.ToString(font.Size) + "," + XmlConvert.ToString(font.Bold) + "," + XmlConvert.ToString(font.Italic) + "," +
                XmlConvert.ToString(font.Underline) + "," + XmlConvert.ToString(font.Strikeout);

            return s;
        }

        private static Font FontFromString(string fontString)
        {
            if(fontString=="")
                return null;

            string[] options=fontString.Split(',');
            FontStyle fontStyle = FontStyle.Regular;
            if (XmlConvert.ToBoolean(options[2]))
                fontStyle |= FontStyle.Bold;
            if (XmlConvert.ToBoolean(options[3]))
                fontStyle |= FontStyle.Italic;
            if (XmlConvert.ToBoolean(options[4]))
                fontStyle |= FontStyle.Underline;
            if (XmlConvert.ToBoolean(options[5]))
                fontStyle |= FontStyle.Strikeout;
            Font font = new Font(options[0], XmlConvert.ToSingle(options[1]), fontStyle);
            return font;
        }

        public static void Deserialize(object element, XmlElement xmlElem)
        {
            Type type=element.GetType();
            foreach(XmlAttribute att in xmlElem.Attributes)
            {
                string name=XmlConvert.DecodeName(att.Name);
                PropertyInfo prop=type.GetProperty(name);
                if(prop!=null)
                {
                    Type t=prop.PropertyType;

                    if (t.IsPrimitive)
                    {
                        if(t==typeof(bool))
                            prop.SetValue(element, XmlConvert.ToBoolean(att.Value), null);
                        else if (t == typeof(Int32))
                            prop.SetValue(element, XmlConvert.ToInt32(att.Value), null);
                        else if (t == typeof(double))
                            prop.SetValue(element, XmlConvert.ToDouble(att.Value), null);
                        else if (t == typeof(float))
                            prop.SetValue(element, XmlConvert.ToSingle(att.Value), null);
                        else if (t == typeof(Int16))
                            prop.SetValue(element, XmlConvert.ToInt16(att.Value), null);
                        else if (t == typeof(Int64))
                            prop.SetValue(element, XmlConvert.ToInt64(att.Value), null);
                        else if (t == typeof(byte))
                            prop.SetValue(element, XmlConvert.ToByte(att.Value), null);
                        else if (t == typeof(char))
                            prop.SetValue(element, XmlConvert.ToChar(att.Value), null);
                        else if (t == typeof(decimal))
                            prop.SetValue(element, XmlConvert.ToDecimal(att.Value), null);
                        else if (t == typeof(Guid))
                            prop.SetValue(element, XmlConvert.ToGuid(att.Value), null);
                        else if (t == typeof(sbyte))
                            prop.SetValue(element, XmlConvert.ToSByte(att.Value), null);
                        else if (t == typeof(TimeSpan))
                            prop.SetValue(element, XmlConvert.ToTimeSpan(att.Value), null);
                        else if (t == typeof(ushort))
                            prop.SetValue(element, XmlConvert.ToUInt16(att.Value), null);
                        else if (t == typeof(uint))
                            prop.SetValue(element, XmlConvert.ToUInt32(att.Value), null);
                        else if (t == typeof(ulong))
                            prop.SetValue(element, XmlConvert.ToUInt64(att.Value), null);
                    }
                    else if (t == typeof(string))
                        prop.SetValue(element, att.Value, null);
                    else if (t.IsEnum)
                        prop.SetValue(element, Enum.Parse(t, att.Value), null);
                    else if (t == typeof(Color))
                        prop.SetValue(element, ColorFromString(att.Value), null);
                    else if (t == typeof(Font))
                        prop.SetValue(element, FontFromString(att.Value), null);
                }
            }

            foreach(XmlElement childElem in xmlElem.ChildNodes)
            {
                if (childElem.Name == XmlImageName)
                {
                    string name = XmlConvert.DecodeName(childElem.GetAttribute(XmlAttributeName));
                    PropertyInfo prop = type.GetProperty(name);
                    if (prop != null)
                    {
                        prop.SetValue(element, DeserializeImage(childElem), null);
                    }
                }
                else if (childElem.Name == XmlIconName)
                {
                    string name = XmlConvert.DecodeName(childElem.GetAttribute(XmlAttributeName));
                    PropertyInfo prop = type.GetProperty(name);
                    if (prop != null)
                    {
                        prop.SetValue(element, DeserializeIcon(childElem), null);
                    }
                }
            }
        }

		public static void SerializeImage(System.Drawing.Image image,XmlElement xml)
		{
			if(image==null)
				return;

			System.IO.MemoryStream mem=new System.IO.MemoryStream(1024);
			// TODO: Beta 2 issue with the ImageFormat. RawFormat on image object does not return the actual image format
			// Right now it is hard coded to PNG but in final version we should get the original image format
			image.Save(mem,System.Drawing.Imaging.ImageFormat.Png);

			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			System.IO.StringWriter sw=new System.IO.StringWriter(sb);
				
			System.Xml.XmlTextWriter xt=new System.Xml.XmlTextWriter(sw);
			xt.WriteBase64(mem.GetBuffer(),0,(int)mem.Length);

			xml.InnerText=sb.ToString();
		}

		public static void SerializeIcon(System.Drawing.Icon icon, XmlElement xml)
		{
			if(icon==null)
				return;

			System.IO.MemoryStream mem=new System.IO.MemoryStream(1024);
			// TODO: Beta 2 issue with the ImageFormat. RawFormat on image object does not return the actual image format
			// Right now it is hard coded to PNG but in final version we should get the original image format
			icon.Save(mem);

			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			System.IO.StringWriter sw=new System.IO.StringWriter(sb);
				
			System.Xml.XmlTextWriter xt=new System.Xml.XmlTextWriter(sw);

			xml.SetAttribute("encoding","binhex");
			//xt.WriteBase64(mem.GetBuffer(),0,(int)mem.Length);
			xt.WriteBinHex(mem.GetBuffer(),0,(int)mem.Length);

			xml.InnerText=sb.ToString();
		}

		/// <summary>
		/// XML element is expected to be something like <image>Image data Base64 encoded</image>
		/// </summary>
		/// <param name="xml">Image data</param>
		/// <returns></returns>
		public static System.Drawing.Image DeserializeImage(XmlElement xml)
		{
			System.Drawing.Image img=null;
			if(xml==null || xml.InnerText=="")
				return null;

			System.IO.StringReader sr=new System.IO.StringReader(xml.OuterXml);
			System.Xml.XmlTextReader xr= new System.Xml.XmlTextReader(sr);
			System.IO.MemoryStream mem=new System.IO.MemoryStream(1024);
			// Skip <image> to data
			xr.Read();
				
			byte[] base64 = new byte[1024];
			int base64len = 0;
			do
			{
				base64len = xr.ReadBase64(base64, 0, 1024);
				if(base64len>0)
					mem.Write(base64,0,base64len);

			} while (base64len!=0);

			img=System.Drawing.Image.FromStream(mem);

			return img;
		}

		public static System.Drawing.Icon DeserializeIcon(XmlElement xml)
		{
			System.Drawing.Icon img=null;
			if(xml==null || xml.InnerText=="")
				return null;
			bool bDecodeBinHex=false;
			if(xml.HasAttribute("encoding") && xml.GetAttribute("encoding")=="binhex")
				bDecodeBinHex=true;
			System.IO.StringReader sr=new System.IO.StringReader(xml.OuterXml);
			System.Xml.XmlTextReader xr= new System.Xml.XmlTextReader(sr);
			System.IO.MemoryStream mem=new System.IO.MemoryStream(1024);
			// Skip <image> to data
			xr.Read();
				
			byte[] base64 = new byte[1024];
			int base64len = 0;
			if(bDecodeBinHex)
			{
				do
				{
					base64len = xr.ReadBinHex(base64, 0, 1024);
					if(base64len>0)
						mem.Write(base64,0,base64len);

				} while (base64len!=0);
			}
			else
			{
				do
				{
					base64len = xr.ReadBase64(base64, 0, 1024);
					if(base64len>0)
						mem.Write(base64,0,base64len);

				} while (base64len!=0);
			}
			mem.Position=0;
			img=new System.Drawing.Icon(mem);

			return img;
		}
    }
}
