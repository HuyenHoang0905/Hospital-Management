using System.ComponentModel;
using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar
{
    /// <summary>
    ///    Summary description for ButtonItem.
    /// </summary>
    [System.ComponentModel.ToolboxItem(false), System.ComponentModel.DesignTimeVisible(false), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.BaseItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class ButtonItem : PopupItem, IPersonalizedMenuItem
    {
        #region Events
        /// <summary>
        /// Occurs when Checked property has changed.
        /// </summary>
        public event EventHandler CheckedChanged;
        /// <summary>
        /// Occurs before an item in option group is checked and provides opportunity to cancel that.
        /// </summary>
        public event OptionGroupChangingEventHandler OptionGroupChanging;
        #endregion

        #region Private Variables
        private const int STATEBUTTON_SPACING = 3;
        private System.Drawing.Image m_Image;
        private System.Drawing.Image m_ImageSmall;
        private int m_ImageIndex; // Image index if image from ImageList is used
        private System.Drawing.Image m_HoverImage;
        private int m_HoverImageIndex; // Image index if image from ImageList is used
        private System.Drawing.Image m_DisabledImage;
        private bool m_DisabledImageCustom = false;
        private int m_DisabledImageIndex; // Image index if image from ImageList is used
        private System.Drawing.Icon m_DisabledIcon = null;
        private System.Drawing.Image m_PressedImage;
        private int m_PressedImageIndex;  // Image index if image from ImageList is used
        private eButtonStyle m_ButtonStyle;
        private eImagePosition m_ImagePosition;
        private Font m_Font;

        private string m_AlternateShortcutText;

        private bool m_MouseOver = false;
        private bool m_MouseOverExpand = false;
        private bool m_MouseDown;

        private bool m_Checked;

        private Rectangle m_ImageDrawRect;
        private Rectangle m_TextDrawRect;
        private Rectangle m_SubItemsRect;
        private int m_VerticalPadding;
        private int m_HorizontalPadding;

        private Color m_ForeColor = System.Drawing.Color.Empty;
        private Color m_HotForeColor = System.Drawing.Color.Empty;
        private bool m_HotFontUnderline = false;
        private bool m_HotFontBold = false;
        private bool m_FontBold = false;
        private bool m_FontItalic = false;
        private bool m_FontUnderline = false;

        // IPersonalizedMenuItem Implementation
        private eMenuVisibility m_MenuVisibility = eMenuVisibility.VisibleAlways;
        private bool m_RecentlyUsed = false;
        private eHotTrackingStyle m_HotTrackingStyle = eHotTrackingStyle.Default;
        private System.Drawing.Image m_ImageCachedIdx = null;
        private ItemPaintArgs _ItemPaintArgs = null;
        private int m_SubItemsExpandWidth = 12;
        private string m_OptionGroup = "";
        private Size m_ImageSizeOverride = Size.Empty;

        internal bool _FitContainer = false;

        System.Drawing.Icon m_Icon = null;
        private bool m_AutoExpandOnClick = false;

        private eButtonColor m_ColorTable = eButtonColor.Orange; private string m_CashedColorName = "Orange";

        private int m_ImagePaddingHorizontal = 8;
        private int m_ImagePaddingVertical = 6;
        private string m_CustomColorName = "";
        private Size m_FixedSize = Size.Empty;
        private bool m_SplitButton = false;
        private bool m_IgnoreAlpha = false;
        private int m_MouseOverFadeAlphaIncrement = 65;
        private int m_PulseFadeAlphaIncrement = 12;
        private bool m_Pulse = false;
        private int m_PulseBeats = 0, m_PulseCount = 0;
        private bool m_StopPulseOnMouseOver = true;
        private bool m_RibbonWordWrap = true;
        private bool m_AutoCheckOnClick = false;
        private bool m_UseSmallImage = false;
        internal bool _FixedSizeCenterText = false;
        #endregion

        #region Constructor, Copy
        /// <summary>
        /// Creates new instance of ButtonItem.
        /// </summary>
        public ButtonItem() : this("", "") { }
        /// <summary>
        /// Creates new instance of ButtonItem and assigns the name to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        public ButtonItem(string sItemName) : this(sItemName, "") { }
        /// <summary>
        /// Creates new instance of ButtonItem and assigns the name and text to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        /// <param name="ItemText">item text.</param>
        public ButtonItem(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            m_IsContainer = false;
            m_Image = null;
            m_ImageIndex = -1;
            m_HoverImage = null;
            m_HoverImageIndex = -1;
            m_DisabledImage = null;
            m_DisabledImageIndex = -1;
            m_PressedImage = null;
            m_PressedImageIndex = -1;
            m_MouseOver = false;
            m_MouseDown = false;
            m_ButtonStyle = eButtonStyle.Default;
            m_ImagePosition = eImagePosition.Left;
            m_Font = null;
            m_ImageDrawRect = Rectangle.Empty;
            m_TextDrawRect = Rectangle.Empty;
            m_SubItemsRect = Rectangle.Empty;
            m_VerticalPadding = 0;
            m_HorizontalPadding = 0;
            m_AlternateShortcutText = "";
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            ButtonItem objCopy = new ButtonItem(m_Name);
            this.CopyToItem(objCopy);
            return objCopy;
        }

        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ButtonItem instance.</param>
        internal void InternalCopyToItem(ButtonItem copy)
        {
            CopyToItem((BaseItem)copy);
        }

        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            ButtonItem objCopy = copy as ButtonItem;
            base.CopyToItem(objCopy);

            if (m_Image != null)
                objCopy.Image = (Image)m_Image.Clone();
            if (m_ImageSmall != null)
                objCopy.ImageSmall = (Image)m_ImageSmall.Clone();
            if (m_HoverImage != null)
                objCopy.HoverImage = (Image)m_HoverImage.Clone();
            if (m_PressedImage != null)
                objCopy.PressedImage = (Image)m_PressedImage.Clone();
            if (m_DisabledImage != null && m_DisabledImageCustom)
                objCopy.DisabledImage = (Image)m_DisabledImage.Clone();
            if (m_Icon != null)
                objCopy.Icon = m_Icon.Clone() as Icon;

            objCopy.ButtonStyle = m_ButtonStyle;
            //objCopy.ButtonType=m_ButtonType;
            objCopy.ImagePosition = m_ImagePosition;
            objCopy.OptionGroup = m_OptionGroup;
            objCopy.Checked = m_Checked;
            objCopy.PopupType = this.PopupType;
            objCopy.AlternateShortCutText = this.AlternateShortCutText;

            if (m_ImageIndex >= 0)
                objCopy.SetImageIndex(m_ImageIndex);
            if (m_PressedImageIndex >= 0)
                objCopy.PressedImageIndex = m_PressedImageIndex;
            if (m_HoverImageIndex >= 0)
                objCopy.HoverImageIndex = m_HoverImageIndex;
            if (m_DisabledImageIndex >= 0)
                objCopy.DisabledImageIndex = m_DisabledImageIndex;

            if (!m_ForeColor.IsEmpty)
                objCopy.ForeColor = m_ForeColor;

            objCopy.HotFontBold = m_HotFontBold;
            objCopy.HotFontUnderline = m_HotFontUnderline;

            if (!m_HotForeColor.IsEmpty)
                objCopy.HotForeColor = m_HotForeColor;

            objCopy.HotTrackingStyle = m_HotTrackingStyle;
            objCopy.MenuVisibility = m_MenuVisibility;
            objCopy.AutoExpandOnClick = this.AutoExpandOnClick;
            objCopy.AlternateShortCutText = this.AlternateShortCutText;
            objCopy.SubItemsExpandWidth = this.SubItemsExpandWidth;
            objCopy.CheckedChanged = this.CheckedChanged;
            objCopy.OptionGroupChanging = this.OptionGroupChanging;

            if (!this.ImageFixedSize.IsEmpty)
                objCopy.ImageFixedSize = this.ImageFixedSize;

            objCopy.ImagePaddingHorizontal = m_ImagePaddingHorizontal;
            objCopy.ImagePaddingVertical = m_ImagePaddingVertical;
            objCopy.FixedSize = this.FixedSize;
            objCopy.RibbonWordWrap = this.RibbonWordWrap;
            objCopy.AutoCheckOnClick = this.AutoCheckOnClick;

            if (this.Shape != null)
                objCopy.Shape = this.Shape;

            objCopy.ColorTable = this.ColorTable;
            objCopy.CustomColorName = this.CustomColorName;
            objCopy.AutoExpandMenuItem = this.AutoExpandMenuItem;

            objCopy.EnableMarkup = this.EnableMarkup;
            objCopy.SplitButton = this.SplitButton;
        }

        protected override void Dispose(bool disposing)
        {
            StopFade();
            StopPulse();
            DisposeMouseOverFont();
            DisposedCachedImageListImage();
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_Icon);
                BarUtilities.DisposeImage(ref m_ImageSmall);
                BarUtilities.DisposeImage(ref m_HoverImage);
                BarUtilities.DisposeImage(ref m_DisabledImage);
                BarUtilities.DisposeImage(ref m_PressedImage);
                BarUtilities.DisposeImage(ref m_DisabledIcon);
            }
            base.Dispose(disposing);
        }

        private void DisposedCachedImageListImage()
        {
            if (m_ImageCachedIdx != null) m_ImageCachedIdx.Dispose();
            m_ImageCachedIdx = null;
        }
        protected override System.Windows.Forms.AccessibleObject CreateAccessibilityInstance()
        {
            return new ButtonItemAccessibleObject(this);
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Overloaded. Serializes the item and all sub-items into the XmlElement.
        /// </summary>
        /// <param name="ThisItem">XmlElement to serialize the item to.</param>
        protected internal override void Serialize(ItemSerializationContext context)
        {
            base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
            ThisItem.SetAttribute("ImagePosition", System.Xml.XmlConvert.ToString(((int)m_ImagePosition)));
            ThisItem.SetAttribute("ButtonStyle", System.Xml.XmlConvert.ToString(((int)m_ButtonStyle)));
            ThisItem.SetAttribute("Checked", System.Xml.XmlConvert.ToString(m_Checked));
            ThisItem.SetAttribute("VerticalPadding", System.Xml.XmlConvert.ToString(m_VerticalPadding));
            ThisItem.SetAttribute("HorizontalPadding", System.Xml.XmlConvert.ToString(m_HorizontalPadding));

            ThisItem.SetAttribute("MenuVisibility", System.Xml.XmlConvert.ToString((int)m_MenuVisibility));
            ThisItem.SetAttribute("RecentlyUsed", System.Xml.XmlConvert.ToString(m_RecentlyUsed));

            if (m_AlternateShortcutText != "")
                ThisItem.SetAttribute("AlternateShortcutText", m_AlternateShortcutText);

            if (!m_ForeColor.IsEmpty)
                ThisItem.SetAttribute("forecolor", BarFunctions.ColorToString(m_ForeColor));

            if (m_HotTrackingStyle != eHotTrackingStyle.Default)
                ThisItem.SetAttribute("hottrack", System.Xml.XmlConvert.ToString((int)m_HotTrackingStyle));

            if (m_HotFontBold)
                ThisItem.SetAttribute("hotfb", System.Xml.XmlConvert.ToString(m_HotFontBold));
            if (m_HotFontUnderline)
                ThisItem.SetAttribute("hotfu", System.Xml.XmlConvert.ToString(m_HotFontUnderline));

            if (!m_HotForeColor.IsEmpty)
                ThisItem.SetAttribute("hotclr", BarFunctions.ColorToString(m_HotForeColor));

            if (m_OptionGroup != "")
                ThisItem.SetAttribute("optiongroup", m_OptionGroup);

            if (m_FontBold)
                ThisItem.SetAttribute("fontbold", System.Xml.XmlConvert.ToString(m_FontBold));
            if (m_FontItalic)
                ThisItem.SetAttribute("fontitalic", System.Xml.XmlConvert.ToString(m_FontItalic));
            if (m_FontUnderline)
                ThisItem.SetAttribute("fontunderline", System.Xml.XmlConvert.ToString(m_FontUnderline));

            if (m_AutoExpandOnClick)
                ThisItem.SetAttribute("autoexpandclick", System.Xml.XmlConvert.ToString(m_AutoExpandOnClick));

            if (m_CustomColorName != "")
                ThisItem.SetAttribute("CustomColorName", m_CustomColorName);
            if (m_ColorTable != eButtonColor.Orange)
                ThisItem.SetAttribute("ColorTable", Enum.GetName(m_ColorTable.GetType(), m_ColorTable));

            System.Xml.XmlElement xmlElem = null, xmlElem2 = null;

            // Serialize Images
            if (m_Image != null || m_ImageIndex >= 0 || m_HoverImage != null || m_HoverImageIndex >= 0 || m_DisabledImage != null || m_DisabledImageIndex >= 0 || m_PressedImage != null || m_PressedImageIndex >= 0 || m_Icon != null || m_ImageSmall != null)
            {
                xmlElem = ThisItem.OwnerDocument.CreateElement("images");
                ThisItem.AppendChild(xmlElem);

                if (m_ImageIndex >= 0)
                    xmlElem.SetAttribute("imageindex", System.Xml.XmlConvert.ToString(m_ImageIndex));
                if (m_HoverImageIndex >= 0)
                    xmlElem.SetAttribute("hoverimageindex", System.Xml.XmlConvert.ToString(m_HoverImageIndex));
                if (m_DisabledImageIndex >= 0)
                    xmlElem.SetAttribute("disabledimageindex", System.Xml.XmlConvert.ToString(m_DisabledImageIndex));
                if (m_PressedImageIndex >= 0)
                    xmlElem.SetAttribute("pressedimageindex", System.Xml.XmlConvert.ToString(m_PressedImageIndex));

                if (m_Image != null)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("image");
                    xmlElem2.SetAttribute("type", "default");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeImage(m_Image, xmlElem2);
                }
                if (m_ImageSmall != null)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("imagesmall");
                    xmlElem2.SetAttribute("type", "small");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeImage(m_ImageSmall, xmlElem2);
                }
                if (m_HoverImage != null)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("image");
                    xmlElem2.SetAttribute("type", "hover");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeImage(m_HoverImage, xmlElem2);
                }
                if (m_DisabledImage != null && m_DisabledImageCustom)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("image");
                    xmlElem2.SetAttribute("type", "disabled");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeImage(m_DisabledImage, xmlElem2);
                }
                if (m_PressedImage != null)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("image");
                    xmlElem2.SetAttribute("type", "pressed");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeImage(m_PressedImage, xmlElem2);
                }
                if (m_Icon != null)
                {
                    xmlElem2 = ThisItem.OwnerDocument.CreateElement("image");
                    xmlElem2.SetAttribute("type", "icon");
                    xmlElem.AppendChild(xmlElem2);
                    BarFunctions.SerializeIcon(m_Icon, xmlElem2);
                }
            }
        }

        /// <summary>
        /// Overloaded. Deserializes the Item from the XmlElement.
        /// </summary>
        /// <param name="ItemXmlSource">Source XmlElement.</param>
        public override void Deserialize(ItemSerializationContext context)
        {
            base.Deserialize(context);

            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;
            m_ImagePosition = (eImagePosition)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("ImagePosition"));
            m_ButtonStyle = (eButtonStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("ButtonStyle"));
            m_Checked = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("Checked"));
            m_VerticalPadding = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("VerticalPadding"));
            m_HorizontalPadding = System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("HorizontalPadding"));

            m_MenuVisibility = (eMenuVisibility)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("MenuVisibility"));
            m_RecentlyUsed = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("RecentlyUsed"));

            if (ItemXmlSource.HasAttribute("forecolor"))
                m_ForeColor = BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("forecolor"));
            else
                m_ForeColor = System.Drawing.Color.Empty;

            if (ItemXmlSource.HasAttribute("hottrack"))
                m_HotTrackingStyle = (eHotTrackingStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("hottrack"));
            else
                m_HotTrackingStyle = eHotTrackingStyle.Default;

            if (ItemXmlSource.HasAttribute("hotclr"))
                m_HotForeColor = BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("hotclr"));
            else
                m_HotForeColor = System.Drawing.Color.Empty;

            if (ItemXmlSource.HasAttribute("hotfb"))
                m_HotFontBold = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("hotfb"));
            else
                m_HotFontBold = false;
            if (ItemXmlSource.HasAttribute("hotfu"))
                m_HotFontUnderline = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("hotfu"));
            else
                m_HotFontUnderline = false;

            if (ItemXmlSource.HasAttribute("optiongroup"))
                m_OptionGroup = ItemXmlSource.GetAttribute("optiongroup");
            else
                m_OptionGroup = "";

            if (ItemXmlSource.HasAttribute("fontbold"))
                m_FontBold = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("fontbold"));
            else
                m_FontBold = false;
            if (ItemXmlSource.HasAttribute("fontitalic"))
                m_FontItalic = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("fontitalic"));
            else
                m_FontItalic = false;
            if (ItemXmlSource.HasAttribute("fontunderline"))
                m_FontUnderline = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("fontunderline"));
            else
                m_FontUnderline = false;

            if (ItemXmlSource.HasAttribute("AlternateShortcutText"))
                m_AlternateShortcutText = ItemXmlSource.GetAttribute("AlternateShortcutText");
            else
                m_AlternateShortcutText = "";

            if (ItemXmlSource.HasAttribute("autoexpandclick"))
                m_AutoExpandOnClick = System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("autoexpandclick"));
            else
                m_AutoExpandOnClick = false;

            if (ItemXmlSource.HasAttribute("CustomColorName"))
                m_CustomColorName = ItemXmlSource.GetAttribute("CustomColorName");
            else
                m_CustomColorName = "";

            if (ItemXmlSource.HasAttribute("ColorTable"))
                m_ColorTable = (eButtonColor)Enum.Parse(m_ColorTable.GetType(), ItemXmlSource.GetAttribute("ColorTable"));
            else
                m_ColorTable = eButtonColor.Orange;

            m_ImageIndex = -1;
            m_HoverImageIndex = -1;
            m_DisabledImageIndex = -1;
            m_PressedImageIndex = -1;
            m_Icon = null;
            m_Image = null;
            m_ImageSmall = null;
            m_HoverImage = null;
            m_DisabledImage = null;
            m_DisabledImageCustom = false;
            m_PressedImage = null;

            // Load Images
            foreach (System.Xml.XmlElement xmlElem in ItemXmlSource.ChildNodes)
            {
                if (xmlElem.Name == "images")
                {
                    if (xmlElem.HasAttribute("imageindex"))
                        m_ImageIndex = System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("imageindex"));
                    if (xmlElem.HasAttribute("hoverimageindex"))
                        m_HoverImageIndex = System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("hoverimageindex"));
                    if (xmlElem.HasAttribute("disabledimageindex"))
                        m_DisabledImageIndex = System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("disabledimageindex"));
                    if (xmlElem.HasAttribute("pressedimageindex"))
                        m_PressedImageIndex = System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("pressedimageindex"));

                    foreach (System.Xml.XmlElement xmlElem2 in xmlElem.ChildNodes)
                    {
                        switch (xmlElem2.GetAttribute("type"))
                        {
                            case "default":
                                {
                                    m_Image = BarFunctions.DeserializeImage(xmlElem2);
                                    m_ImageIndex = -1;
                                    break;
                                }
                            case "icon":
                                {
                                    m_Icon = BarFunctions.DeserializeIcon(xmlElem2);
                                    m_ImageIndex = -1;
                                    break;
                                }
                            case "hover":
                                {
                                    m_HoverImage = BarFunctions.DeserializeImage(xmlElem2);
                                    m_HoverImageIndex = -1;
                                    break;
                                }
                            case "disabled":
                                {
                                    m_DisabledImage = BarFunctions.DeserializeImage(xmlElem2);
                                    m_DisabledImageIndex = -1;
                                    m_DisabledImageCustom = true;
                                    break;
                                }
                            case "pressed":
                                {
                                    m_PressedImage = BarFunctions.DeserializeImage(xmlElem2);
                                    m_PressedImageIndex = -1;
                                    break;
                                }
                            case "small":
                                {
                                    m_ImageSmall = BarFunctions.DeserializeImage(xmlElem2);
                                    break;
                                }
                        }
                    }
                    break;
                }
            }
            this.OnImageChanged();
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Gets/Sets the image position inside the button.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The alignment of the image in relation to text displayed by this item."), System.ComponentModel.DefaultValue(eImagePosition.Left)]
        public virtual eImagePosition ImagePosition
        {
            get
            {
                return m_ImagePosition;
            }
            set
            {
                if (m_ImagePosition != value)
                {
                    m_ImagePosition = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImagePosition");

                    NeedRecalcSize = true;
                    if (this.Parent != null)
                        this.Parent.NeedRecalcSize = true;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom color name. Name specified here must be represented by the corresponding object with the same name that is part
        /// of the Office2007ColorTable.RibbonTabItemColors collection. See documentation for Office2007ColorTable.RibbonTabItemColors for more information.
        /// If color table with specified name cannot be found default color will be used. Valid settings for this property override any
        /// setting to the Color property.
        /// Applies to items with Office 2007 style only.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(""), Category("Appearance"), Description("Indicates custom color table name for the button when Office 2007 style is used.")]
        public virtual string CustomColorName
        {
            get { return m_CustomColorName; }
            set
            {
                m_CustomColorName = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the predefined color of the button. Color specified applies to buttons with Office 2007 style only. It does not have
        /// any effect on other styles. Default value is eButtonColor.Default
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eButtonColor.Orange), Category("Appearance"), Description("Indicates predefined color of button when Office 2007 style is used.")]
        public virtual eButtonColor ColorTable
        {
            get { return m_ColorTable; }
            set
            {
                if (m_ColorTable != value)
                {
                    m_ColorTable = value;
                    m_CashedColorName = Enum.GetName(typeof(eButtonColor), m_ColorTable);
                    this.Refresh();
                }
            }
        }

        internal virtual string GetColorTableName()
        {
            return m_CashedColorName;
        }

        /// <summary>
        /// Gets/Sets the button style which controls the appearance of the button elements. Changing the property can display image only, text only or image and text on the button at all times.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Determines the style of the button."), System.ComponentModel.DefaultValue(eButtonStyle.Default)]
        public virtual eButtonStyle ButtonStyle
        {
            get
            {
                return m_ButtonStyle;
            }
            set
            {
                if (m_ButtonStyle != value)
                {
                    m_ButtonStyle = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ButtonStyle");
                    NeedRecalcSize = true;
                    if (this.Displayed && m_Parent != null)
                    {
                        RecalcSize();
                        m_Parent.SubItemSizeChanged(this);
                    }
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether the item will auto-expand when clicked. 
        /// When item is on top level bar and not on menu and contains sub-items, sub-items will be shown only if user
        /// click the expand part of the button. Setting this propert to true will expand the button and show sub-items when user
        /// clicks anywhere inside of the button. Default value is false which indicates that button is expanded only
        /// if its expand part is clicked.
        /// </summary>
        [DefaultValue(false), Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates whether the item will auto-expand (display pop-up menu or toolbar) when clicked.")]
        public virtual bool AutoExpandOnClick
        {
            get
            {
                return m_AutoExpandOnClick;
            }
            set
            {
                m_AutoExpandOnClick = value;
            }
        }


        /// <summary>
        /// Specifies the Button icon. Icons support multiple image sizes and alpha blending.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the Button icon. Icons support multiple image sizes and alpha blending."), DefaultValue(null)]
        public virtual System.Drawing.Icon Icon
        {
            get
            {
                return m_Icon;
            }
            set
            {
                NeedRecalcSize = true;
                m_Icon = value;
                this.OnImageChanged();
                OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Specifies the Button image.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed on the face of the item."), DefaultValue(null)]
        public System.Drawing.Image Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                NeedRecalcSize = true;
                m_Image = value;
                this.OnImageChanged();
                OnAppearanceChanged();
                this.Refresh();
            }
        }

        /// <summary>
        /// Specifies the small Button image used by Ribbon control when small image variant is needed because of the automatic button resizing or
        /// because the button is on the Quick Access Toolbar.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Specifies the small Button image used by Ribbon control when small image variant is needed"), DefaultValue(null)]
        public virtual System.Drawing.Image ImageSmall
        {
            get
            {
                return m_ImageSmall;
            }
            set
            {
                m_ImageSmall = value;
                if (!m_ImageSizeOverride.IsEmpty || UseSmallImageResolved)
                {
                    NeedRecalcSize = true;
                    OnAppearanceChanged();
                    OnImageChanged();
                    this.Refresh();
                }
            }
        }

        private bool UseSmallImageResolved
        {
            get
            {
                return m_UseSmallImage;
            }
        }

        private eButtonImageListSelection _ImageListSizeSelection = eButtonImageListSelection.NotSet;
        /// <summary>
        /// Gets or sets the image size that is used by the button when multiple ImageList controls are used as source for button image.
        /// By default ImageList assigned to Images property of parent control is used. Using this property you can selection ImagesMedium or
        /// ImagesLarge ImageList to be used as source for image for this button.
        /// </summary>
        [DefaultValue(eButtonImageListSelection.NotSet), Category("Image"), Description("Indicates image size that is used by the button when multiple ImageList controls are used as source for button image.")]
        public virtual eButtonImageListSelection ImageListSizeSelection
        {
            get { return _ImageListSizeSelection; }
            set
            {
                _ImageListSizeSelection = value;
                NeedRecalcSize = true;
                OnImageChanged();
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether button uses the ImageSmall as source of the image displayed on the button if ImageSmall is set to valid image. Default value is false.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseSmallImage
        {
            get { return m_UseSmallImage; }
            set
            {
                if (m_UseSmallImage != value)
                {
                    m_UseSmallImage = value;
                    if (m_ImageSmall != null)
                    {
                        NeedRecalcSize = true;
                        OnImageChanged();
                        this.OnAppearanceChanged();
                    }
                }
            }
        }


        /// <summary>
        /// Specifies the index of the image for the button if ImageList is used.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The image list image index of the image that will be displayed on the face of the item."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)), System.ComponentModel.DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                return m_ImageIndex;
            }
            set
            {
                if (m_ImageCachedIdx != null) m_ImageCachedIdx.Dispose();
                m_ImageCachedIdx = null;
                if (m_ImageIndex != value)
                {
                    //if(this.GetOwner()==null)
                    //	throw(new System.InvalidOperationException("Owner DotNetBarManager is not set. Add containing bar do the bars collection first."));
                    m_ImageIndex = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImageIndex");
                    if (m_Parent != null)
                    {
                        OnImageChanged();
                        NeedRecalcSize = true;
                        this.Refresh();
                    }
                    OnAppearanceChanged();
                }
            }
        }

        internal void SetImageIndex(int iImageIndex)
        {
            m_ImageIndex = iImageIndex;
        }

        /// <summary>
        /// Called when button image has changed.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void OnImageChanged()
        {
            base.OnImageChanged();
            StopPulse();
            if (m_DisabledImage != null && !m_DisabledImageCustom)
            {
                m_DisabledImage.Dispose();
                m_DisabledImage = null;
            }
            if (m_DisabledIcon != null)
            {
                m_DisabledIcon.Dispose();
                m_DisabledIcon = null;
            }

            CompositeImage img = this.GetImage(ImageState.Default);
            if (img != null)
            {
                if (m_Image != null)
                {
                    IBarImageSize iImageSize = null;
                    if (_ItemPaintArgs != null)
                        iImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
                    if (iImageSize == null) iImageSize = this.ContainerControl as IBarImageSize;
                    eBarImageSize imageListSize = GetImageListSize(iImageSize);

                    if (iImageSize != null && imageListSize != eBarImageSize.Default)
                    {
                        if (imageListSize == eBarImageSize.Medium)
                        {
                            this.ImageSize = new Size(32, 32);
                        }
                        else
                        {
                            this.ImageSize = new Size(48, 48);
                        }
                    }
                    else
                        this.ImageSize = new Size(img.Width, img.Height);
                }
                else
                    this.ImageSize = new Size(img.Width, img.Height);
                img.Dispose();
            }
            else
                this.ImageSize = ImageItem._InitalImageSize; // This is ImageItem default size

            if (m_Parent != null)
            {
                ImageItem objParentImageItem = m_Parent as ImageItem;
                if (objParentImageItem != null)
                {
                    //if(this.DesignMode)
                    objParentImageItem.RefreshSubItemImageSize();
                    //else
                    //	objParentImageItem.OnSubItemImageSizeChanged(this);
                }
            }
        }

        /// <summary>
        /// Called when container of the item has changed.
        /// </summary>
        /// <param name="objOldContainer">Previous item container.</param>
        protected internal override void OnContainerChanged(object objOldContainer)
        {
            if (this.DesignMode || (m_ImageIndex >= 0 || m_Icon != null) && this.ImageSize.Width == ImageItem._InitalImageSize.Width && this.ImageSize.Height == ImageItem._InitalImageSize.Height)
                OnImageChanged();
            base.OnContainerChanged(objOldContainer);
        }

        /// <summary>
        /// Specifies the image for the button when mouse is over the item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when mouse hovers over the item."), DefaultValue(null)]
        public virtual System.Drawing.Image HoverImage
        {
            get
            {
                return m_HoverImage;
            }
            set
            {
                m_HoverImage = value;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Specifies the index of the image for the button when mouse is over the item when ImageList is used.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The image list image index of the image that will be displayed when mouse hovers over the item."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)), System.ComponentModel.DefaultValue(-1)]
        public virtual int HoverImageIndex
        {
            get
            {
                return m_HoverImageIndex;
            }
            set
            {
                if (m_HoverImageIndex != value)
                {
                    //if(this.GetOwner()==null)
                    //	throw(new System.InvalidOperationException("Owner DotNetBarManager is not set. Add containing bar do the bars collection first."));
                    m_HoverImageIndex = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HoverImageIndex");

                    this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the image for the button when mouse left button is pressed.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when item is pressed."), DefaultValue(null)]
        public virtual System.Drawing.Image PressedImage
        {
            get
            {
                return m_PressedImage;
            }
            set
            {
                m_PressedImage = value;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Specifies the index of the image for the button when mouse left button is pressed and ImageList is used.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The image list image index of the image that will be displayed when item is pressed."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)), System.ComponentModel.DefaultValue(-1)]
        public virtual int PressedImageIndex
        {
            get
            {
                return m_PressedImageIndex;
            }
            set
            {
                if (m_PressedImageIndex != value)
                {
                    //if(this.GetOwner()==null)
                    //	throw(new System.InvalidOperationException("Owner DotNetBarManager is not set. Add containing bar do the bars collection first."));
                    m_PressedImageIndex = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "PressedImageIndex");

                    OnAppearanceChanged();
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Specifies the image for the button when items Enabled property is set to false.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("The image that will be displayed when item is disabled."), DefaultValue(null)]
        public System.Drawing.Image DisabledImage
        {
            get
            {
                if (!m_DisabledImageCustom)
                    return null;
                return m_DisabledImage;
            }
            set
            {
                m_DisabledImage = value;
                if (value == null)
                    m_DisabledImageCustom = false;
                else
                    m_DisabledImageCustom = true;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Specifies the index of the image for the button when items Enabled property is set to false and ImageList is used.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The image list image index of the image that will be displayed when item is disabled."), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)), System.ComponentModel.DefaultValue(-1)]
        public int DisabledImageIndex
        {
            get
            {
                return m_DisabledImageIndex;
            }
            set
            {
                if (m_DisabledImageIndex != value)
                {
                    if (m_DisabledImage != null)
                        m_DisabledImage.Dispose();
                    m_DisabledImage = null;
                    //if(this.GetOwner()==null)
                    //	throw(new System.InvalidOperationException("Owner DotNetBarManager is not set. Add containing bar do the bars collection first."));
                    m_DisabledImageIndex = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "DisabledImageIndex");

                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Overriden. Draws the item.
        /// </summary>
        /// <param name="g">Item paint arguments.</param>
        public override void Paint(ItemPaintArgs p)
        {
            if (this.SuspendLayout)
                return;

            bool oldMouseOver = m_MouseOver;

            m_FadeImageLock.AcquireReaderLock(-1);
            try
            {
                if (m_ImageState2 != null)
                    m_MouseOver = false;
                try
                {
                    _ItemPaintArgs = p;

                    if (EffectiveStyle == eDotNetBarStyle.Office2000 && !this.IsThemed)
                        PaintOffice(p);
                    else
                    {
                        RenderButton(p);

                        if (m_ImageState2 != null && !m_IgnoreAlpha)
                        {
                            Graphics g = p.Graphics;
                            Rectangle r = this.DisplayRectangle;
                            System.Drawing.Imaging.ColorMatrix matrix1 = new System.Drawing.Imaging.ColorMatrix();
                            matrix1[3, 3] = (float)((float)m_Alpha / 255);
                            using (System.Drawing.Imaging.ImageAttributes imageAtt = new System.Drawing.Imaging.ImageAttributes())
                            {
                                imageAtt.SetColorMatrix(matrix1);
                                g.DrawImage(m_ImageState2, r, 0, 0, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);
                            }
                            return;
                        }
                    }
                }
                finally
                {
                    _ItemPaintArgs = null;
                    m_MouseOver = oldMouseOver;
                }
            }
            finally
            {
                m_FadeImageLock.ReleaseReaderLock();
            }

            this.DrawInsertMarker(p.Graphics);
        }

        protected virtual void RenderButton(ItemPaintArgs p)
        {
            Rendering.BaseRenderer renderer = p.Renderer;
            if (renderer != null)
            {
                p.ButtonItemRendererEventArgs.Graphics = p.Graphics;
                p.ButtonItemRendererEventArgs.ButtonItem = this;
                p.ButtonItemRendererEventArgs.ItemPaintArgs = p;
                renderer.DrawButtonItem(p.ButtonItemRendererEventArgs);
            }
            else
            {
                ButtonItemPainter painter = PainterFactory.CreateButtonPainter(this);
                if (painter != null)
                {
                    painter.PaintButton(this, p);
                }
                else
                {
                    if (EffectiveStyle == eDotNetBarStyle.Office2000)
                        PaintOffice(p);
                    else
                        PaintDotNet(p);
                }
            }
        }

        private void PaintDotNet(ItemPaintArgs pa)
        {
            bool bIsOnMenu = pa.IsOnMenu;
            bool bIsOnMenuBar = pa.IsOnMenuBar;
            bool bThemed = this.IsThemed;

            if (!bIsOnMenu && !bIsOnMenuBar && bThemed)
            {
                ThemedButtonItemPainter.PaintButton(this, pa);
                return;
            }

            bool mouseOver = m_MouseOver;
            if (bIsOnMenu && this.Expanded && pa.ContainerControl != null && pa.ContainerControl.Parent != null)
            {
                if (!pa.ContainerControl.Parent.Bounds.Contains(System.Windows.Forms.Control.MousePosition))
                    mouseOver = true;
            }

            System.Drawing.Graphics g = pa.Graphics;

            Rectangle rect = Rectangle.Empty;
            Rectangle itemRect = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
            Color textColor = System.Drawing.Color.Empty;

            if (mouseOver && !m_HotForeColor.IsEmpty)
                textColor = m_HotForeColor;
            else if (!m_ForeColor.IsEmpty)
                textColor = m_ForeColor;
            else if (mouseOver /*&& m_Checked*/ && !m_Expanded && /*!bIsOnMenu &&*/ m_HotTrackingStyle != eHotTrackingStyle.Image)
                textColor = pa.Colors.ItemHotText;
            else if (m_Expanded)
                textColor = pa.Colors.ItemExpandedText;
            else
            {
                if (bThemed && bIsOnMenuBar && pa.Colors.ItemText == SystemColors.ControlText)
                    textColor = SystemColors.MenuText;
                else
                    textColor = pa.Colors.ItemText;
            }

            Font objFont = null;

            eTextFormat objStringFormat = pa.ButtonStringFormat;
            CompositeImage objImage = GetImage();
            System.Drawing.Size imageSize = System.Drawing.Size.Empty;

            if (m_Font != null)
                objFont = m_Font;
            else
                objFont = GetFont(pa, false);

            // Calculate image position
            if (objImage != null)
            {
                imageSize = this.ImageSize;// objImage.Size;
                if (!bIsOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom))
                    rect = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, itemRect.Width, m_ImageDrawRect.Height);
                else
                    rect = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, m_ImageDrawRect.Height);

                rect.Offset(itemRect.Left, itemRect.Top);
                //if(m_ButtonType==eButtonType.StateButton)
                //	rect.Offset(STATEBUTTON_SPACING+(m_ImageSize.Width-STATEBUTTON_SPACING)/2+(rect.Width-m_Image.Width*2)/2,(rect.Height-m_Image.Height)/2);
                //else
                rect.Offset((rect.Width - imageSize.Width) / 2, (rect.Height - imageSize.Height) / 2);

                rect.Width = imageSize.Width;
                rect.Height = imageSize.Height;
            }

            if (bIsOnMenu)
            {
                // Draw side bar
                if (this.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
                {
                    if (!pa.Colors.MenuUnusedSide2.IsEmpty)
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
                        g.FillRectangle(gradient, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
                        gradient.Dispose();
                    }
                    else
                    {
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.MenuUnusedSide/*ColorFunctions.SideRecentlyBackColor(g)*/))
                            g.FillRectangle(mybrush, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
                    }
                }
                else
                {
                    if (!pa.Colors.MenuSide2.IsEmpty)
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height), pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
                        g.FillRectangle(gradient, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
                        gradient.Dispose();
                    }
                    else
                    {
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.MenuSide))
                            g.FillRectangle(mybrush, m_Rect.Left, m_Rect.Top, m_ImageDrawRect.Right, m_Rect.Height);
                    }
                }
                // Draw Background of the item
                //using(SolidBrush mybrush=new SolidBrush(pa.Colors.MenuBackground))
                //	g.FillRectangle(mybrush,m_Rect.Left+m_ImageDrawRect.Right,m_Rect.Top,m_Rect.Width-m_ImageDrawRect.Right,m_Rect.Height);
            }
            else
            {
                // Draw button background
                //if(bIsOnMenuBar)
                //	g.FillRectangle(SystemBrushes.Control,m_Rect);
                //else
                //	g.FillRectangle(new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(g)),m_Rect);
                if (!pa.Colors.ItemBackground.IsEmpty)
                {
                    if (pa.Colors.ItemBackground2.IsEmpty)
                    {
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemBackground))
                            g.FillRectangle(mybrush, m_Rect);
                    }
                    else
                    {
                        using (System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(this.DisplayRectangle, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle))
                            g.FillRectangle(gradient, this.DisplayRectangle);
                    }
                }
                else if (!GetEnabled(pa.ContainerControl) && !pa.Colors.ItemDisabledBackground.IsEmpty)
                {
                    using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemDisabledBackground))
                        g.FillRectangle(mybrush, m_Rect);
                }
            }

            if (GetEnabled(pa.ContainerControl) || this.DesignMode)
            {
                if (m_Expanded && !bIsOnMenu)
                {
                    // DotNet Style
                    if (pa.Colors.ItemExpandedBackground2.IsEmpty)
                    {
                        Rectangle rBack = new Rectangle(itemRect.Left, itemRect.Top, itemRect.Width, itemRect.Height);
                        if (pa.Colors.ItemExpandedShadow.IsEmpty)
                            rBack.Width -= 2;
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemExpandedBackground))
                            g.FillRectangle(mybrush, rBack);
                    }
                    else
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(itemRect.Left, itemRect.Top, itemRect.Width - 2, itemRect.Height), pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
                        Rectangle rBack = new Rectangle(itemRect.Left, itemRect.Top, itemRect.Width, itemRect.Height);
                        if (pa.Colors.ItemExpandedShadow.IsEmpty)
                            rBack.Width -= 2;
                        g.FillRectangle(gradient, rBack);
                        gradient.Dispose();
                    }
                    Point[] p;
                    if (m_Orientation == eOrientation.Horizontal && this.PopupSide == ePopupSide.Default)
                        p = new Point[4];
                    else
                        p = new Point[5];
                    p[0].X = itemRect.Left;
                    p[0].Y = itemRect.Top + itemRect.Height - 1;
                    p[1].X = itemRect.Left;
                    p[1].Y = itemRect.Top;
                    if (m_Orientation == eOrientation.Horizontal /*&& !pa.Colors.ItemExpandedShadow.IsEmpty*/)
                        p[2].X = itemRect.Left + itemRect.Width - 3;
                    else
                        p[2].X = itemRect.Left + itemRect.Width - 1;
                    p[2].Y = itemRect.Top;
                    if (m_Orientation == eOrientation.Horizontal /*&& !pa.Colors.ItemExpandedShadow.IsEmpty*/)
                        p[3].X = itemRect.Left + itemRect.Width - 3;
                    else
                        p[3].X = itemRect.Left + itemRect.Width - 1;

                    p[3].Y = itemRect.Top + itemRect.Height - 1;
                    if (m_Orientation == eOrientation.Vertical || this.PopupSide != ePopupSide.Default)
                    {
                        p[4].X = itemRect.Left;
                        p[4].Y = itemRect.Top + itemRect.Height - 1;
                    }

                    if (!pa.Colors.ItemExpandedBorder.IsEmpty)
                    {
                        using (Pen mypen = new Pen(pa.Colors.ItemExpandedBorder, 1))
                            g.DrawLines(mypen, p);
                    }
                    // Draw the shadow
                    if (!pa.Colors.ItemExpandedShadow.IsEmpty && m_Orientation == eOrientation.Horizontal)
                    {
                        using (SolidBrush shadow = new SolidBrush(pa.Colors.ItemExpandedShadow))
                            g.FillRectangle(shadow, itemRect.Left + itemRect.Width - 2, itemRect.Top + 2, 2, itemRect.Height - 2); // TODO: ADD GRADIENT SHADOW					
                    }
                }

                if ((mouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded && !bIsOnMenu)
                {
                    // Draw Mouse over marker
                    if (!m_Expanded || bIsOnMenu)
                    {
                        Rectangle r = itemRect;
                        if (bIsOnMenu)
                            r = new Rectangle(itemRect.Left + 1, itemRect.Top, itemRect.Width - 2, itemRect.Height);
                        if (this.DesignMode && this.Focused)
                        {
                            //g.FillRectangle(new SolidBrush(ColorFunctions.MenuBackColor(g)),r);
                            r = m_Rect;
                            r.Inflate(-1, -1);
                            DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
                        }
                        else
                        {
                            if (m_MouseDown)
                            {
                                if (m_HotTrackingStyle == eHotTrackingStyle.Image)
                                {
                                    r = rect;
                                    r.Inflate(2, 2);
                                }
                                if (pa.Colors.ItemPressedBackground2.IsEmpty)
                                {
                                    using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemPressedBackground/*ColorFunctions.PressedBackColor(g)*/))
                                        g.FillRectangle(mybrush, r);
                                }
                                else
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
                                    g.FillRectangle(gradient, r);
                                    gradient.Dispose();
                                }
                                using (Pen mypen = new Pen(pa.Colors.ItemPressedBorder, 1))
                                    NativeFunctions.DrawRectangle(g, mypen, r);
                            }
                            else if (m_HotTrackingStyle == eHotTrackingStyle.Image && !rect.IsEmpty)
                            {
                                Rectangle rImage = rect;
                                rImage.Inflate(2, 2);
                                if (!pa.Colors.ItemHotBackground2.IsEmpty)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(rImage, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                    g.FillRectangle(gradient, rImage);
                                    gradient.Dispose();
                                }
                                else
                                {
                                    using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemHotBackground/*ColorFunctions.HoverBackColor(g)*/))
                                        g.FillRectangle(mybrush, rImage);
                                }
                                using (Pen mypen = new Pen(pa.Colors.ItemHotBorder, 1))
                                    NativeFunctions.DrawRectangle(g, mypen, rImage);
                            }
                            else
                            {
                                if (!pa.Colors.ItemCheckedBackground2.IsEmpty && m_Checked)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                    g.FillRectangle(gradient, r);
                                    gradient.Dispose();
                                }
                                else
                                {
                                    if (!pa.Colors.ItemHotBackground2.IsEmpty)
                                    {
                                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                        g.FillRectangle(gradient, r);
                                        gradient.Dispose();
                                    }
                                    else
                                    {
                                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemHotBackground))
                                            g.FillRectangle(mybrush, r);
                                    }
                                }
                                using (Pen mypen = new Pen(pa.Colors.ItemHotBorder, 1))
                                    NativeFunctions.DrawRectangle(g, mypen, r);
                            }
                        }
                        // TODO: Beta 2 Hack for DrawRectangle Possible bug, need to verify
                        //r.Width-=1;
                        //r.Height-=1;
                        //g.DrawRectangle(SystemPens.Highlight,r);
                    }

                    // Image needs shadow when it has focus
                    if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                    {
                        // We needed the image to stay "up" when button is expanded too so we removed the checking here
                        //if(m_MouseDown || (m_Expanded && !bIsOnMenu) || m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                        if (m_MouseDown || m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                        {
                            objImage.DrawImage(g, rect);// g.DrawImage(objImage,rect);//,0,0,imageSize.Width,imageSize.Height,System.Drawing.GraphicsUnit.Pixel);
                        }
                        else
                        {
                            if (NativeFunctions.ColorDepth >= 16 && EffectiveStyle != eDotNetBarStyle.Office2003)
                            {
                                float[][] array = new float[5][];
                                array[0] = new float[5] { 0, 0, 0, 0, 0 };
                                array[1] = new float[5] { 0, 0, 0, 0, 0 };
                                array[2] = new float[5] { 0, 0, 0, 0, 0 };
                                array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                                array[4] = new float[5] { 0, 0, 0, 0, 0 };
                                System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                                System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                                disabledImageAttr.ClearColorKey();
                                disabledImageAttr.SetColorMatrix(grayMatrix);

                                rect.Offset(1, 1);
                                //g.DrawImage(objImage,rect,0,0,objImage.Width,objImage.Height,GraphicsUnit.Pixel,disabledImageAttr);
                                objImage.DrawImage(g, rect, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel, disabledImageAttr);
                                rect.Offset(-2, -2);
                                //g.DrawImage(objImage,rect);
                                objImage.DrawImage(g, rect);
                            }
                            else
                            {
                                if (EffectiveStyle == eDotNetBarStyle.OfficeXP)
                                    rect.Offset(-1, -1);
                                //g.DrawImage(objImage,rect);
                                objImage.DrawImage(g, rect);
                            }
                        }
                    }

                    if (bIsOnMenu && this.IsOnCustomizeMenu && m_Visible && !this.SystemItem)
                    {
                        // Draw check box if this item is visible
                        Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
                        r.Inflate(-1, -1);
                        //Color clr=g.GetNearestColor(Color.FromArgb(45,SystemColors.Highlight));
                        Color clr = pa.Colors.ItemCheckedBackground/*ColorFunctions.CheckBoxBackColor(g)*/;
                        if (mouseOver && !pa.Colors.ItemHotBackground2.IsEmpty)
                        {
                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                            g.FillRectangle(gradient, r);
                            gradient.Dispose();
                        }
                        else
                        {
                            if (mouseOver)
                                clr = pa.Colors.ItemHotBackground;

                            if (!pa.Colors.ItemCheckedBackground2.IsEmpty && !mouseOver)
                            {
                                System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                g.FillRectangle(gradient, r);
                                gradient.Dispose();
                            }
                            else
                            {
                                SolidBrush objBrush = new SolidBrush(clr);
                                g.FillRectangle(objBrush, r);
                                objBrush.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                    {
                        if (!m_Checked || this.IsOnCustomizeMenu)
                        {
                            if (m_HotTrackingStyle != eHotTrackingStyle.Color)
                            {
                                objImage.DrawImage(g, rect);
                            }
                            else
                            {
                                // Draw gray-scale image for this hover style...
                                float[][] array = new float[5][];
                                array[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0, 0 };
                                array[1] = new float[5] { 0.5f, 0.5f, 0.5f, 0, 0 };
                                array[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0, 0 };
                                array[3] = new float[5] { 0, 0, 0, 1, 0 };
                                array[4] = new float[5] { 0.2f, 0.2f, 0.2f, 0, 1 };
                                System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                                System.Drawing.Imaging.ImageAttributes att = new System.Drawing.Imaging.ImageAttributes();
                                att.SetColorMatrix(grayMatrix);
                                //g.DrawImage(objImage,rect,0,0,objImage.Width,objImage.Height,GraphicsUnit.Pixel,att);
                                objImage.DrawImage(g, rect, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel, att);
                            }
                        }
                        else
                        {
                            if ((m_Checked && bIsOnMenu || !this.Expanded) && !this.IsOnCustomizeDialog)
                            {
                                Rectangle r;
                                if (bIsOnMenu)
                                    r = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
                                else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
                                {
                                    r = rect;
                                    r.Inflate(2, 2);
                                }
                                else
                                    r = m_Rect;
                                if (bIsOnMenu)
                                    r.Inflate(-1, -1);
                                Color clr;
                                if (mouseOver && m_HotTrackingStyle != eHotTrackingStyle.None)
                                {
                                    if (m_Checked && !pa.Colors.ItemCheckedBackground2.IsEmpty)
                                    {
                                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                        g.FillRectangle(gradient, r);
                                        gradient.Dispose();
                                        clr = System.Drawing.Color.Empty;
                                    }
                                    else
                                    {
                                        if (!pa.Colors.ItemHotBackground2.IsEmpty)
                                        {
                                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                            g.FillRectangle(gradient, r);
                                            gradient.Dispose();
                                            clr = System.Drawing.Color.Empty;
                                        }
                                        else
                                            clr = System.Windows.Forms.ControlPaint.Dark(pa.Colors.ItemHotBackground);
                                    }
                                }
                                else
                                {
                                    if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
                                    {
                                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                        g.FillRectangle(gradient, r);
                                        gradient.Dispose();
                                        clr = System.Drawing.Color.Empty;
                                    }
                                    else
                                        clr = pa.Colors.ItemCheckedBackground/*ColorFunctions.CheckBoxBackColor(g)*/;
                                }
                                if (!clr.IsEmpty)
                                {
                                    SolidBrush objBrush = new SolidBrush(clr);
                                    g.FillRectangle(objBrush, r);
                                    objBrush.Dispose();
                                }
                            }
                            objImage.DrawImage(g, rect);
                        }
                    }
                    if (bIsOnMenu && this.IsOnCustomizeMenu && m_Visible && !this.SystemItem)
                    {
                        // Draw check box if this item is visible
                        Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
                        r.Inflate(-1, -1);
                        //Color clr=g.GetNearestColor(Color.FromArgb(96,ColorFunctions.HoverBackColor()));
                        if (pa.Colors.ItemCheckedBackground2.IsEmpty)
                        {
                            Color clr = pa.Colors.ItemCheckedBackground/*ColorFunctions.CheckBoxBackColor(g)*/;
                            SolidBrush objBrush = new SolidBrush(clr);
                            g.FillRectangle(objBrush, r);
                            objBrush.Dispose();
                        }
                        else
                        {
                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                            g.FillRectangle(gradient, r);
                            gradient.Dispose();
                        }
                    }
                    else if (m_Checked && !bIsOnMenu && objImage == null)
                    {
                        Rectangle r = m_Rect;
                        // TODO: In 9188 GetNearestColor on the Graphics that were taken directly from Paint event did not work correctly. Check in future versions...
                        // Draw background
                        //Color clr=g.GetNearestColor(Color.FromArgb(96,ColorFunctions.HoverBackColor(g)));
                        Color clr;
                        if (mouseOver && m_HotTrackingStyle != eHotTrackingStyle.None)
                        {
                            if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
                            {
                                System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                g.FillRectangle(gradient, r);
                                gradient.Dispose();
                                clr = System.Drawing.Color.Empty;
                            }
                            else
                            {
                                if (!pa.Colors.ItemHotBackground2.IsEmpty)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                    g.FillRectangle(gradient, r);
                                    gradient.Dispose();
                                    clr = System.Drawing.Color.Empty;
                                }
                                else
                                    clr = pa.Colors.ItemHotBackground;
                            }
                        }
                        else
                        {
                            if (!pa.Colors.ItemCheckedBackground2.IsEmpty)
                            {
                                System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                g.FillRectangle(gradient, r);
                                gradient.Dispose();
                                clr = System.Drawing.Color.Empty;
                            }
                            else
                                clr = pa.Colors.ItemCheckedBackground;
                        }
                        if (!clr.IsEmpty)
                        {
                            SolidBrush objBrush = new SolidBrush(clr);
                            g.FillRectangle(objBrush, r);
                            objBrush.Dispose();
                        }
                    }
                }

                if (bIsOnMenu && this.IsOnCustomizeMenu && m_Visible && !this.SystemItem)
                {
                    Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
                    r.Inflate(-1, -1);
                    //Color clr=g.GetNearestColor(Color.FromArgb(200,SystemColors.Highlight));
                    Color clr = pa.Colors.ItemCheckedBorder/*SystemColors.Highlight*/;
                    Pen objPen = new Pen(clr, 1);
                    // TODO: Beta 2 fix --> g.DrawRectangle(objPen,r);
                    NativeFunctions.DrawRectangle(g, objPen, r);

                    objPen.Dispose();
                    objPen = new Pen(pa.Colors.ItemCheckedText);
                    // Draw checker...
                    Point[] pt = new Point[3];
                    pt[0].X = r.Left + (r.Width - 5) / 2 - 1;
                    pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                    pt[1].X = pt[0].X + 2;
                    pt[1].Y = pt[0].Y + 2;
                    pt[2].X = pt[1].X + 4;
                    pt[2].Y = pt[1].Y - 4;
                    g.DrawLines(objPen/*SystemPens.ControlText*/, pt);
                    pt[0].X++;
                    pt[1].X++;
                    pt[2].X++;
                    g.DrawLines(objPen/*SystemPens.ControlText*/, pt);
                    objPen.Dispose();
                }

                if (m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                {
                    Rectangle r;

                    if (bIsOnMenu)
                        r = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
                    else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
                    {
                        r = rect;
                        r.Inflate(2, 2);
                    }
                    else
                        r = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
                    if (bIsOnMenu)
                        r.Inflate(-1, -1);

                    // Draw line around...
                    if (bIsOnMenu || !this.Expanded)
                    {
                        if (objImage == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways)
                        {
                            if (mouseOver)
                            {
                                if (m_Checked && !pa.Colors.ItemCheckedBackground2.IsEmpty)
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                    g.FillRectangle(gradient, r);
                                    gradient.Dispose();
                                }
                                else
                                {
                                    if (!pa.Colors.ItemHotBackground2.IsEmpty)
                                    {
                                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                        g.FillRectangle(gradient, r);
                                        gradient.Dispose();
                                    }
                                    else
                                    {
                                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemHotBackground))
                                            g.FillRectangle(mybrush, r);
                                    }
                                }
                            }
                            else
                            {
                                if (pa.Colors.ItemCheckedBackground2.IsEmpty)
                                {
                                    using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemCheckedBackground))
                                        g.FillRectangle(mybrush, r);
                                }
                                else
                                {
                                    System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
                                    g.FillRectangle(gradient, r);
                                    gradient.Dispose();
                                }
                            }
                        }

                        //Color clr=g.GetNearestColor(Color.FromArgb(200,SystemColors.Highlight));
                        Color clr = pa.Colors.ItemCheckedBorder/*SystemColors.Highlight*/;
                        Pen objPen = new Pen(clr, 1);
                        // TODO: Beta 2 fix  ---> g.DrawRectangle(objPen,r);
                        NativeFunctions.DrawRectangle(g, objPen, r);

                        objPen.Dispose();
                    }

                    if ((objImage == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && bIsOnMenu)
                    {
                        // Draw checker...
                        Pen pen = new Pen(pa.Colors.ItemCheckedText);
                        Point[] pt = new Point[3];
                        pt[0].X = r.Left + (r.Width - 5) / 2 - 1;
                        pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                        pt[1].X = pt[0].X + 2;
                        pt[1].Y = pt[0].Y + 2;
                        pt[2].X = pt[1].X + 4;
                        pt[2].Y = pt[1].Y - 4;
                        g.DrawLines(pen/*SystemPens.ControlText*/, pt);
                        pt[0].X++;
                        //pt[0].Y
                        pt[1].X++;
                        //pt[1].Y;
                        pt[2].X++;
                        //pt[2].Y;
                        g.DrawLines(pen/*SystemPens.ControlText*/, pt);
                        pen.Dispose();
                    }
                }
            }
            else
            {
                if (bIsOnMenu && mouseOver && m_HotTrackingStyle == eHotTrackingStyle.Default)
                {
                    Rectangle r = new Rectangle(itemRect.Left + 1, itemRect.Top, itemRect.Width - 2, itemRect.Height);
                    using (Pen mypen = new Pen(pa.Colors.ItemHotBorder, 1))
                        NativeFunctions.DrawRectangle(g, mypen, r);
                }

                // Replicated code from above to draw the item checked box
                if (m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                {
                    Rectangle r;

                    if (bIsOnMenu)
                        r = new Rectangle(m_Rect.X + 1, m_Rect.Y, m_ImageDrawRect.Width - 2, m_Rect.Height);
                    else if (m_HotTrackingStyle == eHotTrackingStyle.Image)
                    {
                        r = rect;
                        r.Inflate(2, 2);
                    }
                    else
                        r = new Rectangle(m_Rect.X, m_Rect.Y, m_Rect.Width, m_Rect.Height);
                    if (bIsOnMenu)
                        r.Inflate(-1, -1);

                    // Draw line around...
                    if (bIsOnMenu || !this.Expanded)
                    {
                        Color clr = pa.Colors.ItemDisabledText;
                        Pen objPen = new Pen(clr, 1);
                        NativeFunctions.DrawRectangle(g, objPen, r);
                        objPen.Dispose();
                    }

                    if ((objImage == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && bIsOnMenu)
                    {
                        // Draw checker...
                        Pen pen = new Pen(pa.Colors.ItemDisabledText);
                        Point[] pt = new Point[3];
                        pt[0].X = r.Left + (r.Width - 5) / 2 - 1;
                        pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
                        pt[1].X = pt[0].X + 2;
                        pt[1].Y = pt[0].Y + 2;
                        pt[2].X = pt[1].X + 4;
                        pt[2].Y = pt[1].Y - 4;
                        g.DrawLines(pen, pt);
                        pt[0].X++;
                        //pt[0].Y
                        pt[1].X++;
                        //pt[1].Y;
                        pt[2].X++;
                        //pt[2].Y;
                        g.DrawLines(pen, pt);
                        pen.Dispose();
                    }
                }
                textColor = pa.Colors.ItemDisabledText;
                if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                {
                    // Draw disabled image
                    objImage.DrawImage(g, rect);
                }
            }

            // Draw menu item text
            if (bIsOnMenu || m_ButtonStyle != eButtonStyle.Default || objImage == null || (!bIsOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)) /*|| !this.IsOnBar*/) // Commented out becouse it caused text to be drawn if item is not on bar no matter what
            {
                if (bIsOnMenu)
                    rect = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, itemRect.Width - m_ImageDrawRect.Right - 26, m_TextDrawRect.Height);
                else
                {
                    rect = m_TextDrawRect;
                    if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
                    {
                        if (m_Orientation == eOrientation.Vertical)
                        {
                            rect = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, m_TextDrawRect.Width, m_TextDrawRect.Height);
                        }
                        else
                        {
                            rect = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, itemRect.Width, m_TextDrawRect.Height);
                            if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems)
                                rect.Width -= 10;
                        }

                        objStringFormat |= eTextFormat.HorizontalCenter;
                    }
                    else if (bIsOnMenuBar && objImage == null)
                        objStringFormat |= eTextFormat.HorizontalCenter;

                    if (m_MouseDown && m_HotTrackingStyle != eHotTrackingStyle.Image)
                    {
                        if (m_ForeColor.IsEmpty)
                        {
                            textColor = pa.Colors.ItemPressedText;
                        }
                        else
                        {
                            textColor = System.Windows.Forms.ControlPaint.Light(m_ForeColor);
                        }
                    }
                }

                rect.Offset(itemRect.Left, itemRect.Top);

                if (m_Orientation == eOrientation.Vertical && !bIsOnMenu)
                {
                    g.RotateTransform(90);
                    TextDrawing.DrawStringLegacy(g, m_Text, objFont, textColor, new Rectangle(rect.Top, -rect.Right, rect.Height, rect.Width), objStringFormat);
                    g.ResetTransform();
                }
                else
                {
                    if (rect.Right > m_Rect.Right)
                        rect.Width = m_Rect.Right - rect.Left;
                    TextDrawing.DrawString(g, m_Text, objFont, textColor, rect, objStringFormat);
                    if (!this.DesignMode && this.Focused && !bIsOnMenu && !bIsOnMenuBar)
                    {
                        //SizeF szf=g.MeasureString(m_Text,objFont,rect.Width,objStringFormat);
                        Rectangle r = rect;
                        //r.Width=(int)Math.Ceiling(szf.Width);
                        //r.Height=(int)Math.Ceiling(szf.Height);
                        //r.Inflate(1,1);
                        System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, r);
                    }
                }

            }

            // Draw Shortcut text if needed
            if (this.DrawShortcutText != "" && bIsOnMenu && !this.IsOnCustomizeDialog)
            {
                objStringFormat |= eTextFormat.HidePrefix | eTextFormat.Right;
                TextDrawing.DrawString(g, this.DrawShortcutText, objFont, textColor, rect, objStringFormat);
            }

            // If it has subitems draw the triangle to indicate that
            if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems)
            {
                if (bIsOnMenu)
                {
                    Point[] p = new Point[3];
                    p[0].X = itemRect.Left + itemRect.Width - 12;
                    p[0].Y = itemRect.Top + (itemRect.Height - 8) / 2;
                    p[1].X = p[0].X;
                    p[1].Y = p[0].Y + 8;
                    p[2].X = p[0].X + 4;
                    p[2].Y = p[0].Y + 4;
                    using (SolidBrush brush = new SolidBrush(textColor))
                        g.FillPolygon(brush, p);
                }
                else if (!m_SubItemsRect.IsEmpty)
                {
                    if (GetEnabled(pa.ContainerControl) && ((mouseOver || m_Checked) && !m_Expanded && m_HotTrackingStyle != eHotTrackingStyle.None && m_HotTrackingStyle != eHotTrackingStyle.Image))
                    {
                        if (m_Orientation == eOrientation.Horizontal)
                        {
                            using (Pen mypen = new Pen(pa.Colors.ItemHotBorder))
                                g.DrawLine(mypen/*SystemPens.Highlight*/, itemRect.Left + m_SubItemsRect.Left, itemRect.Top, itemRect.Left + m_SubItemsRect.Left, itemRect.Bottom - 1);
                        }
                        else
                        {
                            using (Pen mypen = new Pen(pa.Colors.ItemHotBorder))
                                g.DrawLine(mypen/*SystemPens.Highlight*/, itemRect.Left, itemRect.Top + m_SubItemsRect.Top, itemRect.Right - 2, itemRect.Top + m_SubItemsRect.Top);
                        }
                    }
                    Point[] p = new Point[3];
                    if (this.PopupSide == ePopupSide.Default)
                    {
                        if (m_Orientation == eOrientation.Horizontal)
                        {
                            p[0].X = itemRect.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
                            p[0].Y = itemRect.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
                            p[1].X = p[0].X + 5;
                            p[1].Y = p[0].Y;
                            p[2].X = p[0].X + 2;
                            p[2].Y = p[0].Y + 3;
                        }
                        else
                        {
                            p[0].X = itemRect.Left + (m_SubItemsRect.Width - 3) / 2 + 1;
                            p[0].Y = itemRect.Top + m_SubItemsRect.Top + (m_SubItemsRect.Height - 5) / 2;
                            p[1].X = p[0].X;
                            p[1].Y = p[0].Y + 6;
                            p[2].X = p[0].X - 3;
                            p[2].Y = p[0].Y + 3;
                        }
                    }
                    else
                    {
                        switch (this.PopupSide)
                        {
                            case ePopupSide.Left:
                                {
                                    p[0].X = itemRect.Left + m_SubItemsRect.Left + m_SubItemsRect.Width / 2;
                                    p[0].Y = itemRect.Top + m_SubItemsRect.Height / 2 - 3;
                                    p[1].X = p[0].X;
                                    p[1].Y = p[0].Y + 6;
                                    p[2].X = p[0].X + 3;
                                    p[2].Y = p[0].Y + 3;
                                    break;
                                }
                            case ePopupSide.Right:
                                {
                                    p[0].X = itemRect.Left + m_SubItemsRect.Left + m_SubItemsRect.Width / 2 + 3;
                                    p[0].Y = itemRect.Top + m_SubItemsRect.Height / 2 - 3;
                                    p[1].X = p[0].X;
                                    p[1].Y = p[0].Y + 6;
                                    p[2].X = p[0].X - 3;
                                    p[2].Y = p[0].Y + 3;
                                    break;
                                }
                            case ePopupSide.Top:
                                {
                                    p[0].X = itemRect.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
                                    p[0].Y = itemRect.Top + (m_SubItemsRect.Height - 3) / 2 + 4;
                                    p[1].X = p[0].X + 6;
                                    p[1].Y = p[0].Y;
                                    p[2].X = p[0].X + 3;
                                    p[2].Y = p[0].Y - 4;
                                    break;
                                }
                            case ePopupSide.Bottom:
                                {
                                    p[0].X = itemRect.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2 + 1;
                                    p[0].Y = itemRect.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
                                    p[1].X = p[0].X + 5;
                                    p[1].Y = p[0].Y;
                                    p[2].X = p[0].X + 2;
                                    p[2].Y = p[0].Y + 3;
                                    break;
                                }
                        }
                    }
                    if (GetEnabled(pa.ContainerControl))
                    {
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemText))
                            g.FillPolygon(mybrush/*SystemBrushes.ControlText*/, p);
                    }
                    else
                    {
                        using (SolidBrush mybrush = new SolidBrush(pa.Colors.ItemDisabledText))
                            g.FillPolygon(mybrush/*SystemBrushes.ControlDark*/, p);
                    }
                }
            }

            if (this.Focused && this.DesignMode)
            {
                Rectangle r = itemRect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
            }

            if (objImage != null)
                objImage.Dispose();
        }

        internal bool IgnoreAlpha
        {
            get { return m_IgnoreAlpha; }
            set { m_IgnoreAlpha = value; }
        }

        private void PaintThemed(ItemPaintArgs pa)
        {
            System.Drawing.Graphics g = pa.Graphics;
            ThemeToolbar theme = pa.ThemeToolbar;
            ThemeToolbarParts part = ThemeToolbarParts.Button;
            ThemeToolbarStates state = ThemeToolbarStates.Normal;
            eTextFormat format = pa.ButtonStringFormat; //GetStringFormat();
            Color textColor = SystemColors.ControlText;

            Rectangle rectImage = Rectangle.Empty;
            Rectangle itemRect = m_Rect;

            Font font = null;
            CompositeImage image = GetImage();

            if (m_Font != null)
                font = m_Font;
            else
                font = GetFont(pa, false);

            bool bSplitButton = (this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems && !m_SubItemsRect.IsEmpty;

            if (bSplitButton)
                part = ThemeToolbarParts.SplitButton;

            // Calculate image position
            if (image != null)
            {
                if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
                    rectImage = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, itemRect.Width, m_ImageDrawRect.Height);
                else
                    rectImage = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, m_ImageDrawRect.Height);

                rectImage.Offset(itemRect.Left, itemRect.Top);
                rectImage.Offset((rectImage.Width - this.ImageSize.Width) / 2, (rectImage.Height - this.ImageSize.Height) / 2);
                rectImage.Width = this.ImageSize.Width;
                rectImage.Height = this.ImageSize.Height;
            }

            // Set the state and text brush
            if (!GetEnabled(pa.ContainerControl))
            {
                state = ThemeToolbarStates.Disabled;
                textColor = pa.Colors.ItemDisabledText;
            }
            else if (m_MouseDown)
            {
                state = ThemeToolbarStates.Pressed;
                textColor = pa.Colors.ItemPressedText;
            }
            else if (m_MouseOver && m_Checked)
            {
                state = ThemeToolbarStates.HotChecked;
                textColor = pa.Colors.ItemHotText;
            }
            else if (m_MouseOver || m_Expanded)
            {
                state = ThemeToolbarStates.Hot;
                textColor = pa.Colors.ItemHotText;
            }
            else if (m_Checked)
            {
                state = ThemeToolbarStates.Checked;
                textColor = pa.Colors.ItemCheckedText;
            }
            else
                textColor = pa.Colors.ItemText;

            Rectangle backRect = m_Rect;
            if (m_HotTrackingStyle == eHotTrackingStyle.Image && image != null)
            {
                backRect = rectImage;
                backRect.Inflate(3, 3);
            }
            else if (bSplitButton)
            {
                backRect.Width = backRect.Width - m_SubItemsRect.Width;
            }

            // Draw Button Background
            if (m_HotTrackingStyle != eHotTrackingStyle.None)
            {
                theme.DrawBackground(g, part, state, backRect);
            }

            // Draw Image
            if (image != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
            {
                if (state == ThemeToolbarStates.Normal && m_HotTrackingStyle == eHotTrackingStyle.Color)
                {
                    // Draw gray-scale image for this hover style...
                    float[][] array = new float[5][];
                    array[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0, 0 };
                    array[1] = new float[5] { 0.5f, 0.5f, 0.5f, 0, 0 };
                    array[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0, 0 };
                    array[3] = new float[5] { 0, 0, 0, 1, 0 };
                    array[4] = new float[5] { 0.2f, 0.2f, 0.2f, 0, 1 };
                    System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                    System.Drawing.Imaging.ImageAttributes att = new System.Drawing.Imaging.ImageAttributes();
                    att.SetColorMatrix(grayMatrix);
                    //g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,att);
                    image.DrawImage(g, rectImage, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att);
                }
                else if (state == ThemeToolbarStates.Normal && !image.IsIcon)
                {
                    // Draw image little bit lighter, I decied to use gamma it is easy
                    System.Drawing.Imaging.ImageAttributes lightImageAttr = new System.Drawing.Imaging.ImageAttributes();
                    lightImageAttr.SetGamma(.7f, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                    //g.DrawImage(image,rectImage,0,0,image.Width,image.Height,GraphicsUnit.Pixel,lightImageAttr);
                    image.DrawImage(g, rectImage, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, lightImageAttr);
                }
                else
                {
                    image.DrawImage(g, rectImage);
                }
            }

            // Draw Text
            if (m_ButtonStyle == eButtonStyle.ImageAndText || m_ButtonStyle == eButtonStyle.TextOnlyAlways || image == null /*|| !this.IsOnBar*/) // Commented out becouse it caused text to be drawn if item is not on bar no matter what
            {
                Rectangle rectText = m_TextDrawRect;
                if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
                {
                    if (m_Orientation == eOrientation.Vertical)
                    {
                        rectText = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, m_TextDrawRect.Width, m_TextDrawRect.Height);
                    }
                    else
                    {
                        rectText = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, itemRect.Width, m_TextDrawRect.Height);
                        if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems)
                            rectText.Width -= 10;
                    }
                    format |= eTextFormat.HorizontalCenter;
                }

                rectText.Offset(itemRect.Left, itemRect.Top);

                if (m_Orientation == eOrientation.Vertical)
                {
                    g.RotateTransform(90);
                    TextDrawing.DrawStringLegacy(g, m_Text, font, textColor, new Rectangle(rectText.Top, -rectText.Right, rectText.Height, rectText.Width), format);
                    g.ResetTransform();
                }
                else
                {
                    if (rectText.Right > m_Rect.Right)
                        rectText.Width = m_Rect.Right - rectText.Left;
                    TextDrawing.DrawString(g, m_Text, font, textColor, rectText, format);
                    if (!this.DesignMode && this.Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
                    {
                        //SizeF szf=g.MeasureString(m_Text,font,rectText.Width,format);
                        Rectangle r = rectText;
                        //r.Width=(int)Math.Ceiling(szf.Width);
                        //r.Height=(int)Math.Ceiling(szf.Height);
                        //r.Inflate(1,1);
                        System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, r);
                    }
                }
            }

            // If it has subitems draw the triangle to indicate that
            if (bSplitButton)
            {
                part = ThemeToolbarParts.SplitButtonDropDown;

                if (!GetEnabled(pa.ContainerControl))
                    state = ThemeToolbarStates.Disabled;
                else
                    state = ThemeToolbarStates.Normal;

                if (m_HotTrackingStyle != eHotTrackingStyle.None && m_HotTrackingStyle != eHotTrackingStyle.Image && GetEnabled(pa.ContainerControl))
                {
                    if (m_Expanded || m_MouseDown)
                        state = ThemeToolbarStates.Pressed;
                    else if (m_MouseOver && m_Checked)
                        state = ThemeToolbarStates.HotChecked;
                    else if (m_Checked)
                        state = ThemeToolbarStates.Checked;
                    else if (m_MouseOver)
                        state = ThemeToolbarStates.Hot;
                }

                if (m_Orientation == eOrientation.Horizontal)
                {
                    Rectangle r = m_SubItemsRect;
                    r.Offset(itemRect.X, itemRect.Y);
                    theme.DrawBackground(g, part, state, r);
                }
                else
                {
                    Rectangle r = m_SubItemsRect;
                    r.Offset(itemRect.X, itemRect.Y);
                    theme.DrawBackground(g, part, state, r);
                }
                //g.DrawLine(new Pen(pa.Colors.ItemHotBorder)/*SystemPens.Highlight*/,itemRect.Left,itemRect.Top+m_SubItemsRect.Top,itemRect.Right-2,itemRect.Top+m_SubItemsRect.Top);
            }

            if (this.Focused && this.DesignMode)
            {
                Rectangle r = itemRect;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
            }

            if (image != null)
                image.Dispose();
        }

        private void PaintOffice(ItemPaintArgs pa)
        {
            System.Drawing.Graphics g = pa.Graphics;
            Rectangle rect = Rectangle.Empty;
            Rectangle itemRect = m_Rect;
            Rectangle rTmp = Rectangle.Empty;
            Color textColor = SystemColors.ControlText;
            Color color3d = SystemColors.Control;

            if (m_Parent is GenericItemContainer && !((GenericItemContainer)m_Parent).BackColor.IsEmpty)
                color3d = ((GenericItemContainer)m_Parent).BackColor;
            else if (m_Parent is SideBarPanelItem && !((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.IsEmpty)
                color3d = ((SideBarPanelItem)m_Parent).BackgroundStyle.BackColor1.GetCompositeColor();

            if (m_MouseOver && !m_HotForeColor.IsEmpty)
                textColor = m_HotForeColor;
            else if (!m_ForeColor.IsEmpty)
                textColor = m_ForeColor;

            Font objFont = null;
            bool bIsOnMenu = pa.IsOnMenu;
            bool buttonX = pa.ContainerControl is ButtonX;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            //g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            eTextFormat objStringFormat = pa.ButtonStringFormat;
            CompositeImage objImage = GetImage();

            if (m_Font != null)
                objFont = m_Font;
            else
                objFont = GetFont(pa, false);

            // Calculate image position
            if (objImage != null)
            {

                if (!bIsOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom))
                    rect = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y + 1, itemRect.Width, m_ImageDrawRect.Height);
                else
                    rect = m_ImageDrawRect; //new Rectangle(m_ImageDrawRect.X,m_ImageDrawRect.Y,m_ImageDrawRect.Width,m_ImageDrawRect.Height);

                rect.Offset(itemRect.Left, itemRect.Top);
                //if(m_ButtonType==eButtonType.StateButton)
                //	rect.Offset(STATEBUTTON_SPACING+(m_ImageSize.Width-STATEBUTTON_SPACING)/2+(rect.Width-m_Image.Width*2)/2,(rect.Height-m_Image.Height)/2);
                //else
                rect.Offset((rect.Width - this.ImageSize.Width) / 2, (rect.Height - this.ImageSize.Height) / 2);

                rect.Width = this.ImageSize.Width;
                rect.Height = this.ImageSize.Height;
            }

            // Draw background
            if (bIsOnMenu && !this.DesignMode && this.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
                g.FillRectangle(new SolidBrush(ColorFunctions.RecentlyUsedOfficeBackColor()), m_Rect);
            else if (buttonX)
            {
                ButtonState state = ButtonState.Normal;
                if (m_MouseDown)
                    state = ButtonState.Pushed;
                else if (m_Checked || m_Expanded)
                    state = ButtonState.Checked;
                ControlPaint.DrawButton(g, itemRect, state);
            }
            //else
            //	g.FillRectangle(SystemBrushes.Control,m_Rect);

            if (GetEnabled(pa.ContainerControl) || this.DesignMode)
            {
                if (m_Expanded && !bIsOnMenu)
                {
                    // Office 2000 Style
                    g.FillRectangle(SystemBrushes.Control, itemRect);
                    //System.Windows.Forms.ControlPaint.DrawBorder3D(g,itemRect,System.Windows.Forms.Border3DStyle.SunkenOuter,System.Windows.Forms.Border3DSide.All);
                    BarFunctions.DrawBorder3D(g, itemRect, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All, color3d);
                }

                if ((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded || m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                {
                    //if(m_ButtonType!=eButtonType.Label)
                    {
                        // Draw Mouse over marker
                        if (bIsOnMenu || this.IsOnCustomizeDialog)
                        {
                            if ((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded)
                            {
                                if (!(m_MouseOver && this.DesignMode) || this.IsOnCustomizeDialog)
                                    g.FillRectangle(SystemBrushes.Highlight, itemRect);
                            }
                        }
                        else
                        {
                            if (m_MouseDown || (m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog) || this.Expanded && (!this.ShowSubItems || this.IsOnMenuBar))
                            {
                                if (m_SubItemsRect.IsEmpty)
                                {
                                    if (!buttonX)
                                    {
                                        BarFunctions.DrawBorder3D(g, itemRect, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All, color3d);
                                        if (m_Checked && !m_MouseOver && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                                        {
                                            Rectangle r = itemRect; // Rectangle(itemRect.X,itemRect.Y,itemRect.Width,itemRect.Height);
                                            r.Inflate(-1, -1);
                                            g.FillRectangle(ColorFunctions.GetPushedBrush(this), r);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!buttonX)
                                    {
                                        Rectangle r;
                                        if (m_Orientation == eOrientation.Horizontal)
                                            r = new Rectangle(itemRect.X, itemRect.Y, itemRect.Width - m_SubItemsRect.Width, itemRect.Height);
                                        else
                                            r = new Rectangle(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height - m_SubItemsRect.Height);
                                        //System.Windows.Forms.ControlPaint.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.SunkenOuter,System.Windows.Forms.Border3DSide.All);
                                        BarFunctions.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All, color3d);
                                        if (!m_MouseOver)
                                        {
                                            r.Inflate(-1, -1);
                                            g.FillRectangle(ColorFunctions.GetPushedBrush(this), r);
                                        }
                                    }
                                }
                            }
                            else if (!this.DesignMode)
                            {
                                if (m_SubItemsRect.IsEmpty)
                                {
                                    if (!buttonX)
                                        BarFunctions.DrawBorder3D(g, itemRect, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.All, color3d);
                                }
                                else
                                {
                                    Rectangle r;
                                    if (m_Orientation == eOrientation.Horizontal)
                                        r = new Rectangle(itemRect.X, itemRect.Y, itemRect.Width - m_SubItemsRect.Width, itemRect.Height);
                                    else
                                        r = new Rectangle(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height - m_SubItemsRect.Height);
                                    //System.Windows.Forms.ControlPaint.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
                                    BarFunctions.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.All, color3d);
                                }
                            }

                        }

                        // TODO: Add support for Checked buttons etc...
                        if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                        {
                            //if(m_ButtonType==eButtonType.StateButton)
                            //	rTmp=new Rectangle(m_ImageDrawRect.X,m_ImageDrawRect.Y,itemRect.Height,itemRect.Height);
                            //else
                            rTmp = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, itemRect.Height);
                            //if(m_ImagePosition==eImagePosition.Right)
                            //	rTmp.X=itemRect.Width-21-m_ImageDrawRect.Width;

                            rTmp.Offset(itemRect.Left, itemRect.Top);
                            if (bIsOnMenu)
                            {
                                if (m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog)
                                {
                                    //System.Windows.Forms.ControlPaint.DrawBorder3D(g,rTmp,System.Windows.Forms.Border3DStyle.SunkenOuter,System.Windows.Forms.Border3DSide.All);
                                    BarFunctions.DrawBorder3D(g, rTmp, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All, color3d);
                                    if (!m_MouseOver)
                                    {
                                        rTmp.Inflate(-1, -1);
                                        g.FillRectangle(ColorFunctions.GetPushedBrush(this), rTmp);
                                    }
                                    /*if(m_ButtonType==eButtonType.StateButton)
                                    {
                                        // Draw checker...
                                        Point[] pt=new Point[3];
                                        pt[0].X=rTmp.Left+(rTmp.Width-5)/2;
                                        pt[0].Y=rTmp.Top+(rTmp.Height-6)/2+3;
                                        pt[1].X=pt[0].X+2;
                                        pt[1].Y=pt[0].Y+2;
                                        pt[2].X=pt[1].X+4;
                                        pt[2].Y=pt[1].Y-4;
                                        g.DrawLines(SystemPens.ControlText,pt);
                                        pt[0].X++;
                                        //pt[0].Y
                                        pt[1].X++;
                                        //pt[1].Y;
                                        pt[2].X++;
                                        //pt[2].Y;
                                        g.DrawLines(SystemPens.ControlText,pt);
                                    }*/
                                }
                                else
                                {
                                    //System.Windows.Forms.ControlPaint.DrawBorder3D(g,rTmp,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
                                    BarFunctions.DrawBorder3D(g, rTmp, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.All, color3d);
                                }
                                /*if(m_ButtonType==eButtonType.StateButton && m_MouseOver)
                                {
                                    rTmp=new Rectangle(rTmp.Right+STATEBUTTON_SPACING,rTmp.Top,m_ImageDrawRect.Width-rTmp.Width-STATEBUTTON_SPACING,itemRect.Height);
                                    System.Windows.Forms.ControlPaint.DrawBorder3D(g,rTmp,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
                                }*/
                            }
                            else
                            {
                                if (m_MouseDown)
                                    rect.Offset(1, 1);
                            }
                            //g.DrawImage(objImage,rect,0,0,objImage.Width,objImage.Height,System.Drawing.GraphicsUnit.Pixel);
                            objImage.DrawImage(g, rect);
                        }
                        else if ((objImage == null || m_ButtonStyle == eButtonStyle.TextOnlyAlways) && m_Checked && !this.IsOnCustomizeMenu && !this.IsOnCustomizeDialog && bIsOnMenu)
                        {
                            // Draw checked box
                            rTmp = new Rectangle(m_ImageDrawRect.X, m_ImageDrawRect.Y, m_ImageDrawRect.Width, itemRect.Height);
                            rTmp.Offset(itemRect.Left, itemRect.Top);
                            DrawOfficeCheckBox(g, rTmp);
                        }
                    }
                    //else
                    //	g.FillRectangle(SystemBrushes.Highlight,itemRect);
                }
                else
                {
                    if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                    {
                        if (m_HotTrackingStyle != eHotTrackingStyle.Color)
                        {
                            //g.DrawImage(objImage,rect,0,0,objImage.Width,objImage.Height,System.Drawing.GraphicsUnit.Pixel);
                            objImage.DrawImage(g, rect);
                        }
                        else if (m_HotTrackingStyle == eHotTrackingStyle.Color)
                        {
                            // Draw gray-scale image for this hover style...
                            float[][] array = new float[5][];
                            array[0] = new float[5] { 0.2125f, 0.2125f, 0.2125f, 0, 0 };
                            array[1] = new float[5] { 0.5f, 0.5f, 0.5f, 0, 0 };
                            array[2] = new float[5] { 0.0361f, 0.0361f, 0.0361f, 0, 0 };
                            array[3] = new float[5] { 0, 0, 0, 1, 0 };
                            array[4] = new float[5] { 0.2f, 0.2f, 0.2f, 0, 1 };
                            System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                            System.Drawing.Imaging.ImageAttributes att = new System.Drawing.Imaging.ImageAttributes();
                            att.SetColorMatrix(grayMatrix);
                            //g.DrawImage(objImage,rect,0,0,objImage.Width,objImage.Height,GraphicsUnit.Pixel,att);
                            objImage.DrawImage(g, rect, 0, 0, objImage.Width, objImage.Height, GraphicsUnit.Pixel, att);
                        }
                        //g.CompositingMode=System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    }
                }

                if (bIsOnMenu && this.IsOnCustomizeMenu && m_Visible && !this.SystemItem)
                {
                    Rectangle r = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Height, m_Rect.Height);
                    //r.Inflate(-1,-1);
                    DrawOfficeCheckBox(g, r);
                }
            }
            else
            {
                textColor = SystemColors.ControlDark;
                if (objImage != null && m_ButtonStyle != eButtonStyle.TextOnlyAlways)
                {
                    // Draw disabled image
                    //g.DrawImage(objImage,rect);
                    objImage.DrawImage(g, rect);
                }
            }

            // Draw menu item text
            if (bIsOnMenu || m_ButtonStyle != eButtonStyle.Default || objImage == null || (!bIsOnMenu && (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)))
            {
                if (bIsOnMenu)
                    rect = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, itemRect.Width - m_ImageDrawRect.Right - 24, m_TextDrawRect.Height);
                else
                {
                    rect = m_TextDrawRect;
                    if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Bottom)
                    {
                        if (m_Orientation == eOrientation.Horizontal)
                            rect = new Rectangle(m_TextDrawRect.X, m_TextDrawRect.Y, itemRect.Width, m_TextDrawRect.Height);
                        if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && m_Orientation == eOrientation.Horizontal && this.ShowSubItems)
                            rect.Width -= 12;

                        objStringFormat |= eTextFormat.HorizontalCenter;
                    }
                    //else
                    //rect=new Rectangle(m_TextDrawRect.X,m_TextDrawRect.Y,itemRect.Width-m_ImageDrawRect.Width,m_TextDrawRect.Height);
                }

                if (((m_MouseOver && m_HotTrackingStyle != eHotTrackingStyle.None) || m_Expanded) && bIsOnMenu && !this.DesignMode)
                    textColor = SystemColors.HighlightText;

                rect.Offset(itemRect.Left, itemRect.Top);

                if (!bIsOnMenu && (m_MouseDown || (this.Expanded && this.IsOnMenuBar)))
                    rect.Offset(1, 1);

                if (buttonX)
                {
                    objStringFormat |= eTextFormat.HorizontalCenter;
                    rect.Height--;
                }
                if (GetEnabled(pa.ContainerControl) || this.DesignMode)
                {
                    if (m_Orientation == eOrientation.Vertical && !bIsOnMenu)
                    {
                        g.RotateTransform(90);
                        TextDrawing.DrawStringLegacy(g, m_Text, objFont, textColor, new Rectangle(rect.Top, -rect.Right, rect.Height, rect.Width), objStringFormat);
                        g.ResetTransform();
                    }
                    else
                    {
                        TextDrawing.DrawString(g, m_Text, objFont, textColor, rect, objStringFormat);
                        if (!this.DesignMode && this.Focused && !bIsOnMenu && !pa.IsOnMenuBar)
                        {
                            //SizeF szf=g.MeasureString(m_Text,objFont,rect.Width,objStringFormat);
                            Rectangle r = rect;
                            //r.Width=(int)Math.Ceiling(szf.Width);
                            //r.Height=(int)Math.Ceiling(szf.Height);
                            //r.Inflate(1,1);
                            System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, r);
                        }
                    }
                }
                else
                {
                    if (m_Orientation == eOrientation.Vertical && !bIsOnMenu)
                    {
                        g.RotateTransform(90);
                        System.Windows.Forms.ControlPaint.DrawStringDisabled(g, m_Text, objFont, SystemColors.Control, new Rectangle(rect.Top, -rect.Right, rect.Height, rect.Width), TextDrawing.GetStringFormat(objStringFormat));
                        g.ResetTransform();
                    }
                    else
                        System.Windows.Forms.ControlPaint.DrawStringDisabled(g, m_Text, objFont, SystemColors.Control, rect, TextDrawing.GetStringFormat(objStringFormat));
                }
            }

            // Draw Shortcut text if needed
            if (this.DrawShortcutText != "" && bIsOnMenu)
            {
                objStringFormat |= eTextFormat.HidePrefix | eTextFormat.Right;
                if (GetEnabled(pa.ContainerControl) || this.DesignMode)
                    TextDrawing.DrawString(g, this.DrawShortcutText, objFont, textColor, rect, objStringFormat);
                else
                    System.Windows.Forms.ControlPaint.DrawStringDisabled(g, this.DrawShortcutText, objFont, SystemColors.Control, rect, TextDrawing.GetStringFormat(objStringFormat));
            }

            // If it has subitems draw the triangle to indicate that
            if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems)
            {
                if (bIsOnMenu)
                {
                    Point[] p = new Point[3];
                    p[0].X = itemRect.Left + itemRect.Width - 12;
                    p[0].Y = itemRect.Top + (itemRect.Height - 8) / 2;
                    p[1].X = p[0].X;
                    p[1].Y = p[0].Y + 8;
                    p[2].X = p[0].X + 4;
                    p[2].Y = p[0].Y + 4;
                    using (SolidBrush brush = new SolidBrush(textColor))
                        g.FillPolygon(brush, p);
                }
                else if (!m_SubItemsRect.IsEmpty)
                {
                    if (m_MouseOver && !m_Expanded && m_HotTrackingStyle != eHotTrackingStyle.None)
                    {
                        Rectangle r = new Rectangle(m_SubItemsRect.X, m_SubItemsRect.Y, m_SubItemsRect.Width, m_SubItemsRect.Height);
                        r.Offset(itemRect.X, itemRect.Y);
                        //System.Windows.Forms.ControlPaint.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
                        BarFunctions.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.RaisedInner, System.Windows.Forms.Border3DSide.All, color3d);
                    }
                    else if (m_Expanded)
                    {
                        Rectangle r = new Rectangle(m_SubItemsRect.X, m_SubItemsRect.Y, m_SubItemsRect.Width, m_SubItemsRect.Height);
                        r.Offset(itemRect.X, itemRect.Y);
                        //System.Windows.Forms.ControlPaint.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.SunkenOuter,System.Windows.Forms.Border3DSide.All);
                        BarFunctions.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All, color3d);
                    }
                    Point[] p = new Point[3];
                    if (m_Orientation == eOrientation.Horizontal)
                    {
                        p[0].X = itemRect.Left + m_SubItemsRect.Left + (m_SubItemsRect.Width - 5) / 2;
                        p[0].Y = itemRect.Top + (m_SubItemsRect.Height - 3) / 2 + 1;
                        p[1].X = p[0].X + 5;
                        p[1].Y = p[0].Y;
                        p[2].X = p[0].X + 2;
                        p[2].Y = p[0].Y + 3;
                    }
                    else
                    {
                        p[0].X = itemRect.Left + (m_SubItemsRect.Width - 3) / 2 + 1;
                        p[0].Y = itemRect.Top + m_SubItemsRect.Top + (m_SubItemsRect.Height - 5) / 2;
                        p[1].X = p[0].X;
                        p[1].Y = p[0].Y + 6;
                        p[2].X = p[0].X - 3;
                        p[2].Y = p[0].Y + 3;
                    }
                    g.FillPolygon(SystemBrushes.ControlText, p);
                }
            }

            if (this.Focused && this.DesignMode)
            {
                Rectangle r = itemRect;
                r.Inflate(-1, -1);
                g.DrawRectangle(new Pen(SystemColors.ControlText, 2), r);
            }

            if (objImage != null)
                objImage.Dispose();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Image GetItemImage()
        {
            CompositeImage image = GetImage();
            if (image != null)
                return image.Image;
            return null;
        }

        internal CompositeImage GetImage()
        {
            if (!GetEnabled() && !this.IsOnCustomizeDialog) // Issue: B1002
                return GetImage(ImageState.Disabled);
            else if (m_MouseDown || this.Checked || this.Expanded && m_HotTrackingStyle == eHotTrackingStyle.Image)
                return GetImage(ImageState.Pressed);
            else if (m_MouseOver)
                return GetImage(ImageState.Hover);
            return GetImage(ImageState.Default);
        }
        internal CompositeImage GetImage(ImageState state)
        {
            Image image = null;
            if (state == ImageState.Disabled && (m_DisabledImage != null || m_DisabledImageIndex >= 0 || m_DisabledIcon != null || m_Image != null || m_ImageIndex >= 0 || m_Icon != null))
            {
                if (m_DisabledImage != null)
                    return new CompositeImage(m_DisabledImage, false, m_ImageSizeOverride);
                else if (m_DisabledIcon != null)
                    return new CompositeImage(m_DisabledIcon, false, m_ImageSizeOverride);
                if (m_DisabledImageIndex >= 0)
                {
                    DisposedCachedImageListImage();
                    image = GetImageFromImageList(m_DisabledImageIndex);
                    if (image != null)
                        return new CompositeImage(image, (image != m_ImageCachedIdx), m_ImageSizeOverride);
                    return null;
                }

                CreateDisabledImage();

                if (m_DisabledImage != null)
                    return new CompositeImage(m_DisabledImage, false, m_ImageSizeOverride);
                else if (m_DisabledIcon != null)
                    return new CompositeImage(m_DisabledIcon, false, m_ImageSizeOverride);
                else
                    return null;
            }

            if (m_Icon != null)
            {
                System.Drawing.Size iconSize = this.IconSize;
                System.Drawing.Icon icon = null;
                try
                {
                    icon = new System.Drawing.Icon(m_Icon, iconSize);
                }
                catch { icon = null; }
                if (icon == null)
                    return new CompositeImage(m_Icon, false, m_ImageSizeOverride);
                else
                    return new CompositeImage(icon, true, m_ImageSizeOverride);
            }

            if (state == ImageState.Hover && (m_HoverImage != null || m_HoverImageIndex >= 0))
            {
                if (m_HoverImage != null)
                    return new CompositeImage(m_HoverImage, false, m_ImageSizeOverride);
                if (m_HoverImageIndex >= 0)
                {
                    DisposedCachedImageListImage();
                    image = GetImageFromImageList(m_HoverImageIndex);
                    if (image != null)
                        return new CompositeImage(image, (image != m_ImageCachedIdx), m_ImageSizeOverride);
                    return null;
                }
            }

            if (state == ImageState.Pressed && (m_PressedImage != null || m_PressedImageIndex >= 0))
            {
                if (m_PressedImage != null)
                    return new CompositeImage(m_PressedImage, false, m_ImageSizeOverride);
                if (m_PressedImageIndex >= 0)
                {
                    DisposedCachedImageListImage();
                    image = GetImageFromImageList(m_PressedImageIndex);
                    if (image != null)
                        return new CompositeImage(image, (image != m_ImageCachedIdx), m_ImageSizeOverride);
                    return null;
                }
            }

            if (m_ImageSmall != null && (UseSmallImageResolved || m_ImageSizeOverride.Width == m_ImageSmall.Width && m_ImageSizeOverride.Height == m_ImageSmall.Height))
                return new CompositeImage(m_ImageSmall, false);
            if (m_Image != null)
            {
                return new CompositeImage(m_Image, false, m_ImageSizeOverride);
            }
            if (m_ImageIndex >= 0)
            {
                if (m_DisabledImageIndex >= 0 || m_HoverImageIndex >= 0 || m_PressedImageIndex >= 0)
                    DisposedCachedImageListImage();
                image = GetImageFromImageList(m_ImageIndex);
                if (image != null)
                    return new CompositeImage(image, (image != m_ImageCachedIdx), m_ImageSizeOverride);
            }

            return null;
        }

        //private ImageList GetImageList()
        //{
        //    IOwner owner = null;
        //    IBarImageSize iImageSize = null;
        //    if (_ItemPaintArgs != null)
        //    {
        //        owner = _ItemPaintArgs.Owner;
        //        iImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
        //    }
        //    if (owner == null) owner = this.GetOwner() as IOwner;
        //    if (iImageSize == null) iImageSize = this.ContainerControl as IBarImageSize;

        //    if (owner != null)
        //    {
        //        try
        //        {
        //            if (iImageSize != null && iImageSize.ImageSize != eBarImageSize.Default)
        //            {
        //                if (iImageSize.ImageSize == eBarImageSize.Medium && owner.ImagesMedium != null)
        //                    return owner.ImagesMedium;
        //                else if (iImageSize.ImageSize == eBarImageSize.Large && owner.ImagesLarge != null)
        //                    return owner.ImagesLarge;
        //                else if (owner.Images != null)
        //                    return owner.Images;
        //            }
        //            else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != eBarImageSize.Default)
        //            {
        //                eBarImageSize imgSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
        //                if (imgSize == eBarImageSize.Medium && owner.ImagesMedium != null)
        //                    return owner.ImagesMedium;
        //                else if (imgSize == eBarImageSize.Large && owner.ImagesLarge != null)
        //                    return owner.ImagesLarge;
        //                else if (owner.Images != null)
        //                    return owner.Images;
        //            }
        //            else if (owner.Images != null)
        //                 return owner.Images;
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }

        //    return null;
        //}

        private eBarImageSize GetImageListSize(IBarImageSize barImageSize)
        {
            eBarImageSize imageListSize = eBarImageSize.Default;
            if (barImageSize != null) imageListSize = barImageSize.ImageSize;
            if (_ImageListSizeSelection != eButtonImageListSelection.NotSet)
                imageListSize = (eBarImageSize)_ImageListSizeSelection;
            return imageListSize;
        }

        private Image GetImageFromImageList(int ImageIndex)
        {
            if (ImageIndex >= 0)
            {
                IOwner owner = null;
                IBarImageSize iImageSize = null;
                if (_ItemPaintArgs != null)
                {
                    owner = _ItemPaintArgs.Owner;
                    iImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
                }
                if (owner == null) owner = this.GetOwner() as IOwner;
                if (iImageSize == null)
                {
                    iImageSize = this.ContainerControl as IBarImageSize;
                }

                if (owner != null)
                {
                    try
                    {
                        eBarImageSize imageListSize = GetImageListSize(iImageSize);

                        if (imageListSize != eBarImageSize.Default)
                        {
                            if (imageListSize == eBarImageSize.Medium && owner.ImagesMedium != null && !UseSmallImageResolved) // && owner.ImagesMedium.Images.Count>0 && ImageIndex<owner.ImagesMedium.Images.Count)
                                return owner.ImagesMedium.Images[ImageIndex];
                            else if (imageListSize == eBarImageSize.Large && owner.ImagesLarge != null && !UseSmallImageResolved) // && owner.ImagesLarge.Images.Count>0  && ImageIndex<owner.ImagesLarge.Images.Count)
                                return owner.ImagesLarge.Images[ImageIndex];
                            else if (owner.Images != null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
                            {
                                if (ImageIndex == m_ImageIndex)
                                {
                                    if (m_ImageCachedIdx == null)
                                        m_ImageCachedIdx = owner.Images.Images[ImageIndex];
                                    return m_ImageCachedIdx; //owner.Images.Images[ImageIndex];
                                }
                                else
                                    return owner.Images.Images[ImageIndex];
                            }
                        }
                        else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != eBarImageSize.Default)
                        {
                            eBarImageSize imgSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
                            if (imgSize == eBarImageSize.Medium && owner.ImagesMedium != null) // && owner.ImagesMedium.Images.Count>0 && ImageIndex<owner.ImagesMedium.Images.Count)
                                return owner.ImagesMedium.Images[ImageIndex];
                            else if (imgSize == eBarImageSize.Large && owner.ImagesLarge != null) // && owner.ImagesLarge.Images.Count>0  && ImageIndex<owner.ImagesLarge.Images.Count)
                                return owner.ImagesLarge.Images[ImageIndex];
                            else if (owner.Images != null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
                                return owner.Images.Images[ImageIndex];
                        }
                        else if (owner.Images != null)// && owner.Images.Images.Count>0 && ImageIndex<owner.Images.Images.Count)
                        {
                            if (ImageIndex == m_ImageIndex)
                            {
                                if (m_ImageCachedIdx == null)
                                    m_ImageCachedIdx = owner.Images.Images[ImageIndex];

                                return m_ImageCachedIdx; //owner.Images.Images[ImageIndex];
                            }
                            else
                                return owner.Images.Images[ImageIndex];
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Sets fixed size of the image. Image will be scaled and painted it size specified.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false)]
        public System.Drawing.Size ImageFixedSize
        {
            get { return m_ImageSizeOverride; }
            set
            {
                m_ImageSizeOverride = value;
                this.OnImageChanged();
                if (m_Parent != null && m_Parent is ImageItem)
                {
                    ((ImageItem)m_Parent).RefreshImageSize();
                }
            }
        }
        /// <summary>
        /// Resets ImageFixedSize property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetImageFixedSize()
        {
            this.ImageFixedSize = new Size();
        }
        /// <summary>
        /// Gets whether ImageFixedSize property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeImageFixedSize()
        {
            return !m_ImageSizeOverride.IsEmpty;
        }

        /// <summary>
        /// Gets or sets the fixed size of the button. Both width and height must be set to value greater than 0 in order for button to use fixed size.
        /// Setting both width and height to 0 (default value) indicates that button will be sized based on content.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), Description("Indicates fixed size of the button."), Category("Layout")]
        public virtual System.Drawing.Size FixedSize
        {
            get { return m_FixedSize; }
            set
            {
                if (value.Width < 0) value.Width = 0;
                if (value.Height < 0) value.Height = 0;
                m_FixedSize = value;
                this.NeedRecalcSize = true;
                this.OnAppearanceChanged();
            }
        }
        /// <summary>
        /// Gets whether FixedSize property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFixedSize()
        {
            return !m_FixedSize.IsEmpty;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetFixedSize()
        {
            TypeDescriptor.GetProperties(this)["FixedSize"].SetValue(this, Size.Empty);
        }

        private System.Drawing.Size IconSize
        {
            get
            {
                // Default Icon Size
                System.Drawing.Size size = new Size(16, 16);
                IBarImageSize iImageSize = null;
                if (_ItemPaintArgs != null)
                {
                    iImageSize = _ItemPaintArgs.ContainerControl as IBarImageSize;
                }
                if (iImageSize == null) iImageSize = this.ContainerControl as IBarImageSize;

                try
                {
                    eBarImageSize imageListSize = GetImageListSize(iImageSize);
                    if (iImageSize != null && imageListSize != eBarImageSize.Default)
                    {
                        if (imageListSize == eBarImageSize.Medium)
                            size = new Size(24, 24);
                        else if (imageListSize == eBarImageSize.Large)
                            size = new Size(32, 32);
                    }
                    else if (m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize != eBarImageSize.Default)
                    {
                        eBarImageSize imgSize = ((SideBarPanelItem)m_Parent).ItemImageSize;
                        if (imgSize == eBarImageSize.Medium)
                            size = new Size(24, 24);
                        else if (imgSize == eBarImageSize.Large)
                            size = new Size(32, 32);
                    }
                }
                catch (Exception)
                {
                }

                return size;
            }
        }

        /// <summary>
        /// Overridden. Recalculates the size of the item.
        /// </summary>
        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;
            StopPulse();
            if (EffectiveStyle == eDotNetBarStyle.Office2000 && !this.IsThemed)
                RecalcSizeOffice();
            else
                RecalcSizeDotNet();
            base.RecalcSize();
        }

        private void RecalcSizeDotNet()
        {
            ButtonItemLayout.LayoutButton(this);

            //			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
            //			if(!IsHandleValid(objCtrl))
            //				return;
            //			//Graphics g=Graphics.FromHwnd(objCtrl.Handle);
            //            Graphics g = BarFunctions.CreateGraphics(objCtrl);
            //			bool bHasImage=false;
            //			if(GetImage(ImageState.Default)!=null)
            //				bHasImage=true;
            //
            //			m_TextDrawRect=Rectangle.Empty;
            //			m_ImageDrawRect=Rectangle.Empty;
            //			m_SubItemsRect=Rectangle.Empty;
            //			int measureStringWidth=0;
            //			if(_FitContainer)
            //				measureStringWidth=m_Rect.Width-4;
            //			m_Rect.Width=0;
            //			m_Rect.Height=0;
            //
            //			// Get the right image size that we will use for calculation
            //			Size objImageSize=Size.Empty;
            //			if(m_Parent!=null && m_ImageSizeOverride.IsEmpty)
            //			{
            //				ImageItem objParentImageItem=m_Parent as ImageItem;
            //				if(objParentImageItem!=null && !objParentImageItem.SubItemsImageSize.IsEmpty)
            //				{
            //					if(!bHasImage || this.IsOnMenu)
            //						objImageSize=new Size(objParentImageItem.SubItemsImageSize.Width,objParentImageItem.SubItemsImageSize.Height);
            //					else
            //					{
            //						if(this.Orientation==eOrientation.Horizontal)
            //							objImageSize=new Size(this.ImageSize.Width,objParentImageItem.SubItemsImageSize.Height);
            //						else
            //                            objImageSize=new Size(objParentImageItem.SubItemsImageSize.Width,this.ImageSize.Height);							
            //					}
            //				}
            //				else
            //					objImageSize=this.ImageSize;
            //			}
            //			else if(!m_ImageSizeOverride.IsEmpty)
            //				objImageSize=m_ImageSizeOverride;
            //			else
            //				objImageSize=this.ImageSize;
            //			if(_FitContainer && bHasImage && (m_ImagePosition==eImagePosition.Left || m_ImagePosition==eImagePosition.Right))
            //			{
            //				measureStringWidth-=(objImageSize.Width+10);
            //			}
            //
            //			// Measure string
            //			Font objCurrentFont=null;
            //			if(m_Font!=null)
            //				objCurrentFont=m_Font;
            //			else
            //			{
            //				objCurrentFont=GetFont(null);
            //				if(m_HotFontBold)
            //					objCurrentFont=new Font(objCurrentFont,FontStyle.Bold);
            //			}
            //
            //			Size objStringSize=Size.Empty;
            //			eTextFormat objStringFormat=GetStringFormat();
            //			
            //			if(m_Text!="")
            //			{
            //                if (m_Orientation == eOrientation.Vertical && !this.IsOnMenu)
            //                    objStringSize = TextDrawing.MeasureStringLegacy(g, m_Text, objCurrentFont, new Size(measureStringWidth,0), objStringFormat);
            //                else
            //				    objStringSize=TextDrawing.MeasureString(g,m_Text,objCurrentFont,measureStringWidth,objStringFormat);
            //			}
            //
            //            // See if this button is on menu, and do appropriate calculations
            //			if(this.IsOnMenu)
            //			{
            //				if(objImageSize.IsEmpty)
            //					objImageSize=new Size(16,16);
            //				// Add 4 pixel padding to the image size, 2 pixels on each side
            //				objImageSize.Height+=2;
            //				objImageSize.Width+=7;
            //
            //				// Calculate item height
            //				if(objStringSize.Height>objImageSize.Height)
            //					m_Rect.Height=(int)objStringSize.Height+4;
            //				else
            //					m_Rect.Height=objImageSize.Height+4;
            //				
            //				// Add Vertical Padding to it
            //				m_Rect.Height+=m_VerticalPadding;
            //
            //				// We know the image position now, we will center it into this area
            //				if(this.IsOnCustomizeMenu)
            //					m_ImageDrawRect=new Rectangle(m_Rect.Height+2,(m_Rect.Height-objImageSize.Height)/2,objImageSize.Width,objImageSize.Height);
            //				else
            //					m_ImageDrawRect=new Rectangle(0,(m_Rect.Height-objImageSize.Height)/2,objImageSize.Width,objImageSize.Height);
            //				
            //				m_Rect.Width=(int)objStringSize.Width;
            //                // Add short-cut size if we have short-cut
            //				if( this.DrawShortcutText != "" )
            //				{
            //					Size objSizeShortcut=TextDrawing.MeasureString(g,this.DrawShortcutText,objCurrentFont,0,objStringFormat);
            //					m_Rect.Width+=(objSizeShortcut.Width+14); // 14 distance between text and shortcut
            //				}
            //
            //				m_TextDrawRect=new Rectangle(m_ImageDrawRect.Right+8,3,m_Rect.Width,m_Rect.Height-6);
            //
            //				// 8 pixels distance between image and text, 22 pixels if this item has sub items
            //				m_Rect.Width+=(m_ImageDrawRect.Right+8+26);
            //				m_Rect.Width+=m_HorizontalPadding;
            //			}
            //			else
            //			{
            //				bool bThemed=this.IsThemed;
            //				if(m_Orientation==eOrientation.Horizontal && (m_ImagePosition==eImagePosition.Left || m_ImagePosition==eImagePosition.Right))
            //				{
            //					// Recalc size for the Bar button
            //					// Add 8 pixel padding to the image size, 4 pixels on each side
            //					//objImageSize.Height+=4;
            //					objImageSize.Width+=10;
            //
            //					// Calculate item height
            //					if(objStringSize.Height>objImageSize.Height)
            //						m_Rect.Height=(int)objStringSize.Height+6;
            //					else
            //						m_Rect.Height=objImageSize.Height+6;
            //					
            //					// Add Vertical Padding
            //					m_Rect.Height+=m_VerticalPadding;
            //
            //					if(bThemed && !this.IsOnMenuBar)
            //						m_Rect.Height+=4;
            //
            //					m_ImageDrawRect=Rectangle.Empty;
            //					if(m_ButtonStyle!=eButtonStyle.TextOnlyAlways && bHasImage)
            //					{
            //						// We know the image position now, we will center it into this area
            //						m_ImageDrawRect=new Rectangle(0,(m_Rect.Height-objImageSize.Height)/2,objImageSize.Width,objImageSize.Height);
            //					}
            //					
            //					// Draw Text only if needed
            //					m_TextDrawRect=Rectangle.Empty;
            //					if(m_ButtonStyle!=eButtonStyle.Default || !bHasImage)
            //					{
            //						if(m_ImageDrawRect.Right>0)
            //						{
            //							m_Rect.Width=(int)objStringSize.Width+1;
            //							m_TextDrawRect=new Rectangle(m_ImageDrawRect.Right-2,2,m_Rect.Width,m_Rect.Height-4);
            //						}
            //						else
            //						{
            //							m_Rect.Width=(int)objStringSize.Width+6;
            //							if(!bHasImage && this.IsOnMenuBar)
            //							{
            //								m_Rect.Width+=6;
            //								m_TextDrawRect=new Rectangle(0,2,m_Rect.Width,m_Rect.Height-4);
            //							}
            //							else
            //								m_TextDrawRect=new Rectangle(3,2,m_Rect.Width,m_Rect.Height-4);
            //						}
            //					}
            //					// No need for the code below since it causes text to be drawn if item is not on Bar no matter what
            ////					else if(!this.IsOnBar)
            ////					{
            ////						if(m_ImageDrawRect.Right>0)
            ////						{
            ////							m_Rect.Width=(int)objStringSize.Width+4;
            ////							m_TextDrawRect=new Rectangle(m_ImageDrawRect.Right-2,2,m_Rect.Width,m_Rect.Height-4);
            ////						}
            ////						else
            ////						{
            ////							m_Rect.Width=(int)objStringSize.Width+6;
            ////							m_TextDrawRect=new Rectangle(3,2,m_Rect.Width,m_Rect.Height-4);
            ////						}                        						
            ////					}
            //					m_Rect.Width+=m_ImageDrawRect.Right;
            //
            //					if(m_ImagePosition==eImagePosition.Right && m_ImageDrawRect.Right>0)
            //					{
            //						m_TextDrawRect.X=3;
            //						m_ImageDrawRect.X=m_Rect.Width-m_ImageDrawRect.Width;
            //					}
            //
            //					// Add Horizontal padding
            //					m_Rect.Width+=m_HorizontalPadding;
            //				}
            //				else
            //				{
            //					// Image is on top or bottom
            //					// Calculate width, that is easy
            //					if(m_Orientation==eOrientation.Horizontal)
            //					{
            //						if(objStringSize.Width>objImageSize.Width)
            //							m_Rect.Width=(int)objStringSize.Width+6;
            //						else
            //							m_Rect.Width=objImageSize.Width+6;
            //						
            //						// Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
            //						m_Rect.Height=(int)(objImageSize.Height+objStringSize.Height+10);
            //
            //						// Add Horizontal/Vertical padding
            //						m_Rect.Width+=m_HorizontalPadding;
            //						m_Rect.Height+=m_VerticalPadding;
            //
            //						if(m_ImagePosition==eImagePosition.Top)
            //						{
            //							m_ImageDrawRect=new Rectangle(0,m_VerticalPadding/2+2,m_Rect.Width,objImageSize.Height+2);
            //							m_TextDrawRect=new Rectangle((int)(m_Rect.Width-objStringSize.Width)/2,m_ImageDrawRect.Bottom,(int)objStringSize.Width,(int)objStringSize.Height+5);
            //						}
            //						else
            //						{
            //							m_TextDrawRect=new Rectangle((int)(m_Rect.Width-objStringSize.Width)/2,m_VerticalPadding/2,(int)objStringSize.Width,(int)objStringSize.Height+2);
            //							m_ImageDrawRect=new Rectangle(0,m_TextDrawRect.Bottom,m_Rect.Width,objImageSize.Height+5);
            //						}
            //					}
            //					else
            //					{
            //						if(objStringSize.Height>objImageSize.Width && m_ButtonStyle!=eButtonStyle.Default)
            //							m_Rect.Width=(int)objStringSize.Height+6;
            //						else
            //							m_Rect.Width=objImageSize.Width+10;
            //
            //						// Add Horizontal Padding
            //						m_Rect.Width+=m_HorizontalPadding;
            //						
            //						// Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
            //						if(m_ButtonStyle!=eButtonStyle.Default || !bHasImage)
            //						{
            //							if(bHasImage)
            //								m_Rect.Height=(int)(objImageSize.Height+objStringSize.Width+12);
            //							else
            //								m_Rect.Height=(int)(objStringSize.Width+6);
            //						}
            //						else
            //							m_Rect.Height=objImageSize.Height+6;
            //
            //						if(m_ImagePosition==eImagePosition.Top || m_ImagePosition==eImagePosition.Left)
            //						{
            //							if(bHasImage)
            //								m_ImageDrawRect=new Rectangle(0,0,m_Rect.Width,objImageSize.Height+6);
            //							m_TextDrawRect=new Rectangle((int)(m_Rect.Width-objStringSize.Height)/2,m_ImageDrawRect.Bottom+2,(int)objStringSize.Height,(int)objStringSize.Width+5);
            //						}
            //						else
            //						{
            //							m_TextDrawRect=new Rectangle((int)(m_Rect.Width-objStringSize.Width)/2,0,(int)objStringSize.Height,(int)objStringSize.Width+5);
            //							if(bHasImage)
            //								m_ImageDrawRect=new Rectangle(0,m_TextDrawRect.Bottom+2,m_Rect.Width,objImageSize.Height+5);
            //						}
            //
            //						// Add Vertical Padding
            //						m_Rect.Height+=m_VerticalPadding;
            //					}
            //				}
            //
            //				//if(SubItemsCount>0 && this.ShowSubItems && (!this.IsOnMenuBar || this.GetImage()!=null))
            //				if((SubItems.Count>0 || this.PopupType==ePopupType.Container) && this.ShowSubItems && !this.IsOnMenuBar)
            //				{
            //					// Add small button to expand the item
            //					if(m_Orientation==eOrientation.Horizontal)
            //					{
            //						if(bThemed)
            //							m_SubItemsRect=new Rectangle(m_Rect.Width,0,m_SubItemsExpandWidth,m_Rect.Height);
            //						else
            //							m_SubItemsRect=new Rectangle(m_Rect.Width-2,0/*m_Rect.Top*/,m_SubItemsExpandWidth,m_Rect.Height);
            //						m_Rect.Width+=m_SubItemsExpandWidth;
            //					}
            //					else
            //					{
            //						m_SubItemsRect=new Rectangle(/*m_Rect.Left+2*/2,m_Rect.Height-2,m_Rect.Width,m_SubItemsExpandWidth);
            //						m_Rect.Height+=m_SubItemsExpandWidth;
            //					}
            //				}
            //			}
            //			// This button is on Bar
            //			//objCurrentFont.Dispose();
            //			g.Dispose();
            //			objCtrl=null;
        }

        private void RecalcSizeOffice()
        {
            System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
            if (!IsHandleValid(objCtrl))
                return;
            Graphics g = Graphics.FromHwnd(objCtrl.Handle);
            g.PageUnit = System.Drawing.GraphicsUnit.Pixel;

            m_TextDrawRect = Rectangle.Empty;
            m_ImageDrawRect = Rectangle.Empty;
            m_Rect = Rectangle.Empty;
            m_SubItemsRect = Rectangle.Empty;

            bool bHasImage = false;
            if (GetImage(ImageState.Default) != null)
                bHasImage = true;

            // Get the right image size that we will use for calculation
            Size objImageSize;
            if (m_Parent != null)
            {
                ImageItem objImageItem = m_Parent as ImageItem;
                if (objImageItem != null)
                {
                    if (!bHasImage || this.IsOnMenu)
                        objImageSize = new Size(objImageItem.SubItemsImageSize.Width, objImageItem.SubItemsImageSize.Height);
                    else
                    {
                        if (this.Orientation == eOrientation.Horizontal)
                            objImageSize = new Size(this.ImageSize.Width, objImageItem.SubItemsImageSize.Height);
                        else
                            objImageSize = new Size(objImageItem.SubItemsImageSize.Width, this.ImageSize.Height);
                    }
                }
                else
                    objImageSize = this.ImageSize;
            }
            else
                objImageSize = this.ImageSize;

            // Measure string
            Font objCurrentFont = null;
            if (m_Font != null)
                objCurrentFont = m_Font;
            else
                objCurrentFont = GetFont(null, true);

            Size objStringSize;
            eTextFormat objStringFormat = GetStringFormat();

            if (m_Orientation == eOrientation.Vertical && !this.IsOnMenu)
                objStringSize = TextDrawing.MeasureStringLegacy(g, m_Text, objCurrentFont, Size.Empty, objStringFormat);
            else
                objStringSize = TextDrawing.MeasureString(g, m_Text, objCurrentFont, 0, objStringFormat);

            // See if this button is on menu, and do appropriate calculations
            if (this.IsOnMenu)
            {
                // Add 4 pixel padding to the image size, 2 pixels on each side
                //objImageSize.Height+=1;
                objImageSize.Width += 4;

                // Calculate item height
                if (objStringSize.Height > objImageSize.Height)
                    m_Rect.Height = (int)objStringSize.Height + 4;
                else
                    m_Rect.Height = objImageSize.Height + 4;

                // Add Padding
                m_Rect.Height += m_VerticalPadding;

                // We know the image position now, we will center it into this area
                if (this.IsOnCustomizeMenu)
                    m_ImageDrawRect = new Rectangle(m_Rect.Height, 0, m_Rect.Height, m_Rect.Height);
                else
                    m_ImageDrawRect = new Rectangle(0, 0, objImageSize.Width, m_Rect.Height);

                m_Rect.Width = (int)objStringSize.Width;
                // Add short-cut size if we have short-cut
                if (this.DrawShortcutText != "")
                {
                    Size objSizeShortcut = TextDrawing.MeasureString(g, this.DrawShortcutText, objCurrentFont, 0, objStringFormat);
                    m_Rect.Width += (objSizeShortcut.Width + 14); // 14 distance between text and shortcut
                }

                //m_TextDrawRect=new Rectangle(m_ImageDrawRect.Right+2,3,m_Rect.Width,m_Rect.Height-6);
                m_TextDrawRect = new Rectangle(m_ImageDrawRect.Right + 2, (int)(m_Rect.Height - objStringSize.Height) / 2, m_Rect.Width, (int)objStringSize.Height);

                // 8 pixels distance between image and text, 22 pixels if this item has sub items
                m_Rect.Width += (m_ImageDrawRect.Right + 2 + 24);

                // Add Horizontal Padding
                m_Rect.Width += m_HorizontalPadding;

                // Don't support the image alignment in menus yet
                /*if(m_ImagePosition==eImagePosition.Right)
                {
                    int i=m_TextDrawRect.Right-m_ImageDrawRect.Right;
                    m_TextDrawRect.X=m_ImageDrawRect.X;
                    m_ImageDrawRect.X=i;
                }*/

                //if(m_BeginGroup)
                //	m_Rect.Height+=9;
            }
            else
            {
                // Recalc size for the Bar button
                if (m_Orientation == eOrientation.Horizontal && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
                {
                    // Add 8 pixel padding to the image size, 4 pixels on each side
                    //objImageSize.Height+=4;
                    objImageSize.Width += 10;

                    // Calculate item height
                    if (objStringSize.Height > objImageSize.Height)
                        m_Rect.Height = (int)objStringSize.Height + 6;
                    else
                        m_Rect.Height = objImageSize.Height + 6;

                    // Add Vertical Padding
                    m_Rect.Height += m_VerticalPadding;

                    m_ImageDrawRect = Rectangle.Empty;
                    if (m_ButtonStyle != eButtonStyle.TextOnlyAlways && bHasImage)
                    {
                        // We know the image position now, we will center it into this area
                        m_ImageDrawRect = new Rectangle(0, (m_Rect.Height - objImageSize.Height) / 2, objImageSize.Width, objImageSize.Height);
                    }

                    // Draw Text only if needed
                    m_TextDrawRect = Rectangle.Empty;
                    if (m_ButtonStyle != eButtonStyle.Default || !bHasImage)
                    {
                        if (m_ImageDrawRect.Right > 0)
                        {
                            m_Rect.Width = (int)objStringSize.Width + 1;
                            m_TextDrawRect = new Rectangle(m_ImageDrawRect.Right - 2, 2, m_Rect.Width, m_Rect.Height - 4);
                        }
                        else
                        {
                            m_Rect.Width = (int)objStringSize.Width + 6;
                            m_TextDrawRect = new Rectangle(3, 2, m_Rect.Width, m_Rect.Height - 4);
                        }
                    }
                    m_Rect.Width += m_ImageDrawRect.Right;

                    // Add Horizontal Padding
                    m_Rect.Width += m_HorizontalPadding;
                }
                else
                {
                    // Image is on top or bottom
                    if (m_Orientation == eOrientation.Horizontal)
                    {
                        // Calculate width, that is easy
                        if (objStringSize.Width > objImageSize.Width)
                            m_Rect.Width = (int)objStringSize.Width + 6;
                        else
                            m_Rect.Width = objImageSize.Width + 6;

                        // Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
                        m_Rect.Height = (int)(objImageSize.Height + objStringSize.Height + 10);

                        // Add Padding
                        m_Rect.Width += m_HorizontalPadding;
                        m_Rect.Height += m_VerticalPadding;

                        if (m_ImagePosition == eImagePosition.Top)
                        {
                            m_ImageDrawRect = new Rectangle(0, m_VerticalPadding / 2, m_Rect.Width, objImageSize.Height + 2);
                            m_TextDrawRect = new Rectangle((int)(m_Rect.Width - objStringSize.Width) / 2, m_ImageDrawRect.Bottom + 2, (int)objStringSize.Width, (int)objStringSize.Height + 5);
                        }
                        else
                        {
                            m_TextDrawRect = new Rectangle((int)(m_Rect.Width - objStringSize.Width) / 2, m_VerticalPadding / 2, (int)objStringSize.Width, (int)objStringSize.Height + 2);
                            m_ImageDrawRect = new Rectangle(0, m_TextDrawRect.Bottom + 2, m_Rect.Width, objImageSize.Height + 5);
                        }
                    }
                    else
                    {
                        // Calculate width, that is easy
                        if (objStringSize.Height > objImageSize.Height && m_ButtonStyle != eButtonStyle.Default)
                            m_Rect.Width = (int)objStringSize.Height + 6;
                        else
                            m_Rect.Width = objImageSize.Width + 6;

                        m_Rect.Width += m_HorizontalPadding;

                        // Calculate item height 3 padding on top and bottom and 2 pixels distance between the image and text
                        if (m_ButtonStyle != eButtonStyle.Default || !bHasImage)
                        {
                            if (bHasImage)
                                m_Rect.Height = (int)(objImageSize.Height + objStringSize.Width + 12);
                            else
                                m_Rect.Height = (int)(objStringSize.Width + 6);
                        }
                        else
                            m_Rect.Height = objImageSize.Height + 6;

                        if (m_ImagePosition == eImagePosition.Top || m_ImagePosition == eImagePosition.Left)
                        {
                            if (bHasImage)
                                m_ImageDrawRect = new Rectangle(0, 0, m_Rect.Width, objImageSize.Height + 5);
                            m_TextDrawRect = new Rectangle((int)(m_Rect.Width - objStringSize.Height) / 2, m_ImageDrawRect.Bottom + 2, (int)objStringSize.Height, (int)objStringSize.Width + 5);
                        }
                        else
                        {
                            m_TextDrawRect = new Rectangle((int)(m_Rect.Width - objStringSize.Height) / 2, 0, (int)objStringSize.Height, (int)objStringSize.Width + 5);
                            if (bHasImage)
                                m_ImageDrawRect = new Rectangle(0, m_TextDrawRect.Bottom + 2, m_Rect.Width, objImageSize.Height + 5);
                        }
                        m_Rect.Height += m_VerticalPadding;
                    }
                }

                if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems && (!this.IsOnMenuBar || this.GetImage() != null))
                {
                    // Add small button to expand the item
                    if (m_Orientation == eOrientation.Horizontal)
                    {
                        m_SubItemsRect = new Rectangle(m_Rect.Right, 0/*m_Rect.Top*/, 12, m_Rect.Height);
                        m_Rect.Width += m_SubItemsRect.Width;
                    }
                    else
                    {
                        m_SubItemsRect = new Rectangle(0/*m_Rect.Left*/, m_Rect.Bottom, m_Rect.Width, 12);
                        m_Rect.Height += m_SubItemsRect.Height;
                    }
                }
            }

            // This button is on Bar
            //objCurrentFont.Dispose();
            g.Dispose();
            objCtrl = null;
        }

        private void DrawOfficeCheckBox(Graphics g, Rectangle r)
        {
            // Draw checked box
            System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, System.Windows.Forms.Border3DStyle.SunkenOuter, System.Windows.Forms.Border3DSide.All);
            if (!m_MouseOver)
            {
                r.Inflate(-1, -1);
                g.FillRectangle(ColorFunctions.GetPushedBrush(), r);
            }
            // Draw checker...
            Point[] pt = new Point[3];
            pt[0].X = r.Left + (r.Width - 6) / 2;
            pt[0].Y = r.Top + (r.Height - 6) / 2 + 3;
            pt[1].X = pt[0].X + 2;
            pt[1].Y = pt[0].Y + 2;
            pt[2].X = pt[1].X + 4;
            pt[2].Y = pt[1].Y - 4;
            g.DrawLines(SystemPens.ControlText, pt);
            pt[0].X++;
            pt[1].X++;
            pt[2].X++;
            g.DrawLines(SystemPens.ControlText, pt);
        }

        protected internal override void OnItemAdded(BaseItem item)
        {
            base.OnItemAdded(item);
            OnAppearanceChanged();
        }

        /// <summary>
        /// Overloaded. Called when size of the item is changed externaly.
        /// </summary>
        protected override void OnExternalSizeChange()
        {
            base.OnExternalSizeChange();
            ButtonItemLayout.Arrange(this);
            return;
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void ContainerLostFocus(bool appLostFocus)
        {
            base.ContainerLostFocus(appLostFocus);
            if (m_Expanded)
            {
                this.Expanded = false;
                if (m_Parent != null)
                    m_Parent.AutoExpand = false;
            }
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalKeyDown(System.Windows.Forms.KeyEventArgs objArg)
        {
            base.InternalKeyDown(objArg);
            if (this.Expanded || objArg.Handled)
                return;

            if (objArg.KeyCode == System.Windows.Forms.Keys.Enter || objArg.KeyCode == System.Windows.Forms.Keys.Return || objArg.KeyCode == System.Windows.Forms.Keys.Right && this.IsOnMenu)
            {
                if (SubItems.Count > 0 && this.GetEnabled())
                {
                    if (this.Expanded)
                    {
                        if (m_Parent != null)
                            m_Parent.AutoExpand = false;
                        this.Expanded = false;
                    }
                    else
                    {
                        if (m_Parent != null)
                            m_Parent.AutoExpand = true;
                        this.Expanded = true;
                        // Select first item on that popup
                        if (this.PopupType == ePopupType.Menu && this.PopupControl is MenuPanel)
                        {
                            ((MenuPanel)this.PopupControl).SelectFirstItem();
                        }
                    }
                    objArg.Handled = true;
                    return;
                }
            }
            else if (objArg.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                if (SubItems.Count > 0 && this.Expanded)
                {
                    this.Expanded = false;
                    if (m_Parent != null)
                        m_Parent.AutoExpand = false;
                    objArg.Handled = true;
                    return;
                }
            }

            base.InternalKeyDown(objArg);
        }

        #region Fade Effect Support
        private Bitmap m_ImageState2 = null;
        private int m_Direction = 1;
        private int m_Alpha = 0;
        private bool m_FadeAnimation = false;
        private System.Threading.ReaderWriterLock m_FadeImageLock = new System.Threading.ReaderWriterLock();
        private void SetMouseOver(bool value)
        {
            if (value != m_MouseOver)
            {
                if (m_Pulse)
                {
                    StopFade();
                    if (m_StopPulseOnMouseOver)
                        StopPulse();
                }

                bool fadeEnabled = (IsFadeEnabled || m_Pulse) && this.GetEnabled() && this.Displayed && this.Visible && this.WidthInternal > 0 && this.HeightInternal > 0;
                if (fadeEnabled)
                {
                    int direction = value ? 1 : -1;
                    int initialAlpha = value ? 10 : 255;

                    m_MouseOver = value;
                    StartFade(direction, initialAlpha);
                }
                else
                    m_MouseOver = value;
            }
        }

        private void StartFade(int direction, int initialAlpha)
        {
            bool createImage = false;
            m_FadeImageLock.AcquireReaderLock(-1);
            try
            {
                createImage = (m_ImageState2 == null);
            }
            finally
            {
                m_FadeImageLock.ReleaseReaderLock();
            }

            if (createImage)
            {
                bool oldMouseOver = m_MouseOver;
                m_MouseOver = true;
                try
                {
                    bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
                    System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();
                    if (readerLockHeld)
                    {
                        cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
                    }
                    else
                    {
                        m_FadeImageLock.AcquireWriterLock(-1);
                    }

                    try
                    {
                        m_ImageState2 = GetCurrentStateImage();
                    }
                    finally
                    {
                        if (readerLockHeld)
                        {
                            m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
                        }
                        else
                        {
                            m_FadeImageLock.ReleaseWriterLock();
                        }
                    }
                }
                finally
                {
                    m_MouseOver = oldMouseOver;
                }
            }

            m_Direction = direction;
            m_Alpha = initialAlpha;

            FadeAnimator.Fade(this, new EventHandler(this.OnFadeChanged));
            m_FadeAnimation = true;
        }

        private void StopFade()
        {
            if (!m_FadeAnimation)
                return;

            m_FadeAnimation = false;
            FadeAnimator.StopFade(this, new EventHandler(OnFadeChanged));

            bool disposeImage = false;
            m_FadeImageLock.AcquireReaderLock(-1);
            try
            {
                disposeImage = (m_ImageState2 != null);
            }
            finally
            {
                m_FadeImageLock.ReleaseReaderLock();
            }
            if (disposeImage)
                DisposeFadeImage();

            if (m_Alpha > 230)
                m_Alpha = 255;
            else if (m_Alpha < 0)
                m_Alpha = 0;
        }

        private void DisposeFadeImage()
        {
            bool readerLockHeld = m_FadeImageLock.IsReaderLockHeld;
            System.Threading.LockCookie cookie1 = new System.Threading.LockCookie();

            if (readerLockHeld)
            {
                cookie1 = m_FadeImageLock.UpgradeToWriterLock(-1);
            }
            else
            {
                m_FadeImageLock.AcquireWriterLock(-1);
            }

            try
            {
                m_ImageState2.Dispose();
                m_ImageState2 = null;
            }
            finally
            {
                if (readerLockHeld)
                {
                    m_FadeImageLock.DowngradeFromWriterLock(ref cookie1);
                }
                else
                {
                    m_FadeImageLock.ReleaseWriterLock();
                }
            }
        }

        private void OnFadeChanged(object sender, EventArgs e)
        {
            m_Alpha += (m_Direction * (m_Pulse ? m_PulseFadeAlphaIncrement : m_MouseOverFadeAlphaIncrement));

            if (m_Direction < 0 && m_Alpha <= 0 || m_Direction > 0 && m_Alpha >= 255)
            {
                if (m_Pulse)
                {
                    m_Direction *= -1;
                    if (m_Alpha >= 255) m_Alpha = 255; else m_Alpha = 0;
                    m_PulseCount++;
                    if (m_PulseBeats > 0 && m_PulseCount > m_PulseBeats)
                    {
                        StopFade();
                        StopPulse();
                    }
                }
                else
                    StopFade();
            }

            System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
            if (cc != null && BarFunctions.IsHandleValid(cc))
            {
                try
                {
                    cc.Invalidate(this.DisplayRectangle);
                }
                catch (ObjectDisposedException) { }
                catch (System.ComponentModel.Win32Exception) { }
            }
        }

        private Bitmap GetCurrentStateImage()
        {
            Bitmap bitmap = new Bitmap(this.WidthInternal, this.HeightInternal, System.Drawing.Imaging.PixelFormat.Format32bppPArgb); // Format32bppArgb);
            bitmap.MakeTransparent();
            Graphics g = Graphics.FromImage(bitmap);

            try
            {
                System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
                bool antiAlias = false;
                ItemPaintArgs pa = null;
                if (cc is ItemControl)
                {
                    antiAlias = ((ItemControl)cc).AntiAlias;
                    pa = ((ItemControl)cc).GetItemPaintArgs(g);
                }
                else if (cc is Bar)
                {
                    antiAlias = ((Bar)cc).AntiAlias;
                    pa = ((Bar)cc).GetItemPaintArgs(g);
                }
                else if (cc is ButtonX)
                {
                    antiAlias = ((ButtonX)cc).AntiAlias;
                    pa = ((ButtonX)cc).GetItemPaintArgs(g);
                }

                System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
                myMatrix.Translate(-this.DisplayRectangle.X, -this.DisplayRectangle.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
                myMatrix.Dispose();
                myMatrix = null;
                if (antiAlias)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
                }

                if (pa == null)
                {
                    bitmap.Dispose();
                    return null;
                }

                this.Paint(pa);
            }
            finally
            {
                g.Dispose();
            }
            return bitmap;
        }

        internal bool _FadeEnabled = true;
        /// <summary>
        /// Gets whether fade effect is enabled.
        /// </summary>
        protected virtual bool IsFadeEnabled
        {
            get
            {
                eDotNetBarStyle effectiveStyle = EffectiveStyle;

                if (this.DesignMode || m_HotTrackingStyle == eHotTrackingStyle.None || !_FadeEnabled || TextDrawing.UseTextRenderer
                    || effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7)
                    return false;
                if (WinApi.IsGlassEnabled && (this.Parent is CaptionItemContainer || this.Parent is RibbonTabItemContainer && effectiveStyle == eDotNetBarStyle.Office2010))
                    return false;

                System.Windows.Forms.Control cc = this.ContainerControl as System.Windows.Forms.Control;
                if (cc != null)
                {
                    if (cc is ItemControl)
                        return ((ItemControl)cc).IsFadeEnabled;
                    else if (cc is Bar)
                        return ((Bar)cc).IsFadeEnabled;
                    else if (cc is ButtonX)
                        return ((ButtonX)cc).IsFadeEnabled;
                }
                return false;
            }
        }

        /// <summary>
        /// Starts the button pulse effect which alternates slowly between the mouse over and the default state. The pulse effect
        /// continues indefinitely until it is stopped by call to StopPulse method.
        /// </summary>
        public void Pulse()
        {
            Pulse(0);
        }
        /// <summary>
        /// Gets whether Pulse function is enabled.
        /// </summary>
        protected virtual bool IsPulseEnabed
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Starts the button pulse effect which alternates slowly between the mouse over and the default state. Pulse effect
        /// will alternate between the pulse state for the number of times specified by the pulseBeatCount parameter.
        /// </summary>
        /// <param name="pulseBeatCount">Specifies the number of times button alternates between pulse states. 0 indicates indefinite pulse</param>
        public void Pulse(int pulseBeatCount)
        {
            if (this.DesignMode || m_HotTrackingStyle == eHotTrackingStyle.None || !IsPulseEnabed)
                return;

            m_PulseBeats = pulseBeatCount;
            m_PulseCount = 0;
            if (m_Pulse) return;

            m_Pulse = true;
            if (m_MouseOver) return;

            StartFade(1, 0);
        }

        /// <summary>
        /// Stops the button Pulse effect.
        /// </summary>
        public void StopPulse()
        {
            if (m_Pulse)
            {
                m_Pulse = false;

                if (!m_MouseOver)
                    StopFade();
            }
        }

        /// <summary>
        /// Gets whether the button is currently pulsing, alternating slowly between the mouse over and default state.
        /// </summary>
        [Browsable(false)]
        public bool IsPulsing
        {
            get { return m_Pulse; }
        }

        /// <summary>
        /// Gets or sets whether pulse effect started with StartPulse method stops automatically when mouse moves over the button. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Behavior"), Description("Indicates whether pulse effect started with Pulse method stops automatically when mouse moves over the button.")]
        public virtual bool StopPulseOnMouseOver
        {
            get { return m_StopPulseOnMouseOver; }
            set { m_StopPulseOnMouseOver = value; }
        }

        /// <summary>
        /// Gets or sets the pulse speed. The value must be greater than 0 and less than 128. Higher values indicate faster pulse. Default value is 12.
        /// </summary>
        [Browsable(true), DefaultValue(12), Category("Behavior"), Description("Indicates pulse speed. The value must be greater than 0 and less than 128.")]
        public virtual int PulseSpeed
        {
            get { return m_PulseFadeAlphaIncrement; }
            set
            {
                if (value <= 0 || value >= 128)
                    throw new ArgumentOutOfRangeException("PulseSpeed value must be greater than 0 and less than 128");
                m_PulseFadeAlphaIncrement = value;
            }
        }
        #endregion

        /// <summary>
        /// Indicates whether the item enabled property has changed.
        /// </summary>
        protected override void OnEnabledChanged()
        {
            if (!this.GetEnabled())
            {
                SetMouseOver(false);
                SetMouseDown(false);
            }
            base.OnEnabledChanged();
        }

        //[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //public override void InternalMouseEnter()
        //{
        //    base.InternalMouseEnter();

        //}

        public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
        {
            base.InternalMouseMove(objArg);

            if (!this.DisplayRectangle.Contains(objArg.X, objArg.Y))
                return;

            bool refresh = false;

            Rectangle r = GetTotalSubItemsRect(); // m_SubItemsRect;
            if (!r.IsEmpty && this.GetEnabled())
            {
                r.Offset(this.DisplayRectangle.Location);
                if (r.Contains(objArg.X, objArg.Y))
                {
                    if (!m_MouseOverExpand)
                    {
                        m_MouseOverExpand = true;
                        refresh = true;
                    }
                }
                else if (m_MouseOverExpand)
                {
                    m_MouseOverExpand = false;
                    refresh = true;
                }
            }

            if (!m_MouseOver)
            {
                SetMouseOver(true);
                if (this.GetEnabled() || this.IsOnMenu)
                    refresh = true;
            }

            if (refresh)
                this.Refresh();
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseLeave()
        {
            base.InternalMouseLeave();

            m_MouseOverExpand = false;
            SetMouseOver(false);
            m_MouseDown = false;

            if (this.GetEnabled() || this.IsOnMenu)
                this.Refresh();
        }

        internal bool MouseIsOver
        {
            get { return (m_MouseOver); }
            set { m_MouseOver = value; }
        }

        internal bool MouseIsOverExpand
        {
            get { return (m_MouseOverExpand); }
            set { m_MouseOverExpand = value; }
        }

        internal bool MouseIsDown
        {
            get { return (m_MouseDown); }
            set { m_MouseDown = value; }
        }

        internal bool ButtonIsExpanded
        {
            get { return (m_Expanded); }
            set { m_Expanded = value; }
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseHover()
        {
            base.InternalMouseHover();

            if ((this.SubItems.Count > 0 || (this.PopupType == ePopupType.Container && !this.IsOnCustomizeMenu)) && this.IsOnMenu && this.ShowSubItems && this.AutoExpandMenuItem)
            {
                if (!this.Expanded && GetEnabled())
                    this.Expanded = true;
            }
        }

        private bool _AutoExpandMenuItem = true;
        /// <summary>
        /// Gets or sets whether button auto-expands on mouse hover when button is used as menu-item and displayed on menu. Default value is true.
        /// </summary>
        [DefaultValue(true), Browsable(false), Description("Indicates whether button auto-expands on mouse hover when button is used as menu-item and displayed on menu."), Category("Behavior")]
        public bool AutoExpandMenuItem
        {
            get { return _AutoExpandMenuItem; }
            set
            {
                _AutoExpandMenuItem = value;
            }
        }


        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
        {
            base.InternalMouseDown(objArg);
            ButtonMouseDown(objArg);
        }

        protected virtual bool CanShowPopup
        {
            get
            {
                return (this.ShowSubItems || ShouldAutoExpandOnClick) && (this.SubItems.Count > 0 || this.PopupType == ePopupType.Container);
            }
        }
        /// <summary>
        /// Provides internal implementation for ButtonItem mouse down events.
        /// </summary>
        /// <param name="objArg">Mouse event arguments.</param>
        protected virtual void ButtonMouseDown(System.Windows.Forms.MouseEventArgs objArg)
        {
            // If this is a label don't do anything
            //if(m_ButtonType==eButtonType.Label)
            //	return;

            if (objArg.Button != System.Windows.Forms.MouseButtons.Left || !GetEnabled())
                return;

            m_MouseDown = true;

            if ((this.IsOnMenuBar || ShouldAutoExpandOnClick) && !this.DesignMode)
            {
                if (objArg.Clicks != 2) // Ignore Double Click
                {
                    if (CanShowPopup)
                    {
                        if (this.Expanded)
                        {
                            // Order of those two commands is very important since setting autoexpand to false will collapse all other items
                            this.Expanded = false;
                            if (m_Parent != null)
                                m_Parent.AutoExpand = false;
                        }
                        else
                        {
                            if (this.IsOnMenuBar && m_Parent != null)
                                m_Parent.AutoExpand = true;
                            this.Expanded = true;
                        }
                    }
                    else
                        this.Refresh();
                }
            }
            else if ((this.IsOnMenuBar || ShouldAutoExpandOnClick) && this.DesignMode)
                this.Expanded = !this.Expanded;
            else if (!this.IsOnMenu)
            {
                // If user clicks on expand sub items part of this button expand the item
                if ((this.SubItems.Count > 0 || (this.PopupType == ePopupType.Container && !this.IsOnCustomizeMenu)) && this.ShowSubItems)
                {
                    if (!m_Expanded)
                    {
                        Rectangle r = GetTotalSubItemsRect();
                        r.Width += 2;
                        r.Height += 2;
                        //Rectangle r = new Rectangle(m_SubItemsRect.X, m_SubItemsRect.Y, m_SubItemsRect.Width + 2, m_SubItemsRect.Height + 2);
                        r.Offset(m_Rect.X, m_Rect.Y);  // This was a bug since m_SubItemsRect already has the right position see RecalcSize...
                        if (r.Contains(objArg.X, objArg.Y))
                        {
                            if (EffectiveStyle == eDotNetBarStyle.Office2000)
                                m_MouseDown = false;
                            this.Expanded = true;
                        }

                    }
                    else
                        this.Expanded = false;
                }

                this.Refresh();
            }
            else
            {
                if ((this.SubItems.Count > 0 || this.PopupType == ePopupType.Container && !this.IsOnCustomizeMenu) && this.ShowSubItems)
                {
                    if (!(this.IsOnMenu && this.Expanded))
                        this.Expanded = !m_Expanded;
                }
                else
                    this.Refresh();
            }
        }

        internal override void DoAccesibleDefaultAction()
        {
            if (this.VisibleSubItems > 0 && (this.IsOnMenu || this.IsOnMenuBar || _AccessibleExpandAction || ShouldAutoExpandOnClick))
            {
                if (this.Expanded)
                {
                    this.Expanded = false;
                    if (this.Parent != null && this.IsOnMenuBar)
                        this.Parent.AutoExpand = false;
                }
                else
                {
                    if (this.IsOnMenuBar && this.Parent != null)
                        this.Parent.AutoExpand = true;
                    this.Expanded = true;
                }
                this.Refresh();
                _AccessibleExpandAction = false;
            }
            else
                this.RaiseClick(eEventSource.Keyboard);
        }

        /// <summary>
        /// Indicates whether button should popup when clicked automatically.
        /// </summary>
        protected virtual bool ShouldAutoExpandOnClick
        {
            get { return m_AutoExpandOnClick; }
        }

        internal bool GetShouldAutoExpandOnClick()
        {
            return this.ShouldAutoExpandOnClick;
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
        {
            bool bCallBase = true;

            Rectangle rsub = GetTotalSubItemsRect();
            rsub.Width += 2;
            rsub.Height += 2;
            rsub.Offset(m_Rect.X, m_Rect.Y);

            if (objArg.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (GetEnabled() && !this.DesignMode)
                {
                    if (m_MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !m_RecentlyUsed && this.IsOnMenu)
                    {
                        // Propagate to the top
                        m_RecentlyUsed = true;
                        BaseItem objItem = this.Parent;
                        while (objItem != null)
                        {
                            IPersonalizedMenuItem ipm = objItem as IPersonalizedMenuItem;
                            if (ipm != null)
                                ipm.RecentlyUsed = true;
                            objItem = objItem.Parent;
                        }
                    }
                }

                // Since base item does not auto-collapse when clicked if it has subitems and it is on
                // pop-up we need to handle that here and check did user click on expand part of this button
                // and if they did not we need to raise click event and collapse the item.
                if (!this.IsOnMenu && (this.SubItems.Count > 0 || this.PopupType == ePopupType.Container) && this.ShowSubItems && m_HotSubItem == null && !this.DesignMode && !this.IsOnMenuBar && !ShouldAutoExpandOnClick)
                {
                    System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                    if (objCtrl == null)
                        return;

                    //Point p=objCtrl.PointToClient(new Point(objArg.X,objArg.Y));
                    objCtrl = null;
                    if (!rsub.Contains(objArg.X, objArg.Y))
                    {
                        bool bCollapse = true;
                        if (this.PopupType == ePopupType.ToolBar || this.PopupType == ePopupType.Menu)
                        {
                            bool bExpanded = this.Expanded;
                            base.InternalMouseUp(objArg);
                            // Do not collapse if user has expanded it in Click event
                            if (!bExpanded && this.Expanded)
                                bCollapse = false;
                            bCallBase = false;
                        }
                        if (bCollapse)
                            CollapseAll(this);
                    }
                }
            }

            if (!this.IsOnMenu || this.Parent is ItemContainer)
            {
                // If user clicks on expand sub items part of this button do not raise the click event, do not call base MouseUp
                if ((this.SubItems.Count > 0 || (this.PopupType == ePopupType.Container && !this.IsOnCustomizeMenu)) && this.ShowSubItems)
                {
                    if (rsub.Contains(objArg.X, objArg.Y))
                        bCallBase = false;
                }
            }

            if (bCallBase)
                base.InternalMouseUp(objArg);

            if (!m_MouseDown)
                return;

            m_MouseDown = false;

            if (!this.IsOnMenu || !(this.Parent is ButtonItem))
            {
                this.Refresh();
            }
        }

        internal void SetMouseDown(bool mouseDown)
        {
            m_MouseDown = mouseDown;
        }

        /// <summary>
        /// Occurs when the item is double clicked. This is used by internal implementation only.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override void InternalDoubleClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
        {
            base.InternalDoubleClick(mb, mpos);

            if (this.Expanded && this.IsOnMenuBar
                && m_Parent != null && !this.DesignMode && this.PopupType == ePopupType.Menu &&
                (this.PersonalizedMenus == ePersonalizedMenus.Both || this.PersonalizedMenus == ePersonalizedMenus.DisplayOnClick))
            {
                if (this.PopupControl is MenuPanel)
                    ((MenuPanel)this.PopupControl).ExpandRecentlyUsed();
            }
        }

        /*public override void InternalClick(System.Windows.Forms.MouseButtons mb, System.Drawing.Point mpos)
        {
            if(m_Enabled && !this.DesignMode)
            {
                if(m_MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed && !m_RecentlyUsed && this.IsOnMenu)
                {
                    // Propagate to the top
                    m_RecentlyUsed=true;
                    BaseItem objItem=this.Parent;
                    while(objItem!=null)
                    {
                        IPersonalizedMenuItem ipm=objItem as IPersonalizedMenuItem;
                        if(ipm!=null)
                            ipm.RecentlyUsed=true;
                        objItem=objItem.Parent;
                    }
                }
            }

            // Since base item does not auto-collapse when clicked if it has subitems and it is on
            // pop-up we need to handle that here and check did user click on expand part of this button
            // and if they did not we need to raise click event and collapse the item.
            if(!this.IsOnMenu && (this.SubItemsCount>0 || this.PopupType==ePopupType.Container) && this.ShowSubItems && m_HotSubItem==null && !this.DesignMode && !this.IsOnMenuBar)
            {
                Rectangle r=new Rectangle(m_SubItemsRect.X,m_SubItemsRect.Y,m_SubItemsRect.Width,m_SubItemsRect.Height);
                r.Offset(m_Rect.X,m_Rect.Y);
                System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
                if(objCtrl==null)
                {
                    base.InternalClick(mb,mpos);
                    return;
                }
                Point p=objCtrl.PointToClient(mpos);
                objCtrl=null;
                if(!r.Contains(p))
                {
                    CollapseAll(this);
                    RaiseClick();
                }
            }
            else
                base.InternalClick(mb,mpos);
        }*/

        //		private void CreateDisabledImage()
        //		{
        //			if(m_Image==null && m_ImageIndex<0 && m_Icon==null)
        //				return;
        //			if(m_DisabledImage!=null)
        //				m_DisabledImage.Dispose();
        //			m_DisabledImage=null;
        //
        //			CompositeImage defaultImage=GetImage(ImageState.Default);
        //
        //			if(defaultImage==null)
        //				return;
        //			if(!defaultImage.IsIcon && defaultImage.Image!=null && defaultImage.Image is Bitmap)
        //			{
        //				m_DisabledImage=BarFunctions.CreateDisabledBitmap((Bitmap)defaultImage.Image);
        //			}
        //		}
        private void CreateDisabledImage()
        {
            if (m_Image == null && m_ImageIndex < 0 && m_Icon == null)
                return;
            if (m_DisabledImage != null)
                m_DisabledImage.Dispose();
            m_DisabledImage = null;
            if (m_DisabledIcon != null)
                m_DisabledIcon.Dispose();
            m_DisabledIcon = null;

            CompositeImage defaultImage = GetImage(ImageState.Default);

            if (defaultImage == null)
                return;

            if (this.GetOwner() is IOwner && ((IOwner)this.GetOwner()).DisabledImagesGrayScale)
            {
                if (defaultImage.IsIcon)
                {
                    m_DisabledIcon = BarFunctions.CreateDisabledIcon(defaultImage.Icon);
                }
                else
                {
                    m_DisabledImage = ImageHelper.CreateGrayScaleImage(defaultImage.Image as Bitmap);
                }
            }
            if (m_DisabledIcon != null || m_DisabledImage != null)
                return;

            // Use old algorithm if first one failed...
            System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (!defaultImage.IsIcon && defaultImage.Image != null)
                pixelFormat = defaultImage.Image.PixelFormat;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed || pixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed || pixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            Bitmap bmp = new Bitmap(defaultImage.Width, defaultImage.Height, pixelFormat);
            m_DisabledImage = new Bitmap(defaultImage.Width, defaultImage.Height, pixelFormat);

            Graphics g2 = Graphics.FromImage(bmp);
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.White))
                g2.FillRectangle(brush, 0, 0, defaultImage.Width, defaultImage.Height);
            //g2.DrawImage(defaultImage,0,0,defaultImage.Width,defaultImage.Height);
            defaultImage.DrawImage(g2, new Rectangle(0, 0, defaultImage.Width, defaultImage.Height));
            g2.Dispose();
            g2 = Graphics.FromImage(m_DisabledImage);

            bmp.MakeTransparent(System.Drawing.Color.White);
            eDotNetBarStyle effectiveStyle = EffectiveStyle;
            if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && NativeFunctions.ColorDepth >= 8)
            {
                float[][] array = new float[5][];
                array[0] = new float[5] { 0, 0, 0, 0, 0 };
                array[1] = new float[5] { 0, 0, 0, 0, 0 };
                array[2] = new float[5] { 0, 0, 0, 0, 0 };
                array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                array[4] = new float[5] { 0, 0, 0, 0, 0 };
                System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(grayMatrix);
                g2.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, disabledImageAttr);
            }
            else
                System.Windows.Forms.ControlPaint.DrawImageDisabled(g2, bmp, 0, 0, ColorFunctions.MenuBackColor(g2));

            // Clean up
            g2.Dispose();
            g2 = null;
            bmp.Dispose();
            bmp = null;

            defaultImage.Dispose();
        }

        private eTextFormat GetStringFormat()
        {
            eTextFormat format = eTextFormat.Default;
            if (!_FitContainer)
            {
                format |= eTextFormat.SingleLine;
            }
            else
                format |= eTextFormat.WordBreak;

            format |= eTextFormat.VerticalCenter;
            return format;

            //            //bool bMenuBar=this.IsOnMenuBar;
            //            StringFormat sfmt=BarFunctions.CreateStringFormat(); //new StringFormat(StringFormat.GenericDefault);	
            //            if(NativeFunctions.ShowKeyboardCues /*&& bMenuBar*/ || this.IsOnMenu) 
            //                sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
            //            else/* if(bMenuBar)*/
            //            {
            //                System.Windows.Forms.Control ctrl=this.ContainerControl as System.Windows.Forms.Control;
            //                if(ctrl!=null && (ctrl.Focused || ctrl is Bar && ((Bar)ctrl).MenuFocus) || ctrl is NavigationBar)
            //                    sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
            //                else
            //                    sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Hide;
            //            }
            ////			else
            ////				sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Hide;

            //            //sfmt.FormatFlags=sfmt.FormatFlags & ~(sfmt.FormatFlags & StringFormatFlags.DisableKerning);
            //            if(_FitContainer)
            //            {
            //                sfmt.FormatFlags=sfmt.FormatFlags & ~(sfmt.FormatFlags & StringFormatFlags.NoWrap);
            //            }
            //            else
            //            {
            //                sfmt.FormatFlags=sfmt.FormatFlags | StringFormatFlags.NoWrap;
            //            }
            //            sfmt.Trimming=StringTrimming.EllipsisCharacter;
            //            sfmt.Alignment=System.Drawing.StringAlignment.Near;
            //            sfmt.LineAlignment=System.Drawing.StringAlignment.Center;

            //            return sfmt;
        }

        private ThemeTextFormat GetThemeTextFormat()
        {
            ThemeTextFormat format = ThemeTextFormat.Left | ThemeTextFormat.HidePrefix |
                ThemeTextFormat.WordEllipsis |
                ThemeTextFormat.VCenter;
            return format;
        }

        private Font _MouseOverFont = null;
        /// <summary>
        /// Returns the Font object to be used for drawing the item text.
        /// </summary>
        /// <returns>Font object.</returns>
        internal Font GetFont(ItemPaintArgs pa, bool isLayout)
        {
            System.Drawing.Font objFont = null;

            if (pa != null)
                objFont = pa.Font;

            if (objFont == null)
            {
                System.Windows.Forms.Control objCtrl = null;
                if (pa != null)
                    objCtrl = pa.ContainerControl;
                if (objCtrl == null)
                    objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                if (objCtrl != null && objCtrl.Font != null)
                    objFont = (Font)objCtrl.Font;
                else
                    objFont = (Font)System.Windows.Forms.SystemInformation.MenuFont;
            }

            if (m_MouseOver || isLayout && (m_HotFontBold || m_HotFontUnderline))
            {
                if (_MouseOverFont != null && objFont != null && objFont.FontFamily.Name == _MouseOverFont.FontFamily.Name && objFont.SizeInPoints == _MouseOverFont.SizeInPoints)
                    return _MouseOverFont;
                try
                {
                    Font mouseOverFont = null;
                    FontStyle fontStyle = objFont.Style;
                    if (m_HotFontBold) fontStyle |= FontStyle.Bold;
                    if (m_HotFontUnderline) fontStyle |= FontStyle.Underline;
                    mouseOverFont = new Font(objFont, fontStyle);
                    DisposeMouseOverFont();
                    _MouseOverFont = mouseOverFont;
                    return _MouseOverFont;
                }
                catch
                {

                }
            }

            if (m_FontBold || m_FontItalic || m_FontUnderline)
            {
                FontStyle style = objFont.Style;
                if (m_FontBold)
                    style = style | System.Drawing.FontStyle.Bold;
                if (m_FontItalic)
                    style = style | System.Drawing.FontStyle.Italic;
                if (m_FontUnderline)
                    style = style | System.Drawing.FontStyle.Underline;
                Font font = null;
                try
                {
                    font = new Font(objFont, style);
                }
                catch { }
                if (font != null)
                    objFont = font;
            }

            return objFont;
        }

        private void DisposeMouseOverFont()
        {
            if (_MouseOverFont != null)
            {
                _MouseOverFont.Dispose();
                _MouseOverFont = null;
            }
        }

        /// <summary>
        /// Gets or sets whether Checked property is automatically inverted, button checked/unchecked, when button is clicked. Default value is false.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
        public virtual bool AutoCheckOnClick
        {
            get { return m_AutoCheckOnClick; }
            set { m_AutoCheckOnClick = value; }
        }

        /// <summary>
        /// Gets or set a value indicating whether the button is in the checked state.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), Description("Indicates whether item is checked or not."), DefaultValue(false), Bindable(true)]
        public virtual bool Checked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                if (m_Checked != value)
                {
                    // Allow user to cancel the checking
                    if (value && m_OptionGroup.Length > 0 && this.Parent != null)
                    {
                        ButtonItem b = null;
                        foreach (BaseItem item in this.Parent.SubItems)
                        {
                            if (item == this)
                                continue;
                            b = item as ButtonItem;
                            if (b != null && b.OptionGroup == m_OptionGroup && b.Checked)
                            {
                                break;
                            }
                        }
                        OptionGroupChangingEventArgs e = new OptionGroupChangingEventArgs(b, this);
                        InvokeOptionGroupChanging(e);
                        if (e.Cancel)
                            return;
                    }

                    m_Checked = value;
                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "Checked");
                    this.OnCheckedChanged();
                    if (this.Displayed)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Called after Checked property has changed.
        /// </summary>
        protected virtual void OnCheckedChanged()
        {
            if (m_OptionGroup != "" && m_Checked && this.Parent != null)
            {
                foreach (BaseItem item in this.Parent.SubItems)
                {
                    if (item == this)
                        continue;
                    ButtonItem b = item as ButtonItem;
                    if (b != null && b.OptionGroup == m_OptionGroup && b.Checked)
                        b.Checked = false;
                }
            }
            InvokeCheckedChanged();
        }

        /// <summary>
        /// Called when Visibility of the items has changed.
        /// </summary>
        /// <param name="bVisible">New Visible state.</param>
        protected internal override void OnVisibleChanged(bool bVisible)
        {
            base.OnVisibleChanged(bVisible);

            if (!bVisible)
                StopFade();

            if (!bVisible && this.Checked && m_OptionGroup != "")
            {
                this.Checked = false;
                // Try to check first item in the group
                if (this.Parent != null)
                {
                    foreach (BaseItem item in this.Parent.SubItems)
                    {
                        if (item == this || !item.GetEnabled() || !item.Visible)
                            continue;
                        ButtonItem b = item as ButtonItem;
                        if (b != null && b.OptionGroup == m_OptionGroup)
                        {
                            b.Checked = true;
                            break;
                        }
                    }
                }
            }
            else if (bVisible && m_OptionGroup != "" && this.VisibleSubItems <= 1)
            {
                bool checkIt = true;
                // Try to check first item in the group
                if (this.Parent != null)
                {
                    foreach (BaseItem item in this.Parent.SubItems)
                    {
                        if (item == this || !item.GetEnabled() || !item.Visible)
                            continue;
                        ButtonItem b = item as ButtonItem;
                        if (b != null && b.OptionGroup == m_OptionGroup && b.Checked)
                        {
                            checkIt = false;
                            break;
                        }
                    }
                }
                if (checkIt)
                    this.Checked = true;
            }
        }

        /// <summary>
        /// Sets Checked property without firing any events or performing any built-in logic.
        /// </summary>
        internal void SetChecked(bool b)
        {
            m_Checked = b;
        }

        /// <summary>
        /// Fires CheckedChanged event.
        /// </summary>
        protected virtual void InvokeCheckedChanged()
        {
            if (CheckedChanged != null)
                CheckedChanged(this, new EventArgs());
            IOwnerItemEvents owner = this.GetIOwnerItemEvents();
            if (owner != null)
                owner.InvokeCheckedChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires OptionGroupChanging event.
        /// </summary>
        protected virtual void InvokeOptionGroupChanging(OptionGroupChangingEventArgs e)
        {
            if (OptionGroupChanging != null)
                OptionGroupChanging(this, e);
            if (!e.Cancel)
            {
                IOwnerItemEvents owner = this.GetIOwnerItemEvents();
                if (owner != null)
                    owner.InvokeOptionGroupChanging(this, e);
            }
        }

        /// <summary>
        /// Occurs just before Click event is fired.
        /// </summary>
        protected override void OnClick()
        {
            if (m_AutoCheckOnClick && m_OptionGroup == "")
                this.Checked = !this.Checked;
            base.OnClick();
            if (m_OptionGroup != "" && !m_Checked)
                this.Checked = true;
            ExecuteCommand();
        }

        protected override void OnCommandChanged()
        {
            if (!this.DesignMode && this.Command == null)
                this.Enabled = false;
            base.OnCommandChanged();
        }

        /// <summary>
        /// Gets or set the alternative shortcut text.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Design"), System.ComponentModel.Description("Gets or set the alternative Shortcut Text.  This text appears next to the Text instead of any shortcuts"), System.ComponentModel.DefaultValue(""), Localizable(true)]
        public virtual string AlternateShortCutText
        {
            get
            {
                return m_AlternateShortcutText;
            }
            set
            {
                m_AlternateShortcutText = value;
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Returns shortcut text if any that needs to be displayed.
        /// </summary>
        internal string DrawShortcutText
        {
            get
            {
                if (this.AlternateShortCutText != "")
                    return this.AlternateShortCutText;
                else
                    return this.ShortcutString;
            }
        }

        /// <summary>
        /// Returns the shortcut string that is displayed on tooltip.
        /// </summary>
        /// <returns></returns>
        protected override string GetTooltipShortcutString()
        {
            return this.DrawShortcutText;
        }

        /// <summary>
        /// Gets or sets the image bounds.
        /// </summary>
        internal Rectangle ImageDrawRect
        {
            get { return m_ImageDrawRect; }
            set { m_ImageDrawRect = value; }
        }

        /// <summary>
        /// Gets or sets sub items bounds.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle SubItemsRect
        {
            get { return m_SubItemsRect; }
            set { m_SubItemsRect = value; }
        }

        /// <summary>
        /// Gets or sets text bounds.
        /// </summary>
        internal Rectangle TextDrawRect
        {
            get { return m_TextDrawRect; }
            set { m_TextDrawRect = value; }
        }


        /// <summary>
        /// Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group. The choice is reflected by Checked property.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Behavior"), System.ComponentModel.Description("Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group."), System.ComponentModel.DefaultValue("")]
        public virtual string OptionGroup
        {
            get
            {
                return m_OptionGroup;
            }
            set
            {
                if (m_OptionGroup != value)
                {
                    m_OptionGroup = value;
                    if (m_OptionGroup != "" && m_Checked && this.Parent != null)
                    {
                        foreach (BaseItem item in this.Parent.SubItems)
                        {
                            if (item == this)
                                continue;
                            ButtonItem b = item as ButtonItem;
                            if (b != null && b.OptionGroup == m_OptionGroup && b.Checked)
                                this.Checked = false;
                        }
                    }
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text color of the button.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The foreground color used to display text.")]
        public virtual Color ForeColor
        {
            get
            {
                return m_ForeColor;
            }
            set
            {
                if (m_ForeColor != value)
                {
                    m_ForeColor = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ForeColor");

                    if (this.Displayed)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeForeColor()
        {
            if (m_ForeColor.IsEmpty)
                return false;
            return true;
        }

        /// <summary>
        /// Gets or sets the text color of the button when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The foreground color used to display text when mouse is over the item.")]
        public virtual Color HotForeColor
        {
            get
            {
                return m_HotForeColor;
            }
            set
            {
                if (m_HotForeColor != value)
                {
                    m_HotForeColor = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotForeColor");

                    if (this.Displayed && m_MouseOver)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializeHotForeColor()
        {
            if (m_HotForeColor.IsEmpty)
                return false;
            return true;
        }

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is underlined when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies that text font is underlined when mouse is over the item."), System.ComponentModel.DefaultValue(false)]
        public virtual bool HotFontUnderline
        {
            get
            {
                return m_HotFontUnderline;
            }
            set
            {
                if (m_HotFontUnderline != value)
                {
                    DisposeMouseOverFont();
                    m_HotFontUnderline = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotFontUnderline");

                    if (this.Displayed && m_MouseOver)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is bold when mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies that text font is bold when mouse is over the item."), System.ComponentModel.DefaultValue(false)]
        public virtual bool HotFontBold
        {
            get
            {
                return m_HotFontBold;
            }
            set
            {
                if (m_HotFontBold != value)
                {
                    DisposeMouseOverFont();
                    m_HotFontBold = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "HotFontBold");

                    if (this.Displayed && m_MouseOver)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is bold.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies whether the font used to draw the item text is bold."), System.ComponentModel.DefaultValue(false)]
        public virtual bool FontBold
        {
            get
            {
                return m_FontBold;
            }
            set
            {
                if (m_FontBold != value)
                {
                    m_FontBold = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "FontBold");

                    if (this.Displayed)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is italic.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies whether the font used to draw the item text is italic."), System.ComponentModel.DefaultValue(false)]
        public virtual bool FontItalic
        {
            get
            {
                return m_FontItalic;
            }
            set
            {
                if (m_FontItalic != value)
                {
                    m_FontItalic = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "FontItalic");

                    if (this.Displayed)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the font used to draw the item text is underlined.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Specifies whether the font used to draw the item text is underlined."), System.ComponentModel.DefaultValue(false)]
        public virtual bool FontUnderline
        {
            get
            {
                return m_FontUnderline;
            }
            set
            {
                if (m_FontUnderline != value)
                {
                    m_FontUnderline = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "FontUnderline");

                    if (this.Displayed)
                        this.Refresh();
                    OnAppearanceChanged();
                }
            }
        }


        /// <summary>
        /// Gets or sets the width of the expand part of the button item.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates the width of the expand part of the button item."), DefaultValue(12)]
        public virtual int SubItemsExpandWidth
        {
            get { return m_SubItemsExpandWidth; }
            set
            {
                m_SubItemsExpandWidth = value;
                NeedRecalcSize = true;
                if (this.DesignMode)
                    this.Refresh();
                OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Returns the collection of sub items.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(false), System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ButtonItemEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Collection of sub items."), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public override SubItemsCollection SubItems
        {
            get
            {
                return base.SubItems;
            }
        }

        /// <summary>
        /// Gets or sets whether button appears as split button. Split button appearance divides button into two parts. Image which raises the click event
        /// when clicked and text and expand sign which shows button sub items on popup menu when clicked. Button must have both text and image visible (ButtonStyle property) in order to appear as a full split button.
        /// Use AutoExpandOnClick=true if you want to make complete surface of the button display popup when clicked.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates whether button appears as split button. Use AutoExpandOnClick=true if you want to make complete surface of the button display popup when clicked.")]
        public virtual bool SplitButton
        {
            get { return m_SplitButton; }
            set
            {
                m_SplitButton = value;
                OnAppearanceChanged();
            }
        }

        internal Rectangle GetTotalSubItemsRect()
        {
            Rectangle r = this.SubItemsRect;
            if (this.SplitButton && (this.ButtonStyle == eButtonStyle.ImageAndText || this.ImagePosition == eImagePosition.Top || this.ImagePosition == eImagePosition.Bottom)
                && (this.Image != null || this.ImageIndex >= 0) && this.Text.Length > 0)
            {
                Rectangle rText = this.TextDrawRect;
                rText.Inflate(0, -2);
                if (this.ImagePosition == eImagePosition.Left)
                {
                    if (this.Orientation == eOrientation.Horizontal)
                        rText.X -= 3;
                    else
                        rText.Y -= 6;
                }
                if (r.IsEmpty)
                {
                    r = rText;
                    if (m_ImagePosition == eImagePosition.Top)
                    {
                        r.X = 0;// m_Rect.X - 1;
                        r.Width = m_Rect.Width;
                        r.Height = this.DisplayRectangle.Bottom - r.Y;
                    }
                    else if (m_ImagePosition == eImagePosition.Bottom)
                    {
                        r.X = 0;// m_Rect.X - 1;
                        r.Width = m_Rect.Width;
                    }
                }
                else
                    r = Rectangle.Union(r, rText);
                return r;
            }
            return r;
        }

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("The text contained in the item."), System.ComponentModel.Localizable(true), System.ComponentModel.DefaultValue("")]
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

        internal int VerticalPadding
        {
            get { return m_VerticalPadding; }
            set { m_VerticalPadding = value; }
        }

        internal int HorizontalPadding
        {
            get { return m_HorizontalPadding; }
            set { m_HorizontalPadding = value; }
        }

        /// <summary>
        /// Gets or sets the amount of padding added horizontally to the button images when not on menus. Default value is 10 pixels.
        /// </summary>
        [Browsable(true), DefaultValue(8), Category("Layout"), Description("Indicates amount of padding added horizontally to the button images when not on menus")]
        public virtual int ImagePaddingHorizontal
        {
            get { return m_ImagePaddingHorizontal; }
            set
            {
                m_ImagePaddingHorizontal = value;
                this.OnAppearanceChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of padding added vertically to the button images when not on menus. Default value is 6 pixels.
        /// </summary>
        [Browsable(true), DefaultValue(6), Category("Layout"), Description("Indicates amount of padding added vertically to the button images when not on menus")]
        public virtual int ImagePaddingVertical
        {
            get { return m_ImagePaddingVertical; }
            set
            {
                m_ImagePaddingVertical = value;
                this.OnAppearanceChanged();
            }
        }
        /// <summary>
        /// Gets or sets the current font for the button.
        /// </summary>
        //		public System.Drawing.Font Font
        //		{
        //			get
        //			{
        //				return m_Font;
        //			}
        //			set
        //			{
        //				if(m_Font!=value)
        //				{
        //					m_Font=value;
        //					if(this.Displayed)
        //						this.Refresh();
        //				}
        //			}
        //		}

        private ShapeDescriptor _Shape = null;
        /// <summary>
        /// Gets or sets an shape decriptor for the button which describes the shape of the button. Default value is null
        /// which indicates that system default shape is used.
        /// </summary>
        [DefaultValue(null), Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), MergableProperty(false)]
        public virtual ShapeDescriptor Shape
        {
            get { return _Shape; }
            set
            {
                if (_Shape != value)
                {
                    _Shape = value;
                    this.OnAppearanceChanged();
                }
            }
        }

        protected override void OnDisplayedChanged()
        {
            // Reset all internal states upon display changed
            m_MouseDown = false;
            m_MouseOver = false;
        }

        // IPersonalizedMenuItem Impementation
        /// <summary>
        /// Indicates item's visibility when on pop-up menu.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates item's visiblity when on pop-up menu."), System.ComponentModel.DefaultValue(eMenuVisibility.VisibleAlways)]
        public virtual eMenuVisibility MenuVisibility
        {
            get
            {
                return m_MenuVisibility;
            }
            set
            {
                if (m_MenuVisibility != value)
                {
                    m_MenuVisibility = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "MenuVisibility");
                }
            }
        }

        /// <summary>
        /// Indicates whether item was recently used.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool RecentlyUsed
        {
            get
            {
                return m_RecentlyUsed;
            }
            set
            {
                if (m_RecentlyUsed != value)
                {
                    m_RecentlyUsed = value;

                    if (ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "RecentlyUsed");

                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether mouse is over the item.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool IsMouseOver
        {
            get { return m_MouseOver; }
        }

        /// <summary>
        /// Indicates whether mouse is over the expand part of the button.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool IsMouseOverExpand
        {
            get { return m_MouseOverExpand; }
        }

        /// <summary>
        /// Indicates whether mouse is pressed.
        /// </summary>
        [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public bool IsMouseDown
        {
            get { return m_MouseDown; }
        }

        /// <summary>
        /// Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Appearance"), System.ComponentModel.Description("Indicates the way item is painting the picture when mouse is over it. Setting the value to Color will render the image in gray-scale when mouse is not over the item."), System.ComponentModel.DefaultValue(eHotTrackingStyle.Default)]
        public virtual eHotTrackingStyle HotTrackingStyle
        {
            get { return m_HotTrackingStyle; }
            set
            {
                if (m_HotTrackingStyle == value)
                    return;
                m_HotTrackingStyle = value;
                if (ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "HotTrackingStyle");
                this.Refresh();
                OnAppearanceChanged();
            }
        }

        // Property Editor support for ImageIndex selection
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                IOwner owner = this.GetOwner() as IOwner;
                if (owner != null)
                    return owner.Images;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets whether the button text is automatically wrapped over multiple lines when button is used on RibbonBar control. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Localizable(true), Category("Ribbon"), Description("Indicates whether the button text is automatically wrapped over multiple lines when button is used on RibbonBar control.")]
        public virtual bool RibbonWordWrap
        {
            get { return m_RibbonWordWrap; }
            set
            {
                if (m_RibbonWordWrap != value)
                {
                    m_RibbonWordWrap = value;
                    this.OnAppearanceChanged();
                }
            }
        }
        #endregion

        #region Markup Implementation
        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return _EnableMarkup; }
        }

        private bool _EnableMarkup = true;
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return _EnableMarkup; }
            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    NeedRecalcSize = true;
                    OnTextChanged();
                }
            }
        }

        #endregion

        #region ButtonItemAccessibleObject
        /// <summary>
        /// Represents accessible interface for ButtonItem object.
        /// </summary>
        public class ButtonItemAccessibleObject : BaseItem.ItemAccessibleObject
        {
            private ButtonItemPartAccessibleObject m_PushButtonPart = null;
            private ButtonItemPartAccessibleObject m_ExpandPart = null;

            public ButtonItemAccessibleObject(BaseItem owner) : base(owner) { }

            internal ButtonItem ButtonItem
            {
                get { return this.Owner as ButtonItem; }
            }

            private ButtonItemPartAccessibleObject AccessiblePushButtonPart
            {
                get
                {
                    if (m_PushButtonPart == null)
                    {
                        m_PushButtonPart = new ButtonItemPartAccessibleObject(this.ButtonItem, true);
                    }
                    return m_PushButtonPart;
                }
            }

            private ButtonItemPartAccessibleObject AccessibleExpandPart
            {
                get
                {
                    if (m_ExpandPart == null)
                    {
                        m_ExpandPart = new ButtonItemPartAccessibleObject(this.ButtonItem, false);
                    }
                    return m_ExpandPart;
                }
            }

            public override System.Windows.Forms.AccessibleRole Role
            {
                get
                {
                    if (IsSplitButton)
                        return System.Windows.Forms.AccessibleRole.Grouping;
                    return base.Role;
                }
            }

            private bool IsSplitButton
            {
                get
                {
                    ButtonItem b = this.ButtonItem;
                    if (b.VisibleSubItems > 0 && !b.AutoExpandOnClick && !b.IsOnMenu && !b.IsOnMenuBar)
                        return true;
                    return false;
                }
            }

            public override System.Windows.Forms.AccessibleStates State
            {
                get
                {
                    if (this.IsSplitButton)
                    {
                        if (this.Owner == null || !this.Owner.IsAccessible)
                            return System.Windows.Forms.AccessibleStates.Unavailable;

                        System.Windows.Forms.AccessibleStates state = 0;

                        if (!this.Owner.Displayed || !this.Owner.Visible)
                            state |= System.Windows.Forms.AccessibleStates.Invisible;
                        else if (!this.Owner.GetEnabled())
                        {
                            return System.Windows.Forms.AccessibleStates.Unavailable;
                        }

                        return System.Windows.Forms.AccessibleStates.Default;
                    }

                    System.Windows.Forms.AccessibleStates st = base.State;

                    if (this.ButtonItem.GetEnabled())
                    {
                        if (this.ButtonItem.Checked || this.ButtonItem.IsMouseDown)
                            st |= System.Windows.Forms.AccessibleStates.Pressed;
                    }
                    return st;
                }
            }

            public override int GetChildCount()
            {
                if (IsSplitButton)
                    return 2;
                return base.GetChildCount();
            }

            public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
            {
                if (IsSplitButton)
                {
                    if (iIndex == 0)
                        return this.AccessiblePushButtonPart;
                    else if (iIndex == 1)
                        return this.AccessibleExpandPart;
                }

                return base.GetChild(iIndex);
            }

            public override AccessibleObject Navigate(AccessibleNavigation navdir)
            {
                if (IsSplitButton)
                {
                    //if (navdir == AccessibleNavigation.FirstChild)
                    //    return this.AccessiblePushButtonPart;
                    //else if (navdir == AccessibleNavigation.LastChild)
                    //    return this.AccessibleExpandPart;
                    return null;
                }
                return base.Navigate(navdir);
            }

            public override string DefaultAction
            {
                get
                {
                    if (this.IsSplitButton)
                        return "";
                    else
                        return base.DefaultAction;
                }
            }

            public override void DoDefaultAction()
            {
                if (!this.IsSplitButton)
                    base.DoDefaultAction();
            }

            public override string KeyboardShortcut
            {
                get
                {
                    return "";
                }
            }

            public override System.Windows.Forms.AccessibleObject GetSelected()
            {
                if (this.IsSplitButton)
                {
                    if (this.ButtonItem.IsMouseOverExpand)
                        return this.AccessibleExpandPart;
                    else if (this.ButtonItem.IsMouseOver)
                        return this.AccessiblePushButtonPart;
                    return null;
                }
                else
                    return base.GetSelected();
            }

            public override System.Windows.Forms.AccessibleObject HitTest(int x, int y)
            {
                if (this.IsSplitButton)
                {
                    Rectangle r = this.AccessiblePushButtonPart.Bounds;
                    if (r.Contains(x, y))
                        return this.AccessiblePushButtonPart;
                    r = this.AccessibleExpandPart.Bounds;
                    if (r.Contains(x, y))
                        return this.AccessibleExpandPart;

                    return null;
                }
                return base.HitTest(x, y);
            }
        }

        /// <summary>
        /// Represents accessible interface for ButtonItem object.
        /// </summary>
        public class ButtonItemPartAccessibleObject : System.Windows.Forms.AccessibleObject
        {
            private ButtonItem m_Button = null;
            //private bool m_Hot = false;
            private bool m_PushButtonPart = true;

            public ButtonItemPartAccessibleObject(ButtonItem owner, bool pushButtonPart)
            {
                m_Button = owner;
                m_PushButtonPart = pushButtonPart;
            }

            public override string Name
            {
                get
                {
                    if (!m_PushButtonPart) return "Open";

                    if (m_Button == null)
                        return "";

                    if (m_Button.AccessibleName != "")
                        return m_Button.AccessibleName;

                    if (m_Button.Text != null)
                        return m_Button.Text.Replace("&", "");

                    return m_Button.Tooltip;
                }
                set
                {
                    m_Button.AccessibleName = value;
                }
            }

            public override string Description
            {
                get
                {
                    if (m_Button == null)
                        return "";
                    if (m_Button.AccessibleDescription != "")
                        return m_Button.AccessibleDescription;
                    return Name + " button";
                }
            }

            public override System.Windows.Forms.AccessibleRole Role
            {
                get
                {
                    if (m_PushButtonPart)
                    {
#if FRAMEWORK20
                        return System.Windows.Forms.AccessibleRole.SplitButton;
#else
						return System.Windows.Forms.AccessibleRole.PushButton;
#endif
                    }

                    return System.Windows.Forms.AccessibleRole.ButtonDropDown;
                }
            }

            public override System.Windows.Forms.AccessibleStates State
            {
                get
                {
                    if (m_Button == null)
                        return System.Windows.Forms.AccessibleStates.Unavailable;

                    System.Windows.Forms.AccessibleStates state = 0;

                    if (!m_Button.IsAccessible)
                        return System.Windows.Forms.AccessibleStates.Unavailable;

                    if (!m_Button.Displayed || !m_Button.Visible)
                        state |= System.Windows.Forms.AccessibleStates.Invisible;
                    else if (!m_Button.GetEnabled())
                    {
                        state = System.Windows.Forms.AccessibleStates.Unavailable;
                    }
                    else
                    {
                        if (m_PushButtonPart)
                        {
                            if (m_Button.IsMouseOver && !m_Button.IsMouseOverExpand)
                                state |= System.Windows.Forms.AccessibleStates.HotTracked | System.Windows.Forms.AccessibleStates.Focused;
                            else
                                state |= System.Windows.Forms.AccessibleStates.Focusable;
                        }
                        else
                        {
                            if (m_Button.Expanded || m_Button.IsMouseOverExpand)
                                state |= (System.Windows.Forms.AccessibleStates.Focused | System.Windows.Forms.AccessibleStates.HotTracked);
                            if (m_Button.Expanded)
                                state |= System.Windows.Forms.AccessibleStates.Expanded;
                            else
                                state |= System.Windows.Forms.AccessibleStates.Collapsed;
                        }
                    }
                    return state;
                }
            }

            public override System.Windows.Forms.AccessibleObject Parent
            {
                get
                {
                    return m_Button.AccessibleObject;
                }
            }

            public override System.Drawing.Rectangle Bounds
            {
                get
                {
                    System.Windows.Forms.Control parent = m_Button.ContainerControl as System.Windows.Forms.Control;
                    Rectangle r = Rectangle.Empty;

                    if (m_PushButtonPart)
                    {
                        Rectangle rs = m_Button.SubItemsRect;
                        rs.Offset(m_Button.Bounds.Location);
                        r = m_Button.Bounds;
                        if (rs.X == r.X && rs.Y == r.Y)
                        {
                            r.X += rs.Width;
                            r.Width -= rs.Width;
                        }
                        else if (rs.X == r.X && rs.Bottom == r.Bottom && rs.Y > r.Y)
                        {
                            r.Y += rs.Height;
                            r.Height -= rs.Height;
                        }
                        else
                        {
                            r.Width -= rs.Width;
                        }
                    }
                    else
                    {
                        r = m_Button.SubItemsRect;
                        r.Offset(m_Button.Bounds.Location);
                    }
                    if (parent != null)
                        r.Location = parent.PointToScreen(r.Location);
                    return r;
                }
            }

            public override int GetChildCount()
            {
                if (m_PushButtonPart)
                    return 0;
                return m_Button.SubItems.Count;
            }

            public override System.Windows.Forms.AccessibleObject GetChild(int iIndex)
            {
                if (m_Button == null || m_PushButtonPart)
                    return null;

                return m_Button.SubItems[iIndex].AccessibleObject;
            }

            public override string DefaultAction
            {
                get
                {
                    if (m_Button.AccessibleDefaultActionDescription != "")
                        return m_Button.AccessibleDefaultActionDescription;
                    if (m_PushButtonPart)
                        return "Press";
                    else
                        return "Open";
                }
            }

            public override void DoDefaultAction()
            {
                if (m_Button == null)
                    return;

                if (m_PushButtonPart)
                    m_Button._AccessibleExpandAction = false;
                else
                    m_Button._AccessibleExpandAction = true;

                System.Windows.Forms.Control cont = m_Button.ContainerControl as System.Windows.Forms.Control;
                if (cont is MenuPanel && !(cont is IAccessibilitySupport))
                {
                    cont = ((MenuPanel)cont).ParentItem.ContainerControl as System.Windows.Forms.Control;
                }

                IAccessibilitySupport ias = cont as IAccessibilitySupport;
                if (ias != null)
                {
                    ias.DoDefaultActionItem = m_Button;
                    NativeFunctions.PostMessage(cont.Handle.ToInt32(), NativeFunctions.WM_USER + 107, 0, 0);
                }

                base.DoDefaultAction();
            }

            public override string KeyboardShortcut
            {
                get
                {
                    return m_Button.ShortcutString;
                }
            }

            public override System.Windows.Forms.AccessibleObject GetSelected()
            {
                if (m_Button == null || m_PushButtonPart)
                    return base.GetSelected();

                foreach (BaseItem item in m_Button.SubItems)
                {
                    if ((item.AccessibleObject.State & System.Windows.Forms.AccessibleStates.HotTracked) == System.Windows.Forms.AccessibleStates.HotTracked)
                        return item.AccessibleObject;
                }

                return base.GetSelected();
            }

            public override System.Windows.Forms.AccessibleObject HitTest(int x, int y)
            {
                if (m_Button == null)
                    return base.HitTest(x, y);

                Point screen = new Point(x, y);
                foreach (BaseItem item in m_Button.SubItems)
                {
                    System.Windows.Forms.Control cont = item.ContainerControl as System.Windows.Forms.Control;
                    if (cont != null)
                    {
                        Point p = cont.PointToClient(screen);
                        if (item.DisplayRectangle.Contains(p))
                            return item.AccessibleObject;
                    }
                }

                return base.HitTest(x, y);
            }

            public override System.Windows.Forms.AccessibleObject Navigate(System.Windows.Forms.AccessibleNavigation navdir)
            {
                if (m_Button == null || m_PushButtonPart)
                    return base.Navigate(navdir);

                BaseItem item = null;

                if (navdir == System.Windows.Forms.AccessibleNavigation.Down || navdir == System.Windows.Forms.AccessibleNavigation.Right
                || navdir == System.Windows.Forms.AccessibleNavigation.Next)
                {
                    if (m_Button.Parent != null)
                    {
                        BaseItem parent = m_Button.Parent;
                        item = GetFirstVisible(parent.SubItems, parent.SubItems.IndexOf(m_Button) + 1);
                    }
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.FirstChild)
                {
                    item = GetFirstVisible(m_Button.SubItems, 0);
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.LastChild)
                {
                    item = GetFirstVisibleReverse(m_Button.SubItems, m_Button.SubItems.Count - 1);
                }
                else if (navdir == System.Windows.Forms.AccessibleNavigation.Up || navdir == System.Windows.Forms.AccessibleNavigation.Left
                    || navdir == System.Windows.Forms.AccessibleNavigation.Previous)
                {
                    BaseItem parent = m_Button.Parent;
                    item = GetFirstVisibleReverse(m_Button.SubItems, parent.SubItems.IndexOf(m_Button) - 1);
                }

                if (item != null)
                    return item.AccessibleObject;

                return base.Navigate(navdir);
            }

            private BaseItem GetFirstVisible(SubItemsCollection col, int startIndex)
            {
                int count = col.Count;
                if (count == 0) return null;
                if (startIndex >= col.Count) startIndex = col.Count - 1;

                for (int i = startIndex; i < count; i++)
                {
                    if (col[i].Visible)
                        return col[i];
                }
                return null;
            }

            private BaseItem GetFirstVisibleReverse(SubItemsCollection col, int startIndex)
            {
                if (col.Count == 0) return null;
                if (startIndex >= col.Count) startIndex = col.Count - 1;
                for (int i = startIndex; i >= 0; i--)
                {
                    if (col[i].Visible)
                        return col[i];
                }
                return null;
            }
        }
        #endregion
    }

    #region OptionGroupChangingEventHandler
    /// <summary>
    /// Delegate for OptionGroupChanging event.
    /// </summary>
    public delegate void OptionGroupChangingEventHandler(object sender, OptionGroupChangingEventArgs e);
    #endregion

    #region OptionGroupChangingEventArgs
    /// <summary>
    /// Represents event arguments for OptionGroupChanging event.
    /// </summary>
    public class OptionGroupChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Set to true to cancel the checking on NewChecked button.
        /// </summary>
        public bool Cancel = false;
        /// <summary>
        /// Button that will become checked if operation is not cancelled.
        /// </summary>
        public readonly ButtonItem NewChecked;
        /// <summary>
        /// Button that is currently checked and which will be unchecked if operation is not cancelled.
        /// </summary>
        public readonly ButtonItem OldChecked;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public OptionGroupChangingEventArgs(ButtonItem oldchecked, ButtonItem newchecked)
        {
            NewChecked = newchecked;
            OldChecked = oldchecked;
        }
    }
    #endregion

    #region SideBarImage
    /// <summary>
    /// Stores all information for side bar images that are used by Bar or Popup menu.
    /// </summary>
    public struct SideBarImage
    {
        /// <summary>
        /// Gets or sets the side bar image.
        /// </summary>
        public Image Picture;
        /// <summary>
        /// Gets or sets the side bar back color.
        /// </summary>
        public Color BackColor;
        /// <summary>
        /// Gets or sets the gradient staring color.
        /// </summary>
        public Color GradientColor1;
        /// <summary>
        /// Gets or sets the gradient ending color.
        /// </summary>
        public Color GradientColor2;
        /// <summary>
        /// Gets or sets the gradient angle.
        /// </summary>
        public float GradientAngle;
        /// <summary>
        /// Gets or sets the gradient staring color.
        /// </summary>
        public eAlignment Alignment;

        /// <summary>
        /// Gets or sets whether image is stretched so it fills the side bar or not if image is smaller than current side bar size.
        /// </summary>
        public bool StretchPicture;
    }
    #endregion
}
