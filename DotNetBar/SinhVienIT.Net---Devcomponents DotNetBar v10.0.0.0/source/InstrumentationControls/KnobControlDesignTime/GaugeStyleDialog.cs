using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBarKnobControl.Design
{
    public partial class GaugeStyleDialog : Form
    {
        #region Private variables

        private CheckBox _CheckedBox;
        private DateTime _LastClick;

        #endregion

        public GaugeStyleDialog()
        {
            InitializeComponent();

            _CheckedBox = cbCircular;
        }

        #region Public properties

        public string CbSelected
        {
            get { return (_CheckedBox.Name); }
        }

        #endregion

        #region CbCheckedChanged

        private void CbCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb != null)
            {
                if (_CheckedBox != cb)
                {
                    _CheckedBox.Checked = false;

                    _CheckedBox = cb;
                    _CheckedBox.Checked = true;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - _LastClick;

                    if (ts.TotalMilliseconds < SystemInformation.DoubleClickTime)
                        btnOk.PerformClick();
                }
            }

            _LastClick = DateTime.Now;
        }

        #endregion
    }
}