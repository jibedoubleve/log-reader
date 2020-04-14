using System.Linq;
using System.Text.RegularExpressions;

namespace Probel.LogReader.Plugins.IIS
{
    internal static class MatchGroupHelper
    {
        #region Methods

        public static string GetLevel(this Match match)
        {
            var value = match.Groups["sc_status"].Value.TrimEnd('\r', '\n');

            var validCodes = new int[] { 200, 301, 302 };
            var warnCodes = new int[] { 401 };
            var errorCodes = new int[] { 403, 404, 500, 503, 504 };

            if (int.TryParse(value, out var code))
            {
                if (validCodes.Contains(code)) { return "info"; }
                else if (warnCodes.Contains(code)) { return "warn"; }
                else if (errorCodes.Contains(code)) { return "error"; }
                else { return "debug"; }
            }
            else { return "debug"; }
        }

        public static string GetLogger(this Match match) => match.Get("cs_method");

        public static string GetException(this Match match)
        {
            var s_ip = match.Get("s_ip");
            var cs_method = match.Get("cs_method");
            var cs_uri_stem = match.Get("cs_uri_stem");
            var cs_uri_query = match.Get("cs_uri_query");
            var s_port = match.Get("s_port");
            var cs_username = match.Get("cs_username");
            var c_ip = match.Get("c_ip");
            var cs_user_agent = match.Get("cs_user_agent");
            var cs_referer = match.Get("cs_referer");
            var sc_status = match.Get("sc_status").Translate();
            var sc_substatus = match.Get("sc_substatus");
            var sc_win32_status = match.Get("sc_win32_status");
            var time_taken = match.Get("time_taken");

            return $@"
Client ip         : {c_ip}
---
Server IP         : {s_ip}
Server port       : {s_port}
---
Method            : {cs_method}
Uri stem          : {cs_uri_stem}
Query             : {cs_uri_query}
Username          : {cs_username}
---
User agent        : {cs_user_agent}
Referrer          : {cs_referer}
HTTP status       : {sc_status}
Protocol substatus: {sc_substatus}
Win32 Status      : {sc_win32_status}
---
Time taken        : {time_taken}
";
        }
        public static string GetMessage(this Match match)
        {
            var s_ip = match.Get("s_ip");
            var cs_method = match.Get("cs_method");
            var sc_status = match.Get("sc_status").Translate();

            return $@"Client: {s_ip} - Status {sc_status} - Method: {cs_method}";  
        }
        private static string Get(this Match m, string item, string replaceBy = null)
        {
            var value = m.Groups[item].Value.TrimEnd('\r', '\n');
            var replaceEmpty = replaceBy != null;

            return replaceEmpty
                     ? (string.IsNullOrEmpty(value) ? replaceBy : value)
                     : value;
        }

        #endregion Methods
    }
}