using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;
using log4net;


namespace PayPal
{
    public class CertificateHttpHeaderAuthStrategy : AbstractCertificateHttpHeaderAuthStrategy
    {

        private static ILog log = LogManager.GetLogger(typeof(CertificateHttpHeaderAuthStrategy));

        /// <summary>
        /// CertificateHttpHeaderAuthStrategy
        /// </summary>
        /// <param name="endPointUrl"></param>
        public CertificateHttpHeaderAuthStrategy(string endPointUrl) : base(endPointUrl)
        {
            
	    }

        /// <summary>
        /// Processing for {@link TokenAuthorization} under
        /// {@link SignatureCredential}
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
                OAuthGenerator sigGenerator = new OAuthGenerator(certCredential.UserName, certCredential.Password);
                sigGenerator.setHTTPMethod(OAuthGenerator.HTTPMethod.POST);
                sigGenerator.setToken(tokenAuth.AccessToken);
                sigGenerator.setTokenSecret(tokenAuth.TokenSecret);
                string tokenTimeStamp = GenerateTimeStamp();
                sigGenerator.setTokenTimestamp(tokenTimeStamp);
                log.Debug("token = " + tokenAuth.AccessToken + " tokenSecret=" + tokenAuth.TokenSecret + " uri=" + endpointURL);
                sigGenerator.setRequestURI(endpointURL);

                //Compute Signature
                string sig = sigGenerator.ComputeSignature();
                log.Debug("Permissions signature: " + sig);
                string authorization = "token=" + tokenAuth.AccessToken + ",signature=" + sig + ",timestamp=" + tokenTimeStamp;
                log.Debug("Authorization string: " + authorization);
                headers.Add(BaseConstants.PAYPAL_AUTHORIZATION_MERCHANT, authorization);
            }
            catch (OAuthException)
            {
                throw;
            }
            return headers;
        }

        public static string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}
