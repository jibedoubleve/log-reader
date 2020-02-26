using System;

namespace Probel.LogReader.Tests.Helpers
{
    public static class Rand
    {
        #region Fields

        private static string[] _comparisionOperators = new string[] { "<", "<=", "==", "!=", ">", ">=" };
        private static string[] _ensembleOperators = new string[] { "in", "not in" };
        private static string[] _logicalOperators = new string[] { "and", "or" };
        private static Random _random = new Random();

        #endregion Fields

        #region Properties

        public static string ComparisionOperator
        {
            get
            {
                var i = _random.Next(0, 5);
                return _comparisionOperators[i];
            }
        }

        public static string Date => DateTime.Now.ToString();

        public static string EnsembleOperator
        {
            get
            {
                var i = _random.Next(0, 2);
                return _ensembleOperators[i];
            }
        }

        public static string IntegerAsString => _random.Next().ToString();

        public static string LogicalOperator
        {
            get
            {
                var i = _random.Next(0, 2);
                return _logicalOperators[i];
            }
        }

        public static string Text => Guid.NewGuid().ToString();

        #endregion Properties
    }
}