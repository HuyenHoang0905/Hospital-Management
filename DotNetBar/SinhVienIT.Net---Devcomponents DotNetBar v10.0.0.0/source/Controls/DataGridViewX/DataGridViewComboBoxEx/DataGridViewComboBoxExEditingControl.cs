using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxItem(false), ComVisible(false)]
    public class DataGridViewComboBoxExEditingControl : ComboBoxEx, IDataGridViewEditingControl
    {
        #region Private variables

        private DataGridView _DataGridView;

        private int _RowIndex;
        private bool _ValueChanged;

        #endregion

        public DataGridViewComboBoxExEditingControl()
        {
            TabStop = false;
        }

        #region OnTextChanged

        /// <summary>
        /// Handles OnTextChanged events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            _ValueChanged = true;

            _DataGridView.NotifyCurrentCellDirty(true);

            base.OnTextChanged(e);
        }

        #endregion

        #region OnSelectedIndexChanged

        /// <summary>
        /// OnSelectedIndexChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (SelectedIndex != -1)
                NotifyDataGridViewOfValueChange();
        }

        /// <summary>
        /// NotifyDataGridViewOfValueChange
        /// </summary>
        private void NotifyDataGridViewOfValueChange()
        {
            _ValueChanged = true;
            _DataGridView.NotifyCurrentCellDirty(true);
        }

        #endregion

        #region IDataGridViewEditingControl Members

        #region Public properties

        #region EditingControlDataGridView

        /// <summary>
        /// Gets or sets the DataGridView
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get { return (_DataGridView); }
            set { _DataGridView = value; }
        }

        #endregion

        #region EditingControlFormattedValue

        /// <summary>
        /// Gets or sets the Control Formatted Value
        /// </summary>
        public object EditingControlFormattedValue
        {
            get { return (GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting)); }

            set
            {
                string s = value as string;

                if (s != null)
                {
                    Text = s;

                    if (string.Compare(s, this.Text, true, CultureInfo.CurrentCulture) != 0)
                        SelectedIndex = -1;
                }
            }
        }

        #endregion

        #region EditingControlRowIndex

        /// <summary>
        /// Gets or sets the Control RoeIndex
        /// </summary>
        public int EditingControlRowIndex
        {
            get { return (_RowIndex); }
            set { _RowIndex = value; }
        }

        #endregion

        #region EditingControlValueChanged

        /// <summary>
        /// Gets or sets the Control ValueChanged state
        /// </summary>
        public bool EditingControlValueChanged
        {
            get { return (_ValueChanged); }
            set { _ValueChanged = value; }
        }

        #endregion

        #region EditingPanelCursor

        /// <summary>
        /// Gets the Panel Cursor
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get { return (Cursors.Default); }
        }

        #endregion

        #region RepositionEditingControlOnValueChange

        /// <summary>
        /// Gets whether to RepositionEditingControlOnValueChange
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get { return (false); }
        }

        #endregion

        #endregion

        #region ApplyCellStyleToEditingControl

        /// <summary>
        /// ApplyCellStyleToEditingControl
        /// </summary>
        /// <param name="dataGridViewCellStyle"></param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;

            ForeColor = dataGridViewCellStyle.ForeColor;
            BackColor = dataGridViewCellStyle.BackColor;
        }

        #endregion

        #region GetEditingControlFormattedValue

        /// <summary>
        /// Gets EditingControlFormattedValue
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return (Text);
        }

        #endregion

        #region EditingControlWantsInputKey

        /// <summary>
        /// Gets whether the given key wants to be processed
        /// by the Control
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="dataGridViewWantsInputKey"></param>
        /// <returns></returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            if (((((keyData & Keys.KeyCode) != Keys.Down) && ((keyData & Keys.KeyCode) != Keys.Up)) &&
                (!DroppedDown || ((keyData & Keys.KeyCode) != Keys.Escape))) && ((keyData & Keys.KeyCode) != Keys.Return))
            {
                return (dataGridViewWantsInputKey == false);
            }

            return (true);
        }

        #endregion

        #region PrepareEditingControlForEdit

        /// <summary>
        /// PrepareEditingControlForEdit
        /// </summary>
        /// <param name="selectAll"></param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
                SelectAll();
        }

        #endregion

        #endregion

    }
}
