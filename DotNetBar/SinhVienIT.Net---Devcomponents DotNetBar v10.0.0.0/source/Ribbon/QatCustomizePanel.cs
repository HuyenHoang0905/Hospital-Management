using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Ribbon
{
    /// <summary>
    /// Represents the Quick Access Toolbar customization panel which can be used on the custom QAT customization dialogs
    /// so customization of Quick Access Toolbar can be reused.
    /// </summary>
    public class QatCustomizePanel : UserControl
    {
        #region Private/Public variables
        /// <summary>
        /// Gets reference to the internal ItemPanel control that displays the commands for selected category.
        /// </summary>
        public ItemPanel itemPanelCommands;
        /// <summary>
        /// Gets reference to the ItemPanel control that displays the Quick Access Toolbar Items.
        /// </summary>
        public ItemPanel itemPanelQat;
        /// <summary>
        /// Gets reference to the button that perform addition of commands to the Quick Access Toolbar.
        /// </summary>
        public ButtonX buttonAddToQat;
        /// <summary>
        /// Gets reference to the button that perform removal of commands from the Quick Access Toolbar.
        /// </summary>
        public ButtonX buttonRemoveFromQat;
        /// <summary>
        /// Gets reference to the combo box control that holds all categories.
        /// </summary>
        public ComboBoxEx comboCategories;
        /// <summary>
        /// Gets reference to the combo box categories label control.
        /// </summary>
        public Label labelCategories;
        /// <summary>
        /// Gets reference to the check box that changes the placement of the Quick Access Toolbar.
        /// </summary>
        public Controls.CheckBoxX checkQatBelowRibbon;

        private Hashtable m_Categories = new Hashtable();
        private bool m_DataChanged = false;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #endregion

        #region Constructor

        public QatCustomizePanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();
        }
        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.itemPanelCommands = new DevComponents.DotNetBar.ItemPanel();
            this.itemPanelQat = new DevComponents.DotNetBar.ItemPanel();
            this.buttonAddToQat = new DevComponents.DotNetBar.ButtonX();
            this.buttonRemoveFromQat = new DevComponents.DotNetBar.ButtonX();
            this.comboCategories = new Controls.ComboBoxEx();
            this.labelCategories = new System.Windows.Forms.Label();
            this.checkQatBelowRibbon = new Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // itemPanelCommands
            // 
            this.itemPanelCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.itemPanelCommands.AutoScroll = true;
            this.itemPanelCommands.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanelCommands.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelCommands.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanelCommands.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanelCommands.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelCommands.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanelCommands.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelCommands.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanelCommands.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelCommands.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanelCommands.BackgroundStyle.PaddingBottom = 1;
            this.itemPanelCommands.BackgroundStyle.PaddingLeft = 1;
            this.itemPanelCommands.BackgroundStyle.PaddingRight = 1;
            this.itemPanelCommands.BackgroundStyle.PaddingTop = 1;
            this.itemPanelCommands.FadeEffect = false;
            this.itemPanelCommands.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
#if!TRIAL
            this.itemPanelCommands.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
            this.itemPanelCommands.Location = new System.Drawing.Point(9, 46);
            this.itemPanelCommands.Name = "itemPanelCommands";
            this.itemPanelCommands.Size = new System.Drawing.Size(173, 257);
            this.itemPanelCommands.TabIndex = 2;
            this.itemPanelCommands.Text = "itemPanelCommands";
            this.itemPanelCommands.KeyUp += new KeyEventHandler(ItemPanelKeyUp);
            // 
            // itemPanelQat
            // 
            this.itemPanelQat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.itemPanelQat.AutoScroll = true;
            this.itemPanelQat.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanelQat.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelQat.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanelQat.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanelQat.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelQat.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanelQat.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelQat.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanelQat.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanelQat.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanelQat.BackgroundStyle.PaddingBottom = 1;
            this.itemPanelQat.BackgroundStyle.PaddingLeft = 1;
            this.itemPanelQat.BackgroundStyle.PaddingRight = 1;
            this.itemPanelQat.BackgroundStyle.PaddingTop = 1;
            this.itemPanelQat.EnableDragDrop = true;
            this.itemPanelQat.FadeEffect = false;
            this.itemPanelQat.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            #if!TRIAL
            this.itemPanelQat.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            #endif
            this.itemPanelQat.Location = new System.Drawing.Point(266, 46);
            this.itemPanelQat.Name = "itemPanelQat";
            this.itemPanelQat.Size = new System.Drawing.Size(173, 257);
            this.itemPanelQat.TabIndex = 5;
            this.itemPanelQat.Text = "itemPanelCommands";
            this.itemPanelQat.DragDrop += new System.Windows.Forms.DragEventHandler(this.itemPanelQat_DragDrop);
            this.itemPanelQat.UserCustomize += new EventHandler(this.itemPanelQat_UserCustomize);
            this.itemPanelQat.KeyUp += new KeyEventHandler(ItemPanelKeyUp);
            // 
            // buttonAddToQat
            // 
            this.buttonAddToQat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddToQat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddToQat.Location = new System.Drawing.Point(188, 126);
            this.buttonAddToQat.Name = "buttonAddToQat";
            this.buttonAddToQat.Size = new System.Drawing.Size(73, 21);
            this.buttonAddToQat.TabIndex = 3;
            this.buttonAddToQat.Text = "&Add >>";
            this.buttonAddToQat.Click += new System.EventHandler(this.buttonAddToQat_Click);
            // 
            // buttonRemoveFromQat
            // 
            this.buttonRemoveFromQat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveFromQat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonRemoveFromQat.Location = new System.Drawing.Point(188, 153);
            this.buttonRemoveFromQat.Name = "buttonRemoveFromQat";
            this.buttonRemoveFromQat.Size = new System.Drawing.Size(73, 21);
            this.buttonRemoveFromQat.TabIndex = 4;
            this.buttonRemoveFromQat.Text = "&Remove";
            this.buttonRemoveFromQat.Click += new System.EventHandler(this.buttonRemoveFromQat_Click);
            // 
            // comboCategories
            // 
            this.comboCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCategories.Location = new System.Drawing.Point(9, 19);
            this.comboCategories.Name = "comboCategories";
            this.comboCategories.Size = new System.Drawing.Size(173, 21);
            this.comboCategories.Sorted = true;
            this.comboCategories.TabIndex = 1;
            this.comboCategories.SelectedIndexChanged += new System.EventHandler(this.comboCategories_SelectedIndexChanged);
            this.comboCategories.Style = eDotNetBarStyle.Office2007;
            this.comboCategories.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboCategories.ThemeAware = false;
            // 
            // labelCategories
            // 
            this.labelCategories.AutoSize = true;
            this.labelCategories.BackColor = System.Drawing.Color.Transparent;
            this.labelCategories.Location = new System.Drawing.Point(6, 3);
            this.labelCategories.Name = "labelCategories";
            this.labelCategories.Size = new System.Drawing.Size(123, 13);
            this.labelCategories.TabIndex = 0;
            this.labelCategories.Text = "&Choose commands from:";
            // 
            // checkQatBelowRibbon
            // 
            this.checkQatBelowRibbon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkQatBelowRibbon.BackColor = System.Drawing.Color.Transparent;
            this.checkQatBelowRibbon.Location = new System.Drawing.Point(9, 311);
            this.checkQatBelowRibbon.Name = "checkQatBelowRibbon";
            this.checkQatBelowRibbon.Size = new System.Drawing.Size(420, 17);
            this.checkQatBelowRibbon.TabIndex = 6;
            this.checkQatBelowRibbon.Text = "&Place Quick Access Toolbar below the Ribbon";
            this.checkQatBelowRibbon.CheckedChangedEx += new Controls.CheckBoxXChangeEventHandler(this.checkQatBelowRibbon_CheckedChanged);
            // 
            // QatCustomizePanel
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.comboCategories);
            this.Controls.Add(this.checkQatBelowRibbon);
            this.Controls.Add(this.labelCategories);
            this.Controls.Add(this.buttonRemoveFromQat);
            this.Controls.Add(this.buttonAddToQat);
            this.Controls.Add(this.itemPanelQat);
            this.Controls.Add(this.itemPanelCommands);
            this.Name = "QatCustomizePanel";
            this.Size = new System.Drawing.Size(444, 334);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ItemPanelKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                ItemPanel panel = sender as ItemPanel;
                foreach (BaseItem item in panel.Items)
                {
                    ButtonItem button = item as ButtonItem;
                    if (button != null && button.IsMouseOver && !button.Checked)
                    {
                        button.Checked = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Internal Implementation
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.BackColor== Color.Transparent || this.BackColor.A<255)
            {
                base.OnPaintBackground(e);
            }

            if (this.BackColor == Color.Transparent)
                return;

            DisplayHelp.FillRectangle(e.Graphics, this.ClientRectangle, this.BackColor);
        }

        /// <summary>
        /// Loads the items for the customization into the ribbon control. All Ribbon Bars on the ribbon are enumerated and items
        /// are added if they have CanCustomize=true.
        /// </summary>
        /// <param name="rc">Ribbon control to enumerate.</param>
        public void LoadItems(RibbonControl rc)
        {
            m_Categories.Clear();
            itemPanelCommands.Items.Clear();
            itemPanelQat.Items.Clear();

            LoadQatItems(rc);

            if (rc.CategorizeMode == eCategorizeMode.Categories)
                LoadByCategory(rc);
            else
                LoadByRibbonBar(rc);

            LoadCategories();

            checkQatBelowRibbon.Checked = rc.QatPositionedBelowRibbon;
            checkQatBelowRibbon.Visible = rc.EnableQatPlacement;
            m_DataChanged = false;
        }
        #region By Category Loading
        private void LoadByCategory(RibbonControl rc)
        {
            // Loop through all RibbonBars and add the items
            foreach (Control panel in rc.Controls)
            {
                LoadByCategory(panel);
            }

            BaseItem startButton = rc.GetApplicationButton();
            if (startButton!=null)
            {
                LoadByCategory(startButton);
            }
        }

        private void LoadByCategory(Control panel)
        {
            if (panel is RibbonBar)
                LoadByCategory(panel as RibbonBar);
            foreach (Control c in panel.Controls)
                LoadByCategory(c);
        }

        private void LoadByCategory(RibbonBar bar)
        {
            foreach (BaseItem item in bar.Items)
                LoadByCategory(item);
        }

        private void LoadByCategory(BaseItem item)
        {
            if (CanCustomizeItem(item))
            {
                string cat = GetCategory(item);
                ArrayList items = m_Categories[cat] as ArrayList;
                if (items == null)
                {
                    items = new ArrayList();
                    m_Categories.Add(cat, items);
                }
                items.Add(GetCustomizeRepresentation(item));
            }

            foreach (BaseItem child in item.SubItems)
                LoadByCategory(child);

            if (item is GalleryContainer)
            {
                GalleryContainer gallery = item as GalleryContainer;
                foreach (BaseItem child in gallery.PopupGalleryItems)
                    LoadByCategory(child);
            }
        }

        private string GetCategory(BaseItem item)
        {
            if (item.Category != "")
                return item.Category;
            return "Unassigned";
        }
        #endregion

        #region Load by Ribbon Bar
        private void LoadByRibbonBar(RibbonControl rc)
        {
            // Loop through all RibbonBars and add the items
            foreach (Control panel in rc.Controls)
            {
                LoadByRibbonBar(panel);
            }

            BaseItem startButton = rc.GetApplicationButton();
            if (startButton!=null)
            {
                LoadByRibbonBar(startButton, GetRibbonBarCategory(startButton.Text));
            }
        }
        private void LoadByRibbonBar(Control panel)
        {
            if (panel is RibbonBar)
                LoadByRibbonBar(panel as RibbonBar);
            foreach (Control c in panel.Controls)
                LoadByRibbonBar(c);
        }

        private void LoadByRibbonBar(RibbonBar bar)
        {
            foreach (BaseItem item in bar.Items)
                LoadByRibbonBar(item, GetRibbonBarCategory(bar.Text));
        }

        private string GetRibbonBarCategory(string barText)
        {
            StringBuilder ret = new StringBuilder(barText.Length);
            for (int i = 0; i < barText.Length; i++)
            {
                if (barText[i] == '&')
                {
                    if (i + 1 < barText.Length && barText[i + 1] == '&')
                        ret.Append(barText[i]);
                    else
                        continue;
                }
                else
                    ret.Append(barText[i]);
            }

            return ret.ToString();
        }

        private void LoadByRibbonBar(BaseItem item, string category)
        {
            if (CanCustomizeItem(item))
            {
                ArrayList items = m_Categories[category] as ArrayList;
                if (items == null)
                {
                    items = new ArrayList();
                    m_Categories.Add(category, items);
                }
                items.Add(GetCustomizeRepresentation(item));
            }

            foreach (BaseItem child in item.SubItems)
                LoadByRibbonBar(child, category);

            if (item is GalleryContainer)
            {
                GalleryContainer gallery = item as GalleryContainer;
                foreach (BaseItem child in gallery.PopupGalleryItems)
                    LoadByRibbonBar(child, category);
            }
        }
        #endregion

        private void LoadQatItems(RibbonControl rc)
        {
            int count = rc.QuickToolbarItems.Count;
            int start = 0;

            BaseItem startButton = rc.GetApplicationButton();
            if (startButton != null)
                start = rc.QuickToolbarItems.IndexOf(startButton) + 1;

            for (int i = start; i < count; i++)
            {
                BaseItem item = rc.QuickToolbarItems[i];
                if (IsSystemItem(item))
                    continue;
                BaseItem custItem = GetCustomizeRepresentation(item);
                custItem.Tag = null;
                if (!item.CanCustomize) custItem.Visible = false;
                itemPanelQat.Items.Add(custItem);
            }
        }

        private bool IsSystemItem(BaseItem item)
        {
            if (item.SystemItem || item is ItemContainer || item is CustomizeItem || item is SystemCaptionItem)
                return true;
            return false;
        }

        private ButtonItem GetCustomizeRepresentation(BaseItem item)
        {
            ButtonItem ret = new ButtonItem(item.Name);
            ret.Text = item.Text;
            ret.ButtonStyle = eButtonStyle.ImageAndText;
            ret.ImagePaddingVertical = 4;
            ret.OptionGroup = "sys";
            ret.GlobalItem = false;
            if (ret.Text == "")
                ret.Text = "Unassigned";
            ret.Tag = item;

            if (item is ButtonItem)
            {
                ButtonItem b = item as ButtonItem;
                if (b.ImageSmall != null)
                {
                    ret.Image = b.ImageSmall;
                    if (b.ImageSmall.Width != 16 || b.ImageSmall.Height != 16)
                        ret.ImageFixedSize = new Size(16, 16);
                }
                else
                {
                    CompositeImage image = b.GetImage(ImageState.Default);
                    if (image != null)
                    {
                        if (image.IsIcon)
                            ret.Icon = (Icon)image.Icon.Clone();
                        else
                            ret.Image = image.Image;
                        ret.ImageFixedSize = new Size(16, 16);
                    }
                }
            }
            else if (item is ComboBoxItem || item is TextBoxItem || item is ControlContainerItem)
            {
                ret.ImagePosition = eImagePosition.Right;
                ret.Image = BarFunctions.LoadBitmap("SystemImages.QatCustomizeItemCombo.png");
            }

            return ret;
        }

        private bool CanCustomizeItem(BaseItem item)
        {
            if (item == null)
                return false;

            if (!item.CanCustomize || item.SystemItem || item.Name=="tempColorPickerItem")
                return false;

            if (item is ItemContainer || item is GenericItemContainer)
                return false;

            return true;
        }

        private void LoadCategories()
        {
            comboCategories.Items.Clear();
            foreach (string key in m_Categories.Keys)
                comboCategories.Items.Add(key);
            if (comboCategories.Items.Count > 0)
                comboCategories.SelectedIndex = 0;
        }

        private void comboCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCategories.SelectedIndex < 0)
            {
                itemPanelCommands.Items.Clear();
                itemPanelCommands.RecalcLayout();
                return;
            }

            string key = comboCategories.Items[comboCategories.SelectedIndex].ToString();
            ArrayList items = m_Categories[key] as ArrayList;

            itemPanelCommands.Items.Clear();
            foreach (BaseItem item in items)
                itemPanelCommands.Items.Add(item);
            itemPanelCommands.RecalcLayout();
        }

        private void buttonAddToQat_Click(object sender, EventArgs e)
        {
            ButtonItem buttonChecked = itemPanelCommands.GetChecked();
            if (buttonChecked == null)
                return;
            if (itemPanelQat.Items.Contains(buttonChecked.Name))
                return;

            ButtonItem copy = buttonChecked.Copy() as ButtonItem;
            copy.Checked = false;
            itemPanelQat.Items.Add(copy);
            copy.Checked = true;
            itemPanelQat.RecalcLayout();
            m_DataChanged = true;

            int index = itemPanelCommands.Items.IndexOf(buttonChecked) + 1;
            if (index == itemPanelCommands.Items.Count)
                return;
            buttonChecked = itemPanelCommands.Items[index] as ButtonItem;
            if (buttonChecked != null)
            {
                buttonChecked.Checked = true;
                itemPanelCommands.EnsureVisible(buttonChecked);
            }
        }

        private void buttonRemoveFromQat_Click(object sender, EventArgs e)
        {
            ButtonItem buttonChecked = itemPanelQat.GetChecked();
            if (buttonChecked == null)
                return;
            int index = itemPanelQat.Items.IndexOf(buttonChecked);
            itemPanelQat.Items.Remove(buttonChecked);
            if (index >= 0 && itemPanelQat.Items.Count > 0)
            {
                if (index + 1 >= itemPanelQat.Items.Count)
                    index = itemPanelQat.Items.Count - 1;
                buttonChecked = itemPanelQat.Items[index] as ButtonItem;
                if (buttonChecked != null)
                {
                    buttonChecked.Checked = true;
                    itemPanelQat.EnsureVisible(buttonChecked);
                }
            }
            itemPanelQat.RecalcLayout();
            m_DataChanged = true;
        }

        /// <summary>
        /// Gets or sets the value of data changed flag.
        /// </summary>
        [DefaultValue(false), Browsable(false)]
        public bool DataChanged
        {
            get { return m_DataChanged; }
            set { m_DataChanged = value; }
        }

        private void itemPanelQat_DragDrop(object sender, DragEventArgs e)
        {
            m_DataChanged = true;
        }

        private void itemPanelQat_UserCustomize(object sender, EventArgs e)
        {
            m_DataChanged = true;
        }

        private void checkQatBelowRibbon_CheckedChanged(object sender, Controls.CheckBoxXChangeEventArgs e)
        {
            m_DataChanged = true;
        }
        #endregion
    }
}
