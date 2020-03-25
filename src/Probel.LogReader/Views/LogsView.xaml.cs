using Probel.LogReader.Helpers;
using Probel.LogReader.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : UserControl
    {
        #region Fields

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        #endregion Fields

        #region Constructors

        public LogsView()
        {
            InitializeComponent();
            _timer.Tick += OnTimerTicked;
        }

        #endregion Constructors

        #region Properties

        private LogsViewModel ViewModel => DataContext as LogsViewModel;

        #endregion Properties

        #region Methods

        private void OnAutoRefreshTimesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _timer.Stop();
            if (_autoRefreshTimes.SelectedIndex > 0 && _autoRefreshTimes.SelectedItem is ComboBoxItem cbi)
            {
                var seconds = int.Parse(cbi?.Tag as string ?? "0");
                _timer.Interval = TimeSpan.FromSeconds(seconds);
                _timer.Start();
            }
        }

        private void OnCollapseAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(false);

        private void OnDetailPanePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_detailPane.IsAutoHidden) && ViewModel != null)
            {
                ViewModel.IsDetailVisible = _detailPane.IsAutoHidden;
            }
        }

        private void OnDockingManagerLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.IsDetailVisible ?? false)
            {
                _detailPane.ToggleAutoHide();
            }
        }

        private void OnExpandAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(true);

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

        private void OnTimerTicked(object sender, EventArgs e) => ViewModel?.RefreshLogs(false);

        private void OnToggleButtonClick(object sender, RoutedEventArgs e)
        {
            _detailPane.ToggleAutoHide();
            if (ViewModel != null)
            {
                ViewModel.IsDetailVisible = !ViewModel.IsDetailVisible;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is IHierarchy<DateTime> day)
            {
                ViewModel.LoadLogs(day.Value);
            }
        }

        #endregion Methods
    }
}