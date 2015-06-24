using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.Instrumentation.Design
{
    public class KnobControlActionList : DesignerActionList
    {
        #region Private variables

        private KnobControl _KnobControl;
        
        #endregion

        /// <summary>
        /// KnobControlActionList
        /// </summary>
        /// <param name="knobControl">Associated KnobControl</param>
        public KnobControlActionList(KnobControl knobControl)
            : base(knobControl)
        {
            _KnobControl = knobControl;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the KnobStyle
        /// </summary>
        public eKnobStyle KnobStyle
        {
            get { return (_KnobControl.KnobStyle); }
            set { GetPropertyByName("KnobStyle").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the MinValue
        /// </summary>
        public decimal MinValue
        {
            get { return (_KnobControl.MinValue); }
            set { GetPropertyByName("MinValue").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the MaxValue
        /// </summary>
        public decimal MaxValue
        {
            get { return (_KnobControl.MaxValue); }
            set { GetPropertyByName("MaxValue").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the StartAngle
        /// </summary>
        public int StartAngle
        {
            get { return (_KnobControl.StartAngle); }
            set { GetPropertyByName("StartAngle").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the SweepAngle
        /// </summary>
        public int SweepAngle
        {
            get { return (_KnobControl.SweepAngle); }
            set { GetPropertyByName("SweepAngle").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the MaxValue
        /// </summary>
        public decimal MajorTickAmount
        {
            get { return (_KnobControl.MajorTickAmount); }
            set { GetPropertyByName("MajorTickAmount").SetValue(_KnobControl, value); }
        }

        /// <summary>
        /// Gets or sets the MinorTickAmount
        /// </summary>
        public decimal MinorTickAmount
        {
            get { return (_KnobControl.MinorTickAmount); }
            set { GetPropertyByName("MinorTickAmount").SetValue(_KnobControl, value); }
        }

        #endregion

        #region GetPropertyByName

        /// <summary>
        /// Gets the property via the given name
        /// </summary>
        /// <param name="propName">Property name</param>
        /// <returns>PropertyDescriptor</returns>
        private PropertyDescriptor GetPropertyByName(string propName)
        {
            PropertyDescriptor prop =
                TypeDescriptor.GetProperties(_KnobControl)[propName];

            if (prop == null)
                throw new ArgumentException("Matching property not found.", propName);

            return (prop);
        }

        #endregion

        #region GetSortedActionItems

        /// <summary>
        /// Gets the SortedActionItems
        /// </summary>
        /// <returns>DesignerActionItemCollection</returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            items.Add(new DesignerActionPropertyItem("KnobStyle", "Knob Style",
                "", "Sets the Knob Style."));

            items.Add(new DesignerActionHeaderItem("Value Range"));
            items.Add(new DesignerActionHeaderItem("Angle Range"));

            items.Add(new DesignerActionPropertyItem("MinValue", "Minimum Value",
                "Value Range", "Sets the minimum value permitted for the Knob value."));

            items.Add(new DesignerActionPropertyItem("MaxValue", "Maximum Value",
                "Value Range", "Sets the maximum value permitted for the Knob value."));

            items.Add(new DesignerActionPropertyItem("MajorTickAmount", "MajorTick Amount",
                "Value Range", "Sets the interval amount between major tick marks."));

            items.Add(new DesignerActionPropertyItem("MinorTickAmount", "MinorTick Amount",
                "Value Range", "Sets the interval amount between minor tick marks."));
            
            items.Add(new DesignerActionPropertyItem("StartAngle", "Starting Angle",
                "Angle Range", "Sets the Start angle."));

            items.Add(new DesignerActionPropertyItem("SweepAngle", "Sweep Angle",
                "Angle Range", "Sets the Sweep angle."));

            items.Add(new DesignerActionMethodItem(this, "ReverseSweepAngle", "Reverse SweepAngle", "Angle Range"));

            items.Add(new DesignerActionTextItem(
                "SweepAngle can be positive or negative, denoting \n" + 
                "clockwise or counter-clockwise rotation, respectively", "Angle Range"));

            return (items);
        }

        #endregion

        #region ReverseSweepAngle

        /// <summary>
        /// Reverses the start and end angles for the control
        /// </summary>
        public void ReverseSweepAngle()
        {
            int angle = (StartAngle + SweepAngle) % 360;

            if (angle < 0)
                angle += 360;

            StartAngle = angle;
            SweepAngle = -SweepAngle;
        }

        #endregion
    }
}
