using System;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents compact tree bread-crumb control.
    /// </summary>
    [ToolboxBitmap(typeof(CrumbBar), "CrumbBar.CrumbBar.ico"), ToolboxItem(true), Designer("DevComponents.DotNetBar.Design.CrumbBarDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false), DefaultEvent("SelectedItemChanged")]
    public class CrumbBar : ItemControl
    {
        #region Private Variables
        private CrumbBarItemsCollection _Items = null;
        private CrumbBarViewContainer _ViewContainer = null;
        private Office2007Renderer _VistaRenderer = null;
        #endregion

        #region Events
        /// <summary>
        /// Occurs before SelectedItem has changed and provides opportunity to cancel the change. Set Cancel property on event arguments to true to cancel the change.
        /// </summary>
        [Description("Occurs before SelectedItem has changed and provides opportunity to cancel the change.")]
        public event CrumBarSelectionEventHandler SelectedItemChanging;
        /// <summary>
        /// Occurs after SelectedItem has changed. The change of the selected item at this point cannot be canceled. For that use SelectedItemChanging event.
        /// </summary>
        public event CrumBarSelectionEventHandler SelectedItemChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the CrumbBar class.
        /// </summary>
        public CrumbBar()
            : base()
        {
            _Items = new CrumbBarItemsCollection(this);

            _ViewContainer = new CrumbBarViewContainer();
            _ViewContainer.GlobalItem = false;
            _ViewContainer.ContainerControl = this;
            _ViewContainer.Displayed = true;
            _ViewContainer.Style = eDotNetBarStyle.Office2007;
            this.ColorScheme.Style = eDotNetBarStyle.Office2007;
            _ViewContainer.SetOwner(this);
            InitializeVistaRenderer();
            this.SetBaseItemContainer(_ViewContainer);
        }
        #endregion

        #region Internal Implementation
        private CrumbBarItem FindCrumbBarItem(IList subitems, string name)
        {
            foreach (CrumbBarItem item in subitems)
            {
                if (item.Name == Name)
                {
                    return item;
                }
                CrumbBarItem childItem = FindCrumbBarItem(item.SubItems, name);
                if ((childItem != null)) return childItem;
            }
            return null;
        }
        /// <summary>
        /// Finds CrumbBarItem with specified name.
        /// </summary>
        /// <param name="name">Name of item to look for</param>
        /// <returns>Item or null if no item was found.</returns>
        public CrumbBarItem FindByName(string name)
        {
            return FindCrumbBarItem(this.Items, name);
        }


        private CrumbBarItem _SelectedItem;
        /// <summary>
        /// Gets or sets currently selected item.
        /// </summary>
        [DefaultValue(null), Category("Appearance"), Description("Indicates currently selected item")]
        public CrumbBarItem SelectedItem
        {
            get { return _SelectedItem; }
            set 
            {
                SetSelectedItem(value, eEventSource.Code);
            }
        }

        internal void OnItemsCleared()
        {
            this.SelectedItem = null;
        }
        /// <summary>
        /// Sets the currently selected item in the control.
        /// </summary>
        /// <param name="selection">Reference to selected item.</param>
        /// <param name="source">Source of the event.</param>
        public void SetSelectedItem(CrumbBarItem selection, eEventSource source)
        {
            bool raiseChangedEvents = selection != _SelectedItem;

            if (raiseChangedEvents)
            {
                CrumbBarSelectionEventArgs eventArgs = new CrumbBarSelectionEventArgs(selection);
                OnSelectedItemChanging(eventArgs);
                if (eventArgs.Cancel) return;
                selection = eventArgs.NewSelectedItem;
            }

            if (_SelectedItem != null)
                _SelectedItem.IsSelected = false;

            ArrayList newItems = new ArrayList();
            if (selection == null)
                selection = GetFirstVisibleItem();
            _ViewContainer.Expanded = false; // closes any open popups
            _ViewContainer.RestoreOverflowItems();

            if (selection != null)
            {
                CrumbBarItem current = selection;
                while (current != null)
                {
                    newItems.Insert(0, GetItemView(current, true));
                    current = current.Parent as CrumbBarItem;
                }

                UpdateSelectedItemImage(selection);
            }
            else
                UpdateSelectedItemImage(null);

            // Remove current view items
            _ViewContainer.ClearViewItems();
            if (selection != null)
            {
                _ViewContainer.SubItems.AddRange((BaseItem[])newItems.ToArray(typeof(BaseItem)));
            }
            _ViewContainer.NeedRecalcSize = true;

            _SelectedItem = selection;
            if (_SelectedItem != null)
                _SelectedItem.IsSelected = true;

            this.RecalcLayout();

            if (raiseChangedEvents)
            {
                CrumbBarSelectionEventArgs eventArgs = new CrumbBarSelectionEventArgs(selection);
                OnSelectedItemChanged(eventArgs);
            }
        }

        internal void RefreshView()
        {
            UpdateSelectedItemImage(_SelectedItem);
        }

        private void UpdateSelectedItemImage(CrumbBarItem selection)
        {
            if (selection == null)
            {
                _ViewContainer.ImageLabel.Visible = false;
                return;
            }

            CompositeImage image = selection.GetImage();
            if (image != null)
            {
                _ViewContainer.ImageLabel.Visible = true;
                if (image.IsIcon)
                    _ViewContainer.ImageLabel.Icon = image.Icon;
                else
                    _ViewContainer.ImageLabel.Image = image.Image;
            }
            else
            {
                _ViewContainer.ImageLabel.Visible = false;
            }            
        }

        /// <summary>
        /// Shows the selected item popup menu if it has menu items.
        /// </summary>
        /// <returns>true if popup was shown otherwise false</returns>
        public bool ShowSelectedItemPopupMenu()
        {
            CrumbBarItem selectedItem = this.SelectedItem;
            if (selectedItem == null) throw new NullReferenceException("SelectedItem is null");
            CrumbBarItemView view = GetItemView(selectedItem, false) as CrumbBarItemView;
            if (view != null && !view.Expanded && view.AttachedItem != null && view.AttachedItem.SubItems.Count > 0)
            {
                view.Expanded = true;
                return true;
            }
            return false;
        }

        private object GetItemView(CrumbBarItem current, bool canCreateNewView)
        {
            for (int i = 2; i < _ViewContainer.SubItems.Count; i++)
            {
                CrumbBarItemView view = _ViewContainer.SubItems[i] as CrumbBarItemView;
                if (view != null && view.AttachedItem == current) return view;
            }
            if(canCreateNewView)
                return CrumbBarItemView.CreateViewForItem(current);
            return null;
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CrumbBarItem GetFirstVisibleItem()
        {
            foreach (CrumbBarItem crumbBarItem in Items)
            {
                if (crumbBarItem.Visible) return crumbBarItem;
            }
            return null;
        }

        /// <summary>
        /// Gets whether an item is in selected path to the currently selected item as either one of the parents of selected item
        /// or selected item itself.
        /// </summary>
        /// <param name="item">Item to test.</param>
        /// <returns>true if item is in selected path otherwise false.</returns>
        public bool GetIsInSelectedPath(CrumbBarItem item)
        {
            for (int i = 2; i < _ViewContainer.SubItems.Count; i++)
            {
                CrumbBarItemView view = _ViewContainer.SubItems[i] as CrumbBarItemView;
                if (view != null && view.AttachedItem == item) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets collection of items assigned to the control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor("DevComponents.DotNetBar.Design.CrumbBarItemCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
#if FRAMEWORK20
        [Browsable(false)]
#else
        [Browsable(true)]
#endif
        public CrumbBarItemsCollection Items
        {
            get { return _Items; }
        }

        private eCrumbBarStyle _Style = eCrumbBarStyle.Vista;
        /// <summary>
        /// Gets or sets the visual style of the control. Default value is Windows Vista style.
        /// </summary>
        [DefaultValue(eCrumbBarStyle.Vista), Category("Appearance"), Description("Indicates visual style of the control.")]
        public eCrumbBarStyle Style
        {
            get { return _Style; }
            set
            {
                if (value != _Style)
                {
                    eCrumbBarStyle oldValue = _Style;
                    _Style = value;
                    OnStyleChanged(oldValue, value);
                }
            }
        }
        protected virtual void OnStyleChanged(eCrumbBarStyle oldValue, eCrumbBarStyle newValue)
        {
            this.Invalidate();
        }

        /// <summary>
        /// Gets the color table used by the Vista style renderer.
        /// </summary>
        [Browsable(false)]
        public Office2007ColorTable VistaColorTable
        {
            get { return _VistaRenderer.ColorTable; }
        }

        /// <summary>
        /// Returns the renderer control will be rendered with.
        /// </summary>
        /// <returns>The current renderer.</returns>
        public override Rendering.BaseRenderer GetRenderer()
        {
            if (this.RenderMode == eRenderMode.Global && _Style == eCrumbBarStyle.Vista)
                return _VistaRenderer;
            return base.GetRenderer();
        }

        private void InitializeVistaRenderer()
        {
            _VistaRenderer = new Office2007Renderer();
            Office2007ColorTable colorTable = _VistaRenderer.ColorTable;
            colorTable.CrumbBarItemView = GetCrumbBarVistaColorTable();
            // Popup menu style
            colorTable.Menu.Background = new LinearGradientColorTable(ColorScheme.GetColor("FFF0F0F0"));
            colorTable.Menu.Border = new LinearGradientColorTable(ColorScheme.GetColor("FF646464"));
            colorTable.Menu.Side = new LinearGradientColorTable(ColorScheme.GetColor("FFF1F1F1"));
            colorTable.Menu.SideBorder = new LinearGradientColorTable(ColorScheme.GetColor("FFE2E3E3"));
            colorTable.Menu.SideBorderLight = new LinearGradientColorTable(ColorScheme.GetColor("FFFFFFFF"));
            Office2007ButtonItemColorTable menu = colorTable.ButtonItemColors[0];
            menu.Default.Text = Color.Black;
            menu.MouseOver.Background = new LinearGradientColorTable(ColorScheme.GetColor("34C5EBFF"), ColorScheme.GetColor("7081D8FF"), 90);
            menu.MouseOver.OuterBorder = new LinearGradientColorTable(ColorScheme.GetColor("FF96DBFA"));
            menu.MouseOver.InnerBorder = new LinearGradientColorTable(ColorScheme.GetColor("A0FFFFFF"));
            menu.MouseOver.Text = Color.Black;
            menu.Pressed = menu.MouseOver;
        }

        private CrumbBarItemViewColorTable GetCrumbBarVistaColorTable()
        {
            return GetCrumbBarVistaColorTable(new ColorFactory());
        }

        internal static CrumbBarItemViewColorTable GetCrumbBarVistaColorTable(ColorFactory factory)
        {
            CrumbBarItemViewColorTable viewColorTable = new CrumbBarItemViewColorTable();
            CrumbBarItemViewStateColorTable crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            viewColorTable.Default = crumbBarViewTable;
            crumbBarViewTable.Foreground = Color.Black;
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            viewColorTable.MouseOver = crumbBarViewTable;
            crumbBarViewTable.Foreground = Color.Black;
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("EAF6FD")), 0f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("D7EFFC")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("BDE6FD")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("A6D9F4")), 1f)});
            crumbBarViewTable.Border = factory.GetColor(ColorScheme.GetColor("3C7FB1"));
            crumbBarViewTable.BorderLight = factory.GetColor(ColorScheme.GetColor("E0FFFFFF"));
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            viewColorTable.MouseOverInactive = crumbBarViewTable;
            crumbBarViewTable.Foreground = Color.Black;
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFF2F2F2")), 0f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFEAEAEA")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFDCDCDC")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFCFCFCF")), 1f)});
            crumbBarViewTable.Border = factory.GetColor(ColorScheme.GetColor("FF8E8F8F"));
            crumbBarViewTable.BorderLight = factory.GetColor(ColorScheme.GetColor("E0FFFFFF"));
            crumbBarViewTable = new CrumbBarItemViewStateColorTable();
            viewColorTable.Pressed = crumbBarViewTable;
            crumbBarViewTable.Foreground = Color.Black;
            crumbBarViewTable.Background = new BackgroundColorBlendCollection();
            crumbBarViewTable.Background.AddRange(new BackgroundColorBlend[]{
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFC2E4F6")), 0f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFC2E4F6")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FFA9D9F2")), .5f),
                new BackgroundColorBlend(factory.GetColor(ColorScheme.GetColor("FF90CBEB")), 1f)});
            crumbBarViewTable.Border = factory.GetColor(ColorScheme.GetColor("FF6E8D9F"));
            crumbBarViewTable.BorderLight = factory.GetColor(ColorScheme.GetColor("906E8D9F"));

            return viewColorTable;
        }

        /// <summary>
        /// Raises the SelectedItemChanging event. 
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnSelectedItemChanging(CrumbBarSelectionEventArgs e)
        {
            CrumBarSelectionEventHandler eh = SelectedItemChanging;
            if (eh != null) eh(this, e);
        }

        /// <summary>
        /// Raises the SelectedItemChanged event. 
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnSelectedItemChanged(CrumbBarSelectionEventArgs e)
        {
            CrumBarSelectionEventHandler eh = SelectedItemChanged;
            if (eh != null) eh(this, e);
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 22);
            }
        }

