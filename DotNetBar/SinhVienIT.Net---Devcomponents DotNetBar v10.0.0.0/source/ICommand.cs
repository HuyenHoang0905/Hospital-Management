using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    #region ICommand
    /// <summary>
    /// Defines an interface that represents the Command associated with an BaseItem instance.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command without specifying the source of the command.
        /// </summary>
        void Execute();
        /// <summary>
        /// Executes the command and specifies the source of the command.
        /// </summary>
        void Execute(ICommandSource commandSource);
        /// <summary>
        /// Executes the code associated with the command.
        /// </summary>
        event EventHandler Executed;
        /// <summary>
        /// Provides the opportunity to cancel the execution of the command. This event occurs before the Executed event.
        /// </summary>
        event CancelEventHandler PreviewExecuted;
        /// <summary>
        /// Gets or sets the text associated with the items that are using command.
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the value of Checked property if item associated with the command support it.
        /// </summary>
        bool Checked { get; set; }
        /// <summary>
        /// Gets or sets the value of Visible property if item associated with the command support it.
        /// </summary>
        bool Visible { get; set; }
        /// <summary>
        /// Gets or sets the value of Image property if item associated with the command support it.
        /// </summary>
        Image Image { get; set; }
        /// <summary>
        /// Gets or sets the value of small image (ImageSmall) property if item associated with the command support it.
        /// </summary>
        Image ImageSmall { get; set; }
        /// <summary>
        /// Gets or sets the value of Enabled property for items associated with the command.
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// Called when CommandSource is registered for the command.
        /// </summary>
        /// <param name="source">CommandSource registered.</param>
        void CommandSourceRegistered(ICommandSource source);
        /// <summary>
        /// Called when CommandSource is unregistered for the command.
        /// </summary>
        /// <param name="source">CommandSource unregistered.</param>
        void CommandSourceUnregistered(ICommandSource source);
        /// <summary>
        /// Sets an property value on the subscribers through the reflection. If subscriber does not have
        /// specified property with value type its value is not set.
        /// </summary>
        /// <param name="propertyName">Property name to set.</param>
        /// <param name="value">Property value.</param>
        void SetValue(string propertyName, object value);
    }
    #endregion

    #region ICommandSource
    /// <summary>
    /// Defines an interface for the object that knows how to invoke a command.
    /// </summary>
    public interface ICommandSource
    {
        /// <summary>
        /// Gets or sets the command that will be executed when the command source is invoked.
        /// </summary>
        ICommand Command { get;set;}
        /// <summary>
        /// Gets or sets user defined data value that can be passed to the command when it is executed.
        /// </summary>
        object CommandParameter { get;set;}
    }
    #endregion

    #region Command
    /// <summary>
    /// Defines an command that is associated with an instance of BaseItem
    /// </summary>
    [ToolboxItem(true), DesignTimeVisible(true), ToolboxBitmap(typeof(Command), "Command.ico"), DefaultEvent("Executed")]
    public class Command : Component, ICommand
    {
        #region ICommand Members
        /// <summary>
        /// Initializes a new instance of the Command class with the specified container.
        /// </summary>
        /// <param name="container">An IContainer that represents the container for the command.</param>
        public Command(IContainer container)
            : this()
        {
            container.Add(this);
        }
        /// <summary>
        /// Initializes a new instance of the Command class.
        /// </summary>
        public Command()
        {
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void Execute()
        {
            Execute(null);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void Execute(ICommandSource commandSource)
        {
            CancelEventArgs e = new CancelEventArgs();
            OnPreviewExecuted(commandSource, e);
            if (e.Cancel) return;

            OnExecuted(commandSource, new EventArgs());
        }

        /// <summary>
        /// Executes the code associated with the command when an instance of BaseItem is clicked.
        /// </summary>
        public event EventHandler Executed;
        /// <summary>
        /// Raises the Execute event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnExecuted(ICommandSource commandSource, EventArgs e)
        {
            EventHandler eh = Executed;
            if (eh != null)
                eh(commandSource, e);
        }

        /// <summary>
        /// Occurs before the Executed event and allows you to cancel the firing of Executed event.
        /// </summary>
        public event CancelEventHandler PreviewExecuted;
        /// <summary>
        /// Raises the PreviewExecuted event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnPreviewExecuted(ICommandSource commandSource, CancelEventArgs e)
        {
            CancelEventHandler eh = PreviewExecuted;
            if (eh != null)
                eh(commandSource, e);
        }

        private string _Text = null;
        private bool _TextSet = false;
        /// <summary>
        /// Gets or sets the Text that is assigned to all command sources that are using this command and have Text property.
        /// </summary>
        [Localizable(true), Description("Indicates Text that is assigned to all command sources that are using this command and have Text property.")]
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                _TextSet = true;
                OnTextChanged();
            }
        }

        /// <summary>
        /// Called when Text property is set.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            SetTextProperty();
        }

        /// <summary>
        /// Sets the Text property on all subscribers to the command Text.
        /// </summary>
        protected virtual void SetTextProperty()
        {
            ArrayList list = GetSubscribers();
            ArrayList removeList = new ArrayList(list.Count);
            string text = _Text;
            if (this.GetDesignMode())
            {
                foreach (object item in list)
                {
                    if (IsTextPropertyChanged(item, text))
                        SetPropertyValue(item, "Text", text);
                    else
                        removeList.Add(item);
                }
            }
            else
            {
                foreach (object item in list)
                {
                    if (IsTextPropertyChanged(item, text))
                        SetTextProperty(item, text);
                    else
                        removeList.Add(item);
                }
            }
            foreach (object item in removeList)
                list.Remove(item);
            RecalcLayout(list);
        }

        private bool IsTextPropertyChanged(object item, string text)
        {
            if (item is ComboBoxItem)
            {
                ComboBoxItem cb = item as ComboBoxItem;
                if (cb.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    if (cb.SelectedItem is ComboBoxItem)
                        return ((ComboBoxItem)cb.SelectedItem).Text != text;
                    else if (cb.SelectedItem != null)
                        return cb.SelectedItem.ToString() != text;
                    return cb.ComboBoxEx.Text != text;
                }
                else
                    return cb.Text != text;
            }
            else if (item is BaseItem)
                return ((BaseItem)item).Text != text;
            else if (item is TabItem)
                return ((TabItem)item).Text != text;
            else if (item is Control)
                return ((Control)item).Text != text;
            else
                return GetPropertyValue(item, "Text") != text;
            return true;
        }

        protected virtual void SetTextProperty(object item, string text)
        {
            if (item is ComboBoxItem)
            {
                ComboBoxItem cb = item as ComboBoxItem;
                if (cb.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    if (cb.ComboBoxEx.Text != text)
                    {
                        if (text == "" || text == null)
                            cb.SelectedIndex = -1;
                        else
                            cb.SelectedIndex = cb.ComboBoxEx.FindString(text);
                    }
                }
                else
                    cb.Text = text;
            }
            else if (item is BaseItem)
                ((BaseItem)item).Text = text;
            else if (item is TabItem)
                ((TabItem)item).Text = text;
            else if (item is Control)
                ((Control)item).Text = text;
            else
                SetPropertyValue(item, "Text", text);
        }

        private void RecalcLayout(ArrayList list)
        {
            if (!CommandManager.AutoUpdateLayout)
                return;
            ArrayList processedControls = new ArrayList(list.Count);
            foreach (object item in list)
            {
                if (item is BaseItem)
                {
                    Control c = ((BaseItem)item).ContainerControl as Control;
                    if (BarFunctions.IsHandleValid(c) && !processedControls.Contains(c))
                    {
                        InvokeRecalcLayout(c);
                        processedControls.Add(c);
                    }
                }
            }
        }

        private void InvokeRecalcLayout(Control control)
        {
            if (control is Bar)
                ((Bar)control).RecalcLayout();
            else if (control is RibbonBar && ((RibbonBar)control).Parent is RibbonPanel && !((RibbonPanel)((RibbonBar)control).Parent).DefaultLayout)
            {
                ((RibbonBar)control).RecalcLayout();
                ((RibbonPanel)((RibbonBar)control).Parent).PerformLayout();
            }
            else if (control is ItemControl)
                ((ItemControl)control).RecalcLayout();
            else if (control is BaseItemControl)
                ((BaseItemControl)control).RecalcLayout();
            else if (control is MenuPanel)
                ((MenuPanel)control).RecalcLayout();
            else if (control is ExplorerBar)
                ((ExplorerBar)control).RecalcLayout();
            else if (control is SideBar)
                ((SideBar)control).RecalcLayout();
        }

        private bool GetDesignMode()
        {
            if (this.Site != null) return this.Site.DesignMode;
            return false;
        }

        protected virtual void SetPropertyValue(object item, string propertyName, object value)
        {
            if (!CommandManager.UseReflection) return;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            PropertyDescriptor prop = properties.Find(propertyName, false);
            if (prop != null) prop.SetValue(item, value);
        }

        protected virtual object GetPropertyValue(object item, string propertyName)
        {
            if (!CommandManager.UseReflection) return null;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            PropertyDescriptor prop = properties.Find(propertyName, false);
            if (prop != null) return prop.GetValue(item);
            return null;
        }

        /// <summary>
        /// Sets an property value on the subscribers through the reflection. If subscriber does not have
        /// specified property with value type its value is not set.
        /// </summary>
        /// <param name="propertyName">Property name to set.</param>
        /// <param name="value">Property value.</param>
        public void SetValue(string propertyName, object value)
        {
            ArrayList list = GetSubscribers();
            Type valueType = null;
            if (value != null)
                valueType = value.GetType();
            ArrayList processedControls = new ArrayList(list.Count);

            foreach (object item in list)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                PropertyDescriptor prop = properties.Find(propertyName, false);
                if (prop != null && (valueType == null || prop.PropertyType == valueType))
                {
                    prop.SetValue(item, value);
                    processedControls.Add(item);
                }
            }

            RecalcLayout(processedControls);
        }

        private ArrayList GetSubscribers()
        {
            return CommandManager.GetSubscribers(this);
        }

        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeText()
        {
            return _TextSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetText()
        {
            _TextSet = false;
            _Text = null;
        }

        private bool _Checked = false;
        private bool _CheckedSet = false;
        /// <summary>
        /// Gets or sets the value for the Checked property that is assigned to the command subscribers using this command and have Checked property.
        /// </summary>
        [Description("Indicates value for the Checked property that is assigned to the command subscribers using this command and have Checked property.")]
        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                _CheckedSet = true;
                OnCheckedChanged();
            }
        }

        protected virtual void OnCheckedChanged()
        {
            SetCheckedProperty();
        }

        protected virtual void SetCheckedProperty()
        {
            ArrayList list = GetSubscribers();
            bool check = _Checked;
            foreach (object item in list)
            {
                SetCheckedProperty(item, check);
            }
        }

        protected virtual void SetCheckedProperty(object item, bool check)
        {
            if (item is ButtonItem)
                ((ButtonItem)item).Checked = check;
            else if (item is ButtonX)
                ((ButtonX)item).Checked = check;
            else if (item is CheckBoxItem)
                ((CheckBoxItem)item).Checked = check;
            else if (item is SwitchButtonItem)
                ((SwitchButtonItem)item).Value = check;
            else
                SetPropertyValue(item, "Checked", check);
        }
        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeChecked()
        {
            return _CheckedSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetChecked()
        {
            _CheckedSet = false;
            _Checked = false;
        }

        private bool _Visible = false;
        private bool _VisibleSet = false;
        /// <summary>
        /// Gets or sets the value for the Visible property that is assigned to the command subscribers using this command and have Visible property.
        /// </summary>
        [Description("Indicates value for the Visible property that is assigned to the command subscribers using this command and have Visible property.")]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
                _VisibleSet = true;
                OnVisibleChanged();
            }
        }
        protected virtual void OnVisibleChanged()
        {
            SetVisibleProperty();
        }
        protected virtual void SetVisibleProperty()
        {
            ArrayList list = GetSubscribers();
            bool visible = _Visible;
            foreach (object item in list)
            {
                SetVisibleProperty(item, visible);
            }
        }
        protected virtual void SetVisibleProperty(object item, bool visible)
        {
            ArrayList list = GetSubscribers();
            if (this.DesignMode)
                SetPropertyValue(item, "Visible", visible);
            else
            {
                if (item is BaseItem)
                    ((BaseItem)item).Visible = visible;
                else if (item is Control)
                    ((Control)item).Visible = visible;
                else
                    SetPropertyValue(item, "Visible", visible);
            }
            RecalcLayout(list);
        }
        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeVisible()
        {
            return _VisibleSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetVisible()
        {
            _VisibleSet = false;
            _Visible = false;
        }

        private Image _Image = null;
        private bool _ImageSet = false;
        /// <summary>
        /// Gets or sets the image that is assigned to the command subscribers using this command and have Image property.
        /// </summary>
        [Description("Indicates image that is assigned to the command subscribers using this command and have Image property."), Localizable(true)]
        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                _ImageSet = true;
                OnImageChanged();
            }
        }

        protected virtual void OnImageChanged()
        {
            SetImageProperty();
        }

        protected virtual void SetImageProperty()
        {
            ArrayList list = GetSubscribers();
            Image image = _Image;

            if (this.GetDesignMode())
            {
                foreach (object item in list)
                {
                    if (item is ButtonItem)
                    {
                        ButtonItem button = item as ButtonItem;
                        bool qatButton = false;
                        if (button.ContainerControl is RibbonStrip)
                        {
                            RibbonStrip rs = button.ContainerControl as RibbonStrip;
                            if(rs.Parent is RibbonControl && ((RibbonControl)rs.Parent).QuickToolbarItems.Contains(button))
                                qatButton = true;
                        }
                        else if (button.ContainerControl is Ribbon.QatToolbar)
                            qatButton = true;
                        if (qatButton && image != null && (image.Width>16 || image.Height>16))
                        {
                            button.UseSmallImage = true;
                            if (button.ImageSmall == null)
                                TypeDescriptor.GetProperties(button)["ImageFixedSize"].SetValue(button, new Size(16, 16));
                        }
                    }

                    SetPropertyValue(item, "Image", image);
                }
            }
            else
            {
                foreach (object item in list)
                {
                    SetImageProperty(item, image);
                }
            }
            RecalcLayout(list);
        }

        protected virtual void SetImageProperty(object item, Image image)
        {
            if (item is ButtonItem)
                ((ButtonItem)item).Image = image;
            else if (item is ExplorerBarGroupItem)
                ((ExplorerBarGroupItem)item).Image = image;
            else if (item is LabelItem)
                ((LabelItem)item).Image = image;
            else if (item is SideBarPanelItem)
                ((SideBarPanelItem)item).Image = image;
            else if (item is TabItem)
                ((TabItem)item).Image = image;
            else if (item is ButtonX)
                ((ButtonX)item).Image = image;
            else if (item is LabelX)
                ((LabelX)item).Image = image;
            else if (item is DevComponents.DotNetBar.Controls.ReflectionImage)
                ((DevComponents.DotNetBar.Controls.ReflectionImage)item).Image = image;
            else if (item is BubbleButton)
                ((BubbleButton)item).Image = image;
            else
                SetPropertyValue(item, "Image", image);
        }
        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeImage()
        {
            return _ImageSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetImage()
        {
            _ImageSet = false;
            _Image = null;
        }

        private Image _ImageSmall = null;
        private bool _ImageSmallSet = false;
        /// <summary>
        /// Gets or sets the small image that is assigned to the command subscribers using this command and have ImageSmall property.
        /// </summary>
        [Description("Indicates small image that is assigned to the command subscribers using this command and have ImageSmall property."), Localizable(true)]
        public Image ImageSmall
        {
            get
            {
                return _ImageSmall;
            }
            set
            {
                _ImageSmall = value;
                _ImageSmallSet = true;
                OnImageSmallChanged();
            }
        }

        protected virtual void OnImageSmallChanged()
        {
            SetImageSmallProperty();
        }

        protected virtual void SetImageSmallProperty()
        {
            ArrayList list = GetSubscribers();
            Image image = _ImageSmall;
            foreach (object item in list)
            {
                SetImageSmallProperty(item, image);
            }
            RecalcLayout(list);
        }

        protected virtual void SetImageSmallProperty(object item, Image image)
        {
            if (item is ButtonItem)
                ((ButtonItem)item).ImageSmall = image;
            else
                SetPropertyValue(item, "ImageSmall", image);
        }
        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeImageSmall()
        {
            return _ImageSmallSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetImageSmall()
        {
            _ImageSmallSet = false;
            _ImageSmall = null;
        }

        private bool _Enabled = true;
        private bool _EnabledSet = false;
        /// <summary>
        /// Gets or sets the value for Enabled property assigned to the command subscribers using this command and have Enabled property.
        /// </summary>
        [Description("Indicates value for Enabled property assigned to the command subscribers using this command and have Enabled property.")]
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                _EnabledSet = true;
                OnEnabledChanged();
            }
        }

        protected virtual void OnEnabledChanged()
        {
            SetEnabledProperty();
        }

        protected virtual void SetEnabledProperty()
        {
            ArrayList list = GetSubscribers();
            bool enabled = _Enabled;
            foreach (object item in list)
            {
                SetEnabledProperty(item, enabled);
            }
        }

        protected virtual void SetEnabledProperty(object item, bool enabled)
        {
            if (item is BaseItem)
                ((BaseItem)item).Enabled = enabled;
            else if (item is Control)
                ((Control)item).Enabled = enabled;
            else
                SetPropertyValue(item, "Enabled", enabled);
        }
        /// <summary>
        /// Gets whether property is set and whether it will be applied to items associated with the command.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeEnabled()
        {
            return _EnabledSet;
        }
        /// <summary>
        /// Resets the property to its default value and disables its propagation to items that are associated with command.
        /// </summary>
        public void ResetEnabled()
        {
            _EnabledSet = false;
            _Enabled = false;
        }

        /// <summary>
        /// Called when CommandSource is registered for the command.
        /// </summary>
        /// <param name="source">CommandSource registered.</param>
        public virtual void CommandSourceRegistered(ICommandSource source)
        {
            if (source == null || this.GetDesignMode()) return;

            if (_EnabledSet)
                SetEnabledProperty(source, _Enabled);
            if (_CheckedSet)
                SetCheckedProperty(source, _Checked);
            if(_ImageSet)
                SetImageProperty(source, _Image);
            if(_ImageSmallSet)
                SetImageSmallProperty(source, _ImageSmall);
            if(_TextSet)
                SetTextProperty(source, _Text);
        }

        /// <summary>
        /// Called when CommandSource is unregistered for the command.
        /// </summary>
        /// <param name="source">CommandSource unregistered.</param>
        public virtual void CommandSourceUnregistered(ICommandSource source)
        {
        }
        #endregion

        #region Component Implementation
        protected override void Dispose(bool disposing)
        {
            if(disposing)
                CommandManager.UnRegisterCommand(this);
            base.Dispose(disposing);
        }

        private string _Name = "";
        /// <summary>
        /// Returns name of the node that can be used to identify it from the code.
        /// </summary>
        [Browsable(false), Category("Design"), Description("Indicates the name used to identify node.")]
        public string Name
        {
            get
            {
                if (this.Site != null)
                    _Name = this.Site.Name;
                return _Name;
            }
            set
            {
                if (this.Site != null)
                    this.Site.Name = value;
                if (value == null)
                    _Name = "";
                else
                    _Name = value;
            }
        }
        #endregion
    }
    #endregion
}
