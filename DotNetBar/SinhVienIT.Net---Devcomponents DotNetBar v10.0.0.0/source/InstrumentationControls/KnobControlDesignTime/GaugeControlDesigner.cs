using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBarKnobControl.Design;
using Microsoft.Win32;

namespace DevComponents.Instrumentation.Design
{
    /// <summary>
    /// KnobControlDesigner
    /// </summary>
    public class GaugeControlDesigner : ControlDesigner
    {
        #region Private variables

        private GaugeControl _GaugeControl;
        private DesignerActionListCollection _ActionLists;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes our designer
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (component.Site.DesignMode == true)
                _GaugeControl = component as GaugeControl;

#if !TRIAL
            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (dh != null)
                dh.LoadComplete += DhLoadComplete;
#endif
        }

        #endregion

        #region InitializeNewComponent

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            GaugeStyleDialog gsd = new GaugeStyleDialog();

            gsd.ShowDialog();

            if (Component != null && Component.Site != null && Component.Site.DesignMode == true)
            {
                if (_GaugeControl != null)
                {
                    switch (gsd.CbSelected)
                    {
                        case "cbCircular":
                            SetCircularDesignTimeDefaults();
                            break;

                        case "cbC2Scales":
                            SetC2ScalesDesignTimeDefaults();
                            break;

                        case "cbCInsetScale":
                            SetCInsetScaleDesignTimeDefaults();
                            break;

                        case "cbCTopMeter":
                            SetCTopMeterDesignTimeDefaults();
                            break;

                        case "cbCBottomMeter":
                            SetCBottomMeterDesignTimeDefaults();
                            break;

                        case "cbHorizontal":
                            SetHorizontalDesignTimeDefaults();
                            break;

                        case "cbH2Scales":
                            SetH2ScalesDesignTimeDefaults();
                            break;

                        case "cbHMultiBars":
                            SetHMultiBarsDesignTimeDefaults();
                            break;

                        case "cbHThermometer":
                            SetHThermometerDesignTimeDefaults();
                            break;

                        case "cbVertical":
                            SetVerticalDesignTimeDefaults();
                            break;

                        case "cbV2Scales":
                            SetV2ScalesDesignTimeDefaults();
                            break;

                        case "cbVMultiBars":
                            SetVMultiBarsDesignTimeDefaults();
                            break;

                        case "cbVThermometer":
                            SetVThermometerDesignTimeDefaults();
                            break;

                        default:
                            SetCircularDesignTimeDefaults();
                            break;
                    }
                }
            }

#if !TRIAL
            GaugeControl gauge = Control as GaugeControl;

