using System;
using System.Collections.Generic;
using System.Text;

using PayPal.Authentication;

namespace PayPal
{
    public class CertificateSOAPHeaderAuthStrategy : IAuthenticationStrategy<string, CertificateCredential>
    {
        /// <summary>
        /// Instance of ThirdPartyAuthorization
        /// </summary>
        private IThirdPartyAuthorization thirdPartyAuthorization;

        public CertificateSOAPHeaderAuthStrategy()
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

        public string GenerateHeaderStrategy(CertificateCredential credential) 
        {
		    string payLoad = null;
		
            if (thirdPartyAuthorization is TokenAuthorization) 
            {
			    payLoad = TokenAuthPayLoad();
		    } 
            else if (thirdPartyAuthorization is SubjectAuthorization) 
            {
			    AuthPayLoad(credential,
					(SubjectAuthorization) thirdPartyAuthorization);
		    } 
            else 
            {
			    AuthPayLoad(credential, null);
		    }
		    return payLoad;
	    }

        /// <summary>
        /// Returns a empty soap header String, 
        /// token authorization does not bear a credential part
        /// </summary>
        /// <returns></returns>
        private string TokenAuthPayLoad()
        {
            string payLoad = null;
            StringBuilder soapMsg = new StringBuilder();
            soapMsg.Append("<soapenv:Header>");
            soapMsg.Append("<urn:RequesterCredentials/>");
            soapMsg.Append("</soapenv:Header>");
            return payLoad;
        }

        private string AuthPayLoad(CertificateCredential credential,
                SubjectAuthorization subjectAuthorization)
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

            // Append subject credential if available
            if (subjectAuthorization != null)
            {
                soapMsg.Append("<ebl:Subject>" + subjectAuthorization.Subject
                        + "</ebl:Subject>");
            }
            soapMsg.Append("</ebl:Credentials>");
            soapMsg.Append("</urn:RequesterCredentials>");
            soapMsg.Append("</soapenv:Header>");
            return payLoad;
        }
    }
}
