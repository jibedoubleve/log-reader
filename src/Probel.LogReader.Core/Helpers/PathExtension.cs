using System;

namespace Probel.LogReader.Core.Helpers
{
    public static class PathExtension
    {
        #region Methods

        public static string Expand(this string path) => Environment.ExpandEnvironmentVariables(path);

        #endregion Methods
    }
}