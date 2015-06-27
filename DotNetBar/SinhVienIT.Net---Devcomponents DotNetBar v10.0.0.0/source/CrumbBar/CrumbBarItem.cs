using System;
using System.Text;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents an item for CrumbBar control.
    /// </summary>
    [Designer("DevComponents.DotNetBar.Design.CrumbBarItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), ToolboxItem(false), DesignTimeVisible(false)]
    public class CrumbBarItem : ButtonItem
    {
        #region Internal Implementation
        protected override void OnClick()
        {
            Select(eEventSource.Mouse);
            base.OnClick();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool ShowSubItems
        {
            get
            {
                return false;
            }
            set
            {
                base.ShowSubItems = value;
            }
        }

        private bool _IsSelected = false;
        /// <summary>
        /// Gets whether item is selected item in CrumbBar control.
        /// </summary>
        [Browsable(false)]
        public bool IsSelected
        {
            get { return _IsSelected; }
#if FRAMEWORK20
            internal set
#else
			set
#endif
            {
                _IsSelected = value;
                if (_IsSelected)
                    this.FontBold = true;
                else
                    this.FontBold = false;
            }
        }
        

        private void Select(eEventSource source)
        {
            CrumbBar bar = this.GetOwner() as CrumbBar;
            if (bar != null) bar.SetSelectedItem(this, source);
        }

        /// <summary>
        /// Returns the collection of sub items.
        /// </summary>
        [Browsable(false), Editor("DevComponents.DotNetBar.Design.CrumbBarItemCollectionEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public override SubItemsCollection SubItems
        {
            get
            {
                return base.SubItems;
            }
        }

        public override void OnImageChanged()
        {
            base.OnImageChanged();
            CrumbBar bar = this.GetOwner() as CrumbBar;
            if (bar != null && bar.SelectedItem == this)
            {
                bar.RefreshView();
            }
        }

        protected override void OnParentChanged()
        {
            CrumbBar bar = this.GetOwner() as CrumbBar;
            if (bar != null)
            {
                if (bar.SelectedItem == this || bar.GetIsInSelectedPath(this))
                    bar.SetSelectedItem(bar.SelectedItem, eEventSource.Code);
            }
            base.OnParentChanged();
        }

        protected internal override void OnAfterItemRemoved(BaseItem objItem)
        {
            if (objItem is CrumbBarItem && ((CrumbBarItem)objItem).IsSelected)
            {
                CrumbBar bar = this.GetOwner() as CrumbBar;
                if (bar != null)
                    bar.SetSelectedItem(this, eEventSource.Code);
            }
            base.OnAfterItemRemoved(objItem);
        }
        #endregion

        #region Property Hiding
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoCheckOnClick
        {
            get
            {
                return base.AutoCheckOnClick;
            }
            set
            {
                base.AutoCheckOnClick = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoCollapseOnClick
        {
            get
            {
                return base.AutoCollapseOnClick;
            }
            set
            {
                base.AutoCollapseOnClick = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoExpandOnClick
        {
            get
            {
                return base.AutoExpandOnClick;
            }
            set
            {
                base.AutoExpandOnClick = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eButtonStyle ButtonStyle
        {
            get
            {
                return base.ButtonStyle;
            }
            set
            {
                base.ButtonStyle = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool CanCustomize
        {
            get
            {
                return base.CanCustomize;
            }
            set
            {
                base.CanCustomize = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked
        {
            get
            {
                return base.Checked;
            }
            set
            {
                base.Checked = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool ClickAutoRepeat
        {
            get
            {
                return base.ClickAutoRepeat;
            }
            set
            {
                base.ClickAutoRepeat = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int ClickRepeatInterval
        {
            get
            {
                return base.ClickRepeatInterval;
            }
            set
            {
                base.ClickRepeatInterval = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eButtonColor ColorTable
        {
            get
            {
                return base.ColorTable;
            }
            set
            {
                base.ColorTable = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string CustomColorName
        {
            get
            {
                return base.CustomColorName;
            }
            set
            {
                base.CustomColorName = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Description
        {
            get
            {
                return base.Description;
            }
            set
            {
                base.Description = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool FontBold
        {
            get
            {
                return base.FontBold;
            }
            set
            {
                base.FontBold = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool FontItalic
        {
            get
            {
                return base.FontItalic;
            }
            set
            {
                base.FontItalic = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool FontUnderline
        {
            get
            {
                return base.FontUnderline;
            }
            set
            {
                base.FontUnderline = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool GlobalItem
        {
            get
            {
                return base.GlobalItem;
            }
            set
            {
                base.GlobalItem = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string GlobalName
        {
            get
            {
                return base.GlobalName;
            }
            set
            {
                base.GlobalName = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool HotFontBold
        {
            get
            {
                return base.HotFontBold;
            }
            set
            {
                base.HotFontBold = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool HotFontUnderline
        {
            get
            {
                return base.HotFontUnderline;
            }
            set
            {
                base.HotFontUnderline = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eHotTrackingStyle HotTrackingStyle
        {
            get
            {
                return base.HotTrackingStyle;
            }
            set
            {
                base.HotTrackingStyle = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Image HoverImage
        {
            get
            {
                return base.HoverImage;
            }
            set
            {
                base.HoverImage = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int HoverImageIndex
        {
            get
            {
                return base.HoverImageIndex;
            }
            set
            {
                base.HoverImageIndex = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Icon Icon
        {
            get
            {
                return base.Icon;
            }
            set
            {
                base.Icon = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eButtonImageListSelection ImageListSizeSelection
        {
            get
            {
                return base.ImageListSizeSelection;
            }
            set
            {
                base.ImageListSizeSelection = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int ImagePaddingHorizontal
        {
            get
            {
                return base.ImagePaddingHorizontal;
            }
            set
            {
                base.ImagePaddingHorizontal = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int ImagePaddingVertical
        {
            get
            {
                return base.ImagePaddingVertical;
            }
            set
            {
                base.ImagePaddingVertical = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eImagePosition ImagePosition
        {
            get
            {
                return base.ImagePosition;
            }
            set
            {
                base.ImagePosition = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Image ImageSmall
        {
            get
            {
                return base.ImageSmall;
            }
            set
            {
                base.ImageSmall = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eItemAlignment ItemAlignment
        {
            get
            {
                return base.ItemAlignment;
            }
            set
            {
                base.ItemAlignment = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string KeyTips
        {
            get
            {
                return base.KeyTips;
            }
            set
            {
                base.KeyTips = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override eMenuVisibility MenuVisibility
        {
            get
            {
                return base.MenuVisibility;
            }
            set
            {
                base.MenuVisibility = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string OptionGroup
        {
            get
            {
                return base.OptionGroup;
            }
            set
            {
                base.OptionGroup = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ePersonalizedMenus PersonalizedMenus
        {
            get
            {
                return base.PersonalizedMenus;
            }
            set
            {
                base.PersonalizedMenus = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ePopupSide PopupSide
        {
            get
            {
                return base.PopupSide;
            }
            set
            {
                base.PopupSide = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ePopupType PopupType
        {
            get
            {
                return base.PopupType;
            }
            set
            {
                base.PopupType = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int PopupWidth
        {
            get
            {
                return base.PopupWidth;
            }
            set
            {
                base.PopupWidth = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Image PressedImage
        {
            get
            {
                return base.PressedImage;
            }
            set
            {
                base.PressedImage = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int PressedImageIndex
        {
            get
            {
                return base.PressedImageIndex;
            }
            set
            {
                base.PressedImageIndex = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool RibbonWordWrap
        {
            get
            {
                return base.RibbonWordWrap;
            }
            set
            {
                base.RibbonWordWrap = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ShapeDescriptor Shape
        {
            get
            {
                return base.Shape;
            }
            set
            {
                base.Shape = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool SplitButton
        {
            get
            {
                return base.SplitButton;
            }
            set
            {
                base.SplitButton = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Stretch
        {
            get
            {
                return base.Stretch;
            }
            set
            {
                base.Stretch = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int SubItemsExpandWidth
        {
            get
            {
                return base.SubItemsExpandWidth;
            }
            set
            {
                base.SubItemsExpandWidth = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Size FixedSize
        {
            get
            {
                return base.FixedSize;
            }
            set
            {
                base.FixedSize = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int PulseSpeed
        {
            get
            {
                return base.PulseSpeed;
            }
            set
            {
                base.PulseSpeed = value;
            }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool StopPulseOnMouseOver
        {
            get
            {
                return base.StopPulseOnMouseOver;
            }
            set
            {
                base.StopPulseOnMouseOver = value;
            }
        }
        #endregion
    }
}
