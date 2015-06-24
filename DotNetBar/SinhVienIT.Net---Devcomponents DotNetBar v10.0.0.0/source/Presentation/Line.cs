using System;
using System.Text;
using System.Drawing;

namespace DevComponents.DotNetBar.Presentation
{
    /// <summary>
    /// Represents the line drawn between start and end point.
    /// </summary>
    internal class Line : Shape
    {
        #region Private Variables
        private Location m_StartPoint = new Location();
        private Location m_EndPoint = new Location();
        private ShapeBorder m_Border = new ShapeBorder();
        #endregion

        #region Internal Implementation
        public Line() { }
        public Line(Location startPoint, Location endPoint, ShapeBorder border)
        {
            m_StartPoint = startPoint;
            m_EndPoint = endPoint;
            m_Border = border;
        }

        /// <summary>
        /// Gets the start point of the line.
        /// </summary>
        public Location StartPoint
        {
            get { return m_StartPoint; }
        }

        /// <summary>
        /// Gets the end point of the line.
        /// </summary>
        public Location EndPoint
        {
            get { return m_EndPoint; }
        }

        /// <summary>
        /// Gets the line border.
        /// </summary>
        public ShapeBorder Border
        {
            get { return m_Border; }
        }

        public override void Paint(ShapePaintInfo p)
        {
            System.Drawing.Point start = GetLocation(p.Bounds, m_StartPoint);
            System.Drawing.Point end = GetLocation(p.Bounds, m_EndPoint);
            Graphics g = p.Graphics;

            if (m_Border.Color2.IsEmpty)
            {
                if (!m_Border.Color1.IsEmpty)
                {
                    DisplayHelp.DrawLine(g, start, end, m_Border.Color1, m_Border.Width);
                }
            }
            else
            {
                DisplayHelp.DrawGradientLine(g, start, end, m_Border.Color1, m_Border.Color2, m_Border.GradientAngle, m_Border.Width);
            }

            base.Paint(p);
        }
        #endregion
    }
}
