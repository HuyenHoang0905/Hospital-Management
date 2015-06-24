using System;
using System.Text;

namespace DevComponents.AdvTree
{
    /// <summary>
    /// Provides data for the ProvideCustomCellEditor event.
    /// </summary>
    public class CustomCellEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the cell editor. You must set this property in your event handler to the custom
        /// editor to be used for cell editing.
        /// </summary>
        public ICellEditControl EditControl = null;
        /// <summary>
        /// Gets the cell editor will be used for.
        /// </summary>
        public readonly Cell Cell;

        /// <summary>
        /// Initializes a new instance of the CustomCellEditorEventArgs class.
        /// </summary>
        /// <param name="cell"></param>
        public CustomCellEditorEventArgs(Cell cell)
        {
            Cell = cell;
        }
    }
    /// <summary>
    /// Defines delegate for ProvideCustomCellEditor event.
    /// </summary>
    public delegate void CustomCellEditorEventHandler(object sender, CustomCellEditorEventArgs e);
}
