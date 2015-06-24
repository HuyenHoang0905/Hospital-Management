using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design
{
    public class MicroChartDesigner : ControlDesigner
    {
        #region Constructor
        public MicroChartDesigner()
		{
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            if (!component.Site.DesignMode)
                return;

            SetDataPointsDefaults();
        }
        #endregion

        #region Internal Implementation
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

        private void SetDataPointsDefaults()
        {
            MicroChart chart = this.Control as MicroChart;
            if (chart == null)
                return;
            chart.DataPoints = MicroChartItemDesigner.GetRandomDataPoints();
        }
        private void SetDesignTimeDefaults()
        {
            MicroChart chart = this.Control as MicroChart;
            if (chart == null)
                return;
            chart.Style = eDotNetBarStyle.StyleManagerControlled;
        }

        #endregion
    }
}
