using Probel.LogReader.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        #region Constructors

        public LogsView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private LogsViewModel ViewModel => DataContext as LogsViewModel;

        #endregion Properties

        #region Methods

        private void OnDockingManagerLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.IsDetailVisible ?? false)
            {
                _detailPane.ToggleAutoHide();
            }
        }

        private void OnLogsMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void OnRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try { Process.Start(e.Uri.AbsoluteUri); }
            catch (Exception)
            {
                /* Ok, it fails, no problem.
                 * TODO: add logs here
                 */
            }
        }

        #endregion Methods

        private void OnToggleButtonClick(object sender, RoutedEventArgs e)
        {
            _detailPane.ToggleAutoHide();
            if (ViewModel != null)
            {
                ViewModel.IsDetailVisible = !ViewModel.IsDetailVisible;
            }
        }
    }
}