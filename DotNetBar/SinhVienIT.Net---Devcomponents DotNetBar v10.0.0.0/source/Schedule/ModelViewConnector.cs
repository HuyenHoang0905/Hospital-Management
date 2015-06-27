#if FRAMEWORK20
using DevComponents.Schedule.Model;

namespace DevComponents.DotNetBar.Schedule
{
    /// <summary>
    /// Represents base class for the model to view connectors.
    /// </summary>
    internal abstract class ModelViewConnector
    {
        #region Internal Implementation

        #region Private variables

        private string _DisplayOwnerKey = "";

        #endregion

        #region Abstract defines

        /// <summary>
        /// Connects View to a model.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// Disconnects view from model.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Gets whether connector has connected model to a view.
        /// </summary>
        public abstract bool IsConnected { get; }

        public abstract eCalendarView GetView();

        #endregion

        #region Virtual methods

        protected virtual bool IsAppointmentVisible(Appointment app)
        {
            if (app.Visible == false)
                return (false);

            if (string.IsNullOrEmpty(_DisplayOwnerKey)) 
                return (true);

            return (app.OwnerKey == _DisplayOwnerKey);
        }

        protected virtual bool IsCustomItemVisible(CustomCalendarItem item)
        {
            if (item.Visible == false)
                return (false);

            if (string.IsNullOrEmpty(_DisplayOwnerKey))
                return (true);

            return (item.OwnerKey.Equals(_DisplayOwnerKey));
        }

        /// <summary>
        /// Gets or sets the owner key of the owner of the appointments displayed on the view.
        /// </summary>
        public virtual string DisplayOwnerKey
        {
            get { return _DisplayOwnerKey; }

            set
            {
                if (value == null)
                    value = "";

                if (_DisplayOwnerKey != null)
                {
                    _DisplayOwnerKey = value;
                }
            }
        }

        #endregion

        #endregion
    }
}
#endif

