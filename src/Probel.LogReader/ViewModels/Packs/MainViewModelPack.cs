namespace Probel.LogReader.ViewModels.Packs
{
    public class MainViewModelPack
    {
        #region Constructors

        public MainViewModelPack(
            LogsViewModel logsViewModel,
            ManageRepositoryViewModel manageRepositoryViewModel,
            ManageFilterViewModel manageFilterViewModel,
            ManageFilterBindingsViewModel manageFilterBindingsViewModel)
        {
            ManageFilterViewModel = manageFilterViewModel;
            LogsViewModel = logsViewModel;
            ManageRepositoryViewModel = manageRepositoryViewModel;
            ManageFilterBindingsViewModel = manageFilterBindingsViewModel;
        }

        #endregion Constructors

        #region Properties

        public LogsViewModel LogsViewModel { get; }
        public ManageFilterViewModel ManageFilterViewModel { get; set; }
        public ManageRepositoryViewModel ManageRepositoryViewModel { get; }
        public ManageFilterBindingsViewModel ManageFilterBindingsViewModel { get; }

        #endregion Properties
    }
}