using System;
using System.Text;
using DevComponents.WinForms.Drawing;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.AdvTree.Display
{
    /// <summary>
    /// Initializes the tree color tables.
    /// </summary>
    internal class ColorTableInitializer
    {
        #region Office 2007 Blue
        public static void InitOffice2007Blue(TreeColorTable ct, ColorFactory factory)
        {
            #region Tree Selection
            TreeSelectionColors treeSelection = new TreeSelectionColors();
            ct.Selection = treeSelection;
            // Highlight full row
            SelectionColorTable selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xA7CDF0));
            treeSelection.FullRowSelect = selColorTable;
            //  Highlight full row Inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xE5E5E5));
            treeSelection.FullRowSelectInactive = selColorTable;

            // Node Marker
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0x316AC5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x316AC5), 1);
            treeSelection.NodeMarker = selColorTable;
            // Node marker inactibe
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0xE5E5E5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x000000), 1);
            treeSelection.NodeMarkerInactive = selColorTable;

            // Cell selection
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(new ColorStop[] {
                    new ColorStop(factory.GetColor(0xFFFCD9), 0f),
                    new ColorStop(factory.GetColor(0xFFE78D), .4f),
                    new ColorStop(factory.GetColor(0xFFD748), .4f),
                    new ColorStop(factory.GetColor(0xFFE793), 1f)
                });
            selColorTable.Border = new SolidBorder(factory.GetColor(0xDDCF9B), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFF2BE), 1);
            //selColorTable = new SelectionColorTable();
            //selColorTable.Fill = new GradientFill(factory.GetColor(0xF6FBFD), factory.GetColor(0xD5EFFC), 90);
            //selColorTable.Border = new SolidBorder(factory.GetColor(0x99DEFD), 1);
            //selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFFFFF), 1);
            treeSelection.HighlightCells = selColorTable;
            // Cell selection inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.HighlightCellsInactive = selColorTable;

            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.NodeHotTracking = selColorTable;
            #endregion

            #region Expand Buttons
            TreeExpandColorTable expand = new TreeExpandColorTable();
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x000000), 1);
            expand.CollapseFill = new SolidFill(factory.GetColor(0x595959));
            expand.CollapseMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.CollapseMouseOverFill = new SolidFill(factory.GetColor(0x82DFFB));
            expand.ExpandBorder = new SolidBorder(factory.GetColor(0x848484), 1);
            expand.ExpandFill = new SolidFill(factory.GetColor(0xFFFFFF));
            expand.ExpandMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.ExpandMouseOverFill = new SolidFill(factory.GetColor(0xCCEDFA));
            ct.ExpandTriangle = expand;
            // Rectangle
            expand = new TreeExpandColorTable();
            expand.CollapseForeground = new SolidFill(factory.GetColor(0x000000));
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x969696), 1);
            expand.CollapseFill = new GradientFill(new ColorStop[]{
                new ColorStop(factory.GetColor(0xFFFFFF), 0f), new ColorStop(factory.GetColor(0xFFFFFF), .40f), new ColorStop(factory.GetColor(0xB6B6B6), 1f)}, 45);
            expand.CollapseMouseOverForeground = expand.CollapseForeground;
            expand.CollapseMouseOverBorder = expand.CollapseBorder;
            expand.CollapseMouseOverFill = expand.CollapseFill;
            expand.ExpandForeground = expand.CollapseForeground;
            expand.ExpandBorder = expand.CollapseBorder;
            expand.ExpandFill = expand.CollapseFill;
            expand.ExpandMouseOverForeground = expand.CollapseForeground;
            expand.ExpandMouseOverBorder = expand.CollapseBorder;
            expand.ExpandMouseOverFill = expand.CollapseFill;
            ct.ExpandRectangle = expand;
            ct.ExpandEllipse = expand;
            #endregion

            #region Misc Tree Color
            ct.GridLines = factory.GetColor(0xE1E1E1);
            #endregion
        }
        #endregion

        #region Office 2007 Silver
        public static void InitOffice2007Silver(TreeColorTable ct, ColorFactory factory)
        {
            #region Tree Selection
            TreeSelectionColors treeSelection = new TreeSelectionColors();
            ct.Selection = treeSelection;
            // Highlight full row
            SelectionColorTable selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xA7CDF0));
            treeSelection.FullRowSelect = selColorTable;
            //  Highlight full row Inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xE5E5E5));
            treeSelection.FullRowSelectInactive = selColorTable;

            // Node Marker
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0x316AC5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x316AC5), 1);
            treeSelection.NodeMarker = selColorTable;
            // Node marker inactibe
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0xE5E5E5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x000000), 1);
            treeSelection.NodeMarkerInactive = selColorTable;

            // Cell selection
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(new ColorStop[] {
                    new ColorStop(factory.GetColor(0xFFFCD9), 0f),
                    new ColorStop(factory.GetColor(0xFFE78D), .4f),
                    new ColorStop(factory.GetColor(0xFFD748), .4f),
                    new ColorStop(factory.GetColor(0xFFE793), 1f)
                });
            selColorTable.Border = new SolidBorder(factory.GetColor(0xDDCF9B), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFF2BE), 1);
            //selColorTable = new SelectionColorTable();
            //selColorTable.Fill = new GradientFill(factory.GetColor(0xF6FBFD), factory.GetColor(0xD5EFFC), 90);
            //selColorTable.Border = new SolidBorder(factory.GetColor(0x99DEFD), 1);
            //selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFFFFF), 1);
            treeSelection.HighlightCells = selColorTable;
            // Cell selection inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.HighlightCellsInactive = selColorTable;

            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.NodeHotTracking = selColorTable;
            #endregion

            #region Expand Buttons
            TreeExpandColorTable expand = new TreeExpandColorTable();
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x000000), 1);
            expand.CollapseFill = new SolidFill(factory.GetColor(0x595959));
            expand.CollapseMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.CollapseMouseOverFill = new SolidFill(factory.GetColor(0x82DFFB));
            expand.ExpandBorder = new SolidBorder(factory.GetColor(0x848484), 1);
            expand.ExpandFill = new SolidFill(factory.GetColor(0xFFFFFF));
            expand.ExpandMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.ExpandMouseOverFill = new SolidFill(factory.GetColor(0xCCEDFA));
            ct.ExpandTriangle = expand;
            // Rectangle
            expand = new TreeExpandColorTable();
            expand.CollapseForeground = new SolidFill(factory.GetColor(0x000000));
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x969696), 1);
            expand.CollapseFill = new GradientFill(new ColorStop[]{
                new ColorStop(factory.GetColor(0xFFFFFF), 0f), new ColorStop(factory.GetColor(0xFFFFFF), .40f), new ColorStop(factory.GetColor(0xB6B6B6), 1f)}, 45);
            expand.CollapseMouseOverForeground = expand.CollapseForeground;
            expand.CollapseMouseOverBorder = expand.CollapseBorder;
            expand.CollapseMouseOverFill = expand.CollapseFill;
            expand.ExpandForeground = expand.CollapseForeground;
            expand.ExpandBorder = expand.CollapseBorder;
            expand.ExpandFill = expand.CollapseFill;
            expand.ExpandMouseOverForeground = expand.CollapseForeground;
            expand.ExpandMouseOverBorder = expand.CollapseBorder;
            expand.ExpandMouseOverFill = expand.CollapseFill;
            ct.ExpandRectangle = expand;
            ct.ExpandEllipse = expand;
            #endregion

            #region Misc Tree Color
            ct.GridLines = factory.GetColor(0xE1E1E1);
            #endregion
        }
        #endregion

        #region Office 2007 Black
        public static void InitOffice2007Black(TreeColorTable ct, ColorFactory factory)
        {
            #region Tree Selection
            TreeSelectionColors treeSelection = new TreeSelectionColors();
            ct.Selection = treeSelection;
            // Highlight full row
            SelectionColorTable selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xA7CDF0));
            treeSelection.FullRowSelect = selColorTable;
            //  Highlight full row Inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xE5E5E5));
            treeSelection.FullRowSelectInactive = selColorTable;

            // Node Marker
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0x316AC5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x316AC5), 1);
            treeSelection.NodeMarker = selColorTable;
            // Node marker inactibe
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0xE5E5E5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x000000), 1);
            treeSelection.NodeMarkerInactive = selColorTable;

            // Cell selection
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(new ColorStop[] {
                    new ColorStop(factory.GetColor(0xFFFCD9), 0f),
                    new ColorStop(factory.GetColor(0xFFE78D), .4f),
                    new ColorStop(factory.GetColor(0xFFD748), .4f),
                    new ColorStop(factory.GetColor(0xFFE793), 1f)
                });
            selColorTable.Border = new SolidBorder(factory.GetColor(0xDDCF9B), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFF2BE), 1);
            //selColorTable = new SelectionColorTable();
            //selColorTable.Fill = new GradientFill(factory.GetColor(0xF6FBFD), factory.GetColor(0xD5EFFC), 90);
            //selColorTable.Border = new SolidBorder(factory.GetColor(0x99DEFD), 1);
            //selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFFFFF), 1);
            treeSelection.HighlightCells = selColorTable;
            // Cell selection inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.HighlightCellsInactive = selColorTable;

            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xFAFAFB), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFFFFFF), 1);
            treeSelection.NodeHotTracking = selColorTable;
            #endregion

            #region Expand Buttons
            TreeExpandColorTable expand = new TreeExpandColorTable();
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x000000), 1);
            expand.CollapseFill = new SolidFill(factory.GetColor(0x595959));
            expand.CollapseMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.CollapseMouseOverFill = new SolidFill(factory.GetColor(0x82DFFB));
            expand.ExpandBorder = new SolidBorder(factory.GetColor(0x848484), 1);
            expand.ExpandFill = new SolidFill(factory.GetColor(0xFFFFFF));
            expand.ExpandMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.ExpandMouseOverFill = new SolidFill(factory.GetColor(0xCCEDFA));
            ct.ExpandTriangle = expand;
            // Rectangle
            expand = new TreeExpandColorTable();
            expand.CollapseForeground = new SolidFill(factory.GetColor(0x000000));
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x969696), 1);
            expand.CollapseFill = new GradientFill(new ColorStop[]{
                new ColorStop(factory.GetColor(0xFFFFFF), 0f), new ColorStop(factory.GetColor(0xFFFFFF), .40f), new ColorStop(factory.GetColor(0xB6B6B6), 1f)}, 45);
            expand.CollapseMouseOverForeground = expand.CollapseForeground;
            expand.CollapseMouseOverBorder = expand.CollapseBorder;
            expand.CollapseMouseOverFill = expand.CollapseFill;
            expand.ExpandForeground = expand.CollapseForeground;
            expand.ExpandBorder = expand.CollapseBorder;
            expand.ExpandFill = expand.CollapseFill;
            expand.ExpandMouseOverForeground = expand.CollapseForeground;
            expand.ExpandMouseOverBorder = expand.CollapseBorder;
            expand.ExpandMouseOverFill = expand.CollapseFill;
            ct.ExpandRectangle = expand;
            ct.ExpandEllipse = expand;
            #endregion

            #region Misc Tree Color
            ct.GridLines = factory.GetColor(0xE1E1E1);
            #endregion
        }
        #endregion

        #region Office 2007 Vista Glass
        public static void InitOffice2007VistaGlass(TreeColorTable ct, ColorFactory factory)
        {
            #region Tree Selection
            TreeSelectionColors treeSelection = new TreeSelectionColors();
            ct.Selection = treeSelection;
            // Highlight full row
            SelectionColorTable selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xC4E8FA));
            treeSelection.FullRowSelect = selColorTable;
            //  Highlight full row Inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(0xE5E5E5));
            treeSelection.FullRowSelectInactive = selColorTable;

            // Node Marker
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0x316AC5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x316AC5), 1);
            treeSelection.NodeMarker = selColorTable;
            // Node marker inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new SolidFill(factory.GetColor(64, 0xE5E5E5));
            selColorTable.Border = new SolidBorder(factory.GetColor(96, 0x000000), 1);
            treeSelection.NodeMarkerInactive = selColorTable;

            // Cell selection
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(new ColorStop[] {
                    new ColorStop(factory.GetColor(0xF1F8FD), 0f),
                    new ColorStop(factory.GetColor(0xD5EFFC), 1f)
                });
            selColorTable.Border = new SolidBorder(factory.GetColor(0x99DEFD), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xF6FBFD), 1);
            //selColorTable = new SelectionColorTable();
            //selColorTable.Fill = new GradientFill(factory.GetColor(0xF6FBFD), factory.GetColor(0xD5EFFC), 90);
            //selColorTable.Border = new SolidBorder(factory.GetColor(0x99DEFD), 1);
            //selColorTable.InnerBorder = new SolidBorder(factory.GetColor(192, 0xFFFFFF), 1);
            treeSelection.HighlightCells = selColorTable;
            // Cell selection inactive
            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xF8F8F8), factory.GetColor(0xE5E5E5), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD9D9D9), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xFAFAFB), 1);
            treeSelection.HighlightCellsInactive = selColorTable;

            selColorTable = new SelectionColorTable();
            selColorTable.Fill = new GradientFill(factory.GetColor(0xF5FAFD), factory.GetColor(0xE8F5FD), 90);
            selColorTable.Border = new SolidBorder(factory.GetColor(0xD8F0FA), 1);
            selColorTable.InnerBorder = new SolidBorder(factory.GetColor(228, 0xF8FCFE), 1);
            treeSelection.NodeHotTracking = selColorTable;
            #endregion

            #region Expand Buttons
            TreeExpandColorTable expand = new TreeExpandColorTable();
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x000000), 1);
            expand.CollapseFill = new SolidFill(factory.GetColor(0x595959));
            expand.CollapseMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.CollapseMouseOverFill = new SolidFill(factory.GetColor(0x82DFFB));
            expand.ExpandBorder = new SolidBorder(factory.GetColor(0x848484), 1);
            expand.ExpandFill = new SolidFill(factory.GetColor(0xFFFFFF));
            expand.ExpandMouseOverBorder = new SolidBorder(factory.GetColor(0x1CC4F7), 1);
            expand.ExpandMouseOverFill = new SolidFill(factory.GetColor(0xCCEDFA));
            ct.ExpandTriangle = expand;
            // Rectangle
            expand = new TreeExpandColorTable();
            expand.CollapseForeground = new SolidFill(factory.GetColor(0x000000));
            expand.CollapseBorder = new SolidBorder(factory.GetColor(0x969696), 1);
            expand.CollapseFill = new GradientFill(new ColorStop[]{
                new ColorStop(factory.GetColor(0xFFFFFF), 0f), new ColorStop(factory.GetColor(0xFFFFFF), .40f), new ColorStop(factory.GetColor(0xB6B6B6), 1f)}, 45);
            expand.CollapseMouseOverForeground = expand.CollapseForeground;
            expand.CollapseMouseOverBorder = expand.CollapseBorder;
            expand.CollapseMouseOverFill = expand.CollapseFill;
            expand.ExpandForeground = expand.CollapseForeground;
            expand.ExpandBorder = expand.CollapseBorder;
            expand.ExpandFill = expand.CollapseFill;
            expand.ExpandMouseOverForeground = expand.CollapseForeground;
            expand.ExpandMouseOverBorder = expand.CollapseBorder;
            expand.ExpandMouseOverFill = expand.CollapseFill;
            ct.ExpandRectangle = expand;
            ct.ExpandEllipse = expand;
            #endregion

            #region Misc Tree Color
            ct.GridLines = factory.GetColor(0xEDEDED);
            #endregion
        }
        #endregion
    }
}
