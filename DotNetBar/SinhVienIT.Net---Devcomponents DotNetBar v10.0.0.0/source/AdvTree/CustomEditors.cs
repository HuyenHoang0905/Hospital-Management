using System;
using System.Text;
using DevComponents.Editors;
using System.Windows.Forms;

namespace DevComponents.AdvTree
{
#if FRAMEWORK20
    #region Integer Custom Editor
    internal class IntegerCellEditor : IntegerInput, ICellEditControl
    {
        #region ICellEditControl Members

        public void BeginEdit()
        {
#if (FRAMEWORK20)
            this.MinimumSize = new System.Drawing.Size(32, 10);
#endif
        }

        public void EndEdit()
        {
            this.Dispose();
        }

        public object CurrentValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is int)
                    this.Value = (int)value;
                else if (value == null)
                    this.ValueObject = null;
                else
                {
                    string s = Utilities.StripNonNumeric(value.ToString());
                    int i = 0;
#if FRAMEWORK20
                    int.TryParse(s, out i);
#else
            try
            {
                i = int.Parse(s);
            }
            catch { }
#endif
                    this.Value = i;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Tab)
            {
                if (EditComplete != null)
                    EditComplete(this, new EventArgs());
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                if (CancelEdit != null)
                    CancelEdit(this, new EventArgs());
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public event EventHandler EditComplete;

        public event EventHandler CancelEdit;

        public bool EditWordWrap
        {
            get { return false; }
            set { }
        }

        #endregion
    }
    #endregion

    #region Double Custom Editor
    internal class DoubleCellEditor : DoubleInput, ICellEditControl
    {
        #region ICellEditControl Members

        public void BeginEdit()
        {
#if (FRAMEWORK20)
            this.MinimumSize = new System.Drawing.Size(32, 10);
#endif
        }

        public void EndEdit()
        {
            this.Dispose();
        }

        public object CurrentValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is int)
                    this.Value = (int)value;
                else if (value == null)
                    this.ValueObject = null;
                else
                {
                    string s = Utilities.StripNonNumeric(value.ToString());
                    double i = 0;
#if FRAMEWORK20
                    double.TryParse(s, out i);
#else
            try
            {
                i = double.Parse(s);
            }
            catch { }
#endif
                    this.Value = i;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Tab)
            {
                if (EditComplete != null)
                    EditComplete(this, new EventArgs());
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                if (CancelEdit != null)
                    CancelEdit(this, new EventArgs());
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public event EventHandler EditComplete;

        public event EventHandler CancelEdit;

        public bool EditWordWrap
        {
            get { return false; }
            set { }
        }

        #endregion
    }
    #endregion
#endif
}
