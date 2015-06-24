using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the color picker drop down button.
    /// </summary>
    [DefaultEvent("SelectedColorChanged")]
    public class ColorPickerDropDown : ButtonItem
    {
        #region Events
        /// <summary>
        /// Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box. Selected color can be accessed through SelectedColor property.
        /// </summary>
        [Description("Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box.")]
        public event EventHandler SelectedColorChanged;

        /// <summary>
        /// Occurs when mouse is moving over the colors presented by the color picker. You can use it to preview the color before it is selected.
        /// </summary>
        [Description("Occurs when mouse is moving over the colors presented by the color picker")]
        public event ColorPreviewEventHandler ColorPreview;

        /// <summary>
        /// Occurs before color picker dialog is shown. Data property of the event arguments will hold the reference to the Form about to be shown.
        /// </summary>
        public event CancelObjectValueEventHandler BeforeColorDialog;
        #endregion

        #region Private Variables
        private bool m_ColorsInitialized = false;
        private bool m_DisplayThemeColors = true;
        private bool m_DisplayStandardColors = true;
        private bool m_DisplayMoreColors = true;

        private bool m_Localized = false;
        private string m_ThemeColorsLabel = "Theme Colors";
        private string m_StandardColorsLabel = "Standard Colors";
        private string m_MoreColorsLabel = "&More Colors...";
        private Color m_SelectedColor = System.Drawing.Color.Empty;
        private Rectangle m_SelectedColorRectangle = Rectangle.Empty;
        private bool m_SelectedImageCreated = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of ButtonItem.
        /// </summary>
        public ColorPickerDropDown() : this("", "") { }
        /// <summary>
        /// Creates new instance of ButtonItem and assigns the name to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        public ColorPickerDropDown(string sItemName) : this(sItemName, "") { }
        /// <summary>
        /// Creates new instance of ButtonItem and assigns the name and text to it.
        /// </summary>
        /// <param name="sItemName">Item name.</param>
        /// <param name="ItemText">item text.</param>
        public ColorPickerDropDown(string sItemName, string ItemText)
            : base(sItemName, ItemText)
        {
            //this.AutoExpandOnClick = true;
            this.SubItems.Add(new ButtonItem("tempColorPickerItem"));
        }
        #endregion

        #region Internal Implementation
        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            ColorPickerDropDown objCopy = new ColorPickerDropDown();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem c)
        {
            ColorPickerDropDown copy = c as ColorPickerDropDown;
            copy.DisplayMoreColors = this.DisplayMoreColors;
            copy.DisplayStandardColors = this.DisplayStandardColors;
            copy.DisplayThemeColors = this.DisplayThemeColors;
            copy.SelectedColorChanged = this.SelectedColorChanged;
            copy.CustomStandardColors = this.CustomStandardColors;
            copy.CustomThemeColors = this.CustomThemeColors;
            copy.SelectedColorImageRectangle = this.SelectedColorImageRectangle;
            copy.SelectedColor = this.SelectedColor;

            base.CopyToItem(copy);
        }


        /// <summary>
        /// Raises the ColorPreview event.
        /// </summary>
        /// <param name="e">Provides the data for the event.</param>
        protected virtual void OnColorPreview(ColorPreviewEventArgs e)
        {
            if (ColorPreview != null)
                ColorPreview(this, e);
        }

        /// <summary>
        /// Invokes the ColorPreview event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        public void InvokeColorPreview(ColorPreviewEventArgs e)
        {
            OnColorPreview(e);
        }

        /// <summary>
        /// Gets or sets the last selected color from either the drop-down or Custom Color dialog box. Default value is
        /// Color.Empty. You can use SelectedColorChanged event to be notified when this property changes.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor
        {
            get { return m_SelectedColor; }
            set { SetSelectedColor(value, false); }
        }

        /// <summary>
        /// Gets or sets whether theme colors are displayed on drop-down. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), DevCoSerialize(), Category("Appearance"), Description("Indicates whether theme colors are displayed on drop-down.")]
        public bool DisplayThemeColors
        {
            get { return m_DisplayThemeColors; }
            set { m_DisplayThemeColors = value; }
        }

        /// <summary>
        /// Gets or sets whether standard colors are displayed on drop-down. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), DevCoSerialize(), Category("Appearance"), Description("Indicates whether standard colors are displayed on drop-down.")]
        public bool DisplayStandardColors
        {
            get { return m_DisplayStandardColors; }
            set { m_DisplayStandardColors = value; }
        }

        /// <summary>
        /// Gets or sets more colors menu item is visible which allows user to open Custom Colors dialog box. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoBrowsable(true), DevCoSerialize(), Category("Appearance"), Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
        public bool DisplayMoreColors
        {
            get { return m_DisplayMoreColors; }
            set { m_DisplayMoreColors = value; }
        }

        /// <summary>
        /// Indicates whether SubItems collection is serialized. ColorPickerDropDown does not serialize the sub items.
        /// </summary>
        protected override bool ShouldSerializeSubItems()
        {
            return false;
        }

        /// <summary>
        /// Gets or sets the rectangle in Image coordinates where selected color will be painted. Setting this property will
        /// have an effect only if Image property is used to set the image. Default value is an empty rectangle which indicates
        /// that selected color will not be painted on the image.
        /// </summary>
        [Browsable(true), Description("Indicates rectangle in Image coordinates where selected color will be painted. Property will have effect only if Image property is used to set the image."), Category("Behaviour")]
        public Rectangle SelectedColorImageRectangle
        {
            get { return m_SelectedColorRectangle; }
            set
            {
                m_SelectedColorRectangle = value;
                UpdateSelectedColorImage();
            }
        }

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedColorImageRectangle()
        {
            return !m_SelectedColorRectangle.IsEmpty;
        }

        /// <summary>
        /// Resets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedColorImageRectangle()
        {
            SelectedColorImageRectangle = Rectangle.Empty;
        }

        ///// <summary>
        ///// Indicates whether the item will auto-expand when clicked. 
        ///// When item is on top level bar and not on menu and contains sub-items, sub-items will be shown only if user
        ///// click the expand part of the button. Setting this property to true will expand the button and show sub-items when user
        ///// clicks anywhere inside of the button. Default value is true which indicates that button is expanded only
        ///// if its expand part is clicked.
        ///// </summary>
        //[DefaultValue(true), Browsable(true), DevCoBrowsable(true), Category("Behavior"), Description("Indicates whether the item will auto-expand (display pop-up menu or toolbar) when clicked.")]
        //public override bool AutoExpandOnClick
        //{
        //    get { return base.AutoExpandOnClick; }
        //    set { base.AutoExpandOnClick = value; }
        //}

        /// <summary>
        /// Gets whether internal representation of color items on popup has been initialized.
        /// </summary>
        [Browsable(false)]
        public bool ColorsInitialized
        {
            get
            {
                return m_ColorsInitialized;
            }
        }

        protected override void OnStyleChanged()
        {
            m_ColorsInitialized = false;
            base.OnStyleChanged();
        }

        protected override void OnPopupOpen(PopupOpenEventArgs e)
        {
            if (!m_ColorsInitialized)
                InitializeColorDropDown();
            base.OnPopupOpen(e);
        }

        private void InitializeColorDropDown()
        {
            this.SubItems.Clear();

            LoadResources();
            if (m_DisplayThemeColors)
            {
                LabelItem label = new LabelItem();
                label.CanCustomize = false;
                label.Text = m_ThemeColorsLabel;
                label.BackColor = ColorScheme.GetColor("DDE7EE");
                label.ForeColor = ColorScheme.GetColor("00156E");
                label.PaddingBottom = 1;
                label.PaddingLeft = 0;
                label.PaddingRight = 0;
                label.PaddingTop = 1;
                label.BorderSide = eBorderSide.Bottom;
                label.SingleLineColor = ColorScheme.GetColor("C5C5C5");
                label.BorderType = eBorderType.SingleLine;
                this.SubItems.Add(label);
                ArrayList colors = this.GetThemeColors();
                AddColors(colors);
            }

            if (m_DisplayStandardColors)
            {
                LabelItem label = new LabelItem();
                label.CanCustomize = false;
                label.Text = m_StandardColorsLabel;
                label.BackColor = ColorScheme.GetColor("DDE7EE");
                label.ForeColor = ColorScheme.GetColor("00156E");
                label.PaddingBottom = 1;
                label.PaddingLeft = 0;
                label.PaddingRight = 0;
                label.PaddingTop = 1;
                label.BorderSide = eBorderSide.Bottom;
                label.SingleLineColor = ColorScheme.GetColor("C5C5C5");
                label.BorderType = eBorderType.SingleLine;
                this.SubItems.Add(label);
                ArrayList colors = this.GetStandardColors();
                AddColors(colors);
            }

            if (m_DisplayMoreColors)
            {
                ButtonItem button = new ButtonItem("sys_CPMoreColors", m_MoreColorsLabel);
                button.CanCustomize = false;
                button.Image = BarFunctions.LoadBitmap("SystemImages.ColorPickerCustomColors.png");
                button.Click += new EventHandler(MoreColorsButtonClick);
                button.BeginGroup = true;
                this.SubItems.Add(button);
            }

            m_ColorsInitialized = true;
        }

        private void MoreColorsButtonClick(object sender, EventArgs e)
        {
            DisplayMoreColorsDialog();
        }

        /// <summary>
        /// Displays the Colors dialog that allows user to choose the color or create a custom color. If new color is chosen the
        /// SelectedColorChanged event is raised.
        /// </summary>
        public DialogResult DisplayMoreColorsDialog()
        {
            ColorPickerItem.CustomColorDialog d = new ColorPickerItem.CustomColorDialog();
            d.SetStyle(this.EffectiveStyle);
            d.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            if (!this.SelectedColor.IsEmpty)
                d.CurrentColor = this.SelectedColor;
            LocalizeDialog(d);
            CancelObjectValueEventArgs e = new CancelObjectValueEventArgs(d);
            OnBeforeColorDialog(e);
            if (e.Cancel)
            {
                d.Dispose();
                return DialogResult.None;
            }
            if (_OwnerWindow != null)
                d.ShowDialog(_OwnerWindow);
            else
                d.ShowDialog();

            DialogResult result = d.DialogResult;
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (!d.NewColor.IsEmpty)
                {
                    this.SetSelectedColor(d.NewColor);
                }
            }

            d.Dispose();
            return result;
        }

        /// <summary>
        /// Raises the BeforeColorDialog event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeforeColorDialog(CancelObjectValueEventArgs e)
        {
            CancelObjectValueEventHandler h = this.BeforeColorDialog;
            if (h != null)
                h(this, e);
        }

        private IWin32Window _OwnerWindow = null;
        /// <summary>
        /// Gets or sets the Owner Window that will be used as owner for the colors modal dialog when displayed.
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public IWin32Window OwnerWindow
        {
            get { return _OwnerWindow; }
            set { _OwnerWindow = value; }
        }

        private void LocalizeDialog(ColorPickerItem.CustomColorDialog d)
        {
            if (this.GetOwner() != null)
                m_Localized = true;
            using (LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize))
            {
                string s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogOKButton);
                if (s != "") d.buttonOK.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogCancelButton);
                if (s != "") d.buttonCancel.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogNewColorLabel);
                if (s != "") d.labelNewColor.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogCurrentColorLabel);
                if (s != "") d.labelCurrentColor.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogStandardColorsLabel);
                if (s != "") d.labelStandardColors.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogCustomColorsLabel);
                if (s != "") d.labelCustomColors.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogGreenLabel);
                if (s != "") d.labelGreen.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogBlueLabel);
                if (s != "") d.labelBlue.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogRedLabel);
                if (s != "") d.labelRed.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerTabStandard);
                if (s != "") d.tabItemStandard.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerTabCustom);
                if (s != "") d.tabItemCustom.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerCaption);
                if (s != "") d.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogColorModelLabel);
                if (s != "") d.labelColorModel.Text = s;
                s = lm.GetLocalizedString(LocalizationKeys.ColorPickerDialogRgbLabel);
                if (s != "") d.comboColorModel.Items[0] = s;
            }
        }

        private void AddColors(ArrayList colors)
        {
            int colorItemSpacing = 3;
            for (int i = 0; i < colors.Count; i++)
            {
                ItemContainer cont = new ItemContainer();
                cont.Orientation = eOrientation.Horizontal;
                cont.ItemSpacing = colorItemSpacing;
                if (i == 0)
                {
                    cont.BackgroundStyle.MarginBottom = 5;
                    cont.BackgroundStyle.MarginTop = 3;
                }
                if (i == colors.Count - 1)
                    cont.BackgroundStyle.MarginBottom = 3;

                ColorItem[] colorLine = (ColorItem[])colors[i];
                foreach (ColorItem ci in colorLine)
                {
                    cont.SubItems.Add(ci);
                    ci.Click += new EventHandler(ColorItemClick);
                    ci.AutoCollapseOnClick = false;
                }
                this.SubItems.Add(cont);
            }
        }

        private void ColorItemClick(object sender, EventArgs e)
        {
            ColorItem ci = sender as ColorItem;
            if (ci == null)
                return;

            SetSelectedColor(ci.Color);
            BaseItem.CollapseAll(this);
        }

        private void SetSelectedColor(Color c)
        {
            SetSelectedColor(c, true);
        }

        private void SetSelectedColor(Color c, bool raiseClick)
        {
            bool valueChanged = (m_SelectedColor != c);
            m_SelectedColor = c;

            OnSelectedColorChanged(new EventArgs());
            if (raiseClick)
                RaiseClick();

            if (valueChanged && ShouldSyncProperties)
                BarFunctions.SyncProperty(this, "SelectedColor");

            UpdateSelectedColorImage();
        }

        /// <summary>
        /// Update the selected color image if the SelectedColorImageRectangle has been set and button is using Image property to display the image.
        /// </summary>
        public void UpdateSelectedColorImage()
        {
            if (m_SelectedColorRectangle.IsEmpty || m_SelectedColorRectangle.Width <= 0 || m_SelectedColorRectangle.Height <= 0 ||
                this.Image == null || this.DesignMode) return;

            Bitmap bmp = new Bitmap(this.Image.Width, this.Image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle r = m_SelectedColorRectangle;
                g.DrawImage(this.Image, 0, 0);
                Color selColor = this.SelectedColor;
                bool noColor = false;
                if (selColor.IsEmpty || selColor == Color.Transparent)
                {
                    selColor = Color.White;
                    noColor = true;
                }
                using (SolidBrush b = new SolidBrush(selColor))
                {
                    g.FillRectangle(b, r);
                }
                if (noColor && r.Height > 5)
                {
                    r.Width--;
                    r.Height--;
                    using (Pen p = new Pen(Color.Black))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawLine(p, r.X, r.Y, r.Right, r.Bottom);
                        g.DrawLine(p, r.Right, r.Y, r.X, r.Bottom);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    }
                }
            }
            if (m_SelectedImageCreated)
            {
                Image img = this.Image;
                this.Image = bmp;
                img.Dispose();
            }
            else
                this.Image = bmp;
            m_SelectedImageCreated = true;
        }

