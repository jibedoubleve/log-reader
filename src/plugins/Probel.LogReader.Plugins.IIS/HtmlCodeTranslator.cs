using System.Collections.Generic;

namespace Probel.LogReader.Plugins.IIS
{
    internal static class HtmlCodeTranslator
    {
        #region Fields

        private static readonly Dictionary<string, string> Codes = new Dictionary<string, string>()
        {
            { "100", "Continue"},
            { "101", "SwitchingProtocols"},
            { "102", "Processing"},
            { "103", "EarlyHints"},
            { "200", "OK"},
            { "201", "Created"},
            { "202", "Accepted"},
            { "203", "Non-AuthoritativeInformation"},
            { "204", "NoContent"},
            { "205", "ResetContent"},
            { "206", "PartialContent"},
            { "207", "Multi-Status"},
            { "208", "AlreadyReported"},
            { "210", "ContentDifferent"},
            { "226", "IMUsed"},
            { "300", "MultipleChoices"},
            { "301", "MovedPermanently"},
            { "302", "Found"},
            { "303", "SeeOther"},
            { "304", "NotModified"},
            { "305", "UseProxy(depuisHTTP/1.1)"},
            { "306", "SwitchProxy"},
            { "307", "TemporaryRedirect"},
            { "308", "PermanentRedirect"},
            { "310", "ToomanyRedirects"},
            { "400", "BadRequest"},
            { "401", "Unauthorized"},
            { "402", "PaymentRequired"},
            { "403", "Forbidden"},
            { "404", "NotFound"},
            { "405", "MethodNotAllowed"},
            { "406", "NotAcceptable"},
            { "407", "ProxyAuthenticationRequired"},
            { "408", "RequestTime-out"},
            { "409", "Conflict"},
            { "410", "Gone"},
            { "411", "LengthRequired"},
            { "412", "PreconditionFailed"},
            { "413", "RequestEntityTooLarge"},
            { "414", "Request-URITooLong"},
            { "415", "UnsupportedMediaType"},
            { "416", "Requestedrangeunsatisfiable"},
            { "417", "Expectationfailed"},
            { "418", "I’mateapot"},
            { "421", "Badmapping/MisdirectedRequest"},
            { "422", "Unprocessableentity"},
            { "423", "Locked"},
            { "424", "Methodfailure"},
            { "425", "UnorderedCollection"},
            { "426", "UpgradeRequired"},
            { "428", "PreconditionRequired"},
            { "429", "TooManyRequests"},
            { "431", "RequestHeaderFieldsTooLarge"},
            { "449", "RetryWith"},
            { "450", "BlockedbyWindowsParentalControls"},
            { "451", "UnavailableForLegalReasons"},
            { "456", "UnrecoverableError"},
            { "444", "NoResponse"},
            { "495", "SSLCertificateError"},
            { "496", "SSLCertificateRequired"},
            { "497", "HTTPRequestSenttoHTTPSPort"},
            { "498", "Tokenexpired/invalid"},
            { "499", "ClientClosedRequest"},
            { "500", "InternalServerError"},
            { "501", "NotImplemented"},
            { "502", "BadGatewayouProxyError"},
            { "503", "ServiceUnavailable"},
            { "504", "GatewayTime-out"},
            { "505", "HTTPVersionnotsupported"},
            { "506", "VariantAlsoNegotiates"},
            { "507", "Insufficientstorage"},
            { "508", "Loopdetected"},
            { "509", "BandwidthLimitExceeded"},
            { "510", "Notextended"},
            { "511", "Networkauthenticationrequired"},
            { "520", "UnknownError"},
            { "521", "WebServerIsDown"},
            { "522", "ConnectionTimedOut"},
            { "523", "OriginIsUnreachable"},
            { "524", "ATimeoutOccurred"},
            { "525", "SSLHandshakeFailed"},
            { "526", "InvalidSSLCertificate"},
            { "527", "RailgunError"}
        };

        #endregion Fields

        #region Methods

        public static string Translate(this string code)
        {
            return $"{code} - {Codes[code]}";
        }

        #endregion Methods
    }
}