using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using Probel.LogReader.Helpers;
using Probel.LogReader.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
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

            var fileName = ViewModel.DockingStateFile;
            var serializer = new XmlLayoutSerializer(_dockingManager);

            if (File.Exists(fileName))
            {
                using (var stream = new StreamReader(fileName))
                {
                    serializer.Deserialize(stream);
                }
            }
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

        private void OnCollapseAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(false);

        private void OnExpandAll(object sender, RoutedEventArgs e) => _treeView.SetExpansion(true);

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4) { ToggleDetails(); }
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

        private void OnSaveLayout(object sender, RoutedEventArgs e) => SaveLayout();

        private void OnTimerTicked(object sender, EventArgs e) => ViewModel?.RefreshLogs(false);

        private void OnToggleButtonClick(object sender, RoutedEventArgs e) => ToggleDetails();

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is IHierarchy<DateTime> day)
            {
                ViewModel.LoadLogs(day.Value);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) => SaveLayout();

        private void SaveLayout()
        {
            var serializer = new XmlLayoutSerializer(_dockingManager);
            var fileName = ViewModel.DockingStateFile;

            using (var stream = new StreamWriter(fileName))
            {
                serializer.Serialize(stream);
            }
        }

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