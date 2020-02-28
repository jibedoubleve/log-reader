using Probel.LogReader.Core.Helpers;
using Probel.LogReader.ViewModels;
using System;
using System.Diagnostics;
using System.Reflection;
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
        private void OnWindowLoaded(object sender, RoutedEventArgs e) => ViewModel?.LoadMenus();

        #endregion Methods

        private void OnClickAbout(object sender, RoutedEventArgs e)
        {
            var v = VersionManager.Retrieve(Assembly.GetExecutingAssembly());

            var nl = Environment.NewLine;
            var msg = $"Version: {v.Version}{nl}File Version: {v.FileVersion}{nl}SemVer: {v.SemVer}{nl}Author: JB Wautier";
            MessageBox.Show(msg, "Log Reader", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnViewWebsite(object sender, RoutedEventArgs e) => Process.Start("https://github.com/jibedoubleve/log-reader");

        private void OnViewLicense(object sender, RoutedEventArgs e) => Process.Start("https://github.com/jibedoubleve/log-reader/blob/develop/LICENSE");
    }
}