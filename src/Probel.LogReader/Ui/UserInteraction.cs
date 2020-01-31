using System;
using System.Windows;

namespace Probel.LogReader.Ui
{
    public class UserInteraction : IUserInteraction
    {
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

        public void Inform(string message, string title = "INFO") => MessageBox.Show(title, message, MessageBoxButton.OK, MessageBoxImage.Information);

        #endregion Methods
    }
}