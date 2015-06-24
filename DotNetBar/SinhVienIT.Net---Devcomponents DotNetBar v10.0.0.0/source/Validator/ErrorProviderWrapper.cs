#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator
{
    internal class ErrorProviderWrapper : IErrorProvider
    {
        #region Constructors
        private ErrorProvider _ErrorProvider = null;

        /// <summary>
        /// Initializes a new instance of the ErrorProviderWrapper class.
        /// </summary>
        /// <param name="errorProvider"></param>
        public ErrorProviderWrapper(ErrorProvider errorProvider)
        {
            _ErrorProvider = errorProvider;
        }

        /// <summary>
        /// Initializes a new instance of the ErrorProviderWrapper class.
        /// </summary>
        /// <param name="errorProvider"></param>
        /// <param name="iconPadding"></param>
        public ErrorProviderWrapper(ErrorProvider errorProvider, int iconPadding)
        {
            _ErrorProvider = errorProvider;
            _IconPadding = iconPadding;
        }

        private int _IconPadding = 0;
        public int IconPadding
        {
            get { return _IconPadding; }
            set
            {
                _IconPadding = value;
            }
        }
        #endregion



        #region IErrorProvider Members
        public void SetError(Control control, string value)
        {
            _ErrorProvider.SetError(control, value);
            if (_IconPadding != 0)
                _ErrorProvider.SetIconPadding(control, _IconPadding);
        }
       
        public void ClearError(Control control)
        {
            _ErrorProvider.SetError(control, null);
            if (_IconPadding != 0)
                _ErrorProvider.SetIconPadding(control, 0);
        }

        #endregion
    }
}
#endif