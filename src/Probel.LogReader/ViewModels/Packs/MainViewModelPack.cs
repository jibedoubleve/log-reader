namespace Probel.LogReader.ViewModels.Packs
{
    public class MainViewModelPack
    {
        #region Constructors

        public MainViewModelPack(DaysViewModel daysViewModel, LogsViewModel logsViewModel, ManageRepositoryViewModel manageRepositoryViewModel, ManageFilterViewModel manageFilterViewModel)
        {
            ManageFilterViewModel = manageFilterViewModel;
            DaysViewModel = daysViewModel;
            LogsViewModel = logsViewModel;
            ManageRepositoryViewModel = manageRepositoryViewModel;
        }

        #endregion Constructors

        #region Properties

        public DaysViewModel DaysViewModel { get; set; }
        public LogsViewModel LogsViewModel { get; }
        public ManageFilterViewModel ManageFilterViewModel { get; set; }
        public ManageRepositoryViewModel ManageRepositoryViewModel { get; }

        #endregion Properties
    }
}