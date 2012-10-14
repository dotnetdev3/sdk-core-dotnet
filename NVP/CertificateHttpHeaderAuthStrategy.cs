using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Authentication;
using PayPal.Exception;
using log4net;

namespace PayPal.NVP
{
    public class CertificateHttpHeaderAuthStrategy : AbstractCertificateHttpHeaderAuthStrategy 
    {
        private static ILog log = LogManager.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// CertificateHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public CertificateHttpHeaderAuthStrategy(string endPointUrl) : base(endPointUrl) { }
            
        /// <summary>
        /// Processing for TokenAuthorization using SignatureCredential
        /// </summary>
        /// <param name="sigCred"></param>
        /// <param name="tokenAuth"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> ProcessTokenAuthorization(
                CertificateCredential certCredential, TokenAuthorization tokenAuth)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator signGenerator = new OAuthGenerator(certCredential.UserName, certCredential.Password);
                signGenerator.setHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                signGenerator.setToken(tokenAuth.AccessToken);
                signGenerator.setTokenSecret(tokenAuth.TokenSecret);
                string tokenTimeStamp = Timestamp;
                signGenerator.setTokenTimestamp(tokenTimeStamp);
                log.Debug("token = " + tokenAuth.AccessToken + " tokenSecret=" + tokenAuth.TokenSecret + " uri=" + endpointURL);
                signGenerator.setRequestURI(endpointURL);

                //Compute Signature
                string sign = signGenerator.ComputeSignature();
                log.Debug("Permissions signature: " + sign);
                string authorization = "token=" + tokenAuth.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                log.Debug("Authorization string: " + authorization);
                headers.Add(BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM, authorization);
            }
            catch (OAuthException)
            {
                throw;
            }
            return headers;
        }

        public static string Timestamp
        {
            get
            {
                TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(span.TotalSeconds).ToString();
            }            
        }
    }
}