            if (gauge != null)
                gauge.LicenseKey = GetLicenseKey();
#endif
        }

        #endregion

        #region SetCircularDesignTimeDefaults

        private void SetCircularDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Circular;

            SetBaseGuageColor();

            GaugeCircularScale scale = new GaugeCircularScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Radius = .38f;

            _GaugeControl.CircularScales.Add(scale);

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";
            section.FillColor = new GradientFillColor(Color.CornflowerBlue, Color.Purple);

            scale.Sections.Add(section);

            GaugeRange range = new GaugeRange(scale);
            range.Name = "Range1";

            range.FillColor = new GradientFillColor(Color.Lime, Color.Red);
            range.FillColor.BorderColor = Color.DimGray;
            range.FillColor.BorderWidth = 1;

            range.ScaleOffset = .28f;
            range.StartValue = 70;

            scale.Ranges.Add(range);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Needle;
            pointer.Length = 0.358F;

            pointer.FillColor = new GradientFillColor(Color.WhiteSmoke, Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            pointer.CapFillColor = new GradientFillColor(Color.WhiteSmoke, Color.DimGray, 90);
            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;

            scale.Pointers.Add(pointer);
        }

        #endregion

        #region SetC2ScalesDesignTimeDefaults

        private void SetC2ScalesDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Circular;

            SetBaseGuageColor();

            GaugeCircularScale scale1 = new GaugeCircularScale(_GaugeControl);

            scale1.Name = "Scale1";
            scale1.Radius = .38f;

            _GaugeControl.CircularScales.Add(scale1);

            GaugeSection section = new GaugeSection(scale1);

            section.Name = "Section1";
            section.FillColor = new GradientFillColor(Color.CornflowerBlue, Color.Purple);

            scale1.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale1);

            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Needle;
            pointer.Length = 0.358F;

            pointer.FillColor = new GradientFillColor(Color.WhiteSmoke, Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            pointer.CapFillColor = new GradientFillColor(Color.WhiteSmoke, Color.DimGray, 90);
            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;

            scale1.Pointers.Add(pointer);

            GaugeCircularScale scale2 = new GaugeCircularScale(_GaugeControl);

            scale2.Name = "Scale2";
            scale2.Radius = .24f;
            scale2.MaxValue = 200;

            scale2.MajorTickMarks.Interval = 20;
            scale2.MaxValue = 200;
            scale2.MinorTickMarks.Interval = 4;
            scale2.Name = "Scale2";
            scale2.Radius = 0.24F;

            _GaugeControl.CircularScales.Add(scale2);

            section = new GaugeSection(scale2);

            section.Name = "Section1";
            section.FillColor.Color1 = Color.CornflowerBlue;
            section.Name = "Section1";
            scale2.Sections.Add(section);
        }

        #endregion

        #region SetCInsetScaleDesignTimeDefaults

        private void SetCInsetScaleDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Circular;

            SetBaseGuageColor();

            GaugeCircularScale scale1 = new GaugeCircularScale(_GaugeControl);
            scale1.Name = "Scale1";
            scale1.Radius = 0.092F;
            scale1.Width = 0.139F;

            scale1.MaxValue = 10;
            scale1.Labels.Layout.RotateLabel = false;
            scale1.Labels.Layout.Font = new Font("Microsoft Sans Serif", 18F);

            GradientFillColor fillColor = new GradientFillColor(Color.White);
            fillColor.BorderColor = Color.DimGray;
            fillColor.BorderWidth = 1;

            scale1.MajorTickMarks.Interval = 1;
            scale1.MajorTickMarks.Layout.FillColor = fillColor;
            scale1.MajorTickMarks.Layout.Length = 0.263F;
            scale1.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Circle;
            scale1.MajorTickMarks.Layout.Width = 0.263F;

            scale1.MinorTickMarks.Interval = 0.5;
            scale1.MinorTickMarks.Layout.FillColor = new GradientFillColor(Color.Black);
            scale1.MinorTickMarks.Layout.Length = 0.2F;
            scale1.PivotPoint = new PointF(.50f, .68f);

            GaugePointer pointer = new GaugePointer(scale1);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Needle;
            pointer.Length = 0.54F;
            pointer.Width = 0.2F;
            pointer.Placement = DisplayPlacement.Near;
            pointer.NeedleStyle = NeedlePointerStyle.Style6;

            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;
            pointer.CapFillColor.Color1 = Color.WhiteSmoke;
            pointer.CapFillColor.Color2 = Color.Brown;
            pointer.CapFillColor.GradientFillType = GradientFillType.Center;

            pointer.CapStyle = NeedlePointerCapStyle.Style1;
            pointer.CapWidth = 0.4F;

            pointer.FillColor.BorderColor = Color.DarkSlateGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Turquoise;

            scale1.Pointers.Add(pointer);

            GaugeSection section = new GaugeSection(scale1);
            section.FillColor.Color1 = Color.CornflowerBlue;
            section.Name = "Section1";

            scale1.Sections.Add(section);

            _GaugeControl.CircularScales.Add(scale1);

            GaugeCircularScale scale2 = new GaugeCircularScale(_GaugeControl);
            scale2.Name = "Scale2";
            scale2.Radius = .38f;

            section = new GaugeSection(scale2);
            section.Name = "Section1";
            section.FillColor = new GradientFillColor(Color.CornflowerBlue, Color.Purple);

            scale2.Sections.Add(section);

            pointer = new GaugePointer(scale2);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Needle;
            pointer.Length = 0.358F;

            pointer.FillColor = new GradientFillColor(Color.WhiteSmoke, Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            pointer.CapFillColor = new GradientFillColor(Color.WhiteSmoke, Color.DimGray, 90);
            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;

            scale2.Pointers.Add(pointer);

            _GaugeControl.CircularScales.Add(scale2);
        }

        #endregion

        #region SetCTopMeterDesignTimeDefaults

        private void SetCTopMeterDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;
            _GaugeControl.Size = new Size(_GaugeControl.Size.Width, _GaugeControl.Size.Width / 3);

            SetBaseGuageColor();

            GaugeCircularScale scale = new GaugeCircularScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Radius = 2.5F;

            scale.StartAngle = 60F;
            scale.SweepAngle = 60F;
            scale.Width = 0.011F;

            scale.MajorTickMarks.Interval = 50;
            scale.MajorTickMarks.Layout.Length = 0.063F;
            scale.MajorTickMarks.Layout.Width = 0.033F;

            scale.MaxPin.EndOffset = 0.01F;
            scale.MaxPin.Length = 0.026F;
            scale.MaxPin.ScaleOffset = -0.06F;
            scale.MaxPin.Width = 0.026F;

            GradientFillColor fillColor = new GradientFillColor(Color.LightYellow);
            fillColor.BorderColor = Color.DimGray;
            fillColor.BorderWidth = 1;

            scale.MinorTickMarks.Interval = 10;
            scale.MinorTickMarks.Layout.FillColor = fillColor;
            scale.MinorTickMarks.Layout.Length = 0.042F;
            scale.MinorTickMarks.Layout.Width = 0.013F;

            scale.MinPin.EndOffset = 0.01F;
            scale.MinPin.Length = 0.026F;
            scale.MinPin.ScaleOffset = -0.06F;
            scale.MinPin.Width = 0.026F;

            scale.PivotPoint = new PointF(.5f, -1.7f);

            scale.Labels.Layout.Font = new Font("Microsoft Sans Serif", 4F);
            scale.Labels.Layout.ScaleOffset = 0.075F;

            GaugeRange range = new GaugeRange(scale);
            range.Name = "Range1";
            range.ScaleOffset = 0.033F;

            range.StartValue = 0;
            range.StartWidth = 0.07F;
            range.EndValue = 100;
            range.EndWidth = 0.07F;

            range.FillColor.BorderColor = Color.Gray;
            range.FillColor.BorderWidth = 1;
            range.FillColor.Color1 = Color.Yellow;
            range.FillColor.Color2 = Color.Lime;

            scale.Ranges.Add(range);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";

            pointer.Style = PointerStyle.Needle;
            pointer.ScaleOffset = -0.057F;
            pointer.Width = 0.032F;

            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;
            pointer.CapFillColor.Color1 = Color.Gainsboro;
            pointer.CapFillColor.Color2 = Color.Silver;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Black;

            scale.Pointers.Add(pointer);

            _GaugeControl.CircularScales.Add(scale);
        }

        #endregion

        #region SetCBottomMeterDesignTimeDefaults

        private void SetCBottomMeterDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;
            _GaugeControl.Frame.BackColor = new GradientFillColor(Color.Gainsboro, Color.DarkGray);
            _GaugeControl.Frame.FrameColor = new GradientFillColor(Color.White, Color.DimGray);
            _GaugeControl.Frame.FrameColor.BorderColor = Color.Gainsboro;
            _GaugeControl.Frame.FrameColor.BorderWidth = 1;

            _GaugeControl.Size = new Size(_GaugeControl.Size.Width, _GaugeControl.Size.Width / 3);

            GaugeCircularScale scale = new GaugeCircularScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Radius = 2.5F;

            scale.StartAngle = 240F;
            scale.SweepAngle = 60F;
            scale.Width = 0.011F;

            scale.MajorTickMarks.Interval = 50;
            scale.MajorTickMarks.Layout.Length = 0.063F;
            scale.MajorTickMarks.Layout.Width = 0.033F;

            scale.MaxPin.EndOffset = 0.01F;
            scale.MaxPin.Length = 0.026F;
            scale.MaxPin.ScaleOffset = -0.06F;
            scale.MaxPin.Width = 0.026F;

            GradientFillColor fillColor = new GradientFillColor(Color.LightYellow);
            fillColor.BorderColor = Color.DimGray;
            fillColor.BorderWidth = 1;

            scale.MinorTickMarks.Interval = 10;
            scale.MinorTickMarks.Layout.FillColor = fillColor;
            scale.MinorTickMarks.Layout.Length = 0.042F;
            scale.MinorTickMarks.Layout.Width = 0.013F;

            scale.MinPin.EndOffset = 0.01F;
            scale.MinPin.Length = 0.026F;
            scale.MinPin.ScaleOffset = -0.06F;
            scale.MinPin.Width = 0.026F;

            scale.PivotPoint = new PointF(.5f, 2.7f);

            scale.Labels.Layout.Font = new Font("Microsoft Sans Serif", 4F);
            scale.Labels.Layout.ScaleOffset = 0.075F;

            GaugeRange range = new GaugeRange(scale);
            range.Name = "Range1";
            range.ScaleOffset = 0.033F;

            range.StartValue = 0;
            range.StartWidth = 0.07F;
            range.EndValue = 100;
            range.EndWidth = 0.07F;

            range.FillColor.BorderColor = Color.Gray;
            range.FillColor.BorderWidth = 1;
            range.FillColor.Color1 = Color.Yellow;
            range.FillColor.Color2 = Color.Lime;

            scale.Ranges.Add(range);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";

            pointer.Style = PointerStyle.Needle;
            pointer.ScaleOffset = -0.057F;
            pointer.Width = 0.032F;

            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;
            pointer.CapFillColor.Color1 = Color.Gainsboro;
            pointer.CapFillColor.Color2 = Color.Silver;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Black;

            scale.Pointers.Add(pointer);

            _GaugeControl.CircularScales.Add(scale);
        }

        #endregion

        #region SetHorizontalDesignTimeDefaults

        private void SetHorizontalDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.MaxValue = 50;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            _GaugeControl.LinearScales.Add(scale);

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";
            section.FillColor = new GradientFillColor(Color.CornflowerBlue, Color.Purple);

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Marker;
            pointer.Placement = DisplayPlacement.Far;
            pointer.ScaleOffset = .05f;

            pointer.FillColor = new GradientFillColor(Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            Color color1 = Color.FromArgb(100, 60, 60, 60);

            pointer.ThermoBackColor = new GradientFillColor(color1);
            pointer.ThermoBackColor.BorderColor = Color.Black;
            pointer.ThermoBackColor.BorderWidth = 1;

            scale.Pointers.Add(pointer);
        }

        #endregion

        #region SetH2ScalesDesignTimeDefaults

        private void SetH2ScalesDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.MaxValue = 50;
            scale.Width = 0.14F;

            scale.BorderColor = Color.Gray;
            scale.BorderWidth = 1;
            scale.Location = new PointF(.5f, .53f);

            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MajorTickMarks.Layout.Width = 0.047F;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Near;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";

            section.FillColor.Color1 = Color.CornflowerBlue;

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Bar;

            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;
            pointer.CapFillColor.Color1 = Color.WhiteSmoke;
            pointer.CapFillColor.Color2 = Color.DimGray;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Red;

            pointer.Value = 15;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);

            scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale2";
            scale.Width = 0.14F;
            scale.MaxValue = 25;

            scale.Location = new PointF(.5f, .53f);

            scale.Labels.Layout.Placement = DisplayPlacement.Far;
            scale.Labels.Layout.ScaleOffset = 0.054F;

            scale.MajorTickMarks.Interval = 5;
            scale.MajorTickMarks.Layout.Width = 0.047F;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Far;

            scale.MinorTickMarks.Interval = 1.25f;
            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Far;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            _GaugeControl.LinearScales.Add(scale);
        }

        #endregion

        #region SetHMultiBarsDesignTimeDefaults

        private void SetHMultiBarsDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.MaxValue = 50;
            scale.Width = 0;

            scale.Labels.Layout.Placement = DisplayPlacement.Far;
            scale.Labels.Layout.ScaleOffset = -0.016F;
            scale.Location = new PointF(.5f, .68f);

            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Far;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.044F;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Far;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.ScaleOffset = 0.022F;
            pointer.Placement = DisplayPlacement.Near;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Yellow;

            pointer.Value = 15;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer2";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.ScaleOffset = 0.19F;
            pointer.Placement = DisplayPlacement.Near;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Gold;

            pointer.Value = 25;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer3";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.ScaleOffset = 0.358F;
            pointer.Placement = DisplayPlacement.Near;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Chartreuse;

            pointer.Value = 35;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer4";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.ScaleOffset = 0.526F;
            pointer.Placement = DisplayPlacement.Near;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Turquoise;

            pointer.Value = 50;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);
        }

        #endregion

        #region SetHMThermometerDesignTimeDefaults

        private void SetHThermometerDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Width = 0.1F;

            scale.Location = new PointF(.55f, .51f);

            scale.MinValue = -30;
            scale.MaxValue = 40;

            scale.Labels.FormatString = "0°";

            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.008F;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MinorTickMarks.Layout.Width = 0.016F;

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";

            section.FillColor.Color1 = Color.SteelBlue;
            section.FillColor.Color2 = Color.LightCyan;
            section.FillColor.GradientFillType = GradientFillType.HorizontalCenter;

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Width = 0.1F;

            pointer.Style = PointerStyle.Thermometer;
            pointer.BulbSize = 0.132F;
            pointer.BulbOffset = .026F;

            pointer.FillColor.Color1 = Color.Red;
            pointer.FillColor.Color2 = Color.Empty;
            pointer.ThermoBackColor.Color1 = Color.FromArgb(100, 60, 60, 60);

            pointer.Value = 12;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);

            scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale2";

            scale.Width = 0.1F;
            scale.Location = new PointF(.55f, .51f);

            scale.MinValue = -22;
            scale.MaxValue = 104;

            scale.Labels.FormatString = "0°";
            scale.Labels.Layout.Placement = DisplayPlacement.Far;
            scale.Labels.ShowMaxLabel = false;
            scale.Labels.ShowMinLabel = false;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            scale.MajorTickMarks.Interval = 20;
            scale.MajorTickMarks.IntervalOffset = 2;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Far;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.008F;

            scale.MinorTickMarks.Interval = 4;
            scale.MinorTickMarks.IntervalOffset = 2;
            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Far;
            scale.MinorTickMarks.Layout.Width = 0.016F;

            _GaugeControl.LinearScales.Add(scale);

            GaugeText text = new GaugeText(_GaugeControl);
            text.Location = new PointF(.08f, .28f);
            text.Name = "Text1";
            text.Text = "C°";

            _GaugeControl.GaugeItems.Add(text);

            text = new GaugeText(_GaugeControl);
            text.Location = new PointF(.08f, .73f);
            text.Name = "Text2";
            text.Text = "F°";

            _GaugeControl.GaugeItems.Add(text);
        }

        #endregion

        #region SetVerticalDesignTimeDefaults

        private void SetVerticalDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Orientation = Orientation.Vertical;

            scale.MaxValue = 50;
            scale.Labels.Layout.ScaleOffset = .03f;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            _GaugeControl.LinearScales.Add(scale);

            GaugeSection section = new GaugeSection(scale);

            section.Name = "Section1";
            section.FillColor = new GradientFillColor(Color.CornflowerBlue, Color.Purple);

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);

            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Marker;
            pointer.Placement = DisplayPlacement.Far;
            pointer.ScaleOffset = .05f;

            pointer.FillColor = new GradientFillColor(Color.Red);
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            Color color1 = Color.FromArgb(100, 60, 60, 60);

            pointer.ThermoBackColor = new GradientFillColor(color1);
            pointer.ThermoBackColor.BorderColor = Color.Black;
            pointer.ThermoBackColor.BorderWidth = 1;

            scale.Pointers.Add(pointer);
        }

        #endregion

        #region SetV2ScalesDesignTimeDefaults

        private void SetV2ScalesDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Orientation = Orientation.Vertical;

            scale.MaxValue = 50;
            scale.Width = 0.14F;

            scale.BorderColor = Color.Gray;
            scale.BorderWidth = 1;
            scale.Location = new PointF(.5f, .53f);

            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MajorTickMarks.Layout.Width = 0.047F;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Near;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";

            section.FillColor.Color1 = Color.CornflowerBlue;

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Bar;

            pointer.CapFillColor.BorderColor = Color.DimGray;
            pointer.CapFillColor.BorderWidth = 1;
            pointer.CapFillColor.Color1 = Color.WhiteSmoke;
            pointer.CapFillColor.Color2 = Color.DimGray;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Red;

            pointer.Value = 15;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);

            scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale2";
            scale.Orientation = Orientation.Vertical;
            
            scale.Width = 0.14F;
            scale.MaxValue = 25;

            scale.Location = new PointF(.5f, .53f);

            scale.Labels.Layout.Placement = DisplayPlacement.Far;
            scale.Labels.Layout.ScaleOffset = 0.054F;

            scale.MajorTickMarks.Interval = 5;
            scale.MajorTickMarks.Layout.Width = 0.047F;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Far;

            scale.MinorTickMarks.Interval = 1.25f;
            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Far;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            _GaugeControl.LinearScales.Add(scale);
        }

        #endregion

        #region SetVMultiBarsDesignTimeDefaults

        private void SetVMultiBarsDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Orientation = Orientation.Vertical;

            scale.MaxValue = 50;
            scale.Width = 0;

            scale.Labels.Layout.Placement = DisplayPlacement.Near;
            scale.Location = new PointF(.38f, .5f);

            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.044F;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Near;

            scale.MaxPin.Visible = false;
            scale.MinPin.Visible = false;

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.Width = .12f;
            pointer.ScaleOffset = 0.022F;
            pointer.Placement = DisplayPlacement.Far;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Yellow;

            pointer.Value = 15;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer2";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.Width = .12f;
            pointer.ScaleOffset = 0.17F;
            pointer.Placement = DisplayPlacement.Far;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Gold;

            pointer.Value = 25;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer3";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.Width = .12f;
            pointer.ScaleOffset = 0.318F;
            pointer.Placement = DisplayPlacement.Far;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Chartreuse;

            pointer.Value = 35;

            scale.Pointers.Add(pointer);

            pointer = new GaugePointer(scale);
            pointer.Name = "Pointer4";
            pointer.Style = PointerStyle.Bar;
            pointer.BarStyle = BarPointerStyle.Rounded;

            pointer.Width = .12f;
            pointer.ScaleOffset = 0.46F;
            pointer.Placement = DisplayPlacement.Far;

            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;
            pointer.FillColor.Color1 = Color.Turquoise;

            pointer.Value = 50;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);
        }

        #endregion

        #region SetVMThermometerDesignTimeDefaults

        private void SetVThermometerDesignTimeDefaults()
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;

            SetBaseGuageColor();

            GaugeLinearScale scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale1";
            scale.Orientation = Orientation.Vertical;

            scale.Width = 0.1F;
            scale.Size = new SizeF(.75f, .75f);
            scale.Location = new PointF(.50f, .46f);

            scale.MinValue = -30;
            scale.MaxValue = 40;

            scale.Labels.FormatString = "0°";

            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.008F;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Near;
            scale.MinorTickMarks.Layout.Width = 0.016F;

            GaugeSection section = new GaugeSection(scale);
            section.Name = "Section1";

            section.FillColor.Color1 = Color.SteelBlue;
            section.FillColor.Color2 = Color.LightCyan;
            section.FillColor.GradientFillType = GradientFillType.VerticalCenter;

            scale.Sections.Add(section);

            GaugePointer pointer = new GaugePointer(scale);
            pointer.Name = "Pointer1";
            pointer.Width = 0.1F;

            pointer.Style = PointerStyle.Thermometer;
            pointer.BulbSize = 0.132F;
            pointer.BulbOffset = .026F;

            pointer.FillColor.Color1 = Color.Red;
            pointer.FillColor.Color2 = Color.Empty;
            pointer.FillColor.BorderColor = Color.DimGray;
            pointer.FillColor.BorderWidth = 1;

            pointer.ThermoBackColor.Color1 = Color.FromArgb(100, 60, 60, 60);
            pointer.FillColor.BorderColor = Color.Black;
            pointer.FillColor.BorderWidth = 1;

            pointer.Value = 12;

            scale.Pointers.Add(pointer);

            _GaugeControl.LinearScales.Add(scale);

            scale = new GaugeLinearScale(_GaugeControl);
            scale.Name = "Scale2";
            scale.Orientation = Orientation.Vertical;

            scale.Width = 0.1F;
            scale.Size = new SizeF(.75f, .75f);
            scale.Location = new PointF(.50f, .46f);

            scale.MinValue = -22;
            scale.MaxValue = 104;

            scale.Labels.FormatString = "0°";
            scale.Labels.Layout.Placement = DisplayPlacement.Far;
            scale.Labels.ShowMaxLabel = false;
            scale.Labels.ShowMinLabel = false;

            scale.MinPin.Visible = false;
            scale.MaxPin.Visible = false;

            scale.MajorTickMarks.Interval = 20;
            scale.MajorTickMarks.IntervalOffset = 2;
            scale.MajorTickMarks.Layout.Placement = DisplayPlacement.Far;
            scale.MajorTickMarks.Layout.Style = GaugeMarkerStyle.Rectangle;
            scale.MajorTickMarks.Layout.Width = 0.008F;

            scale.MinorTickMarks.Interval = 4;
            scale.MinorTickMarks.IntervalOffset = 2;
            scale.MinorTickMarks.Layout.Placement = DisplayPlacement.Far;
            scale.MinorTickMarks.Layout.Width = 0.016F;

            _GaugeControl.LinearScales.Add(scale);

            GaugeText text = new GaugeText(_GaugeControl);
            text.Location = new PointF(.26f, .9f);
            text.Name = "Text1";
            text.Text = "C°";

            _GaugeControl.GaugeItems.Add(text);

            text = new GaugeText(_GaugeControl);
            text.Location = new PointF(.76f, .9f);
            text.Name = "Text2";
            text.Text = "F°";

            _GaugeControl.GaugeItems.Add(text);
        }

        #endregion

        #region SetBaseGuageColor

        private void SetBaseGuageColor()
        {
            _GaugeControl.Frame.BackColor = new GradientFillColor(Color.Gainsboro, Color.DarkGray);

            _GaugeControl.Frame.FrameColor = new GradientFillColor(Color.White, Color.DimGray);
            _GaugeControl.Frame.FrameColor.BorderColor = Color.Gainsboro;
            _GaugeControl.Frame.FrameColor.BorderWidth = 1;
        }

        #endregion

        #region Verbs

        /// <summary>
        /// Creates our verb collection
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[]
                {
                    new DesignerVerb("Circular Frame", SetStyleCircular),
                    new DesignerVerb("Rectangular Frame", SetStyleRectangular),
                    new DesignerVerb("Rectangular Round Frame", SetStyleRoundedRectangular),
                    new DesignerVerb("No Frame", SetStyleNone),
                };

                return (new DesignerVerbCollection(verbs));
            }
        }

        #endregion

        #region SetStyle

        /// <summary>
        /// Sets the control to Style1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyleCircular(object sender, EventArgs e)
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Circular;
        }

        /// <summary>
        /// Sets the control to SetStyleRectangular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyleRectangular(object sender, EventArgs e)
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.Rectangular;
        }

        /// <summary>
        /// Sets the control to SetStyleRoundedRectangular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyleRoundedRectangular(object sender, EventArgs e)
        {
           _GaugeControl.Frame.Style = GaugeFrameStyle.RoundedRectangular;
        }

        /// <summary>
        /// Sets the control to None
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetStyleNone(object sender, EventArgs e)
        {
            _GaugeControl.Frame.Style = GaugeFrameStyle.None;
        }

        #endregion

        #region ActionLists

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionLists == null)
                {
                    _ActionLists = new DesignerActionListCollection();

                    _ActionLists.Add(new GaugeControlActionList(_GaugeControl));
                }

                return (_ActionLists);
            }
        }

        #endregion

        #region Licensing Stuff

#if !TRIAL
        private string GetLicenseKey()
        {
            string key = "";

            RegistryKey regkey =
                Registry.LocalMachine.OpenSubKey("Software\\DevComponents\\Licenses", false);

            if (regkey != null)
            {
                object keyValue = regkey.GetValue("DevComponents.DotNetBar.DotNetBarManager2");

                if (keyValue != null)
                    key = keyValue.ToString();
            }

            return (key);
        }

        private void DhLoadComplete(object sender, EventArgs e)
        {
            IDesignerHost dh = GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (dh != null)
                dh.LoadComplete -= DhLoadComplete;

            string key = GetLicenseKey();

            GaugeControl gauge = Control as GaugeControl;

            if (key != "" && gauge != null && gauge.LicenseKey == "" && gauge.LicenseKey != key)
                TypeDescriptor.GetProperties(gauge)["LicenseKey"].SetValue(gauge, key);
        }
#endif

        #endregion
    }
}
