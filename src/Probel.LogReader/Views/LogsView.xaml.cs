using AvalonDock.Layout;
using Probel.LogReader.Helpers;
using Probel.LogReader.Ui;
using Probel.LogReader.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
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

        private void LoadLayout()
        {
            //var currentContentsList = _dockingManager.Layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();
            new LayoutPersister(ViewModel.DockingStateFile).Load(_dockingManager);
        }

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

        private void OnClickResetFilter(object sender, RoutedEventArgs e) => _tbFilter.Text = string.Empty;

        private void OnCollapseAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(false);

        private void OnDeleteLayout(object sender, RoutedEventArgs e) => new LayoutPersister(ViewModel.DockingStateFile).Delete();

        private void OnExpandAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(true);

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                _btnDetail.IsChecked = !_btnDetail.IsChecked;
                ToggleDetails();
            }
            else if (e.Key == Key.F5) { ViewModel.RefreshLogs(true); }
            e.Handled = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => LoadLayout();

        private void OnLoadLayout(object sender, RoutedEventArgs e) => LoadLayout();

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

        private void OnResetLayout(object sender, RoutedEventArgs e) => new LayoutPersister(ViewModel.DockingStateFile).ResetLayout(_dockingManager);

        private void OnSaveLayout(object sender, RoutedEventArgs e) => SaveLayout();

        private void OnSearchKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.FilterMessage(_tbFilter.Text);

                //Select the whole text
                _tbFilter.SelectionStart = 0;
                _tbFilter.SelectionLength = _tbFilter.Text.Length;
            }

            e.Handled = false;
        }

        private void OnTimerTicked(object sender, EventArgs e) => ViewModel?.RefreshLogs(false);

        private void OnToggleButtonClick(object sender, RoutedEventArgs e) => ToggleDetails();

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is IHierarchy<DateTime> day && day.Level == 3)
            {
                ViewModel.LoadLogs(day.Value);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) => SaveLayout();

        private void OnUserControlPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _tbFilter.Focus();
            }

            e.Handled = false;
        }

        private void SaveLayout() => new LayoutPersister(ViewModel.DockingStateFile).Save(_dockingManager);

        private void ToggleDetails()
        {
            /* HACK: I find the panel I want to "autohide" because when retrieving layout from file
             * seems to loose the reference on the pane (or it creates a new one)
             */
            var foundPane = _dockingManager.Layout.Descendents().OfType<LayoutAnchorable>().Where(c => c.ContentId == "_detailPane").FirstOrDefault();

            foundPane?.ToggleAutoHide();
        }

        #endregion Methods
    }
}