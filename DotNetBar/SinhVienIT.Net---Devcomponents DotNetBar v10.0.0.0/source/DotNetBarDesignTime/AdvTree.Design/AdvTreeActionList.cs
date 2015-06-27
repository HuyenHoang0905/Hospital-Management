#if FRAMEWORK20
using System;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.AdvTree.Design
{
    internal class AdvTreeActionList : DesignerActionList
    {
        private AdvTreeDesigner _Designer = null;

        /// <summary>
        /// Initializes a new instance of the AdvTreeActionList class.
        /// </summary>
        /// <param name="designer"></param>
        public AdvTreeActionList(AdvTreeDesigner designer):base(designer.Component)
        {
            _Designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionHeaderItem("Nodes"));
            items.Add(new DesignerActionHeaderItem("Columns"));
            items.Add(new DesignerActionHeaderItem("Selection"));

            items.Add(new DesignerActionMethodItem(this, "CreateNode", "Create Node", "Nodes", true));
            items.Add(new DesignerActionPropertyItem("CellEdit", "Allow node text editing?", "Nodes", "Indicates whether node cells are editable"));

            items.Add(new DesignerActionMethodItem(this, "EditColumns", "Edit Columns...", "Columns", "Edit Tree Control Columns", true));
            items.Add(new DesignerActionPropertyItem("ColumnsVisible", "Column header visible?", "Columns", "Indicates whether tree column header is visible"));
            items.Add(new DesignerActionPropertyItem("GridColumnLines", "Show grid column lines?", "Columns", "Indicates whether grid lines are visible"));
            items.Add(new DesignerActionPropertyItem("GridRowLines", "Show grid row lines?", "Nodes", "Indicates whether grid lines between nodes are visible"));
            items.Add(new DesignerActionPropertyItem("GridLinesColor", "Grid lines color:", "Columns", "Indicates custom color for grid lines"));

            items.Add(new DesignerActionPropertyItem("SelectionBox", "Show selection?", "Selection", "Indicates whether selection is shown for selected node"));
            items.Add(new DesignerActionPropertyItem("SelectionBoxStyle", "Selection style:", "Selection", "Indicates selection style"));
            items.Add(new DesignerActionPropertyItem("HotTracking", "Highlight mouse over node?", "Selection", "Indicates whether node that mouse is over is highlighted"));
   
            return items;
        }

        public void CreateNode()
        {
            _Designer.CreateNode();
        }

        public void EditColumns()
        {
            _Designer.EditColumns();
        }

        public bool HotTracking
        {
            get
            {
                return ((AdvTree)base.Component).HotTracking;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["HotTracking"].SetValue(base.Component, value);
            }
        }

        public bool SelectionBox
        {
            get
            {
                return ((AdvTree)base.Component).SelectionBox;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["SelectionBox"].SetValue(base.Component, value);
            }
        }

        public eSelectionStyle SelectionBoxStyle
        {
            get
            {
                return ((AdvTree)base.Component).SelectionBoxStyle;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["SelectionBoxStyle"].SetValue(base.Component, value);
            }
        }

        public bool ColumnsVisible
        {
            get
            {
                return ((AdvTree)base.Component).ColumnsVisible;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["ColumnsVisible"].SetValue(base.Component, value);
            }
        }

        public bool GridColumnLines
        {
            get
            {
                return ((AdvTree)base.Component).GridColumnLines;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["GridColumnLines"].SetValue(base.Component, value);
            }
        }

        public bool GridRowLines
        {
            get
            {
                return ((AdvTree)base.Component).GridRowLines;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["GridRowLines"].SetValue(base.Component, value);
            }
        }

        public Color GridLinesColor
        {
            get
            {
                return ((AdvTree)base.Component).GridLinesColor;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["GridLinesColor"].SetValue(base.Component, value);
            }
        }

        public bool CellEdit
        {
            get
            {
                return ((AdvTree)base.Component).CellEdit;
            }
            set
            {
                TypeDescriptor.GetProperties(base.Component)["CellEdit"].SetValue(base.Component, value);
            }
        }
 

    }
}
#endif