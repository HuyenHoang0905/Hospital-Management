using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ProgressBarX.ico"), ToolboxItem(false), ComVisible(false)]
    public class DataGridViewProgressBarXColumn : DataGridViewButtonColumn, IDataGridViewColumn
    {
        #region Events

        [Description("Occurs right before a ProgressBarX Cell is painted.")]
        public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

        [Description("Occurs when a ProgressBarX Cell is Clicked.")]
        public event EventHandler<EventArgs> Click;

        #endregion

        #region Private variables

        private ProgressBarX _ProgressBarX;
        private Bitmap _CellBitmap;

        private bool _InCellCallBack;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DataGridViewProgressBarXColumn()
        {
            CellTemplate = new DataGridViewProgressBarXCell();

            _ProgressBarX = new ProgressBarX();
            _ProgressBarX.Visible = false;

            HookEvents(true);
        }

        #region Hidden properties

        /// <summary>
        /// Button FlatStyle
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        /// <summary>
        /// Button UseColumnTextForButtonValue
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool UseColumnTextForButtonValue
        {
            get { return base.UseColumnTextForButtonValue; }
            set { base.UseColumnTextForButtonValue = value; }
        }

        #endregion

        #region Internal properties

        #region InCallBack

        /// <summary>
        /// InCallBack
        /// </summary>
        internal bool InCallBack
        {
            get { return (_InCellCallBack); }
            set { _InCellCallBack = value; }
        }

        #endregion

        #region ProgressBarX

        /// <summary>
        /// Gets the Control ProgressBarX
        /// </summary>
        internal ProgressBarX ProgressBarX
        {
            get { return (_ProgressBarX); }
        }

        #endregion

        #endregion

        #region Public properties

        #region ChunkColor

        /// <summary>
        /// Gets or sets the color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance")]
        [Description("Gets or sets the color of the progress chunk.")]
        public Color ChunkColor
        {
            get { return (_ProgressBarX.ChunkColor); }
            set { _ProgressBarX.ChunkColor = value; }
        }

        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor()
        {
            return (_ProgressBarX.ChunkColor.IsEmpty == false);
        }

        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor()
        {
            _ProgressBarX.ChunkColor = Color.Empty;
        }

        #endregion

        #region ChunkColor2

        /// <summary>
        /// Gets or sets the target gradient color of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance")]
        [Description("Gets or sets the target gradient color of the progress chunk.")]
        public Color ChunkColor2
        {
            get { return (_ProgressBarX.ChunkColor2); }
            set { _ProgressBarX.ChunkColor2 = value; }
        }

        /// <summary>
        /// Gets whether ChunkColor property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeChunkColor2()
        {
            return (_ProgressBarX.ChunkColor2.IsEmpty == false);
        }

        /// <summary>
        /// Resets the ChunkColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetChunkColor2()
        {
            _ProgressBarX.ChunkColor2 = Color.Empty;
        }

        #endregion

        #region ChunkGradientAngle

        /// <summary>
        /// Gets or sets the gradient angle of the progress chunk.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), DefaultValue(0), Category("Appearance")]
        [Description("Gets or sets the gradient angle of the progress chunk.")]
        public int ChunkGradientAngle
        {
            get { return (_ProgressBarX.ChunkGradientAngle); }
            set { _ProgressBarX.ChunkGradientAngle = value; }
        }

        #endregion

        #region ColorTable

        /// <summary>
        /// Gets or sets the predefined color state table for progress bar. Color
        /// specified applies to items with Office 2007 style only. It does not have
        /// any effect on other styles. You can use ColorTable to indicate the state
        /// of the operation that Progress Bar is tracking. Default value is eProgressBarItemColor.Normal.
        /// </summary>
        [Browsable(true), DevCoBrowsable(false), DefaultValue(eProgressBarItemColor.Normal), Category("Appearance")]
        [Description("Indicates predefined color of item when Office 2007 style is used.")]
        public eProgressBarItemColor ColorTable
        {
            get { return (_ProgressBarX.ColorTable); }
            set { _ProgressBarX.ColorTable = value; }
        }

        #endregion

        #region Maximum

        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(100)]
        [Description("Indicates the maximum value of the range of the control.")]
        public int Maximum
        {
            get { return (_ProgressBarX.Maximum); }
            set { _ProgressBarX.Maximum = value; }
        }

        #endregion

        #region Minimum

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), DefaultValue(0)]
        [Description("Indicates the minimum value of the range of the control.")]
        public int Minimum
        {
            get { return (_ProgressBarX.Minimum); }
            set { _ProgressBarX.Minimum = value; }
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets/Sets the visual style for the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eDotNetBarStyle.Office2007)]
        [Description("Specifies the visual style of the control.")]
        public virtual eDotNetBarStyle Style
        {
            get { return (_ProgressBarX.Style); }
            set { _ProgressBarX.Style = value; }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the default Text to display on the Progress Bar
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue("")]
        [Description("Indicates the default Text to display on the Progress Bar.")]
        public new string Text
        {
            get
            {
                return (_InCellCallBack == true ?
                    _ProgressBarX.Text : base.Text);
            }

            set
            {
                if (_InCellCallBack == true)
                    _ProgressBarX.Text = value;
                else
                    base.Text = value;
            }
        }

        #endregion

        #region TextVisible

        /// <summary>
        /// Gets or sets whether the text inside the progress bar is displayed.
        /// </summary>
        [Browsable(true), Category("Behavior"), DefaultValue(false)]
        [Description("Gets or sets whether the text inside the progress bar is displayed.")]
        public bool TextVisible
        {
            get { return (_ProgressBarX.TextVisible); }
            set { _ProgressBarX.TextVisible = value; }
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Behavior"), DefaultValue(0)]
        [Description("Indicates the current position of the progress bar.")]
        public int Value
        {
            get { return (_ProgressBarX.Value); }
            set { _ProgressBarX.Value = value; }
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
                _ProgressBarX.ProgressBarItem.Click += ProgressBarItem_Click;
            }
            else
            {
                _ProgressBarX.ProgressBarItem.Click -= ProgressBarItem_Click;
            }
        }

        #endregion

        #region Event processing

        #region ProgressBarItem_Click

        /// <summary>
        /// ProgressBarItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProgressBarItem_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
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
            DataGridViewProgressBarXColumn bc = base.Clone() as DataGridViewProgressBarXColumn;

            if (bc != null)
                _ProgressBarX.ProgressBarItem.InternalCopyToItem(bc.ProgressBarX.ProgressBarItem);

            return (bc);
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
