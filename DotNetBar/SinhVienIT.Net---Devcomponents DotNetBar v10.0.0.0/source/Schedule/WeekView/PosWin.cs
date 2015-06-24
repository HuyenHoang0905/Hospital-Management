#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Schedule
{
    public partial class PosWin : Form
    {
        #region Private variables

        private string _PosText = "";           // Window content
        private int _PosHeight;                 // Calculated window height

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PosWin()
        {
            InitializeComponent();
        }

        #region CreateParams / show support

        // This code permits us to be able to create a
        // window with a drop shadow

        private const int CS_DROPSHADOW = 0x00020000;

        protected override CreateParams CreateParams
        {
            get 
            { 
                CreateParams parameters = base.CreateParams; 
                
                parameters.ClassStyle =
                    (parameters.ClassStyle | CS_DROPSHADOW);
            
                return (parameters); 
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets and sets the window content text
        /// </summary>
        public string PosText
        {
            get { return (_PosText); }

            set
            {
                if (_PosText != value)
                {
                    _PosText = value;

                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets the calculated window height
        /// </summary>
        public int PosHeight
        {
            get
            {
                if (_PosHeight <= 0)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        eTextFormat tf = eTextFormat.Default |
                            eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;

                        Size sz = TextDrawing.MeasureString(g, "ABC", SystemFonts.CaptionFont, 0, tf);

                        _PosHeight = sz.Height + 4;
                    }
                }

                return (_PosHeight);
            }
        }

        #endregion

        #region Paint processing

        private void PosWin_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            eTextFormat tf = eTextFormat.Default | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;

            Size sz = TextDrawing.MeasureString(g, _PosText, SystemFonts.CaptionFont, 0, tf);

            sz.Width += 6;
            sz.Height += 4;

            this.Size = sz;

            int swidth = Screen.FromControl(this).Bounds.Width;

            if (Location.X + sz.Width > swidth)
                Location = new Point(swidth - sz.Width, Location.Y);

            Rectangle r = new Rectangle(0, 0, sz.Width - 1, sz.Height - 1);

            g.DrawRectangle(Pens.Black, r);

            TextDrawing.DrawString(g, _PosText, SystemFonts.CaptionFont, Color.Black, r, tf);
        }

        #endregion
    }
}
#endif
