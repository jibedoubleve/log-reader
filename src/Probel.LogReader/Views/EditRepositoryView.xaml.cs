using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for EditRepositoryView.xaml
    /// </summary>
    public partial class EditRepositoryView : UserControl
    {
        #region Constructors

        public EditRepositoryView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion Methods
    }
}