using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.SessionState;
using System.Text;
using log4net;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    /// <summary>
    /// Calls the actual Platform API web service for the given Payload and APIProfile settings
    /// </summary>
    public class APIService
    {
        /// <summary>
        /// HTTP Method needs to be set.
        /// </summary>
        private const string RequestMethod = BaseConstants.REQUESTMETHOD;

        private static readonly ILog log = LogManager.GetLogger( typeof(APIService) );

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        private string serviceName;

        public APIService(string serviceName)
        {
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Calls the platform API web service for given payload and returns the response payload.
        /// </summary>
        /// <returns>returns the response payload</returns>
        public string makeRequest(String method, string requestPayload, string apiUsername)
        {

            ConfigManager configMgr = ConfigManager.Instance;
            CredentialManager credMgr = CredentialManager.Instance;
            string url, responseString = string.Empty;

            // Constructing the URL to be called from Profile settings
            // Most of PayPal's APIs include the service method name as part of the URL
            url = getAPIEndpoint(method);
            log.Debug("Connecting to " + url);

            // Constructing HttpWebRequest object                
            ConnectionManager conn = ConnectionManager.Instance;

            
            HttpWebRequest httpRequest = conn.getConnection(url);
            httpRequest.Method = RequestMethod;

            // Set up Headers                
            credMgr.SetAuthenticationParams(httpRequest, apiUsername);                
    
            // Adding payLoad to HttpWebRequest object
            using (StreamWriter myWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                myWriter.Write(requestPayload);                    
                log.Debug(requestPayload);                    
            }

            int numRetries = (configMgr.GetProperty("requestRetries") != null ) ? 
                Int32.Parse(configMgr.GetProperty("requestRetries")) : 0;
            int retries = 0;

            do {
                try
                {
                    // calling the plaftform API web service and getting the resoponse
                    using (WebResponse response = httpRequest.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            responseString = sr.ReadToEnd();
                            log.Debug("Service response");
                            log.Debug(responseString);                                                  
                            return responseString;
                        }
                    }
                }
                // server responses in the range of 4xx and 5xx throw a WebException
                catch (WebException we)
                {
                    HttpStatusCode statusCode =  ( (HttpWebResponse) we.Response ).StatusCode;
                    log.Info("Got " + statusCode.ToString() + " response from server");
                    if (!requiresRetry(we))
                    {
                        throw new ConnectionException("Invalid HTTP response " + we.Message);
                    }
                }           
                catch (System.Exception ex)
                {                
                    throw ex;
                }
            } while ( retries++ < numRetries);

            throw new ConnectionException("Invalid HTTP response");
        }

        private string getAPIEndpoint(string method) {
            ConfigManager configMgr = ConfigManager.Instance;
            return configMgr.GetProperty("endpoint") + this.serviceName + '/' + method;
        }

        /// <summary>
        /// returns true if a HTTP retry is required
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private Boolean requiresRetry(WebException ex)
        {
            if (ex.Status != WebExceptionStatus.ProtocolError)
                return false;
            HttpStatusCode status = ((HttpWebResponse)ex.Response).StatusCode;
            return retryCodes.Contains(status); 
        }

    }


}
