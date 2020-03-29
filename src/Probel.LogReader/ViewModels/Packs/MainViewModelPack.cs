namespace Probel.LogReader.ViewModels.Packs
{
    public class MainViewModelPack
    {
        #region Constructors

        public MainViewModelPack(LogsViewModel logsViewModel, ManageRepositoryViewModel manageRepositoryViewModel, ManageFilterViewModel manageFilterViewModel)
        {
            ManageFilterViewModel = manageFilterViewModel;
            LogsViewModel = logsViewModel;
            ManageRepositoryViewModel = manageRepositoryViewModel;
        }

        #endregion Constructors

        #region Properties

        public LogsViewModel LogsViewModel { get; }
        public ManageFilterViewModel ManageFilterViewModel { get; set; }
        public ManageRepositoryViewModel ManageRepositoryViewModel { get; }

        #endregion Properties
    }
}