#if FRAMEWORK20
        private ColorItem[][] _CustomThemeColors = null;
        /// <summary>
        /// Gets or sets the array of ColorItem objects that will be used as theme colors instead of built-in color palette.
        /// See: http://www.devcomponents.com/kb2/?p=79 for instructions.
        /// </summary>
        [DefaultValue(null), Category("Colors"), Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
        public ColorItem[][] CustomThemeColors
        {
            get { return _CustomThemeColors; }
            set
            {
                _CustomThemeColors = value;
                m_ColorsInitialized = false;
            }
        }
#endif

        /// <summary>
        /// Gets collection of ColorItem[] arrays that represent themed colors. Each ColorItem[] array is used to represent single line of theme colors. 
        /// </summary>
        /// <returns>Collection of ColorItem[] arrays.</returns>
        protected virtual ArrayList GetThemeColors()
        {
            ArrayList colors = new ArrayList();
#if FRAMEWORK20
            if (_CustomThemeColors == null || _CustomThemeColors.Length == 0)
#endif
            {
                if (EffectiveStyle == eDotNetBarStyle.Office2010)
                {
                    colors.Add(new ColorItem[] {
                        GetColorItem("FFFFFF"),
                        GetColorItem("000000"),
                        GetColorItem("EEECE1"),
                        GetColorItem("1F497D"),
                        GetColorItem("4F81BD"),
                        GetColorItem("C0504D"),
                        GetColorItem("9BBB59"),
                        GetColorItem("8064A2"),
                        GetColorItem("4BACC6"),
                        GetColorItem("F79646")});

                    colors.Add(new ColorItem[] {
                        GetColorItemTop("F2F2F2"),
                        GetColorItemTop("7F7F7F"),
                        GetColorItemTop("DDD9C3"),
                        GetColorItemTop("C6D9F0"),
                        GetColorItemTop("DBE5F1"),
                        GetColorItemTop("F2DCDB"),
                        GetColorItemTop("EBF1DD"),
                        GetColorItemTop("E5E0EC"),
                        GetColorItemTop("DBEEF3"),
                        GetColorItemTop("FDEADA")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("D8D8D8"),
                        GetColorItemMiddle("595959"),
                        GetColorItemMiddle("C4BD97"),
                        GetColorItemMiddle("8DB3E2"),
                        GetColorItemMiddle("B8CCE4"),
                        GetColorItemMiddle("E5B9B7"),
                        GetColorItemMiddle("D7E3BC"),
                        GetColorItemMiddle("CCC1D9"),
                        GetColorItemMiddle("B7DDE8"),
                        GetColorItemMiddle("FBD5B5")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("BFBFBF"),
                        GetColorItemMiddle("3F3F3F"),
                        GetColorItemMiddle("938953"),
                        GetColorItemMiddle("548DD4"),
                        GetColorItemMiddle("95B3D7"),
                        GetColorItemMiddle("D99694"),
                        GetColorItemMiddle("C3D69B"),
                        GetColorItemMiddle("B2A1C7"),
                        GetColorItemMiddle("92CDDC"),
                        GetColorItemMiddle("FAC08F")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("A5A5A5"),
                        GetColorItemMiddle("262626"),
                        GetColorItemMiddle("494429"),
                        GetColorItemMiddle("17365D"),
                        GetColorItemMiddle("366092"),
                        GetColorItemMiddle("953734"),
                        GetColorItemMiddle("76923C"),
                        GetColorItemMiddle("5F497A"),
                        GetColorItemMiddle("31859B"),
                        GetColorItemMiddle("E36C09")});

                    colors.Add(new ColorItem[] {
                        GetColorItemBottom("7F7F7F"),
                        GetColorItemBottom("0C0C0C"),
                        GetColorItemBottom("1D1B10"),
                        GetColorItemBottom("0F243E"),
                        GetColorItemBottom("244061"),
                        GetColorItemBottom("632423"),
                        GetColorItemBottom("4F6128"),
                        GetColorItemBottom("3F3151"),
                        GetColorItemBottom("205867"),
                        GetColorItemBottom("974806")});
                }
                else
                {
                    colors.Add(new ColorItem[] {
                        GetColorItem("FFFFFF"),
                        GetColorItem("000000"),
                        GetColorItem("FAF3E8"),
                        GetColorItem("1F497D"),
                        GetColorItem("5C83B4"),
                        GetColorItem("C0504D"),
                        GetColorItem("9DBB61"),
                        GetColorItem("8066A0"),
                        GetColorItem("4BACC6"),
                        GetColorItem("F59D56")});

                    colors.Add(new ColorItem[] {
                        GetColorItemTop("D8D8D8"),
                        GetColorItemTop("7F7F7F"),
                        GetColorItemTop("BBB6AE"),
                        GetColorItemTop("C7D1DE"),
                        GetColorItemTop("D6E0EC"),
                        GetColorItemTop("EFD3D2"),
                        GetColorItemTop("E6EED7"),
                        GetColorItemTop("DFD8E7"),
                        GetColorItemTop("D2EAF0"),
                        GetColorItemTop("FCE6D4")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("BFBFBF"),
                        GetColorItemMiddle("727272"),
                        GetColorItemMiddle("A29D96"),
                        GetColorItemMiddle("8FA4BE"),
                        GetColorItemMiddle("ADC1D9"),
                        GetColorItemMiddle("DFA7A6"),
                        GetColorItemMiddle("CEDDB0"),
                        GetColorItemMiddle("BFB2CF"),
                        GetColorItemMiddle("A5D5E2"),
                        GetColorItemMiddle("FACEAA")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("A5A5A5"),
                        GetColorItemMiddle("595959"),
                        GetColorItemMiddle("7D7974"),
                        GetColorItemMiddle("57769D"),
                        GetColorItemMiddle("84A2C6"),
                        GetColorItemMiddle("CF7B79"),
                        GetColorItemMiddle("B5CC88"),
                        GetColorItemMiddle("9F8CB7"),
                        GetColorItemMiddle("78C0D4"),
                        GetColorItemMiddle("F7B580")});

                    colors.Add(new ColorItem[] {
                        GetColorItemMiddle("8C8C8C"),
                        GetColorItemMiddle("3F3F3F"),
                        GetColorItemMiddle("575551"),
                        GetColorItemMiddle("17365D"),
                        GetColorItemMiddle("456287"),
                        GetColorItemMiddle("903C39"),
                        GetColorItemMiddle("758C48"),
                        GetColorItemMiddle("604C78"),
                        GetColorItemMiddle("388194"),
                        GetColorItemMiddle("B77540")});

                    colors.Add(new ColorItem[] {
                        GetColorItemBottom("7F7F7F"),
                        GetColorItemBottom("262626"),
                        GetColorItemBottom("3E3C3A"),
                        GetColorItemBottom("0F243E"),
                        GetColorItemBottom("2E415A"),
                        GetColorItemBottom("602826"),
                        GetColorItemBottom("4E5D30"),
                        GetColorItemBottom("403350"),
                        GetColorItemBottom("255663"),
                        GetColorItemBottom("7A4E2B")});
                }
            }
#if FRAMEWORK20
            else
                colors.AddRange(_CustomThemeColors);
#endif

            return colors;
        }

        /// <summary>
        /// Returns an array that represents the standard colors. Usually instances of ColorItem are included. 
        /// </summary>
        /// <returns>ArrayList containing objects that describe standard colors.</returns>
        protected virtual ArrayList GetStandardColors()
        {
            ArrayList colors = new ArrayList();
#if FRAMEWORK20
            if (_CustomStandardColors == null || _CustomStandardColors.Length == 0)
#endif
            {
                if (EffectiveStyle == eDotNetBarStyle.Office2010)
                {
                    colors.Add(new ColorItem[] {
                        GetColorItem("C00000"),
                        GetColorItem("FF0000"),
                        GetColorItem("FFC000"),
                        GetColorItem("FFFF00"),
                        GetColorItem("92D050"),
                        GetColorItem("00B050"),
                        GetColorItem("00B0F0"),
                        GetColorItem("0070C0"),
                        GetColorItem("002060"),
                        GetColorItem("7030A0")});
                }
                else
                {
                    colors.Add(new ColorItem[] {
                        GetColorItem("BA1419"),
                        GetColorItem("ED1C24"),
                        GetColorItem("FFC20E"),
                        GetColorItem("FFF200"),
                        GetColorItem("9DDA4E"),
                        GetColorItem("22B14C"),
                        GetColorItem("00B7EF"),
                        GetColorItem("0072BC"),
                        GetColorItem("2F3699"),
                        GetColorItem("6F3198")});
                }
            }
#if FRAMEWORK20
            else
                colors.AddRange(_CustomStandardColors);
#endif

            return colors;
        }
#if FRAMEWORK20
        private ColorItem[][] _CustomStandardColors = null;
        /// <summary>
        /// Gets or sets the array of ColorItem objects that will be used as standard colors instead of built-in color palette.
        /// See: http://www.devcomponents.com/kb2/?p=79 for instructions.
        /// </summary>
        [DefaultValue(null), Category("Colors"), Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
        public ColorItem[][] CustomStandardColors
        {
            get { return _CustomStandardColors; }
            set
            {
                _CustomStandardColors = value;
                m_ColorsInitialized = false;
            }
        }
#endif

        private ColorItem GetColorItem(string color)
        {
            ColorItem c = new ColorItem("", "", ColorScheme.GetColor(color));

            return c;
        }

        private ColorItem GetColorItemTop(string color)
        {
            ColorItem c = new ColorItem("", "", ColorScheme.GetColor(color));
            eColorItemBorder border = eColorItemBorder.Top | eColorItemBorder.Left | eColorItemBorder.Right;
            c.Border = border;
            return c;
        }

        private ColorItem GetColorItemMiddle(string color)
        {
            ColorItem c = new ColorItem("", "", ColorScheme.GetColor(color));
            eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right;
            c.Border = border;
            return c;
        }

        private ColorItem GetColorItemBottom(string color)
        {
            ColorItem c = new ColorItem("", "", ColorScheme.GetColor(color));
            eColorItemBorder border = eColorItemBorder.Bottom | eColorItemBorder.Left | eColorItemBorder.Right;
            c.Border = border;
            return c;
        }

        private void LoadResources()
        {
            if (!m_Localized)
            {
                if (this.GetOwner() != null)
                    m_Localized = true;
                using (LocalizationManager lm = new LocalizationManager(this.GetOwner() as IOwnerLocalize))
                {
                    string s = lm.GetLocalizedString(LocalizationKeys.ColorPickerThemeColorsLabel);
                    if (s != "") m_ThemeColorsLabel = s;
                    s = lm.GetLocalizedString(LocalizationKeys.ColorPickerStandardColorsLabel);
                    if (s != "") m_StandardColorsLabel = s;
                    s = lm.GetLocalizedString(LocalizationKeys.ColorPickerMoreColorsMenuItem);
                    if (s != "") m_MoreColorsLabel = s;
                }
            }
        }

        /// <summary>
        /// Invokes SelectedColorChanged event.
        /// </summary>
        protected virtual void OnSelectedColorChanged(EventArgs e)
        {
            if (SelectedColorChanged != null)
                SelectedColorChanged(this, e);
            DotNetBarManager manager = this.GetOwner() as DotNetBarManager;
            if (manager != null)
                manager.InvokeColorPickerSelectedColorChanged(this);
        }

        /// <summary>
        /// Indicates whether button should popup when clicked automatically.
        /// </summary>
        protected override bool ShouldAutoExpandOnClick
        {
            get { return base.ShouldAutoExpandOnClick || m_SelectedColor.IsEmpty; }
        }

        protected override void OnCommandChanged()
        {
        }
        #endregion
    }
}
