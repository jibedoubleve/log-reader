using System;
using System.Diagnostics;
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

        #region Methods

        private void OnLogsMouseDoubleClick(object sender, MouseButtonEventArgs e) => IsDetailsVisible.IsChecked = true;

        #endregion Methods

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
    }
}