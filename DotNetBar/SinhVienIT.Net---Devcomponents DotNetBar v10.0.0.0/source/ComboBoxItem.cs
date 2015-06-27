using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Summary description for ComboBoxItem.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false), DefaultEvent("Click"), System.ComponentModel.DesignTimeVisible(false), Designer("DevComponents.DotNetBar.Design.ComboBoxItemDesigner, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public class ComboBoxItem:ImageItem,IPersonalizedMenuItem
	{
		private Controls.ComboBoxEx m_ComboBox=null;
		private int m_ComboWidth=64;
		private bool m_MouseOver=false;
		//private string m_ControlText="";
		private bool m_AlwaysShowCaption;
		private bool m_FontCombo=false;

		// IPersonalizedMenuItem Implementation
		private eMenuVisibility m_MenuVisibility=eMenuVisibility.VisibleAlways;
		private bool m_RecentlyUsed=false;
		internal bool _CopyInProgress=false;

		private bool m_PreventEnterBeep=false;

        /// <summary>
        /// Occurs when underlining control ComboBox.Text property has changed.
        /// </summary>
        public event EventHandler ComboBoxTextChanged;

        /// <summary>
        /// Occurs when selected item on combo box has changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

		/// <summary>
		/// Creates new instance of ComboBoxItem.
		/// </summary>
		public ComboBoxItem():this("","") {}
		/// <summary>
		/// Creates new instance of ComboBoxItem and assigns item name.
		/// </summary>
		/// <param name="sName">Item Name.</param>
		public ComboBoxItem(string sName):this(sName,""){}
		/// <summary>
		/// Creates new instance of ComboBoxItem and assigns item name and item text.
		/// </summary>
		/// <param name="sName">Item Name</param>
		/// <param name="ItemText">Item Text.</param>
		public ComboBoxItem(string sName, string ItemText):base(sName,ItemText)
		{
			CreateComboBox();
			m_ComboWidth=64;
			m_SupportedOrientation=eSupportedOrientation.Horizontal;
			this.IsAccessible=true;
            this.AccessibleRole = AccessibleRole.ComboBox;
		}
		private void CreateComboBox()
		{
			if(m_ComboBox!=null)
			{
				m_ComboBox.Dispose();
				m_ComboBox=null;
			}
            m_ComboBox = new Controls.ComboBoxEx();
            m_ComboBox.IsStandalone = false;
            m_ComboBox.TabStop = false;
            m_ComboBox.TabIndex = 9999;
            m_ComboBox.Style = this.Style;
            m_ComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            m_ComboBox.IntegralHeight = false;
            //m_ComboBox.ItemHeight = 13;
            m_ComboBox.ThemeAware = false;
            m_ComboBox.Visible = false;
            m_ComboBox.Text = this.Text;
            m_ComboBox.SelectionStart = 0;
            m_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            m_ComboBox.LostFocus += new EventHandler(this.ComboLostFocus);
            m_ComboBox.GotFocus += new EventHandler(this.ComboGotFocus);
            m_ComboBox.MouseHover += new EventHandler(this.ComboMouseHover);
            m_ComboBox.MouseEnter += new EventHandler(this.ComboMouseEnter);
            m_ComboBox.MouseLeave += new EventHandler(this.ComboMouseLeave);
            m_ComboBox.VisibleChanged += new EventHandler(ComboBoxVisibleChanged);
            m_ComboBox.DropDownChange += new Controls.ComboBoxEx.OnDropDownChangeEventHandler(this.ComboDropDownChange);
            m_ComboBox.TextChanged += new EventHandler(this.InternalComboTextChanged);
            m_ComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ComboKeyDown);
            m_ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboSelChanged);
            m_ComboBox.PreventEnterBeep = m_PreventEnterBeep;
            m_ComboBox.ParentItem = this;
			if(m_FontCombo)
				m_ComboBox.LoadFonts();

			if(this.ContainerControl!=null)
			{
				System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
				if(objCtrl!=null)
				{
					objCtrl.Controls.Add(m_ComboBox);
					m_ComboBox.Refresh();
				}
			}

			if(this.Displayed)
			{
				m_ComboBox.Visible=true;
			}
		}

		public override BaseItem Copy()
		{
			ComboBoxItem objCopy=new ComboBoxItem(this.Name);
			this.CopyToItem(objCopy);
			objCopy.DropDownStyle=this.DropDownStyle;
			objCopy.AlwaysShowCaption=this.AlwaysShowCaption;
			objCopy.FontCombo=this.FontCombo;
			objCopy.ItemHeight=this.ItemHeight;
			return objCopy;
		}
		protected override void CopyToItem(BaseItem copy)
		{
			((ComboBoxItem)copy)._CopyInProgress=true;
			try
			{
				ComboBoxItem objCopy=copy as ComboBoxItem;
				base.CopyToItem(objCopy);

				
				objCopy.ComboWidth=m_ComboWidth;
				objCopy.FontCombo=m_FontCombo;
				objCopy.PreventEnterBeep=m_PreventEnterBeep;
                if (m_ComboBox != null)
                {
                    objCopy.DisplayMember = m_ComboBox.DisplayMember;

                    Controls.ComboBoxEx cb = objCopy.ComboBoxEx;
                    if (!m_FontCombo)
                    {
                        foreach (object o in m_ComboBox.Items)
                            cb.Items.Add(o);
                    }
                    cb.SelectedIndex = m_ComboBox.SelectedIndex;
                }
			}
			finally
			{
				((ComboBoxItem)copy)._CopyInProgress=false;
			}
		}
        protected override void Dispose(bool disposing)
		{
            base.Dispose(disposing);
            // Control gets disposed by the parent control it is added to
            //if (m_ComboBox != null)
            //{
            //    if (m_ComboBox.Parent == null && !m_ComboBox.IsDisposed)
            //        m_ComboBox.Dispose();
            //}
		}
        protected override System.Windows.Forms.AccessibleObject CreateAccessibilityInstance()
        {
            return m_ComboBox.AccessibilityObject;
        }
        /// <summary>
        /// Gets or sets the accessible role of the item.
        /// </summary>
        [Browsable(true), Category("Accessibility"), Description("Gets or sets the accessible role of the item."), DefaultValue(System.Windows.Forms.AccessibleRole.ComboBox)]
        public override System.Windows.Forms.AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole; }
            set { base.AccessibleRole = value; }
        }
		protected internal override void Serialize(ItemSerializationContext context)
		{
			base.Serialize(context);
            System.Xml.XmlElement ThisItem = context.ItemXmlElement;
			ThisItem.SetAttribute("ComboWidth",System.Xml.XmlConvert.ToString(m_ComboWidth));
			ThisItem.SetAttribute("FontCombo",System.Xml.XmlConvert.ToString(m_FontCombo));

			ThisItem.SetAttribute("MenuVisibility",System.Xml.XmlConvert.ToString((int)m_MenuVisibility));
			ThisItem.SetAttribute("RecentlyUsed",System.Xml.XmlConvert.ToString(m_RecentlyUsed));
			ThisItem.SetAttribute("DropDownStyle",System.Xml.XmlConvert.ToString((int)m_ComboBox.DropDownStyle));
			//ThisItem.SetAttribute("CText",m_ControlText);
			ThisItem.SetAttribute("ThemeAware",System.Xml.XmlConvert.ToString(m_ComboBox.ThemeAware));

			ThisItem.SetAttribute("AlwaysShowCaption",System.Xml.XmlConvert.ToString(m_AlwaysShowCaption));
			ThisItem.SetAttribute("ItemHeight",System.Xml.XmlConvert.ToString(m_ComboBox.ItemHeight));

            if(m_ComboBox.DisplayMember!="")
                ThisItem.SetAttribute("DisplayMembers", m_ComboBox.DisplayMember);

			if(m_PreventEnterBeep)
				ThisItem.SetAttribute("nobeep",System.Xml.XmlConvert.ToString(m_PreventEnterBeep));
			
			if(!m_FontCombo && m_ComboBox.Items.Count>0)
			{
				System.Xml.XmlElement xmlItems=ThisItem.OwnerDocument.CreateElement("cbitems");
				ThisItem.AppendChild(xmlItems);
				foreach(object item in m_ComboBox.Items)
				{
					DevComponents.Editors.ComboItem ci=item as DevComponents.Editors.ComboItem;
					if(ci!=null)
					{
						System.Xml.XmlElement xmlChild=ThisItem.OwnerDocument.CreateElement("ci");

						if(!ci.BackColor.IsEmpty)
							xmlChild.SetAttribute("bc",BarFunctions.ColorToString(ci.BackColor));
						if(ci.FontName!="")
							xmlChild.SetAttribute("fn",ci.FontName);
						if(ci.FontSize!=8)
							xmlChild.SetAttribute("fs",System.Xml.XmlConvert.ToString(ci.FontSize));
						
						xmlChild.SetAttribute("fy",System.Xml.XmlConvert.ToString((int)ci.FontStyle));

						if(!ci.ForeColor.IsEmpty)
							xmlChild.SetAttribute("fc",BarFunctions.ColorToString(ci.ForeColor));

						BarFunctions.SerializeImage(ci.Image,xmlChild);

						if(ci.ImageIndex>=0)
							xmlChild.SetAttribute("img",System.Xml.XmlConvert.ToString(ci.ImageIndex));

						if(ci.ImagePosition!=System.Windows.Forms.HorizontalAlignment.Left)
							xmlChild.SetAttribute("ip",System.Xml.XmlConvert.ToString((int)ci.ImagePosition));
						
						xmlChild.SetAttribute("text",ci.Text);

						xmlChild.SetAttribute("ta",System.Xml.XmlConvert.ToString((int)ci.TextAlignment));
						xmlChild.SetAttribute("tla",System.Xml.XmlConvert.ToString((int)ci.TextLineAlignment));
						
						if(m_ComboBox.SelectedItem==item)
							xmlChild.SetAttribute("selected","1");

						xmlItems.AppendChild(xmlChild);
					}
					else
					{
						System.Xml.XmlElement xmlChild=ThisItem.OwnerDocument.CreateElement("co");
						xmlChild.InnerText=item.ToString();
						xmlItems.AppendChild(xmlChild);

						if(m_ComboBox.SelectedItem==item)
							xmlChild.SetAttribute("selected","1");
					}	
				}
			}
		}

		public override void Deserialize(ItemSerializationContext context)
		{
			base.Deserialize(context);
            System.Xml.XmlElement ItemXmlSource = context.ItemXmlElement;
			m_ComboWidth=System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("ComboWidth"));
			m_FontCombo=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("FontCombo"));

			m_MenuVisibility=(eMenuVisibility)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("MenuVisibility"));
			m_RecentlyUsed=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("RecentlyUsed"));
			if(ItemXmlSource.HasAttribute("DropDownStyle"))
				m_ComboBox.DropDownStyle=(System.Windows.Forms.ComboBoxStyle)System.Xml.XmlConvert.ToInt32(ItemXmlSource.GetAttribute("DropDownStyle"));

			if(ItemXmlSource.HasAttribute("CText"))
				this.Text=ItemXmlSource.GetAttribute("CText");

			if(ItemXmlSource.HasAttribute("ThemeAware"))
				m_ComboBox.ThemeAware=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("ThemeAware"));
			else
                m_ComboBox.ThemeAware=true;

			if(ItemXmlSource.HasAttribute("AlwaysShowCaption"))
				m_AlwaysShowCaption=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("AlwaysShowCaption"));

            if(ItemXmlSource.HasAttribute("nobeep"))
				this.PreventEnterBeep=System.Xml.XmlConvert.ToBoolean(ItemXmlSource.GetAttribute("nobeep"));

			System.Xml.XmlNodeList list=ItemXmlSource.GetElementsByTagName("cbitems");
			if(!m_FontCombo && list.Count>0)
			{
				foreach(System.Xml.XmlElement xmlChild in list[0].ChildNodes)
				{
					if(xmlChild.Name=="ci")
					{
						DevComponents.Editors.ComboItem ci=new DevComponents.Editors.ComboItem();
						if(xmlChild.HasAttribute("bc"))
							ci.BackColor=BarFunctions.ColorFromString(xmlChild.GetAttribute("bk"));
						if(xmlChild.HasAttribute("fn"))
							ci.FontName=xmlChild.GetAttribute("fn");
						if(xmlChild.HasAttribute("fs"))
							ci.FontSize=System.Xml.XmlConvert.ToSingle(xmlChild.GetAttribute("fs"));
						if(xmlChild.HasAttribute("fy"))
							ci.FontStyle=(FontStyle)System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("fy"));
						if(xmlChild.HasAttribute("fc"))
							ci.ForeColor=BarFunctions.ColorFromString(xmlChild.GetAttribute("fc"));
						
						ci.Image=BarFunctions.DeserializeImage(xmlChild);

						if(xmlChild.HasAttribute("img"))
							ci.ImageIndex=System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("img"));

						if(xmlChild.HasAttribute("ip"))
							ci.ImagePosition=(System.Windows.Forms.HorizontalAlignment)System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("ip"));

						if(xmlChild.HasAttribute("ItemHeight"))
							m_ComboBox.ItemHeight=System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("ItemHeight"));
						
						ci.Text=xmlChild.GetAttribute("text");

						ci.TextAlignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("ta"));
						ci.TextLineAlignment=(StringAlignment)System.Xml.XmlConvert.ToInt32(xmlChild.GetAttribute("tla"));

						m_ComboBox.Items.Add(ci);

						if(xmlChild.HasAttribute("selected") && xmlChild.GetAttribute("selected")=="1")
							m_ComboBox.SelectedItem=ci;
					}
					else if(xmlChild.Name=="co")
					{
						m_ComboBox.Items.Add(xmlChild.InnerText);
						if(xmlChild.HasAttribute("selected") && xmlChild.GetAttribute("selected")=="1")
							m_ComboBox.SelectedItem=m_ComboBox.Items[m_ComboBox.Items.Count-1];
					}
				}
			}
			if(m_FontCombo)
				m_ComboBox.LoadFonts();

			if(m_ComboBox!=null)
				m_ComboBox.Enabled=this.Enabled;

            if (ItemXmlSource.HasAttribute("DisplayMembers") && m_ComboBox!=null)
                m_ComboBox.DisplayMember = ItemXmlSource.GetAttribute("DisplayMembers");
		}

		/// <summary>
		/// Gets or sets whether combo box generates the audible alert when Enter key is pressed.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether combo box generates the audible alert when Enter key is pressed."),System.ComponentModel.DefaultValue(false)]
		public bool PreventEnterBeep
		{
			get
			{
				return m_PreventEnterBeep;
			}
			set
			{
				m_PreventEnterBeep=value;
				if(m_ComboBox!=null)
					m_ComboBox.PreventEnterBeep=value;
			}
		}

        private Color _LabelForeColor = Color.Empty;
        /// <summary>
        /// Gets or sets the text color of the combo box label.
        /// </summary>
        [Category("Columns"), Description("Indicates color of combo box label.")]
        public Color LabelForeColor
        {
            get { return _LabelForeColor; }
            set { _LabelForeColor = value; this.Refresh(); }
        }
        /// <summary>
        /// Gets whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeLabelForeColor()
        {
            return !_LabelForeColor.IsEmpty;
        }
        /// <summary>
        /// Resets property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetLabelForeColor()
        {
            this.LabelForeColor = Color.Empty;
        }

		public override void Paint(ItemPaintArgs pa)
		{
			if(this.SuspendLayout)
				return;
			System.Drawing.Graphics g=pa.Graphics;
			Rectangle r=this.DisplayRectangle;
			Size objImageSize=GetMaxImageSize();
            bool bOnMenu = this.IsOnMenu && !(this.Parent is ItemContainer);
            bool enabled = GetEnabled(pa.ContainerControl);
            Color textColor = enabled ? SystemColors.ControlText : SystemColors.ControlDark;
            if (BarFunctions.IsOffice2007Style(this.EffectiveStyle) && pa.Renderer is Rendering.Office2007Renderer &&
                ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors.Count > 0)
            {
                textColor = enabled ? ((Rendering.Office2007Renderer)pa.Renderer).ColorTable.ButtonItemColors[0].Default.Text : pa.Colors.ItemDisabledText;
            }
            if (!_LabelForeColor.IsEmpty)
                textColor = _LabelForeColor;

			if(this.Orientation==eOrientation.Horizontal)
			{
                if (bOnMenu && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
				{
					objImageSize.Width+=7;
					r.Width-=objImageSize.Width;
					r.X+=objImageSize.Width;
					if(this.IsOnCustomizeMenu)
						objImageSize.Width+=objImageSize.Height+8;
					// Draw side bar
                    Rectangle sideRect = new Rectangle(m_Rect.Left, m_Rect.Top, objImageSize.Width, m_Rect.Height);
					if(this.MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed && !this.RecentlyUsed)
					{
						if(!pa.Colors.MenuUnusedSide2.IsEmpty)
						{
							System.Drawing.Drawing2D.LinearGradientBrush gradient=BarFunctions.CreateLinearGradientBrush(sideRect,pa.Colors.MenuUnusedSide,pa.Colors.MenuUnusedSide2,pa.Colors.MenuUnusedSideGradientAngle);
							g.FillRectangle(gradient,sideRect);
							gradient.Dispose();
						}
						else
                            g.FillRectangle(new SolidBrush(pa.Colors.MenuUnusedSide), sideRect);
					}
					else
					{
						if(!pa.Colors.MenuSide2.IsEmpty)
						{
                            System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
                            g.FillRectangle(gradient, sideRect);
							gradient.Dispose();
						}
						else
                            g.FillRectangle(new SolidBrush(pa.Colors.MenuSide), sideRect);
					}
                    if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
                    {
                        Office2007MenuColorTable mt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
                        if (mt != null && !mt.SideBorder.IsEmpty)
                        {
                            if (pa.RightToLeft)
                                DisplayHelp.DrawGradientLine(g, sideRect.X + 1, sideRect.Y, sideRect.X + 1, sideRect.Bottom - 1, mt.SideBorder, 1);
                            else
                                DisplayHelp.DrawGradientLine(g, sideRect.Right - 2, sideRect.Y, sideRect.Right - 2, sideRect.Bottom - 1, mt.SideBorder, 1);
                        }
                        if (mt != null && !mt.SideBorderLight.IsEmpty)
                        {
                            if (pa.RightToLeft)
                                DisplayHelp.DrawGradientLine(g, sideRect.X, sideRect.Y, sideRect.X, sideRect.Bottom - 1, mt.SideBorder, 1);
                            else
                                DisplayHelp.DrawGradientLine(g, sideRect.Right - 1, sideRect.Y, sideRect.Right - 1, sideRect.Bottom - 1, mt.SideBorderLight, 1);
                        }
                    }
				}

				if(this.IsOnCustomizeMenu)
				{
                    if (this.EffectiveStyle == eDotNetBarStyle.OfficeXP || this.EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
					{
						r.X+=(objImageSize.Height+8);
						r.Width-=(objImageSize.Height+8);
					}
					else
					{
						r.X+=(objImageSize.Height+4);
						r.Width-=(objImageSize.Height+4);
					}
				}

                if (bOnMenu && (this.EffectiveStyle == eDotNetBarStyle.OfficeXP || this.EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
				{
					if(m_MouseOver)
					{
						Rectangle rHover=this.DisplayRectangle;
						rHover.Inflate(-1,0);
                        if (BarFunctions.IsOffice2007Style(EffectiveStyle) && !(pa.Owner is DotNetBarManager) && GlobalManager.Renderer is Office2007Renderer)
                        {
                            Office2007ButtonItemStateColorTable bt = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ButtonItemColors[0].MouseOver;
                            Office2007ButtonItemPainter.PaintBackground(g, bt, rHover, RoundRectangleShapeDescriptor.RoundCorner3);
                        }
                        else
                        {
                            if (!pa.Colors.ItemHotBackground2.IsEmpty)
                            {
                                System.Drawing.Drawing2D.LinearGradientBrush gradient = BarFunctions.CreateLinearGradientBrush(rHover, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
                                g.FillRectangle(gradient, rHover);
                                gradient.Dispose();
                            }
                            else
                                g.FillRectangle(new SolidBrush(pa.Colors.ItemHotBackground), rHover);
                            NativeFunctions.DrawRectangle(g, new Pen(pa.Colors.ItemHotBorder), rHover);
                        }
					}
				}

				// Draw text if needed
                if (Caption != "" && (m_AlwaysShowCaption || bOnMenu))
				{
					eTextFormat objStringFormat=GetStringFormat();
					Font objFont=this.GetFont();
					Rectangle rText=new Rectangle(r.X+8,r.Y,r.Width,r.Height);

                    if (EffectiveStyle == eDotNetBarStyle.Office2000)
					{
                        TextDrawing.DrawString(g, Caption, objFont, textColor, rText, objStringFormat);
					}
					else
					{
                        TextDrawing.DrawString(g, Caption, objFont, textColor, rText, objStringFormat);
					}
                    Size textSize = TextDrawing.MeasureString(g, Caption, objFont, 0, objStringFormat);
					r.X+=(int)textSize.Width+8;
					r.Width-=((int)textSize.Width+8);
				}

				if(m_ComboBox==null || this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode)
				{
					r.Inflate(-3,-2);
					g.FillRectangle(SystemBrushes.Window,r);
					NativeFunctions.DrawRectangle(g,SystemPens.ControlDark,r);
					r.X=r.Right-(System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth-2);
					r.Width=System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth-2;
					System.Windows.Forms.ControlPaint.DrawComboButton(g,r,System.Windows.Forms.ButtonState.Flat);
				}
				else
				{
                    int selLength = m_ComboBox.SelectionLength;
                    r.Inflate(-2,-2);
                    if(m_ComboBox.Width!=r.Width)
					    m_ComboBox.Width=r.Width;
					Point loc=r.Location;
					loc.Offset((r.Width-m_ComboBox.Width)/2,(r.Height-m_ComboBox.Height)/2);

                    ScrollableControl scc = pa.ContainerControl as ScrollableControl;
                    if (scc != null && scc.AutoScroll)
                        loc.Offset(scc.AutoScrollPosition.X, scc.AutoScrollPosition.Y);
                    if (m_ComboBox.Location != loc)
                        m_ComboBox.Location = loc;
                    
                    if (selLength > 0 && selLength < 1000 && m_ComboBox.SelectionLength != selLength && m_ComboBox.Text.Length > selLength && m_ComboBox.DropDownStyle == ComboBoxStyle.DropDown)
                    {
                        m_ComboBox.SelectionLength = selLength;
                    }
				}

				if(this.IsOnCustomizeMenu && this.Visible)
				{
					// Draw check box if this item is visible
					Rectangle rBox=new Rectangle(m_Rect.Left,m_Rect.Top,m_Rect.Height,m_Rect.Height);
                    if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
						rBox.Inflate(-1,-1);
                    BarFunctions.DrawMenuCheckBox(pa, rBox, EffectiveStyle, m_MouseOver);
				}
			}
			else
			{
				string Caption=this.Text;
				if(Caption=="")
					Caption="...";
				else
					Caption+=" »";

                if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
					g.FillRectangle(new SolidBrush(ColorFunctions.ToolMenuFocusBackColor(g)),this.DisplayRectangle);
				else
					g.FillRectangle(SystemBrushes.Control,this.DisplayRectangle);

				if(m_MouseOver && !this.DesignMode)
				{
                    if (EffectiveStyle == eDotNetBarStyle.Office2000)
					{
						//r.Inflate(-1,-1);
						System.Windows.Forms.ControlPaint.DrawBorder3D(g,r,System.Windows.Forms.Border3DStyle.RaisedInner,System.Windows.Forms.Border3DSide.All);
					}
                    else if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
					{
						r.Inflate(-1,-1);
						g.FillRectangle(new SolidBrush(ColorFunctions.HoverBackColor(g)),r);
						NativeFunctions.DrawRectangle(g,SystemPens.Highlight,r);
					}
				}

				r=new Rectangle(m_Rect.Top,-m_Rect.Right,m_Rect.Height,m_Rect.Width);
				g.RotateTransform(90);
				eTextFormat sf=GetStringFormat();
                sf |= eTextFormat.HorizontalCenter;
                TextDrawing.DrawStringLegacy(g, Caption, GetFont(), textColor, r, sf);
				g.ResetTransform();
			}

			if(this.Focused && this.DesignMode)
			{
				r=this.DisplayRectangle;
				r.Inflate(-1,-1);
				DesignTime.DrawDesignTimeSelection(g,r,pa.Colors.ItemDesignTimeBorder);
			}

			//if(this.DesignMode)
			this.DrawInsertMarker(g);
		}

        /// <summary>
        /// IBlock member implementation
        /// </summary>
        [Browsable(false), DevCoBrowsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Drawing.Rectangle Bounds
        {
            get { return base.Bounds; }
            set
            {
                if (base.Bounds != value)
                {
                    bool topLocationChanged = base.Bounds.Top != value.Top;
                    bool leftLocationChanged = base.Bounds.Left != value.Left;
                    base.Bounds = value;
                    if ((topLocationChanged || leftLocationChanged) && this.ContainerControl is ItemPanel)
                        UpdateControlLocation();
                }
            }
        }

        private void UpdateControlLocation()
        {
            if (m_ComboBox == null) return;
            Rectangle r = this.DisplayRectangle;
            r.Inflate(-2, -2);
            m_ComboBox.Location = r.Location;
        }

        public override void RecalcSize()
        {
            if (this.SuspendLayout)
                return;

            bool bOnMenu = this.IsOnMenu;

            if (this.Orientation == eOrientation.Horizontal)
            {
                // Default Height
                if (this.Parent != null && this.Parent is ImageItem)
                    m_Rect.Height = ((ImageItem)this.Parent).SubItemsImageSize.Height + 4;
                else
                    m_Rect.Height = this.SubItemsImageSize.Height + 4;

                eDotNetBarStyle effectiveStyle = EffectiveStyle;
                if (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
                {
                    if (m_ComboBox != null && m_Rect.Height < (m_ComboBox.Height + 2))
                        m_Rect.Height = m_ComboBox.Height + 2;
                }
                else
                {
                    if (m_ComboBox != null && m_Rect.Height < (m_ComboBox.Height + 2))
                        m_Rect.Height = m_ComboBox.Height + 2;
                }

                // Default width
                m_Rect.Width = m_ComboWidth + 4;

                // Calculate Item Height
                if (Caption != "" && (m_AlwaysShowCaption || bOnMenu))
                {
                    System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                    if (objCtrl != null && IsHandleValid(objCtrl))
                    {
                        Graphics g = BarFunctions.CreateGraphics(objCtrl);
                        try
                        {
                            Size textSize = Size.Empty;
                            if (m_Orientation == eOrientation.Vertical && !bOnMenu)
                                textSize = TextDrawing.MeasureStringLegacy(g, Caption, GetFont(), Size.Empty, GetStringFormat());
                            else
                                textSize = TextDrawing.MeasureString(g, Caption, GetFont(), 0, GetStringFormat());
                            if (textSize.Height > this.SubItemsImageSize.Height && textSize.Height > m_Rect.Height)
                                m_Rect.Height = (int)textSize.Height + 4;
                            m_Rect.Width = m_ComboWidth + 4 + (int)textSize.Width + 8;
                        }
                        finally
                        {
                            g.Dispose();
                        }
                    }
                }

                Size objImageSize = GetMaxImageSize();
                if (this.IsOnMenu && (effectiveStyle == eDotNetBarStyle.OfficeXP || effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle)))
                {
                    // THis is side bar that will need to be drawn for DotNet style
                    m_Rect.Width += (objImageSize.Width + 7);
                }

                if (this.IsOnCustomizeMenu)
                    m_Rect.Width += (objImageSize.Height + 2);
            }
            else
            {
                // Default width
                m_Rect.Width = this.SubItemsImageSize.Width + 4;
                string caption = this.Caption;
                if (caption == "")
                    caption = "...";
                else
                    caption += " »";
                System.Windows.Forms.Control objCtrl = this.ContainerControl as System.Windows.Forms.Control;
                if (objCtrl != null && IsHandleValid(objCtrl))
                {
                    Graphics g = BarFunctions.CreateGraphics(objCtrl);
                    try
                    {
                        SizeF textSize = TextDrawing.MeasureString(g, caption, GetFont(), 0, GetStringFormat());
                        if (textSize.Height > this.SubItemsImageSize.Height)
                            m_Rect.Width = (int)textSize.Height + 4;
                        m_Rect.Height = (int)textSize.Width + 8;
                    }
                    finally
                    {
                        g.Dispose();
                    }
                }

            }

            // Always call base implementation to reset resize flag
            base.RecalcSize();
        }
		protected internal override void OnContainerChanged(object objOldContainer)
		{	
			base.OnContainerChanged(objOldContainer);
			if(m_ComboBox!=null)
			{
				if(m_ComboBox.Parent!=null)
				{
					//bool bVisible=false;
					if(m_ComboBox.Visible)
					{
						m_ComboBox.Visible=false;
						//bVisible=true;
					}
					System.Windows.Forms.Control parent=m_ComboBox.Parent;
					parent.Controls.Remove(m_ComboBox);
					//if(bVisible)
						//m_ComboBox.Visible=true;
				}

				System.Windows.Forms.Control objCtrl=null;
				if(this.ContainerControl!=null)
				{
					objCtrl=this.ContainerControl as System.Windows.Forms.Control;
					if(objCtrl!=null)
					{
						objCtrl.Controls.Add(m_ComboBox);
						OnDisplayedChanged();
						m_ComboBox.Refresh();
					}
				}
			}
		}
		protected internal override void OnAfterItemRemoved(BaseItem item)
		{
			base.OnBeforeItemRemoved(item);
			this.ContainerControl=null;
		}
		protected internal override void OnVisibleChanged(bool newValue)
		{
			if(m_ComboBox!=null && !newValue)
				m_ComboBox.Visible=newValue;
			base.OnVisibleChanged(newValue);
		}
		protected override void OnDisplayedChanged()
		{
			if(m_ComboBox!=null && !(this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode))
			{
				m_ComboBox.Visible=this.Displayed;
			}
		}
        [EditorBrowsable(EditorBrowsableState.Never)]
		public override void OnGotFocus()
		{
			base.OnGotFocus();
			if(m_ComboBox==null)
				return;
			if(m_ComboBox.Focused || this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode)
				return;
			m_ComboBox.Focus();
		}

		private void ComboLostFocus(object sender, EventArgs e)
		{
			this.HideToolTip();
			this.Text=m_ComboBox.Text;
			this.ReleaseFocus();
			if(!m_MouseOver)
				return;
			m_MouseOver=false;
			this.Refresh();
		}

		private void ComboMouseHover(object sender, EventArgs e)
		{
			if(this.DesignMode)
				return;
			if(System.Windows.Forms.Control.MouseButtons==System.Windows.Forms.MouseButtons.None)
				ShowToolTip();
		}

		private void ComboMouseEnter(object sender, EventArgs e)
		{
			if(!m_MouseOver)
			{
				m_MouseOver=true;			
				this.Refresh();
			}
		}

		private void ComboMouseLeave(object sender, EventArgs e)
		{
			this.HideToolTip();
			if(m_ComboBox.Focused)
				return;
			if(m_MouseOver)
			{
				m_MouseOver=false;
				this.Refresh();
			}
		}
        private void ComboBoxVisibleChanged(object sender, EventArgs e)
        {
            this.HideToolTip();
        }

		private void ComboGotFocus(object sender, EventArgs e)
		{
			this.HideToolTip();
			this.Focus();

            if (GetEnabled() && !this.DesignMode)
			{
				if(m_MenuVisibility==eMenuVisibility.VisibleIfRecentlyUsed && !m_RecentlyUsed && this.IsOnMenu)
				{
					// Propagate to the top
					m_RecentlyUsed=true;
					BaseItem objItem=this.Parent;
					while(objItem!=null)
					{
						IPersonalizedMenuItem ipm=objItem as IPersonalizedMenuItem;
						if(ipm!=null)
							ipm.RecentlyUsed=true;
						objItem=objItem.Parent;
					}
				}
			}
		}

		private void ComboSelChanged(object sender, EventArgs e)
		{
            if (!_CopyInProgress)
            {
                this.RaiseClick();
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, e);
                if (ShouldSyncProperties)
                    BarFunctions.SyncProperty(this, "SelectedIndex");
                ExecuteCommand();
            }
            
		}

		private void ComboKeyDown(object sender,System.Windows.Forms.KeyEventArgs e)
		{
			this.HideToolTip();
			if(e.KeyCode==System.Windows.Forms.Keys.Enter && this.Parent is PopupItem)
			{
				((PopupItem)this.Parent).ClosePopup();
			}
		}
		private void ComboDropDownChange(object sender,bool bExpanded)
		{
			this.Expanded=bExpanded;
			if(!bExpanded)
				this.ReleaseFocus();
		}
		protected override void OnIsOnCustomizeDialogChanged()
		{
			base.OnIsOnCustomizeDialogChanged();
			CustomizeChanged();
		}

		protected override void OnDesignModeChanged()
		{
			base.OnDesignModeChanged();
			CustomizeChanged();
		}

		protected override void OnIsOnCustomizeMenuChanged()
		{
			base.OnIsOnCustomizeMenuChanged();
			CustomizeChanged();
		}

		private void CustomizeChanged()
		{
			if(this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.DesignMode)
			{
				m_ComboBox.Visible=false;
			}
			else
			{
				m_ComboBox.Visible=this.Displayed;
			}
		}

        /// <summary>
        /// Gets or sets the width of the of the drop-down portion of a combo box. 
        /// </summary>
        [Category("Layout"), Browsable(true), Description("Indicates width of the of the drop-down portion of a combo box.")]
        public int DropDownWidth
        {
            get { return m_ComboBox.DropDownWidth; }
            set { m_ComboBox.DropDownWidth = value; }
        }

        /// <summary>
        /// Returns whether property should be serialized.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeDropDownWidth()
        {
            return m_ComboBox.Width != m_ComboBox.DropDownWidth;
        }

        /// <summary>
        /// Gets or sets the height of the of the drop-down portion of a combo box. 
        /// </summary>
        [Category("Layout"), Browsable(true), DefaultValue(0), Description("Indicates height of the of the drop-down portion of a combo box.")]
        public int DropDownHeight
        {
            get { return m_ComboBox.DropDownHeight; }
            set { m_ComboBox.DropDownHeight = value; }
        }

		/// <summary>
		/// Indicates the Width of the combo box part of the item.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Layout"),System.ComponentModel.Description("Indicates the Width of the combo box part of the item."),System.ComponentModel.DefaultValue(64)]
		public int ComboWidth
		{
			get
			{
				return m_ComboWidth;
			}
			set
			{
				if(m_ComboWidth!=value)
				{
					m_ComboWidth=value;
					if(this.Name!="" && this.GlobalItem)
					{
						BarFunctions.SetProperty(this.GetOwner(),this.GetType(),m_Name,System.ComponentModel.TypeDescriptor.GetProperties(this)["ComboWidth"],m_ComboWidth);
					}
					NeedRecalcSize=true;
					this.Refresh();
				}
				OnAppearanceChanged();
			}
		}

		/// <summary>
		/// Overridden. Releases the input focus.
		/// </summary>
		public override void ReleaseFocus()
		{
			if(m_ComboBox!=null && m_ComboBox.Focused)
				m_ComboBox.ReleaseFocus();
			base.ReleaseFocus();
		}

		/// <summary>
		/// Returns the reference to the inner combo box control.
		/// </summary>
		[Browsable(false)]
		public ComboBoxEx ComboBoxEx
		{
			get
			{
				return m_ComboBox;
			}
		}

//		[System.ComponentModel.Browsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("The caption of the item.")]
//		public string Caption
//		{
//			get
//			{
//				return m_Caption;
//			}
//			set
//			{
//				if(m_Caption!=value)
//				{
//					m_Caption=value;
//					if(this.Name!="" && this.GlobalItem)
//					{
//						BarFunctions.SetProperty(this.GetOwner(),this.GetType(),m_Name,System.ComponentModel.TypeDescriptor.GetProperties(this)["Caption"],m_Caption);
//					}
//					m_NeedRecalcSize=true;
//					this.Refresh();
//				}
//			}
//		}

		/// <summary>
		/// Indicates whether item caption is always shown.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates whether item caption is always shown."), System.ComponentModel.DefaultValue(false)]
		public bool AlwaysShowCaption
		{
			get
			{
				return m_AlwaysShowCaption;
			}
			set
			{
				if(m_AlwaysShowCaption!=value)
				{
					m_AlwaysShowCaption=value;
				}
				NeedRecalcSize=true;
			}
		}
		private Size GetMaxImageSize()
		{
			if(m_Parent!=null)
			{
				ImageItem objParentImageItem=m_Parent as ImageItem;
				if(objParentImageItem!=null)
					return objParentImageItem.SubItemsImageSize;
				else
					return this.ImageSize;
			}
			else
				return this.ImageSize;
		}
        private eTextFormat GetStringFormat()
		{
            eTextFormat format = eTextFormat.Default;
            format |= eTextFormat.SingleLine;
            format |= eTextFormat.EndEllipsis;
            format |= eTextFormat.VerticalCenter;
            return format;
            //sfmt.HotkeyPrefix=System.Drawing.Text.HotkeyPrefix.Show;
			//sfmt.FormatFlags=sfmt.FormatFlags | StringFormatFlags.NoWrap;
            //sfmt.Trimming=StringTrimming.EllipsisCharacter;
            //sfmt.Alignment=System.Drawing.StringAlignment.Near;
            //sfmt.LineAlignment=System.Drawing.StringAlignment.Center;

			//return sfmt;
		}
		protected virtual Font GetFont()
		{
			System.Windows.Forms.Control objCtrl=this.ContainerControl as System.Windows.Forms.Control;
			if(objCtrl!=null)
				return (Font)objCtrl.Font;
			return (Font)System.Windows.Forms.SystemInformation.MenuFont;
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseEnter()
		{
			base.InternalMouseEnter();
			//if(this.DesignMode || this.IsOnCustomizeMenu || this.IsOnCustomizeDialog || this.Orientation==eOrientation.Vertical)
			if(!m_MouseOver)
			{
				m_MouseOver=true;
				this.Refresh();
			}
		}
		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void InternalMouseLeave()
		{
			base.InternalMouseLeave();
			if(m_MouseOver)
			{
				m_MouseOver=false;
				this.Refresh();
			}
		}
		protected internal override bool IsAnyOnHandle(int iHandle)
		{
			bool bRet=base.IsAnyOnHandle(iHandle);
			if(!bRet && m_ComboBox!=null && m_ComboBox.DroppedDown && m_ComboBox.DropDownHandle!=IntPtr.Zero && m_ComboBox.DropDownHandle.ToInt32()==iHandle)
				bRet=true;
			return bRet;
		}
		protected override void OnStyleChanged()
		{
			base.OnStyleChanged();
			m_ComboBox.Style=this.Style;
		}

		/// <summary>
		/// Gets an object representing the collection of the items contained in inner ComboBoxEx.
		/// </summary>
        [System.ComponentModel.Editor("DevComponents.DotNetBar.Design.ComboItemsEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor)), System.ComponentModel.Category("Data"), DevCoBrowsable(true), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public System.Windows.Forms.ComboBox.ObjectCollection Items
		{
			get
			{
				return m_ComboBox.Items;
				
			}
		}

		/// <summary>
		/// Gets or sets a value specifying the style of the combo box.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),DefaultValue(System.Windows.Forms.ComboBoxStyle.DropDownList),System.ComponentModel.Description("Gets or sets a value specifying the style of the combo box.")]
		public System.Windows.Forms.ComboBoxStyle DropDownStyle
		{
			get
			{
				return m_ComboBox.DropDownStyle;
			}
			set
			{
                m_ComboBox.DropDownStyle=value;
			}
		}

		/// <summary>
		/// Gets or sets the starting index of text selected in the combo box.
		/// </summary>
		[Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.DefaultValue(0)]
		public int SelectionStart
		{
			get
			{
				return m_ComboBox.SelectionStart;
			}
			set
			{
				m_ComboBox.SelectionStart=value;
			}
		}

		/// <summary>
		/// Gets or sets the number of characters selected in the editable portion of the combo box.
		/// </summary>
		[Browsable(false),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),System.ComponentModel.DefaultValue(0)]
		public int SelectionLength
		{
			get
			{
				return m_ComboBox.SelectionLength;
			}
			set
			{
				m_ComboBox.SelectionLength=value;
			}
		}

		/// <summary>
		/// Gets or sets the text that is selected in the editable portion of a combo box.
		/// </summary>
		[Browsable(false),System.ComponentModel.DefaultValue(""),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string SelectedText
		{
			get
			{
				return m_ComboBox.SelectedText;
			}
			set
			{
				m_ComboBox.SelectedText=value;
			}
		}

		/// <summary>
		/// Gets or sets currently selected item in the combo box.
		/// </summary>
		[Browsable(false),System.ComponentModel.DefaultValue(null),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return m_ComboBox.SelectedItem;
			}
			set
			{
				m_ComboBox.SelectedItem=value;
			}
		}

		/// <summary>
		/// Gets or sets the index specifying the currently selected item.
		/// </summary>
		[Browsable(false),System.ComponentModel.DefaultValue(-1),System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return m_ComboBox.SelectedIndex;
			}
			set
			{
				m_ComboBox.SelectedIndex=value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether the item automatically loads all the fonts available into the combo box.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Automatically loads all the fonts available into the combo box."),System.ComponentModel.DefaultValue(false)]
		public bool FontCombo
		{
			get
			{
				return m_FontCombo;
			}
			set
			{
				if(m_FontCombo!=value)
				{
					m_FontCombo=value;
					if(m_FontCombo)
					{
						m_ComboBox.LoadFonts();
						m_ComboBox.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
						m_ComboBox.DrawMode=System.Windows.Forms.DrawMode.OwnerDrawVariable;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the height of an item in the combo box.
		/// </summary>
		[DefaultValue(15), System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Behavior"),System.ComponentModel.Description("Indicates the height of an item in the combo box.")]
		public int ItemHeight
		{
			get
			{
				return m_ComboBox.ItemHeight;
			}
			set
			{
				m_ComboBox.ItemHeight=value;
			}
		}

		// IPersonalizedMenuItem Implementation
		/// <summary>
		/// Indicates item's visibility when on pop-up menu.
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Indicates item's visiblity when on pop-up menu."),System.ComponentModel.DefaultValue(eMenuVisibility.VisibleAlways)]
		public eMenuVisibility MenuVisibility
		{
			get
			{
				return m_MenuVisibility;
			}
			set
			{
				if(m_MenuVisibility!=value)
				{
					m_MenuVisibility=value;
					if(m_Name!="" && this.GlobalItem)
					{
						BarFunctions.SetProperty(this.GetOwner(),this.GetType(),m_Name,System.ComponentModel.TypeDescriptor.GetProperties(this)["MenuVisibility"],m_MenuVisibility);
					}
				}
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the item was recently used.
		/// </summary>
		[System.ComponentModel.Browsable(false),System.ComponentModel.DefaultValue(false)]
		public bool RecentlyUsed
		{
			get
			{
				return m_RecentlyUsed;
			}
			set
			{
				if(m_RecentlyUsed!=value)
				{
					m_RecentlyUsed=value;
					if(m_Name!="" && this.GlobalItem)
					{
						BarFunctions.SetProperty(this.GetOwner(),this.GetType(),m_Name,System.ComponentModel.TypeDescriptor.GetProperties(this)["RecentlyUsed"],m_RecentlyUsed);
					}
				}
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			if(m_ComboBox!=null)
				m_ComboBox.Enabled=this.Enabled;
		}

		private void InternalComboTextChanged(object sender, EventArgs e)
		{
            if(!_SettingText)
                this.Text = m_ComboBox.Text;
            OnComboBoxTextChanged(e);
		}

        /// <summary>
        /// Raises the ComboBoxTextChanged event.
        /// </summary>
        /// <param name="e">Provides event arguments.</param>
        protected virtual void OnComboBoxTextChanged(EventArgs e)
        {
            if (ComboBoxTextChanged != null)
                ComboBoxTextChanged(this, e);
        }

		/// <summary>
		/// Overridden. Gets or sets the text associated with this item.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Obsolete("Please use Text property instead to access ComboBox text.")]
		public virtual string ControlText
		{
			get
			{
				return this.Text;
			}
			set
			{
                this.Text = value;
            }
		}

        /// <summary>
        /// Gets or sets the text associated with this item.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("The text contained in the underlining Control portion of the item."), DefaultValue("")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        private bool _SettingText = false;
        protected override void OnTextChanged()
        {
            if (m_ComboBox != null && m_ComboBox.Text != this.Text)
            {
                try
                {
                    _SettingText = true;
                    m_ComboBox.Text = this.Text;
                }
                finally
                {
                    _SettingText = false;
                }
            }
            base.OnTextChanged();
        }

        private string _Caption = "";
        /// <summary>
        /// Gets or sets the item caption text displayed next to the combo box.
        /// </summary>
        [DefaultValue(""), System.ComponentModel.Category("Appearance"), Description("Indicates the item Caption displayed next to the combo box."), Localizable(true)]
        public string Caption
        {
            get { return _Caption; }
            set
            {
                if (value == null) value = "";
                _Caption = value;
            }
        }

        /// <summary>
        /// Gets or sets a string that specifies the property of the data source whose contents you want to display. When ComboBoxItem is used in DropDown mode
        /// and objects like ComboItem are added to ComboBoxItems.Items collection DisplayMembers should be set to the name of the property you would
        /// like to use as text representation in editable portion of ComboBox. For example in case of ComboItem objects property should be set to Text.
        /// </summary>
        [System.ComponentModel.Browsable(true), DevCoBrowsable(true), System.ComponentModel.Category("Data"), System.ComponentModel.Description("Indicates string that specifies the property of the data source whose contents you want to display."), System.ComponentModel.DefaultValue("")]
        public virtual string DisplayMember
        {
            get
            {
                if(m_ComboBox!=null)
                    return m_ComboBox.DisplayMember;
                return "";
            }
            set
            {
                if (m_ComboBox != null)
                    m_ComboBox.DisplayMember = value;
            }
        }

		/// <summary>
		/// Specifies whether combo box is drawn using themes when running on OS that supports themes like Windows XP
		/// </summary>
		[System.ComponentModel.Browsable(true),DevCoBrowsable(true),System.ComponentModel.Category("Appearance"),System.ComponentModel.Description("Specifies whether combo box is drawn using themes when running on OS that supports themes like Windows XP.")]
		public override bool ThemeAware
		{
			get
			{
				return m_ComboBox.ThemeAware;
			}
			set
			{
				m_ComboBox.ThemeAware=value;
			}
		}

		[System.ComponentModel.Browsable(false),System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsWindowed
		{
			get {return true;}
		}

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return m_ComboBox.WatermarkFont; }
            set { m_ComboBox.WatermarkFont = value; }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return m_ComboBox.WatermarkColor; }
            set { m_ComboBox.WatermarkColor = value; }
        }
        /// <summary>
        /// Indicates whether property should be serialized by Windows Forms designer.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeWatermarkColor()
        {
            return m_ComboBox.WatermarkColor != SystemColors.GrayText;
        }
        /// <summary>
        /// Resets the property to default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetWatermarkColor()
        {
            this.WatermarkColor = SystemColors.GrayText;
        }

        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public virtual bool WatermarkEnabled
        {
            get { return m_ComboBox.WatermarkEnabled; }
            set { m_ComboBox.WatermarkEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// Note that WatermarkText is not compatible with the auto-complete feature of .NET Framework 2.0.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=9.5.0.16, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return m_ComboBox.WatermarkText; }
            set
            {
                m_ComboBox.WatermarkText = value;
            }
        }

        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return m_ComboBox.WatermarkBehavior; }
            set { m_ComboBox.WatermarkBehavior = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), RefreshProperties(RefreshProperties.All), ParenthesizePropertyName(true)]
        public ControlBindingsCollection DataBindings
        {
            get { return m_ComboBox.DataBindings; }
        }

        /// <summary>
        /// Gets or sets whether control is stand-alone control. Stand-alone flag affects the appearance of the control in Office 2007 style.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Appearance"), Description("Indicates the appearance of the control.")]
        public bool IsStandalone
        {
            get { return m_ComboBox.IsStandalone; }
            set
            {
                m_ComboBox.IsStandalone = value;
            }
        }
	}
}