#if FRAMEWORK20

        private Size _PreferredSize = Size.Empty;
        [Localizable(true), Browsable(false)]
        public new System.Windows.Forms.Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!_PreferredSize.IsEmpty) return _PreferredSize;

            if (!BarFunctions.IsHandleValid(this))
                return base.GetPreferredSize(proposedSize);

            if (this.Items.Count == 0 || !BarFunctions.IsHandleValid(this) || _ViewContainer.SubItems.Count == 0)
                return new Size(base.GetPreferredSize(proposedSize).Width, 22);

            int height = ElementStyleLayout.VerticalStyleWhiteSpace(this.GetBackgroundStyle());
            height += _ViewContainer.CalculatedHeight > 0 ? _ViewContainer.CalculatedHeight : 20;

            _PreferredSize = new Size(proposedSize.Width, height);
            return _PreferredSize;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is automatically resized to display its entire contents. You can set MaximumSize.Width property to set the maximum width used by the control.
        /// </summary>
        [Browsable(true), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    InvalidateAutoSize();
                    AdjustSize();
                }
            }
        }

        private void InvalidateAutoSize()
        {
            _PreferredSize = Size.Empty;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSize)
            {
                Size preferredSize = base.PreferredSize;
                if (preferredSize.Width > 0)
                    width = preferredSize.Width;
                if (preferredSize.Height > 0)
                    height = preferredSize.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void AdjustSize()
        {
            if (this.AutoSize)
            {
                System.Drawing.Size prefSize = base.PreferredSize;
                if (prefSize.Width > 0 && prefSize.Height > 0)
                    this.Size = base.PreferredSize;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
#if FRAMEWORK20
            if (this.AutoSize)
                this.AdjustSize();
#endif
        }
#endif
        #endregion

        #region Property Hiding
        [Browsable(false)]
        public override eBarImageSize ImageSize
        {
            get
            {
                return base.ImageSize;
            }
            set
            {
                base.ImageSize = value;
            }
        }
        [Browsable(false)]
        public override System.Windows.Forms.ImageList ImagesLarge
        {
            get
            {
                return base.ImagesLarge;
            }
            set
            {
                base.ImagesLarge = value;
            }
        }
        [Browsable(false)]
        public override System.Windows.Forms.ImageList ImagesMedium
        {
            get
            {
                return base.ImagesMedium;
            }
            set
            {
                base.ImagesMedium = value;
            }
        }
        [Browsable(false)]
        public override Font KeyTipsFont
        {
            get
            {
                return base.KeyTipsFont;
            }
            set
            {
                base.KeyTipsFont = value;
            }
        }
        [Browsable(false)]
        public override bool ShowShortcutKeysInToolTips
        {
            get
            {
                return base.ShowShortcutKeysInToolTips;
            }
            set
            {
                base.ShowShortcutKeysInToolTips = value;
            }
        }
        [Browsable(false)]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        #endregion

        #region Licensing
#if !TRIAL
        private string m_LicenseKey = "";
        [Browsable(false), DefaultValue("")]
        public string LicenseKey
        {
            get { return m_LicenseKey; }
            set
            {
                if (NativeFunctions.ValidateLicenseKey(value))
                    return;
                m_LicenseKey = (!NativeFunctions.CheckLicenseKey(value) ? "9dsjkhds7" : value);
            }
        }
#endif
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
#if !TRIAL
            if (NativeFunctions.keyValidated2 != 266)
                TextDrawing.DrawString(e.Graphics, "Invalid License", this.Font, Color.FromArgb(180, Color.Red), this.ClientRectangle, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
#else
            if (NativeFunctions.ColorExpAlt() || !NativeFunctions.CheckedThrough)
		    {
			    e.Graphics.Clear(SystemColors.Control);
                return;
            }
#endif
        }
        #endregion
    }

    #region Event support
    /// <summary>
    /// Defines delegate for CrumbBar selection events.
    /// </summary>
    public delegate void CrumBarSelectionEventHandler(object sender, CrumbBarSelectionEventArgs e);
    /// <summary>
    /// Provides data for CrumbBar selection events.
    /// </summary>
    public class CrumbBarSelectionEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets or sets newly selected item.
        /// </summary>
        public CrumbBarItem NewSelectedItem = null;
        /// <summary>
        /// Initializes a new instance of the CrumbBarSelectionEventArgs class.
        /// </summary>
        /// <param name="newSelectedItem"></param>
        public CrumbBarSelectionEventArgs(CrumbBarItem newSelectedItem)
        {
            NewSelectedItem = newSelectedItem;
        }
    }
    #endregion
}
