#if FRAMEWORK20
using System;
using System.Collections.Generic;
using DevComponents.Schedule.Model;
using System.ComponentModel;

namespace DevComponents.DotNetBar.Schedule
{
    internal class ModelWeekDayViewConnector : ModelViewConnector
    {
        #region Private variables

        private CalendarModel _Model;       // The associated CalendarModel
        private WeekDayView _View;          // The associated WeekDayView

        private bool _IsConnected;          // Connection status

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Assoc CalendarModel</param>
        /// <param name="weekDayView">Assoc WeekDayView</param>
        public ModelWeekDayViewConnector(CalendarModel model, WeekDayView weekDayView)
        {
            _Model = model;
            _View = weekDayView;
        }

        #region Public properties

        /// <summary>
        /// Gets the connection status
        /// </summary>
        public override bool IsConnected
        {
            get { return (_IsConnected); }
        }

        #endregion

        #region Connect processing

        /// <summary>
        /// Performs Model connection processing
        /// </summary>
        public override void Connect()
        {
            VerifyModel();

            if (_IsConnected)
                Disconnect();

            LoadData();

            // Get notification on Model property changes

            _Model.PropertyChanged += ModelPropertyChanged;
            _Model.SubPropertyChanged += ModelSubPropertyChanged;

            _View.CalendarView.CustomItems.CollectionChanged +=
                CustomItemsCollectionChanged;

            _IsConnected = true;
        }

        #endregion

        #region Disconnect processing

        /// <summary>
        /// Severs the Model/WeekDayView connection
        /// </summary>
        public override void Disconnect()
        {
            VerifyModel();

            if (_IsConnected)
            {
                // Clear all AllDayPanel items

                ClearAllDayPanelItems();

                // Loop through each DayColumn, clearing each
                // associated view connection

                for (int i = 0; i < _View.DayColumns.Length; i++)
                    ClearWeekDayColumn(_View.DayColumns[i]);

                _View.SubItems.Clear();

                // Stop notification on Model property changes

                _Model.PropertyChanged -= ModelPropertyChanged;
                _Model.SubPropertyChanged -= ModelSubPropertyChanged;

                _View.CalendarView.CustomItems.CollectionChanged -=
                    CustomItemsCollectionChanged;

                _IsConnected = false;

                OnSubItemsChanged();
            }
        }

        private void ClearAllDayPanelItems()
        {
            List<CalendarItem> items = _View.AllDayPanel.CalendarItems;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                AppointmentWeekDayView view = items[i] as AppointmentWeekDayView;

                if (view != null)
                {
                    view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                    view.Appointment = null;
                    view.IsSelected = false;
                    view.AllDayPanel = null;
                }

                items.RemoveAt(i);
            }

            _View.AllDayPanel.SubItems.Clear();
        }

        /// <summary>
        /// Clears individual DayColumn view connections
        /// </summary>
        /// <param name="dayColumn">DayColumn</param>
        private void ClearWeekDayColumn(DayColumn dayColumn)
        {
            if (dayColumn.CalendarItems.Count > 0)
            {
                // Loop through each CalendarItem, resetting
                // it's associated connection

                for (int i = dayColumn.CalendarItems.Count - 1; i >= 0; i--)
                {
                    AppointmentWeekDayView view =
                        dayColumn.CalendarItems[i] as AppointmentWeekDayView;

                    if (view != null)
                    {
                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;

                        view.Appointment = null;
                        view.IsSelected = false;
                        view.DayColumn = null;
                    }

                    dayColumn.CalendarItems.RemoveAt(i);
                }
            }
        }

        #endregion

        #region LoadData processing

        /// <summary>
        /// Loads Model/WeekDayView connection data
        /// </summary>
        private void LoadData()
        {
            List<List<Appointment>> dayAppts =
                new List<List<Appointment>>(_View.DayColumns.Length);

            // Loop through each column

            foreach (DayColumn dayColumn in _View.DayColumns)
            {
                // Accumulate Model appointments for the column

                List<Appointment> appts =
                    new List<Appointment>(_Model.GetDay(dayColumn.Date).Appointments);

                // Remove multi-day duplicates 

                appts = RemoveDuplicates(dayAppts, appts);
                dayAppts.Add(appts);

                // Update the column appointments and Custom CalendarItems

                UpdateColumnAppts(dayColumn, appts, null);
                UpdateCustomItems(dayColumn, null);
            }

            UpdateAllDayPanelView(dayAppts);
            UpdateAllDayPanelCustomItems();

            OnSubItemsChanged();
        }
        
