#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    public partial class ShapeEditorForm : Form
    {
        public ShapeEditorForm()
        {
            InitializeComponent();
#if (!TRIAL)
            this.itemPanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
#endif
        }

        private void itemPanel1_OptionGroupChanging(object sender, OptionGroupChangingEventArgs e)
        {
            BaseItem parent = e.NewChecked.Parent;
            tabControl1.SelectedTabIndex = parent.SubItems.IndexOf(e.NewChecked);
        }

        private IShapeDescriptor _Shape = null;
        public IShapeDescriptor Value
        {
            get
            {
                return _Shape;
            }
            set
            {
                _Shape = value;
                OnShapeChanged();
            }
        }

        private void OnShapeChanged()
        {
            if (_Shape is RoundRectangleShapeDescriptor)
            {
                RoundRectangleShapeDescriptor rcd = (RoundRectangleShapeDescriptor)_Shape;
                if (rcd.IsEmpty)
                    buttonRect.Checked = true;
                else
                    buttonRound.Checked = true;
            }
            else if (_Shape is EllipticalShapeDescriptor)
                buttonEllipse.Checked = true;
            else
                buttonDefault.Checked = true;
        }

        private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
        {
            if (e.NewTab == tabRound)
            {
                RoundRectangleShapeDescriptor rcd = _Shape as RoundRectangleShapeDescriptor;
                if (rcd == null)
                {
                    rcd = new RoundRectangleShapeDescriptor(2);
                }
                
                roundTopLeft.Value = rcd.TopLeft;
                roundTopRight.Value = rcd.TopRight;
                roundBottomLeft.Value = rcd.BottomLeft;
                roundBottomRight.Value = rcd.BottomRight;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (buttonDefault.Checked)
                _Shape = null;
            else if (buttonRect.Checked)
                _Shape = new RoundRectangleShapeDescriptor(0);
            else if (buttonRound.Checked)
            {
                _Shape = new RoundRectangleShapeDescriptor(roundTopLeft.Value, roundTopRight.Value, roundBottomLeft.Value, roundBottomRight.Value);
            }
            else if (buttonEllipse.Checked)
                _Shape = new EllipticalShapeDescriptor();
        }

        private void RoundCorner_ValueChanged(object sender, EventArgs e)
        {
            RoundRectangleShapeDescriptor rd = new RoundRectangleShapeDescriptor(roundTopLeft.Value, roundTopRight.Value, roundBottomLeft.Value, roundBottomRight.Value);
            buttonRoundPreview.Shape = rd;
        }
    }
}
#endif