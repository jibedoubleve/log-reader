using Probel.LogReader.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Probel.LogReader.Views
{
    /// <summary>
    /// Interaction logic for EditFilterBindingView.xaml
    /// </summary>
    public partial class EditFilterBindingsView : UserControl
    {
        #region Fields

        private const string ACTIVE = "active";
        private const string INACTIVE = "inactive";

        #endregion Fields

        #region Constructors

        public EditFilterBindingsView()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        private EditFilterBindingsViewModel ViewModel => DataContext as EditFilterBindingsViewModel;

        #endregion Properties

        #region Methods

        private void OnActiveFiltersDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(ACTIVE))
            {
                var dropped = e.Data.GetData(ACTIVE);

                ViewModel.AddActive(dropped);
            }
        }

        private void OnActiveFiltersMouseDoubleClick(object sender, MouseButtonEventArgs e) => ViewModel.AddInactive(_activeFilters.SelectedItem);

        private void OnActiveFiltersPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var af = _activeFilters.SelectedItem;

            if (e.LeftButton == MouseButtonState.Pressed && af != null)
            {
                DataObject data = new DataObject();
                data.SetData(INACTIVE, af);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void OnInactiveFiltersDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(INACTIVE))
            {
                var dropped = e.Data.GetData(INACTIVE);

                ViewModel.AddInactive(dropped);
            }
        }

        private void OnInactiveFiltersMouseDoubleClick(object sender, MouseButtonEventArgs e) => ViewModel.AddActive(_inactiveFilters.SelectedItem);

        private void OnInactiveFiltersPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var af = _inactiveFilters.SelectedItem;

            if (e.LeftButton == MouseButtonState.Pressed && af != null)
            {
                DataObject data = new DataObject();
                data.SetData(ACTIVE, af);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        #endregion Methods
    }
}