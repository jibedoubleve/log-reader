using System;

namespace Probel.LogReader.Core.Plugins
{
    public class PluginInfo
    {
        #region Constructors

        public PluginInfo(Guid id, string name, string docUrl, string description, string colouration)
        {
            DocUrl = docUrl;
            Id = id;
            Name = name;
            Description = description;
            Colouration = colouration;
        }

        #endregion Constructors

        #region Properties

        public string Colouration { get; }
        public string Description { get; set; }
        public string DocUrl { get; }
        public Guid Id { get; }
        public string Name { get; }

        #endregion Properties
    }
}