using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Indicates the Balloon tip position.
	/// </summary>
	public enum eTipPosition
	{
		/// <summary>
		/// Tip is on the top.
		/// </summary>
		Top=0,
		/// <summary>
		/// Tip is on the left side.
		/// </summary>
		Left=1,
		/// <summary>
		/// Tip is on the right side.
		/// </summary>
		Right=2,
		/// <summary>
		/// Tip is on the bottom.
		/// </summary>
		Bottom=3
	}

	/// <summary>
	/// Indicates the style of the balloon.
	/// </summary>
	public enum eBallonStyle
	{
		Balloon=0,
		Alert=1,
        Office2007Alert
	}

	/// <summary>
	/// Indicates type of Alert animation performed when alert is displayed.
	/// </summary>
	public enum eAlertAnimation
	{
		/// <summary>
		/// No animation take place when alert is displayed.
		/// </summary>
		None=0,
		/// <summary>
		/// Alert is animated from bottom to top. (Default)
		/// </summary>
		BottomToTop=1,
		/// <summary>
		/// Alert is animated from top to bottom.
		/// </summary>
		TopToBottom=2,
		/// <summary>
		/// Alert is animated from left to right.
		/// </summary>
		LeftToRight=3,
		/// <summary>
		/// Alert is animated from right to left.
		/// </summary>
		RightToLeft=4
	}

	/// <summary>
	/// Delegate for custom paint event handler.
	/// </summary>
	public delegate void CustomPaintEventHandler(object sender, CustomPaintEventArgs e);

	/// <summary>
	/// Summary description for Balloon.
	/// </summary>
    [ToolboxItem(false), System.Runtime.InteropServices.ComVisible(false), Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design, Version=9.5.0.16, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class Balloon : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		#region Event Definition
		/// <summary>
		/// Occurs when background is redrawn.
		/// </summary>
		public event CustomPaintEventHandler PaintBackground;
		/// <summary>
		/// Occurs when caption image is redrawn.
		/// </summary>
		public event CustomPaintEventHandler PaintCaptionImage;
		/// <summary>
		/// Occurs when caption text is redrawn.
		/// </summary>
		public event CustomPaintEventHandler PaintCaptionText;
		/// <summary>
		/// Occurs when text is redrawn.
		/// </summary>
		public event CustomPaintEventHandler PaintText;
		/// <summary>
		/// Occurs when close button is clicked.
		/// </summary>
		public event EventHandler CloseButtonClick;
		/// <summary>
		/// Occurs when TipPosition property has changed.
		/// </summary>
		public event EventHandler TipPositionChanged;
		#endregion


		private int m_TipLength=16;
		private int m_CornerSize=8;
		private int m_TipOffset=32;
		private eTipPosition m_TipPosition=eTipPosition.Top;

		private Color m_BackColor2=Color.Empty;
		private int m_BackColorGradientAngle=90;

		private eBackgroundImagePosition m_BackgroundImagePosition=eBackgroundImagePosition.Stretch;
		private int m_BackgroundImageAlpha=255;

		private Color m_BorderColor=SystemColors.InfoText;

		private bool m_ShowCloseButton=true;
		private ImageButton m_CloseButton=null;

		private eBallonStyle m_Style=eBallonStyle.Balloon;

		private Image m_CloseButtonNormal=null, m_CloseButtonHot=null, m_CloseButtonPressed=null;
		private Image m_CaptionImage=null;
		private Icon m_CaptionIcon=null;
		private Font m_CaptionFont=null;
		private string m_CaptionText="";
		private StringAlignment m_CaptionAlignment=StringAlignment.Near;
		private Color m_CaptionColor=SystemColors.InfoText;

		private StringAlignment m_TextAlignment=StringAlignment.Near;
		private StringAlignment m_TextLineAlignment=StringAlignment.Center;

		// Layout rectangles
		private Rectangle m_CaptionImageRectangle=Rectangle.Empty;
		private Rectangle m_CaptionRectangle=Rectangle.Empty;
		private Rectangle m_TextRectangle=Rectangle.Empty;

		private bool m_AutoClose=true;
		private int m_AutoCloseTimeOut=0;
		private System.Windows.Forms.Timer m_Timer=null;

		const int IMAGETEXT_SPACING=4;
		const int TEXTCLOSE_SPACING=4;
		const int CAPTIONTEXT_VSPACING=1;
		const int CAPTIONALERT_VSPACING=1;

		private Color m_BorderColor1=Color.FromArgb(241,239,226);
		private Color m_BorderColor2=Color.White;
		private Color m_BorderColor3=Color.FromArgb(236,233,216);
		private Color m_BorderColor4=Color.FromArgb(172,168,153);
		private Color m_BorderColor5=Color.FromArgb(113,111,100);

		private eAlertAnimation m_AlertAnimation=eAlertAnimation.BottomToTop;
		private int m_AlertAnimationDuration=200;
		private bool m_AnimationInProgress=false;
        private bool m_AntiAlias = true;

		public Balloon()
		{
			CreateCloseButton();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			if(components==null)
				this.components = new System.ComponentModel.Container();
			m_CaptionFont=new Font(this.Font,FontStyle.Bold);

			this.SetStyle(ControlStyles.UserPaint,true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			//this.SetStyle(ControlStyles.Opaque,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
			this.SetStyle(DisplayHelp.DoubleBufferFlag,true);
			this.SetStyle(ControlStyles.ContainerControl,true);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			DestroyAutoCloseTimer();
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}

            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_CaptionIcon);
                BarUtilities.DisposeImage(ref m_CaptionImage);
                BarUtilities.DisposeImage(ref m_CloseButtonHot);
                BarUtilities.DisposeImage(ref m_CloseButtonNormal);
                BarUtilities.DisposeImage(ref m_CloseButtonPressed);
            }
			base.Dispose( disposing );
		}

        /// <summary>
        /// Gets or sets whether anti-alias smoothing is used while painting. Default value is false.
        /// </summary>
        [DefaultValue(true), Browsable(true), Category("Appearance"), Description("Gets or sets whether anti-aliasing is used while painting.")]
        public bool AntiAlias
        {
            get { return m_AntiAlias; }
            set
            {
                m_AntiAlias = value;
                this.Invalidate();
            }
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Balloon
			// 
			this.AccessibleRole = System.Windows.Forms.AccessibleRole.HelpBalloon;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Info; // ColorScheme.GetColor(0xFFFFBD);//
			this.ClientSize = new System.Drawing.Size(192, 80);
			this.ControlBox = false;
			this.ForeColor = System.Drawing.SystemColors.InfoText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Balloon";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;

		}
		#endregion

		protected override void OnClick(EventArgs e)
		{
			if(m_ShowCloseButton && m_CloseButton.Bounds.Contains(this.PointToClient(Control.MousePosition)))
				return;
			base.OnClick(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
            SmoothingMode sm = e.Graphics.SmoothingMode;
            TextRenderingHint th = e.Graphics.TextRenderingHint;

            if (m_AntiAlias)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
            }

			if(m_Style==eBallonStyle.Balloon)
				PaintBalloon(e.Graphics);
			else
				PaintAlert(e.Graphics);

            e.Graphics.SmoothingMode = sm;
            e.Graphics.TextRenderingHint = th;
		}

		private void PaintAlert(Graphics g)
		{
			Rectangle client=this.GetAlertClientRect();
			if(PaintBackground!=null)
				PaintBackground(this,new CustomPaintEventArgs(g,client));
			else
			{
				PaintBackgroundInternal(g);
                if (m_Style == eBallonStyle.Alert)
                {
                    // Paint Caption Background
                    Rectangle rCap = client;
                    rCap.Height = Math.Max(m_CaptionImageRectangle.Bottom, m_CaptionRectangle.Bottom) - rCap.Top;
                    rCap.Height += CAPTIONALERT_VSPACING;
                    if (rCap.Width > 0 && rCap.Height > 0)
                    {
                        using (LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(rCap, this.BackColor, m_BackColor2, m_BackColorGradientAngle))
                        {
                            Blend b = new Blend(4);
                            b.Positions = new float[] { 0f, .5f, .5f, 1f };
                            b.Factors = new float[] { 0f, 1f, 1f, 0f };
                            gradient.Blend = b;
                            g.FillRectangle(gradient, rCap);
                        }
                    }
                }
			}

			// Draw Border
			Rectangle r=this.DisplayRectangle;
            if (m_Style == eBallonStyle.Office2007Alert)
            {
                DisplayHelp.DrawRectangle(g, m_BorderColor, r);
            }
            else
            {
                r.Width--;
                r.Height--;
                using (Pen pen = new Pen(m_BorderColor1, 1))
                {
                    g.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
                    g.DrawLine(pen, r.X, r.Y, r.X, r.Bottom);
                    g.DrawLine(pen, r.X + 4, r.Bottom - 4, r.Right - 4, r.Bottom - 4);
                    g.DrawLine(pen, r.Right - 4, r.Y + 4, r.Right - 4, r.Bottom - 4);
                }
                using (Pen pen = new Pen(m_BorderColor2, 1))
                {
                    g.DrawLine(pen, r.X + 1, r.Y + 1, r.Right - 1, r.Y + 1);
                    g.DrawLine(pen, r.X + 1, r.Y + 1, r.X + 1, r.Bottom - 1);
                    g.DrawLine(pen, r.X + 3, r.Bottom - 3, r.Right - 3, r.Bottom - 3);
                    g.DrawLine(pen, r.Right - 3, r.Y + 3, r.Right - 3, r.Bottom - 3);
                }
                using (Pen pen = new Pen(m_BorderColor3, 1))
                {
                    g.DrawLine(pen, r.X + 2, r.Y + 2, r.Right - 2, r.Y + 2);
                    g.DrawLine(pen, r.X + 2, r.Y + 2, r.X + 2, r.Bottom - 2);
                    g.DrawLine(pen, r.X + 2, r.Bottom - 2, r.Right - 2, r.Bottom - 2);
                    g.DrawLine(pen, r.Right - 2, r.Y + 2, r.Right - 2, r.Bottom - 2);
                }
                using (Pen pen = new Pen(m_BorderColor4, 1))
                {
                    g.DrawLine(pen, r.X + 3, r.Y + 3, r.Right - 3, r.Y + 3);
                    g.DrawLine(pen, r.X + 3, r.Y + 3, r.X + 3, r.Bottom - 3);
                    g.DrawLine(pen, r.X + 1, r.Bottom - 1, r.Right - 1, r.Bottom - 1);
                    g.DrawLine(pen, r.Right - 1, r.Y + 1, r.Right - 1, r.Bottom - 1);
                }
                using (Pen pen = new Pen(m_BorderColor5, 1))
                {
                    g.DrawLine(pen, r.X + 4, r.Y + 4, r.Right - 4, r.Y + 4);
                    g.DrawLine(pen, r.X + 4, r.Y + 4, r.X + 4, r.Bottom - 4);
                    g.DrawLine(pen, r.X, r.Bottom, r.Right, r.Bottom);
                    g.DrawLine(pen, r.Right, r.Y, r.Right, r.Bottom);
                }
            }

	
			if(m_CloseButton!=null)
				m_CloseButton.Paint(g);

			if(PaintCaptionImage!=null)
				PaintCaptionImage(this,new CustomPaintEventArgs(g,m_CaptionImageRectangle));
			else
			{
				CompositeImage image=this.ImageInternal;
				if(image!=null && !m_CaptionImageRectangle.IsEmpty)
					image.DrawImage(g,m_CaptionImageRectangle);
			}

			if(PaintCaptionText!=null)
				PaintCaptionText(this,new CustomPaintEventArgs(g,m_CaptionRectangle));
			else
			{
				if(m_CaptionText!="" && !m_CaptionRectangle.IsEmpty)
				{
                    eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                    if (m_CaptionAlignment == StringAlignment.Center)
                        format |= eTextFormat.HorizontalCenter;
                    else if (m_CaptionAlignment == StringAlignment.Far)
                        format |= eTextFormat.Right;
					Font font=m_CaptionFont;
					if(font==null)
						font=this.Font;

					TextDrawing.DrawString(g, m_CaptionText, font, m_CaptionColor, m_CaptionRectangle, format);
				}
			}

			if(PaintText!=null)
				PaintText(this,new CustomPaintEventArgs(g,m_TextRectangle));
			else
			{
				if(this.Text!="")
				{
                    eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                    if (m_TextAlignment == StringAlignment.Center)
                        format |= eTextFormat.HorizontalCenter;
                    else if (m_TextAlignment == StringAlignment.Far)
                        format |= eTextFormat.Right;
                    if (m_TextLineAlignment == StringAlignment.Center)
                        format |= eTextFormat.VerticalCenter;
                    else if (m_TextLineAlignment == StringAlignment.Far)
                        format |= eTextFormat.Bottom;
                    TextDrawing.DrawString(g, this.Text, this.Font, this.ForeColor, m_TextRectangle, format);
				}
			}
		}

		private void PaintBalloon(Graphics g)
		{
            Rectangle r = new Rectangle(0, 0, this.Width, this.Height); //this.DisplayRectangle;
			GraphicsPath path=this.GetBalloonPath(r);
			if(PaintBackground!=null)
				PaintBackground(this,new CustomPaintEventArgs(g,r));
			else
				PaintBackgroundInternal(g);

			using(Pen pen=new Pen(m_BorderColor,1))
			{
				pen.Alignment=PenAlignment.Inset;
				g.DrawPath(pen,path);
			}

			if(m_CloseButton!=null)
				m_CloseButton.Paint(g);

			if(PaintCaptionImage!=null)
				PaintCaptionImage(this,new CustomPaintEventArgs(g,m_CaptionImageRectangle));
			else
			{
				CompositeImage image=this.ImageInternal;
				if(image!=null && !m_CaptionImageRectangle.IsEmpty)
					image.DrawImage(g,m_CaptionImageRectangle);
			}

			if(PaintCaptionText!=null)
				PaintCaptionText(this,new CustomPaintEventArgs(g,m_CaptionRectangle));
			else
			{
				if(m_CaptionText!="" && !m_CaptionRectangle.IsEmpty)
				{
                    eTextFormat format = eTextFormat.Default | TextDrawing.TranslateHorizontal(m_CaptionAlignment) | eTextFormat.WordBreak;
					Font font=m_CaptionFont;
					if(font==null)
						font=this.Font;

                    TextDrawing.DrawString(g, m_CaptionText, font, m_CaptionColor, m_CaptionRectangle, format);
				}
			}

			if(PaintText!=null)
				PaintText(this,new CustomPaintEventArgs(g,m_TextRectangle));
			else
			{
				if(this.Text!="")
				{
                    eTextFormat format = eTextFormat.Default | TextDrawing.TranslateHorizontal(m_TextAlignment) |
                        TextDrawing.TranslateVertical(m_TextLineAlignment) | eTextFormat.WordBreak;
					TextDrawing.DrawString(g, this.Text, this.Font, this.ForeColor, m_TextRectangle, format);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if(!m_AnimationInProgress)
				this.RecalcLayout();
		}
        private int _MinimumBalloonWidth = 180;
        /// <summary>
        /// Gets or sets the minimum balloon width when auto sizing balloon. Default value is 180.
        /// </summary>
        [DefaultValue(180), Category("Appearance"), Description("Indicates minimum balloon width when auto sizing balloon.")]
        public int MinimumBalloonWidth
        {
            get { return _MinimumBalloonWidth; }
            set { _MinimumBalloonWidth = value; }
        }

		/// <summary>
		/// Auto resize balloon to the content. Balloon width is calculated so image and caption text can fit in single line.
		/// </summary>
		public void AutoResize()
		{
            System.Drawing.Size newSize = new Size(_MinimumBalloonWidth, 0);
			
			Size strSize=Size.Empty;

			// Calculates caption text rectangle height
			Font font=m_CaptionFont;
			if(font==null)
				font=this.Font;
			Graphics g=this.CreateGraphics();
			try
			{
                strSize = TextDrawing.MeasureString(g,(m_CaptionText != "" ? m_CaptionText : " "), font);

                newSize.Width = Math.Max(newSize.Width, strSize.Width);
				if(m_CloseButton!=null)
					newSize.Width+=m_CloseButton.Size.Width;
				else
					newSize.Width+=18;
				newSize.Width+=TEXTCLOSE_SPACING;
				
				int imageHeight=0;
				if(m_CaptionIcon!=null)
				{
					newSize.Width+=(m_CaptionIcon.Width+IMAGETEXT_SPACING);
                    imageHeight = m_CaptionIcon.Height;
				}
				else if(m_CaptionImage!=null)
				{
					newSize.Width+=(m_CaptionImage.Width+IMAGETEXT_SPACING);
                    imageHeight = m_CaptionImage.Height;
				}

				newSize.Height+=Math.Max(imageHeight,strSize.Height);
				newSize.Height+=IMAGETEXT_SPACING*2;

				strSize = TextDrawing.MeasureString(g, (this.Text != "" ? this.Text : " "), this.Font, newSize.Width,eTextFormat.Default | eTextFormat.WordBreak);
				newSize.Height+=strSize.Height;

				newSize.Width+=m_CornerSize*2;
				newSize.Height+=m_CornerSize*2;
				
				if(m_Style==eBallonStyle.Balloon)
				{
					if(m_TipPosition==eTipPosition.Left || m_TipPosition==eTipPosition.Right)
						newSize.Width+=m_TipLength;
					else
						newSize.Height+=m_TipLength;
				}
				else
					newSize.Height+=(int)((float)newSize.Height*.3);
			}
			finally
			{
				g.Dispose();
			}

			this.Size=newSize;
		}

		/// <summary>
		/// Recalculates layout of the balloon.
		/// </summary>
		public void RecalcLayout()
		{
			if(this.DisplayRectangle.Width==0 || this.DisplayRectangle.Height==0)
				return;
            
			if(m_Style==eBallonStyle.Balloon)
			{
				Rectangle client=new Rectangle(m_CornerSize/2,m_CornerSize/2,this.Width-m_CornerSize,this.Height-m_CornerSize);
				if(m_TipPosition==eTipPosition.Bottom)
					client.Height-=m_TipLength;
				else if(m_TipPosition==eTipPosition.Left)
				{
					client.X+=m_TipLength;
					client.Width-=m_TipLength;
				}
				else if(m_TipPosition==eTipPosition.Right)
					client.Width-=m_TipLength;
				else
				{
					// TOP
					client.Y+=m_TipLength;
					client.Height-=m_TipLength;
				}

				System.Drawing.Size imageSize=System.Drawing.Size.Empty;
				if(m_CaptionIcon!=null)
					imageSize=m_CaptionIcon.Size;
				else if(m_CaptionImage!=null)
					imageSize=m_CaptionImage.Size;

				m_CaptionRectangle=client;
				if(!imageSize.IsEmpty)
				{
					m_CaptionRectangle.X+=(imageSize.Width+IMAGETEXT_SPACING);
					m_CaptionRectangle.Width-=(imageSize.Width+IMAGETEXT_SPACING);
				}
				
				// Calculates position of the close button if visible
				if(m_CloseButton!=null)
				{
					m_CloseButton.Location=new Point(client.Right-m_CloseButton.Bounds.Width,client.Top);
					m_CaptionRectangle.Width-=(m_CloseButton.Size.Width+TEXTCLOSE_SPACING);
				}

				// Calculates caption text rectangle height
				Font font=m_CaptionFont;
				if(font==null)
					font=this.Font;
				Graphics g=this.CreateGraphics();
				try
				{
                    eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                    if (m_CaptionAlignment == StringAlignment.Center)
                        format = eTextFormat.HorizontalCenter;
                    else if (m_CaptionAlignment == StringAlignment.Far)
                        format = eTextFormat.Right;
					Size strSize=TextDrawing.MeasureString(g,(m_CaptionText!=""?m_CaptionText:" "),font,m_CaptionRectangle.Width,format);
					m_CaptionRectangle.Height=strSize.Height;
				}
				finally
				{
					g.Dispose();
				}

				// Calculates position of the image rectangle
				if(!imageSize.IsEmpty)
				{
					m_CaptionImageRectangle=new Rectangle(client.X,client.Y,imageSize.Width,imageSize.Height);
					if(m_CaptionRectangle.Height>imageSize.Height)
						m_CaptionImageRectangle.Y+=(int)(m_CaptionRectangle.Height-imageSize.Height)/2;
					else if(imageSize.Height>m_CaptionRectangle.Height)
						m_CaptionRectangle.Y+=(int)(imageSize.Height-m_CaptionRectangle.Height)/2;

				}
				else if(m_CloseButton!=null && m_CloseButton.Size.Height>m_CaptionRectangle.Height)
					m_CaptionRectangle.Y+=(int)(m_CloseButton.Size.Height-m_CaptionRectangle.Height)/2;
				
				// Calculates the rectangle for balloon text
				if(m_CaptionRectangle.Bottom>m_CaptionImageRectangle.Bottom)
					m_TextRectangle=new Rectangle(client.X,m_CaptionRectangle.Bottom+CAPTIONTEXT_VSPACING,client.Width,client.Bottom-(m_CaptionRectangle.Bottom+CAPTIONTEXT_VSPACING));
				else
					m_TextRectangle=new Rectangle(client.X,m_CaptionImageRectangle.Bottom+CAPTIONTEXT_VSPACING,client.Width,client.Bottom-(m_CaptionImageRectangle.Bottom+CAPTIONTEXT_VSPACING));

				// Calculates and sets the balloon window region
				GraphicsPath path=this.GetBalloonPath(this.DisplayRectangle);
				GraphicsPath tmpPath = (GraphicsPath)path.Clone();
				tmpPath.Widen(Pens.Black);
				Region region = new Region(tmpPath);
				region.Union(path);
				this.Region=region;
                path.Dispose();
                tmpPath.Dispose();
			}
			else
			{
				Rectangle client=this.GetAlertClientRect();
				System.Drawing.Size imageSize=System.Drawing.Size.Empty;

				if(m_CaptionIcon!=null)
					imageSize=m_CaptionIcon.Size;
				else if(m_CaptionImage!=null)
					imageSize=m_CaptionImage.Size;

				m_CaptionRectangle=client;
				if(!imageSize.IsEmpty)
				{
					m_CaptionRectangle.X+=(imageSize.Width+IMAGETEXT_SPACING*2);
					m_CaptionRectangle.Width-=(imageSize.Width+IMAGETEXT_SPACING*2);
				}
				
				// Calculates position of the close button if visible
				if(m_CloseButton!=null)
				{
					m_CloseButton.Location=new Point(client.Right-m_CloseButton.Bounds.Width-CAPTIONALERT_VSPACING,client.Top);
					m_CaptionRectangle.Width-=(m_CloseButton.Size.Width+TEXTCLOSE_SPACING+CAPTIONALERT_VSPACING);
				}

				// Calculates caption text rectangle height
				Font font=m_CaptionFont;
				if(font==null)
					font=this.Font;
				Graphics g=this.CreateGraphics();
				try
				{
                    eTextFormat format = eTextFormat.Default | eTextFormat.WordBreak;
                    if (m_CaptionAlignment == StringAlignment.Center)
                        format = eTextFormat.HorizontalCenter;
                    else if (m_CaptionAlignment == StringAlignment.Far)
                        format = eTextFormat.Right;
                    Size strSize = TextDrawing.MeasureString(g, (m_CaptionText != "" ? m_CaptionText : " "), font, m_CaptionRectangle.Width, format);
					m_CaptionRectangle.Height=strSize.Height;
				}
				finally
				{
					g.Dispose();
				}

				// Calculates position of the image rectangle
				if(!imageSize.IsEmpty)
				{
					m_CaptionImageRectangle=new Rectangle(client.X+IMAGETEXT_SPACING,client.Y,imageSize.Width,imageSize.Height);
					if(m_CaptionRectangle.Height>imageSize.Height)
						m_CaptionImageRectangle.Y+=(int)(m_CaptionRectangle.Height-imageSize.Height)/2;
					else if(imageSize.Height>m_CaptionRectangle.Height)
					{
						m_CaptionRectangle.Y+=(int)(imageSize.Height+-m_CaptionRectangle.Height)/2;
					}

				}
				else if(m_CloseButton!=null && m_CloseButton.Size.Height>m_CaptionRectangle.Height)
					m_CaptionRectangle.Y+=(int)(m_CloseButton.Size.Height-m_CaptionRectangle.Height)/2;

				m_CaptionRectangle.Offset(0,CAPTIONALERT_VSPACING);
				m_CaptionImageRectangle.Offset(0,CAPTIONALERT_VSPACING);

				if(m_CloseButton!=null)
				{
					m_CloseButton.Location=new Point(m_CloseButton.Location.X,m_CaptionRectangle.Y+(m_CaptionRectangle.Height-m_CloseButton.Size.Height)/2);
				}

				
				// Calculates the rectangle for balloon text
				if(m_CaptionRectangle.Bottom>m_CaptionImageRectangle.Bottom)
					m_TextRectangle=new Rectangle(client.X,m_CaptionRectangle.Bottom+CAPTIONTEXT_VSPACING*4,client.Width,client.Bottom-(m_CaptionRectangle.Bottom+CAPTIONTEXT_VSPACING*4));
				else
					m_TextRectangle=new Rectangle(client.X,m_CaptionImageRectangle.Bottom+CAPTIONTEXT_VSPACING*4,client.Width,client.Bottom-(m_CaptionImageRectangle.Bottom+CAPTIONTEXT_VSPACING*4));

				this.Region=new Region(this.DisplayRectangle);
			}
		}

		private Rectangle GetAlertClientRect()
		{
            Rectangle r = new Rectangle(0, 0, this.Width, this.Height); // this.DisplayRectangle;
			r.Inflate(-4,-4);
			return r;
		}

		private GraphicsPath GetBalloonPath(Rectangle rect)
		{
			GraphicsPath path = new GraphicsPath();

			Rectangle clipBounds=rect;
			Rectangle balloonBounds=clipBounds;
			int cornerDiameter=m_CornerSize*2;
			Matrix matrix=new Matrix();
			Point[] anchorPoints=new Point[3];
			int tipOffset=m_TipOffset;

			balloonBounds.Size=new Size(balloonBounds.Width-1,balloonBounds.Height-1);

			bool adjustOffset = false;
			if(m_TipPosition==eTipPosition.Bottom)
			{
				adjustOffset=true;
				matrix.Translate(balloonBounds.Width, balloonBounds.Height);
				matrix.Rotate(180);
			}
            else if (m_TipPosition == eTipPosition.Left && this.RightToLeft == RightToLeft.No || 
                m_TipPosition == eTipPosition.Right && this.RightToLeft == RightToLeft.Yes)
			{
				adjustOffset = true;
				balloonBounds.Size=new Size(balloonBounds.Height, balloonBounds.Width);
				matrix.Translate(0, balloonBounds.Width);
				matrix.Rotate(-90);
			}
			else if(m_TipPosition==eTipPosition.Right && this.RightToLeft == RightToLeft.No ||
                m_TipPosition == eTipPosition.Left && this.RightToLeft == RightToLeft.Yes)
			{
				balloonBounds.Size=new Size(balloonBounds.Height, balloonBounds.Width);
				matrix.Translate(balloonBounds.Height, 0);
				matrix.Rotate(90);
			}

			int balloonEdgeCenter =((int)(balloonBounds.Width-(m_CornerSize*2))/2)+1;
			int offsetFromEdge = m_CornerSize;
			if(adjustOffset)
			{
				tipOffset=(balloonBounds.Width-(offsetFromEdge*2)-m_TipOffset)+1;
			}
			if(tipOffset<balloonEdgeCenter)
			{
				anchorPoints[0]=new Point(tipOffset+offsetFromEdge,balloonBounds.Y+m_TipLength);
				anchorPoints[1]=new Point(tipOffset+offsetFromEdge,balloonBounds.Y);
				anchorPoints[2]=new Point(tipOffset+m_TipLength+offsetFromEdge,balloonBounds.Y+m_TipLength);
			} 
			else if(tipOffset>balloonEdgeCenter)
			{
				anchorPoints[0]=new Point(tipOffset-m_TipLength+offsetFromEdge,balloonBounds.Y+m_TipLength);
				anchorPoints[1]=new Point(tipOffset+offsetFromEdge,balloonBounds.Y);
				anchorPoints[2]=new Point(tipOffset+offsetFromEdge,balloonBounds.Y+m_TipLength);
			} 
			else 
			{
				anchorPoints[0]=new Point(tipOffset-m_TipLength+offsetFromEdge,balloonBounds.Y+m_TipLength);
				anchorPoints[1]=new Point(tipOffset+offsetFromEdge,balloonBounds.Y);
				anchorPoints[2]=new Point(tipOffset+m_TipLength+offsetFromEdge,balloonBounds.Y+m_TipLength);
			}

			path.AddArc(balloonBounds.Left,balloonBounds.Top+m_TipLength,cornerDiameter, cornerDiameter,180, 90);

			path.AddLine(anchorPoints[0],anchorPoints[1]);
			path.AddLine(anchorPoints[1],anchorPoints[2]);

			path.AddArc(balloonBounds.Width-cornerDiameter,balloonBounds.Top+m_TipLength,cornerDiameter, cornerDiameter, -90, 90);
			path.AddArc(balloonBounds.Width-cornerDiameter,balloonBounds.Bottom-cornerDiameter,cornerDiameter, cornerDiameter, 0, 90);
			path.AddArc(balloonBounds.Left, balloonBounds.Bottom-cornerDiameter,cornerDiameter, cornerDiameter, 90, 90);

			path.CloseFigure();
				
			path.Transform(matrix);
			
			return path;
		}

		private void PaintBackgroundInternal(Graphics g)
		{
			Rectangle r=this.ClientRectangle;
			if(r.Width==0 || r.Height==0)
				return;

			if(m_BackColor2.IsEmpty)
			{
				using(SolidBrush brush=new SolidBrush(this.BackColor))
					g.FillRectangle(brush,r);
			}
			else
			{
				using(LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(r,this.BackColor,m_BackColor2,m_BackColorGradientAngle))
				{
					if(m_Style==eBallonStyle.Alert)
					{
						Blend b=new Blend(4);
						b.Positions=new float[]{0f,.25f,.4f,.5f,.5f,.85f,.85f,1f};
						b.Factors=new float[] {0f,1f,1f,0f,0f,0f,0f,1f};
						gradient.Blend=b;
					}
					g.FillRectangle(gradient,r);
				}
			}

			if(this.BackgroundImage==null)
				return;

//			System.Drawing.Imaging.ImageAttributes imageAtt=null;
//			
//			if(m_BackgroundImageAlpha!=255)
//			{
//				System.Drawing.Imaging.ColorMatrix colorMatrix=new System.Drawing.Imaging.ColorMatrix();
//				colorMatrix.Matrix33=255-m_BackgroundImageAlpha;
//				imageAtt=new System.Drawing.Imaging.ImageAttributes();
//				imageAtt.SetColorMatrix(colorMatrix,System.Drawing.Imaging.ColorMatrixFlag.Default,System.Drawing.Imaging.ColorAdjustType.Bitmap);
//			}

			BarFunctions.PaintBackgroundImage(g,r,this.BackgroundImage,m_BackgroundImagePosition,m_BackgroundImageAlpha);

//			switch(m_BackgroundImagePosition)
//			{
//				case eBackgroundImagePosition.Stretch:
//				{
//					if(imageAtt!=null)
//						g.DrawImage(this.BackgroundImage,r,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel,imageAtt);
//					else
//						g.DrawImage(this.BackgroundImage,r,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel);
//					break;
//				}
//				case eBackgroundImagePosition.Center:
//				{
//					Rectangle destRect=new Rectangle(r.X,r.Y,this.BackgroundImage.Width,this.BackgroundImage.Height);
//					if(r.Width>this.BackgroundImage.Width)
//						destRect.X+=(r.Width-this.BackgroundImage.Width)/2;
//					if(r.Height>this.BackgroundImage.Height)
//						destRect.Y+=(r.Height-this.BackgroundImage.Height)/2;
//					if(imageAtt!=null)
//						g.DrawImage(this.BackgroundImage,destRect,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel,imageAtt);
//					else
//						g.DrawImage(this.BackgroundImage,destRect,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel);
//					break;
//				}
//				case eBackgroundImagePosition.TopLeft:
//				case eBackgroundImagePosition.TopRight:
//				case eBackgroundImagePosition.BottomLeft:
//				case eBackgroundImagePosition.BottomRight:
//				{
//					Rectangle destRect=new Rectangle(r.X,r.Y,this.BackgroundImage.Width,this.BackgroundImage.Height);
//					if(m_BackgroundImagePosition==eBackgroundImagePosition.TopRight)
//						destRect.X=r.Right-this.BackgroundImage.Width;
//					else if(m_BackgroundImagePosition==eBackgroundImagePosition.BottomLeft)
//						destRect.Y=r.Bottom-this.BackgroundImage.Height;
//					else if(m_BackgroundImagePosition==eBackgroundImagePosition.BottomRight)
//					{
//						destRect.Y=r.Bottom-this.BackgroundImage.Height;
//						destRect.X=r.Right-this.BackgroundImage.Width;
//					}
//
//					if(imageAtt!=null)
//						g.DrawImage(this.BackgroundImage,destRect,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel,imageAtt);
//					else
//						g.DrawImage(this.BackgroundImage,destRect,0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel);
//					break;
//				}
//				case eBackgroundImagePosition.Tile:
//				{
//					if(imageAtt!=null)
//					{
//						if(r.Width>this.BackgroundImage.Width || r.Height>this.BackgroundImage.Height)
//						{
//							int x=r.X,y=r.Y;
//							while(y<r.Bottom)
//							{
//								while(x<r.Right)
//								{
//									Rectangle destRect=new Rectangle(x,y,this.BackgroundImage.Width,this.BackgroundImage.Height);
//									if(destRect.Right>r.Right)
//										destRect.Width=destRect.Width-(destRect.Right-r.Right);
//									if(destRect.Bottom>r.Bottom)
//										destRect.Height=destRect.Height-(destRect.Bottom-r.Bottom);
//									g.DrawImage(this.BackgroundImage,destRect,0,0,destRect.Width,destRect.Height,GraphicsUnit.Pixel,imageAtt);
//									x+=this.BackgroundImage.Width;
//								}
//								x=r.X;
//								y+=this.BackgroundImage.Height;
//							}
//						}
//						else
//						{
//							g.DrawImage(this.BackgroundImage,new Rectangle(0,0,this.BackgroundImage.Width,this.BackgroundImage.Height),0,0,this.BackgroundImage.Width,this.BackgroundImage.Height,GraphicsUnit.Pixel,imageAtt);
//						}
//					}
//					else
//					{
//						TextureBrush brush=new TextureBrush(this.BackgroundImage); //,r);
//						brush.WrapMode=System.Drawing.Drawing2D.WrapMode.Tile;
//						g.FillRectangle(brush,r);
//						brush.Dispose();
//					}
//					break;
//				}
//			}
		}

		/// <summary>
		/// Gets or sets the target gradient background color.
		/// </summary>
		[Browsable(true),Description("Indicates the target gradient background color."),Category("Style")]
		public Color BackColor2
		{
			get {return m_BackColor2;}
			set
			{
				m_BackColor2=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBackColor2()
		{
			return !m_BackColor2.IsEmpty;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBackColor2()
		{
			m_BackColor2=Color.Empty;
		}

		/// <summary>
		/// Gets or sets gradient fill angle.
		/// </summary>
		[Browsable(true),Description("Indicates gradient fill angle."),Category("Style"),DefaultValue(90)]
		public int BackColorGradientAngle
		{
			get {return m_BackColorGradientAngle;}
			set
			{
				if(value!=m_BackColorGradientAngle)
				{
					m_BackColorGradientAngle=value;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}
		/// <summary>
		/// Specifies the transparency of background image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(255),Description("Specifies the transparency of background image.")]
		public int BackgroundImageAlpha
		{
			get {return m_BackgroundImageAlpha;}
			set
			{
				if(value<0)
					value=0;
				else if(value>255)
					value=255;
				if(m_BackgroundImageAlpha!=value)
				{
					m_BackgroundImageAlpha=value;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}
		/// <summary>
		/// Specifies background image position when container is larger than image.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Appearance"),DefaultValue(eBackgroundImagePosition.Stretch),Description("Specifies background image position when container is larger than image.")]
		public eBackgroundImagePosition BackgroundImagePosition
		{
			get {return m_BackgroundImagePosition;}
			set
			{
				if(m_BackgroundImagePosition!=value)
				{
					m_BackgroundImagePosition=value;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the border color..
		/// </summary>
		[Browsable(true),Description("Indicates border color."),Category("Style")]
		public Color BorderColor
		{
			get {return m_BorderColor;}
			set
			{
				m_BorderColor=value;
				if(this.DesignMode)
					this.Refresh();
			}
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBorderColor()
		{
			return m_BorderColor!=SystemColors.InfoText;
		}
		[Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
		public void ResetBorderColor()
		{
			m_BorderColor=SystemColors.InfoText;
		}

		/// <summary>
		/// Specifies balloon style.
		/// </summary>
		[Browsable(true),DevCoBrowsable(true),Category("Style"),DefaultValue(eBallonStyle.Balloon),Description("Specifies balloon style.")]
		public eBallonStyle Style
		{
			get {return m_Style;}
			set
			{
				m_Style=value;
				if(m_Style==eBallonStyle.Alert)
				{
					this.BackColor=Color.FromArgb(207,221,244);
					this.BackColor2=Color.White;
					this.ForeColor=Color.FromArgb(102,114,196);
					m_TextAlignment=StringAlignment.Center;
				}
                else if (m_Style == eBallonStyle.Office2007Alert)
                {
                    Office2007ColorTable ct = GetOffice2007ColorTable();
                    ColorScheme cs = ct.LegacyColors;
                    if (ct.SuperTooltip.BackgroundColors.IsEmpty)
                    {
                        this.BackColor = cs.PanelBackground;
                        this.BackColor2 = cs.PanelBackground2;
                        this.ForeColor = cs.PanelText;
                    }
                    else
                    {
                        this.BackColor = ct.SuperTooltip.BackgroundColors.Start;
                        this.BackColor2 = ct.SuperTooltip.BackgroundColors.End;
                        this.ForeColor = ct.SuperTooltip.TextColor;
                    }
                    m_BorderColor = cs.PanelBorder;
                    m_TextAlignment = StringAlignment.Near;
                }
                else
                {
                    m_TextAlignment = StringAlignment.Near;
                    m_BorderColor = SystemColors.InfoText;
                }
                if(m_ShowCloseButton)
				    this.CreateCloseButton();
				this.RecalcLayout();
				if(this.DesignMode)
					this.Refresh();
			}
		}

        private Office2007ColorTable GetOffice2007ColorTable()
        {
            Office2007Renderer r = GlobalManager.Renderer as Office2007Renderer;
            if (r != null)
                return r.ColorTable;
            return new Office2007ColorTable();
        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(m_CloseButton!=null)
				m_CloseButton.OnMouseDown(e);
			if(this.Focused)
				this.Focus();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(m_CloseButton!=null)
				m_CloseButton.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(m_CloseButton!=null)
				m_CloseButton.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if(m_CloseButton!=null)
				m_CloseButton.OnMouseLeave(e);
		}

		/// <summary>
		/// Gets or sets whether the Close button is displayed.
		/// </summary>
		[Browsable(true),Description("Indicates whether the Close button is displayed."),Category("Behavior"),DefaultValue(true)]
		public bool ShowCloseButton
		{
			get {return m_ShowCloseButton;}
			set
			{
				if(value!=m_ShowCloseButton)
				{
					m_ShowCloseButton=value;
					OnShowCloseButtonChanged();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		private void OnShowCloseButtonChanged()
		{
			if(m_ShowCloseButton)
			{
				CreateCloseButton();
				RecalcLayout();
			}
			else
			{
				m_CloseButton=null;
			}
		}

		private void CreateCloseButton()
		{
            if (m_CloseButtonNormal != null)
            {
                m_CloseButton = new ImageButton(this, new Rectangle(0, 0, m_CloseButtonNormal.Width, m_CloseButtonNormal.Height));
                if (m_CloseButton.Normal != null)
                {
                    m_CloseButton.Normal.Dispose();
                    m_CloseButton.Normal = null;
                }
                m_CloseButton.Normal = m_CloseButtonNormal;
                if (m_CloseButton.Hover != null)
                {
                    m_CloseButton.Hover.Dispose();
                    m_CloseButton.Hover = null;
                }
                m_CloseButton.Hover = m_CloseButtonHot;
                if (m_CloseButton.Pressed != null)
                {
                    m_CloseButton.Pressed.Dispose();
                    m_CloseButton.Pressed = null;
                }
                m_CloseButton.Pressed = m_CloseButtonPressed;
            }
            else // Default Image Size is 18x18
            {
                if (m_Style == eBallonStyle.Balloon)
                {
                    m_CloseButton = new ImageButton(this, new Rectangle(0, 0, 18, 18));
                    m_CloseButton.Normal = BarFunctions.LoadBitmap("BalloonImages.BalloonNormal.png");
                    m_CloseButton.Hover = BarFunctions.LoadBitmap("BalloonImages.BalloonHot.png");
                    m_CloseButton.Pressed = BarFunctions.LoadBitmap("BalloonImages.BalloonPress.png");
                }
                else
                {
                    m_CloseButton = new ImageButton(this, new Rectangle(0, 0, 13, 13));
                    m_CloseButton.Normal = BarFunctions.LoadBitmap("BalloonImages.AlertNormal.png");
                    m_CloseButton.Hover = BarFunctions.LoadBitmap("BalloonImages.AlertHot.png");
                    m_CloseButton.Pressed = BarFunctions.LoadBitmap("BalloonImages.AlertPress.png");
                }
            }
			m_CloseButton.Click+=new EventHandler(this.CloseButtonClickInternal);
		}

		private void CloseButtonClickInternal(object sender, EventArgs e)
		{
			if(CloseButtonClick!=null)
				CloseButtonClick(this,new EventArgs());
			if(m_AutoClose)
			{
				this.Hide();
				this.Close();
			}
		}

		/// <summary>
		/// Gets or sets the animation type used to display Alert type balloon.
		/// </summary>
		[Browsable(true),Description("Gets or sets the animation type used to display Alert type balloon."),Category("Behavior"),DefaultValue(eAlertAnimation.BottomToTop)]
		public eAlertAnimation AlertAnimation
		{
			get {return m_AlertAnimation;}
			set {m_AlertAnimation=value;}
		}

		/// <summary>
		/// Gets or sets the total time in milliseconds alert animation takes.
		/// Default value is 200.
		/// </summary>
		[Browsable(true),Description("Gets or sets the total time in milliseconds alert animation takes."),Category("Behavior"),DefaultValue(200)]
		public int AlertAnimationDuration
		{
			get {return m_AlertAnimationDuration;}
			set {m_AlertAnimationDuration=value;}
		}

		/// <summary>
		/// Gets or sets whether balloon will close automatically when user click the close button.
		/// </summary>
		[Browsable(true),Description("Indicates whether balloon will close automatically when user click the close button."),Category("Behavior"),DefaultValue(true)]
		public bool AutoClose
		{
			get {return m_AutoClose;}
			set {m_AutoClose=value;}
		}

		/// <summary>
		/// Gets or sets time period in seconds after balloon closes automatically.
		/// </summary>
		[Browsable(true),Description("Indicates time period in seconds after balloon closes automatically."),Category("Behavior"),DefaultValue(0)]
		public int AutoCloseTimeOut
		{
			get {return m_AutoCloseTimeOut;}
			set
			{
				m_AutoCloseTimeOut=value;
				OnAutoCloseTimeOutChanged();
			}
		}

		protected void OnAutoCloseTimeOutChanged()
		{
			if(m_AutoCloseTimeOut>0 && !this.DesignMode)
			{
				if(m_Timer==null)
					m_Timer=new System.Windows.Forms.Timer(components);
				m_Timer.Enabled=false;
				m_Timer.Interval=m_AutoCloseTimeOut*1000;
				m_Timer.Tick+=new EventHandler(this.AutoCloseTimeOutEllapsed);
				if(this.Visible)
				{
					m_Timer.Enabled=true;
					m_Timer.Start();
				}
			}
			else
			{
				DestroyAutoCloseTimer();
			}
		}

		protected virtual void AutoCloseTimeOutEllapsed(object sender, EventArgs e)
		{
			if(this.IsDisposed)
				return;
			DestroyAutoCloseTimer();
			this.Hide();
			this.Close();
		}

		private void DestroyAutoCloseTimer()
		{
			if(m_Timer!=null)
			{
				m_Timer.Enabled=false;
				m_Timer.Tick-=new EventHandler(this.AutoCloseTimeOutEllapsed);
				components.Remove(m_Timer);
				m_Timer.Dispose();
				m_Timer=null;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if(this.Visible)
			{
				if(m_Timer!=null && !m_Timer.Enabled)
				{
					m_Timer.Enabled=true;
					m_Timer.Start();
				}
			}
			else
			{
				if(m_Timer!=null && m_Timer.Enabled)
				{
					m_Timer.Stop();
					m_Timer.Enabled=false;
				}
			}
		}

		/// <summary>
		/// Gets or sets the custom image for Close Button.
        /// </summary>
        [Browsable(true),Description("Indicates custom image for Close Button."),Category("Appearance")]
		public Image CloseButtonNormal
		{
			get {return m_CloseButtonNormal;}
			set 
			{
				if(m_CloseButtonNormal!=value)
				{
					m_CloseButtonNormal=value;
					OnShowCloseButtonChanged();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the custom image for Close Button when mouse is over the button.
        /// </summary>
        [Browsable(true),Description("Indicates custom image for Close Button when mouse is over the button."),Category("Appearance"),DefaultValue(null)]
		public Image CloseButtonHot
		{
			get {return m_CloseButtonHot;}
			set 
			{
				if(m_CloseButtonHot!=value)
				{
					m_CloseButtonHot=value;
					OnShowCloseButtonChanged();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the custom image for Close Button when button is pressed.
        /// </summary>
        [Browsable(true),Description("Indicates custom image for Close Button when button is pressed."),Category("Appearance"),DefaultValue(null)]
		public Image CloseButtonPressed
		{
			get {return m_CloseButtonPressed;}
			set 
			{
				if(m_CloseButtonPressed!=value)
				{
					m_CloseButtonPressed=value;
					OnShowCloseButtonChanged();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the Caption image.
        /// </summary>
        [Browsable(true),Description("Indicates Caption image."),Category("Appearance"),DefaultValue(null)]
		public Image CaptionImage
		{
			get {return m_CaptionImage;}
			set 
			{
				if(m_CaptionImage!=value)
				{
					m_CaptionImage=value;
					this.RecalcLayout();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets the Caption icon. Icon is used to provide support for alpha-blended images in caption.
		/// </summary>
		[Browsable(true),Description("Indicates Caption icon. Icon is used to provide support for alpha-blended images in caption."),Category("Appearance"),DefaultValue(null)]
		public Icon CaptionIcon
		{
			get {return m_CaptionIcon;}
			set 
			{
				if(m_CaptionIcon!=value)
				{
					m_CaptionIcon=value;
					this.RecalcLayout();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		private CompositeImage ImageInternal
		{
			get
			{
				if(m_CaptionIcon!=null)
					return new CompositeImage(m_CaptionIcon,false);
				else if(m_CaptionImage!=null)
					return new CompositeImage(m_CaptionImage,false);
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the Caption font.
		/// </summary>
		[Browsable(true),Description("Indicates Caption font."),Category("Appearance")]
		public Font CaptionFont
		{
			get {return m_CaptionFont;}
			set 
			{
				if(m_CaptionFont!=value)
				{
					m_CaptionFont=value;
					this.RecalcLayout();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets text displayed in caption.
		/// </summary>
		[Browsable(true),Description("Indicates text displayed in caption."),Category("Appearance"),DefaultValue(""), Localizable(true)]
		public string CaptionText
		{
			get {return m_CaptionText;}
			set 
			{
				if(m_CaptionText!=value)
				{
					m_CaptionText=value;
					this.RecalcLayout();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or sets color of caption text.
		/// </summary>
		[Browsable(true),Description("Indicates color of caption text"),Category("Appearance")]
		public Color CaptionColor
		{
			get {return m_CaptionColor;}
			set 
			{
				if(m_CaptionColor!=value)
				{
					m_CaptionColor=value;
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Gets or set position of the balloon tip.
		/// </summary>
		[Browsable(true),Description("Indicates position of the balloon tip."),Category("Appearance"),DefaultValue(eTipPosition.Top)]
		public eTipPosition TipPosition
		{
			get {return m_TipPosition;}
			set
			{
				if(m_TipPosition!=value)
				{
					m_TipPosition=value;
					this.RecalcLayout();
					if(TipPositionChanged!=null)
						TipPositionChanged(this,new EventArgs());
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		private void InternalTipPositionChanged(eTipPosition newValue)
		{
			if(this.Controls.Count==0 || this.Style!=eBallonStyle.Balloon)
				return;
			if(newValue==eTipPosition.Top && m_TipPosition==eTipPosition.Bottom)
			{
				foreach(Control control in this.Controls)
				{
					control.Top+=m_TipLength;
				}
			}
			else if(newValue==eTipPosition.Bottom && m_TipPosition==eTipPosition.Top)
			{
				foreach(Control control in this.Controls)
				{
					control.Top-=m_TipLength;
				}
			}
		}

		/// <summary>
		/// Gets or sets tip distance from the edge of the balloon.
		/// </summary>
		[Browsable(true),Description("Indicates tip distance from the edge of the balloon."),Category("Appearance"),DefaultValue(32)]
		public int TipOffset
		{
			get {return m_TipOffset;}
			set
			{
				if(m_TipOffset!=value)
				{
					if(value<m_CornerSize)
						value=m_CornerSize+1;
					else if((m_TipPosition==eTipPosition.Top || m_TipPosition==eTipPosition.Bottom) && value>this.Width-m_CornerSize-m_TipLength)
						value=this.Width-m_CornerSize-m_TipLength;
					else if((m_TipPosition==eTipPosition.Left || m_TipPosition==eTipPosition.Right) && value>this.Height-m_CornerSize-m_TipLength)
						value=this.Height-m_CornerSize-m_TipLength;

					m_TipOffset=value;
					this.RecalcLayout();
					if(this.DesignMode)
						this.Refresh();
				}
			}
		}

		/// <summary>
		/// Returns length of the tip.
		/// </summary>
		[Browsable(false),Description("Returns tip length"),Category("Appearance")]
		public int TipLength
		{
			get {return m_TipLength;}
		}

		/// <summary>
		/// Displays balloon using control to automatically calculate balloon location. Method is usually used display balloon that is showing information for the certain control.
		/// </summary>
		/// <param name="referenceControl">Control used for balloon positioning.</param>
		public void Show(Control referenceControl)
		{
			this.Show(referenceControl,true);
		}

		/// <summary>
		/// Displays balloon using control to automatically calculate balloon location. Method is usually used display balloon that is showing information for the certain control.
		/// </summary>
		/// <param name="referenceControl">Control used for balloon positioning.</param>
		/// <param name="balloonFocus">Indicates whether balloon receives input focus.</param>
		public void Show(Control referenceControl, bool balloonFocus)
		{
			if(referenceControl==null)
			{
				this.Show();
				return;
			}
			Point scrLocCtrl=Point.Empty;
			
			if(referenceControl.Parent!=null)
				scrLocCtrl=referenceControl.Parent.PointToScreen(referenceControl.Location);
			else
				scrLocCtrl=referenceControl.Location;
			
			this.Show(new Rectangle(scrLocCtrl,referenceControl.Size),balloonFocus);
		}

		/// <summary>
		/// Displays balloon using rectangle to automatically calculate balloon location. Method is usually used display balloon that is showing information for the certain screen region.
		/// </summary>
		/// <param name="referenceScreenRect">Rectangle in screen coordinates used for balloon positioning.</param>
		/// <param name="balloonFocus">Indicates whether balloon receives input focus.</param>
		public void Show(Rectangle referenceScreenRect, bool balloonFocus)
		{
			if(referenceScreenRect.IsEmpty)
			{
				this.Show();
				return;
			}

			int tipPositionOffset=m_TipLength+m_CornerSize*3;
			int tipOffset=m_TipLength/2+m_CornerSize*3;
			Point balloonLocation=Point.Empty;
			ScreenInformation screen=BarFunctions.ScreenFromPoint(referenceScreenRect.Location);
			
			if(referenceScreenRect.X+referenceScreenRect.Width/2+this.Width>screen.WorkingArea.Right)
			{
				balloonLocation.X=referenceScreenRect.X+referenceScreenRect.Width/2-this.Width+tipPositionOffset;
				tipOffset=this.Width-tipPositionOffset;
			}
			else
				balloonLocation.X=referenceScreenRect.X+referenceScreenRect.Width/2-tipPositionOffset;
			
			if(referenceScreenRect.Y+referenceScreenRect.Height+this.Height>screen.WorkingArea.Bottom)
			{
				balloonLocation.Y=referenceScreenRect.Y-this.Height;
				InternalTipPositionChanged(eTipPosition.Bottom);
				this.TipPosition=eTipPosition.Bottom;
			}
			else
			{
				balloonLocation.Y=referenceScreenRect.Y+referenceScreenRect.Height;
				InternalTipPositionChanged(eTipPosition.Top);
				this.TipPosition=eTipPosition.Top;
			}

			this.TipOffset=tipOffset;
			this.Location=balloonLocation;
			this.Show(balloonFocus);
		}

		/// <summary>
		/// Displays balloon using item to automatically calculate balloon location. Method is usually used display balloon that is showing information for the certain item.
		/// </summary>
		/// <param name="item">Item used for balloon positioning.</param>
		/// <param name="balloonFocus">Indicates whether balloon receives input focus.</param>
		public void Show(BaseItem item, bool balloonFocus)
		{
			if(item==null || item.ContainerControl==null)
			{
				this.Show();
				return;
			}
			Point loc=((Control)item.ContainerControl).PointToScreen(item.DisplayRectangle.Location);
			this.Show(new Rectangle(loc,item.DisplayRectangle.Size),balloonFocus);
		}

		/// <summary>
		/// Display balloon.
		/// </summary>
		/// <param name="balloonFocus">Indicates whether balloon receives input focus.</param>
		public void Show(bool balloonFocus)
		{
			Rectangle rEnd=new Rectangle(this.Location,this.Size);
			Rectangle rStart=GetAnimationRectangle();

			if(!balloonFocus)
			{
				if(this.TopMost)
				{
					this.TopMost=false;
					NativeFunctions.SetWindowPos(this.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
					this.TopMost=true;
				}
				else
					NativeFunctions.SetWindowPos(this.Handle,NativeFunctions.HWND_TOP,0,0,0,0,NativeFunctions.SWP_SHOWWINDOW | NativeFunctions.SWP_NOACTIVATE | NativeFunctions.SWP_NOMOVE | NativeFunctions.SWP_NOSIZE);
			}

			if(ShouldAnimate())
			{
				try
				{
					m_AnimationInProgress=true;
					BarFunctions.AnimateControl(this,true,m_AlertAnimationDuration,rStart,rEnd);
				}
				finally
				{
					m_AnimationInProgress=false;
				}
			}
			else
				base.Show();
		}

		/// <summary>
		/// Displays balloon.
		/// </summary>
		public new void Show()
		{
			this.Show(true);
		}
        /// <summary>
        /// Called when balloon is hidden.
        /// </summary>
        protected virtual void HideBalloon()
        {
            DestroyAutoCloseTimer();
            if (ShouldAnimate())
            {
                Rectangle rStart = new Rectangle(this.Location, this.Size);
                Rectangle rEnd = GetAnimationRectangle();
                try
                {
                    m_AnimationInProgress = true;
                    BarFunctions.AnimateControl(this, false, m_AlertAnimationDuration, rStart, rEnd);
                }
                finally
                {
                    m_AnimationInProgress = false;
                }
            }
            else
                base.Hide();
        }

        /// <summary>
		/// Hides balloon.
		/// </summary>
		public new void Hide()
		{
            HideBalloon();
		}

		private bool ShouldAnimate()
		{
			if(m_Style==eBallonStyle.Alert && m_AlertAnimation!=eAlertAnimation.None && !this.DesignMode)
				return true;
			return false;
		}

		private Rectangle GetAnimationRectangle()
		{
			Rectangle r=new Rectangle(this.Location,this.Size);
			if(m_AlertAnimation==eAlertAnimation.BottomToTop)// && bShowing || m_AlertAnimation==eAlertAnimation.TopToBottom && !bShowing)
			{
				r.Y=r.Bottom-1;
				r.Height=1;
			}
			else if(m_AlertAnimation==eAlertAnimation.TopToBottom)
			{
				r.Height=1;
			}
			else if(m_AlertAnimation==eAlertAnimation.LeftToRight)
			{
				r.Width=2;
			}
			else if(m_AlertAnimation==eAlertAnimation.RightToLeft)
			{
				r.X=r.Right-1;
				r.Width=1;
			}
			return r;
		}

		/// <summary>
		/// Gets/Sets whether Balloon is visible.
		/// </summary>
		[DevCoBrowsable(true)]
		public new bool Visible
		{
			get { return base.Visible;}
			set
			{
				if(value)
					this.Show();
				else
					this.Hide();
			}
		}

		const int WM_MOUSEACTIVATE = 0x21;
		const int MA_NOACTIVATE = 3;
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_MOUSEACTIVATE)
			{
				m.Result=new System.IntPtr(MA_NOACTIVATE);
				return;
			}
			base.WndProc(ref m);
		}

		private class ImageButton
		{
			private Rectangle m_Bounds=Rectangle.Empty;
			private Control m_Parent=null;
			private bool m_MouseOver=false;
			private bool m_MouseDown=false;
			public Image Normal=null;
			public Image Hover=null;
			public Image Pressed=null;

			public event EventHandler Click;

			public ImageButton(Control parent, Rectangle bounds)
			{
				m_Parent=parent;
				m_Bounds=bounds;
			}

			public virtual void Paint(Graphics g)
			{
				if(m_MouseDown)
				{
					if(Pressed!=null)
						g.DrawImage(Pressed,m_Bounds);
				}
				else if(m_MouseOver)
				{
					if(Hover!=null)
						g.DrawImage(Hover,m_Bounds);
				}
				else
				{
					if(Normal!=null)
						g.DrawImage(Normal,m_Bounds);
				}
			}

			public virtual void OnMouseMove(MouseEventArgs e)
			{
				if(m_MouseDown)
					return;

				if(m_Bounds.Contains(e.X,e.Y))
				{
					if(!m_MouseOver)
					{
						m_MouseOver=true;
						if(m_Parent!=null)
							m_Parent.Invalidate(m_Bounds);
					}
				}
				else if(m_MouseOver)
				{
					m_MouseOver=false;
					if(m_Parent!=null)
						m_Parent.Invalidate(m_Bounds);
				}
			}
			public virtual void OnMouseDown(MouseEventArgs e)
			{
				if(m_Bounds.Contains(e.X,e.Y) && !m_MouseDown)
				{
					m_MouseDown=true;
					if(m_Parent!=null)
						m_Parent.Invalidate(m_Bounds);
				}
			}

			public virtual void OnMouseUp(MouseEventArgs e)
			{
				if(m_Bounds.Contains(e.X,e.Y) && m_MouseDown)
				{
					m_MouseDown=false;
					if(m_Parent!=null)
						m_Parent.Invalidate(m_Bounds);
					if(Click!=null)
						Click(this,new EventArgs());
				}
				else if(m_MouseOver || m_MouseDown)
				{
					m_MouseDown=false;
					m_MouseOver=false;
					if(m_Parent!=null)
						m_Parent.Invalidate(m_Bounds);
				}
			}

			public virtual void OnMouseLeave(EventArgs e)
			{
				if(m_MouseOver && !m_MouseDown)
				{
					m_MouseOver=false;
					if(m_Parent!=null)
						m_Parent.Invalidate(m_Bounds);
				}
			}

			public bool MouseDown
			{
				get {return m_MouseDown;}
			}

			public bool MouseOver
			{
				get {return m_MouseOver;}
			}

			public Rectangle Bounds
			{
				get {return m_Bounds;}
				set {m_Bounds=value;}
			}

			public Point Location
			{
				get {return m_Bounds.Location;}
				set {m_Bounds.Location=value;}
			}

			public System.Drawing.Size Size
			{
				get {return m_Bounds.Size;}
				set {m_Bounds.Size=value;}
			}
		}
	}

	public class CustomPaintEventArgs:EventArgs
	{
		private System.Drawing.Graphics m_Graphics=null;
		private Rectangle m_PaintRectangle=Rectangle.Empty;
		public CustomPaintEventArgs(Graphics g, Rectangle r)
		{
			m_Graphics=g;
			m_PaintRectangle=r;
		}

		public System.Drawing.Graphics Graphics
		{
			get {return m_Graphics;}
		}

		public Rectangle PaintRectangle
		{
			get {return m_PaintRectangle;}
		}
	}
}
