using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls
{
    [ToolboxItem(false), ComVisible(false)]
    public class DataGridViewMaskedTextBoxAdvEditingControl : MaskedTextBoxAdv, IDataGridViewEditingControl
    {
        #region Private variables

        private DataGridView _DataGridView;

        private int _RowIndex;
        private bool _ValueChanged;

        #endregion

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
            get { return (Text); }
            set { Text = (string) value; }
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
            get { return (base.Cursor); }
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
            return (EditingControlFormattedValue);
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
            return (dataGridViewWantsInputKey == false);
        }

        #endregion

        #region PrepareEditingControlForEdit

        /// <summary>
        /// PrepareEditingControlForEdit
        /// </summary>
        /// <param name="selectAll"></param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        #endregion

        #endregion
    }
}
