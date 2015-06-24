#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevComponents.Schedule.Model
{
    /// <summary>
    /// Defines an interface for property notification change.
    /// </summary>
    public interface INotifySubPropertyChanged
    {
        /// <summary>
        /// Occurs when property on object or its sub-objects has changed.
        /// </summary>
        event SubPropertyChangedEventHandler SubPropertyChanged;
    }

    public delegate void SubPropertyChangedEventHandler(object sender, SubPropertyChangedEventArgs e);
    /// <summary>
    /// Defines event arguments for SubPropertyChanged event.
    /// </summary>
    public class SubPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Reference to PropertyChangedArgs of changed property.
        /// </summary>
        public PropertyChangedEventArgs PropertyChangedArgs = null;
        /// <summary>
        /// Reference to the source object of the event.
        /// </summary>
        public object Source = null;

        /// <summary>
        /// Initializes a new instance of the SubPropertyChangedEventArgs class.
        /// </summary>
        /// <param name="propertyChangedArgs"></param>
        /// <param name="source"></param>
        public SubPropertyChangedEventArgs(object source, PropertyChangedEventArgs propertyChangedArgs)
        {
            PropertyChangedArgs = propertyChangedArgs;
            Source = source;
        }
    }
}
#endif

