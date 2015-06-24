using System;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing;

namespace DevComponents.DotNetBar.Design
{
    public class TextBoxItemDesigner : SimpleBaseItemDesigner
    {
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties["WatermarkText"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkText"], null);
            properties["WatermarkBehavior"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkBehavior"], null);
            properties["WatermarkColor"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkColor"], null);
            properties["WatermarkEnabled"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkEnabled"], null);
            properties["WatermarkFont"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkFont"], null);
        }

        // <summary>
        /// Gets or sets the watermark (tip) text displayed inside of the control when Text is not set and control does not have input focus. This property supports text-markup.
        /// </summary>
        [Browsable(true), DefaultValue(""), Localizable(true), Category("Appearance"), Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus."), Editor(typeof(Design.TextMarkupUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string WatermarkText
        {
            get { return (string)ShadowProperties["WatermarkText"]; }
            set
            {
                ShadowProperties["WatermarkText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the watermark hiding behaviour. Default value indicates that watermark is hidden when control receives input focus.
        /// </summary>
        [DefaultValue(eWatermarkBehavior.HideOnFocus), Category("Behavior"), Description("Indicates watermark hiding behaviour.")]
        public eWatermarkBehavior WatermarkBehavior
        {
            get { return (eWatermarkBehavior)ShadowProperties["WatermarkBehavior"]; }
            set { ShadowProperties["WatermarkBehavior"] = value; }
        }

        /// <summary>
        /// Gets or sets the watermark text color.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark text color.")]
        public Color WatermarkColor
        {
            get { return (Color)ShadowProperties["WatermarkColor"]; }
            set { ShadowProperties["WatermarkColor"] = value; }
        }

        /// <summary>
        /// Gets or sets whether watermark text is displayed when control is empty. Default value is true.
        /// </summary>
        [DefaultValue(true), Description("Indicates whether watermark text is displayed when control is empty.")]
        public bool WatermarkEnabled
        {
            get { return (bool)ShadowProperties["WatermarkEnabled"]; }
            set { ShadowProperties["WatermarkEnabled"] = value; }
        }

        /// <summary>
        /// Gets or sets the watermark font.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("Indicates watermark font."), DefaultValue(null)]
        public Font WatermarkFont
        {
            get { return (Font)ShadowProperties["WatermarkFont"]; }
            set { ShadowProperties["WatermarkFont"] = value; }
        }
    }
}
