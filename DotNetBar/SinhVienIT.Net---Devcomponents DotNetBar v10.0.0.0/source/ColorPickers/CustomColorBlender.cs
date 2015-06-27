using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace DevComponents.DotNetBar.ColorPickers
{
    /// <summary>
    /// Represents custom color blend selection control.
    /// </summary>
    [ToolboxItem(true), DefaultEvent("SelectedColorChanged"), DefaultProperty("SelectedColor"), ToolboxBitmap(typeof(CustomColorBlender), "ColorPickerItem.CustomColorBlender.ico")]
    public class CustomColorBlender : System.Windows.Forms.Control
    {
        #region Events
        /// <summary>
        /// Occurs when SelectedColor has changed.
        /// </summary>
        public event EventHandler SelectedColorChanged;
        /// <summary>
        /// Raises SelectedColorChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnSelectedColorChanged(EventArgs e)
        {
            EventHandler handler = SelectedColorChanged;
            if (handler != null)
                handler(this, e);
        }
        #endregion
    	
        #region Private Variables
        private Bitmap m_ColorBlendBitmap = null;
        private Point m_SelectedPoint = new Point(-1, -1);
        #endregion

        #region Constructor, Dispose
        public CustomColorBlender()
		{
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(DisplayHelp.DoubleBufferFlag, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_ColorBlendBitmap != null)
                {
                    m_ColorBlendBitmap.Dispose();
                    m_ColorBlendBitmap = null;
                }
            }
            base.Dispose(disposing);
        }

        private void CreateBlendBitmap()
        {
            int stripeCount = 6;
            Rectangle clientRect = this.ClientRectangle;

            int stripeWidth = clientRect.Width / stripeCount;
            int ySteps=127;
            int xStart = clientRect.X;
            Bitmap bmp = new Bitmap(clientRect.Width, clientRect.Height, PixelFormat.Format24bppRgb);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.FillRectangle(SystemBrushes.Control, clientRect);

                for (int stripe = 0; stripe < stripeCount; stripe++)
                {
                    // Calculate X steps and point Width
                    int pointWidth = 4;
                    int colorStepX = 255 / (stripeWidth / pointWidth);

                    if (colorStepX<1)
                    {
                        pointWidth = stripeWidth / 255;
                        colorStepX = 1;
                    }

                    int pointHeight = 4;
                    int colorStepY = ySteps / (clientRect.Height / pointHeight);
                    
                    if (colorStepY<1)
                    {
                        pointHeight = clientRect.Height / ySteps;
                        colorStepY = 1;
                    }

                    int x = xStart;
                    int y = clientRect.Y;
                    int r = 0, g = 0, b = 0;
                    int rXInc = 0, gXInc = 0, bXInc = 0;
                    int rYInc = 0, gYInc = 0, bYInc = 0;

                    if (stripe == 0)
                    {
                        r = 255;
                        g = 0;
                        b = 0;
                        gXInc = colorStepX;
                    }
                    else if (stripe == 1)
                    {
                        r = 255;
                        g = 255;
                        b = 0;
                        rXInc = - colorStepX;
                    }
                    else if (stripe == 2)
                    {
                        r = 0;
                        g = 255;
                        b = 0;
                        bXInc = colorStepX;
                    }
                    else if (stripe == 3)
                    {
                        r = 0;
                        g = 255;
                        b = 255;
                        gXInc = -colorStepX;
                    }
                    else if (stripe == 4)
                    {
                        r = 0;
                        g = 0;
                        b = 255;
                        rXInc = colorStepX;
                    }
                    else if (stripe == 5)
                    {
                        r = 255;
                        g = 0;
                        b = 255;
                        bXInc = -colorStepX;
                    }
                    
                    for (int i = 0; i < stripeWidth; i += pointWidth)
                    {
                        int ry = r, gy = g, by = b;
                        rYInc = 127-r;
                        gYInc = 127-g;
                        bYInc = 127-b;

                        for (int j = clientRect.Y; j < clientRect.Height; j += pointHeight)
                        {
                            using (SolidBrush brush = new SolidBrush(Color.FromArgb(ry, gy, by)))
                                graph.FillRectangle(brush, new Rectangle(x, y, pointWidth, pointHeight));
                            y += pointHeight;

                            ry = r + (int)(rYInc * ((float)j / (float)clientRect.Height));
                            gy = g + (int)(gYInc * ((float)j / (float)clientRect.Height));
                            by = b + (int)(bYInc * ((float)j / (float)clientRect.Height));
                        }

                        x += pointWidth;
                        y = clientRect.Y;
                        r += rXInc;
                        g += gXInc;
                        b += bXInc;

                    }
                    xStart = x;
                    if (stripe == 5)
                        break;
                }
            }

            if (m_ColorBlendBitmap != null)
                m_ColorBlendBitmap.Dispose();
            m_ColorBlendBitmap = bmp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!this.BackColor.IsEmpty && this.BackColor != Color.Transparent)
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    g.FillRectangle(brush, -1, -1, this.Width + 1, this.Height + 1);
            }

            if (this.BackColor == Color.Transparent || this.BackgroundImage != null)
            {
                base.OnPaintBackground(e);
            }

            if (m_ColorBlendBitmap == null) return;

            g.DrawImageUnscaled(m_ColorBlendBitmap, 0, 0);

            if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
            {
                Color clr = Color.White;
                using (SolidBrush brush = new SolidBrush(clr))
                {
                    Rectangle r = new Rectangle(m_SelectedPoint.X - 2, m_SelectedPoint.Y - 9, 3, 5);
                    g.FillRectangle(brush, r);
                    r.Offset(0, 10);
                    g.FillRectangle(brush, r);
                    r = new Rectangle(m_SelectedPoint.X - 8, m_SelectedPoint.Y - 3, 5, 3);
                    g.FillRectangle(brush, r);
                    r.Offset(10, 0);
                    g.FillRectangle(brush, r);
                }

            }

            base.OnPaint(e);
        }

        private void SetSelectedPoint(Point p)
        {
            Rectangle r = this.ClientRectangle;

            if (m_ColorBlendBitmap != null)
            {
                r.Width = m_ColorBlendBitmap.Width;
                r.Height = m_ColorBlendBitmap.Height;
            }

            if (p.X < 0)
                p.X = 0;
            if (p.Y < 0)
                p.Y = 0;
            if (p.X > r.Right)
                p.X = r.Right - 1;
            if (p.Y > r.Bottom)
                p.Y = r.Bottom - 1;

            if (p != m_SelectedPoint)
            {
                if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
                {
                    Rectangle inv = new Rectangle(m_SelectedPoint, Size.Empty);
                    inv.Inflate(10, 10);
                    this.Invalidate(inv);
                }
                m_SelectedPoint = p;
                if (m_SelectedPoint.X >= 0 && m_SelectedPoint.Y >= 0)
                {
                    Rectangle inv = new Rectangle(m_SelectedPoint, Size.Empty);
                    inv.Inflate(10, 10);
                    this.Invalidate(inv);
                }
                OnSelectedColorChanged(EventArgs.Empty);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            SetSelectedPoint(new Point(e.X, e.Y));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
            	int x = e.X;
            	int y = e.Y;
            	if(x<0)
            		x = 0;
            	if(x>=this.ClientRectangle.Width)
            		x = this.ClientRectangle.Width - 1;
				if(y<0)
					y = 0;
				if(y>=this.ClientRectangle.Height)
					y = this.ClientRectangle.Height - 1;
            	SetSelectedPoint(new Point(x, y));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            CreateBlendBitmap();
            base.OnResize(e);
        }

        /// <summary>
        /// Gets or sets the color selected by the control. Color that is assigned must be visible on the face of the control otherwise the SelectedColor will be set to Color.Empty
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor
        {
            get
            {
                if (m_SelectedPoint.X < 0 || m_SelectedPoint.Y < 0 || m_ColorBlendBitmap==null)
                    return Color.Empty;
                return m_ColorBlendBitmap.GetPixel(m_SelectedPoint.X, m_SelectedPoint.Y);
            }
            set
            {
                if (value.Equals(Color.Transparent) || value.IsEmpty)
                {
                    if (!m_SelectedPoint.IsEmpty)
                    {
                        m_SelectedPoint = new Point(-1, -1);
                        this.Invalidate();
                        OnSelectedColorChanged(EventArgs.Empty);
                    }
                }
                else
                {
                    Point colorPoint = FindColorPoint(value);
                    if (m_SelectedPoint != colorPoint)
                    {
                        m_SelectedPoint = colorPoint;
                        this.Invalidate();
                        OnSelectedColorChanged(EventArgs.Empty);
                    }
                }
            }
        }
        private Point FindColorPoint(Color value)
        {
            if (m_ColorBlendBitmap == null || value.IsEmpty || value == Color.Transparent) return new Point(-1, -1);
            for (int x = 0; x < m_ColorBlendBitmap.Width; x++)
            {
                for (int y = 0; y < m_ColorBlendBitmap.Height; y++)
                {
                    Color color = m_ColorBlendBitmap.GetPixel(x, y);
                    if (color.Equals(value))
                        return new Point(x, y);
                }
            }
            return new Point(-1, -1);
        }
        #endregion
    }
}
