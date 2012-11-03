using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;
using log4net;

namespace PayPal.SOAP
{
    public class CertificateHttpHeaderAuthStrategy : AbstractCertificateHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Exception log
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// CertificateHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public CertificateHttpHeaderAuthStrategy(string endPointUrl) : base(endPointUrl) { }

        /// <summary>
        /// Processing for TokenAuthorization} using SignatureCredential
        /// </summary>
        /// <param name="certCredential"></param>
        /// <param name="toknAuthorization"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> ProcessTokenAuthorization(
                CertificateCredential certCredential, TokenAuthorization toknAuthorization)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator signGenerator = new OAuthGenerator(certCredential.UserName, certCredential.Password);
                signGenerator.setHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                signGenerator.setToken(toknAuthorization.AccessToken);
                signGenerator.setTokenSecret(toknAuthorization.TokenSecret);
                string tokenTimeStamp = Timestamp;
                signGenerator.setTokenTimestamp(tokenTimeStamp);
                log.Debug("token = " + toknAuthorization.AccessToken + " tokenSecret=" + toknAuthorization.TokenSecret + " uri=" + endpointURL);
                signGenerator.setRequestURI(endpointURL);

                //Compute Signature
                string sign = signGenerator.ComputeSignature();
                log.Debug("Permissions signature: " + sign);
                string authorization = "token=" + toknAuthorization.AccessToken + ",signature=" + sign + ",timestamp=" + tokenTimeStamp;
                log.Debug("Authorization string: " + authorization);
                headers.Add(BaseConstants.PAYPAL_AUTHORIZATION_MERCHANT, authorization);
            }
            catch (OAuthException ae)
            {
                throw ae;
            }
            return headers;
        }

        /// <summary>
        /// Gets the UTC Timestamp
        /// </summary>
        private static string Timestamp
        {
            get
            {
                TimeSpan span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(span.TotalSeconds).ToString();
            }
        }
    }
}
