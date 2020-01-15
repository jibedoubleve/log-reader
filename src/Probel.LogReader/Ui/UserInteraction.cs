using System;
using System.Windows;

namespace Probel.LogReader.Ui
{
    public class UserInteraction : IUserInteraction
    {
        #region Methods

        public UserAnswers Ask(string title, string question)
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

        #endregion Methods
    }
}