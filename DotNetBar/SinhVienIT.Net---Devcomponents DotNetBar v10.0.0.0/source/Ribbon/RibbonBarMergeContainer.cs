using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the container for RibbonBar objects that will be merged into the MDI parent ribbon control.
    /// </summary>
    [ToolboxBitmap(typeof(RibbonBarMergeContainer), "Ribbon.RibbonControl.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.RibbonBarMergeContainerDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
    public class RibbonBarMergeContainer : RibbonPanel
    {
        #region Private variables
        private bool m_AutoActivateTab = true;
        private string m_RibbonTabText = "";
        private string m_MergeIntoRibbonTabItemName = "";
        private ArrayList m_MergedRibbonBarsList = new ArrayList();
        private bool m_RibbonTabItemCreated = false;
        private bool m_PreMergedVisibleState = false;
        private bool m_AllowMerge = true;
        private string m_MergeRibbonGroupName = "";
        private int m_MergeRibbonTabItemIndex = -1;
        private eRibbonTabColor m_RibbonTabColorTable = eRibbonTabColor.Default;
        #endregion

        #region Events
        /// <summary>
        /// Occurs before the RibbonBar objects from container are merged into the Ribbon control.
        /// </summary>
        public event EventHandler BeforeRibbonMerge;
        /// <summary>
        /// Occurs after the RibbonBar objects are merged into the Ribbon control.
        /// </summary>
        public event EventHandler AfterRibbonMerge;

        /// <summary>
        /// Occurs after the RibbonBar objects are removed from the Ribbon control.
        /// </summary>
        public event EventHandler BeforeRibbonUnmerge;

        /// Occurs before the RibbonBar objects are removed from the Ribbon control.
        /// </summary>
        public event EventHandler AfterRibbonUnmerge;
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets whether RibbonBar controls are merged into the RibbonControl.
        /// </summary>
        public bool IsMerged
        {
            get
            {
                return m_MergedRibbonBarsList.Count > 0;
            }
        }
        /// <summary>
        /// Removes any RibbonBar objects that were merged into the Ribbon control.
        /// </summary>
        /// <param name="ribbon">Reference to ribbon control to remove RibbonBar objects from.</param>
        public virtual void RemoveMergedRibbonBars(RibbonControl ribbon)
        {
            OnBeforeRibbonUnmerge(new EventArgs());
            Control parent = null;
            foreach (RibbonBar c in m_MergedRibbonBarsList)
            {
                if (c.Parent != null)
                {
                    if (parent == null)
                    {
                        parent = c.Parent;
                        parent.SuspendLayout();
                    }
                    c.Parent.Controls.Remove(c);
                }
                this.Controls.Add(c);
                c.Enabled = false; // Disable shortcuts
            }

            m_MergedRibbonBarsList.Clear();

            if (parent != null)
                parent.ResumeLayout();

            if (m_RibbonTabItemCreated)
            {
                this.RibbonTabItem.Parent.SubItems.Remove(this.RibbonTabItem);
                if (this.RibbonTabItem.Panel != null && this.RibbonTabItem.Panel.Parent!=null)
                {
                    this.RibbonTabItem.Panel.Parent.Controls.Remove(this.RibbonTabItem.Panel);
                }
                this.RibbonTabItem.Panel.Dispose();
                this.RibbonTabItem.Panel = null;
                m_RibbonTabItemCreated = false;
            }

            this.RibbonTabItem = null;
            if (this.Visible != m_PreMergedVisibleState)
                this.Visible = m_PreMergedVisibleState;

            OnAfterRibbonUnmerge(new EventArgs());
        }

        /// <summary>
        /// Merges RibbonBar objects from this container into the Ribbon control.
        /// </summary>
        /// <param name="ribbon">Reference to ribbon control to remove RibbonBar objects from.</param>
        public virtual void MergeRibbonBars(RibbonControl ribbon)
        {
            OnBeforeRibbonMerge(new EventArgs());
            m_PreMergedVisibleState = this.Visible;
            if (this.Visible) this.Visible = false;
            RibbonTabItem tab = GetCreateRibbonTabItem(ribbon);
            Control[] controls = new Control[this.Controls.Count];
            this.Controls.CopyTo(controls, 0);
            int xpos = tab.Panel.Width + 1;
            tab.Panel.SuspendLayout();
            foreach (Control c in controls)
            {
                if (c is RibbonBar)
                {
                    this.Controls.Remove(c);
                    if (m_MergeIntoRibbonTabItemName.Length > 0 && !tab.Panel.DefaultLayout)
                    {
                        c.Left = xpos;
                        xpos += c.Width;
                    }
                    tab.Panel.Controls.Add(c);
                    c.Enabled = true;
                    m_MergedRibbonBarsList.Add(c);
                }
            }
            tab.Panel.ResumeLayout();
            OnAfterRibbonMerge(new EventArgs());
        }

        private RibbonTabItem GetCreateRibbonTabItem(RibbonControl ribbon)
        {
            if (m_MergeIntoRibbonTabItemName != "")
            {
                if(ribbon.Items.IndexOf(m_MergeIntoRibbonTabItemName)<0)
                    throw new NullReferenceException("MergeIntoRibbonTabItemName specified (" + m_MergeIntoRibbonTabItemName + ") cannot be found in RibbonControl.Items collection");
                this.RibbonTabItem = ribbon.Items[m_MergeIntoRibbonTabItemName] as RibbonTabItem;

                return this.RibbonTabItem;
            }

            string tabText = m_RibbonTabText;
            if (tabText.Length == 0)
                tabText = this.Name;
            string tabName = (this.Parent != null ? this.Parent.Name + "." : "") + this.Name;
            this.RibbonTabItem = ribbon.CreateRibbonTab(tabText, tabName, m_MergeRibbonTabItemIndex);
            this.RibbonTabItem.ColorTable = m_RibbonTabColorTable;
            m_RibbonTabItemCreated = true;

            if (m_MergeRibbonGroupName.Length > 0)
            {
                RibbonTabItemGroup group = ribbon.TabGroups[m_MergeRibbonGroupName];
                if (group != null)
                {
                    int lastTabGroupIndex = -1;
                    for (int i = 0; i < ribbon.Items.Count; i++)
                    {
                        if (ribbon.Items[i] is RibbonTabItem && ((RibbonTabItem)ribbon.Items[i]).Group == group)
                            lastTabGroupIndex = i;
                    }
                    this.RibbonTabItem.Group = group;
                    if (lastTabGroupIndex >= 0)
                    {
                        ribbon.Items.Remove(this.RibbonTabItem);
                        ribbon.Items.Insert(lastTabGroupIndex + 1, this.RibbonTabItem);
                    }
                }
            }

            return this.RibbonTabItem;
        }

        /// <summary>
        /// Gets or sets whether RibbonTab item the RibbonBar controls are added to when merged is automatically activated (selected) after
        /// controls are merged. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behaviour"), Description("Indicates whether RibbonTab item the RibbonBar controls are added to when merged is automatically activated (selected) after controls are merged.")]
        public bool AutoActivateTab
        {
            get { return m_AutoActivateTab; }
            set { m_AutoActivateTab = value; }
        }

        /// <summary>
        /// Gets or sets whether merge functionality is enabled for the container. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behaviour"), Description("Indicates whether merge functionality is enabled for the container.")]
        public bool AllowMerge
        {
            get { return m_AllowMerge; }
            set { m_AllowMerge = value; }
        }

        /// <summary>
        /// Gets or sets the Ribbon Tab text for the tab that will be created when ribbon bar objects from this container are merged into the ribbon.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Data"), Description("Indicates the Ribbon Tab text for the tab that will be created when ribbon bar objects from this container are merged into the ribbon.")]
        public string RibbonTabText
        {
            get { return m_RibbonTabText; }
            set 
            { 
                m_RibbonTabText = value;
                if (this.IsMerged && this.RibbonTabItem != null)
                    this.RibbonTabItem.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the predefined color for the ribbon tab that is created when ribbon bar controls are merged into the ribbon.
        /// Default value is eRibbonTabColor.Default
        /// </summary>
        [Browsable(true), DefaultValue(eRibbonTabColor.Default), Category("Appearance"), Description("Indicates predefined color for the ribbon tab that is created when ribbon bar controls are merged into the ribbon.")]
        public eRibbonTabColor RibbonTabColorTable
        {
            get { return m_RibbonTabColorTable; }
            set { m_RibbonTabColorTable = value; }
        }

        /// <summary>
        /// Gets or sets the name of RibbonTabItem object that already exists on Ribbon control into which the RibbonBar controls are merged.
        /// If name is not specified new RibbonTabItem is created and RibbonBar controls are added to it.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Data"), Description("Indicates the name of RibbonTabItem object that already exists on Ribbon control into which the RibbonBar controls are merged.")]
        public string MergeIntoRibbonTabItemName
        {
            get { return m_MergeIntoRibbonTabItemName; }
            set { m_MergeIntoRibbonTabItemName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the RibbonTabItemGroup the new Ribbon Tab Item that is created will be added to. The RibbonTabItemGroup
        /// must be created and added to RibbonControl.TabGroups collection.
        /// </summary>
        [Browsable(true), DefaultValue(""), Category("Data"), Description("Indicates the name of the RibbonTabItemGroup the new Ribbon Tab Item that is created will be added to.")]
        public string MergeRibbonGroupName
        {
            get { return m_MergeRibbonGroupName; }
            set { m_MergeRibbonGroupName = value; }
        }

        /// <summary>
        /// Gets or sets the insertion index for the ribbon tab item that is created when ribbon bars are merged into the ribbon control.
        /// Default value is -1 which means that ribbon tab item is appended to the existing ribbon tab items.
        /// </summary>
        [Browsable(true), DefaultValue(-1), Category("Data"), Description("Indicates the insertion index for the ribbon tab item that is created when ribbon bars are merged into the ribbon control.")]
        public int MergeRibbonTabItemIndex
        {
            get { return m_MergeRibbonTabItemIndex; }
            set { m_MergeRibbonTabItemIndex = value; }
        }

        /// <summary>
        /// Raises the BeforeRibbonMerge event.
        /// </summary>
        protected virtual void OnBeforeRibbonMerge(EventArgs e)
        {
            if (BeforeRibbonMerge != null)
                BeforeRibbonMerge(this, e);
        }

        /// <summary>
        /// Raises the AfterRibbonMerge event.
        /// </summary>
        protected virtual void OnAfterRibbonMerge(EventArgs e)
        {
            if (AfterRibbonMerge != null)
                AfterRibbonMerge(this, e);
        }

        /// <summary>
        /// Raises the BeforeRibbonUnmerge event.
        /// </summary>
        protected virtual void OnBeforeRibbonUnmerge(EventArgs e)
        {
            if (BeforeRibbonUnmerge != null)
                BeforeRibbonUnmerge(this, e);
        }

        /// <summary>
        /// Raises the AfterRibbonUnmerge event.
        /// </summary>
        protected virtual void OnAfterRibbonUnmerge(EventArgs e)
        {
            if (AfterRibbonUnmerge != null)
                AfterRibbonUnmerge(this, e);
        }
        #endregion
    }
}
