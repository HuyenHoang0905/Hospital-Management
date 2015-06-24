using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.DotNetBar.Design
{
    public class MicroChartItemDesigner : BaseItemDesigner
    {
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            if (component != null && component.Site != null && component.Site.DesignMode)
                SetDataPointsDefaults();
        }
        private void SetDataPointsDefaults()
        {
            MicroChartItem item = (MicroChartItem)this.Component;
            item.DataPoints = GetRandomDataPoints();
        }

        public override void InitializeExistingComponent(System.Collections.IDictionary defaultValues)
        {
            SetDataPointsDefaults();
            base.InitializeExistingComponent(defaultValues);
        }

        protected override void SetDesignTimeDefaults()
        {
            SetDataPointsDefaults();
            base.SetDesignTimeDefaults();
        }

        internal static List<double> GetRandomDataPoints(bool allowNegative)
        {
            return GetRandomDataPoints(allowNegative, 12);
        }
        internal static List<double> GetRandomDataPoints(bool allowNegative, int pointsToCreate)
        {
            List<double> points = new List<double>();
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Random rnd2 = new Random();

            for (int i = 0; i < pointsToCreate; i++)
            {
                points.Add(allowNegative ? ((rnd2.Next(50) > 25 ? 1 : -1) * rnd.Next(100)) : rnd.Next(100));
            }

            return points;
        }
        internal static List<double> GetRandomDataPoints()
        {
            return GetRandomDataPoints(true);
        }

    }
}
