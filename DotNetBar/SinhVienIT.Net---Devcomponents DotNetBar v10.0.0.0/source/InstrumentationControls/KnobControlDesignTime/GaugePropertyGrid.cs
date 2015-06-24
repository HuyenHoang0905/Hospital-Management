using System.Windows.Forms;

namespace DevComponents.DotNetBarKnobControl.Design
{
    public partial class GaugePropertyGrid : Form
    {
        public GaugePropertyGrid()
        {
            InitializeComponent();
        }

        #region Public properties

        public object SelectedObject
        {
            get { return (propertyGrid1.SelectedObject); }
            set { propertyGrid1.SelectedObject = value; }
        }

        #endregion
    }
}