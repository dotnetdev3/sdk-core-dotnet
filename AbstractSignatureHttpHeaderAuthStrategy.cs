using System;
using System.Collections.Generic;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal
{
    public abstract class AbstractSignatureHttpHeaderAuthStrategy : IAuthenticationStrategy<Dictionary<string, string>, SignatureCredential>
    {
        /// <summary>
        /// Endpoint url
        /// </summary>
        protected string endpointURL;

        /// <summary>
        /// AbstractCertificateHttpHeaderAuthStrategy constructor
        /// </summary>
        /// <param name="endPointUrl"></param>
        public AbstractSignatureHttpHeaderAuthStrategy(string endpointURL)
        {
            this.endpointURL = endpointURL;
        }

        /// <summary>
        /// Returns CertificateCredential as HTTP headers
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public Dictionary<string, string> GenerateHeaderStrategy(SignatureCredential signCredential)
        {
            Dictionary<string, string> headers = null;

            try
            {
                if (signCredential.ThirdPartyAuthorization is TokenAuthorization)
                {
                    headers = ProcessTokenAuthorization(signCredential,(TokenAuthorization)signCredential.ThirdPartyAuthorization);
                }
                else
                {
                    headers = new Dictionary<string, string>();
                    headers.Add(BaseConstants.PAYPAL_SECURITY_USERID_HEADER, signCredential.UserName);
                    headers.Add(BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER,signCredential.Password);
                    headers.Add(BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER,signCredential.Signature);
                }
            }
            catch (OAuthException)
            {
                throw;
            }
            return headers;
        }

        /// <summary>
        /// Process Token Authorization based on API format
        /// </summary>
        /// <param name="credentialSignature"></param>
        /// <param name="tokenAuthorization"></param>
        /// <returns></returns>
        protected abstract Dictionary<string, string> ProcessTokenAuthorization(SignatureCredential credentialSignature, TokenAuthorization tokenAuthorization);
    }
}
