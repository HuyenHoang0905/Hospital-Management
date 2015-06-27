#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using DevComponents.DotNetBar.Controls;
using System.Collections;
using DevComponents.DotNetBar.Rendering;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design
{
    public class MaskedTextBoxAdvDesigner : ControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            MaskedTextBoxAdv control = this.Control as MaskedTextBoxAdv;
            control.BackgroundStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
            control.ButtonClear.Visible = true;
            control.Height = control.PreferredHeight;
            control.Text = "";
            control.Style = eDotNetBarStyle.StyleManagerControlled;
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return (base.SelectionRules & ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable));
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            string[] strArray = new string[] { "Text", "PasswordChar" };
            Attribute[] attributes = new Attribute[0];
            for (int i = 0; i < strArray.Length; i++)
            {
                PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)properties[strArray[i]];
                if (oldPropertyDescriptor != null)
                {
                    properties[strArray[i]] = TypeDescriptor.CreateProperty(typeof(MaskedTextBoxAdvDesigner), oldPropertyDescriptor, attributes);
                }
            }
        }

        public char PasswordChar
        {
            get
            {
                MaskedTextBoxAdv control = this.Control as MaskedTextBoxAdv;
                if (control.UseSystemPasswordChar)
                {
                    control.UseSystemPasswordChar = false;
                    char passwordChar = control.PasswordChar;
                    control.UseSystemPasswordChar = true;
                    return passwordChar;
                }
                return control.PasswordChar;
            }
            set
            {
                MaskedTextBoxAdv control = this.Control as MaskedTextBoxAdv;
                control.PasswordChar = value;
            }
        }

        public string Text
        {
            get
            {
                MaskedTextBoxAdv control = this.Control as MaskedTextBoxAdv;
                if (string.IsNullOrEmpty(control.Mask))
                {
                    return control.Text;
                }
                return control.MaskedTextProvider.ToString(false, false);
            }
            set
            {
                MaskedTextBoxAdv control = this.Control as MaskedTextBoxAdv;
                if (string.IsNullOrEmpty(control.Mask))
                {
                    control.Text = value;
                }
                else
                {
                    bool resetOnSpace = control.ResetOnSpace;
                    bool resetOnPrompt = control.ResetOnPrompt;
                    bool skipLiterals = control.SkipLiterals;
                    control.ResetOnSpace = true;
                    control.ResetOnPrompt = true;
                    control.SkipLiterals = true;
                    control.Text = value;
                    control.ResetOnSpace = resetOnSpace;
                    control.ResetOnPrompt = resetOnPrompt;
                    control.SkipLiterals = skipLiterals;
                }
            }
        }

        private DesignerActionListCollection _Actions = null;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (this._Actions == null)
                {
                    this._Actions = new DesignerActionListCollection();
                    this._Actions.Add(new MaskedTextBoxAdvDesignerActionList(this));
                }
                return this._Actions;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (this._Verbs == null)
                {
                    this._Verbs = new DesignerVerbCollection();
                    this._Verbs.Add(new DesignerVerb("Set Mask...", new EventHandler(this.OnVerbSetMask)));
                }
                return this._Verbs;
            }
        }

        private DesignerVerbCollection _Verbs = null;
        private void OnVerbSetMask(object sender, EventArgs e)
        {
            new MaskedTextBoxAdvDesignerActionList(this).SetMask();
        }
    }

    public class MaskedTextBoxAdvDesignerActionList : DesignerActionList
    {
        // Fields
        private ITypeDiscoveryService discoverySvc;
        private IHelpService helpService;
        private MaskedTextBoxAdv maskedTextBox;
        private IUIService uiSvc;

        // Methods
        public MaskedTextBoxAdvDesignerActionList(MaskedTextBoxAdvDesigner designer)
            : base(designer.Component)
        {
            this.maskedTextBox = (MaskedTextBoxAdv)designer.Component;
            this.discoverySvc = base.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
            this.uiSvc = base.GetService(typeof(IUIService)) as IUIService;
            this.helpService = base.GetService(typeof(IHelpService)) as IHelpService;
            if (this.discoverySvc != null)
            {
                IUIService uiSvc = this.uiSvc;
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "SetMask", "Set Mask..."));
            return items;
        }

        public void SetMask()
        {
            string str = MaskAdvPropertyEditor.EditMask(this.discoverySvc, this.uiSvc, this.maskedTextBox, this.helpService);
            if (str != null)
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.maskedTextBox)["Mask"];
                if (descriptor != null)
                {
                    descriptor.SetValue(this.maskedTextBox, str);
                }
            }
        }
    }

    public class MaskAdvPropertyEditor : UITypeEditor
    {
        // Methods
        internal static string EditMask(ITypeDiscoveryService discoverySvc, IUIService uiSvc, MaskedTextBoxAdv instance, IHelpService helpService)
        {
            string mask = null;
            Type formType = Type.GetType("System.Windows.Forms.Design.MaskDesignerDialog, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            ConstructorInfo ci = formType.GetConstructor(new Type[] { typeof(MaskedTextBox), typeof(IHelpService) });
            Form form = ci.Invoke(new object[]{instance.MaskedTextBox, helpService}) as Form;
            
            try
            {
                MethodInfo mi = formType.GetMethod("DiscoverMaskDescriptors");
                mi.Invoke(form, new object[]{discoverySvc});
                //form.DiscoverMaskDescriptors(discoverySvc);
                DialogResult result = (uiSvc != null) ? uiSvc.ShowDialog(form) : form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    PropertyInfo pi = formType.GetProperty("Mask");
                    mask = (string)pi.GetValue(form, null);
                    pi = formType.GetProperty("ValidatingType");
                    Type validatingType = pi.GetValue(form, null) as Type;
                    //mask = form.Mask;
                    if (validatingType == instance.ValidatingType)
                    {
                        return mask;
                    }
                    instance.ValidatingType = validatingType;
                }
            }
            finally
            {
                form.Dispose();
            }
            return mask;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                ITypeDiscoveryService discoverySvc = (ITypeDiscoveryService)provider.GetService(typeof(ITypeDiscoveryService));
                IUIService service = (IUIService)provider.GetService(typeof(IUIService));
                IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
                string str = EditMask(discoverySvc, service, context.Instance as MaskedTextBoxAdv, helpService);
                if (str != null)
                {
                    return str;
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }


}
#endif