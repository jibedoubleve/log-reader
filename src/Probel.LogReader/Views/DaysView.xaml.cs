using Probel.LogReader.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for DaysView.xaml
    /// </summary>
    public partial class DaysView : UserControl
    {
        #region Constructors

        public DaysView()
        {
            InitializeComponent();      
        }

        #endregion Constructors

        #region Properties

        private DaysViewModel ViewModel => DataContext as DaysViewModel;

        #endregion Properties

        #region Methods

        //TODO: Error handling
        private async void OnMouseDoubleClickOnDay(object sender, MouseButtonEventArgs e) => await ViewModel.LoadLogsAsync();

        #endregion Methods
    }
}