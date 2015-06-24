using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;

namespace DevComponents.DotNetBar
{
	/// <summary>
	/// Represents a balloon style pop-up window that displays a brief description of a control's purpose when the mouse hovers over the control or when controls receives input focus.
	/// </summary>
	[ToolboxItem(true),ProvideProperty("BalloonCaption",typeof(Control)),
	ProvideProperty("BalloonText",typeof(Control)),
	System.Runtime.InteropServices.ComVisible(false)]
	public class BalloonTip:Component, IExtenderProvider
	{
        /// <summary>
        /// Occurs before balloon is displayed.
        /// </summary>
        [Description("Occurs before balloon is displayed.")]
        public event EventHandler BalloonDisplaying;
        /// <summary>
        /// Occurs before balloon is closed and allows to cancel the action.
        /// </summary>
        [Description("Occurs before balloon is closed and allows to cancel the action.")]
        public event CancelEventHandler BalloonClosing;

		private bool m_BalloonFocus=false;
		private bool m_Enabled=true;
		private bool m_ShowAlways=false;
		private eBallonStyle m_Style=eBallonStyle.Balloon;
		private int m_InitialDelay=500;
		private bool m_AutoClose=true;
		private int m_AutoCloseTimeOut=5;
		private eAlertAnimation m_AlertAnimation=eAlertAnimation.BottomToTop;
		private int m_AlertAnimationDuration=200;
		private bool m_ShowBalloonOnFocus=false;
		private bool m_ShowCloseButton=true;

		private Balloon m_BalloonControl=null;

		private System.Drawing.Image m_CaptionImage=null;
		private System.Drawing.Icon m_CaptionIcon=null;

		private Hashtable m_BalloonsInfo=new Hashtable();

		private Control m_MouseOverControl=null;
		private Control m_FocusedControl=null;

		private Timer m_DelayTimer=null;

		private Control m_BalloonTriggerControl=null;
		private int m_DefaultBalloonWidth=256;
        private bool m_AntiAlias = true;
		/// <summary>
		/// Initializes a new instance of the BalloonTip class.
		/// </summary>
		public BalloonTip()
		{
		}

		/// <summary>
		///    Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			RemoveAll();
			m_MouseOverControl=null;
			m_FocusedControl=null;
			m_BalloonTriggerControl=null;
            if (BarUtilities.DisposeItemImages &&  !this.DesignMode)
            {
                BarUtilities.DisposeImage(ref m_CaptionImage);
                BarUtilities.DisposeImage(ref m_CaptionIcon);
            }
			DestroyDelayTimer();
			base.Dispose(disposing);
		}

