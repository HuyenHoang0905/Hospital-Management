#if FRAMEWORK20
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
    public partial class SuperTabDragWindow : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SuperTabDragWindow()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// ShowWithoutActivation
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
    }
}
#endif