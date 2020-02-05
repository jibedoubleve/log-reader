using Probel.LogReader.Colouration;
using Probel.LogReader.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for EditRepositoryView.xaml
    /// </summary>
    public partial class EditRepositoryView : UserControl
    {
        #region Fields

        private readonly IColourator _colourator;

        #endregion Fields

        #region Constructors

        public EditRepositoryView()
        {
            InitializeComponent();
            _colourator = new Colourator(_logEditor, _dayEditor);
        }

        #endregion Constructors

        #region Properties

        private EditRepositoryViewModel ViewModel => DataContext as EditRepositoryViewModel;

        #endregion Properties

        #region Methods

        private void OnLoaded(object sender, RoutedEventArgs e) => ViewModel.Refresh(_colourator);

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion Methods
    }
}