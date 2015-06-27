using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.DotNetBarKnobControl.Design;

namespace DevComponents.Instrumentation.Design
{
    public class GaugeControlActionList : DesignerActionList
    {
        #region Private variables

        private GaugeControl _GaugeControl;

        #endregion

        /// <summary>
        /// GaugeControlActionList
        /// </summary>
        /// <param name="gaugeControl">Associated GaugeControl</param>
        public GaugeControlActionList(GaugeControl gaugeControl)
            : base(gaugeControl)
        {
            _GaugeControl = gaugeControl;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the FrameStyle
        /// </summary>
        public GaugeFrameStyle FrameStyle
        {
            get { return (_GaugeControl.Frame.Style); }
            set { GetFramePropertyByName("Style").SetValue(_GaugeControl.Frame, value); }
        }

        /// <summary>
        /// Gets or sets the Glass Effect
        /// </summary>
        public bool AddGlassEffect
        {
            get { return (_GaugeControl.Frame.AddGlassEffect); }
            set { GetFramePropertyByName("AddGlassEffect").SetValue(_GaugeControl.Frame, value); }
        }

        /// <summary>
        /// Gets or sets the frame inner bevel size
        /// </summary>
        [Editor(typeof(HalfRadiusRangeValueEditor), typeof(UITypeEditor))]
        public float InnerBevel
        {
            get { return (_GaugeControl.Frame.InnerBevel); }
            set { GetFramePropertyByName("InnerBevel").SetValue(_GaugeControl.Frame, value); }
        }

        /// <summary>
        /// Gets or sets the frame outer bevel size
        /// </summary>
        [Editor(typeof(HalfRadiusRangeValueEditor), typeof(UITypeEditor))]
        public float OuterBevel
        {
            get { return (_GaugeControl.Frame.OuterBevel); }
            set { GetFramePropertyByName("OuterBevel").SetValue(_GaugeControl.Frame, value); }
        }

        /// <summary>
        /// Gets or sets the RoundRectangleArc size
        /// </summary>
        [Editor(typeof(HalfRadiusRangeValueEditor), typeof(UITypeEditor))]
        public float RoundRectangleArc
        {
            get { return (_GaugeControl.Frame.RoundRectangleArc); }
            set { GetFramePropertyByName("RoundRectangleArc").SetValue(_GaugeControl.Frame, value); }
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
                TypeDescriptor.GetProperties(_GaugeControl)[propName];

            if (prop == null)
                throw new ArgumentException("Matching property not found.", propName);

            return (prop);
        }

        #endregion

        #region GetFramePropertyByName

        /// <summary>
        /// Gets the Frame property via the given name
        /// </summary>
        /// <param name="propName">Property name</param>
        /// <returns>PropertyDescriptor</returns>
        private PropertyDescriptor GetFramePropertyByName(string propName)
        {
            PropertyDescriptor prop =
                TypeDescriptor.GetProperties(_GaugeControl.Frame)[propName];

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

            items.Add(new DesignerActionMethodItem(this, "AddCircularScale", "Add Circular Scale", "Scale"));
            items.Add(new DesignerActionMethodItem(this, "AddHLinearScale", "Add Horizontal Linear Scale", "Scale"));
            items.Add(new DesignerActionMethodItem(this, "AddVLinearScale", "Add Vertical Linear Scale", "Scale"));

            items.Add(new DesignerActionHeaderItem("Gauge Frame"));

            items.Add(new DesignerActionPropertyItem("FrameStyle", "Style",
                "Gauge Frame", "Sets the frame Style."));

            items.Add(new DesignerActionPropertyItem("InnerBevel", "Inner Bevel",
                "Gauge Frame", "Sets the frame inner bevel size."));

            items.Add(new DesignerActionPropertyItem("OuterBevel", "Outer Bevel",
                "Gauge Frame", "Sets the frame outer bevel size."));

            items.Add(new DesignerActionPropertyItem("RoundRectangleArc", "Round Rectangle Arc",
                "Gauge Frame", "Sets the rectangle corner arc."));

            items.Add(new DesignerActionTextItem(
                "Frame sizes are measured as a\n" +
                "percentage of the width/height of the Gauge", "Gauge Frame"));

            return (items);
        }

        #endregion

        #region AddCircularScale

        /// <summary>
        /// Adds a CircularScale
        /// </summary>
        public void AddCircularScale()
        {
            GaugeCircularScale scale = NewCircularScale();

            _GaugeControl.CircularScales.Add(scale);

            GaugePropertyGrid prop = new GaugePropertyGrid();

            prop.SelectedObject = scale;

            DialogResult result = prop.ShowDialog();

            if (result != DialogResult.OK)
                _GaugeControl.CircularScales.Remove(scale);
        }

        #region NewCircularScale

        private GaugeCircularScale NewCircularScale()
        {
            GaugeCircularScale scale = new GaugeCircularScale(_GaugeControl);

            scale.Name = GetScaleName(scale);

            GaugeSection section = new GaugeSection(scale);
            section.Name = GetSectionName(scale);

            section.FillColor = new GradientFillColor(Color.CornflowerBlue);

            scale.Sections.Add(section);

            return (scale);
        }

        #endregion

        #endregion

        #region AddHLinearScale

        /// <summary>
        /// Adds a Horizontal LinearScale
        /// </summary>
        public void AddHLinearScale()
        {
            GaugeLinearScale scale = NewLinearScale(Orientation.Horizontal);

            _GaugeControl.LinearScales.Add(scale);

            GaugePropertyGrid prop = new GaugePropertyGrid();

            prop.SelectedObject = scale;

            DialogResult result = prop.ShowDialog();

            if (result != DialogResult.OK)
                _GaugeControl.LinearScales.Remove(scale);
        }

        #endregion

        #region AddVLinearScale

        /// <summary>
        /// Adds a Vertical LinearScale
        /// </summary>
        public void AddVLinearScale()
        {
            GaugeLinearScale scale = NewLinearScale(Orientation.Vertical);

            _GaugeControl.LinearScales.Add(scale);

            GaugePropertyGrid prop = new GaugePropertyGrid();

            prop.SelectedObject = scale;

            DialogResult result = prop.ShowDialog();

            if (result != DialogResult.OK)
                _GaugeControl.LinearScales.Remove(scale);
        }

        #endregion

        #region NewLinearScale

        private GaugeLinearScale NewLinearScale(Orientation orientation)
        {
            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Orientation = orientation;
            scale.Name = GetScaleName(scale);

            scale.MinPin.EndOffset = .028f;
            scale.MaxPin.EndOffset = .028f;

            GaugeSection section = new GaugeSection(scale);
            section.Name = GetSectionName(scale);
            section.FillColor = new GradientFillColor(Color.CornflowerBlue);

            scale.Sections.Add(section);

            return (scale);
        }

        #endregion

        #region GetScaleName

        private string GetScaleName(GaugeCircularScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Scale" + i.ToString();

                if (scale.GaugeControl.CircularScales[s] == null)
                    return (s);
            }

            return (null);
        }

        private string GetScaleName(GaugeLinearScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Scale" + i.ToString();

                if (scale.GaugeControl.LinearScales[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #region GetSectionName

        private string GetSectionName(GaugeScale scale)
        {
            for (int i = 1; i < 100; i++)
            {
                string s = "Section" + i.ToString();

                if (scale.Sections[s] == null)
                    return (s);
            }

            return (null);
        }

        #endregion

        #region GetDefaultOrientation

        private Orientation GetDefaultOrientation(GaugeLinearScale scale)
        {
            foreach (GaugeLinearScale gls in scale.GaugeControl.LinearScales)
            {
                if (gls != scale)
                    return (gls.Orientation);
            }

            return (scale.GaugeControl.Width > scale.GaugeControl.Height
                ? Orientation.Horizontal : Orientation.Vertical);
        }

        #endregion
    }
}
