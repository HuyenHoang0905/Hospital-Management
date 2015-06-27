using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.ColorPickers
{
    internal class CombCell
    {
        #region Private Variables
        private Color m_Color = Color.Empty;
        private Point[] m_HexagonPoints=new Point[6];
        private const float Tan30 = 0.57735026918962F;
        private bool m_MouseOver = false;
        private bool m_Selected = false;
        private Rectangle m_Bounds = Rectangle.Empty;
        #endregion

        #region Internal Implementation
        public Color Color
        {
            get { return m_Color; }
            set { m_Color=value; }
        }

        public void SetPosition(float x, float y, int nWidth)
        {
            float nSideLength = (float)((float)nWidth * Tan30);
            m_HexagonPoints[0] = new Point((int)Math.Floor(x - (float)(nWidth / 2)), (int)Math.Floor(y - (nSideLength / 2))-1);
            m_HexagonPoints[1] = new Point((int)Math.Floor((float)x), (int)Math.Floor(y - (float)(nWidth / 2))-1);
            m_HexagonPoints[2] = new Point((int)Math.Floor(x + (float)(nWidth / 2)), (int)Math.Floor(y - (nSideLength / 2))-1);
            m_HexagonPoints[3] = new Point((int)Math.Floor(x + (float)(nWidth / 2)), (int)Math.Floor(y + (nSideLength / 2)) + 1);
            m_HexagonPoints[4] = new Point((int)Math.Floor((float)x), (int)Math.Floor(y + (float)(nWidth / 2)) + 1);
            m_HexagonPoints[5] = new Point((int)Math.Floor(x - (float)(nWidth / 2)), (int)Math.Floor(y + (nSideLength / 2)) + 1);

            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(m_HexagonPoints);
            m_Bounds = Rectangle.Round(path.GetBounds());
            m_Bounds.Inflate(2, 2);
        }

        public void Draw(Graphics g)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(m_HexagonPoints);
            path.CloseAllFigures();

            using (SolidBrush brush = new SolidBrush(m_Color))
            {
                g.FillPath(brush, path);
            }

            if (m_MouseOver || m_Selected)
            {
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(41, 92, 150), 2))
                    g.DrawPath(pen, path);
                using (Pen pen = new Pen(Color.FromArgb(149, 178, 239), 1))
                    g.DrawPath(pen, path);
                g.SmoothingMode = sm;
            }

            path.Dispose();
        }

        public void Draw(Graphics g, Color c)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(m_HexagonPoints);
            using (SolidBrush brush = new SolidBrush(c))
            {
                g.FillPath(brush, path);
            }
            path.Dispose();
        }

        public Rectangle Bounds
        {
            get
            {
                return m_Bounds;
            }
        }

        public bool MouseOver
        {
            get { return m_MouseOver; }
            set { m_MouseOver = value; }
        }

        public bool Selected
        {
            get { return m_Selected; }
            set { m_Selected = value; }
        }
        #endregion

    }
}
