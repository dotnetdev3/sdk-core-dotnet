using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal.Authentication
{
    /// <summary>
    /// CertificateCredential
    /// Encapsulates certificate credential information
    /// used by service authentication systems
    /// </summary>
    public class CertificateCredential : ICredential
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
        /// Certificate file
        /// </summary>
        private string certFile;

        /// <summary>
        /// Password of the Certificate's Private Key
        /// </summary>
        private string priKeyPassword;

        /// <summary>
        /// Instance of IThirdPartyAuthorization
        /// </summary>
        private IThirdPartyAuthorization thrdPartyAuthorization;

        /// <summary>
        /// CertificateCredential constructor
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="appId"></param>
        /// <param name="certFile"></param>
        /// <param name="priKeyPassword"></param>
        public CertificateCredential(string usrName, string pasWord, string appId, 
            string certFile, string priKeyPassword) : base()
        {
            if (string.IsNullOrEmpty(usrName) || string.IsNullOrEmpty(pasWord) ||
                string.IsNullOrEmpty(certFile) || string.IsNullOrEmpty(priKeyPassword))
            {
                throw new ArgumentException("Certificate Credential arguments cannot be null");
            }
            this.usrName = usrName;
            this.pasWord = pasWord;
            this.appId = appId;
            this.certFile = certFile;
            this.priKeyPassword = priKeyPassword;
        }                    

        /// <summary>
        /// CertificateCredential constructor overload
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="pasWord"></param>
        /// <param name="appId"></param>
        /// <param name="certFile"></param>
        /// <param name="priKeyPassword"></param>
        /// <param name="thrdPartyAuthorize"></param>
        public CertificateCredential(string usrName, string pasWord,
                string appId, string certFile,
                string priKeyPassword, IThirdPartyAuthorization thrdPartyAuthorization)
            : this(usrName, pasWord, appId, certFile, priKeyPassword)
        {
           this.thrdPartyAuthorization = thrdPartyAuthorization;
        }
        
      
        
        /// <summary>
        ///  Gets and sets the instance of IThirdPartyAuthorization
        /// </summary>
        public IThirdPartyAuthorization ThirdPartyAuthorization
        {
            get
            {
                return thrdPartyAuthorization;
            }
            set
            {
                value = thrdPartyAuthorization;
            }
        }

        /// <summary>
        /// Gets the Username credential
        /// </summary>
        public string UserName
        {
            get
            {
                return usrName;
            }
        }
       
        /// <summary>
        /// Gets the Password credential
        /// </summary>
        public string Password
        {
            get
            {
                return pasWord;
            }
        }

        /// <summary>
        /// Gets and sets the Application Id (Used by Platform APIs)
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return appId;
            }
            set
            {
                appId = value;
            }
        }

        /// <summary>
        /// Gets the File Name of the Certificate
        /// </summary>
        public string CertificateFile
        {
            get
            {
                return this.certFile;
            }
        }

        /// <summary>
        /// Gets the Password of the Certificate's Private Key
        /// </summary>
        public string PrivateKeyPassword
        {
            get
            {
                return this.priKeyPassword;
            }
        }
    }
}