        #endregion

        #region RefreshData processing

        /// <summary>
        /// Refreshes the data in a previously established
        /// and loaded connection
        /// </summary>
        public void RefreshData()
        {
            List<List<Appointment>> dayAppts = new List<List<Appointment>>(_View.DayColumns.Length);

            List<AppointmentWeekDayView> removedViews = null;
            List<CustomCalendarItem> removedItems = null;

            foreach (DayColumn dayColumn in _View.DayColumns)
            {
                Day day = _Model.GetDay(dayColumn.Date);

                List<Appointment> appts = new List<Appointment>(day.Appointments);

                // Remove duplicate multi-day appointments and add
                // then to our column appointments list

                appts = RemoveDuplicates(dayAppts, appts);
                dayAppts.Add(appts);

                // Process and record any out-dated DayColumn views

                RemoveOutdatedViews(ref removedViews, dayColumn, appts);
                RemoveOutdatedCustomItems(ref removedItems, dayColumn);
            }

            // Process and record any out-dated AllDayAppt views

            RemoveOutdatedAllDayViews(ref removedViews, dayAppts);
            RemoveOutdatedAllDayCustomItems(ref removedItems);

            // Process each DayColumn using our accumulated info

            for (int i = 0; i < _View.DayColumns.Length; i++)
            {
                DayColumn dayColumn = _View.DayColumns[i];

                UpdateColumnAppts(dayColumn, dayAppts[i], removedViews);
                UpdateCustomItems(dayColumn, removedItems);
            }

            // Update the AllDayAppt views

            UpdateAllDayPanelView(dayAppts);
            UpdateAllDayPanelCustomItems();

            // Process remaining removed data

            ProcessRemovedData(removedViews, removedItems);

            OnSubItemsChanged();
        }

        #region ProcessRemovedData

        /// <summary>
        /// Process any remaining removed data
        /// </summary>
        /// <param name="removedViews"></param>
        /// <param name="removedItems"></param>
        private void ProcessRemovedData(List<AppointmentWeekDayView> removedViews,
            List<CustomCalendarItem> removedItems)
        {
            // Process any remaining removedViews

            if (removedViews != null && removedViews.Count > 0)
            {
                for (int i = 0; i < removedViews.Count; i++)
                {
                    if (removedViews[i] == _View.SelectedItem)
                        _View.SelectedItem = null;

                    removedViews[i].Dispose();
                }
            }

            // Process any remaining removed items

            if (removedItems != null && removedItems.Count > 0)
            {
                for (int i = 0; i < removedItems.Count; i++)
                {
                    if (removedItems[i] == _View.SelectedItem)
                        _View.SelectedItem = null;

                    removedItems[i].Dispose();
                }

                _View.NeedRecalcLayout = true;
            }
        }

        #endregion

        #endregion

        #region UpdateColumnAppts

        /// <summary>
        /// Updates individual DayColumn views
        /// </summary>
        /// <param name="dayColumn">DayColumn</param>
        /// <param name="appointments">List of appointments</param>
        /// <param name="cachedViews">List of cached views</param>
        private void UpdateColumnAppts(DayColumn dayColumn,
            List<Appointment> appointments, List<AppointmentWeekDayView> cachedViews)
        {
            // Loop through each appointment
            // updating the assoc view accordingly

            foreach (Appointment appointment in appointments)
            {
                if (IsAppointmentVisible(appointment))
                {
                    // Get the assoc view

                    if (appointment.IsMultiDayOrAllDayEvent == false)
                    {
                        AppointmentWeekDayView view = (GetViewFromColumn(dayColumn, appointment) ??
                                                       GetViewFromCache(appointment, cachedViews)) ??
                                                       GetNewView(appointment);

                        if (view.StartTime != appointment.StartTime || view.EndTime != appointment.EndTime)
                            dayColumn.NeedRecalcLayout = true;

                        // Set the view start and end times to
                        // match the assoc appointment

                        view.StartTime = appointment.StartTime;
                        view.EndTime = appointment.EndTime;

                        // Update the DayColumn data

                        if (view.DayColumn == null)
                        {
                            view.DayColumn = dayColumn;

                            dayColumn.CalendarItems.Add(view);
                            dayColumn.NeedRecalcLayout = true;

                            _View.SubItems.Add(view);
                            _View.UpdateItemOrder(view);

                            view.IsSelectedChanged += _View.ItemIsSelectedChanged;
                        }
                    }
                }
            }

            // Update workDay details

            UpdateWorkDayDetails(dayColumn);
        }

