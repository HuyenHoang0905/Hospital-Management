using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class ExpandElement : MarkupElement
    {
        #region Internal Implementation
        private Size m_DefaultSize = new Size(5, 4);
        private eExpandDirection m_Direction = eExpandDirection.Default;

        public override void Measure(System.Drawing.Size availableSize, MarkupDrawContext d)
        {
            this.Bounds = new Rectangle(Point.Empty, m_DefaultSize);
        }

        protected override void ArrangeCore(System.Drawing.Rectangle finalRect, MarkupDrawContext d) { }

        public override void Render(MarkupDrawContext d)
        {
            Rectangle r = this.Bounds;
            r.Offset(d.Offset);

            if (!d.ClipRectangle.IsEmpty && !r.IntersectsWith(d.ClipRectangle))
                return;

            Graphics g = d.Graphics;
            Color color = d.CurrentForeColor;
            //Color shadeColor = Color.FromArgb(96, Color.White);

            eExpandDirection direction = eExpandDirection.Bottom;
            if (m_Direction != eExpandDirection.Default)
                direction = m_Direction;

            #if DOTNETBAR
            if(d.ContextObject is ButtonItem)
            {
                if (m_Direction == eExpandDirection.Default)
                {
                    ButtonItem item = d.ContextObject as ButtonItem;
                    if (item.IsOnMenu)
                    {
                        direction = eExpandDirection.Right;
                        if (item.PopupSide == ePopupSide.Default && d.RightToLeft || item.PopupSide == ePopupSide.Left)
                            direction = eExpandDirection.Left;
                    }
                    else if (item.PopupSide == ePopupSide.Default)
                        direction = eExpandDirection.Bottom;
                    else if (item.PopupSide == ePopupSide.Left)
                        direction = eExpandDirection.Left;
                    else if (item.PopupSide == ePopupSide.Right)
                        direction = eExpandDirection.Right;
                    else if (item.PopupSide == ePopupSide.Bottom)
                        direction = eExpandDirection.Bottom;
                    else if (item.PopupSide == ePopupSide.Top)
                        direction = eExpandDirection.Top;
                }
            }
            #endif

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;

            Rectangle shadeRect = r;
            if (direction == eExpandDirection.Bottom || direction == eExpandDirection.PopupDropDown)
                shadeRect.Offset(0, 1);
            else if (direction == eExpandDirection.Top)
                shadeRect.Offset(0, -1);
            else if (direction == eExpandDirection.Left)
                shadeRect.Offset(1, 0);
            else if (direction == eExpandDirection.Right)
                shadeRect.Offset(-1, 0);
            Point[] p = GetExpandPolygon(shadeRect, direction);
            //using (SolidBrush brush = new SolidBrush(shadeColor))
            //    g.FillPolygon(brush, p);

            p = GetExpandPolygon(r, direction);
            using(SolidBrush brush = new SolidBrush(color))
                g.FillPolygon(brush, p);

            if (direction == eExpandDirection.PopupDropDown)
            {
                using (Pen pen = new Pen(color, 1))
                    g.DrawLine(pen, r.X, r.Y - 2, r.Right - 1, r.Y - 2);
                //using (Pen pen = new Pen(shadeColor, 1))
                //    g.DrawLine(pen, r.X, r.Y - 1, r.Right - 1, r.Y - 1);
            }

            g.SmoothingMode = sm;

            this.RenderBounds = r;
        }

        private Point[] GetExpandPolygon(Rectangle r, eExpandDirection direction)
        {
            Point[] p = new Point[3];
            switch (direction)
            {
                case eExpandDirection.Right:
                    {
                        p[0].X = r.Left + 1;
                        p[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 - 1;
                        p[1].X = p[0].X;
                        p[1].Y = p[0].Y + 6;
                        p[2].X = p[0].X + 3;
                        p[2].Y = p[0].Y + 3;
                        break;
                    }
                case eExpandDirection.Left:
                    {
                        p[0].X = r.Left + 3;
                        p[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 - 1;
                        p[1].X = p[0].X;
                        p[1].Y = p[0].Y + 6;
                        p[2].X = p[0].X - 3;
                        p[2].Y = p[0].Y + 3;
                        break;
                    }
                case eExpandDirection.Top:
                    {
                        p[0].X = r.Left - 1;
                        p[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 + m_DefaultSize.Height;
                        p[1].X = p[0].X + 6;
                        p[1].Y = p[0].Y;
                        p[2].X = p[0].X + 3;
                        p[2].Y = p[0].Y - 4;
                        break;
                    }
                case eExpandDirection.Bottom:
                case eExpandDirection.PopupDropDown:
                    {
                        p[0].X = r.Left;
                        p[0].Y = r.Top + (r.Height - m_DefaultSize.Height) / 2 + 1;
                        p[1].X = p[0].X + 5;
                        p[1].Y = p[0].Y;
                        p[2].X = p[0].X + 2;
                        p[2].Y = p[0].Y + 3;
                        break;
                    }
            }
            return p;
        }

        public override void ReadAttributes(XmlTextReader reader)
        {
            m_Direction = eExpandDirection.Default;

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "direction")
                {
                    string s = reader.Value.ToLower();
                    if (s == "left")
                        m_Direction = eExpandDirection.Left;
                    else if (s == "right")
                        m_Direction = eExpandDirection.Right;
                    else if (s == "top")
                        m_Direction = eExpandDirection.Top;
                    else if (s == "bottom")
                        m_Direction = eExpandDirection.Bottom;
                    else if (s == "popup")
                        m_Direction = eExpandDirection.PopupDropDown;
                    break;
                }
            }
        }

        private enum eExpandDirection
        {
            Left,
            Right,
            Top,
            Bottom,
            Default,
            PopupDropDown
        }

        /// <summary>
        /// Returns whether layout manager can start new line with this element.
        /// </summary>
        public override bool CanStartNewLine
        {
            get { return false; }
        }
        #endregion


    }
}
