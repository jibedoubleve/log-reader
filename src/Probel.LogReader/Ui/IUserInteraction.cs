using Probel.LogReader.Core.Helpers;
using System;

namespace Probel.LogReader.Ui
{
    public enum UserAnswers
    {
        Yes,
        No,
    }

    public interface IUserInteraction
    {
        #region Properties

        ILogger Logger { get; }

        #endregion Properties

        #region Methods

        UserAnswers Ask(string question, string title = "QUESTION");

        void HandleError(Exception ex);

        void HandleError(string message, Exception ex);

        void Inform(string message, string title = "INFO");

        /// <summary>
        /// This method create a <see cref="WaitNotification"/> which after construction
        /// changes the mouse cursor to a <c>Wait</c> cursor and on dipose reset the
        /// mouse cursor to its default status
        /// </summary>
        /// <returns></returns>
        WaitNotification NotifyWait();

        #endregion Methods
    }
}