        #endregion

        #region UpdateCustomItems

        private void UpdateCustomItems(
            DayColumn dayColumn, List<CustomCalendarItem> cachedItems)
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true && item.IsMultiDayOrAllDayEvent == false &&
                        (item.StartTime < dayColumn.Date.AddDays(1) && item.EndTime > dayColumn.Date))
                    {
                        item.CalendarView = _View.CalendarView;

                        CustomCalendarItem ci = GetItemFromColumn(dayColumn, item);

                        if (ci == null)
                        {
                            ci = GetItemFromCache(item, cachedItems) ?? GetNewCustomItem(item);

                            dayColumn.CalendarItems.Add(ci);
                            dayColumn.NeedRecalcLayout = true;

                            _View.SubItems.Add(ci);
                            _View.UpdateItemOrder(ci);

                            ci.IsSelectedChanged += _View.ItemIsSelectedChanged;
                        }

                        ci.StartTime = item.StartTime;
                        ci.EndTime = item.EndTime;

                        dayColumn.NeedRecalcLayout = true;
                    }
                }
            }
        }

        #endregion

        #region GetItemFromColumn

        private CustomCalendarItem GetItemFromColumn(DayColumn dayColumn, CustomCalendarItem item)
        {
            foreach (CalendarItem citem in dayColumn.CalendarItems)
            {
                CustomCalendarItem view = citem as CustomCalendarItem;

                if (view != null && (view == item || view.BaseCalendarItem == item))
                    return (view);
            }

            return (null);
        }

        #endregion

        #region GetItemFromCache

        private CustomCalendarItem GetItemFromCache(
            CustomCalendarItem item, List<CustomCalendarItem> cachedItems)
        {
            if (cachedItems != null && cachedItems.Count > 0)
            {
                foreach (CustomCalendarItem view in cachedItems)
                {
                    if (view == item || view.BaseCalendarItem == item)
                    {
                        cachedItems.Remove(view);

                        return (view);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region UpdateAllDayPanelView

        private void UpdateAllDayPanelView(List<List<Appointment>> dayAppts)
        {
            // Loop through each appointment
            // updating the assoc view accordingly

            for (int i = 0; i < dayAppts.Count; i++)
            {
                List<Appointment> appointments = dayAppts[i];

                foreach (Appointment appointment in appointments)
                {
                    if (IsAppointmentVisible(appointment))
                    {
                        // Get the assoc view

                        if (appointment.IsMultiDayOrAllDayEvent)
                        {
                            AppointmentWeekDayView view =
                                GetViewFromAllDayPanel(appointment) ??
                                GetNewView(appointment);

                            // Set the view start and end times to
                            // match the assoc appointment

                            view.StartTime = appointment.StartTime;
                            view.EndTime = appointment.EndTime;

                            if (view.AllDayPanel == null)
                            {
                                view.AllDayPanel = _View.AllDayPanel;

                                _View.AllDayPanel.CalendarItems.Add(view);
                                _View.AllDayPanel.SubItems.Add(view);

                                _View.NeedRecalcLayout = true;

                                view.IsSelectedChanged += _View.ItemIsSelectedChanged;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region UpdateAllDayPanelCustomItems

        private void UpdateAllDayPanelCustomItems()
        {
            CustomCalendarItemCollection items = _View.CalendarView.CustomItems;

            if (items != null)
            {
                DateTime startDate = _View.StartDate;
                DateTime endDate = _View.EndDate.AddDays(1);

                for (int i = 0; i < items.Count; i++)
                {
                    CustomCalendarItem item = items[i];

                    if (IsCustomItemVisible(item) == true && item.IsMultiDayOrAllDayEvent == true &&
                        (item.StartTime < endDate && item.EndTime > startDate))
                    {
                        item.CalendarView = _View.CalendarView;

                        CustomCalendarItem ci = GetItemFromAllDayPanel(item);

                        if (ci == null)
                        {
                            ci = GetNewCustomItem(item);

                            _View.AllDayPanel.CalendarItems.Add(ci);
                            _View.NeedRecalcLayout = true;

                            _View.AllDayPanel.SubItems.Add(ci);

                            ci.IsSelectedChanged += _View.ItemIsSelectedChanged;
                        }

                        if (ci.StartTime != item.StartTime || ci.EndTime != item.EndTime)
                        {
                            ci.StartTime = item.StartTime;
                            ci.EndTime = item.EndTime;

                            _View.NeedRecalcLayout = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region UpdateWorkDayDetails

        /// <summary>
        /// Updates DayColumn workday details
        /// </summary>
        /// <param name="dayColumn">DayColumn to update</param>
        private void UpdateWorkDayDetails(DayColumn dayColumn)
        {
            // Update workDay timings

            Owner owner = _Model.Owners[_View.OwnerKey];

            if (owner == null || GetCalendarWorkDays(dayColumn, owner.CalendarWorkDays) == false)
            {
                if (GetCalendarWorkDays(dayColumn, _Model.CalendarWorkDays) == false)
                {
                    if (owner == null || GetWorkDays(dayColumn, owner.WorkDays) == false)
                    {
                        if (GetWorkDays(dayColumn, _Model.WorkDays) == false)
                        {
                            dayColumn.WorkStartTime = new WorkTime();
                            dayColumn.WorkEndTime = new WorkTime();
                        }
                    }
                }
            }
        }

        #region GetCalendarWorkDays

        /// <summary>
        /// GetCalendarWorkDays
        /// </summary>
        /// <param name="dayColumn"></param>
        /// <param name="calendarWorkDays"></param>
        /// <returns></returns>
        private bool GetCalendarWorkDays(
            DayColumn dayColumn, CalendarWorkDayCollection calendarWorkDays)
        {
            if (calendarWorkDays != null && calendarWorkDays.Count > 0)
            {
                CalendarWorkDay cwd = calendarWorkDays[dayColumn.Date];

                if (cwd != null)
                {
                    dayColumn.WorkStartTime = cwd.WorkStartTime;
                    dayColumn.WorkEndTime = cwd.WorkEndTime;

                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region GetWorkDays

        /// <summary>
        /// GetWorkDays
        /// </summary>
        /// <param name="dayColumn"></param>
        /// <param name="workDays"></param>
        /// <returns></returns>
        private bool GetWorkDays(
            DayColumn dayColumn, WorkDayCollection workDays)
        {
            if (workDays != null && workDays.Count > 0)
            {
                WorkDay wd = workDays[dayColumn.Date.DayOfWeek];

                if (wd != null)
                {
                    dayColumn.WorkStartTime = wd.WorkStartTime;
                    dayColumn.WorkEndTime = wd.WorkEndTime;
                }
                else
                {
                    dayColumn.WorkStartTime = new WorkTime();
                    dayColumn.WorkEndTime = new WorkTime();
                }

                return (true);
            }

            return (false);
        }

        #endregion

        #endregion

        #region GetView routines

        /// <summary>
        /// Gets the view from all lists
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private AppointmentWeekDayView GetViewFromAll(Appointment app)
        {
            for (int i = 0; i < _View.NumberOfColumns; i++)
            {
                AppointmentWeekDayView view =
                    GetViewFromColumn(_View.DayColumns[i], app);

                if (view != null)
                    return (view);
            }

            return (GetViewFromAllDayPanel(app));
        }

        /// <summary>
        /// Gets the view from the DayColumn list
        /// </summary>
        /// <param name="dayColumn">DayColumn</param>
        /// <param name="appointment">Appointment</param>
        /// <returns>Appointment view</returns>
        private AppointmentWeekDayView GetViewFromColumn(DayColumn dayColumn, Appointment appointment)
        {
            foreach (CalendarItem item in dayColumn.CalendarItems)
            {
                AppointmentWeekDayView view = item as AppointmentWeekDayView;

                if (view != null && view.Appointment == appointment)
                    return (view);
            }

            return (null);
        }

        /// <summary>
        /// Gets the view from the AllDayPanel list
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>Appointment view</returns>
        private AppointmentWeekDayView GetViewFromAllDayPanel(Appointment appointment)
        {
            foreach (CalendarItem item in _View.AllDayPanel.CalendarItems)
            {
                AppointmentWeekDayView view = item as AppointmentWeekDayView;

                if (view != null && view.Appointment == appointment)
                    return (view);
            }

            return (null);
        }

        /// <summary>
        /// Gets the view from the cached list
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <param name="cachedViews">Cached views</param>
        /// <returns>Appointment view</returns>
        private AppointmentWeekDayView GetViewFromCache(Appointment appointment,
            List<AppointmentWeekDayView> cachedViews)
        {
            if (cachedViews != null && cachedViews.Count > 0)
            {
                foreach (AppointmentWeekDayView appView in cachedViews)
                {
                    if (appView.Appointment == appointment)
                    {
                        cachedViews.Remove(appView);

                        return (appView);
                    }
                }
            }

            return (null);
        }

        #endregion

        #region GetItemFromAllDayPanel

        /// <summary>
        /// Gets the CustomCalendarItem from the AllDayPanel list
        /// </summary>
        /// <param name="item">CustomCalendarItem</param>
        /// <returns>CustomCalendarItem</returns>
        private CustomCalendarItem GetItemFromAllDayPanel(CustomCalendarItem item)
        {
            foreach (CalendarItem citem in _View.AllDayPanel.CalendarItems)
            {
                CustomCalendarItem view = citem as CustomCalendarItem;

                if (view != null && (view == item || view.BaseCalendarItem == item))
                    return (view);
            }

            return (null);
        }


        #endregion

        #region GetNewView

        /// <summary>
        /// Gets a new appointment view
        /// </summary>
        /// <param name="appointment">Appointment</param>
        /// <returns>New view</returns>
        private AppointmentWeekDayView GetNewView(Appointment appointment)
        {
            AppointmentWeekDayView view = new AppointmentWeekDayView(_View, appointment);

            view.Tooltip = appointment.Tooltip;

            return (view);
        }

        #endregion

        #region GetNewCustomItem

        private CustomCalendarItem GetNewCustomItem(CustomCalendarItem item)
        {
            CustomCalendarItem ci = (CustomCalendarItem)item.Copy();
            ci.BaseCalendarItem = item;

            return (ci);
        }

        #endregion

        #region RemoveDuplicates

        /// <summary>
        /// Removes duplicate multi-day appointments
        /// </summary>
        /// <param name="dayAppts"></param>
        /// <param name="appointments"></param>
        /// <returns>Trimmed list</returns>
        private List<Appointment> RemoveDuplicates(
            List<List<Appointment>> dayAppts, List<Appointment> appointments)
        {
            List<Appointment> a = new List<Appointment>();

            foreach (Appointment app in appointments)
            {
                if (app.Visible == true)
                {
                    if (IsDuplicateAppointment(dayAppts, app) == false)
                        a.Add(app);
                }
            }

            return (a);
        }

        /// <summary>
        /// Determines if an appointment is a duplicate
        /// </summary>
        /// <param name="dayAppts">List of days appointments</param>
        /// <param name="appointment">Appointment in question</param>
        /// <returns></returns>
        private bool IsDuplicateAppointment(
            List<List<Appointment>> dayAppts, Appointment appointment)
        {
            foreach (List<Appointment> dayAppt in dayAppts)
            {
                if (dayAppt.Contains(appointment) == true)
                    return (true);
            }

            return (false);
        }

        #endregion

        #region RemoveOutdatedViews

        /// <summary>
        /// Removes out-dated views
        /// </summary>
        /// <param name="removedViews"></param>
        /// <param name="dayColumn"></param>
        /// <param name="appointments"></param>
        private void RemoveOutdatedViews(ref List<AppointmentWeekDayView> removedViews,
            DayColumn dayColumn, List<Appointment> appointments)
        {
            List<AppointmentWeekDayView> removeList = null;

            for (int i = dayColumn.CalendarItems.Count - 1; i >= 0; i--)
            {
                AppointmentWeekDayView view =
                    dayColumn.CalendarItems[i] as AppointmentWeekDayView;

                if (view != null)
                {
                    if (!appointments.Contains(view.Appointment) || !IsAppointmentVisible(view.Appointment))
                    {
                        if (removeList == null)
                            removeList = new List<AppointmentWeekDayView>();

                        removeList.Add(view);

                        view.DayColumn.NeedRecalcLayout = true;
                        view.DayColumn = null;

                        _View.NeedRecalcSize = true;
                        _View.SubItems._Remove(view);

                        dayColumn.CalendarItems.RemoveAt(i);

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }

            if (removeList != null)
            {
                if (removedViews == null)
                    removedViews = new List<AppointmentWeekDayView>();

                removedViews.AddRange(removeList);
            }
        }

        #endregion

        #region RemoveOutdatedCustomItems

        private void RemoveOutdatedCustomItems(
            ref List<CustomCalendarItem> removedItems, DayColumn dayColumn)
        {
            List<CustomCalendarItem> removeList = null;

            for (int i = dayColumn.CalendarItems.Count - 1; i >= 0; i--)
            {
                CustomCalendarItem view =
                    dayColumn.CalendarItems[i] as CustomCalendarItem;

                if (view != null)
                {
                    if (IsValidItem(view) == false ||
                        (view.EndTime < dayColumn.Date || view.StartTime > dayColumn.Date.AddDays(1)))
                    {
                        if (removeList == null)
                            removeList = new List<CustomCalendarItem>();

                        removeList.Add(view);

                        _View.SubItems._Remove(view);
                        _View.NeedRecalcSize = true;

                        dayColumn.NeedRecalcLayout = true;
                        dayColumn.CalendarItems.RemoveAt(i);

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }

            if (removeList != null)
            {
                if (removedItems == null)
                    removedItems = new List<CustomCalendarItem>();

                removedItems.AddRange(removeList);
            }
        }

        private bool IsValidItem(CustomCalendarItem view)
        {
            return (IsCustomItemVisible(view) == true &&
                _View.CalendarView.CustomItems.Contains(view.BaseCalendarItem) == true);
        }

        #endregion

        #region RemoveOutdatedAllDayViews

        /// <summary>
        /// Removes any outdated AllDayAppt views
        /// </summary>
        /// <param name="removedViews"></param>
        /// <param name="dayAppts">Accumulated DayAppts</param>
        private void RemoveOutdatedAllDayViews(
            ref List<AppointmentWeekDayView> removedViews, List<List<Appointment>> dayAppts)
        {
            List<AppointmentWeekDayView> removeList = null;

            for (int i = _View.AllDayPanel.CalendarItems.Count - 1; i >= 0; i--)
            {
                AppointmentWeekDayView view =
                    _View.AllDayPanel.CalendarItems[i] as AppointmentWeekDayView;

                if (view != null)
                {
                    if (!IsAppointmentVisible(view.Appointment) ||
                        !AllDayAppFound(dayAppts, view.Appointment) ||
                        view.Appointment.IsMultiDayOrAllDayEvent == false)
                    {
                        if (removeList == null)
                            removeList = new List<AppointmentWeekDayView>();

                        removeList.Add(view);

                        _View.AllDayPanel.CalendarItems.RemoveAt(i);
                        _View.AllDayPanel.SubItems.Remove(view);

                        _View.NeedRecalcLayout = true;

                        view.AllDayPanel = null;

                        view.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }

            if (removeList != null)
            {
                if (removedViews == null)
                    removedViews = new List<AppointmentWeekDayView>();

                removedViews.AddRange(removeList);
            }
        }

        #region AllDayAppFound

        /// <summary>
        /// Looks for the given appointment in the
        /// accumulated dayAppts list
        /// </summary>
        /// <param name="dayAppts">Accumulated appts list</param>
        /// <param name="app">Appointment to look for</param>
        /// <returns>true if found</returns>
        private bool AllDayAppFound(List<List<Appointment>> dayAppts, Appointment app)
        {
            for (int i = 0; i < dayAppts.Count; i++)
            {
                List<Appointment> appts = dayAppts[i];

                for (int j = 0; j < appts.Count; j++)
                {
                    if (appts.Contains(app))
                        return (true);
                }
            }

            return (false);
        }

        #endregion

        #endregion

        #region RemoveOutdatedAllDayCustomItems

        private void RemoveOutdatedAllDayCustomItems(
            ref List<CustomCalendarItem> removedItems)
        {
            List<CustomCalendarItem> removeList = null;

            DateTime startDate = _View.StartDate;
            DateTime endDate = _View.EndDate;

            for (int i = _View.AllDayPanel.CalendarItems.Count - 1; i >= 0; i--)
            {
                CustomCalendarItem item =
                    _View.AllDayPanel.CalendarItems[i] as CustomCalendarItem;

                if (item != null)
                {
                    if (item.IsMultiDayOrAllDayEvent == false ||
                        IsValidItem(item) == false ||
                        (item.StartTime > endDate || item.EndTime < startDate))
                    {
                        if (removeList == null)
                            removeList = new List<CustomCalendarItem>();

                        removeList.Add(item);

                        _View.AllDayPanel.SubItems._Remove(item);
                        _View.AllDayPanel.NeedRecalcSize = true;
                        _View.NeedRecalcLayout = true;

                        _View.AllDayPanel.CalendarItems.RemoveAt(i);

                        item.IsSelectedChanged -= _View.ItemIsSelectedChanged;
                    }
                }
            }

            if (removeList != null)
            {
                if (removedItems == null)
                    removedItems = new List<CustomCalendarItem>();

                removedItems.AddRange(removeList);
            }
        }

        #endregion

        #region View support routines

        /// <summary>
        /// Returns the view
        /// </summary>
        /// <returns></returns>
        public override eCalendarView GetView()
        {
            return (eCalendarView.Week);
        }

        /// <summary>
        /// Verifies the Model and MonthView are valid
        /// </summary>
        private void VerifyModel()
        {
            if (_Model == null)
                throw new NullReferenceException("CalendarModel must be set on connector.");

            if (_View == null)
                throw new NullReferenceException("WeekDayCalendarView must be set on connector.");
        }

        #endregion

        #region Model property change processing

        /// <summary>
        /// Handles Model property change notifications
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CalendarModel.AppointmentsPropertyName)
            {
                RefreshData();
                UpdateDisplay();
            }
            else if (e.PropertyName == CalendarModel.WorkDaysPropertyName)
            {
                for (int i = 0; i < _View.NumberOfColumns; i++)
                    UpdateWorkDayDetails(_View.DayColumns[i]);

                _View.CalendarView.CalendarPanel.RecalcSize();
            }
        }

        #endregion

        #region ModelSubPropertyChanged

        /// <summary>
        /// Handles ModelSubProperty change notifications
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SubPropertyChangedEventArgs</param>
        private void ModelSubPropertyChanged(object sender, SubPropertyChangedEventArgs e)
        {
            if (e.Source is WorkDay)
            {
                for (int i = 0; i < _View.NumberOfColumns; i++)
                    UpdateWorkDayDetails(_View.DayColumns[i]);

                _View.CalendarView.CalendarPanel.RecalcSize();
            }
            else if (e.Source is Owner && e.PropertyChangedArgs.PropertyName == Owner.DisplayNamePropertyName)
            {
                Owner owner = (Owner)e.Source;

                if (_View.OwnerKey != null && _View.OwnerKey.Equals(owner.Key))
                    _View.DisplayName = owner.DisplayName;
            }
            else if (e.Source is Appointment)
            {
                Appointment app = e.Source as Appointment;
                AppointmentWeekDayView appView;

                string name = e.PropertyChangedArgs.PropertyName;

                if (name.Equals("Tooltip"))
                {
                    appView = GetViewFromAll(app);

                    if (appView != null)
                        appView.Tooltip = app.Tooltip;
                }
                else if (name.Equals("IsSelected"))
                {
                    appView = GetViewFromAll(app);

                    if (appView != null)
                        appView.IsSelected = app.IsSelected;
                }
                else if (name.Equals("CategoryColor") || name.Equals("TimeMarkedAs"))
                {
                    appView = GetViewFromAll(app);

                    if (appView != null)
                        appView.Refresh();
                }
                else if (name.Equals("OwnerKey"))
                {
                    if (_View.CalendarView.IsMultiCalendar == true)
                    {
                        if (_View.OwnerKey == app.OwnerKey)
                        {
                            RefreshData();
                            UpdateDisplay();
                        }
                        else
                        {
                            appView = GetViewFromAll(app);

                            if (appView != null)
                            {
                                RefreshData();
                                UpdateDisplay();
                            }
                        }
                    }
                }
                else if (name.Equals("Visible"))
                {
                    RefreshData();
                    UpdateDisplay();
                }
            }
        }

        #endregion

        #region CustomItems_CollectionChanged

        void CustomItemsCollectionChanged(object sender, EventArgs e)
        {
            RefreshData();
            UpdateDisplay();
        }

        #endregion

        #region UpdateDisplay

        private void UpdateDisplay()
        {
            if (_View.Displayed == true)
            {
                if (_View.NeedRecalcLayout == false)
                {
                    for (int i = 0; i < _View.NumberOfColumns; i++)
                    {
                        if (_View.DayColumns[i].NeedRecalcLayout)
                        {
                            _View.RecalcSize();
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region OnSubItemsChanged

        private void OnSubItemsChanged()
        {
            System.Windows.Forms.Cursor.Position =
                System.Windows.Forms.Cursor.Position;
        }

        #endregion

    }
}
#endif

