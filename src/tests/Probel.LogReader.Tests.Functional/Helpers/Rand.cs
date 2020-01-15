using System;

namespace Probel.LogReader.Tests.Helpers
{
    public static class Rand
    {
        #region Properties

        public static string Text => Guid.NewGuid().ToString();

        #endregion Properties
    }
}