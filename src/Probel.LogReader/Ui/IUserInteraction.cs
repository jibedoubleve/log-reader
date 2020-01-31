namespace Probel.LogReader.Ui
{
    public enum UserAnswers
    {
        Yes,
        No,
    }

    public interface IUserInteraction
    {
        #region Methods

        UserAnswers Ask(string question, string title = "QUESTION");

        void Inform(string message, string title = "INFO");

        #endregion Methods
    }
}