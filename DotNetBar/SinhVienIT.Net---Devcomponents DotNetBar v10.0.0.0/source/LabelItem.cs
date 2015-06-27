using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for LabelItem.
	/// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), DefaultEvent("Click"), Designer("DevComponents.DotNetBar.Design.SimpleBaseItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class LabelItem:BaseItem
    {
        #region Private Variables
        internal const eBorderSide DEFAULT_BORDERSIDE = eBorderSide.Top | eBorderSide.Bottom | eBorderSide.Left | eBorderSide.Right;
        private eBorderType m_BorderType=eBorderType.None;
		private Color m_BackColor=Color.Empty;
		private Color m_ForeColor=Color.Empty;
		private Color m_SingleLineColor=SystemColors.ControlDark;
		private bool m_SingleLineColorCustom=false;
		private System.Drawing.StringAlignment m_TextAlignment=System.Drawing.StringAlignment.Near;
		private System.Drawing.StringAlignment m_TextLineAlignment=System.Drawing.StringAlignment.Center;
		private System.Drawing.Font m_Font=null;
		private bool m_DividerStyle=false;

		private int m_PaddingLeft=0, m_PaddingRight=0, m_PaddingTop=0, m_PaddingBottom=0;
		private int m_Width=0, m_Height=0;
		private bool m_WordWrap=false;

		private eImagePosition m_ImagePosition;
		private System.Drawing.Image m_Image;
		private System.Drawing.Image m_ImageCachedIdx=null;
        private System.Drawing.Image m_DisabledImage = null;
        private System.Drawing.Icon m_DisabledIcon = null;
		System.Drawing.Icon m_Icon=null;
		private int m_ImageIndex=-1;
		private ItemPaintArgs _ItemPaintArgs=null;
		private const int IMAGETEXT_SPACING=2;
        private eBorderSide m_BorderSide = DEFAULT_BORDERSIDE;
        private bool m_ShowPrefix = false;
        internal bool SuspendPaint = false;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        #endregion

        #region Internal Implementation
        public LabelItem():this("","") {}
		public LabelItem(string sName):this(sName,""){}
		public LabelItem(string sName, string ItemText):base(sName,ItemText)
		{
			//this.CanCustomize=false;
			this.IsAccessible=false;
		}

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_Image);
                BarUtilities.DisposeImage(ref m_ImageCachedIdx);
                BarUtilities.DisposeImage(ref m_Icon);
            }
            base.Dispose(disposing);
        }

		public override BaseItem Copy()
		{
			LabelItem copy=new LabelItem(m_Name);
			this.CopyToItem(copy);

			return copy;
		}

        internal void InternalCopyToItem(LabelItem copy)
        {
            CopyToItem(copy);
        }

	    protected override void CopyToItem(BaseItem copy)
		{
			LabelItem objCopy=copy as LabelItem;
			base.CopyToItem(objCopy);

			objCopy.BorderType=m_BorderType;
            objCopy.BorderSide = m_BorderSide;
			objCopy.BackColor=m_BackColor;
			objCopy.ForeColor=m_ForeColor;
			if(m_SingleLineColorCustom)
				objCopy.SingleLineColor=m_SingleLineColor;
			objCopy.WidthInternal=this.WidthInternal;
			objCopy.HeightInternal=this.HeightInternal;
			objCopy.TextAlignment=this.TextAlignment;
			objCopy.TextLineAlignment=this.TextLineAlignment;
			objCopy.Font=m_Font;
			objCopy.PaddingBottom=m_PaddingBottom;
			objCopy.PaddingLeft=m_PaddingLeft;
			objCopy.PaddingRight=m_PaddingRight;
			objCopy.PaddingTop=m_PaddingTop;
            if (this.Image != null)
                objCopy.Image = (Image) this.Image.Clone();
            objCopy.ImageIndex = this.ImageIndex;
            objCopy.ImagePosition = this.ImagePosition;
            objCopy.WordWrap = this.WordWrap;
            objCopy.DividerStyle = this.DividerStyle;
            objCopy.EnableMarkup = this.EnableMarkup;
            objCopy.Width = this.Width;
            objCopy.Height = this.Height;
		}

        public override void Paint(ItemPaintArgs pa)
        {
            if (this.SuspendLayout)
                return;
            
            _ItemPaintArgs = pa;
            CompositeImage image = this.GetImage();
            System.Drawing.Graphics g = pa.Graphics;
            Rectangle thisRect = this.DisplayRectangle;
            Rectangle rClip = Rectangle.Empty;
            System.Windows.Forms.Border3DSide borderside;
            if (m_DividerStyle)
            {
                borderside = System.Windows.Forms.Border3DSide.Top;
            }
            else
            {
                if (m_BorderSide == eBorderSide.All)
                    borderside = (System.Windows.Forms.Border3DSide.Left |
                    System.Windows.Forms.Border3DSide.Right | System.Windows.Forms.Border3DSide.Top |
                    System.Windows.Forms.Border3DSide.Bottom);
                else
                    borderside = (((m_BorderSide & eBorderSide.Left) != 0) ? System.Windows.Forms.Border3DSide.Left : 0) |
                    (((m_BorderSide & eBorderSide.Right) != 0) ? System.Windows.Forms.Border3DSide.Right : 0) |
                    (((m_BorderSide & eBorderSide.Top) != 0) ? System.Windows.Forms.Border3DSide.Top : 0) |
                    (((m_BorderSide & eBorderSide.Bottom) != 0) ? System.Windows.Forms.Border3DSide.Bottom : 0);
            }

            DisplayHelp.FillRectangle(g, this.DisplayRectangle, m_BackColor, Color.Empty);

            if (m_DividerStyle)
            {
                System.Windows.Forms.Control ctrl = this.ContainerControl as System.Windows.Forms.Control;
                System.Windows.Forms.RightToLeft rtl = System.Windows.Forms.RightToLeft.No;
                if (ctrl != null && ctrl.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                    rtl = System.Windows.Forms.RightToLeft.Yes;

                Size sz = TextDrawing.MeasureString(g, m_Text, this.GetFont(), m_Rect.Width, this.GetStringFormat());
                sz.Width += 4;
                if (sz.Width > thisRect.Width)
                    sz.Width = thisRect.Width;
                rClip = new Rectangle(thisRect.X, thisRect.Y, (int)sz.Width, (int)sz.Height);
                if (m_TextAlignment == StringAlignment.Center)
                    rClip.Offset((thisRect.Width - (int)sz.Width) / 2, 0);
                else if (m_TextAlignment == StringAlignment.Far && rtl == System.Windows.Forms.RightToLeft.No || m_TextAlignment == StringAlignment.Near && rtl == System.Windows.Forms.RightToLeft.Yes)
                    rClip.Offset(thisRect.Width - rClip.Width, 0);
                if (m_TextLineAlignment == StringAlignment.Center)
                    rClip.Offset(0, (thisRect.Height - rClip.Height) / 2);
                else if (m_TextLineAlignment == StringAlignment.Far && rtl == System.Windows.Forms.RightToLeft.No || m_TextLineAlignment == StringAlignment.Near && rtl == System.Windows.Forms.RightToLeft.Yes)
                    rClip.Offset(0, thisRect.Height - rClip.Height);
            }
            
            switch (m_BorderType)
            {
                case eBorderType.Bump:
                    if (m_DividerStyle)
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, System.Windows.Forms.Border3DStyle.Bump, borderside);
                    }
                    else
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.Bump, borderside);
                        thisRect.Inflate(-2, -2);
                    }
                    break;
                case eBorderType.Etched:
                    if (m_DividerStyle)
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, System.Windows.Forms.Border3DStyle.Etched, borderside);
                    }
                    else
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.Etched, borderside);
                        thisRect.Inflate(-2, -2);
                    }
                    break;
                case eBorderType.Raised:
                case eBorderType.RaisedInner:
                    if (m_DividerStyle)
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, System.Windows.Forms.Border3DStyle.RaisedInner, borderside);
                    }
                    else
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.RaisedInner, borderside);
                        thisRect.Inflate(-2, -2);
                    }
                    break;
                case eBorderType.Sunken:
                    if (m_DividerStyle)
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Width, 2, System.Windows.Forms.Border3DStyle.SunkenOuter, borderside);
                    }
                    else
                    {
                        System.Windows.Forms.ControlPaint.DrawBorder3D(g, m_Rect, System.Windows.Forms.Border3DStyle.SunkenOuter, borderside);
                        thisRect.Inflate(-2, -2);
                    }
                    break;
                case eBorderType.SingleLine:
                    if (m_DividerStyle)
                    {
                        g.DrawLine(new Pen(GetSingleLineColor(), 1), m_Rect.X, m_Rect.Y + m_Rect.Height / 2, m_Rect.Right, m_Rect.Y + m_Rect.Height / 2);
                    }
                    else
                    {
                        BarFunctions.DrawBorder(g, eBorderType.SingleLine, m_Rect, GetSingleLineColor(), m_BorderSide, DashStyle.Solid, 1);
                        thisRect.Inflate(-1, -1);
                    }
                    break;
                case eBorderType.DoubleLine:
                    {
                        BarFunctions.DrawBorder(g, eBorderType.DoubleLine, m_Rect, GetSingleLineColor());
                        thisRect.Inflate(-2, -2);
                        break;
                    }
                default:
                    break;
            }

            if (m_DividerStyle)
                g.FillRectangle(new SolidBrush(m_BackColor), rClip);

            eOrientation orientation = this.Orientation;
            if (_TextOrientation == eOrientation.Vertical) orientation = _TextOrientation;

            if (orientation == eOrientation.Horizontal)
                thisRect.X += m_PaddingLeft;
            else
                thisRect.Y += m_PaddingLeft;

            if (orientation == eOrientation.Horizontal)
                thisRect.Width -= (m_PaddingLeft + m_PaddingRight);
            else
                thisRect.Width -= (m_PaddingTop + m_PaddingBottom);
            thisRect.Y += m_PaddingTop;
            if (orientation == eOrientation.Horizontal)
                thisRect.Height -= (m_PaddingTop + m_PaddingBottom);
            else
                thisRect.Height -= (m_PaddingLeft + m_PaddingRight);

            if (image != null)
            {
                switch (m_ImagePosition)
                {
                    case eImagePosition.Left:
                        {
                            if (orientation == eOrientation.Horizontal)
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X + IMAGETEXT_SPACING, (thisRect.Y + (thisRect.Height - image.Height) / 2), image.Width, image.Height));
                                thisRect.X += (image.Width + IMAGETEXT_SPACING * 2);
                                thisRect.Width -= (image.Width + IMAGETEXT_SPACING * 2);
                            }
                            else
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X + (thisRect.Width - image.Height) / 2, thisRect.Y + IMAGETEXT_SPACING, image.Width, image.Height));
                                thisRect.Y += (image.Height + IMAGETEXT_SPACING * 2);
                                thisRect.Height -= (image.Height + IMAGETEXT_SPACING * 2);
                            }
                            break;
                        }
                    case eImagePosition.Right:
                        {
                            if (orientation == eOrientation.Horizontal)
                            {
                                image.DrawImage(g, new Rectangle(thisRect.Right - image.Width - IMAGETEXT_SPACING, (thisRect.Y + (thisRect.Height - image.Height) / 2), image.Width, image.Height));
                                thisRect.Width -= (image.Width + IMAGETEXT_SPACING * 2);
                            }
                            else
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X, thisRect.Bottom - image.Height - IMAGETEXT_SPACING, image.Width, image.Height));
                                thisRect.Height -= (image.Height + IMAGETEXT_SPACING * 2);
                            }
                            break;
                        }
                    case eImagePosition.Top:
                        {
                            if (orientation == eOrientation.Horizontal)
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X + (thisRect.Width - image.Width) / 2, thisRect.Y, image.Width, image.Height));
                                thisRect.Y += (image.Height + IMAGETEXT_SPACING);
                                thisRect.Height -= (image.Height + IMAGETEXT_SPACING);
                            }
                            else
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X, thisRect.Y + (thisRect.Height - image.Height) / 2, image.Width, image.Height));
                                thisRect.X += (image.Width + IMAGETEXT_SPACING);
                                thisRect.Width -= (image.Width + IMAGETEXT_SPACING);
                            }
                            break;
                        }
                    case eImagePosition.Bottom:
                        {
                            if (orientation == eOrientation.Horizontal)
                            {
                                image.DrawImage(g, new Rectangle(thisRect.X + (thisRect.Width - image.Width) / 2, thisRect.Bottom - image.Height, image.Width, image.Height));
                                thisRect.Height -= (image.Height + IMAGETEXT_SPACING);
                            }
                            else
                            {
                                image.DrawImage(g, new Rectangle(thisRect.Right - image.Width, thisRect.Y + (thisRect.Height - image.Height) / 2, image.Width, image.Height));
                                thisRect.Width -= (image.Width + IMAGETEXT_SPACING);
                            }
                            break;
                        }
                }
            }

            Color textColor = GetTextColor(pa);

            if (thisRect.Height > 0 && thisRect.Width > 0)
            {
                if (orientation == eOrientation.Horizontal)
                {
                    if (m_TextAlignment == StringAlignment.Far) thisRect.Width--;
                    if (this.TextMarkupBody != null)
                    {
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.GetFont(), textColor, pa.RightToLeft);
                        d.HotKeyPrefixVisible = !((GetStringFormat() & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                        if (m_TextLineAlignment == StringAlignment.Near)
                            this.TextMarkupBody.Bounds = new Rectangle(thisRect.Location, this.TextMarkupBody.Bounds.Size);
                        else if (m_TextLineAlignment == StringAlignment.Center)
                            this.TextMarkupBody.Bounds = new Rectangle(new Point(thisRect.X, thisRect.Y + (thisRect.Height - this.TextMarkupBody.Bounds.Height) / 2), this.TextMarkupBody.Bounds.Size);
                        else if (m_TextLineAlignment == StringAlignment.Far)
                            this.TextMarkupBody.Bounds = new Rectangle(new Point(thisRect.X, thisRect.Bottom - this.TextMarkupBody.Bounds.Height), this.TextMarkupBody.Bounds.Size);
                        Region oldClip = g.Clip;
                        Rectangle clipRect = thisRect;
                        clipRect.Inflate(2, 2);
                        g.SetClip(clipRect, CombineMode.Replace);
                        this.TextMarkupBody.Render(d);
                        if (oldClip != null)
                            g.Clip = oldClip;
                        else
                            g.ResetClip();
                    }
                    else
                    {
                        eTextFormat format = GetStringFormat();
                        if (pa.RightToLeft) format |= eTextFormat.RightToLeft;
#if FRAMEWORK20
                        if (pa.GlassEnabled && this.Parent is CaptionItemContainer && !(this.ContainerControl is Ribbon.QatToolbar))
                        {
                            if (!pa.CachedPaint)
                                Office2007RibbonControlPainter.PaintTextOnGlass(g, m_Text, this.GetFont(), thisRect, TextDrawing.GetTextFormat(format));
                        }
                        else
#endif
                            TextDrawing.DrawString(g, m_Text, this.GetFont(), textColor, thisRect, format);
                    }
                }
                else
                {
                    if(_VerticalTextTopUp)
                        g.RotateTransform(90);
                    else
                        g.RotateTransform(-90);
                    if (this.TextMarkupBody != null)
                    {
                        this.TextMarkupBody.Bounds = (_VerticalTextTopUp ? new Rectangle(thisRect.Top, -thisRect.Right, this.TextMarkupBody.Bounds.Width + 2, this.TextMarkupBody.Bounds.Height) :
                            new Rectangle(-thisRect.Bottom, thisRect.X, thisRect.Height + 1, thisRect.Width + 1));
                        TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, this.GetFont(), textColor, pa.RightToLeft);
                        d.HotKeyPrefixVisible = !((GetStringFormat() & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                        this.TextMarkupBody.Render(d);
                    }
                    else
                        TextDrawing.DrawStringLegacy(g, m_Text, this.GetFont(), textColor, (_VerticalTextTopUp ? new Rectangle(thisRect.Top, -thisRect.Right, thisRect.Height + 1, thisRect.Width + 1):
                            new Rectangle(-thisRect.Bottom, thisRect.X, thisRect.Height + 1, thisRect.Width + 1)), GetStringFormat());
                    g.ResetTransform();
                }
            }

            //if(this.DesignMode)
            this.DrawInsertMarker(g);

            if (this.DesignMode && this.Focused)
            {
                Rectangle r = m_Rect;
                r.Width--;
                r.Height--;
                //r.Inflate(-1,-1);
                DesignTime.DrawDesignTimeSelection(g, r, pa.Colors.ItemDesignTimeBorder);
            }

            _ItemPaintArgs = null;
        }

        private Color GetTextColor(ItemPaintArgs pa)
        {
            return GetTextColor(pa, this.EffectiveStyle, this.Enabled && GetEnabled(pa.ContainerControl), m_ForeColor);
        }

        internal static Color GetTextColor(ItemPaintArgs pa, eDotNetBarStyle effectiveStyle, bool isEnabled, Color defaultColor)
        {
            if (BarFunctions.IsOffice2007Style(effectiveStyle) && defaultColor.IsEmpty && pa.Renderer is Rendering.Office2007Renderer)
            {
                if ((pa.IsOnMenu || pa.IsOnPopupBar) && ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors.Count>0)
                {
                    return isEnabled ? ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText;
                }
                else
                {
                    if((pa.ContainerControl is RibbonStrip || pa.ContainerControl is Bar)&& ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors.Count>0)
                        return ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors[0].Default.Text;
                    Rendering.Office2007ButtonItemColorTable ct = ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
                    if (ct != null && !ct.Default.Text.IsEmpty)
                        return isEnabled ? ct.Default.Text : pa.Colors.ItemDisabledText;
                }
            }
            else if (!isEnabled)
            {
            	return pa.Colors.ItemDisabledText;
            }

            if (defaultColor.IsEmpty) return pa.Colors.ItemText;

            return defaultColor;
        }

        private Graphics CreateGraphics()
        {
            System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
            if (!IsHandleValid(objCtrl))
                return null;

            return BarFunctions.CreateGraphics(objCtrl);
        }

        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;

            if (this.TextMarkupBody != null)
            {
                RecalcSizeMarkup();
            }
            else
            {
                CompositeImage image = this.GetImage();

                eOrientation orientation = this.Orientation;
                if (_TextOrientation == eOrientation.Vertical) orientation = _TextOrientation;

                if (orientation == eOrientation.Horizontal || m_Width == 0)
                {
                    if (m_Width == 0)
                    {
                        // Auto-size label
                        Graphics g = this.CreateGraphics();
                        if (g == null) return;
                        try
                        {
                            // Measure string
                            Font objCurrentFont = null;
                            if (m_Font != null)
                                objCurrentFont = m_Font;
                            else
                                objCurrentFont = GetFont();

                            Size objStringSize = Size.Empty;
                            eTextFormat objStringFormat = GetStringFormat();

                            if (m_Text != "")
                            {
                                string text = m_Text;
                                if (text.EndsWith(" "))
                                    text += ".";
                                int stringAreaWidth = 0;
                                if (m_WordWrap && this.Stretch && this.WidthInternal > 16)
                                    stringAreaWidth = this.WidthInternal;
                                if (orientation == eOrientation.Vertical && !this.IsOnMenu)
                                    objStringSize = TextDrawing.MeasureStringLegacy(g, text, objCurrentFont, new Size(stringAreaWidth, 0), objStringFormat);
                                else
                                    objStringSize = TextDrawing.MeasureString(g, text, objCurrentFont, stringAreaWidth, objStringFormat);
                                objStringSize.Width += 2;
                            }
                            else
                            {
                                if (orientation == eOrientation.Vertical && !this.IsOnMenu)
                                    objStringSize = TextDrawing.MeasureStringLegacy(g, " ", objCurrentFont, Size.Empty, objStringFormat);
                                else
                                    objStringSize = TextDrawing.MeasureString(g, " ", objCurrentFont, 0, objStringFormat);
                                objStringSize.Width += 2;
                            }
                            if (this.Parent is CaptionItemContainer) objStringSize.Width += 2;
                            if (m_BorderType != eBorderType.None)
                            {
                                objStringSize.Width += 4;
                                objStringSize.Height += 2;
                                if (m_BorderType != eBorderType.SingleLine)
                                    objStringSize.Height += 2;

                            }

                            if (orientation == eOrientation.Horizontal)
                            {
                                if (this.Stretch && !(this.Parent is ItemContainer))
                                    this.WidthInternal = 8;
                                else
                                    this.WidthInternal = objStringSize.Width + m_PaddingLeft + m_PaddingRight;
                                this.HeightInternal = objStringSize.Height + m_PaddingTop + m_PaddingBottom; // +1;
                                if (image != null)
                                {
                                    if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
                                    {
                                        this.WidthInternal += image.Width + IMAGETEXT_SPACING * 2;
                                        this.HeightInternal = Math.Max(this.HeightInternal, image.Height + IMAGETEXT_SPACING);
                                    }
                                    else
                                    {
                                        this.HeightInternal += image.Height + IMAGETEXT_SPACING * 2;
                                    }
                                }
                            }
                            else
                            {
                                if (this.Stretch && !(this.Parent is ItemContainer))
                                    this.HeightInternal = 8;
                                else
                                    this.HeightInternal = objStringSize.Width + m_PaddingLeft + m_PaddingRight;
                                this.WidthInternal = objStringSize.Height + m_PaddingTop + m_PaddingBottom;
                                if (image != null)
                                {
                                    if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
                                    {
                                        this.HeightInternal += image.Width + IMAGETEXT_SPACING * 2;
                                        this.WidthInternal = Math.Max(this.WidthInternal, image.Width + IMAGETEXT_SPACING);
                                    }
                                    else
                                    {
                                        this.WidthInternal += image.Height + IMAGETEXT_SPACING * 2;
                                        this.HeightInternal = Math.Max(image.Height + IMAGETEXT_SPACING, this.HeightInternal);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                            g.SmoothingMode = SmoothingMode.Default;
                            g.Dispose();
                        }
                    }
                    else
                    {
                        this.WidthInternal = m_Width;
                        if (m_Height == 0)
                        {
                            // Measure string
                            System.Drawing.Font objCurrentFont = null;
                            if (m_Font != null)
                                objCurrentFont = m_Font;
                            else
                                objCurrentFont = GetFont();
                            if (this.WordWrap)
                            {
                                Graphics g = this.CreateGraphics();
                                if (g != null)
                                {
                                    int availableWidth = this.WidthInternal - (m_PaddingLeft + m_PaddingRight);
                                    if (image != null && orientation == eOrientation.Horizontal && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
                                        availableWidth -= image.Width + 4;
                                    Size textSize = TextDrawing.MeasureString(g, this.Text, objCurrentFont, availableWidth);
                                    this.HeightInternal = textSize.Height + 2;
                                }
                                else
                                    this.HeightInternal = (int)Math.Ceiling(objCurrentFont.GetHeight());
                            }
                            else
                                this.HeightInternal = (int)Math.Ceiling(objCurrentFont.GetHeight());
                            this.HeightInternal += m_PaddingTop + m_PaddingBottom + (this.IsRightToLeft ? 2 : 0);

                            if (m_BorderType != eBorderType.None)
                            {
                                this.HeightInternal += 2;
                                if (m_BorderType != eBorderType.SingleLine)
                                    this.HeightInternal += 2;
                            }

                            if (image != null)
                            {
                                if (orientation == eOrientation.Horizontal)
                                {
                                    if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
                                        this.HeightInternal = Math.Max(this.HeightInternal, image.Height/*+2*/);
                                    else
                                        this.HeightInternal += image.Height + 4;
                                }
                                else
                                {
                                    if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
                                        this.WidthInternal = Math.Max(this.HeightInternal, image.Height + 2);
                                    else
                                        this.WidthInternal += image.Height + 4;
                                }
                            }
                        }
                        else
                            this.HeightInternal = m_Height;
                    }
                }
                else
                {
                    this.WidthInternal = m_Height;
                    this.HeightInternal = m_Width;
                }
            }
            
            base.RecalcSize();
        }

        #region Markup Implementation
        protected override void OnExternalSizeChange()
        {
            if (this.TextMarkupBody != null)
            {
                if(_TextOrientation == eOrientation.Horizontal)
                    ArrangeMarkup(this.WidthInternal);
                else
                    ArrangeMarkup(this.HeightInternal + 1);
            }
            base.OnExternalSizeChange();
        }

        private void ArrangeMarkup(int specifiedWidth)
        {
            System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
            if (!IsHandleValid(objCtrl))
                return;
            using (Graphics g = BarFunctions.CreateGraphics(objCtrl))
            {
                Font objCurrentFont = null;
                if (m_Font != null)
                    objCurrentFont = m_Font;
                else
                    objCurrentFont = GetFont();

                CompositeImage image = this.GetImage();

                Size availableSize = GetMarkupAvailableSize(specifiedWidth);

                TextMarkup.MarkupDrawContext d = new DevComponents.DotNetBar.TextMarkup.MarkupDrawContext(g, objCurrentFont, this.ForeColor, false);
                d.HotKeyPrefixVisible = !((GetStringFormat() & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                TextMarkupBody.Measure(availableSize, d);
                d.RightToLeft = (objCtrl.RightToLeft == System.Windows.Forms.RightToLeft.Yes);

                if (specifiedWidth == 0 && m_Width == 0)
                    availableSize = TextMarkupBody.Bounds.Size;

                TextMarkupBody.Arrange(new Rectangle(Point.Empty, availableSize), d);
            }
        }

        private Size GetMarkupAvailableSize(int specifiedWidth)
        {
            Size availableSize = new Size((specifiedWidth==0?m_Width:specifiedWidth), 1);
            if(_TextOrientation == eOrientation.Vertical)
                availableSize = new Size((specifiedWidth == 0 ? m_Height: specifiedWidth), 1);

            CompositeImage image = this.GetImage();

            if (availableSize.Width == 0)
                availableSize.Width = 1600;
            else
            {
                availableSize.Width -= (m_PaddingLeft + m_PaddingRight);
                if (image != null && (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right))
                    availableSize.Width -= (image.Width + IMAGETEXT_SPACING * 2);
                if (m_BorderType != eBorderType.None)
                    availableSize.Width -= 4;
            }
            return availableSize;
        }

        private void RecalcSizeMarkup()
        {
            RecalcSizeMarkup(0);
        }

        internal void RecalcSizeMarkup(int proposedWidth)
        {
            eOrientation orientation = this.Orientation;
            if (_TextOrientation == eOrientation.Vertical) orientation = _TextOrientation;

            ArrangeMarkup(proposedWidth);

            int width = TextMarkupBody.Bounds.Width;
            int height = TextMarkupBody.Bounds.Height;

            if (m_BorderType != eBorderType.None)
            {
                width += 4;
                height += 4;
            }

            CompositeImage image = this.GetImage();

            if (image != null)
            {
                if (m_Width == 0)
                {
                    if (m_ImagePosition == eImagePosition.Left || m_ImagePosition == eImagePosition.Right)
                    {
                        width += (image.Width + IMAGETEXT_SPACING * 2);
                        height = Math.Max(height, image.Height + IMAGETEXT_SPACING);
                    }
                    else
                    {
                        height += (image.Height + IMAGETEXT_SPACING * 2);
                        width = Math.Max(width, image.Width + IMAGETEXT_SPACING);
                    }
                }
            }

            if(m_Width==0)
            {
                width+=(m_PaddingLeft + m_PaddingRight);
                height+=(m_PaddingTop + m_PaddingBottom);

                if (this.Stretch && !(this.Parent is ItemContainer))
                    width = 8;
            }

            if (orientation == eOrientation.Vertical)
            {
                width += 2;
                int old = width;
                width = height;
                height = old;
            }

            if (m_Width == 0)
            {
                m_Rect.Width = width;
            }
            else
                m_Rect.Width = m_Width;
            if (m_Height == 0)
                m_Rect.Height = height;
            else
                m_Rect.Height = m_Height;
        }

        /// <summary>
        /// Gets whether item supports text markup. Default is false.
        /// </summary>
        protected override bool IsMarkupSupported
        {
            get { return _EnableMarkup; }
        }

        private bool _EnableMarkup = true;
        /// <summary>
        /// Gets or sets whether text-markup support is enabled for items Text property. Default value is true.
        /// Set this property to false to display HTML or other markup in the item instead of it being parsed as text-markup.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates whether text-markup support is enabled for items Text property.")]
        public bool EnableMarkup
        {
            get { return _EnableMarkup; }
            set
            {
                if (_EnableMarkup != value)
                {
                    _EnableMarkup = value;
                    NeedRecalcSize = true;
                    OnTextChanged();
                }
            }
        }
        #endregion

		/// <summary>
		/// Gets or sets the type of the border drawn around the label.
		/// </summary>
		[System.ComponentModel.Browsable(true),DefaultValue(eBorderType.None),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates the type of the border drawn around the label.")]
		public eBorderType BorderType
		{
			get
			{
				return m_BorderType;
			}
			set
			{
				if(m_BorderType==value)
					return;
				m_BorderType=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

        /// <summary>
        /// Gets or sets the border sides that are displayed. Default value specifies border on all 4 sides.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Appearance"), DefaultValue(DEFAULT_BORDERSIDE), Description("Specifies border sides that are displayed.")]
        public eBorderSide BorderSide
        {
            get { return m_BorderSide; }
            set
            {
                if (m_BorderSide != value)
                {
                    m_BorderSide = value;
                    this.Refresh();
                }
            }
        }

		/// <summary>
		/// Gets or sets the background color of the label.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Determines the background color of the label.")]
		public Color BackColor
		{
			get
			{
				return m_BackColor;
			}
			set
			{
				if(m_BackColor==value)
					return;
				m_BackColor=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return !m_BackColor.IsEmpty;
        }

		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates text color.")]
		public Color ForeColor
		{
			get
			{
				return m_ForeColor;
			}
			set
			{
				if(m_ForeColor==value)
					return;
				m_ForeColor=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}
        /// <summary>
        /// Reset property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetForeColor()
        {
            this.ForeColor = Color.Empty;
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeForeColor()
        {
            return !m_ForeColor.IsEmpty;
        }

		/// <summary>
		/// Gets or sets the border line color when border is single line.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates border line color when border is single line.")]
		public Color SingleLineColor
		{
			get
			{
				return m_SingleLineColor;
			}
			set
			{
				if(m_SingleLineColor==value)
					return;
				m_SingleLineColor=value;
                if (m_SingleLineColor != SystemColors.ControlDark)
                    m_SingleLineColorCustom = true;
                else
                    m_SingleLineColorCustom = false;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSingleLineColor()
        {
            return (m_SingleLineColor != SystemColors.ControlDark);
        }

        /// <summary>
        /// Resets the SingleLineColor property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetSingleLineColor()
        {
            SingleLineColor = SystemColors.ControlDark;
        }

        private Color GetSingleLineColor()
        {
            if (m_SingleLineColorCustom)
                return m_SingleLineColor;
            Color c = m_SingleLineColor;

            eDotNetBarStyle effectiveStyle = EffectiveStyle;
            if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle))
            {
                if (BarFunctions.IsOffice2007Style(effectiveStyle))
                {
                    if (this.ContainerControl is IRenderingSupport)
                    {
                        Office2007Renderer renderer = ((IRenderingSupport)this.ContainerControl).GetRenderer() as Office2007Renderer;
                        if (renderer == null)
                            renderer = GlobalManager.Renderer as Office2007Renderer;
                        if (renderer != null)
                            c = renderer.ColorTable.Bar.ToolbarBottomBorder;
                    }
                }
                else
                {
                    ColorScheme scheme = null;
                    if (this.ContainerControl is Bar)
                        scheme = ((Bar)this.ContainerControl).ColorScheme;
                    else if (this.ContainerControl is MenuPanel)
                        scheme = ((MenuPanel)this.ContainerControl).ColorScheme;
                    else
                        scheme = new ColorScheme(this.EffectiveStyle);
                    if (scheme != null)
                        m_SingleLineColor = scheme.BarDockedBorder;
                    else
                        m_SingleLineColor = SystemColors.ControlDark;
                }
            }

            return c;
        }

		/// <summary>
        /// Gets or sets the horizontal text alignment.
		/// </summary>
        [Browsable(true), DefaultValue(System.Drawing.StringAlignment.Near), DevCoBrowsable(true), Category("Layout"), Description("Indicates text alignment.")]
		public System.Drawing.StringAlignment TextAlignment
		{
			get
			{
				return m_TextAlignment;
			}
			set
			{
				if(m_TextAlignment==value)
					return;
				m_TextAlignment=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the text vertical alignment.
		/// </summary>
		[Browsable(true),DefaultValue(System.Drawing.StringAlignment.Center),DevCoBrowsable(true),Category("Layout"),Description("Indicates text line alignment.")]
		public System.Drawing.StringAlignment TextLineAlignment
		{
			get
			{
				return m_TextLineAlignment;
			}
			set
			{
				if(m_TextLineAlignment==value)
					return;
				m_TextLineAlignment=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the label font.
		/// </summary>
		[Browsable(true),DefaultValue(null), DevCoBrowsable(true),Category("Appearance"),Description("Indicates label font.")]
		public System.Drawing.Font Font
		{
			get
			{
				return m_Font;
			}
			set
			{
				if(m_Font!=value)
				{
					m_Font=value;
					this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the left padding in pixels.
		/// </summary>
        [System.ComponentModel.Browsable(true), DefaultValue(0), DevCoBrowsable(true), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates left padding in pixels.")]
		public int PaddingLeft
		{
			get
			{
				return m_PaddingLeft;
			}
			set
			{
				if(m_PaddingLeft==value)
					return;
				m_PaddingLeft=value;
				NeedRecalcSize=true;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the right padding in pixels.
		/// </summary>
        [System.ComponentModel.Browsable(true), DefaultValue(0), DevCoBrowsable(true), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates right padding in pixels.")]
		public int PaddingRight
		{
			get
			{
				return m_PaddingRight;
			}
			set
			{
				if(m_PaddingRight==value)
					return;
				m_PaddingRight=value;
				NeedRecalcSize=true;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the top padding in pixels.
		/// </summary>
        [System.ComponentModel.Browsable(true), DefaultValue(0), DevCoBrowsable(true), System.ComponentModel.Category("Layout"), System.ComponentModel.Description("Indicates top padding in pixels.")]
		public int PaddingTop
		{
			get
			{
				return m_PaddingTop;
			}
			set
			{
				if(m_PaddingTop==value)
					return;
				m_PaddingTop=value;
				NeedRecalcSize=true;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the bottom padding in pixels.
		/// </summary>
		[Browsable(true),DefaultValue(0),DevCoBrowsable(true),Category("Layout"),Description("Indicates bottom padding in pixels.")]
		public int PaddingBottom
		{
			get
			{
				return m_PaddingBottom;
			}
			set
			{
				if(m_PaddingBottom==value)
					return;
				m_PaddingBottom=value;
				NeedRecalcSize=true;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets the width of the label in pixels.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(0),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates the width of the label in pixels.")]
		public int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				if(m_Width==value)
					return;
				m_Width=value;
				NeedRecalcSize=true;
                if (!SuspendPaint)
                {
                    this.Refresh();
                    OnAppearanceChanged();
                }
			}
		}

		/// <summary>
		/// Gets or sets the height of the label.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.DefaultValue(0),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates height of the label in pixels.")]
		public int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				if(m_Height==value)
					return;
				m_Height=value;
				NeedRecalcSize=true;
                if (!SuspendPaint)
                {
                    this.Refresh();
                    OnAppearanceChanged();
                }
			}
		}

		/// <summary>
		/// Gets or sets whether the label is drawn as a divider label.
		/// </summary>
		[Browsable(true),DefaultValue(false),DevCoBrowsable(true),Category("Appearance"),Description("Indicates whether label has divider style.")]
		public bool DividerStyle
		{
			get
			{
				return m_DividerStyle;
			}
			set
			{
				if(m_DividerStyle==value)
					return;
				m_DividerStyle=value;
				this.Refresh();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets or sets a value that determines whether text is displayed in multiple lines or one long line.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Style"),System.ComponentModel.DefaultValue(false),System.ComponentModel.Description("Gets or sets a value that determines whether text is displayed in multiple lines or one long line.")]
		public bool WordWrap
		{
			get {return m_WordWrap;}
			set
			{
                if (m_WordWrap != value)
                {
                    m_WordWrap = value;
                    m_NeedRecalcSize = true;
                }
				OnAppearanceChanged();
			}
		}

		protected virtual Font GetFont()
		{
			// TODO: Check performance implications of cloning the font all the time instead of
			// getting the stored copy all the time
			if(m_Font!=null)
				return m_Font;
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl!=null)
				return (Font)objCtrl.Font;
			return (Font)System.Windows.Forms.SystemInformation.MenuFont;
		}

        internal bool ShowPrefix
        {
            get { return m_ShowPrefix; }
            set { m_ShowPrefix = value; }
        }

		private eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.Default | eTextFormat.ExpandTabs | eTextFormat.EndEllipsis;
            if (!m_ShowPrefix)
                format |= eTextFormat.NoPrefix;

            if (!m_WordWrap)
                format |= eTextFormat.SingleLine;
            else
                format |= eTextFormat.WordBreak;
            if (m_TextAlignment == StringAlignment.Center)
                format |= eTextFormat.HorizontalCenter;
            else if (m_TextAlignment == StringAlignment.Far)
                format |= eTextFormat.Right;
            if (m_TextLineAlignment == StringAlignment.Center)
                format |= eTextFormat.VerticalCenter;
            else if (m_TextLineAlignment == StringAlignment.Far)
                format |= eTextFormat.Bottom;
            if (this.IsRightToLeft)
                format |= eTextFormat.RightToLeft;

            return format;
		}

		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			ThisItem.SetAttribute("bt",System.Xml.XmlConvert.ToString(((int)m_BorderType)));
            if(m_BorderSide!=DEFAULT_BORDERSIDE)
                ThisItem.SetAttribute("BorderSide", System.Xml.XmlConvert.ToString(((int)m_BorderSide)));
			if(!m_BackColor.IsEmpty)
				ThisItem.SetAttribute("bc",BarFunctions.ColorToString(m_BackColor));
			ThisItem.SetAttribute("fc",BarFunctions.ColorToString(m_ForeColor));
			if(m_SingleLineColorCustom)
				ThisItem.SetAttribute("sc",BarFunctions.ColorToString(m_SingleLineColor));
			ThisItem.SetAttribute("ta",System.Xml.XmlConvert.ToString(((int)m_TextAlignment)));
			ThisItem.SetAttribute("tla",System.Xml.XmlConvert.ToString(((int)m_TextLineAlignment)));
			ThisItem.SetAttribute("ds",System.Xml.XmlConvert.ToString(m_DividerStyle));
			ThisItem.SetAttribute("pl",System.Xml.XmlConvert.ToString(m_PaddingLeft));
			ThisItem.SetAttribute("pr",System.Xml.XmlConvert.ToString(m_PaddingRight));
			ThisItem.SetAttribute("pt",System.Xml.XmlConvert.ToString(m_PaddingTop));
			ThisItem.SetAttribute("pb",System.Xml.XmlConvert.ToString(m_PaddingBottom));
			ThisItem.SetAttribute("w",System.Xml.XmlConvert.ToString(m_Width));
			ThisItem.SetAttribute("h",System.Xml.XmlConvert.ToString(m_Height));

			// Save Font information if needed
			if(this.Font!=null)
			{
				if(this.Font.Name!=System.Windows.Forms.SystemInformation.MenuFont.Name || this.Font.Size!=System.Windows.Forms.SystemInformation.MenuFont.Size || this.Font.Style!=System.Windows.Forms.SystemInformation.MenuFont.Style)
				{
					ThisItem.SetAttribute("fontname",this.Font.Name);
					ThisItem.SetAttribute("fontemsize",System.Xml.XmlConvert.ToString(this.Font.Size));
					ThisItem.SetAttribute("fontstyle",System.Xml.XmlConvert.ToString((int)this.Font.Style));
				}
			}

			System.Xml.XmlElement xmlElem=null, xmlElem2=null;
            
			// Serialize Images
			if(m_Image!=null || m_ImageIndex>=0 || m_Icon!=null)
			{
				xmlElem=ThisItem.OwnerDocument.CreateElement("images");
				ThisItem.AppendChild(xmlElem);

				if(m_ImageIndex>=0)
					xmlElem.SetAttribute("imageindex",System.Xml.XmlConvert.ToString(m_ImageIndex));

				if(m_Image!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","default");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeImage(m_Image,xmlElem2);
				}
				if(m_Icon!=null)
				{
					xmlElem2=ThisItem.OwnerDocument.CreateElement("image");
					xmlElem2.SetAttribute("type","icon");
					xmlElem.AppendChild(xmlElem2);
					BarFunctions.SerializeIcon(m_Icon,xmlElem2);
				}
			}
		}

		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);

            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;

			m_BorderType=(eBorderType)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("bt"));

            if (ItemXmlSource.HasAttribute("BorderSide"))
                m_BorderSide = (eBorderSide)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("BorderSide"));
            else
                m_BorderSide = DEFAULT_BORDERSIDE;

			if(ItemXmlSource.HasAttribute("bc"))
                m_BackColor=BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("bc"));
			else
				m_BackColor=Color.Empty;
			m_ForeColor=BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("fc"));

            if (ItemXmlSource.HasAttribute("sc"))
            {
                m_SingleLineColor = BarFunctions.ColorFromString(ItemXmlSource.GetAttribute("sc"));
                if (m_SingleLineColor != SystemColors.ControlDark)
                    m_SingleLineColorCustom = true;
            }
            else
            {
                m_SingleLineColorCustom = false;
                m_SingleLineColor = SystemColors.ControlDark;
            }

			m_TextAlignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("ta"));
			m_TextLineAlignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("tla"));

			m_DividerStyle=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("ds"));

            m_PaddingLeft=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("pl"));
			m_PaddingRight=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("pr"));
			m_PaddingTop=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("pt"));
			m_PaddingBottom=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("pb"));

			m_Width=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("w"));
			m_Height=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("h"));

			// Load font information if it exists
			if(ItemXmlSource.HasAttribute("fontname"))
			{
				string FontName=ItemXmlSource.GetAttribute("fontname");
				float FontSize=System.Xml.XmlConvert.ToSingle(ItemXmlSource.GetAttribute("fontemsize"));
				System.Drawing.FontStyle FontStyle=(System.Drawing.FontStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("fontstyle"));
				try
				{
					this.Font=new Font(FontName,FontSize,FontStyle);
				}
				catch(Exception)
				{
					this.Font=System.Windows.Forms.SystemInformation.MenuFont.Clone() as Font;
				}
			}

			// Load Images
			foreach(System.Xml.XmlElement xmlElem in ItemXmlSource.ChildNodes)
			{
				if(xmlElem.Name=="images")
				{
					if(xmlElem.HasAttribute("imageindex"))
						m_ImageIndex=System.Xml.XmlConvert.ToInt32(xmlElem.GetAttribute("imageindex"));

					foreach(System.Xml.XmlElement xmlElem2 in xmlElem.ChildNodes)
					{
						switch(xmlElem2.GetAttribute("type"))
						{
							case "default":
							{
								m_Image=BarFunctions.DeserializeImage(xmlElem2);
								m_ImageIndex=-1;
								break;
							}
							case "icon":
							{
								m_Icon=BarFunctions.DeserializeIcon(xmlElem2);
								m_ImageIndex=-1;
								break;
							}
						}
					}
					break;
				}
			}
		}

		/// <summary>
		/// Gets or sets the text associated with this item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf",typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The text contained in the item.")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if(m_Text==value)
					return;
				if(value==null)
					m_Text="";
				else
					m_Text=value;
				m_AccessKey=NativeFunctions.GetAccessKey(m_Text);

                if(ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "Text");

				//NeedRecalcSize=true;
				if(this.Displayed && m_Parent!=null && !this.SuspendLayout)
				{
					if(m_Width==0)
						NeedRecalcSize=true;
                    if(!(this.ContainerControl is LabelX))
					    this.Refresh(); 
					// No need for this Labels are always fixed in size
					//RecalcSize();
					//m_Parent.SubItemSizeChanged(this);
				}

				this.OnTextChanged();
				OnAppearanceChanged();
                if (this.ContainerControl is BaseItemControl)
                {
                    this.NeedRecalcSize = true;
                    this.Refresh();
                }
			}
		}

		/// <summary>
		/// Specifies the label icon. Icons support alpha blending.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies the label icon."),System.ComponentModel.DefaultValue(null)]
		public System.Drawing.Icon Icon
		{
			get
			{
				return m_Icon;
			}
			set
			{
				m_Icon=value;
				this.OnImageChanged();
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Specifies label image.
        /// </summary>
        [Browsable(true),DevCoBrowsable(true),Category("Appearance"),Description("The image that will be displayed on the face of the item."),DefaultValue(null)]
		public System.Drawing.Image Image
		{
			get
			{
				return m_Image;
			}
			set
			{
				m_Image=value;
				this.OnImageChanged();
				OnAppearanceChanged();
			}
		}

        // Property Editor support for ImageIndex selection
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                IOwner owner = this.GetOwner() as IOwner;
                if (owner != null)
                    return owner.Images;
                return null;
            }
        }
		/// <summary>
		/// Specifies the index of the image for the label if ImageList is used.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The image list image index of the image that will be displayed on the face of the item."),System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)),System.ComponentModel.TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter)),System.ComponentModel.DefaultValue(-1)]
		public int ImageIndex
		{
			get
			{
				return m_ImageIndex;
			}
			set
			{
				m_ImageCachedIdx=null;
				if(m_ImageIndex!=value)
				{
					m_ImageIndex=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImageIndex");
					OnImageChanged();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Gets/Sets the image position inside the label.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The alignment of the image in relation to text displayed by this item."),System.ComponentModel.DefaultValue(eImagePosition.Left)]
		public eImagePosition ImagePosition
		{
			get
			{
				return m_ImagePosition;
			}
			set
			{
				if(m_ImagePosition!=value)
				{
					m_ImagePosition=value;
                    if(ShouldSyncProperties)
                        BarFunctions.SyncProperty(this, "ImagePosition");
					NeedRecalcSize=true;
					this.Refresh();
				}
				OnAppearanceChanged();
			}
		}


		private void OnImageChanged()
		{
            if (m_DisabledImage != null)
            {
                m_DisabledImage.Dispose();
                m_DisabledImage = null;
            }
            if (m_DisabledIcon != null)
            {
                m_DisabledIcon.Dispose();
                m_DisabledIcon = null;
            }

			NeedRecalcSize=true;
			this.Refresh();
		}

        private CompositeImage GetImage()
        {
            if (this.GetEnabled())
                return GetImage(ImageState.Default);
            else
                return GetImage(ImageState.Disabled);
        }

		private CompositeImage GetImage(ImageState state)
		{
			Image image=null;

            if (state == ImageState.Disabled)
            {
                if (m_DisabledImage == null && m_DisabledIcon == null && (m_Icon != null || m_Image != null || m_ImageIndex >= 0))
                    CreateDisabledImage();

                if(m_DisabledIcon!=null)
                    return new CompositeImage(new System.Drawing.Icon(m_DisabledIcon, IconSize), true);
                if(m_DisabledImage!=null)
                    return new CompositeImage(m_DisabledImage, false);
            }

			if(m_Icon!=null)
			{
				System.Drawing.Size iconSize=this.IconSize;
				return new CompositeImage(new System.Drawing.Icon(m_Icon,iconSize),true);
			}

			if(m_Image!=null)
			{
				return new CompositeImage(m_Image,false);
			}

			if(m_ImageIndex>=0)
			{
				image=GetImageFromImageList(m_ImageIndex);
				if(image!=null)
					return new CompositeImage(image,false);
			}

			return null;
		}

        private void CreateDisabledImage()
        {
            if (m_Image == null && m_ImageIndex < 0 && m_Icon == null)
                return;
            if (m_DisabledImage != null)
                m_DisabledImage.Dispose();
            m_DisabledImage = null;
            if (m_DisabledIcon != null)
                m_DisabledIcon.Dispose();
            m_DisabledIcon = null;

            CompositeImage defaultImage = GetImage(ImageState.Default);

            if (defaultImage == null)
                return;

            if (this.GetOwner() is IOwner && ((IOwner)this.GetOwner()).DisabledImagesGrayScale)
            {
                if (defaultImage.IsIcon)
                {
                    m_DisabledIcon = BarFunctions.CreateDisabledIcon(defaultImage.Icon);
                }
                else
                {
                    m_DisabledImage = ImageHelper.CreateGrayScaleImage(defaultImage.Image as Bitmap);
                }
            }
            if (m_DisabledIcon != null || m_DisabledImage != null)
                return;

            // Use old algorithm if first one failed...
            System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            if (!defaultImage.IsIcon && defaultImage.Image != null)
                pixelFormat = defaultImage.Image.PixelFormat;

            if (pixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed || pixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed || pixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            Bitmap bmp = new Bitmap(defaultImage.Width, defaultImage.Height, pixelFormat);
            m_DisabledImage = new Bitmap(defaultImage.Width, defaultImage.Height, pixelFormat);

            Graphics g2 = Graphics.FromImage(bmp);
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.White))
                g2.FillRectangle(brush, 0, 0, defaultImage.Width, defaultImage.Height);
            //g2.DrawImage(defaultImage,0,0,defaultImage.Width,defaultImage.Height);
            defaultImage.DrawImage(g2, new Rectangle(0, 0, defaultImage.Width, defaultImage.Height));
            g2.Dispose();
            g2 = Graphics.FromImage(m_DisabledImage);

            bmp.MakeTransparent(System.Drawing.Color.White);
            eDotNetBarStyle effectiveStyle = EffectiveStyle;
            if ((effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)) && NativeFunctions.ColorDepth >= 8)
            {
                float[][] array = new float[5][];
                array[0] = new float[5] { 0, 0, 0, 0, 0 };
                array[1] = new float[5] { 0, 0, 0, 0, 0 };
                array[2] = new float[5] { 0, 0, 0, 0, 0 };
                array[3] = new float[5] { .5f, .5f, .5f, .5f, 0 };
                array[4] = new float[5] { 0, 0, 0, 0, 0 };
                System.Drawing.Imaging.ColorMatrix grayMatrix = new System.Drawing.Imaging.ColorMatrix(array);
                System.Drawing.Imaging.ImageAttributes disabledImageAttr = new System.Drawing.Imaging.ImageAttributes();
                disabledImageAttr.ClearColorKey();
                disabledImageAttr.SetColorMatrix(grayMatrix);
                g2.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, disabledImageAttr);
            }
            else
                System.Windows.Forms.ControlPaint.DrawImageDisabled(g2, bmp, 0, 0, ColorFunctions.MenuBackColor(g2));

            // Clean up
            g2.Dispose();
            g2 = null;
            bmp.Dispose();
            bmp = null;

            defaultImage.Dispose();
        }

		private Image GetImageFromImageList(int ImageIndex)
		{
			if(ImageIndex>=0)
			{
				IOwner owner=null;
				IBarImageSize iImageSize=null;
				if(_ItemPaintArgs!=null)
				{
					owner=_ItemPaintArgs.Owner;
					iImageSize=_ItemPaintArgs.ContainerControl as IBarImageSize;
				}
				if(owner==null) owner=this.GetOwner() as IOwner;
				if(iImageSize==null) iImageSize=this.ContainerControl as IBarImageSize; 

				if(owner!=null)
				{
					try
					{
						if(iImageSize!=null && iImageSize.ImageSize!=eBarImageSize.Default)
						{
							if(iImageSize.ImageSize==eBarImageSize.Medium && owner.ImagesMedium!=null)
								return owner.ImagesMedium.Images[ImageIndex];
							else if(iImageSize.ImageSize==eBarImageSize.Large && owner.ImagesLarge!=null)
								return owner.ImagesLarge.Images[ImageIndex];
							else if(owner.Images!=null)
							{
								if(ImageIndex==m_ImageIndex)
								{
									if(m_ImageCachedIdx==null)
										m_ImageCachedIdx=owner.Images.Images[ImageIndex];
									return m_ImageCachedIdx;
								}
								else
									return owner.Images.Images[ImageIndex];
							}
						}
						else if(m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize!=eBarImageSize.Default)
						{
							eBarImageSize imgSize=((SideBarPanelItem)m_Parent).ItemImageSize;
							if(imgSize==eBarImageSize.Medium && owner.ImagesMedium!=null)
								return owner.ImagesMedium.Images[ImageIndex];
							else if(imgSize==eBarImageSize.Large && owner.ImagesLarge!=null)
								return owner.ImagesLarge.Images[ImageIndex];
							else if(owner.Images!=null)
								return owner.Images.Images[ImageIndex];
						}
						else if(owner.Images!=null)
						{
							if(ImageIndex==m_ImageIndex)
							{
								if(m_ImageCachedIdx==null)
									m_ImageCachedIdx=owner.Images.Images[ImageIndex];

								return m_ImageCachedIdx;
							}
							else
								return owner.Images.Images[ImageIndex];
						}
					}
					catch(Exception)
					{
						return null;
					}
				}
			}
			return null;
		}

		private System.Drawing.Size IconSize
		{
			get
			{
				// Default Icon Size
				System.Drawing.Size size=new Size(16,16);
				IBarImageSize iImageSize=null;
				if(_ItemPaintArgs!=null)
				{
					iImageSize=_ItemPaintArgs.ContainerControl as IBarImageSize;
				}
				if(iImageSize==null) iImageSize=this.ContainerControl as IBarImageSize; 

				try
				{
					if(iImageSize!=null && iImageSize.ImageSize!=eBarImageSize.Default)
					{
						if(iImageSize.ImageSize==eBarImageSize.Medium)
							size=new Size(24,24);
						else if(iImageSize.ImageSize==eBarImageSize.Large)
							size=new Size(32,32);
					}
					else if(m_Parent is SideBarPanelItem && ((SideBarPanelItem)m_Parent).ItemImageSize!=eBarImageSize.Default)
					{
						eBarImageSize imgSize=((SideBarPanelItem)m_Parent).ItemImageSize;
						if(imgSize==eBarImageSize.Medium)
							size=new Size(24,24);
						else if(imgSize==eBarImageSize.Large)
							size=new Size(32,32);
					}
				}
				catch(Exception)
				{
				}

				return size;
			}
        }

        protected override void OnCommandChanged()
        {
            if (!this.DesignMode && this.Command == null)
                this.Text = "";
            base.OnCommandChanged();
        }

        private eOrientation _TextOrientation = eOrientation.Horizontal;
        /// <summary>
        /// Gets or sets text-orientation. Default is horizontal.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance"), Description("Indicates text-orientation")]
        public eOrientation TextOrientation
        {
            get { return _TextOrientation; }
            set 
            {
                _TextOrientation = value;
                NeedRecalcSize = true;
                this.Refresh();
            }
        }

        private bool _VerticalTextTopUp = true;
        /// <summary>
        /// Gets or sets how vertical text is rotated when TextOrientation = Vertical.
        /// </summary>
        [DefaultValue(true), Category("Appearance"), Description("Indicates how vertical text is rotated when TextOrientation = Vertical.")]
        public bool VerticalTextTopUp
        {
            get { return _VerticalTextTopUp; }
            set 
            { 
                _VerticalTextTopUp = value;
                if (_TextOrientation == eOrientation.Vertical)
                {
                    this.Refresh();
                }
            }
        }
        #endregion

        #region Markup Support
        protected override void TextMarkupLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if(link!=null)
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            base.TextMarkupLinkClick(sender, e);
        }

        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (this.MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }
        #endregion
    }
}
