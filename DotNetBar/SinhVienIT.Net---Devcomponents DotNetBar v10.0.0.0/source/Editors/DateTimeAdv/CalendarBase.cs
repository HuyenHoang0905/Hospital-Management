#if FRAMEWORK20
using System;
using System.Text;
using DevComponents.DotNetBar;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Represents base class for the CalendarMonth and MultiMonthCalendar controls. This class is used internally by DotNetBar and is not intended for public use.
    /// </summary>
    public class CalendarBase : ImageItem, IDesignTimeProvider
    {
        #region Private Variables
        private ElementStyle _BackgroundStyle = new ElementStyle();
        #endregion

        #region Events
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MultiMonthCalendar class.
        /// </summary>
        public CalendarBase()
        {
            m_IsContainer = true;
            this.AutoCollapseOnClick = true;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            _BackgroundStyle.StyleChanged += BackgroundStyleChanged;
        }

        protected override void Dispose(bool disposing)
        {
            _BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
            _BackgroundStyle.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Must be overridden by class that is inheriting to provide the painting for the item.
        /// </summary>
        public override void Paint(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;
            Region oldClip = null;
            bool clipSet = false;

            PaintBackground(p);

            Rectangle clip = GetClipRectangle();
            oldClip = g.Clip;
            g.SetClip(clip, CombineMode.Intersect);
            clipSet = true;

            ItemDisplay display = GetItemDisplay();
            display.Paint(this, p);

            if (clipSet)
            {
                if (oldClip != null)
                    g.Clip = oldClip;
                else
                    g.ResetClip();
            }

            if (oldClip != null)
                oldClip.Dispose();

            this.DrawInsertMarker(p.Graphics);
        }

        protected virtual Rectangle GetClipRectangle()
        {
            Rectangle clip = this.DisplayRectangle;
            bool disposeStyle = false;
            ElementStyle style = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
            clip.X += ElementStyleLayout.LeftWhiteSpace(style);
            clip.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(style);
            clip.Y += ElementStyleLayout.TopWhiteSpace(style);
            clip.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(style);
            if (disposeStyle) style.Dispose();
            return clip;
        }

        /// <summary>
        /// Paints background of the item.
        /// </summary>
        /// <param name="p">Provides painting arguments</param>
        protected virtual void PaintBackground(ItemPaintArgs p)
        {
            _BackgroundStyle.SetColorScheme(p.Colors);
            bool disposeStyle = false;
            ElementStyle style = GetRenderBackgroundStyle(out disposeStyle);
            Graphics g = p.Graphics;
            ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(style, g, this.DisplayRectangle));
            if(disposeStyle)
                style.Dispose();
        }

        protected virtual ElementStyle GetRenderBackgroundStyle(out bool disposeStyle)
        {
            disposeStyle = false;
            return ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
        }

        private ItemDisplay _ItemDisplay = null;
        internal ItemDisplay GetItemDisplay()
        {
            if (_ItemDisplay == null)
                _ItemDisplay = new ItemDisplay();
            return _ItemDisplay;
        }

        private void BackgroundStyleChanged(object sender, EventArgs e)
        {
            this.OnAppearanceChanged();
        }

        /// <summary>
        /// Specifies the container background style. Default value is an empty style which means that container does not display any background.
        /// BeginGroup property set to true will override this style on some styles.
        /// </summary>
        [Browsable(true), Category("Style"), Description("Gets or sets container background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return _BackgroundStyle; }
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            CalendarBase objCopy = new CalendarBase();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        /// <summary>
        /// Copies the CalendarMonth specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem c)
        {
            CalendarBase copy = c as CalendarBase;
            copy.BackgroundStyle.ApplyStyle(_BackgroundStyle);

            base.CopyToItem(copy);
        }

        #endregion

        #region Property Hiding
        /// <summary>
        /// Returns name of the item that can be used to identify item from the code.
        /// </summary>
        [DefaultValue(""), Category("Design"), Description("Indicates the name used to identify item.")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the accessible role of the item.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Category("Accessibility"), Description("Gets or sets the accessible role of the item."), DefaultValue(System.Windows.Forms.AccessibleRole.Grouping)]
        public override System.Windows.Forms.AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole; }
            set { base.AccessibleRole = value; }
        }

        // <summary>
        /// Gets or sets the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar. Use KeyTips property
        /// when you want to assign the one or more letters to be used to access an item. For example assigning the FN to KeyTips property
        /// will require the user to press F then N keys to select an item. Pressing the F letter will show only keytips for the items that start with letter F.
        /// </summary>
        [Browsable(false), Category("Appearance"), DefaultValue(""), Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
        public override string KeyTips
        {
            get { return base.KeyTips; }
            set { base.KeyTips = value; }
        }

        /// <summary>
        /// Indicates whether the item will auto-collapse (fold) when clicked. 
        /// When item is on popup menu and this property is set to false, menu will not
        /// close when item is clicked.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue(true), System.ComponentModel.Description("Indicates whether the item will auto-collapse (fold) when clicked.")]
        public override bool AutoCollapseOnClick
        {
            get { return base.AutoCollapseOnClick; }
            set { base.AutoCollapseOnClick = value; }
        }

        /// <summary>
        /// Gets or sets whether item can be customized by end user.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Indicates whether item can be customized by user.")]
        public override bool CanCustomize
        {
            get { return base.CanCustomize; }
            set { base.CanCustomize = value; }
        }

        /// <summary>
        /// Returns category for this item. If item cannot be customzied using the
        /// customize dialog category is empty string.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(""), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates item category used to group similar items at design-time.")]
        public override string Category
        {
            get { return base.Category; }
            set { base.Category = value; }
        }

        /// <summary>
        /// Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
        public override bool ClickAutoRepeat
        {
            get { return base.ClickAutoRepeat; }
            set { base.ClickAutoRepeat = value; }
        }

        /// <summary>
        /// Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(600), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
        public override int ClickRepeatInterval
        {
            get { return base.ClickRepeatInterval; }
            set { base.ClickRepeatInterval = value; }
        }

        /// <summary>
        /// Specifies the mouse cursor displayed when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(null), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies the mouse cursor displayed when mouse is over the item.")]
        public override System.Windows.Forms.Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        /// <summary>
        /// Gets or sets item description. This description is displayed in
        /// Customize dialog to describe the item function in an application.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(""), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates description of the item that is displayed during design.")]
        public override string Description
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        /// <summary>
        /// Gets or sets whether item is global or not.
        /// This flag is used to propagate property changes to all items with the same name.
        /// Setting for example Visible property on the item that has GlobalItem set to true will
        /// set visible property to the same value on all items with the same name.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Indicates whether certain global properties are propagated to all items with the same name when changed.")]
        public override bool GlobalItem
        {
            get { return base.GlobalItem; }
            set { base.GlobalItem = value; }
        }

        /// <summary>
        /// Gets or sets item alignment inside the container.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(DevComponents.DotNetBar.eItemAlignment.Near), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Determines alignment of the item inside the container.")]
        public override DevComponents.DotNetBar.eItemAlignment ItemAlignment
        {
            get { return base.ItemAlignment; }
            set { base.ItemAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the collection of shortcut keys associated with the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Indicates list of shortcut keys for this item."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public override ShortcutsCollection Shortcuts
        {
            get { return base.Shortcuts; }
            set { base.Shortcuts = value; }
        }

        /// <summary>
        /// Gets or sets whether item will display sub items.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Determines whether sub-items are displayed.")]
        public override bool ShowSubItems
        {
            get { return base.ShowSubItems; }
            set { base.ShowSubItems = value; }
        }

        /// <summary>
        /// Gets or sets whether the item expands automatically to fill out the remaining space inside the container. Applies to Items on stretchable, no-wrap Bars only.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
        public override bool Stretch
        {
            get { return base.Stretch; }
            set { base.Stretch = value; }
        }

        /// <summary>
        /// Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(false), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
        public override bool ThemeAware
        {
            get { return base.ThemeAware; }
            set { base.ThemeAware = value; }
        }

        /// <summary>
        /// Gets/Sets informational text (tooltip) for the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), DevCoBrowsable(false), System.ComponentModel.DefaultValue(""), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates the text that is displayed when mouse hovers over the item."), System.ComponentModel.Localizable(true)]
        public override string Tooltip
        {
            get { return base.Tooltip; }
            set { base.Tooltip = value; }
        }

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [Browsable(false)]
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

        #region IDesignTimeProvider Implementation
        protected virtual InsertPosition GetContainerInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return DesignTimeProviderContainer.GetInsertPosition(this, pScreen, dragItem);
        }
        InsertPosition IDesignTimeProvider.GetInsertPosition(Point pScreen, BaseItem dragItem)
        {
            return GetContainerInsertPosition(pScreen, dragItem);
        }
        void IDesignTimeProvider.DrawReversibleMarker(int iPos, bool Before)
        {
            DesignTimeProviderContainer.DrawReversibleMarker(this, iPos, Before);
        }
        void IDesignTimeProvider.InsertItemAt(BaseItem objItem, int iPos, bool Before)
        {
            DesignTimeProviderContainer.InsertItemAt(this, objItem, iPos, Before);
        }

        #endregion
    }
}
#endif