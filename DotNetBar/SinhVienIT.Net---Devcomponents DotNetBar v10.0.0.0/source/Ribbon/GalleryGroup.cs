using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the popup gallery group that groups set of items inside of gallery into the group.
    /// </summary>
    [DesignTimeVisible(false), ToolboxItem(false), TypeConverterAttribute("DevComponents.DotNetBar.Design.GalleryGroupConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class GalleryGroup : Component
    {
        #region Private Variables
        private string m_Text = "";
        private string m_Name = "";
        private GalleryContainer m_ParentGallery = null;
        private int m_DisplayOrder = 0;
        private SubItemsCollection m_Items = null;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public GalleryGroup()
        {
            m_Items = new SubItemsCollection(null);
            m_Items.IgnoreEvents = true;
            m_Items.AllowParentRemove = false;
        }

        /// <summary>
        /// Gets or sets title of the group that will be displayed on the group label when on popup gallery.
        /// </summary>
        [Browsable(true), DefaultValue(""), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Localizable(true), Description("Indicates title of the group that will be displayed on the group label when on popup gallery.")]
        public string Text
        {
            get { return m_Text; }
            set
            {
                if (value == null) value = "";
                m_Text = value;
            }
        }

        /// <summary>
        /// Gets or sets name of the group that can be used to identify item from the code.
        /// </summary>
        [Browsable(false), Category("Design"), Description("Indicates the name used to identify the group.")]
        public string Name
        {
            get
            {
                if (this.Site != null)
                    m_Name = this.Site.Name;
                return m_Name;
            }
            set
            {
                if (this.Site != null)
                    this.Site.Name = value;
                if (value == null)
                    m_Name = "";
                else
                    m_Name = value;
            }
        }

        /// <summary>
        /// Gets the parent gallery for the group.
        /// </summary>
        [Browsable(false)]
        public GalleryContainer ParentGallery
        {
            get
            {
                return m_ParentGallery;
            }
        }

        internal void SetParentGallery(GalleryContainer value)
        {
            m_ParentGallery = value;
        }

        public override string ToString()
        {
            if (m_Text.Length > 0)
                return m_Text;
            return base.ToString();
        }

        /// <summary>
        /// Gets or sets the display order for the group when displayed on the popup. Lower values are displayed closer to the top. Default value is 0. 
        /// </summary>
        [Browsable(true), DefaultValue(0), Description("Indicates display order for the group when displayed on the popup."), Category("Layout")]
        public int DisplayOrder
        {
            get { return m_DisplayOrder; }
            set
            {
                m_DisplayOrder = value;
            }
        }

        /// <summary>
        /// Gets the collection of the items assigned to this group.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SubItemsCollection Items
        {
            get { return m_Items; }
        }
        #endregion
    }
}