		bool IExtenderProvider.CanExtend(object target) 
		{
			if (target is Control)
				return true;
			return false;
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
            }
        }

		/// <summary>
		/// Retrieves the Balloon Caption text associated with the specified control.
		/// </summary>
        [DefaultValue(""), Localizable(true)]
		public string GetBalloonCaption(Control control)
		{
			if(m_BalloonsInfo.Contains(control))
			{
				BalloonTipInfo info=m_BalloonsInfo[control] as BalloonTipInfo;
				if(info!=null)
					return info.Caption;
			}
			return "";
		}

		/// <summary>
		/// Associates Balloon Caption text with the specified control.
		/// </summary>
		/// <param name="control">The Control to associate the Balloon Caption text with.</param>
		/// <param name="value">The Balloon Caption text to display on the Balloon.</param>
		[Localizable(true)]
        public void SetBalloonCaption(Control control, string caption)
		{
			if(caption==null)
				caption="";
			if(m_BalloonsInfo.Contains(control))
			{
				BalloonTipInfo info=m_BalloonsInfo[control] as BalloonTipInfo;
				if(info!=null)
				{
					info.Caption=caption;
					if(info.Caption=="" && info.Text=="")
					{
						this.Remove(control);
					}
				}
			}
			else if(caption!="")
			{
				BalloonTipInfo info=new BalloonTipInfo();
				info.Caption=caption;
				this.AddControl(control,info);
			}
		}

		/// <summary>
		/// Retrieves the Balloon text associated with the specified control.
		/// </summary>
		[DefaultValue(""), Localizable(true)]
		public string GetBalloonText(Control control)
		{
			if(m_BalloonsInfo.Contains(control))
			{
				BalloonTipInfo info=m_BalloonsInfo[control] as BalloonTipInfo;
				if(info!=null)
					return info.Text;
			}
			return "";
		}

		/// <summary>
		/// Associates Balloon text with the specified control.
		/// </summary>
		/// <param name="control">The Control to associate the Balloon text with.</param>
		/// <param name="value">The Balloon text to display on the Balloon.</param>
		[Localizable(true)]
        public void SetBalloonText(Control control, string text)
		{
			if(text==null)
				text="";
			if(m_BalloonsInfo.Contains(control))
			{
				BalloonTipInfo info=m_BalloonsInfo[control] as BalloonTipInfo;
				if(info!=null)
				{
					info.Text=text;
					if(info.Caption=="" && info.Text=="")
					{
						this.Remove(control);
					}
				}
			}
			else if(text!="")
			{
				BalloonTipInfo info=new BalloonTipInfo();
				info.Text=text;
				this.AddControl(control,info);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the BalloonTip is currently active.
		/// true if the BalloonTip is currently active; otherwise, false. The default is true.
		/// </summary>
		[DefaultValue(true),Category("Misc"),Description("Gets or sets a value indicating whether the BalloonTip is currently active.")]
		public bool Enabled
		{
			get{return m_Enabled;}
			set
			{
				m_Enabled=value;
			}
		}

		/// <summary>
		/// Removes all Balloon texts currently associated with the BalloonTip control.
		/// </summary>
		public void RemoveAll()
		{
			Control[] ctrls=new Control[m_BalloonsInfo.Keys.Count];

			m_BalloonsInfo.Keys.CopyTo(ctrls,0);
			foreach(Control c in ctrls)
			{
				this.Remove(c);
			}
			m_BalloonsInfo.Clear();
		}

		/// <summary>
		/// Removes specific Balloon texts currently associated with the BalloonTip control.
		/// </summary>
		/// <param name="control">Control that has Balloon texts associated.</param>
		public void Remove(Control control)
		{
			if(m_BalloonsInfo.Contains(control))
			{
				// Remove event handlers
				try
				{
					control.MouseEnter-=new EventHandler(this.ControlMouseEnter);
					control.MouseLeave-=new EventHandler(this.ControlMouseLeave);
					control.Enter-=new EventHandler(this.ControlGotFocus);
					control.Leave-=new EventHandler(this.ControlLeave);
				}
				catch{}
				m_BalloonsInfo.Remove(control);
			}
		}

		private void AddControl(Control control, BalloonTipInfo info)
		{
			if(m_BalloonsInfo.Contains(control))
				return;
			m_BalloonsInfo.Add(control,info);
			// Hook-up events
			control.MouseEnter+=new EventHandler(this.ControlMouseEnter);
			control.MouseLeave+=new EventHandler(this.ControlMouseLeave);
			control.Enter+=new EventHandler(this.ControlGotFocus);
			control.Leave+=new EventHandler(this.ControlLeave);
		}

		/// <summary>
		/// Shows Balloon for specific control. Control must have Balloon already assigned to it.
		/// </summary>
		/// <param name="control">Control that has Balloon already assigned.</param>
		public virtual void ShowBalloon(Control control)
		{
			if(m_BalloonsInfo.Contains(control))
			{
				BalloonTipInfo info=m_BalloonsInfo[control] as BalloonTipInfo;
				if(info!=null)
				{
					CloseBalloon();

					// Closing of balloon was denied exit.
					if(m_BalloonControl!=null)
					{
						return;
					}

					m_BalloonControl=CreateBalloonControl(info);
					m_BalloonControl.Location=new System.Drawing.Point(0,0);
					m_BalloonTriggerControl=control;
					
					if(BalloonDisplaying!=null)
						BalloonDisplaying(this,new EventArgs());

					// If Balloon Control is still around
					if(m_BalloonControl!=null)
					{
						Form parentForm=control.FindForm();
						if(parentForm!=null)
							m_BalloonControl.Owner=parentForm;
						if(!m_BalloonControl.Location.IsEmpty)
							m_BalloonControl.Show(m_BalloonFocus);
						else
							m_BalloonControl.Show(control,m_BalloonFocus);
					}
				}
			}
		}

		/// <summary>
		/// Returns reference to the control that triggered balloon.
		/// </summary>
		public Control BalloonTriggerControl
		{
			get
			{
				return m_BalloonTriggerControl;
			}
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

		private Balloon CreateBalloonControl(BalloonTipInfo info)
		{
			Balloon b=new Balloon();
			b.CaptionText=info.Caption;
			b.Text=info.Text;
			b.AutoClose=m_AutoClose;
			b.AutoCloseTimeOut=m_AutoCloseTimeOut;
			b.CaptionImage=this.CaptionImage;
			b.CaptionIcon=this.CaptionIcon;
			b.AlertAnimation=m_AlertAnimation;
			b.AlertAnimationDuration=m_AlertAnimationDuration;
			b.ShowCloseButton=m_ShowCloseButton;
			b.Style=m_Style;
            b.MinimumBalloonWidth = _MinimumBalloonWidth;
			if(info.Caption=="" || info.Caption==null)
			{
				b.Width=this.DefaultBalloonWidth;
			}
			else
				b.AutoResize();

			return b;
		}

		/// <summary>
		/// Closes Balloon control if visible.
		/// </summary>
		public virtual void CloseBalloon()
		{
			m_BalloonTriggerControl=null;

			if(m_BalloonControl==null)
				return;

			CancelEventArgs e=new CancelEventArgs(false);
			if(BalloonClosing!=null)
				BalloonClosing(this,e);
			if(e.Cancel)
				return;

			if(m_BalloonControl.Visible)
			{
				m_BalloonControl.Hide();
			}
            
			m_BalloonControl.Close();
            m_BalloonControl.Dispose();
            m_BalloonControl=null;
		}

		/// <summary>
		/// Gets or sets a value indicating whether Balloon receives input focus when displayed.
		/// Default value is false.
		/// </summary>
		[DefaultValue(false),Category("Misc"),Description("Gets or sets a value indicating whether Balloon receives input focus when displayed.")]
		public bool BalloonFocus
		{
			get {return m_BalloonFocus;}
			set {m_BalloonFocus=value;}
		}

		/// <summary>
		/// Gets or sets the time (in milliseconds) that passes before the BalloonTip appears.
		/// </summary>
		[DefaultValue(500),Category("Misc"),Description("Indicates the time (in milliseconds) that passes before the BalloonTip appears.")]
		public int InitialDelay
		{
			get{return m_InitialDelay;}
			set{m_InitialDelay=value;}
		}

		/// <summary>
		/// Gets or sets a value indicating whether a Balloon window is displayed even when its parent form is not active. Default value is false.
		/// </summary>
		[DefaultValue(false),Category("Misc"),Description("Indicates whether a Balloon window is displayed even when its parent form is not active.")]
		public bool ShowAlways
		{
			get{return m_ShowAlways;}
			set{m_ShowAlways=value;}
		}

		/// <summary>
		/// Gets or sets the internal Balloon control that is used to display Balloon.
		/// This property will have valid value only during time Balloon is actually
		/// displayed on the screen. Value will also be valid during BalloonDisplaying event.
		/// You can use this property to further customize Balloon control before it is
		/// displayed to the user. You can also set it to your own instance of the Balloon
		/// control (or the control that is inheriting it) for ultimate customization options.
		/// Note that new instance of Balloon control is created each time Balloon needs to be displayed.
		/// Once Balloon is closed control is disposed.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Balloon BalloonControl
		{
			get {return m_BalloonControl;}
			set {m_BalloonControl=value;}
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
				if(m_Style!=value)
				{
					m_Style=value;
				}
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
		[Browsable(true),Description("Indicates time period in seconds after balloon closes automatically."),Category("Behavior"),DefaultValue(5)]
		public int AutoCloseTimeOut
		{
			get {return m_AutoCloseTimeOut;}
			set
			{
				m_AutoCloseTimeOut=value;
			}
		}

		/// <summary>
		/// Gets or sets whether Balloon is shown after control receives input focus. Default value is false. When set to true Balloon will not be displayed on mouse hover.
		/// </summary>
		[Browsable(true),Description("Indicates whether Balloon is shown after control receives input focus."),Category("Behavior"),DefaultValue(false)]
		public bool ShowBalloonOnFocus
		{
			get {return m_ShowBalloonOnFocus;}
			set
			{
				if(m_ShowBalloonOnFocus!=value)
				{
					m_ShowBalloonOnFocus=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the Balloon Close button is displayed.
		/// </summary>
		[Browsable(true),Description("Indicates whether the Balloon Close button is displayed."),Category("Behavior"),DefaultValue(true)]
		public bool ShowCloseButton
		{
			get {return m_ShowCloseButton;}
			set
			{
				if(value!=m_ShowCloseButton)
				{
					m_ShowCloseButton=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets default balloon width. Usually the width of the balloon is calculated based on the width of the caption text. If caption text is not set then this value will be used as default width of the balloon.
		/// </summary>
		[Browsable(true),Description("Indicates default balloon width. Usually the width of the balloon is calculated based on the width of the caption text. If caption text is not set then this value will be used as default width of the balloon."),Category("Appearance"),DefaultValue(256)]
		public int DefaultBalloonWidth
		{
			get {return m_DefaultBalloonWidth;}
			set {m_DefaultBalloonWidth=value;}
		}

		/// <summary>
		/// Gets or sets the Balloon Caption image.
        /// </summary>
        [Browsable(true),Description("Indicates Balloon Caption image."),Category("Appearance"),DefaultValue(null)]
		public System.Drawing.Image CaptionImage
		{
			get {return m_CaptionImage;}
			set 
			{
				if(m_CaptionImage!=value)
				{
					m_CaptionImage=value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Balloon Caption icon. Icon is used to provide support for alpha-blended images in caption.
		/// </summary>
		[Browsable(true),Description("Indicates Balloon Caption icon. Icon is used to provide support for alpha-blended images in caption."),Category("Appearance"),DefaultValue(null)]
		public System.Drawing.Icon CaptionIcon
		{
			get {return m_CaptionIcon;}
			set 
			{
				if(m_CaptionIcon!=value)
				{
					m_CaptionIcon=value;
				}
			}
		}

		private void DestroyDelayTimer()
		{
			if(m_DelayTimer!=null)
			{
				m_DelayTimer.Stop();
				m_DelayTimer.Tick-=new EventHandler(this.DelayTimerTick);
				m_DelayTimer.Dispose();
				m_DelayTimer=null;
			}
		}

		private void DelayTimerTick(object sender, EventArgs e)
		{
			m_DelayTimer.Enabled=false;
			DestroyDelayTimer();

            if (m_Enabled)
            {
                if (m_ShowBalloonOnFocus && m_FocusedControl != null)
                    ShowBalloon(m_FocusedControl);
                else if (!m_ShowBalloonOnFocus && m_MouseOverControl != null)
                    ShowBalloon(m_MouseOverControl);
            }
		}

		private void ShowBalloonDelayed(Control control)
		{
			if(control==null || !m_Enabled)
				return;

			if(!m_ShowAlways)
			{
				Form parentForm=control.FindForm();
				if(parentForm.IsMdiChild)
				{
					if(parentForm.MdiParent!=null && parentForm.MdiParent.ActiveMdiChild!=parentForm)
						return;
				}
				else
				{
					if(Form.ActiveForm!=parentForm)
						return;
				}
			}

			if(m_InitialDelay==0)
			{
				ShowBalloon(control);
				return;
			}
			
			if(m_DelayTimer==null)
			{
				m_DelayTimer=new Timer();
				m_DelayTimer.Tick+=new EventHandler(this.DelayTimerTick);
				m_DelayTimer.Interval=m_InitialDelay;
				m_DelayTimer.Start();
			}
		}

		private void OnMouseEnter()
		{
			if(!m_Enabled || m_MouseOverControl==null || m_ShowBalloonOnFocus || !m_BalloonsInfo.Contains(m_MouseOverControl))
				return;

			ShowBalloonDelayed(m_MouseOverControl);
		}

		private void OnMouseLeave()
		{
			if(!m_ShowBalloonOnFocus)
			{
				DestroyDelayTimer();
				CloseBalloon();
			}
		}

		private void OnControlGotFocus()
		{
			if(!m_Enabled || m_FocusedControl==null || !m_ShowBalloonOnFocus || !m_BalloonsInfo.Contains(m_FocusedControl))
				return;

			ShowBalloonDelayed(m_FocusedControl);
		}

		private void OnControlLeave()
		{
			if(m_ShowBalloonOnFocus)
			{
				DestroyDelayTimer();
				CloseBalloon();
			}
		}

		private void ControlMouseEnter(object sender, EventArgs e)
		{
			m_MouseOverControl=sender as Control;
			OnMouseEnter();
		}

		private void ControlMouseLeave(object sender, EventArgs e)
		{
			m_MouseOverControl=null;
			OnMouseLeave();
		}

		private void ControlGotFocus(object sender, EventArgs e)
		{
			m_FocusedControl=sender as Control;
			OnControlGotFocus();
		}

		private void ControlLeave(object sender, EventArgs e)
		{
			m_FocusedControl=null;
			OnControlLeave();
		}

		private class BalloonTipInfo
		{
			public string Caption;
			public string Text;
		}
	}
}
