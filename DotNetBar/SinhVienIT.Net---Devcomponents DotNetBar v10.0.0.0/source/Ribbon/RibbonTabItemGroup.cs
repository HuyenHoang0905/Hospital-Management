using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents a group RibbonTabItem objects are assigned to.
	/// </summary>
    [DesignTimeVisible(false), ToolboxItem(false), TypeConverterAttribute("DevComponents.DotNetBar.Design.RibbonTabItemGroupConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class RibbonTabItemGroup:Component
	{
		#region Private Variables & Constructor
		private ElementStyle m_Style=null;
		private string m_GroupTitle="";
		private RibbonStrip m_ParentRibbonStrip=null;
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ArrayList DisplayPositions=new ArrayList();
        private eRibbonTabGroupColor m_Color = eRibbonTabGroupColor.Default;
        private string m_Name = "";
        private string m_CustomColorName = "";

		public RibbonTabItemGroup()
		{
			m_Style=new ElementStyle();
			m_Style.StyleChanged+=new EventHandler(StyleChanged);
		}
		#endregion

		#region Internal Implementation
        /// <summary>
        /// Gets or sets the predefined color of the group. Color specified here applies to groups with Office 12 style only. It does not have
        /// any effect on other styles. Default value is eRibbonTabGroupColor.Default
        /// </summary>
        [Browsable(true), DefaultValue(eRibbonTabGroupColor.Default), Category("Appearance"), Description("Indicates predefined color of the group when Office 12 style is used.")]
        public eRibbonTabGroupColor Color
        {
            get { return m_Color; }
            set
            {
                if (m_Color != value)
                {
                    m_Color = value;
                    if (this.ParentRibbonStrip != null)
                        this.ParentRibbonStrip.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom color name. Name specified here must be represented by the coresponding object with the same name that is part
        /// of the Office2007ColorTable.RibbonTabGroupColors collection. See documentation for Office2007ColorTable.RibbonTabGroupColors for more information.
        /// If color table with specified name cannot be found default color will be used. Valid settings for this property override any
        /// setting to the Color property.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(""), Category("Appearance"), Description("Indicates custom color table name for the button when Office 2007 style is used.")]
        public string CustomColorName
        {
            get { return m_CustomColorName; }
            set
            {
                m_CustomColorName = value;
                if (this.ParentRibbonStrip != null)
                    this.ParentRibbonStrip.Invalidate();
            }
        }

		/// <summary>
		/// Gets the style for tab group.
		/// </summary>
		[Browsable(false),Category("Background"),Description("Gets the style for tab group."),DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ElementStyle Style
		{
			get {return m_Style;}
		}

		/// <summary>
		/// Gets or sets title of the group that will be displayed when group is visually represented.
		/// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(""), Localizable(true)]
		public string GroupTitle
		{
			get {return m_GroupTitle;}
			set
			{
				m_GroupTitle=value;
				if(this.DesignMode && m_ParentRibbonStrip!=null)
				{
					m_ParentRibbonStrip.RecalcLayout();
				}
			}
		}

		private void StyleChanged(object sender, EventArgs e)
		{
			if(m_ParentRibbonStrip!=null)
				m_ParentRibbonStrip.Refresh();
		}
		
		/// <summary>
		/// Gets or sets parent ribbon strip for this group.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Browsable(false)]
		internal RibbonStrip ParentRibbonStrip
		{
			get {return m_ParentRibbonStrip;}
			set
			{
				m_ParentRibbonStrip=value;
				if(m_ParentRibbonStrip!=null)
					m_Style.SetColorScheme(m_ParentRibbonStrip.ColorScheme);
			}
		}

		public override string ToString()
		{
			if(m_GroupTitle.Length>0)
				return m_GroupTitle;
			return base.ToString ();
		}

		/// <summary>
		/// Gets or sets whether RibbonTabItem objects that belong to this group are visible. Setting this property will
		/// show/hide all RibbonTabItem objects that are assigned to this group through RibbonTabItem.Group property.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Visible
		{
			get
			{
				if(m_ParentRibbonStrip==null)
					return true;
				bool visible=false;
				foreach(BaseItem item in m_ParentRibbonStrip.Items)
				{
					if(item is RibbonTabItem)
					{
						RibbonTabItem tab=item as RibbonTabItem;
						if(tab.Group == this && tab.Visible)
						{
							visible=true;
							break;
						}
					}
				}

				return visible;
			}
			set
			{
				if(m_ParentRibbonStrip==null)
					return;
				foreach(BaseItem item in m_ParentRibbonStrip.Items)
				{
					if(item is RibbonTabItem)
					{
						RibbonTabItem tab=item as RibbonTabItem;
						if(tab.Group==this)
							tab.Visible=value;
					}
				}
				m_ParentRibbonStrip.RecalcLayout();
			}
		}

        /// <summary>
        /// Gets whether any tab from this tab group is selected.
        /// </summary>
        [Browsable(false)]
        public bool IsTabFromGroupSelected
        {
            get
            {
                if(m_ParentRibbonStrip==null)
					return false;
				foreach(BaseItem item in m_ParentRibbonStrip.Items)
				{
					if(item is RibbonTabItem)
					{
						RibbonTabItem tab=item as RibbonTabItem;
                        if (tab.Group == this && tab.Checked)
                            return true;
					}
				}
                return false;
            }
        }

        /// <summary>
        /// Selected first tab that is part of this group.
        /// </summary>
        public void SelectFirstTab()
        {
            if (m_ParentRibbonStrip == null)
                return;
            foreach (BaseItem item in m_ParentRibbonStrip.Items)
            {
                if (item is RibbonTabItem)
                {
                    RibbonTabItem tab = item as RibbonTabItem;
                    if (tab.Group == this && tab.Visible)
                    {
                        tab.Checked = true;
                        break;
                    }
                }
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
        /// Gets an array of Rectangle objects that describe the visual position on the ribbon control of the group titles displayed.
        /// If tabs that belong to a tab group are not next to each other then there will be multiple rectangle returned as part of the array
        /// for each tab group that is apart.
        /// </summary>
        [Browsable(false)]
        public Rectangle[] TitleBounds
        {
            get
            {
                return (Rectangle[])DisplayPositions.ToArray(typeof(Rectangle));
            }
        }
		#endregion
	}
}
