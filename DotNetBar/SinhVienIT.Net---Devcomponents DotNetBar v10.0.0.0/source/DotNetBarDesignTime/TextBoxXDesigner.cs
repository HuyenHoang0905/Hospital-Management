using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DevComponents.DotNetBar.Design
{
    public class TextBoxXDesigner : ControlDesigner
    {
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
            PropertyDescriptor descriptor1 = TypeDescriptor.GetProperties(base.Component)["Text"];
            if (descriptor1 != null && descriptor1.PropertyType == typeof(string) && !descriptor1.IsReadOnly && descriptor1.IsBrowsable)
                descriptor1.SetValue(base.Component, "");
            TextBoxX tx = this.Control as TextBoxX;
            tx.Border.Class = ElementStyleClassKeys.TextBoxBorderKey;
            tx.SetAutoHeight();
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            string[] ta = new string[] { "PasswordChar" };
            Attribute[] aa = new Attribute[0];
            for (int num1 = 0; num1 < ta.Length; num1++)
            {
                PropertyDescriptor desc = (PropertyDescriptor)properties[ta[num1]];
                if (desc != null)
                {
                    properties[ta[num1]] = TypeDescriptor.CreateProperty(typeof(TextBoxXDesigner), desc, aa);
                }
            }

            ta = new string[] { "Text" };
            aa = new Attribute[0];
            for (int num1 = 0; num1 < ta.Length; num1++)
            {
                PropertyDescriptor desc = (PropertyDescriptor)properties[ta[num1]];
                if (desc != null)
                {
                    properties[ta[num1]] = TypeDescriptor.CreateProperty(typeof(TextBoxXDesigner), desc, aa);
                }
            }

        }

        public void ResetText()
        {
            this.Control.Text = "";
        }

        public bool ShouldSerializeText()
        {
            return TypeDescriptor.GetProperties(typeof(TextBoxX))["Text"].ShouldSerializeValue(base.Component);
        }

        public string Text
        {
            get
            {
                return this.Control.Text;
            }
            set
            {
                this.Control.Text = value;
                ((TextBoxX)this.Control).Select(0, 0);
            }
        }
 
        public char PasswordChar
        {
            get
            {
                TextBox box1 = this.Control as TextBox;
#if FRAMEWORK20
                if (box1.UseSystemPasswordChar)
                {
                    box1.UseSystemPasswordChar = false;
                    char ch1 = box1.PasswordChar;
                    box1.UseSystemPasswordChar = true;
                    return ch1;
                }
#endif
                return box1.PasswordChar;
            }
            set
            {
                TextBox box1 = this.Control as TextBox;
                box1.PasswordChar = value;
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules sr = base.SelectionRules;
                object o = base.Component;
                sr |= SelectionRules.AllSizeable;
                PropertyDescriptor desc = TypeDescriptor.GetProperties(o)["Multiline"];
                if (desc != null)
                {
                    object o2 = desc.GetValue(o);
                    if ((o2 is bool) && !((bool)o2))
                    {
                        PropertyDescriptor desc2 = TypeDescriptor.GetProperties(o)["AutoSize"];
                        if (desc2 != null)
                        {
                            object o3 = desc2.GetValue(o);
                            if ((o3 is bool) && ((bool)o3))
                            {
                                sr &= ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
                            }
                        }
                    }
                }
                return sr;
            }
        }
 
#if FRAMEWORK20
        private DesignerActionListCollection m_ActionLists;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (m_ActionLists == null)
                {
                    m_ActionLists = new DesignerActionListCollection();
                    object o = this.GetType().Assembly.CreateInstance("System.Windows.Forms.Design.TextBoxActionList", false, System.Reflection.BindingFlags.NonPublic, null, new object[] { this }, null, null);
                    if (o != null)
                        m_ActionLists.Add(o as DesignerActionList);
                }
                return m_ActionLists;
            }
        }

        public override IList SnapLines
        {
            get
            {
                ArrayList sl = base.SnapLines as ArrayList;
                int textBaseLine = GetTextBaseline(this.Control, ContentAlignment.TopLeft);
                BorderStyle border = BorderStyle.Fixed3D;
                PropertyDescriptor d = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
                if (d != null)
                {
                    border = (BorderStyle)d.GetValue(base.Component);
                }
                if (border == BorderStyle.FixedSingle || border == BorderStyle.None)
                {
                    textBaseLine += 2;
                }
                else if (border == BorderStyle.Fixed3D)
                {
                    textBaseLine += 3;
                }
                sl.Add(new System.Windows.Forms.Design.Behavior.SnapLine(System.Windows.Forms.Design.Behavior.SnapLineType.Baseline, textBaseLine, System.Windows.Forms.Design.Behavior.SnapLinePriority.Medium));
                return sl;
            }
        }

        private static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
        {
            Rectangle clientRect = ctrl.ClientRectangle;
            int ascent = 0;
            int height = 0;
            using (Graphics g = ctrl.CreateGraphics())
            {
                IntPtr hdc = g.GetHdc();
                IntPtr hFont = ctrl.Font.ToHfont();
                try
                {
                    IntPtr oldObject = WinApi.SelectObject(hdc, hFont);
                    WinApi.TEXTMETRIC tm = new WinApi.TEXTMETRIC();
                    WinApi.GetTextMetrics(new HandleRef(ctrl, hdc), tm);
                    ascent = tm.tmAscent + 1;
                    height = tm.tmHeight;
                    WinApi.SelectObject(hdc, oldObject);
                }
                finally
                {
                    WinApi.DeleteObject(hFont);
                    g.ReleaseHdc(hdc);
                }
            }
            if ((alignment & (ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft)) != (ContentAlignment)0)
            {
                return (clientRect.Top + ascent);
            }
            if ((alignment & ( ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft)) != (ContentAlignment)0)
            {
                return (((clientRect.Top + (clientRect.Height / 2)) - (height / 2)) + ascent);
            }
            return ((clientRect.Bottom - height) + ascent);
        }
#endif
    }
}
