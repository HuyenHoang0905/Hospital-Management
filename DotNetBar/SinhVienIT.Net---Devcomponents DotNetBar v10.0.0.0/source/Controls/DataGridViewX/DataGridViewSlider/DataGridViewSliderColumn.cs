using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.Slider.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewSliderColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
    {
        #region Events

        /// <summary>
        /// Occurs right before a Slider Cell is painted
        /// </summary>
        [Description("Occurs right before a Slider Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        [Description("Occurs when a Slider Cell is Clicked.")]
        public event EventHandler<EventArgs> Click;

        #endregion

        #region Private variables

        private Slider _Slider;
        private Bitmap _CellBitmap;

        private int _ActiveRowIndex = -1;
        private bool _BindingComplete;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewSliderColumn()
        {
            CellTemplate = new DataGridViewSliderCell();

            _Slider = new Slider();
            _Slider.Visible = false;

            HookEvents(true);
        }

        #region Internal properties

        #region ActiveRowIndex

        /// <summary>
        /// Gets or sets the active row index
        /// </summary>
        internal int ActiveRowIndex
        {
            get { return (_ActiveRowIndex); }
            set { _ActiveRowIndex = value; }
        }

        #endregion

        #region BindingComplete

        /// <summary>
        /// Gets or sets the DataBindingComplete state
        /// </summary>
        internal bool BindingComplete
        {
            get { return (_BindingComplete); }
            set { _BindingComplete = value; }
        }

        #endregion

        #region Slider

        /// <summary>
        /// Gets the Control Slider
        /// </summary>
        internal Slider Slider
        {
            get { return (_Slider); }
        }

        #endregion

        #endregion

        #region Public properties

        #region Enabled

        /// <summary>
        /// Gets or sets whether the control can respond to user interaction
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior")]
        [Description("Indicates whether the control can respond to user interaction.")]
        public bool Enabled
        {
            get { return (_Slider.Enabled); }
            set { _Slider.Enabled = value; }
        }

        #endregion

        #region EnableMarkup

        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance")]
        [Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return (_Slider.EnableMarkup); }
            set { _Slider.EnableMarkup = value; }
        }

        #endregion

        #region LabelPosition

        /// <summary>
        /// Gets or sets the text label position in relationship to the slider. Default value is Left.
        /// </summary>
        [Browsable(true), DefaultValue(eSliderLabelPosition.Left), Category("Layout")]
        [Description("Indicates text label position in relationship to the slider")]
        public eSliderLabelPosition LabelPosition
        {
            get { return (_Slider.LabelPosition); }
            set { _Slider.LabelPosition = value; }
        }

        #endregion

        #region LabelVisible

        /// <summary>
        /// Gets or sets whether the text label next to the slider is displayed.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true)]
        [Description("Gets or sets whether the label text is displayed."), Category("Behavior"), DefaultValue(true)]
        public bool LabelVisible
        {
            get { return (_Slider.LabelVisible); }
            set { _Slider.LabelVisible = value; }
        }

        #endregion

        #region LabelWidth

        /// <summary>
        /// Gets or sets the width of the label part of the item in pixels. Value must be greater than 0. Default value is 38.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(38), Category("Layout")]
        [Description("Indicates width of the label part of the item in pixels.")]
        public int LabelWidth
        {
            get { return (_Slider.LabelWidth); }
            set { _Slider.LabelWidth = value; }
        }

        #endregion

        #region Maximum

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Category("Behavior"), DefaultValue(100)]
        [Description("Gets or sets the maximum value of the range of the control.")]
        public int Maximum
        {
            get { return (_Slider.Maximum); }
            set { _Slider.Maximum = value; }
        }

        #endregion

        #region Minimum

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Category("Behavior"), DefaultValue(0)]
        [Description("Gets or sets the minimum value of the range of the control.")]
        public int Minimum
        {
            get { return (_Slider.Minimum); }
            set { _Slider.Minimum = value; }
        }

        #endregion

        #region Step

        /// <summary>
        /// Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider. Value must be greater than 0.
        /// </summary>
        [DevCoBrowsable(true), Browsable(true), Category("Behavior"), DefaultValue(1)]
        [Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
        public int Step
        {
            get { return (_Slider.Step); }
            set { _Slider.Step = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DefaultValue(""), Category("Appearance")]
        [Description("The text contained in the item.")]
        public string Text
        {
            get { return (_Slider.Text); }
            set { _Slider.Text = value; }
        }

        #endregion

        #region TextColor

        /// <summary>
        /// Gets or sets the color of the label text.
        /// </summary>
        [Browsable(true), Category("Appearance")]
        [Description("Indicates color of the label text.")]
        public Color TextColor
        {
            get { return (_Slider.TextColor); }
            set { _Slider.TextColor = value; }
        }

        /// <summary>
        /// Returns whether property should be serialized. Used by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return (_Slider.TextColor.IsEmpty == false);
        }

        /// <summary>
        /// Resets the property to default value. Used by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            _Slider.TextColor = Color.Empty;
        }

        #endregion

        #region TrackMarker

        /// <summary>
        /// Gets or sets whether vertical line track marker is displayed on the slide line. Default value is true.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(true)]
        [Description("Indicates whether vertical line track marker is displayed on the slide line.")]
        public virtual bool TrackMarker
        {
            get { return (_Slider.TrackMarker); }
            set { _Slider.TrackMarker = value; }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the current position of the slider.
        /// </summary>
        [Browsable(false)]
        [Description("Indicates the current position of the slider.")]
        public int Value
        {
            get { return (_Slider.Value); }
            set { _Slider.Value = value; }
        }

        #endregion

        #endregion

        #region HookEvents

        /// <summary>
        /// Hooks or unhooks our system events
        /// </summary>
        /// <param name="hook"></param>
        private void HookEvents(bool hook)
        {
            if (hook == true)
            {
                _Slider.SliderItem.Click += SliderItem_Click;
                _Slider.SliderItem.ValueChanged += SliderItem_ValueChanged;
            }
            else
            {
                _Slider.SliderItem.Click -= SliderItem_Click;
                _Slider.SliderItem.ValueChanged -= SliderItem_ValueChanged;
            }
        }

        #endregion

        #region Event processing

        #region SliderItem_Click

        /// <summary>
        /// SliderItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SliderItem_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
        }

        #endregion

        #region SliderItem_ValueChanged

        /// <summary>
        /// SliderItem_ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SliderItem_ValueChanged(object sender, EventArgs e)
        {
            if (_Slider.SliderItem.MouseDownPart == eSliderPart.IncreaseButton ||
                _Slider.SliderItem.MouseDownPart == eSliderPart.DecreaseButton)
            {
                DataGridViewSliderCell cell = DataGridView.Rows[ActiveRowIndex].Cells[Index] as DataGridViewSliderCell;

                if (cell != null)
                {
                    int value = cell.GetSliderValue(cell.Value);

                    if (value != _Slider.SliderItem.Value)
                    {
                        cell.Value = _Slider.SliderItem.Value;
                        cell.RefreshSlider(Index, ActiveRowIndex);

                    }
                }
            }
        }

        #endregion

        #endregion

        #region GetCellBitmap

        /// <summary>
        /// Gets the cell paint bitmap
        /// </summary>
        /// <param name="cellBounds"></param>
        /// <returns></returns>
        internal Bitmap GetCellBitmap(Rectangle cellBounds)
        {
            if (_CellBitmap == null ||
                (_CellBitmap.Width != cellBounds.Width || _CellBitmap.Height < cellBounds.Height))
            {
                if (_CellBitmap != null)
                    _CellBitmap.Dispose();

                _CellBitmap = new Bitmap(cellBounds.Width, cellBounds.Height);
            }

            return (_CellBitmap);
        }

        #endregion

        #region OnBeforeCellPaint

        /// <summary>
        /// Invokes BeforeCellPaint user events
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <param name="columnIndex">Column index</param>
        internal void OnBeforeCellPaint(int rowIndex, int columnIndex)
        {
            if (BeforeCellPaint != null)
                BeforeCellPaint(this, new BeforeCellPaintEventArgs(rowIndex, columnIndex));
        }

        #endregion

        #region ICloneable members

        /// <summary>
        /// Clones the ButtonX Column
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewSliderColumn sc = base.Clone() as DataGridViewSliderColumn;

            if (sc != null)
            {
                _Slider.SliderItem.InternalCopyToItem(sc.Slider.SliderItem);

                sc.Enabled = Enabled;
            }

            return (sc);
        }

        #endregion

        #region IDataGridViewColumn Members

        /// <summary>
        /// Gets the Cell paint setting for the ButtonX control
        /// </summary>
        [Browsable(false)]
        public bool OwnerPaintCell
        {
            get { return (true); }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            HookEvents(false);

            base.Dispose(disposing);
        }

        #endregion

    }
}
