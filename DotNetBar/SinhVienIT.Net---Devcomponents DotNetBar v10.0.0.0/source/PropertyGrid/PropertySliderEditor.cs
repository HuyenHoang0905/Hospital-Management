using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar.Controls;
using System.Drawing;
using System.ComponentModel;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Defines base attribute for custom AdvPropertyGrid in-line property value editors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class PropertyValueEditor : Attribute
    {
        /// <summary>
        /// Creates a control that is used as property value in-line editor. Control must implement IPropertyValueEditor interface.
        /// </summary>
        /// <param name="propertyDescriptor">PropertyDescriptor for the property being edited.</param>
        /// <param name="targetObject">Target object that owns the property.</param>
        /// <returns>Control that represents in-line editor.</returns>
        public abstract IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject);
    }


    /// <summary>
    /// Defines a attribute which applies an slider in-line editor to a property when used with AdvPropertyGrid control.
    /// </summary>
    public class PropertySliderEditor : PropertyValueEditor
    {
        /// <summary>
        /// Gets or sets the minimum slider value.
        /// </summary>
        public int MinValue = 0;
        /// <summary>
        /// Gets or sets the maximum slider value.
        /// </summary>
        public int MaxValue = 100;
        /// <summary>
        /// Gets or sets whether slider text label is visible.
        /// </summary>
        public bool LabelVisible = true;
        /// <summary>
        /// Gets or sets the slider label width. Default value is 18.
        /// </summary>
        public int LabelWidth = 22;
        /// <summary>
        /// Gets or sets label text color.
        /// </summary>
        public Color TextColor = Color.Black;

        /// <summary>
        /// Initializes a new instance of the PropertySliderEditor class.
        /// </summary>
        /// <param name="minValue">Minimum value for slider.</param>
        /// <param name="maxValue">Maximum value for slider.</param>
        public PropertySliderEditor(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySliderEditor class.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="textColor"></param>
        public PropertySliderEditor(int minValue, int maxValue, Color textColor)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            TextColor = textColor;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySliderEditor class.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="labelVisible"></param>
        /// <param name="labelWidth"></param>
        /// <param name="textColor"></param>
        public PropertySliderEditor(int minValue, int maxValue, bool labelVisible, int labelWidth, Color textColor)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            LabelVisible = labelVisible;
            LabelWidth = labelWidth;
            TextColor = textColor;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySliderEditor class.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="labelVisible"></param>
        public PropertySliderEditor(int minValue, int maxValue, bool labelVisible)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            LabelVisible = labelVisible;
        }

        /// <summary>
        /// Initializes a new instance of the PropertySliderEditor class.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="labelVisible"></param>
        /// <param name="labelWidth"></param>
        public PropertySliderEditor(int minValue, int maxValue, bool labelVisible, int labelWidth)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            LabelVisible = labelVisible;
            LabelWidth = labelWidth;
        }

        /// <summary>
        /// Creates a control that is used as property value in-line editor. Control must implement IPropertyValueEditor interface.
        /// </summary>
        /// <param name="propertyDescriptor">PropertyDescriptor for the property being edited.</param>
        /// <param name="targetObject">Target object that owns the property.</param>
        /// <returns>Control that represents in-line editor.</returns>
        public override IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject)
        {
            SliderPropertyEditor editor = new SliderPropertyEditor();
            editor.Style = eDotNetBarStyle.StyleManagerControlled;
            editor.LabelVisible = LabelVisible;
            editor.LabelWidth = LabelWidth;
            editor.Minimum = MinValue;
            editor.Maximum = MaxValue;
            editor.Height = 16;
            editor.FocusCuesEnabled = false;
            if (!TextColor.IsEmpty)
                editor.TextColor = TextColor;
            return editor;
        }

        private class SliderPropertyEditor : Slider, IPropertyValueEditor
        {
            #region IPropertyValueEditor Members

            public System.Drawing.Font EditorFont
            {
                get
                {
                    return this.Font;
                }
                set
                {
                    this.Font = value;
                }
            }

            public bool IsEditorFocused
            {
                get { return this.Focused; }
            }

            public void FocusEditor()
            {
                this.Focus();
            }

            public object EditValue
            {
                get
                {
                    return this.Value;
                }
                set
                {
                    if (value is int)
                        this.Value = (int)value;
                    else if (value is double)
                        this.Value = (int)(double)value;
                    else if (value is float)
                        this.Value = (int)(float)value;
                    else if (value is decimal)
                        this.Value = (int)(decimal)value;
                    else if (value is byte)
                        this.Value = (int)(byte)value;
                    else if (value is long)
                        this.Value = (int)(long)value;
                    else if (value is Single)
                        this.Value = (int)(Single)value;
                    this.Text = this.Value.ToString();
                }
            }

            //protected override void OnValueChanged()
            //{
            //    this.Text = this.Value.ToString();
            //    OnEditValueChanged(EventArgs.Empty);
            //    base.OnValueChanged();
            //}

            protected override void OnValueChanged(EventArgs e)
            {
                this.Text = this.Value.ToString();
                OnEditValueChanged(e);
                base.OnValueChanged(e);
            }

            private void OnEditValueChanged(EventArgs e)
            {
                EventHandler ev = EditValueChanged;
                if (ev != null) ev(this, e);
            }
            public event EventHandler EditValueChanged;

            #endregion
        }
    }
}
