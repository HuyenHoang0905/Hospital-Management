using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.Instrumentation.Design
{
    [ToolboxItem(false)]
    public partial class RangeValueDropDown : UserControl
    {
        #region Private variables

        private float _Value;
        private bool _EscapePressed;

        private IWindowsFormsEditorService _EditorService;
        private ITypeDescriptorContext _Context;

        #endregion

        public RangeValueDropDown(float value, float minimum, float maximum,
            IWindowsFormsEditorService editorService, ITypeDescriptorContext context)
        {
            InitializeComponent();

            _EditorService = editorService;
            _Context = context;

            if (value < minimum)
            {
                if ((minimum * 1000 > Int16.MinValue) && (minimum * 1000 < Int16.MaxValue))
                    minimum = value;
            }

            if (value > maximum)
            {
                if ((maximum * 1000 > Int16.MinValue) && (maximum * 1000 < Int16.MaxValue))
                    maximum = value;
            }

            _TrackBar.Minimum = (int)(minimum * 1000);
            _TrackBar.Maximum = (int)(maximum * 1000);
            _TrackBar.SmallChange = _TrackBar.Maximum / 100;
            _TrackBar.LargeChange = _TrackBar.Maximum / 10;
            _TrackBar.TickFrequency = _TrackBar.LargeChange;

            _TrackBar.Value = (int)(value * 1000);

            _LabelMin.Text = String.Format("{0:f}", minimum);
            _LabelMax.Text = String.Format("{0:f}", maximum);

            Value = value;
        }

        public RangeValueDropDown()
        {
            InitializeComponent();
        }

        #region DefaultSize

        protected override Size DefaultSize
        {
            get { return new Size(186, 40); }
        }

        #endregion

        #region Public properties

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

        #region Value

        public float Value
        {
            get { return (_Value); }

            set
            {
                _Value = value;

                _LabelValue.Text = String.Format("{0:f3}", value);
                _LabelValue.Update();

                _Context.OnComponentChanging();
                _Context.PropertyDescriptor.SetValue(_Context.Instance, value);
                _Context.OnComponentChanged();
            }
        }

        #endregion

        #endregion

        #region OnResize

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int width = _TrackBar.Width;

            int dx = width / 3;

            _LabelMin.Width = dx;
            _LabelMax.Width = dx;
            _LabelValue.Width = dx;

            _LabelValue.Location = new Point((width - dx) / 2, _LabelValue.Location.Y);
            _LabelMax.Location = new Point(width - dx, _LabelMax.Location.Y);
        }

        #endregion

        #region _TrackBar_ValueChanged

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            Value = ((float)_TrackBar.Value) / 1000;
        }

        #endregion

        #region _TrackBar_PreviewKeyDown

        private void _TrackBar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                _EscapePressed = true;
        }

        #endregion
    }
}
