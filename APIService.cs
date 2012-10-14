using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using System.Text;
using log4net;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.Exception;
using System.Collections.Generic;

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

        private static readonly ILog log = LogManager.GetLogger(typeof(APIService));

        private static ArrayList retryCodes = new ArrayList(new HttpStatusCode[] 
                                                { HttpStatusCode.GatewayTimeout,
                                                  HttpStatusCode.RequestTimeout,
                                                  HttpStatusCode.InternalServerError,
                                                  HttpStatusCode.ServiceUnavailable,
                                                });

        /// <summary>
        /// Makes a request to API service
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string MakeRequestUsing(IAPICallPreHandler apiCallHandler)
        {
            string responseString = string.Empty;
            string uri = apiCallHandler.GetEndPoint();
            Dictionary<string, string> headers = apiCallHandler.GetHeaderMap();
            string payLoad = apiCallHandler.GetPayLoad();
            ConfigManager configMgr = ConfigManager.Instance;
            // Constructing HttpWebRequest object                
            ConnectionManager connMgr = ConnectionManager.Instance;
            HttpWebRequest httpRequest = connMgr.GetConnection(uri);
            httpRequest.Method = RequestMethod;
            foreach (KeyValuePair<string, string> header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            if (log.IsDebugEnabled)
            {
                foreach (string headerName in httpRequest.Headers)
                {
                    log.Debug(headerName + ":" + httpRequest.Headers[headerName]);
                }
            }
            // Adding payLoad to HttpWebRequest object
            using (StreamWriter myWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                myWriter.Write(payLoad);
                log.Debug(payLoad);
            }
            // Fire request. Retry if configured to do so
            int numRetries = (configMgr.GetProperty(BaseConstants.HTTP_CONNECTION_RETRY) != null) ?
                Convert.ToInt32(configMgr.GetProperty(BaseConstants.HTTP_CONNECTION_RETRY)) : 0;
            int retries = 0;

            do
            {
                try
                {
                    // calling the plaftform API web service and getting the response
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
                    HttpStatusCode statusCode = ((HttpWebResponse)we.Response).StatusCode;

                    log.Info("Got " + statusCode.ToString() + " response from server");
                    if (!RequiresRetry(we))
                    {
                        throw new ConnectionException("Invalid HTTP response " + we.Message);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
            } while (retries++ < numRetries);
            throw new ConnectionException("Invalid HTTP response");
        }

        /// <summary>
        /// Returns true if a HTTP retry is required
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static bool RequiresRetry(WebException ex)
        {
            if (ex.Status != WebExceptionStatus.ProtocolError)
            {
                return false;
            }
            HttpStatusCode status = ((HttpWebResponse)ex.Response).StatusCode;
            return retryCodes.Contains(status);
        }

    }


}
