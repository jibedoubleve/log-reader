using Probel.LogReader.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        #region Constructors

        public MainView() => InitializeComponent();

        #endregion Constructors

        #region Properties

        private MainViewModel ViewModel => DataContext as MainViewModel;

        #endregion Properties

        #region Methods
        private void OnWindowLoaded(object sender, RoutedEventArgs e) =>  ViewModel?.LoadMenus();

        #endregion Methods
    }
}