namespace Probel.LogReader.Plugins.Csv.Config
{
    public class QueryLogSettings
    {
        #region Properties

        public string Delimiter { get; set; }
        public string Encoding { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string ThreadId { get; set; }
        public string Time { get; set; }

        #endregion Properties
    }
}