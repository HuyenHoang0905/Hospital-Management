using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the command link for a text box item
    /// </summary>
    public class CommandLinkTextBoxItem : CommandLink
    {
        #region Events
        /// <summary>
        /// Occurs when text in text box item has changed.
        /// </summary>
        public event EventHandler InputTextChanged;
        #endregion

        /// <summary>
        /// Gets reference to the TextBoxItem this CommandLink is linked to. Note that this is the first TextBoxItem object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public TextBoxItem Item
        {
            get
            {
                return this.GetItem(typeof(TextBoxItem)) as TextBoxItem;
            }
        }

        protected override void ConnectManager()
        {
            base.ConnectManager();

            if (this.Manager != null)
            {
                this.Manager.TextBoxItemTextChanged += new EventHandler(this.ItemInputTextChanged);
            }
        }

        protected override void DisconnectManager()
        {
            base.DisconnectManager();
            if(this.Manager!=null)
                this.Manager.TextBoxItemTextChanged -= new EventHandler(this.ItemInputTextChanged);
        }

        private void ItemInputTextChanged(object sender, EventArgs e)
        {
            if (InputTextChanged != null)
                InputTextChanged(sender, e);
        }
    }
}
