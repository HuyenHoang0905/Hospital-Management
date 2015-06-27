using System.ComponentModel;
using System.Drawing;
using System;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents Rendering Tab used on RibbonControl.
	/// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), Designer("DevComponents.DotNetBar.Design.RibbonTabItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class RibbonTabItem:ButtonItem
	{
		#region Private Variables & Constructor
		private RibbonPanel m_Panel=null;
		private RibbonTabItemGroup m_Group=null;
        private eRibbonTabColor m_ColorTable = eRibbonTabColor.Default; private string m_CashedColorTableName = "Default";
        private bool m_ReducedSize = false;
        private int m_PaddingHorizontal = 0;
		#endregion
        
		#region Internal Implementation
        private bool _RenderTabState = true;
        /// <summary>
        /// Gets or sets whether tab renders its state. Used internally by DotNetBar. Do not set.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool RenderTabState
        {
            get { return _RenderTabState; }
            set
            {
                _RenderTabState = value;
                if (this.ContainerControl is System.Windows.Forms.Control)
                    ((System.Windows.Forms.Control)this.ContainerControl).Invalidate();
                else
                    this.Refresh();
            }
        }

        protected override bool IsFadeEnabled
        {
            get
            {
                if (this.EffectiveStyle == eDotNetBarStyle.Office2010 && WinApi.IsGlassEnabled)
                    return false;
                return base.IsFadeEnabled;
            }
        }
        /// <summary>
        /// Gets or sets the additional padding added around the tab item in pixels. Default value is 0.
        /// </summary>
        [Browsable(true), DefaultValue(0), Category("Layout"), Description("Indicates additional padding added around the tab item in pixels.")]
        public int PaddingHorizontal
        {
            get { return m_PaddingHorizontal; }
            set
            {
                m_PaddingHorizontal = value;
                UpdateTabAppearance();
            }
        }

        /// <summary>
        /// Selects the tab.
        /// </summary>
        public void Select()
        {
            this.Checked = true;
        }
        /// <summary>
        /// Gets or sets whether size of the tab has been reduced below the default calculated size.
        /// </summary>
        internal bool ReducedSize
        {
            get { return m_ReducedSize; }
            set { m_ReducedSize = value; }
        }
        ///// <summary>
        ///// Gets or sets the custom color name. Name specified here must be represented by the coresponding object with the same name that is part
        ///// of the Office2007ColorTable.RibbonTabItemColors collection. See documentation for Office2007ColorTable.RibbonTabItemColors for more information.
        ///// If color table with specified name cannot be found default color will be used. Valid settings for this property override any
        ///// setting to the Color property.
        ///// Applies to items with Office 2007 style only.
        ///// </summary>
        //[Browsable(true), DefaultValue(""), Category("Appearance"), Description("Indicates custom color table name for the item when Office 12 style is used.")]
        //public string CustomColorName
        //{
        //    get { return m_CustomColorName; }
        //    set
        //    {
        //        m_CustomColorName = value;
        //        this.Refresh();
        //    }
        //}

        /// <summary>
        /// Gets or sets the predefined color of item. Color specified here applies to items with Office 2007 style only. It does not have
        /// any effect on other styles. Default value is eRibbonTabColor.Default
        /// </summary>
        [Browsable(true), DefaultValue(eRibbonTabColor.Default), Category("Appearance"), Description("Indicates predefined color of item when Office 2007 style is used.")]
        public new eRibbonTabColor ColorTable
        {
            get { return m_ColorTable; }
            set
            {
                if (m_ColorTable != value)
                {
                    m_ColorTable = value;
                    m_CashedColorTableName = Enum.GetName(typeof(eRibbonTabColor), m_ColorTable);
                    this.Refresh();
                }
            }
        }

        internal override string GetColorTableName()
        {
            return this.CustomColorName != "" ? this.CustomColorName : m_CashedColorTableName;
        }

		/// <summary>
		/// Gets or sets the group this tab belongs to. Groups are optional classification that is used to 
		/// visually group tabs that belong to same functions. These tabs should be positioned next to each other.
		/// </summary>
        [Editor("DevComponents.DotNetBar.Design.RibbonTabItemGroupTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), Browsable(true), DefaultValue(null), DevCoBrowsable(true), Category("Tab Group"), Description("Indicates the group tab belongs to.")]
		public RibbonTabItemGroup Group
		{
			get {return m_Group;}
			set 
			{
				m_Group=value;
				if(this.DesignMode)
				{
					ItemControl c=this.ContainerControl as ItemControl;
					if(c!=null)
						c.RecalcLayout();
				}
			}
		}

		/// <summary>
		/// Resets Group property to default value null.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetGroup()
		{
			TypeDescriptor.GetProperties(this)["Group"].SetValue(this,null);
		}

		/// <summary>
		/// Gets or sets the panel assigned to this ribbon tab item.
		/// </summary>
		[Browsable(false),DefaultValue(null)]
		public RibbonPanel Panel
		{
			get {return m_Panel;}
			set
			{
				m_Panel=value;
				OnPanelChanged();
			}
		}

		private void OnPanelChanged()
		{
			ChangePanelVisibility();
		}

		/// <summary>
		/// Called after Checked property has changed.
		/// </summary>
		protected override void OnCheckedChanged()
		{
            if(this.Checked && this.Parent!=null)
			{
                ChangePanelVisibility();
				foreach(BaseItem item in this.Parent.SubItems)
				{
					if(item==this)
						continue;
					RibbonTabItem b=item as RibbonTabItem;
                    if (b != null && b.Checked)
                    {
                        if (this.DesignMode)
                            TypeDescriptor.GetProperties(b)["Checked"].SetValue(b, false);
                        else
                            b.Checked = false;
                    }
				}
			}

            m_LastAutoExpandTime = DateTime.MinValue;

            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && this.ContainerControl is System.Windows.Forms.Control)
                ((System.Windows.Forms.Control)this.ContainerControl).Invalidate();
            if(!this.Checked)
                ChangePanelVisibility();
            InvokeCheckedChanged();
		}

		private void ChangePanelVisibility()
		{
			if(this.Checked && m_Panel!=null)
			{
                if (this.DesignMode)
                {
                    if (!m_Panel.Visible) m_Panel.Visible = true;
                    TypeDescriptor.GetProperties(m_Panel)["Visible"].SetValue(m_Panel, true);
                    m_Panel.BringToFront();
                }
                else
                {
                    m_Panel.SuspendLayout();
                    m_Panel.Visible = true;
                    m_Panel.ResumeLayout(true);
                    m_Panel.BringToFront();
                }
			}
			else if(!this.Checked && m_Panel!=null && !m_Panel.IsPopupMode) // Panels in popup mode will be taken care of by Ribbon
			{
                if (this.DesignMode)
                    TypeDescriptor.GetProperties(m_Panel)["Visible"].SetValue(m_Panel, false);
                else
                    m_Panel.Visible = false;
			}
		}

        /// <summary>
        /// Occurs just before Click event is fired.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();
            if (!this.Checked)
            {
                if (this.DesignMode)
                    TypeDescriptor.GetProperties(this)["Checked"].SetValue(this, true);
                else
                    this.Checked = true;
            }

            System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
            if (c is RibbonStrip && ((RibbonStrip)c).AutoExpand && c.Parent is RibbonControl)
            {
                RibbonControl rc = (RibbonControl)c.Parent;
                if (!rc.Expanded)
                {
                    TimeSpan span = DateTime.Now.Subtract(m_LastAutoExpandTime);
                    if (m_LastAutoExpandTime == DateTime.MinValue ||
                        Math.Abs(span.TotalMilliseconds) > System.Windows.Forms.SystemInformation.DoubleClickTime)
                    {
                        rc.RibbonTabItemClick(this);
                        m_LastAutoExpandTime = DateTime.MinValue;
                    }
                }
            }
        }

        private DateTime m_LastAutoExpandTime = DateTime.MinValue;
        /// <summary>
        /// Occurs when the item is clicked. This is used by internal implementation only.
        /// </summary>
        //[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //public override void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
        //{
        //                base.InternalClick(mb, mpos);
        //}

        protected override void InvokeDoubleClick()
        {
            System.Windows.Forms.Control c = this.ContainerControl as System.Windows.Forms.Control;
            if (c is RibbonStrip && ((RibbonStrip)c).AutoExpand && c.Parent is RibbonControl)
            {
                //if (m_LastAutoExpandTime == DateTime.Now || m_LastAutoExpandTime == DateTime.MinValue ||
                //    m_LastAutoExpandTime != DateTime.Now && Math.Abs(DateTime.Now.Subtract(m_LastAutoExpandTime).Milliseconds) > System.Windows.Forms.SystemInformation.DoubleClickTime)
                //{
                    ((RibbonControl)c.Parent).RibbonTabItemDoubleClick(this);
                    m_LastAutoExpandTime = DateTime.Now; // DateTime.MinValue;
                //}
            }

            base.InvokeDoubleClick();
        }

		/// <summary>
		/// Called when Visibility of the items has changed.
		/// </summary>
		/// <param name="bVisible">New Visible state.</param>
		protected internal override void OnVisibleChanged(bool bVisible)
		{
			base.OnVisibleChanged(bVisible);
			if(!bVisible && this.Checked)
			{
				TypeDescriptor.GetProperties(this)["Checked"].SetValue(this,false);
				// Try to check first item in the group
				if(this.Parent!=null)
				{
					foreach(BaseItem item in this.Parent.SubItems)
					{
                        if (item == this || !item.GetEnabled() || !item.Visible)
							continue;
						RibbonTabItem b=item as RibbonTabItem;
						if(b!=null)
						{
							TypeDescriptor.GetProperties(b)["Checked"].SetValue(this,true);
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group. The choice is reflected by Checked property.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(""),EditorBrowsable(EditorBrowsableState.Never)]
		public override string OptionGroup
		{
			get {return base.OptionGroup;}
			set {base.OptionGroup=value;}
		}

		/// <summary>
		/// Occurs after item visual style has changed.
		/// </summary>
		protected override void OnStyleChanged()
		{
			base.OnStyleChanged();
            UpdateTabAppearance();
		}

        private void UpdateTabAppearance()
        {
            if (this.EffectiveStyle == eDotNetBarStyle.Office2007)
            {
                this.VerticalPadding = 0;
                this.HorizontalPadding = 12 + m_PaddingHorizontal;
            }
            else if (this.EffectiveStyle == eDotNetBarStyle.Office2010)
            {
                this.VerticalPadding = 1;
                this.HorizontalPadding = 16 + m_PaddingHorizontal;
            }
            else if (this.EffectiveStyle == eDotNetBarStyle.Windows7)
            {
                this.VerticalPadding = 0;
                this.HorizontalPadding = 16 + m_PaddingHorizontal;
            }
            else if (this.EffectiveStyle == eDotNetBarStyle.Office2003)
            {
                this.VerticalPadding = -1;
                this.HorizontalPadding = 2 + m_PaddingHorizontal;
            }
            else
            {
                this.VerticalPadding = 0;
                this.HorizontalPadding = 0 + m_PaddingHorizontal;
            }
            this.NeedRecalcSize = true;
            this.OnAppearanceChanged();
        }

		/// <summary>
		/// Returns the collection of sub items.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
		public override SubItemsCollection SubItems
		{
			get {return base.SubItems;}
		}

        internal override void DoAccesibleDefaultAction()
        {
            this.Checked = true;
        }

        protected override void Invalidate(System.Windows.Forms.Control containerControl)
        {
            Rectangle r = m_Rect;
            r.Width++;
            r.Height++;
            containerControl.Invalidate(r, true);
        }
		#endregion

		#region Hidden Properties
		/// <summary>
		/// Indicates whether the item will auto-collapse (fold) when clicked. 
		/// When item is on popup menu and this property is set to false, menu will not
		/// close when item is clicked.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Behavior"), DefaultValue(true), Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
		public override bool AutoCollapseOnClick
		{
			get
			{
				return base.AutoCollapseOnClick;
			}
			set
			{
				base.AutoCollapseOnClick=value;
			}
		}

		/// <summary>
		/// Indicates whether the item will auto-expand when clicked. 
		/// When item is on top level bar and not on menu and contains sub-items, sub-items will be shown only if user
		/// click the expand part of the button. Setting this propert to true will expand the button and show sub-items when user
		/// clicks anywhere inside of the button. Default value is false which indicates that button is expanded only
		/// if its expand part is clicked.
		/// </summary>
        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DevCoBrowsable(false), Category("Behavior"), Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
		public override bool AutoExpandOnClick
		{
			get
			{
				return base.AutoExpandOnClick;
			}
			set
			{
				base.AutoExpandOnClick=value;
			}
		}

		/// <summary>
		/// Gets or sets whether item can be customized by end user.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Indicates whether item can be customized by user.")]
		public override bool CanCustomize
		{
			get
			{
				return base.CanCustomize;
			}
			set
			{
				base.CanCustomize=value;
			}
		}

		/// <summary>
		/// Gets or set a value indicating whether the button is in the checked state.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),Category("Appearance"),Description("Indicates whether item is checked or not."),DefaultValue(false)]
		public override bool Checked
		{
			get
			{
				return base.Checked;
			}
			set
			{
				base.Checked=value;
			}
		}

		/// <summary>
		/// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false), Category("Behavior"), Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
		public override bool ClickAutoRepeat
		{
			get
			{
				return base.ClickAutoRepeat;
			}
			set
			{
				base.ClickAutoRepeat=value;                
			}
		}

		/// <summary>
		/// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(600), Category("Behavior"), Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
		public override int ClickRepeatInterval
		{
			get
			{
				return base.ClickRepeatInterval;
			}
			set
			{
				base.ClickRepeatInterval=value;                
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the item is enabled.
		/// </summary>
		[Browsable(false),DevCoBrowsable(false),DefaultValue(true),Category("Behavior"),Description("Indicates whether is item enabled.")]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled=value;
			}
		}

		/// <summary>
		/// Indicates item's visiblity when on pop-up menu.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Appearance"), Description("Indicates item's visiblity when on pop-up menu."), DefaultValue(eMenuVisibility.VisibleAlways)]
		public override eMenuVisibility MenuVisibility
		{
			get
			{
				return base.MenuVisibility;
			}
			set
			{
				base.MenuVisibility=value;
			}
		}

		/// <summary>
		/// Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true.
		/// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DevCoBrowsable(false), Category("Appearance"), Description("Indicates when menu items are displayed when MenuVisiblity is set to VisibleIfRecentlyUsed and RecentlyUsed is true."), DefaultValue(ePersonalizedMenus.Disabled)]
		public override ePersonalizedMenus PersonalizedMenus
		{
			get
			{
				return base.PersonalizedMenus;
			}
			set
			{
				base.PersonalizedMenus=value;
			}
		}

		/// <summary>
		/// Indicates Animation type for Popups.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Behavior"), Description("Indicates Animation type for Popups."), DefaultValue(ePopupAnimation.ManagerControlled)]
		public override ePopupAnimation PopupAnimation
		{
			get
			{
				return base.PopupAnimation;
			}
			set
			{
				base.PopupAnimation=value;
			}
		}

		/// <summary>
		/// Indicates the font that will be used on the popup window.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Appearance"), Description("Indicates the font that will be used on the popup window."), DefaultValue(null)]
		public override System.Drawing.Font PopupFont
		{
			get
			{
				return base.PopupFont;
			}
			set
			{
				base.PopupFont=value;
			}
		}

		/// <summary>
		/// Indicates whether sub-items are shown on popup Bar or popup menu.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Appearance"), Description("Indicates whether sub-items are shown on popup Bar or popup menu."), DefaultValue(ePopupType.Menu)]
		public override ePopupType PopupType
		{
			get
			{
				return base.PopupType;
			}
			set
			{
				base.PopupType=value;
			}
		}

		/// <summary>
		/// Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Layout"), Description("Specifies the inital width for the Bar that hosts pop-up items. Applies to PopupType.Toolbar only."), DefaultValue(200)]
		public override int PopupWidth
		{
			get
			{
				return base.PopupWidth;
			}
			set
			{
				base.PopupWidth=value;
			}
		}

		/// <summary>
		/// Gets or sets whether item will display sub items.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(true), Category("Behavior"), Description("Determines whether sub-items are displayed.")]
		public override bool ShowSubItems
		{
			get
			{
				return base.ShowSubItems;
			}
			set
			{
				base.ShowSubItems=value;
			}
		}

		/// <summary>
		/// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false), Category("Appearance"), Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
		public override bool Stretch
		{
			get
			{
				return base.Stretch;
			}
			set
			{
				base.Stretch=value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the expand part of the button item.
		/// </summary>
        [Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), Category("Behavior"), Description("Indicates the width of the expand part of the button item."), DefaultValue(12)]
		public override int SubItemsExpandWidth
		{
			get {return base.SubItemsExpandWidth;}
			set
			{
				base.SubItemsExpandWidth=value;
			}
		}

        /// <summary>
        /// Gets or set the alternative shortcut text.
        /// </summary>
        [System.ComponentModel.Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DevCoBrowsable(false), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts"), System.ComponentModel.DefaultValue("")]
        public override string AlternateShortCutText
        {
            get
            {
                return base.AlternateShortCutText;
            }
            set
            {
                base.AlternateShortCutText = value;
            }
        }

        /// <summary>
        /// Gets or sets whether item separator is shown before this item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates whether this item is beginning of the group.")]
        public override bool BeginGroup
        {
            get
            {
                return base.BeginGroup;
            }
            set
            {
                base.BeginGroup = value;
            }
        }

        /// <summary>
        /// Returns category for this item. If item cannot be customzied using the
        /// customize dialog category is empty string.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(""), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates item category used to group similar items at design-time."), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Category
        {
            get
            {

                return base.Category;
            }
            set
            {
                base.Category = value;
            }
        }

        /// <summary>
        /// Gets or sets the text color of the button when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The foreground color used to display text when mouse is over the item."), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color HotForeColor
        {
            get
            {
                return base.HotForeColor;
            }
            set
            {
                base.HotForeColor = value;
            }
        }

        /// <summary>
        /// Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item."), System.ComponentModel.DefaultValue(eHotTrackingStyle.Default), EditorBrowsable(EditorBrowsableState.Never)]
        public override eHotTrackingStyle HotTrackingStyle
        {
            get { return base.HotTrackingStyle; }
            set
            {
                base.HotTrackingStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the text color of the button.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), EditorBrowsable(EditorBrowsableState.Never), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The foreground color used to display text.")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }
		#endregion
	}
}
