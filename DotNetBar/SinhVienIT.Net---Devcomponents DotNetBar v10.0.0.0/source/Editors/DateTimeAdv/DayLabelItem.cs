#if FRAMEWORK20
using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors.DateTimeAdv
{
    public class DayLabel : PopupItem
    {
        #region Private Variables
        private ElementStyle _BackgroundStyle = new ElementStyle();
        private Rectangle _ImageRenderBounds = Rectangle.Empty;
        #endregion

        #region Events

        /// <summary>
        /// Occurs when label is rendered and it allows you to override default rendering.
        /// </summary>
        public event DayPaintEventHandler PaintLabel;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the DayLabel class.
        /// </summary>
        public DayLabel()
        {
            _BackgroundStyle.StyleChanged += new EventHandler(BackgroundStyleChanged);
            MouseUpNotification = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (BarUtilities.DisposeItemImages && !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref _Image);
            }
            _BackgroundStyle.StyleChanged -= BackgroundStyleChanged;
            _BackgroundStyle.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        #region Internal Implementation
        public override void Paint(ItemPaintArgs p)
        {
            DayPaintEventArgs e = new DayPaintEventArgs(p, this);
            OnPaintLabel(e);
            if (e.RenderParts == eDayPaintParts.None) return;
            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null)
            {
                month.OnPaintLabel(this, e);
            }
            if (e.RenderParts == eDayPaintParts.None) return;

            if (this.Enabled && (e.RenderParts & eDayPaintParts.Background) == eDayPaintParts.Background)
                PaintBackground(p);

            if ((e.RenderParts & eDayPaintParts.Text) == eDayPaintParts.Text)
                PaintText(p, null, Color.Empty, _TextAlign);

            if ((e.RenderParts & eDayPaintParts.Image) == eDayPaintParts.Image)
                PaintImage(p, _Image, _ImageAlign);
        }

        internal void PaintImage(ItemPaintArgs p, Image image, eLabelPartAlignment imageAlign)
        {
            if (image == null) return;
            Graphics g = p.Graphics;

            Rectangle imageRect = GetAlignedRect(this.DisplayRectangle, image.Size, imageAlign);
            CompositeImage ci = new CompositeImage(image, false);
            ci.DrawImage(g, imageRect);
            ci.Dispose();
            _ImageRenderBounds = imageRect;
        }

        private Rectangle GetAlignedRect(Rectangle bounds, Size innerSize, eLabelPartAlignment partAlign)
        {
            Rectangle innerRect = new Rectangle(bounds.Right - innerSize.Width, bounds.Y, innerSize.Width, innerSize.Height);
            if (partAlign == eLabelPartAlignment.BottomCenter)
                innerRect.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Bottom - innerSize.Height);
            else if (partAlign == eLabelPartAlignment.BottomLeft)
                innerRect.Location = new Point(bounds.X, bounds.Bottom - innerSize.Height);
            else if (partAlign == eLabelPartAlignment.BottomRight)
                innerRect.Location = new Point(bounds.Right - innerSize.Width, bounds.Bottom - innerSize.Height);
            else if (partAlign == eLabelPartAlignment.MiddleCenter)
                innerRect.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Y + (bounds.Height - innerSize.Height) / 2);
            else if (partAlign == eLabelPartAlignment.MiddleLeft)
                innerRect.Location = new Point(bounds.X, bounds.Y + (bounds.Height - innerSize.Height) / 2);
            else if (partAlign == eLabelPartAlignment.MiddleRight)
                innerRect.Location = new Point(bounds.Right - innerSize.Width, bounds.Y + (bounds.Height - innerSize.Height) / 2);
            else if (partAlign == eLabelPartAlignment.TopCenter)
                innerRect.Location = new Point(bounds.X + (bounds.Width - innerSize.Width) / 2, bounds.Y);
            else if (partAlign == eLabelPartAlignment.TopLeft)
                innerRect.Location = new Point(bounds.X, bounds.Y);

            return innerRect;
        }

        /// <summary>
        /// Raises the PaintLabel event.
        /// </summary>
        /// <param name="e">Provides event data.</param>
        protected virtual void OnPaintLabel(DayPaintEventArgs e)
        {
            if (PaintLabel != null)
                PaintLabel(this, e);
        }

        internal void PaintBackground(ItemPaintArgs p)
        {
            Graphics g = p.Graphics;
            Rectangle r = this.DisplayRectangle;
            Color backColor = Color.Empty, backColor2 = Color.Empty, borderColor = Color.Empty;

            if (_BackgroundStyle.Custom && (!this.IsMouseOver && !this.IsMouseDown || !_TrackMouse))
            {
                _BackgroundStyle.SetColorScheme(p.Colors);
                bool disposeStyle = false;
                ElementStyle style = ElementStyleDisplay.GetElementStyle(_BackgroundStyle, out disposeStyle);
                ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(style, g, r));
                if(disposeStyle) style.Dispose();
            }
            bool customColors = false;
            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            MonthCalendarColors colors = null;
            if (month != null) colors = month.GetColors();

            if (this.IsMouseDown && _TrackMouse)
            {
                backColor = p.Colors.ItemPressedBackground;
                backColor2 = p.Colors.ItemPressedBackground2;
                borderColor = p.Colors.ItemPressedBorder;
            }
            else if (this.IsMouseOver && _TrackMouse)
            {
                backColor = p.Colors.ItemHotBackground;
                backColor2 = p.Colors.ItemHotBackground2;
                borderColor = p.Colors.ItemHotBorder;
            }
            else if (this.IsSelected)
            {
                backColor = p.Colors.ItemCheckedBackground;
                if (colors != null && colors.Selection.IsCustomized)
                {
                    if (!colors.Selection.BackColor.IsEmpty)
                    {
                        backColor = colors.Selection.BackColor;
                        if (!colors.Selection.BackColor2.IsEmpty)
                            backColor2 = colors.Selection.BackColor2;
                    }
                    if (!colors.Selection.BorderColor.IsEmpty)
                        borderColor = colors.Selection.BorderColor;
                    customColors = true;
                }
            }
            else if (_IsToday)
            {
                borderColor = p.Colors.ItemCheckedBorder;
                if (colors != null && colors.Today.IsCustomized)
                {
                    if (!colors.Today.BorderColor.IsEmpty)
                        borderColor = colors.Today.BorderColor;
                    if (!colors.Today.BackColor.IsEmpty)
                    {
                        backColor = colors.Today.BackColor;
                        if (!colors.Today.BackColor2.IsEmpty)
                            backColor2 = colors.Today.BackColor2;
                    }
                    customColors = true;
                }
            }
            else if (this.IsWeekOfYear)
            {
                backColor = Color.LightGray;
                if (colors != null && colors.WeekOfYear.IsCustomized)
                {
                    if (!colors.WeekOfYear.BorderColor.IsEmpty)
                        borderColor = colors.WeekOfYear.BorderColor;
                    if (!colors.WeekOfYear.BackColor.IsEmpty)
                    {
                        backColor = colors.WeekOfYear.BackColor;
                        if (!colors.WeekOfYear.BackColor2.IsEmpty)
                            backColor2 = colors.WeekOfYear.BackColor2;
                    }
                    customColors = true;
                }
            }

            else if (this.IsTrailing)
            {
                if (colors != null && colors.TrailingDay.IsCustomized)
                {
                    if (!colors.TrailingDay.BackColor.IsEmpty)
                    {
                        backColor = colors.TrailingDay.BackColor;
                        if (!colors.TrailingDay.BackColor2.IsEmpty)
                            backColor2 = colors.TrailingDay.BackColor2;
                    }
                    if (!colors.TrailingDay.BorderColor.IsEmpty)
                        borderColor = colors.TrailingDay.BorderColor;
                    customColors = true;
                }
                if (IsWeekend(Date) && colors != null && colors.TrailingWeekend.IsCustomized)
                {
                    if (!colors.TrailingWeekend.BackColor.IsEmpty)
                    {
                        backColor = colors.TrailingWeekend.BackColor;
                        if (!colors.TrailingWeekend.BackColor2.IsEmpty)
                            backColor2 = colors.TrailingWeekend.BackColor2;
                    }
                    if (!colors.TrailingWeekend.BorderColor.IsEmpty)
                        borderColor = colors.TrailingWeekend.BorderColor;
                    customColors = true;
                }
            }
            else if (this.IsDayLabel)
            {
                if (colors != null && colors.DayLabel.IsCustomized)
                {
                    if (!colors.DayLabel.BackColor.IsEmpty)
                    {
                        backColor = colors.DayLabel.BackColor;
                        if (!colors.DayLabel.BackColor2.IsEmpty)
                            backColor2 = colors.DayLabel.BackColor2;
                    }
                    if (!colors.DayLabel.BorderColor.IsEmpty)
                        borderColor = colors.DayLabel.BorderColor;
                    customColors = true;
                }
            }
            else if (this.Date != DateTime.MinValue)
            {
                if (colors != null && colors.Day.IsCustomized)
                {
                    if (!colors.Day.BackColor.IsEmpty)
                    {
                        backColor = colors.Day.BackColor;
                        if (!colors.Day.BackColor2.IsEmpty)
                            backColor2 = colors.Day.BackColor2;
                    }
                    if (!colors.Day.BorderColor.IsEmpty)
                        borderColor = colors.Day.BorderColor;
                    customColors = true;
                }

                if (IsWeekend(Date) && colors != null && colors.Weekend.IsCustomized)
                {
                    if (!colors.Weekend.BackColor.IsEmpty)
                    {
                        backColor = colors.Weekend.BackColor;
                        if (!colors.Weekend.BackColor2.IsEmpty)
                            backColor2 = colors.Weekend.BackColor2;
                    }
                    if (!colors.Weekend.BorderColor.IsEmpty)
                        borderColor = colors.Weekend.BorderColor;
                    customColors = true;
                }
            }

            if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !this.FlatOffice2007Style && !customColors)
            {
                Office2007ButtonItemStateColorTable ct = GetOffice2007StateColorTable(p);
                if (ct != null)
                {
                    Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RectangleShape);
                    backColor = Color.Empty;
                    backColor2 = Color.Empty;
                    borderColor = Color.Empty;
                }
            }

            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;

            if (!backColor.IsEmpty)
                DisplayHelp.FillRectangle(g, r, backColor, backColor2);

            if (!borderColor.IsEmpty)
                DisplayHelp.DrawRectangle(g, borderColor, r);

            if (this.IsDayLabel)
            {
                borderColor = p.Colors.BarDockedBorder;
                if (colors != null && !colors.DaysDividerBorderColors.IsEmpty)
                    borderColor = colors.DaysDividerBorderColors;
                if (!borderColor.IsEmpty)
                    DisplayHelp.DrawLine(g, r.X, r.Bottom - 1, r.Right, r.Bottom - 1, borderColor, 1);
            }

            g.SmoothingMode = sm;
        }

        protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(ItemPaintArgs p)
        {
            if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !this.FlatOffice2007Style)
            {
                if (p.Renderer is Office2007Renderer)
                {
                    Office2007ColorTable ct = ((Office2007Renderer)p.Renderer).ColorTable;
                    Office2007ButtonItemColorTable buttonColorTable = ct.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
                    if (!this.Enabled)
                        return buttonColorTable.Disabled;
                    else if (this.IsMouseDown && _TrackMouse)
                        return buttonColorTable.Pressed;
                    else if (this.IsMouseOver && _TrackMouse)
                        return buttonColorTable.MouseOver;
                    else if (this.IsSelected)
                        return buttonColorTable.Checked;
                }
            }

            return null;
        }

        internal static bool IsWeekend(DateTime d)
        {
            return d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday;
        }

        internal void PaintText(ItemPaintArgs p, Font textFont, Color textColor, eLabelPartAlignment textAlign)
        {
            Graphics g = p.Graphics;
            string text = this.Text;

            if (_Date != DateTime.MinValue)
                text = _Date.Day.ToString();
            bool isBold = _IsBold;
            if (textColor.IsEmpty)
            {
                if (!_TextColor.IsEmpty)
                    textColor = _TextColor;
                else if (!this.Enabled)
                {
                    textColor = p.Colors.ItemDisabledText;
                }
                else
                {
                    textColor = _IsTrailing ? p.Colors.ItemDisabledText : p.Colors.ItemText;

                    SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
                    MonthCalendarColors colors = null;
                    if (month != null) colors = month.GetColors();

                    if (colors != null)
                    {
                        if (_Date != DateTime.MinValue)
                        {
                            if (_IsSelected && colors.Selection.IsCustomized)
                            {
                                if (!colors.Selection.TextColor.IsEmpty)
                                    textColor = colors.Selection.TextColor;
                                if (colors.Selection.IsBold)
                                    isBold = colors.Selection.IsBold;
                            }
                            else
                            {
                                if (_IsTrailing)
                                {
                                    if (!colors.TrailingDay.TextColor.IsEmpty)
                                        textColor = colors.TrailingDay.TextColor;
                                    if (colors.TrailingDay.IsBold)
                                        isBold = colors.TrailingDay.IsBold;
                                }
                                else if (colors.Day.IsCustomized)
                                {
                                    if (!colors.Day.TextColor.IsEmpty)
                                        textColor = colors.Day.TextColor;
                                    if (colors.Day.IsBold)
                                        isBold = colors.Day.IsBold;
                                }

                                if (IsWeekend(_Date))
                                {
                                    if (_IsTrailing)
                                    {
                                        if (!colors.TrailingWeekend.TextColor.IsEmpty)
                                            textColor = colors.TrailingWeekend.TextColor;
                                        if (colors.TrailingWeekend.IsBold)
                                            isBold = colors.TrailingWeekend.IsBold;
                                    }
                                    else
                                    {
                                        if (!colors.Weekend.TextColor.IsEmpty)
                                            textColor = colors.Weekend.TextColor;
                                        if (colors.Weekend.IsBold)
                                            isBold = colors.TrailingWeekend.IsBold;
                                    }
                                }
                            }
                        }
                        else if (IsWeekOfYear)
                        {
                            if (colors.WeekOfYear.IsCustomized)
                            {
                                if (!colors.WeekOfYear.TextColor.IsEmpty)
                                    textColor = colors.WeekOfYear.TextColor;
                                if (colors.WeekOfYear.IsBold)
                                    isBold = colors.WeekOfYear.IsBold;
                            }
                        }
                        else if (IsDayLabel)
                        {
                            if (!colors.DayLabel.TextColor.IsEmpty)
                                textColor = colors.DayLabel.TextColor;
                            if (colors.DayLabel.IsBold)
                                isBold = colors.DayLabel.IsBold;
                        }
                    }
                }
            }

            bool disposeFont = false;
            if (textFont == null)
            {
                if (isBold)
                {
                    textFont = new Font(p.Font, FontStyle.Bold);
                    disposeFont = true;
                }
                else
                    textFont = p.Font;
            }

            if (_Date != DateTime.MinValue)
            {
                Size size = TextDrawing.MeasureString(g, "32", textFont);
                Rectangle r = GetAlignedRect(this.DisplayRectangle, size, textAlign);
                eTextFormat format = eTextFormat.Right | eTextFormat.VerticalCenter;
                TextDrawing.DrawString(g, text, textFont, textColor,
                    r, format);
            }
            else
            {
                eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
                if (textAlign == eLabelPartAlignment.BottomCenter)
                    format = eTextFormat.Bottom | eTextFormat.HorizontalCenter;
                else if (textAlign == eLabelPartAlignment.BottomLeft)
                    format = eTextFormat.Left | eTextFormat.Bottom;
                else if (textAlign == eLabelPartAlignment.BottomRight)
                    format = eTextFormat.Bottom | eTextFormat.Right;
                else if (textAlign == eLabelPartAlignment.MiddleLeft)
                    format = eTextFormat.Left | eTextFormat.VerticalCenter;
                else if (textAlign == eLabelPartAlignment.MiddleRight)
                    format = eTextFormat.Right | eTextFormat.VerticalCenter;
                else if (textAlign == eLabelPartAlignment.TopCenter)
                    format = eTextFormat.Top | eTextFormat.VerticalCenter;
                else if (textAlign == eLabelPartAlignment.TopLeft)
                    format = eTextFormat.Top | eTextFormat.Left;
                else if (textAlign == eLabelPartAlignment.TopRight)
                    format = eTextFormat.Top | eTextFormat.Right;

                TextDrawing.DrawString(g, text, textFont, textColor,
                    this.Bounds, format);
            }

            if (disposeFont) textFont.Dispose();
        }

        public override void RecalcSize()
        {
            this.Bounds = new Rectangle(this.Bounds.Location, SingleMonthCalendar._DefaultDaySize);
            base.RecalcSize();
        }

        /// <summary>
        /// Returns copy of the item.
        /// </summary>
        public override BaseItem Copy()
        {
            DayLabel objCopy = new DayLabel();
            this.CopyToItem(objCopy);
            return objCopy;
        }
        /// <summary>
        /// Copies the DayLabel specific properties to new instance of the item.
        /// </summary>
        /// <param name="c">New ButtonItem instance.</param>
        protected override void CopyToItem(BaseItem c)
        {
            DayLabel copy = c as DayLabel;

            base.CopyToItem(copy);
        }

        private DateTime _Date = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the date represented by this label. DateTime.MinValue indicates that label is either used as textual day representation
        /// or the week number as specified by the IsWeekOfYear property.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                }
            }
        }

        private bool _IsDayLabel = false;
        /// <summary>
        /// Gets or sets whether this label is used as the label that displays the day name.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IsDayLabel
        {
            get { return _IsDayLabel; }
            set
            {
                if (_IsDayLabel != value)
                {
                    _IsDayLabel = value;
                    this.Refresh();
                }
            }
        }

        private bool _IsWeekOfYear = false;
        /// <summary>
        /// Gets or sets whether this label is used as the week of year label.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IsWeekOfYear
        {
            get { return _IsWeekOfYear; }
            set
            {
                if (_IsWeekOfYear != value)
                {
                    _IsWeekOfYear = value;
                    this.Refresh();
                }
            }
        }

        private bool _IsTrailing = false;
        /// <summary>
        /// Gets whether the label for date represents the trailing date, i.e. date that is from next or previous month for the month displayed.
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        public bool IsTrailing
        {
            get { return _IsTrailing; }
            set
            {
                if (_IsTrailing != value)
                {
                    _IsTrailing = value;
                    this.Refresh();
                }
            }
        }

        private bool _IsMouseOver = false;
        /// <summary>
        /// Gets or sets whether mouse is over the item.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
            set
            {
                _IsMouseOver = value;
                this.Refresh();
            }
        }

        private bool _IsMouseDown = false;
        /// <summary>
        /// Gets or sets whether left-mouse button is pressed over the item.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMouseDown
        {
            get { return _IsMouseDown; }
            set
            {
                _IsMouseDown = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Occurs when the mouse pointer enters the item. This is used by internal implementation only.
        /// </summary>
        public override void InternalMouseEnter()
        {
            this.IsMouseOver = true;
            base.InternalMouseEnter();

            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseEnter(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the mouse pointer leaves the item. This is used by internal implementation only.
        /// </summary>
        public override void InternalMouseLeave()
        {
            this.IsMouseOver = false;
            base.InternalMouseLeave();

            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseLeave(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the item and a mouse button is pressed. This is used by internal implementation only.
        /// </summary>
        public override void InternalMouseDown(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (objArg.Button == System.Windows.Forms.MouseButtons.Left)
                this.IsMouseDown = true;
            if (this.IsMouseDown && this.SubItems.Count > 0 && this.ShowSubItems &&
                (_ImageRenderBounds.Contains(objArg.X, objArg.Y) || _ExpandOnMouseDown))
            {
                this.Expanded = !this.Expanded;
            }
            base.InternalMouseDown(objArg);

            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseDown(this, objArg);
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the item and a mouse button is released. This is used by internal implementation only.
        /// </summary>
        public override void InternalMouseUp(System.Windows.Forms.MouseEventArgs objArg)
        {
            if (objArg.Button == System.Windows.Forms.MouseButtons.Left)
                this.IsMouseDown = false;
            base.InternalMouseUp(objArg);

            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseUp(this, objArg);
        }

        protected override void OnClick()
        {
            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.DayLabelClick(this);
            base.OnClick();
        }

        public override void InternalMouseMove(System.Windows.Forms.MouseEventArgs objArg)
        {
            base.InternalMouseMove(objArg);
            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseMove(this, objArg);
        }

        public override void InternalMouseHover()
        {
            base.InternalMouseHover();
            SingleMonthCalendar month = this.Parent as SingleMonthCalendar;
            if (month != null) month.OnLabelMouseHover(this, new EventArgs());
        }

        private void BackgroundStyleChanged(object sender, EventArgs e)
        {
            this.OnAppearanceChanged();
        }

        /// <summary>
        /// Specifies the item background style. Default value is an empty style which means that container does not display any background.
        /// BeginGroup property set to true will override this style on some styles.
        /// </summary>
        [Browsable(true), DevCoBrowsable(true), Category("Style"), Description("Gets or sets container background style."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ElementStyle BackgroundStyle
        {
            get { return _BackgroundStyle; }
        }

        private bool _IsSelected = false;
        /// <summary>
        /// Gets or sets whether label appears as selected.
        /// </summary>
        [DefaultValue(false), Browsable(false)]
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    this.Refresh();
                }
            }
        }

        private bool _TrackMouse = true;
        /// <summary>
        /// Gets or sets whether label provides visual indicator when mouse is over the label or pressed while over the label. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether label provides visual indicator when mouse is over the label or pressed while over the label.")]
        public bool TrackMouse
        {
            get { return _TrackMouse; }
            set
            {
                if (_TrackMouse != value)
                {
                    _TrackMouse = value;
                    this.Refresh();
                }
            }
        }

        private bool _Selectable = true;
        /// <summary>
        /// Gets or sets whether label is selectable. IsSelected property returns whether label is selected. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether label is selectable.")]
        public bool Selectable
        {
            get { return _Selectable; }
            set
            {
                if (_Selectable != value)
                {
                    _Selectable = value;
                    this.Refresh();
                }
            }
        }

        private Color _TextColor = Color.Empty;
        /// <summary>
        /// Gets or sets the label text color. Default value is an empty color.
        /// </summary>
        [Category("Colors"), Description("Indicates label text color.")]
        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                this.Refresh();
            }
        }
        /// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTextColor()
        {
            return !TextColor.IsEmpty;
        }
        /// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetTextColor()
        {
            TextColor = Color.Empty;
        }

        private bool _IsBold = false;
        /// <summary>
        /// Gets or sets whether text is drawn using Bold font. Default value is false.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether text is drawn using Bold font.")]
        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                if (_IsBold != value)
                {
                    _IsBold = value;
                    this.Refresh();
                }
            }
        }

        //private Color _BorderColor = Color.Empty;
        ///// <summary>
        ///// Gets or sets the label border color. Default value is an empty color.
        ///// </summary>
        //[Category("Colors"), Description("Indicates label border color.")]
        //public Color BorderColor
        //{
        //    get { return _BorderColor; }
        //    set
        //    {
        //        _BorderColor = value;
        //        this.Refresh();
        //    }
        //}
        ///// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeBorderColor()
        //{
        //    return !BorderColor.IsEmpty;
        //}
        ///// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetBorderColor()
        //{
        //    BorderColor = Color.Empty;
        //}

        //private Color _BackColor = Color.Empty;
        ///// <summary>
        ///// Gets or sets the label back color.
        ///// </summary>
        //[Category("Colors"), Description("Indicates label back color.")]
        //public Color BackColor
        //{
        //    get { return _BackColor; }
        //    set
        //    {
        //        _BackColor = value;
        //        this.Refresh();
        //    }
        //}
        ///// <summary>Gets whether property should be serialized. Provided for WinForms designer support.</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public bool ShouldSerializeBackColor()
        //{
        //    return !BackColor.IsEmpty;
        //}
        ///// <summary>Resets property to its default value. Provided for WinForms designer support.</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void ResetBackColor()
        //{
        //    BackColor = Color.Empty;
        //}

        private eLabelPartAlignment _TextAlign = eLabelPartAlignment.MiddleCenter;
        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        [DefaultValue(eLabelPartAlignment.MiddleCenter), Description("Indicates text alignment.")]
        public eLabelPartAlignment TextAlign
        {
            get { return _TextAlign; }
            set
            {
                if (_TextAlign != value)
                {
                    _TextAlign = value;
                    this.Refresh();
                }
            }
        }

        private eLabelPartAlignment _ImageAlign = eLabelPartAlignment.MiddleRight;
        /// <summary>
        /// Gets or sets the image alignment.
        /// </summary>
        [DefaultValue(eLabelPartAlignment.MiddleRight), Description("Indicates image alignment.")]
        public eLabelPartAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                if (_ImageAlign != value)
                {
                    _ImageAlign = value;
                    this.Refresh();
                }
            }
        }

        private Image _Image = null;
        /// <summary>
        /// Gets or sets the image displayed on the label.
        /// </summary>
        [DefaultValue(null), Description("Indicates image displayed on the label.")]
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    _ImageRenderBounds = Rectangle.Empty;
                    this.Refresh();
                }
            }
        }

        private bool _FlatOffice2007Style = false;
        /// <summary>
        /// Gets or sets whether flat Office 2007 style is used to render the item. Default value is false.
        /// </summary>
        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool FlatOffice2007Style
        {
            get { return _FlatOffice2007Style; }
            set
            {
                _FlatOffice2007Style = value;
                this.Refresh();
            }
        }

        private bool _IsToday = false;
        /// <summary>
        /// Gets or sets whether date represented by label is marked as todays date.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsToday
        {
            get { return _IsToday; }
            set
            {
                if (_IsToday != value)
                {
                    _IsToday = value;
                    this.Refresh();
                }
            }
        }

        private bool _ExpandOnMouseDown = false;
        /// <summary>
        /// Gets or sets whether popup is displayed when mouse is pressed anywhere over the item. Default value is false which indicates
        /// that popup is displayed only if image assigned to the item and mouse is pressed over image.
        /// </summary>
        [DefaultValue(false), Description("Indicates whether popup is displayed when mouse is pressed anywhere over the item.")]
        public bool ExpandOnMouseDown
        {
            get { return _ExpandOnMouseDown; }
            set
            {
                if (_ExpandOnMouseDown != value)
                {
                    _ExpandOnMouseDown = value;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Indicates the alignment of the DayLabel part like text or image.
    /// </summary>
    public enum eLabelPartAlignment
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}
#endif

