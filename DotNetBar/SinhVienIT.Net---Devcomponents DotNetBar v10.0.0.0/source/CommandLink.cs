using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the base class that provides link between DotNetBar BaseItem commands and typed code representation for them.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), DefaultEvent("Click")]
    public class CommandLink : Component
    {
        #region Private Variables
        private string m_Name = "";
        private DotNetBarManager m_Manager = null;
        private static string CommandPrefix = "cmd";
        #endregion

        #region Events
        /// <summary>
        /// Occurs when item connected to the command link is clicked.
        /// </summary>
        public event EventHandler Click;
        #endregion

        /// <summary>
        /// Returns name of the item that can be used to identify item from the code. You should not set this property directly since it is populated by DotNetBar designer.
        /// </summary>
        [Browsable(false), Category("Design"), Description("Indicates the name used to identify item command link is connected to.")]
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

        protected virtual BaseItem GetItem(Type expectedItemType)
        {
            if (this.Manager == null)
                throw new InvalidOperationException("Manager property is not assigned to the instance of DotNetBarManager this CommandLink is linking to.");

            string itemName = GetItemName(this.Name);
            if (itemName == "")
            {
                throw new InvalidOperationException("Command link name is not in expected format. Could not retrive item.");
            }

            BaseItem item = this.Manager.GetItem(itemName, true);

            if (item == null)
                throw new InvalidOperationException("Item '" + itemName + "' cannot be found. DotNetBarManager definition might not be loaded. Try moving your code to Load event.");

            if (item.GetType() != expectedItemType)
                throw new InvalidOperationException("This CommandLink type can only by connected to " + expectedItemType.Name + " type. Currently link is connecting to " + item.GetType().ToString());

            return item;
        }

        /// <summary>
        /// Disconnects the CommandLink object to the DotNetBarManager.
        /// </summary>
        protected virtual void DisconnectManager()
        {
            if (m_Manager == null)
                return;
            m_Manager.ItemClick -= new EventHandler(ItemClick);
            m_Manager.CommandLinks.Remove(this);
        }

        /// <summary>
        /// Connects the CommandLink object to the DotNetBarManager.
        /// </summary>
        protected virtual void ConnectManager()
        {
            if (m_Manager == null)
                return;
            m_Manager.ItemClick += new EventHandler(ItemClick);
            m_Manager.CommandLinks.Add(this);
        }

        private void ItemClick(object sender, EventArgs e)
        {
			if(sender is BaseItem)
			{
				BaseItem item = sender as BaseItem;
				if(item.Name == GetItemName(this.Name))
				{
					if (Click != null)
						Click(sender, e);
				}
			}
        }

        /// <summary>
        /// Gets or sets the instance of DotNetBarManager associated with the command link.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public DotNetBarManager Manager
        {
            get { return m_Manager; }
            set
            {
                if (m_Manager != null)
                    DisconnectManager();
                m_Manager = value;

                if (m_Manager != null)
                    ConnectManager();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.Manager = null;
            base.Dispose(disposing);
        }

        internal static string GetCommandLinkName(string itemName)
        {
            return CommandPrefix + itemName;
        }

        internal static string GetItemName(string commandLinkName)
        {
            if (commandLinkName.StartsWith(CommandPrefix))
                return commandLinkName.Substring(CommandPrefix.Length);

            return "";
        }
    }
}
