using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Design
{
    /// <summary>
    /// Represents windows forms designer support for ExpandableSplitter control.
    /// </summary>
    public class ExpandableSplitterDesigner : ControlDesigner
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExpandableSplitterDesigner() { }

#if FRAMEWORK20
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            SetDesignTimeDefaults();
        }
#else
        public override void OnSetComponentDefaults()
        {
            SetDesignTimeDefaults();
            base.OnSetComponentDefaults();
        }
#endif

        private void SetDesignTimeDefaults()
        {
            this.Style = eSplitterStyle.Office2007;
            ((ExpandableSplitter)this.Control).Width = 6;
        }

        protected override void PreFilterProperties(
            System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["Style"] =
                TypeDescriptor.CreateProperty(typeof(ExpandableSplitterDesigner),
                (PropertyDescriptor)properties["Style"],
                new Attribute[0]);
        }

        /// <summary>
        /// Gets or sets visual style of the control. Default value is eSplitterStyle.Office2003.
        /// </summary>
        [Browsable(true), Category("Appearance"), DefaultValue(eSplitterStyle.Office2003), Description("Indicates visual style of the control.")]
        public eSplitterStyle Style
        {
            get { return ((ExpandableSplitter)this.Control).Style; }
            set
            {
                ((ExpandableSplitter)this.Control).Style = value;
                IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (dh != null && dh.Loading)
                    return;

                ((ExpandableSplitter)this.Control).ApplyStyle(value);
                OnStyleChanged();
            }
        }

        private void OnStyleChanged()
        {
            ExpandableSplitter splitter = this.Control as ExpandableSplitter;
            IComponentChangeService change = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (change != null)
            {
                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["Style"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["Style"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor2"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor2"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripLightColor"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripLightColor"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripDarkColor"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripDarkColor"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["EpxandFillColor"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["ExpandFillColor"], null, null);

                change.OnComponentChanging(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["EpxandLineColor"]);
                change.OnComponentChanged(splitter, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["ExpandLineColor"], null, null);
            }
        }

        private IDesignerHost GetIDesignerHost()
        {
            try
            {
                IDesignerHost dh = (IDesignerHost)GetService(typeof(IDesignerHost));
                return dh;
            }
            catch { }
            return null;
        }
    }
}
