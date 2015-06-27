using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;
using DevComponents.DotNetBar.Ribbon;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Represents rating item control which provides rating functionality.
    /// </summary>
    [ToolboxItem(false), DesignTimeVisible(false), DefaultEvent("RatingChanged"), Designer("DevComponents.DotNetBar.Design.RatingItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
    public class RatingItem : PopupItem
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RatingItem class.
        /// </summary>
        public RatingItem()
        {
            _BackgroundStyle.StyleChanged += new EventHandler(BackgroundStyleChanged);
            _CustomImages = new RatingImages(this);
            this.AutoCollapseOnClick = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _DefaultImage);
                _CustomImages.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when Rating property has changed.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event EventHandler RatingChanged;
        /// <summary>
        /// Occurs when RatingValue property has changed.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event EventHandler RatingValueChanged;
        /// <summary>
        /// Occurs when Rating property is about to be changed and provides opportunity to cancel the change.
        /// </summary>
        [Description("Occurs when Rating property has changed.")]
        public event RatingChangeEventHandler RatingChanging;
        /// <summary>
        /// Occurs when AverageRating property has changed.
        /// </summary>
        [Description("Occurs when AverageRating property has changed.")]
        public event EventHandler AverageRatingChanged;
        /// <summary>
        /// Occurs when AverageRatingValue property has changed.
        /// </summary>
        [Description("Occurs when AverageRatingValue property has changed.")]
        public event EventHandler AverageRatingValueChanged;
        /// <summary>
        /// Occurs when text markup link is clicked. Markup links can be created using "a" tag, for example:
        /// <a name="MyLink">Markup link</a>
        /// </summary>
        public event MarkupLinkClickEventHandler MarkupLinkClick;
        /// <summary>
        /// Occurs when RatingValue property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseIntegerValueEventHandler ParseRatingValue;
        /// <summary>
        /// Occurs when AverageRatingValue property is set and it allows you to provide custom parsing for the values.
        /// </summary>
        public event ParseDoubleValueEventHandler ParseAverageRatingValue;
        #endregion

        #region Internal Implementation
        public override BaseItem Copy()
        {
            RatingItem objCopy = new RatingItem();
            objCopy.Name = this.Name;
            this.CopyToItem(objCopy);
            return objCopy;
        }
        /// <summary>
        /// Copies the ButtonItem specific properties to new instance of the item.
        /// </summary>
        /// <param name="copy">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem copy)
        {
            RatingItem objCopy = copy as RatingItem;
            objCopy.AverageRating = this.AverageRating;
            objCopy.Rating = this.Rating;
            objCopy.RatingOrientation = this.RatingOrientation;
            objCopy.IsEditable = this.IsEditable;
            objCopy.TextSpacing = this.TextSpacing;
            objCopy.TextColor = this.TextColor;
            objCopy.TextVisible = this.TextVisible;
            base.CopyToItem(objCopy);
        }

        private double _AverageRating = 0;
        /// <summary>
        /// Gets or sets the average rating shown by control. Control will display average rating (if set) when no explicit
        /// Rating value is set through Rating property. Minimum value is 0 and Maximum value is 5.
        /// </summary>
        [DefaultValue(0d), Category("Data"), Description("Indicates average rating shown by control.")]
        public double AverageRating
        {
            get { return _AverageRating; }
            set
            {
                if (value < 0) value = 0;
                if (value > 5) value = 5;
                if (value != _AverageRating)
                {
                    _AverageRating = value;
                    OnAverageRatingChanged(EventArgs.Empty);
                    this.Refresh();
                }
            }
        }
        /// <summary>
        /// Raises the AverageRatingChanged event.
        /// </summary>
        /// <param name="eventArgs">Event data.</param>
        protected virtual void OnAverageRatingChanged(EventArgs eventArgs)
        {
            if (AverageRatingChanged != null) AverageRatingChanged(this, eventArgs);
            if (AverageRatingValueChanged != null) AverageRatingValueChanged(this, eventArgs);
        }
        /// <summary>
        /// Gets or sets the AverageRating property. This property is provided for Data-Binding with NULL value support.
        /// </summary>
        [Bindable(true), Browsable(false), RefreshProperties(RefreshProperties.All), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter))]
        public object AverageRatingValue
        {
            get
            {
                return AverageRating;
            }
            set
            {
                if (AcceptCustomAverageRatingValue(value))
                    return;
                else if (IsNull(value))
                    this.AverageRating = 0;
                else if (value is double || value is int || value is float)
                {
                    this.AverageRating = (double)value;
                }
                else if (value is long)
                {
                    string t = value.ToString();
                    this.AverageRating = double.Parse(t);
                }
                else if (value is string)
                {
                    double i = 0;
#if FRAMEWORK20
                    if (double.TryParse(value.ToString(), out i))
                        this.AverageRating = i;
                    else
                        throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
#else
					this.AverageRating = double.Parse(value.ToString());
#endif
                }
                else
                    throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
            }
        }
        private bool AcceptCustomAverageRatingValue(object value)
        {
            ParseDoubleValueEventArgs e = new ParseDoubleValueEventArgs(value);
            OnParseAverageRatingValue(e);
            if (e.IsParsed)
            {
                this.AverageRating = e.ParsedValue;
            }

            return e.IsParsed;
        }
        /// <summary>
        /// Raises the ParseAverageRatingValue event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
        {
            if (ParseAverageRatingValue != null)
                ParseAverageRatingValue(this, e);
        }

        /// <summary>
        /// Sets the Rating value of the control and provides information about source of the rating change.
        /// </summary>
        /// <param name="newRating">New Rating value.</param>
        /// <param name="eEventSource">Source of this change.</param>
        public void SetRating(int newRating, eEventSource source)
        {
            if (newRating == _Rating) return;
            RatingChangeEventArgs e = new RatingChangeEventArgs(newRating, _Rating, source);
            OnRatingChanging(e);
            if (e.Cancel) return;
            newRating = e.NewRating;
            _Rating = newRating;
            OnRatingChanged(e);
            ExecuteCommand();
            this.Refresh();
        }

        /// <summary>
        /// Raises RatingChanging event.
        /// </summary>
        /// <param name="e">Event data</param>
        protected virtual void OnRatingChanging(RatingChangeEventArgs e)
        {
            if (RatingChanging != null)
                RatingChanging(this, e);
        }

        private int _Rating = 0;
        /// <summary>
        /// Gets or sets the rating value represented by the control. Default value is 0 which indicates
        /// that there is no rating set. Maximum value is 5.
        /// </summary>
        [DefaultValue(0), Category("Data"), Description("Indicates rating value represented by the control.")]
        public int Rating
        {
            get { return _Rating; }
            set
            {
                if (value < 0) value = 0;
                if (value > 5) value = 5;
                if (_Rating != value)
                {
                    SetRating(value, eEventSource.Code);
                }
            }
        }
        /// <summary>
        /// Raises the RatingChanged event.
        /// </summary>
        /// <param name="eventArgs">Event data.</param>
        protected virtual void OnRatingChanged(EventArgs eventArgs)
        {
            EventHandler handler = RatingChanged;
            if (handler != null) handler(this, eventArgs);

            handler = RatingValueChanged;
            if (handler != null) handler(this, eventArgs);
        }
        /// <summary>
        /// Gets or sets the Rating property value. This property is provided for Data-Binding with NULL value support.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), RefreshProperties(RefreshProperties.All), Bindable(true)]
        public object RatingValue
        {
            get { return _Rating; }
            set 
            {
                if (AcceptCustomRatingValue(value))
                    return;
                else if (IsNull(value))
                    this.Rating = 0;
                else if (value is int)
                {
                    this.Rating = (int)value;
                }
                else if (value is string)
                {
                    int i = 0;
#if FRAMEWORK20
                    if (int.TryParse(value.ToString(), out i))
                        this.Rating = i;
                    else
                        throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
#else
					this.Rating = int.Parse(value.ToString());
#endif
                }
                else
                    throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
            }
        }
        protected virtual bool IsNull(object value)
        {
            if (value == null || value is DBNull) return true;
            return false;
        }
        private bool AcceptCustomRatingValue(object value)
        {
            ParseIntegerValueEventArgs e = new ParseIntegerValueEventArgs(value);
            OnParseRatingValue(e);
            if (e.IsParsed)
            {
                this.Rating = e.ParsedValue;
            }

            return e.IsParsed;
        }
        /// <summary>
        /// Raises the ParseRating event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
        {
            if (ParseRatingValue != null)
                ParseRatingValue(this, e);
        }

        private bool _IsEditable = true;
        /// <summary>
        /// Gets or sets whether rating can be edited. Default value is true.
        /// </summary>
        [DefaultValue(true), Category("Behavior"), Description("Indicates whether rating can be edited.")]
        public bool IsEditable
        {
            get { return _IsEditable; }
            set
            {
                if (_IsEditable != value)
                {
                    _IsEditable = value;
                    SetMouseOverIndex(-1);
                }
            }
        }

        RatingInfo[] _RatingInfo = new RatingInfo[5];
        private Size _DefaultImageSize = new Size(13, 13);
        public override void Paint(ItemPaintArgs p)
        {
            PaintBackground(p);

            Rectangle bounds = this.DisplayRectangle;
            if(_RenderMenuSide)
                PaintMenuItemSide(p, ref bounds);
            bounds = GetClientRectangle(bounds);
            if (_TextVisible && !_TextSize.IsEmpty)
                PaintText(p, bounds);

            Graphics g = p.Graphics;

            if (_MouseOverRatingIndex >= 0)
            {
                RatingImage rated = GetRatedMouseOverImage();
                RatingImage unrated = GetUnratedMouseOverImage();
                for (int i = 0; i < 5; i++)
                {
                    if (_RatingInfo[i].IsMouseOver)
                        g.DrawImage(rated.Image, _RatingInfo[i].Bounds, rated.SourceBounds, GraphicsUnit.Pixel);
                    else
                        g.DrawImage(unrated.Image, _RatingInfo[i].Bounds, unrated.SourceBounds, GraphicsUnit.Pixel);
                }
            }
            else if (_AverageRating > 0 && _Rating == 0)
            {
                RatingImage rated = GetAverageRatedImage();
                RatingImage unrated = GetUnratedImage();
                for (int i = 0; i < 5; i++)
                {
                    if (i + 1 <= _AverageRating)
                        g.DrawImage(rated.Image, _RatingInfo[i].Bounds, rated.SourceBounds, GraphicsUnit.Pixel);
                    else if (i + 1 == Math.Ceiling(_AverageRating))
                    {
                        int size = (int)((RatingOrientation == eOrientation.Horizontal ? _RatingInfo[i].Bounds.Width : _RatingInfo[i].Bounds.Height) * (_AverageRating - Math.Floor(_AverageRating)));
                        g.DrawImage(unrated.Image, _RatingInfo[i].Bounds, unrated.SourceBounds, GraphicsUnit.Pixel);
                        if (size > 0)
                        {
                            Region oldClip = g.Clip;
                            if (RatingOrientation == eOrientation.Horizontal)
                                g.SetClip(new Rectangle(_RatingInfo[i].Bounds.X, _RatingInfo[i].Bounds.Y, size, _RatingInfo[i].Bounds.Height), System.Drawing.Drawing2D.CombineMode.Intersect);
                            else
                                g.SetClip(new Rectangle(_RatingInfo[i].Bounds.X + (_RatingInfo[i].Bounds.Height - size), _RatingInfo[i].Bounds.Y, _RatingInfo[i].Bounds.Width, size), System.Drawing.Drawing2D.CombineMode.Intersect);
                            g.DrawImage(rated.Image, _RatingInfo[i].Bounds, rated.SourceBounds, GraphicsUnit.Pixel);
                            g.Clip = oldClip;
                        }
                    }
                    else
                        g.DrawImage(unrated.Image, _RatingInfo[i].Bounds, unrated.SourceBounds, GraphicsUnit.Pixel);
                }
            }
            else
            {
                RatingImage rated = GetRatedImage();
                RatingImage unrated = GetUnratedImage();
                for (int i = 0; i < 5; i++)
                {
                    if (_Rating > 0 && i + 1 <= _Rating)
                        g.DrawImage(rated.Image, _RatingInfo[i].Bounds, rated.SourceBounds, GraphicsUnit.Pixel);
                    else
                        g.DrawImage(unrated.Image, _RatingInfo[i].Bounds, unrated.SourceBounds, GraphicsUnit.Pixel);
                }
            }

            if (this.Focused && this.DesignMode)
            {
                Rectangle r = this.DisplayRectangle;
                r.Inflate(-1, -1);
                DesignTime.DrawDesignTimeSelection(g, r, p.Colors.ItemDesignTimeBorder);
            }
        }

        private void PaintText(ItemPaintArgs p, Rectangle bounds)
        {
            Color textColor = Color.Empty;
            if (!_TextColor.IsEmpty)
                textColor = _TextColor;
            else
                textColor = GetTextColor(p);
            bool rtl = p.RightToLeft;
            Rectangle textRect = bounds;
            if (_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && !rtl || rtl && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near)
            {
                textRect.X = textRect.Right - _TextSize.Width;
                textRect.Width = _TextSize.Width;
            }
            else
                textRect.Width = _TextSize.Width;
            Graphics g = p.Graphics;
            Font font = p.Font;
            eTextFormat textFormat = _BackgroundStyle.TextFormat;

            if (_TextVisible && this.Text != "" && !textRect.IsEmpty && !textColor.IsEmpty)
            {
                if (this.TextMarkupBody != null)
                {
                    TextMarkup.MarkupDrawContext d = new TextMarkup.MarkupDrawContext(g, font, textColor, rtl);
                    d.HotKeyPrefixVisible = !((textFormat & eTextFormat.HidePrefix) == eTextFormat.HidePrefix);
                    if ((textFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
                        textRect.Y = this.TopInternal + (this.Bounds.Height - textRect.Height) / 2;
                    else if ((textFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
                        textRect.Y += (this.Bounds.Height - textRect.Height) + 1;

                    this.TextMarkupBody.Bounds = textRect;
                    this.TextMarkupBody.Render(d);
                }
                else
                {
#if FRAMEWORK20
                    if (p.GlassEnabled && this.Parent is CaptionItemContainer && !(p.ContainerControl is QatToolbar))
                    {
                        if (!p.CachedPaint)
                            Office2007RibbonControlPainter.PaintTextOnGlass(g, this.Text, font, textRect, TextDrawing.GetTextFormat(textFormat));
                    }
                    else
#endif
                        TextDrawing.DrawString(g, this.Text, font, textColor, textRect, textFormat);
                }
            }
        }

        private Color GetTextColor(ItemPaintArgs pa)
        {
            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && pa.Renderer is Rendering.Office2007Renderer)
            {
                if ((pa.IsOnMenu || pa.IsOnPopupBar) && ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors.Count > 0)
                {
                    return GetEnabled(pa.ContainerControl) ? ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText;
                }
                else
                {
                    if ((pa.ContainerControl is RibbonStrip || pa.ContainerControl is Bar) && ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors.Count > 0)
                        return ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.RibbonTabItemColors[0].Default.Text;
                    Rendering.Office2007ButtonItemColorTable ct = ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
                    if (ct != null && !ct.Default.Text.IsEmpty)
                        return GetEnabled(pa.ContainerControl) ? ct.Default.Text : pa.Colors.ItemDisabledText;
                }
            }
            else
                return pa.Colors.ItemText;

            return _TextColor;
        }

        private void PaintMenuItemSide(ItemPaintArgs pa, ref Rectangle r)
        {
            bool isOnMenu = this.IsOnMenu && !(this.Parent is ItemContainer);
            Size objImageSize = GetMaxImageSize();

            if (isOnMenu && (this.EffectiveStyle == eDotNetBarStyle.OfficeXP || this.EffectiveStyle == eDotNetBarStyle.Office2003 || this.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle)))
            {
                Graphics g = pa.Graphics;

                objImageSize.Width += 7;
                r.Width -= objImageSize.Width + _MenuIndent;
                if (!pa.RightToLeft)
                    r.X += objImageSize.Width + _MenuIndent;
                if (this.IsOnCustomizeMenu)
                    objImageSize.Width += objImageSize.Height + 8;
                Rectangle sideRect = new Rectangle(m_Rect.Left, m_Rect.Top, objImageSize.Width, m_Rect.Height);
                if (pa.RightToLeft)
                    sideRect.X = m_Rect.Right - sideRect.Width;
                // Draw side bar
                //if (this.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
                //{
                //    if (!pa.Colors.MenuUnusedSide2.IsEmpty)
                //    {
                //        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(new Rectangle(m_Rect.Left, m_Rect.Top, objImageSize.Width, m_Rect.Height), pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
                //        g.FillRectangle(gradient, sideRect);
                //        gradient.Dispose();
                //    }
                //    else
                //        g.FillRectangle(new SolidBrush(pa.Colors.MenuUnusedSide), sideRect);
                //}
                //else
                {
                    if (!pa.Colors.MenuSide2.IsEmpty)
                    {
                        System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
                        g.FillRectangle(gradient, sideRect);
                        gradient.Dispose();
                    }
                    else
                        g.FillRectangle(new SolidBrush(pa.Colors.MenuSide), sideRect);
                }
                if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
                {
                    Office2007MenuColorTable mt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
                    if (mt != null && !mt.SideBorder.IsEmpty)
                    {
                        if (pa.RightToLeft)
                            DisplayHelp.DrawGradientLine(g, sideRect.X, sideRect.Y, sideRect.X, sideRect.Bottom - 1, mt.SideBorder, 1);
                        else
                            DisplayHelp.DrawGradientLine(g, sideRect.Right - 2, sideRect.Y, sideRect.Right - 2, sideRect.Bottom - 1, mt.SideBorder, 1);
                    }
                    if (mt != null && !mt.SideBorderLight.IsEmpty)
                    {
                        if (pa.RightToLeft)
                            DisplayHelp.DrawGradientLine(g, sideRect.X + 1, sideRect.Y, sideRect.X + 1, sideRect.Bottom - 1, mt.SideBorderLight, 1);
                        else
                            DisplayHelp.DrawGradientLine(g, sideRect.Right - 1, sideRect.Y, sideRect.Right - 1, sideRect.Bottom - 1, mt.SideBorderLight, 1);
                    }
                }
            }

            if (this.IsOnCustomizeMenu)
            {
                if (this.EffectiveStyle == eDotNetBarStyle.OfficeXP || this.EffectiveStyle == eDotNetBarStyle.Office2003 || this.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle))
                {
                    r.X += (objImageSize.Height + 8);
                    r.Width -= (objImageSize.Height + 8);
                }
                else
                {
                    r.X += (objImageSize.Height + 4);
                    r.Width -= (objImageSize.Height + 4);
                }
            }
        }

        private RatingImage GetAverageRatedImage()
        {
            if (_CustomImages.AverageRated != null)
                return new RatingImage(_CustomImages.AverageRated, new Rectangle(Point.Empty, _CustomImages.AverageRated.Size));
            EnsureDefaultImage();
            return new RatingImage(_DefaultImage, new Rectangle(0, 52, _DefaultImageSize.Width, _DefaultImageSize.Height));
        }

        private RatingImage GetRatedImage()
        {
            if (_CustomImages.Rated != null)
                return new RatingImage(_CustomImages.Rated, new Rectangle(Point.Empty, _CustomImages.Rated.Size));
            EnsureDefaultImage();
            return new RatingImage(_DefaultImage, new Rectangle(0, 26, _DefaultImageSize.Width, _DefaultImageSize.Height));
        }

        private RatingImage GetUnratedImage()
        {
            if (_CustomImages.Unrated != null)
                return new RatingImage(_CustomImages.Unrated, new Rectangle(Point.Empty, _CustomImages.Unrated.Size));
            EnsureDefaultImage();
            return new RatingImage(_DefaultImage, new Rectangle(0, 0, _DefaultImageSize.Width, _DefaultImageSize.Height));
        }

        private RatingImage GetUnratedMouseOverImage()
        {
            if (_CustomImages.UnratedMouseOver != null)
                return new RatingImage(_CustomImages.UnratedMouseOver, new Rectangle(Point.Empty, _CustomImages.UnratedMouseOver.Size));
            EnsureDefaultImage();
            return new RatingImage(_DefaultImage, new Rectangle(0, 13, _DefaultImageSize.Width, _DefaultImageSize.Height));
        }

        private RatingImage GetRatedMouseOverImage()
        {
            if (_CustomImages.RatedMouseOver != null)
                return new RatingImage(_CustomImages.RatedMouseOver, new Rectangle(Point.Empty, _CustomImages.RatedMouseOver.Size));
            EnsureDefaultImage();
            return new RatingImage(_DefaultImage, new Rectangle(0, 39, _DefaultImageSize.Width, _DefaultImageSize.Height));
        }

        private Image _DefaultImage = null;
        private void EnsureDefaultImage()
        {
            if (_DefaultImage == null)
                _DefaultImage = BarFunctions.LoadBitmap("SystemImages.Rating.png");
        }

        private struct RatingImage
        {
            /// <summary>
            /// Initializes a new instance of the RatingImage structure.
            /// </summary>
            /// <param name="image"></param>
            /// <param name="sourceBounds"></param>
            public RatingImage(Image image, Rectangle sourceBounds)
            {
                Image = image;
                SourceBounds = sourceBounds;
            }
            public Image Image;
            public Rectangle SourceBounds;
        }

        private struct RatingInfo
        {
            public Rectangle Bounds;
            public bool IsMouseOver;
        }

        public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (_IsEditable)
            {
                int mouseOverIndex = -1;
                if (RatingOrientation == eOrientation.Horizontal)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (e.X >= _RatingInfo[i].Bounds.X && e.X <= _RatingInfo[i].Bounds.Right)
                        {
                            mouseOverIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (e.Y >= _RatingInfo[i].Bounds.Y && e.Y <= _RatingInfo[i].Bounds.Bottom && (!_TextVisible || _TextSize.IsEmpty) ||
                            _RatingInfo[i].Bounds.Contains(e.X, e.Y))
                        {
                            mouseOverIndex = i;
                            break;
                        }
                    }
                }

                SetMouseOverIndex(mouseOverIndex);
            }
            else
                SetMouseOverIndex(-1);

            base.InternalMouseMove(e);
        }

        public override void InternalClick(System.Windows.Forms.MouseButtons buttons, Point position)
        {
            if (_MouseOverRatingIndex >= 0)
                SetRating(_MouseOverRatingIndex + 1, eEventSource.Mouse);
            base.InternalClick(buttons, position);
        }

        private int _MouseOverRatingIndex = -1;
        private void SetMouseOverIndex(int mouseOverIndex)
        {
            if (mouseOverIndex == _MouseOverRatingIndex) return;
            _MouseOverRatingIndex=mouseOverIndex;
            
            for (int i = 0; i < 5; i++)
            {
                _RatingInfo[i].IsMouseOver = (mouseOverIndex >= 0 && i <= mouseOverIndex);
            }
            Refresh();
        }

        public override void InternalMouseLeave()
        {
            if (_IsEditable)
            {
                SetMouseOverIndex(-1);
            }
            base.InternalMouseLeave();
        }

        private Rectangle GetClientRectangle()
        {
            return GetClientRectangle(this.DisplayRectangle);
        }
        private Rectangle GetClientRectangle(Rectangle bounds)
        {
            bounds.X += ElementStyleLayout.LeftWhiteSpace(_BackgroundStyle);
            bounds.Y += ElementStyleLayout.TopWhiteSpace(_BackgroundStyle);
            bounds.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(_BackgroundStyle);
            bounds.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(_BackgroundStyle);
            return bounds;
        }

        private void UpdateRatingBounds()
        {
            Rectangle bounds = GetClientRectangle();

            if (_TextVisible && !_TextSize.IsEmpty)
            {
                if (_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && this.IsRightToLeft || !this.IsRightToLeft && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near)
                    bounds.X += (_TextSpacing + _TextSize.Width + (_RenderMenuSide ? _MenuIndent : 0));
            }
            if (_RenderMenuSide)
            {
                if (_BackgroundStyle.TextAlignment == eStyleTextAlignment.Far && this.IsRightToLeft || !this.IsRightToLeft && _BackgroundStyle.TextAlignment == eStyleTextAlignment.Near)
                {
                    Size objImageSize = GetMaxImageSize();
                    // Get the right image size that we will use for calculation
                    bounds.X += (objImageSize.Width + 7);
                    if (this.IsOnCustomizeMenu)
                        bounds.X += (bounds.Height + 2);
                }
            }

            Size size = DisplayHelp.MaxSize(_DefaultImageSize, _CustomImages.LargestImageSize);
            if (bounds.Height > size.Height && this.RatingOrientation == eOrientation.Horizontal)
                bounds.Y += (bounds.Height - size.Height) / 2;

            if (this.RatingOrientation == eOrientation.Horizontal)
            {
                Point p = bounds.Location;
                for (int i = 0; i < 5; i++)
                {
                    _RatingInfo[i].Bounds = new Rectangle(p, size);
                    p.X += size.Width;
                }
            }
            else
            {
                Point p = new Point(bounds.X, bounds.Bottom - size.Height);
                for (int i = 0; i < 5; i++)
                {
                    _RatingInfo[i].Bounds = new Rectangle(p, size);
                    p.Y -= size.Height;
                }
            }
        }

        private int _MenuIndent = 8;
        private Size _TextSize = Size.Empty;
        private bool _RenderMenuSide = false;
        public override void RecalcSize()
        {
            Rectangle bounds = this.DisplayRectangle;
            Size size = DisplayHelp.MaxSize(_DefaultImageSize, _CustomImages.LargestImageSize);

            if (this.RatingOrientation == eOrientation.Horizontal)
            {
                size.Width *= 5;
            }
            else
            {
                size.Height *= 5;
            }

            size.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(_BackgroundStyle);
            size.Height += ElementStyleLayout.VerticalStyleWhiteSpace(_BackgroundStyle);

            _RenderMenuSide = false;
            bool isOnMenu = this.IsOnMenu && !(this.Parent is ItemContainer);
            if (isOnMenu && (this.EffectiveStyle == eDotNetBarStyle.OfficeXP || this.EffectiveStyle == eDotNetBarStyle.Office2003 || this.EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(this.EffectiveStyle)))
            {
                Size objImageSize = GetMaxImageSize();
                // Get the right image size that we will use for calculation
                size.Width += (objImageSize.Width + 7);
                if (this.IsOnCustomizeMenu)
                    size.Width += (size.Height + 2);
                size.Width += _MenuIndent;
                _RenderMenuSide = true;
            }
            _TextSize = Size.Empty;
            if (_TextVisible)
            {
                Control objCtrl = this.ContainerControl as Control;
                if (objCtrl == null || objCtrl.Disposing || objCtrl.IsDisposed)
                    return;

                Graphics g = BarFunctions.CreateGraphics(objCtrl);
                if (g == null) return;
                try
                {
                    Size textSize = ButtonItemLayout.MeasureItemText(this, g, 0, objCtrl.Font, eTextFormat.Default, objCtrl.RightToLeft == RightToLeft.Yes);
                    textSize.Width += 1;
                    size.Width += _TextSpacing + textSize.Width;
                    size.Height = Math.Max(size.Height, textSize.Height);
                    _TextSize = textSize;
                }
                finally
                {
                    g.Dispose();
                }
            }

            bounds.Size = size;

            if(!(this.ContainerControl is DevComponents.DotNetBar.Controls.RatingStar))
                m_Rect = bounds;
            UpdateRatingBounds();
            base.RecalcSize();
        }
        private Size GetMaxImageSize()
        {
            if (m_Parent != null)
            {
                ImageItem objParentImageItem = m_Parent as ImageItem;
                if (objParentImageItem != null)
                    return objParentImageItem.SubItemsImageSize;
                else
                    return this.ImageSize;
            }
            else
                return this.ImageSize;
        }

        public override Rectangle Bounds
        {
            get { return (base.Bounds); }

            set
            {
                if (base.Bounds.Equals(value) == false)
                {
                    base.Bounds = value;

                    UpdateRatingBounds();
                }
            }
        }

        protected override void OnLeftLocationChanged(int oldValue)
        {
            UpdateRatingBounds();
            base.OnLeftLocationChanged(oldValue);
        }

        protected override void OnTopLocationChanged(int oldValue)
        {
            UpdateRatingBounds();
            base.OnTopLocationChanged(oldValue);
        }

        //protected override void OnExternalSizeChange()
        //{
        //    UpdateRatingBounds();
        //    base.OnExternalSizeChange();
        //}

        protected virtual void PaintBackground(ItemPaintArgs p)
        {
            _BackgroundStyle.SetColorScheme(p.Colors);
            Graphics g = p.Graphics;
            ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_BackgroundStyle, g, this.DisplayRectangle));
        }

        internal void SetBackgroundStyle(ElementStyle elementStyle)
        {
            _BackgroundStyle.StyleChanged -= new EventHandler(BackgroundStyleChanged);
            _BackgroundStyle = elementStyle;
            _BackgroundStyle.StyleChanged += new EventHandler(BackgroundStyleChanged);
        }

        private ElementStyle _BackgroundStyle = new ElementStyle();
        /// <summary>
        /// Specifies the control background style. Default value is an empty style which means that container does not display any background.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets control background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return _BackgroundStyle; }
        }
        private void BackgroundStyleChanged(object sender, EventArgs e)
        {
            this.NeedRecalcSize = true;
            this.OnAppearanceChanged();
        }

        private RatingImages _CustomImages;
        /// <summary>
        /// Gets the reference to custom rating images.
        /// </summary>
        [Browsable(true), Category("Images"), Description("Gets the reference to custom rating images."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RatingImages CustomImages
        {
            get { return _CustomImages; }
        }

        private bool _TextVisible = true;
        /// <summary>
        /// Gets or sets whether text assigned to the check box is visible. Default value is true.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Appearance"), Description("Indicates whether text assigned to the check box is visible.")]
        public bool TextVisible
        {
            get { return _TextVisible; }
            set
            {
                _TextVisible = value;
                this.NeedRecalcSize = true;
                OnAppearanceChanged();
            }
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color. Default value is Color.Empty which indicates that default color is used.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates text color.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                OnAppearanceChanged();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !_TextColor.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            this.TextColor = Color.Empty;
        }

        private int _TextSpacing = 0;
        /// <summary>
        /// Gets or sets the spacing between optional text and the rating.
        /// </summary>
        [DefaultValue(0), Category("Appearance"), Description("Gets or sets the spacing between optional text and the rating.")]
        public int TextSpacing
        {
            get { return _TextSpacing; }
            set
            {
                if (_TextSpacing != value)
                {
                    _TextSpacing = value;
                    NeedRecalcSize = true;
                    this.Refresh();
                }
            }
        }

        private eOrientation _RatingOrientation = eOrientation.Horizontal;
        /// <summary>
        /// Gets or sets the orientation of rating control.
        /// </summary>
        [DefaultValue(eOrientation.Horizontal), Category("Appearance"), Description("Gets or sets the orientation of rating control.")]
        public eOrientation RatingOrientation
        {
            get { return _RatingOrientation; }
            set 
            {
                if (_RatingOrientation != value)
                {
                    _RatingOrientation = value;
                    NeedRecalcSize = true;
                    this.Refresh();
                }
            }
        }
        #endregion

        #region Markup Implementation
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

        protected override void TextMarkupLinkClick(object sender, EventArgs e)
        {
            TextMarkup.HyperLink link = sender as TextMarkup.HyperLink;
            if (link != null)
                OnMarkupLinkClick(new MarkupLinkClickEventArgs(link.Name, link.HRef));
            base.TextMarkupLinkClick(sender, e);
        }

        protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
        {
            if (this.MarkupLinkClick != null)
                MarkupLinkClick(this, e);
        }
        #endregion

        #region Property Hiding
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override SubItemsCollection SubItems
        {
            get
            {
                return base.SubItems;
            }
        }

        [Browsable(false)]
        public override bool ClickAutoRepeat
        {
            get
            {
                return base.ClickAutoRepeat;
            }
            set
            {
                base.ClickAutoRepeat = value;
            }
        }
        [Browsable(false)]
        public override int ClickRepeatInterval
        {
            get
            {
                return base.ClickRepeatInterval;
            }
            set
            {
                base.ClickRepeatInterval = value;
            }
        }
        [Browsable(false)]
        public override ePersonalizedMenus PersonalizedMenus
        {
            get
            {
                return base.PersonalizedMenus;
            }
            set
            {
                base.PersonalizedMenus = value;
            }
        }
        [Browsable(false)]
        public override ePopupAnimation PopupAnimation
        {
            get
            {
                return base.PopupAnimation;
            }
            set
            {
                base.PopupAnimation = value;
            }
        }
        [Browsable(false)]
        public override Font PopupFont
        {
            get
            {
                return base.PopupFont;
            }
            set
            {
                base.PopupFont = value;
            }
        }
        [Browsable(false)]
        public override ePopupSide PopupSide
        {
            get
            {
                return base.PopupSide;
            }
            set
            {
                base.PopupSide = value;
            }
        }
        [Browsable(false)]
        public override ePopupType PopupType
        {
            get
            {
                return base.PopupType;
            }
            set
            {
                base.PopupType = value;
            }
        }
        [Browsable(false)]
        public override int PopupWidth
        {
            get
            {
                return base.PopupWidth;
            }
            set
            {
                base.PopupWidth = value;
            }
        }
        [Browsable(false)]
        public override ShortcutsCollection Shortcuts
        {
            get
            {
                return base.Shortcuts;
            }
            set
            {
                base.Shortcuts = value;
            }
        }
        [Browsable(false)]
        public override bool ShowSubItems
        {
            get
            {
                return base.ShowSubItems;
            }
            set
            {
                base.ShowSubItems = value;
            }
        }
        [Browsable(false)]
        public override bool Stretch
        {
            get
            {
                return base.Stretch;
            }
            set
            {
                base.Stretch = value;
            }
        }
        [Browsable(false)]
        public override bool ThemeAware
        {
            get
            {
                return base.ThemeAware;
            }
            set
            {
                base.ThemeAware = value;
            }
        }
        [DefaultValue(false)]
        public override bool AutoCollapseOnClick
        {
            get
            {
                return base.AutoCollapseOnClick;
            }
            set
            {
                base.AutoCollapseOnClick = value;
            }
        }
        #endregion
    }

    #region RatingImages
    /// <summary>
    /// Defines the custom rating images for Rating control.
    /// </summary>
    [ToolboxItem(false), TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class RatingImages
    {
        private RatingItem _Parent = null;
        /// <summary>
        /// Initializes a new instance of the RatingImages class.
        /// </summary>
        internal RatingImages(RatingItem parentItem)
        {
            _Parent = parentItem;
        }

        public void Dispose()
        {
            BarUtilities.DisposeImage(ref _AverageRated);
            BarUtilities.DisposeImage(ref _Rated);
            BarUtilities.DisposeImage(ref _RatedMouseOver);
            BarUtilities.DisposeImage(ref _Unrated);
            BarUtilities.DisposeImage(ref _UnratedMouseOver);
        }

        private Image _Unrated = null;
        /// <summary>
        /// Gets or sets the image used for unrated rating part.
        /// </summary>
        [DefaultValue(null), Description("Gets or sets the image used for unrated rating part.")]
        public Image Unrated
        {
            get
            {
                return _Unrated;
            }
            set
            {
                if (_Unrated != value)
                {
                    _Unrated = value;
                    OnImageChanged();
                }
            }
        }

        private Image _UnratedMouseOver = null;
        /// <summary>
        /// Gets or sets the image used for unrated rating part when mouse is over the control.
        /// </summary>
        [DefaultValue(null), Description("Gets or sets the image used for unrated rating part when mouse is over the control.")]
        public Image UnratedMouseOver
        {
            get
            {
                return _UnratedMouseOver;
            }
            set
            {
                if (_UnratedMouseOver != value)
                {
                    _UnratedMouseOver = value;
                    OnImageChanged();
                }
            }
        }

        private Image _Rated = null;
        /// <summary>
        /// Gets or sets the image used for rated part of the control.
        /// </summary>
        [DefaultValue(null), Description("Gets or sets the image used for rated part of the control.")]
        public Image Rated
        {
            get
            {
                return _Rated;
            }
            set
            {
                if (_Rated != value)
                {
                    _Rated = value;
                    OnImageChanged();
                }
            }
        }

        private Image _AverageRated = null;
        /// <summary>
        /// Gets or sets the image used for Average Rated part of the control.
        /// </summary>
        [DefaultValue(null), Description("Gets or sets the image used for Average Rated part of the control.")]
        public Image AverageRated
        {
            get
            {
                return _AverageRated;
            }
            set
            {
                if (_AverageRated != value)
                {
                    _AverageRated = value;
                    OnImageChanged();
                }
            }
        }

        private Image _RatedMouseOver = null;
        /// <summary>
        /// Gets or sets the image used for rated part of the control when mouse is over the control.
        /// </summary>
        [DefaultValue(null), Description("Gets or sets the image used for rated part of the control when mouse is over the control.")]
        public Image RatedMouseOver
        {
            get
            {
                return _RatedMouseOver;
            }
            set
            {
                if (_RatedMouseOver != value)
                {
                    _RatedMouseOver = value;
                    OnImageChanged();
                }
            }
        }

        private Size _LargestImageSize = Size.Empty;
        internal Size LargestImageSize
        {
            get
            {
                return _LargestImageSize;
            }
        }

        private void OnImageChanged()
        {
            _LargestImageSize = new Size();
            if (_Unrated != null)
                _LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _Unrated.Size);
            if (_UnratedMouseOver != null)
                _LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _UnratedMouseOver.Size);
            if (_Rated != null)
                _LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _Rated.Size);
            if (_RatedMouseOver != null)
                _LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _RatedMouseOver.Size);
            if (_AverageRated != null)
                _LargestImageSize = DisplayHelp.MaxSize(_LargestImageSize, _AverageRated.Size);

            if (_Parent != null)
            {
                _Parent.NeedRecalcSize = true;
                _Parent.OnAppearanceChanged();
            }
        }
    }
    #endregion

    /// <summary>
    /// Delegate for Rating change events.
    /// </summary>
    public delegate void RatingChangeEventHandler(object sender, RatingChangeEventArgs e);

    #region CheckBoxChangeEventArgs
    /// <summary>
    /// Represents event arguments for Rating change events.
    /// </summary>
    public class RatingChangeEventArgs : CancelEventArgs
    {
        /// <summary>
        /// New Rating value being assigned.
        /// </summary>
        public int NewRating;
        /// <summary>
        /// Previous or current value (if RatingChanging event).
        /// </summary>
        public readonly int OldRating;
        /// <summary>
        /// Indicates the action that has caused the event.
        /// </summary>
        public readonly eEventSource EventSource = eEventSource.Code;

        /// <summary>
        /// Initializes a new instance of the RatingChangeEventArgs class.
        /// </summary>
        /// <param name="newRating"></param>
        /// <param name="oldRating"></param>
        /// <param name="eventSource"></param>
        public RatingChangeEventArgs(int newRating, int oldRating, eEventSource eventSource)
        {
            NewRating = newRating;
            OldRating = oldRating;
            EventSource = eventSource;
        }
    }
    #endregion
}
