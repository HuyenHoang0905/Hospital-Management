using System;
using System.Drawing.Drawing2D;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Presentation
{
    internal class ShapeFill : Shape
    {
        #region Properties
        /// <summary>
        /// Gets or sets the starting fill color.
        /// </summary>
        public Color Color1 = Color.Empty;

        /// <summary>
        /// Gets or sets the end fill color.
        /// </summary>
        public Color Color2 = Color.Empty;

        /// <summary>
        /// Gets or sets the gradient angle.
        /// </summary>
        public int GradientAngle = 90;
        
        /// <summary>
        /// Gets or sets the background color collection blend.
        /// </summary>
        public BackgroundColorBlendCollection BackgroundColorBlend = null;

        /// <summary>
        /// Gets or sets the fill gradient type.
        /// </summary>
        public eGradientType GradientType = eGradientType.Linear;
        #endregion

        #region Internal Implementation
        public ShapeFill() { }
        public ShapeFill(Color color1, Color color2)
        {
            this.Color1 = color1;
            this.Color2 = color2;
        }
        public ShapeFill(LinearGradientColorTable table)
        {
            this.Color1 = table.Start;
            this.Color2 = table.End;
        }
        public ShapeFill(Color color1)
        {
            this.Color1 = color1;
            this.Color2 = Color.Empty;
        }
        
        public void Apply(LinearGradientColorTable table)
        {
            if (table == null)
            {
                this.Color1 = Color.Empty;
                this.Color2 = Color.Empty;
                this.GradientAngle = 90;
            }
            else
            {
                this.Color1 = table.Start;
                this.Color2 = table.End;
                this.GradientAngle = table.GradientAngle;
            }
        }
        
        public Brush CreateBrush(System.Drawing.Rectangle r)
        {
            if (r.Width <= 0 || r.Height <= 0)
                return null;

            if(this.BackgroundColorBlend!=null && BackgroundColorBlend.Count>0)
            {
                return DisplayHelp.CreateBrush(r, BackgroundColorBlend, GradientAngle, GradientType);
            }
            else if(Color2.IsEmpty && !Color1.IsEmpty)
            {
                return new SolidBrush(Color1);
            }
            else if (!Color2.IsEmpty && !Color1.IsEmpty)
            {
                if (GradientType == eGradientType.Linear)
                    return new LinearGradientBrush(r, Color1, Color2, GradientAngle);
                else
                {
                    int d = (int)Math.Sqrt(r.Width * r.Width + r.Height * r.Height) + 4;
                    GraphicsPath fillPath = new GraphicsPath();
                    fillPath.AddEllipse(r.X - (d - r.Width) / 2, r.Y - (d - r.Height) / 2, d, d);
                    PathGradientBrush brush = new PathGradientBrush(fillPath);
                    brush.CenterColor = this.Color1;
                    brush.SurroundColors = new Color[] { this.Color2 };
                    return brush;
                }
            }
            return null;
        }
        #endregion

    }
}
