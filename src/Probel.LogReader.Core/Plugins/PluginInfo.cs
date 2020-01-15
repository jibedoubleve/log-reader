using System;

namespace Probel.LogReader.Core.Plugins
{
    public class PluginInfo
    {
        #region Constructors

        public PluginInfo(Guid id, string name, string docUrl)
        {
            DocUrl = docUrl;
            Id = id;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string DocUrl { get; }
        public Guid Id { get; }
        public string Name { get; }

        #endregion Properties
    }
}