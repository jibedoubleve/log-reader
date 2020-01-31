namespace Probel.LogReader.Ui
{
    public class UiEvent
    {
        #region Properties

        public static UiEvent RefreshMenus => new UiEvent() { Event = UiEvents.RefreshMenus };
        public object Context { get; set; }

        public UiEvents Event { get; set; }

        #endregion Properties

        #region Methods

        public static UiEvent ShowMenuFilter(bool isVisible) => new UiEvent() { Event = UiEvents.FilterVisibility, Context = isVisible };

        #endregion Methods
    }
}