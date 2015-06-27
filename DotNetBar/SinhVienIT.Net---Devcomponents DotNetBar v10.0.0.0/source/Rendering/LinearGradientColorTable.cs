using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering
{
    /// <summary>
    /// Represents the color table of linear gradient.
    /// </summary>
    public class LinearGradientColorTable
    {
        public static readonly LinearGradientColorTable Empty = new LinearGradientColorTable();

        #region Private variables

        private Color _Start = Color.Empty;
        private Color _End = Color.Empty;
        private int _GradientAngle = 90;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        public LinearGradientColorTable() { }
        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        public LinearGradientColorTable(Color start)
        {
            this.Start = start;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        public LinearGradientColorTable(Color start, Color end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in hexadecimal representation like FFFFFF.</param>
        /// <param name="end">End color in hexadecimal representation like FFFFFF.</param>
        public LinearGradientColorTable(string start, string end)
        {
            this.Start = ColorScheme.GetColor(start);
            this.End = ColorScheme.GetColor(end);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in 32-bit RGB representation.</param>
        /// <param name="end">End color in 32-bit RGB representation.</param>
        public LinearGradientColorTable(int start, int end)
        {
            this.Start = ColorScheme.GetColor(start);
            this.End = ColorScheme.GetColor(end);
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color in 32-bit RGB representation.</param>
        /// <param name="end">End color in 32-bit RGB representation.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public LinearGradientColorTable(int start, int end, int gradientAngle)
        {
            this.Start = ColorScheme.GetColor(start);
            this.End = ColorScheme.GetColor(end);
            this.GradientAngle = gradientAngle;
        }

        /// <summary>
        /// Creates new instance of the object.
        /// </summary>
        /// <param name="start">Start color.</param>
        /// <param name="end">End color.</param>
        /// <param name="gradientAngle">Gradient angle.</param>
        public LinearGradientColorTable(Color start, Color end, int gradientAngle)
        {
            this.Start = start;
            this.End = end;
            this.GradientAngle = gradientAngle;
        }

        #endregion

        #region Public properties

        #region Start

        /// <summary>
        /// Gets or sets the start color.
        /// </summary>
        public Color Start
        {
            get { return (_Start); }
            set { _Start = value; }
        }

        #endregion

        #region End

        /// <summary>
        /// Gets or sets the end color.
        /// </summary>
        public Color End
        {
            get { return (_End); }
            set { _End = value; } 
        }

        #endregion

        #region GradientAngle

        /// <summary>
        /// Gets or sets the gradient angle. Default value is 90.
        /// </summary>
        public int GradientAngle
        {
            get { return (_GradientAngle); }
            set { _GradientAngle = value; } 
        }

        #endregion

        #region IsEmpty

        /// <summary>
        /// Gets whether both colors assigned are empty.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEmpty
        {
            get { return (Start.IsEmpty && End.IsEmpty && GradientAngle == 90); }
        }

        #endregion

        #endregion

    }
}
