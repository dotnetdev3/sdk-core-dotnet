using System;
using System.Collections.Generic;
using System.Text;

using PayPal.Authentication;

namespace PayPal
{
    public class SignatureSOAPHeaderAuthStrategy : IAuthenticationStrategy<string, SignatureCredential>
    {
        /// <summary>
        /// Instance of ThirdPartyAuthorization
        /// </summary>
        private IThirdPartyAuthorization thirdPartyAuthorization;

        /// <summary>
        /// Constructor
        /// </summary>
        public SignatureSOAPHeaderAuthStrategy()
        {
        }

        /// <summary>
        /// Gets and sets any instance of {@link ThirdPartyAuthorization}
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorize
        {
            get
            {
                return thirdPartyAuthorization;
            }
            set
            {
                this.thirdPartyAuthorization = value;
            }
        }

        public string GenerateHeaderStrategy(SignatureCredential credential)
        {
            string payLoad = null;
            if (thirdPartyAuthorization is TokenAuthorization)
            {
                payLoad = tokenAuthPayLoad();
            }
            else if (thirdPartyAuthorization is SubjectAuthorization)
            {
                authPayLoad(credential, (SubjectAuthorization)thirdPartyAuthorization);
            }
            else
            {
                authPayLoad(credential, null);
            }
            return payLoad;
        }

        private string tokenAuthPayLoad()
        {
            string payLoad = null;
            StringBuilder soapMsg = new StringBuilder();
            soapMsg.Append("<soapenv:Header>");
            soapMsg.Append("<urn:RequesterCredentials/>");
            soapMsg.Append("</soapenv:Header>");
            return payLoad;
        }

        private string authPayLoad(SignatureCredential credential,
                SubjectAuthorization subjectAuth)
        {
            string payLoad = null;
            StringBuilder soapMsg = new StringBuilder();
            soapMsg.Append("<soapenv:Header>");
            soapMsg.Append("<urn:RequesterCredentials>");
            soapMsg.Append("<ebl:Credentials>");
            soapMsg.Append("<ebl:Username>" + credential.UserName
                    + "</ebl:Username>");
            soapMsg.Append("<ebl:Password>" + credential.Password
                    + "</ebl:Password>");
            soapMsg.Append("<ebl:Signature>" + credential.Signature
                    + "</ebl:Signature>");
            if (subjectAuth != null)
            {
                soapMsg.Append("<ebl:Subject>" + subjectAuth.Subject
                        + "</ebl:Subject>");
            }
            soapMsg.Append("</ebl:Credentials>");
            soapMsg.Append("</urn:RequesterCredentials>");
            soapMsg.Append("</soapenv:Header>");
            return payLoad;
        }
    }
}
