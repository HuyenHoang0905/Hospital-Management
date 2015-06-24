using System;
using System.Text;
using System.ComponentModel.Design;
using DevComponents.AdvTree;
using System.Collections;

namespace DevComponents.AdvTree.Design
{
    /// <summary>
    /// Defines designer for ColumnHeader class.
    /// </summary>
    public class ColumnHeaderDesigner : ComponentDesigner
    {
        #region Internal Implementation
        #if FRAMEWORK20
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            SetDefaults();
            base.InitializeNewComponent(defaultValues);
        }
#else
		/// <summary>Sets design-time defaults for component.</summary>
		public override void OnSetComponentDefaults()
		{
			base.OnSetComponentDefaults();
			SetDefaults();
		}
#endif

        private void SetDefaults()
        {
            ColumnHeader ch = this.Component as ColumnHeader;
            ch.Width.Absolute = 150;
            ch.Text = "Column";
        }
        #endregion
    }
}
