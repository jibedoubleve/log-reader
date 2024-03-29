﻿namespace Probel.LogReader.Ui
{
    public class UiEvent
    {
        #region Properties

        public static UiEvent RefreshMenus => new UiEvent() { Event = UiEvents.RefreshMenus };
        public object Context { get; set; }

        public UiEvents Event { get; set; }

        #endregion Properties

        #region Methods

        public static UiEvent ShowMenuFilter() => new UiEvent() { Event = UiEvents.FilterVisibility, Context = true };
        public static UiEvent HideMenuFilter() => new UiEvent() { Event = UiEvents.FilterVisibility, Context = false };
        public static UiEvent FilterApplied(string filterName) => new UiEvent() { Event = UiEvents.FilterApplied, Context = filterName };

        #endregion Methods
    }
}