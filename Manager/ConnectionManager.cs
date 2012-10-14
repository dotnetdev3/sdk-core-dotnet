using System;
using System.Text;
using System.Net;
using log4net;
using PayPal.Exception;

namespace PayPal.Manager
{
    /// <summary>
    ///  ConnectionManager retrieves HttpConnection objects used by API service
    /// </summary>
    public sealed class ConnectionManager
    {
        private static ILog log = LogManager.GetLogger(typeof(ConnectionManager));
        
        /// <summary>
        /// Singleton instance of ConnectionManager
        /// </summary>
        private static readonly ConnectionManager singletonInstance = new ConnectionManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static ConnectionManager() { }
        
        /// <summary>
        /// Private constructor
        /// </summary>
        private ConnectionManager() { }

        /// <summary>
        /// Gets the Singleton instance of ConnectionManager
        /// </summary>
        public static ConnectionManager Instance
        {
            get
            {
                return singletonInstance;
            }
        }

        /// <summary>
        /// Create and Config a HttpWebRequest
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public HttpWebRequest GetConnection(string url)
        {
            ConfigManager configMgr = ConfigManager.Instance;
            HttpWebRequest httpRequest = null;
                        
            try
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (UriFormatException ex)
            {
                log.Debug(ex.Message);
                throw new ConfigException("Invalid URI " + url);
            }

            // Set connection timeout
            int ConnectionTimeout = 0;
            bool Success = int.TryParse(configMgr.GetProperty(BaseConstants.HTTP_CONNECTION_TIMEOUT), out ConnectionTimeout);
            if (!Success)
            {
                ConnectionTimeout = BaseConstants.DEFAULT_TIMEOUT;
            }

            httpRequest.Timeout = ConnectionTimeout;

            // Set request proxy for tunnelling http requests via a proxy server
            string proxyAddress = configMgr.GetProperty(BaseConstants.HTTP_PROXY_ADDRESS);
            if (proxyAddress != null)
            {
                WebProxy requestProxy = new WebProxy();
                requestProxy.Address = new Uri(proxyAddress);
                string proxyCredentials = configMgr.GetProperty(BaseConstants.HTTP_PROXY_CREDENTIAL);
                if (proxyCredentials != null)
                {
                    string[] proxyDetails = proxyCredentials.Split(':');
                    if (proxyDetails.Length == 2)
                    {
                        requestProxy.Credentials = new NetworkCredential(proxyDetails[0], proxyDetails[1]);
                    }
                }                
                httpRequest.Proxy = requestProxy;
            }
            return httpRequest;
        }
    }
}
