using System;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents the color picker button control.
    /// </summary>
    [ToolboxBitmap(typeof(ColorPickerButton), "Controls.ColorPickerButton.ico"), ToolboxItem(true), DefaultEvent("SelectedColorChanged"), Designer("DevComponents.DotNetBar.Design.ColorPickerButtonDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf"), System.Runtime.InteropServices.ComVisible(false)]
    public class ColorPickerButton : ButtonX
    {
        #region Private Variables
        #endregion

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
        #endregion

        #region Constructor
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public ColorPickerButton()
            : base()
        {
            ColorPickerDropDown cp = GetColorPickerDropDown();
            cp.SelectedColorChanged += new EventHandler(InternalSelectedColorChanged);
            cp.ColorPreview += new ColorPreviewEventHandler(InternalColorPreview);
        }
        #endregion

        #region Internal Implementation
#if FRAMEWORK20
        /// <summary>
        /// Gets or sets the array of ColorItem objects that will be used as standard colors instead of built-in color palette.
        /// See: http://www.devcomponents.com/kb2/?p=79 for instructions.
        /// </summary>
        [DefaultValue(null), Browsable(false), Category("Colors"), Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
        public ColorItem[][] CustomStandardColors
        {
            get { return GetColorPickerDropDown().CustomStandardColors; }
            set
            {
                GetColorPickerDropDown().CustomStandardColors = value;
            }
        }
        /// <summary>
        /// Gets or sets the array of ColorItem objects that will be used as theme colors instead of built-in color palette.
        /// See: http://www.devcomponents.com/kb2/?p=79 for instructions.
        /// </summary>
        [DefaultValue(null), Browsable(false), Category("Colors"), Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
        public ColorItem[][] CustomThemeColors
        {
            get { return GetColorPickerDropDown().CustomThemeColors; }
            set
            {
                GetColorPickerDropDown().CustomThemeColors = value;
            }
        }
#endif

        /// <summary>
        /// Displays the Colors dialog that allows user to choose the color or create a custom color. If new color is chosen the
        /// SelectedColorChanged event is raised.
        /// </summary>
        public System.Windows.Forms.DialogResult DisplayMoreColorsDialog()
        {
            return GetColorPickerDropDown().DisplayMoreColorsDialog();
        }

        protected override ButtonItem CreateButtonItem()
        {
            return new ColorPickerDropDown();
        }

        private ColorPickerDropDown GetColorPickerDropDown()
        {
            return InternalItem as ColorPickerDropDown;
        }

        private void InternalColorPreview(object sender, ColorPreviewEventArgs e)
        {
            OnColorPreview(e);
        }

        /// <summary>
        /// Raises the ColorPreview event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnColorPreview(ColorPreviewEventArgs e)
        {
            if (ColorPreview != null)
                ColorPreview(this, e);
        }

        private void InternalSelectedColorChanged(object sender, EventArgs e)
        {
            OnSelectedColorChanged(e);
            ExecuteCommand();
        }

        /// <summary>
        /// Gets whether command is executed when button is clicked.
        /// </summary>
        protected virtual bool ExecuteCommandOnClick
        {
            get { return false; }
        }

        /// <summary>
        /// Raises the SelectedColorChanged event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnSelectedColorChanged(EventArgs e)
        {
            if (SelectedColorChanged != null)
                SelectedColorChanged(this, e);
        }

        /// <summary>
        /// Gets or sets more colors menu item is visible which allows user to open Custom Colors dialog box. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoSerialize(), Category("Appearance"), Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
        public bool DisplayMoreColors
        {
            get { return GetColorPickerDropDown().DisplayMoreColors; }
            set { GetColorPickerDropDown().DisplayMoreColors = value; }
        }

        /// <summary>
        /// Gets or sets the last selected color from either the drop-down or Custom Color dialog box. Default value is
        /// Color.Empty. You can use SelectedColorChanged event to be notified when this property changes.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor
        {
            get { return GetColorPickerDropDown().SelectedColor; }
            set { GetColorPickerDropDown().SelectedColor = value; }
        }

        /// <summary>
        /// Gets or sets whether theme colors are displayed on drop-down. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoSerialize(), Category("Appearance"), Description("Indicates whether theme colors are displayed on drop-down.")]
        public bool DisplayThemeColors
        {
            get { return GetColorPickerDropDown().DisplayThemeColors; }
            set { GetColorPickerDropDown().DisplayThemeColors = value; }
        }

        /// <summary>
        /// Gets or sets whether standard colors are displayed on drop-down. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), DevCoSerialize(), Category("Appearance"), Description("Indicates whether standard colors are displayed on drop-down.")]
        public bool DisplayStandardColors
        {
            get { return GetColorPickerDropDown().DisplayStandardColors; }
            set { GetColorPickerDropDown().DisplayStandardColors = value; }
        }

        /// <summary>
        /// Indicates whether SubItems collection is serialized. ColorPickerDropDown does not serialize the sub items.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSubItems()
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
            get { return GetColorPickerDropDown().SelectedColorImageRectangle; }
            set { GetColorPickerDropDown().SelectedColorImageRectangle = value; }
        }

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSelectedColorImageRectangle()
        {
            return GetColorPickerDropDown().ShouldSerializeSelectedColorImageRectangle();
        }

        /// <summary>
        /// Resets the property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSelectedColorImageRectangle()
        {
            GetColorPickerDropDown().ResetSelectedColorImageRectangle();
        }

        /// <summary>
        /// Invokes the ColorPreview event.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        public void InvokeColorPreview(ColorPreviewEventArgs e)
        {
            GetColorPickerDropDown().InvokeColorPreview(e);
        }

        /// <summary>
        /// Update the selected color image if the SelectedColorImageRectangle has been set and button is using Image property to display the image.
        /// </summary>
        public void UpdateSelectedColorImage()
        {
            GetColorPickerDropDown().UpdateSelectedColorImage();
        }
        #endregion
    }
}
