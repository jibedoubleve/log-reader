using Caliburn.Micro;

namespace Probel.LogReader.ViewModels
{
    public class ErrorViewModel : Screen
    {
        #region Fields

        private string _exception;
        private string _message;

        #endregion Fields

        #region Properties

        public string Exception
        {
            get => _exception;
            set => Set(ref _exception, value, nameof(Exception));
        }

        public string Message
        {
            get => _message;
            set => Set(ref _message, value, nameof(Message));
        }

        #endregion Properties
    }
}