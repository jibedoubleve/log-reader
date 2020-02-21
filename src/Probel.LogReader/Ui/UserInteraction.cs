using Caliburn.Micro;
using Probel.LogReader.Core.Helpers;
using Probel.LogReader.ViewModels;
using System;
using System.Windows;
using Unity;

namespace Probel.LogReader.Ui
{
    public class UserInteraction : IUserInteraction
    {
        #region Fields

        protected IUnityContainer _container;
        private readonly IWindowManager _windowManager;

        #endregion Fields

        #region Constructors

        public UserInteraction(ILogger logger, IWindowManager windowManager, IUnityContainer container)
        {
            Logger = logger;
            _windowManager = windowManager;
            _container = container;
        }

        #endregion Constructors

        #region Properties

        public ILogger Logger { get; }

        #endregion Properties

        #region Methods

        public UserAnswers Ask(string question, string title = "QUESTION")
        {
            var result = MessageBox.Show(question, title, MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes: return UserAnswers.Yes;
                case MessageBoxResult.No: return UserAnswers.No;
                //case MessageBoxResult.None:
                //case MessageBoxResult.OK:
                //case MessageBoxResult.Cancel:
                default: throw new NotSupportedException($"The user answer '{result}' is not supported.");
            }
        }

        public void HandleError(Exception ex) => HandleError(ex.ToText(), ex);

        public void HandleError(string message, Exception ex)
        {
            var screen = _container.Resolve<ErrorViewModel>();
            screen.Message = message;
            screen.Exception = ex.ToString();

            //https://stackoverflow.com/questions/2329978/the-calling-thread-must-be-sta-because-many-ui-components-require-this
            Application.Current.Dispatcher.Invoke(() =>
            {
                _windowManager.ShowDialog(screen);
            });
        }

        public void Inform(string message, string title = "INFO") => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public WaitNotification NotifyWait()
        {
            var notification = new WaitNotification();
            notification.StartWaiting();
            return notification;
        }

        #endregion Methods
    }
}