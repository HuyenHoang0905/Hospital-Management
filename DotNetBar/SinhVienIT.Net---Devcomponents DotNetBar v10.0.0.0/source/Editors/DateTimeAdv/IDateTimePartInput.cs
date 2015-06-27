#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.DateTimeAdv
{
    /// <summary>
    /// Defines an interface for the DateTime Part interaction.
    /// </summary>
    public interface IDateTimePartInput
    {
        /// <summary>
        /// Gets or sets the date/time part value.
        /// </summary>
        int Value { get;set;}
        /// <summary>
        /// Gets or sets the minimum value for the date/time part entry.
        /// </summary>
        int MinValue { get;set;}

        /// <summary>
        /// Gets or sets the maximum value for the date/time part entry.
        /// </summary>
        int MaxValue { get;set;}

        /// <summary>
        /// Gets the date time part control represents.
        /// </summary>
        eDateTimePart Part { get;}

        /// <summary>
        /// Gets or sets whether input part is empty.
        /// </summary>
        bool IsEmpty { get; set;}

        /// <summary>
        /// Reverts to the last input value control held.
        /// </summary>
        void UndoInput();
    }
}
#endif

