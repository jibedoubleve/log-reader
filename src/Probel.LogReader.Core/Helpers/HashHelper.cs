using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Probel.LogReader.Core.Helpers
{
    public static class HashHelper
    {
        #region Methods

        public static string GetHash(object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetString(Hash(str));
        }

        public static bool HasSameHashAs(this object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null) { return false; }
            else if (obj1.GetType() != obj2.GetType()) { return false; }
            else
            {
                var hash1 = GetHash(obj1);
                var hash2 = GetHash(obj2);
                return hash1 == hash2;
            }
        }

        private static byte[] Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        #endregion Methods
    }
}