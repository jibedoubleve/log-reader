using Caliburn.Micro;
using Notifications.Wpf;
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
        private readonly INotificationManager _notifyer;
        private readonly IWindowManager _windowManager;

        #endregion Fields

        #region Constructors

        public UserInteraction(ILogger logger, IWindowManager windowManager, IUnityContainer container, INotificationManager notifyer)
        {
            _notifyer = notifyer;
            Logger = logger;
            _windowManager = windowManager;
            _container = container;
        }

        #endregion Constructors

        #region Properties

        public ILogger Logger { get; }

        #endregion Properties

        #region Methods

        public UserAnswers Ask(string question, string title = "Question")
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

            NotifyError(message);

            //https://stackoverflow.com/questions/2329978/the-calling-thread-must-be-sta-because-many-ui-components-require-this
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    _windowManager.ShowDialog(screen);
            //});
        }

        public void NotifyInformation(string message) => Notify(message);

        public void NotifySuccess(string message) => Notify(message, NotificationType.Success);

        public WaitNotification NotifyWait()
        {
            var notification = new WaitNotification();
            notification.StartWaiting();
            return notification;
        }

        public void NotifyWarning(string message) => Notify(message, NotificationType.Warning);

        public void NotifyError(string message) => Notify(message, NotificationType.Error);

        private void Notify(string message, NotificationType type = NotificationType.Information, string title = null)
        {
            title = string.IsNullOrEmpty(title) ? type.ToString() : title;
            var content = new NotificationContent()
            {
                Title = title,
                Message = message,
                Type = type
            };
            _notifyer.Show(content, areaName: "WindowArea");
        }

        #endregion Methods
    }
}