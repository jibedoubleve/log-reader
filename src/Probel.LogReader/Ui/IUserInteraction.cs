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

        UserAnswers Ask(string title, string question);

        #endregion Methods
    }
}