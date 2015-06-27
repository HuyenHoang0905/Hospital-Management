using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBarKnobControl.Design.Properties;

namespace DevComponents.Instrumentation.Design
{
    [ToolboxItem(false)]
    public partial class FormatStringDropDown : UserControl
    {
        #region Private variables

        private string _FormatString;
        private bool _EscapePressed;

        private IWindowsFormsEditorService _EditorService;
        private ITypeDescriptorContext _Context;

        private string[] _Items = new string[] {
                "Custom",
                "Currency",
                "Decimal",
                "Fixed-point",
                "General",
                "Hexadecimal",
                "Number",
                "Percent",
                "Scientific"};

        private Format[] _Formats = new Format[] {
                new Format("",  (string)(Resources.ResourceManager.GetObject("CustomDesc"))),
                new Format("C", (string)(Resources.ResourceManager.GetObject("CurrencyDesc"))),
                new Format("D", (string)(Resources.ResourceManager.GetObject("DecimalDesc"))),
                new Format("F", (string)(Resources.ResourceManager.GetObject("FixedPointDesc"))),
                new Format("G", (string)(Resources.ResourceManager.GetObject("GeneralDesc"))),
                new Format("X", (string)(Resources.ResourceManager.GetObject("HexadecimalDesc"))),
                new Format("N", (string)(Resources.ResourceManager.GetObject("NumericDesc"))),
                new Format("P", (string)(Resources.ResourceManager.GetObject("PercentDesc"))),
                new Format("E", (string)(Resources.ResourceManager.GetObject("ExponentialDesc")))};

        #endregion

        public FormatStringDropDown()
        {
            InitFormatList();
        }

        public FormatStringDropDown(string formatString,
            IWindowsFormsEditorService editorService, ITypeDescriptorContext context)
        {
            _FormatString = formatString;

            _EditorService = editorService;
            _Context = context;

            InitFormatList();

            BackColor = SystemColors.Control;
        }

        #region InitFormatList

        private void InitFormatList()
        {
            InitializeComponent();

            _CbxFmtType.DrawMode = DrawMode.OwnerDrawFixed;
            _CbxFmtType.DrawItem += CbxFmtType_DrawItem;

            _CbxFmtType.Items.AddRange(_Items);
            _CbxFmtType.SelectedIndex = 3;
        }

        #endregion

        #region Public properties

        #region FormatString

        public string FormatString
        {
            get { return (_FormatString); }

            set
            {
                _FormatString = value;

                _Context.OnComponentChanging();
                _Context.PropertyDescriptor.SetValue(_Context.Instance, value);
                _Context.OnComponentChanged();

                Invalidate();
            }
        }

        #endregion

        #region EditorService

        public IWindowsFormsEditorService EditorService
        {
            get { return (_EditorService); }
            set { _EditorService = value; }
        }

        #endregion

        #region EscapePressed

        public bool EscapePressed
        {
            get { return (_EscapePressed); }
            set { _EscapePressed = value; }
        }

        #endregion

        #endregion

        #region CbxFmtType_DrawItem

        void CbxFmtType_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index == 0)
            {
                e.Graphics.DrawLine(Pens.Gainsboro, new Point(e.Bounds.Left, e.Bounds.Bottom - 1),
                                    new Point(e.Bounds.Right, e.Bounds.Bottom - 1));
            }

            TextRenderer.DrawText(e.Graphics, _CbxFmtType.Items[e.Index].ToString(),
                                  _CbxFmtType.Font, e.Bounds, _CbxFmtType.ForeColor, TextFormatFlags.Left);

            e.DrawFocusRectangle();
        }

        #endregion

        #region TextBoxKeyPress

        private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region CbxFmtType_SelectedIndexChanged

        private void CbxFmtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((uint)_CbxFmtType.SelectedIndex < _Formats.Length)
            {
                Format fmt = _Formats[_CbxFmtType.SelectedIndex];

                _LbDescription.Text = fmt.Description;

                UpdateSample();
            }
            else
            {
                _TbSample.Text = "";
                _LbDescription.Text = "";
            }
        }

        #endregion

        #region UpdateSample

        private void UpdateSample()
        {
            if (_CbxFmtType.SelectedIndex == 0)
            {
                _TbCustom.Visible = true;
                _TbPrecision.Visible = false;
                _LbPrecision.Text = @"Format String:";

                UpdateCustomSample();
            }
            else if (_CbxFmtType.SelectedIndex > 0)
            {
                _TbPrecision.Visible = true;
                _TbCustom.Visible = false;
                _LbPrecision.Text = @"Precision:";

                UpdateNumericSample();
            }
        }

        #endregion

        #region UpdateCustomSample

        private void UpdateCustomSample()
        {
            _FormatString = _TbCustom.Text;

            try
            {
                _TbSample.Text = String.Format("{0:" + _FormatString + "}", 1234567890);
            }
            catch
            {
                _TbSample.Text = @"Invalid Format String.";
            }
        }

        #endregion

        #region UpdateNumericSample

        private void UpdateNumericSample()
        {
            Format fmt = _Formats[_CbxFmtType.SelectedIndex];

            try
            {
                int precision = GetPrecision();

                _FormatString = fmt.FormatCode + precision.ToString();

                _TbSample.Text = String.Format("{0:" + _FormatString + "}", 1234567890);
            }
            catch
            {
                _TbSample.Text = @"Invalid Format String.";
            }
        }

        #endregion

        #region GetPrecision

        private int GetPrecision()
        {
            int precision;

            if (int.TryParse(_TbPrecision.Text, out precision))
                return (precision < 0 ? 0 : precision > 99 ? 99 : precision);

            return (0);
        }

        #endregion

        #region MyTextChanged

        private void MyTextChanged(object sender, EventArgs e)
        {
            UpdateSample();
        }

        #endregion

        #region BtnOk_Click

        private void BtnOk_Click(object sender, EventArgs e)
        {
            _EditorService.CloseDropDown();
        }

        #endregion

        #region BtnCancel_Click

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _EscapePressed = true;
            _EditorService.CloseDropDown();
        }

        #endregion

        #region MyPreviewKeyDown

        private void MyPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                _EscapePressed = true;
        }

        #endregion

        #region Format

        private class Format
        {
            #region Private variables

            private string _FormatCode;
            private string _Description;

            #endregion

            public Format(string formatCode, string description)
            {
                _FormatCode = formatCode;
                _Description = description;
            }

            #region Public properties

            public string FormatCode
            {
                get { return (_FormatCode); }
            }

            public string Description
            {
                get { return (_Description); }
            }

            #endregion
        }

        #endregion
    }
}
