using System;
using System.Diagnostics;
using System.Reflection;

namespace Probel.LogReader.Core.Helpers
{
    public class VersionManager
    {
        #region Properties

        public string FileVersion { get; private set; }

        public string SemVer { get; private set; }

        public string Version { get; private set; }

        #endregion Properties

        #region Methods

        public static VersionManager Retrieve(Assembly asm = null)
        {
            if (asm == null) { asm = Assembly.GetExecutingAssembly(); }

            var version = asm.GetName().Version.ToString();
            var fileVersion = FileVersionInfo.GetVersionInfo(asm.Location).FileVersion.ToString();

            var semver = FileVersionInfo.GetVersionInfo(asm.Location).ProductVersion.ToString();
            var semverSplit = semver.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            semver = semverSplit.Length > 0 ? semverSplit[0] : semver;

            return new VersionManager
            {
                Version = version,
                FileVersion = fileVersion,
                SemVer = semver,
            };
        }

        #endregion Methods
    }
}