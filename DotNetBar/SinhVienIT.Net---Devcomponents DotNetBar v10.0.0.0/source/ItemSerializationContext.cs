using System;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides context information for serialization.
    /// </summary>
    public class ItemSerializationContext
    {
        /// <summary>
        /// Gets or sets reference to context XmlElement an item is being serialized to.
        /// </summary>
        public System.Xml.XmlElement ItemXmlElement = null;

        /// <summary>
        /// Gets or sets whether SerializeItem event handler has been defined and whether event should be fired.
        /// </summary>
        public bool HasSerializeItemHandlers = false;

        /// <summary>
        /// Gets or sets whether DeserializeItem event handler has been defined and whether event should be fired.
        /// </summary>
        public bool HasDeserializeItemHandlers = false;

        /// <summary>
        /// Provides access to serializer.
        /// </summary>
        public ICustomSerialization Serializer = null;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public System.ComponentModel.Design.IDesignerHost _DesignerHost = null;
        internal Hashtable DockControls = null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public BaseItem CreateItemFromXml(System.Xml.XmlElement xmlItem)
        {
            if (_DesignerHost != null)
            {
                BaseItem item = null;
                string name = "";
                if (xmlItem.HasAttribute("name")) name = xmlItem.GetAttribute("name");
                try
                {
                    item = BarFunctions.CreateItemFromXml(xmlItem, _DesignerHost, name);
                }
                catch { }
                if (item == null) item = BarFunctions.CreateItemFromXml(xmlItem, _DesignerHost, "");
                if (name != "") item.GlobalName = name;

                return item;
            }
            else
                return BarFunctions.CreateItemFromXml(xmlItem);
        }
    }
}
