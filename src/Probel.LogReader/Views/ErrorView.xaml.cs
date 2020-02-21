using System.Windows;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for ErrorView.xaml
    /// </summary>
    public partial class ErrorView : Window
    {
        #region Constructors

        public ErrorView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void OnClose(object sender, RoutedEventArgs e) => this.Close();

        #endregion Methods
    }
}