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
    }
}