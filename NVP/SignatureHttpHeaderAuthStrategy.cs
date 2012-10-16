using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Exception;
using PayPal.Authentication;
using log4net;

namespace PayPal.NVP
{
    public class SignatureHttpHeaderAuthStrategy : AbstractSignatureHttpHeaderAuthStrategy
    {
        /// <summary>
        /// Exception log
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// SignatureHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public SignatureHttpHeaderAuthStrategy(string endpointURL) : base(endpointURL) { }

	    /// <summary>
        /// Processing TokenAuthorization} using SignatureCredential
        /// </summary>
        /// <param name="sigCred"></param>
        /// <param name="tokenAuth"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> ProcessTokenAuthorization(
                SignatureCredential sigCred, TokenAuthorization tokenAuth)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            try
            {
                OAuthGenerator sigGenerator = new OAuthGenerator(sigCred.UserName, sigCred.Password);
                sigGenerator.setHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                sigGenerator.setToken(tokenAuth.AccessToken);
                sigGenerator.setTokenSecret(tokenAuth.TokenSecret);
                string tokenTimeStamp = Timestamp;
                sigGenerator.setTokenTimestamp(tokenTimeStamp);
                log.Debug("token = " + tokenAuth.AccessToken + " tokenSecret=" + tokenAuth.TokenSecret + " uri=" + endpointURL);
                sigGenerator.setRequestURI(endpointURL);

                //Compute Signature
                string sign = sigGenerator.ComputeSignature();
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
