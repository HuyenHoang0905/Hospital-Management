using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the typed command link for ColorPickerDropDown type.
    /// </summary>
    public class CommandLinkColorPickerDropDown : CommandLink
    {
        #region Events
        /// <summary>
        /// Occurs when color is choosen from drop-down color picker or from Custom Colors dialog box. Selected color can be accessed through SelectedColor property.
        /// </summary>
        [Description("Occurs when color is choosen from drop-down color picker or from Custom Colors dialog box.")]
        public event EventHandler SelectedColorChanged;
        #endregion

        /// <summary>
        /// Gets reference to the ColorPickerDropDown this CommandLink is linked to. Note that this is the first ColorPickerDropDown object found by DotNetBarManager.
        /// It is possible that multiple buttons have same name see Global Items under DotNetBar Fundamentals in help file for more details.
        /// </summary>
        [Browsable(false)]
        public ColorPickerDropDown Item
        {
            get
            {
                return this.GetItem(typeof(ColorPickerDropDown)) as ColorPickerDropDown;
            }
        }

        protected override void ConnectManager()
        {
            base.ConnectManager();
            if(this.Manager!=null)
                this.Manager.ColorPickerSelectedColorChanged += new EventHandler(ManagerColorPickerSelectedColorChanged);
        }

        private void ManagerColorPickerSelectedColorChanged(object sender, EventArgs e)
        {
            if (SelectedColorChanged != null)
                SelectedColorChanged(sender, e);
        }

        protected override void DisconnectManager()
        {
            base.DisconnectManager();
            if(this.Manager!=null)
                this.Manager.ColorPickerSelectedColorChanged -= new EventHandler(ManagerColorPickerSelectedColorChanged);
        }
    }
}
