using Caliburn.Micro;
using System.Windows.Input;

namespace Probel.LogReader.Models
{
    public class MenuItemModel : PropertyChangedBase
    {
        #region Fields

        private string _name;

        #endregion Fields

        #region Properties

        public ICommand MenuCommand
        {
            get;
            set;
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        #endregion Properties
    }
}