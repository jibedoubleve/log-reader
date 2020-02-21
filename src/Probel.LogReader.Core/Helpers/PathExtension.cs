using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Core.Helpers
{
    public static class PathExtension
    {
        #region Methods

        public static string Expand(this string path) => Environment.ExpandEnvironmentVariables(path);

        public static bool IsValidPath(this string toTest)
        {
            Regex containsABadCharacter = new Regex("["
                  + Regex.Escape(new string(System.IO.Path.GetInvalidPathChars())) + "]");
            if (containsABadCharacter.IsMatch(toTest)) { return false; };

            // other checks for UNC, drive-path format, etc

            return true;
        }

        public static bool IsDirectory(this string toTest)
        {
            if (toTest.IsValidPath()) { return Directory.Exists(toTest); }
            else { return false; }

            #endregion Methods
        }
    }
}