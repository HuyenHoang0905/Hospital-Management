using System;
using System.Text;
using DevComponents.DotNetBar.Controls;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents the Windows Forms designer for the GroupPanel control.
    /// </summary>
    public class GroupPanelDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs;
                verbs = new DesignerVerb[]
				{
					new DesignerVerb("Reset Style", new EventHandler(ResetStyle)),
				};
                return new DesignerVerbCollection(verbs);
            }
        }

        private void ResetStyle(object sender, EventArgs e)
        {
            GroupPanel p = this.Control as GroupPanel;
            if (p == null)
                return;

            IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (change != null)
                change.OnComponentChanging(this.Component, null);

            p.SetDefaultPanelStyle();

            if (change != null)
                change.OnComponentChanged(this.Component, null, null, null);
        }

#if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
            SetDesignTimeDefaults();
		}
#endif

        private void SetDesignTimeDefaults()
        {
            GroupPanel p = this.Control as GroupPanel;
            if (p == null)
                return;
            p.SetDefaultPanelStyle();
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            PanelControl p;
            p = this.Component as PanelControl;
            if (p.Style.Border == eStyleBorderType.None)
                this.DrawBorder(pe.Graphics);
            base.OnPaintAdornments(pe);
        }

        /// <summary>
        /// Draws design-time border around the panel when panel does not have one.
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawBorder(Graphics g)
        {
            PanelControl panel = this.Control as PanelControl;
            Color border = SystemColors.ControlDarkDark;
            Rectangle rClient = this.Control.ClientRectangle;
            Color backColor = panel.Style.BackColor;

            Helpers.DrawDesignTimeSelection(g, rClient, backColor, border, 1);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinApi.WindowsMessages.WM_NCHITTEST)
            {
                GroupPanel gp = this.Control as GroupPanel;
                if (gp != null)
                {
                    int x = WinApi.LOWORD(m.LParam);
                    int y = WinApi.HIWORD(m.LParam);
                    Point p = gp.PointToClient(new Point(x, y));
                    if (p.Y<0)
                    {
                        m.Result = new IntPtr((int)WinApi.WindowHitTestRegions.ClientArea);
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }
    }
}
