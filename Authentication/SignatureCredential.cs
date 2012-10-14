using System;

namespace PayPal.Authentication
{
    /// <summary>
    /// SignatureCredential 
    /// Encapsulates signature credential information 
    /// used by service authentication systems
    /// </summary>
    public class SignatureCredential : ICredential
    {
        /// <summary>
        /// Username credential
        /// </summary> 
        private string usrName;
                
        /// <summary>
        /// Password credential
        /// </summary>
        private string pasWord;

        /// <summary>
        /// Application Id (Used by Platform APIs)
        /// </summary>
        private string appId;

        /// <summary>
        /// Signature
        /// </summary>
        private string sign;

        /// <summary>
        /// Instance of IThirdPartyAuthorization
        /// </summary>
        private IThirdPartyAuthorization thrdPartyAuthorization;

        /// <summary>
        /// SignatureCredential constructor
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pssword"></param>
        /// <param name="appId"></param>
        /// <param name="sign"></param>
        public SignatureCredential(string usrName, string pssword,
                string appId, string sign) : base()
        {
            if (string.IsNullOrEmpty(usrName) || string.IsNullOrEmpty(pssword) ||
                string.IsNullOrEmpty(sign))
            {
                throw new ArgumentException("Signature Credential arguments cannot be null");
            }
            this.usrName = usrName;
            this.pasWord = pssword;
            this.appId = appId;
            this.sign = sign;
        }

        /// <summary>
        /// SignatureCredential constructor overload
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="applicationId"></param>
        /// <param name="signature"></param>
        /// <param name="thrdPartyAuthorize"></param>
        public SignatureCredential(string usrName, string pasWord,
                string applicationId, string signature,
                IThirdPartyAuthorization thrdPartyAuthorization)
            : this(usrName, pasWord, applicationId, signature)
        {

            this.thrdPartyAuthorization = thrdPartyAuthorization;
        }
        
        /// <summary>
        /// Gets and sets the ApplicationId
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return appId;
            }

            set
            {
                this.appId = value;
            }
        }
       
        /// <summary>
        /// Gets and sets any instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return thrdPartyAuthorization;
            }
            set
            {
                this.thrdPartyAuthorization = value;
            }
        }

        /// <summary>
        /// Gets the UserName
        /// </summary>
        public string UserName
        {
            get
            {
                return usrName;
            }
        }

        /// <summary>
        /// Gets the Password
        /// </summary>
        public string Password
        {
            get
            {
                return pasWord;
            }
        }
        
        /// <summary>
        /// Gets the Signature
        /// </summary>
        public string Signature
        {
            get
            {
                return sign;
            }
        }
    }
}
