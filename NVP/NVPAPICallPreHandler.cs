using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Authentication;
using PayPal.Manager;
using PayPal.Exception;

namespace PayPal.NVP
{
    public class NVPAPICallPreHandler : IAPICallPreHandler
    {
        /// <summary>
        /// Service Name
        /// </summary>
	    private readonly string serviceName;

        /// <summary>
        /// API method
        /// </summary>
	    private readonly string method;

        /// <summary>
        /// Raw payload from stubs
        /// </summary>
		private readonly string rawPayLoad;

	    /// <summary>
	    /// API Username for authentication
	    /// </summary>
	    private string apiUserName;

	    /// <summary>
	    /// {@link ICredential} for authentication
	    /// </summary>
	    private ICredential credential;
        
        /// <summary>
        /// Access token if any for authorization
        /// </summary>
		private string accessToken;
        
        /// <summary>
        /// TokenSecret if any for authorization
        /// </summary>
        private string tokenSecret;

	    /// <summary>
	    /// SDK Name used in tracking
	    /// </summary>
        private string sdkName;

        /// <summary>
        /// SDK Version
        /// </summary>
	    private string sdkVersion;
        
        /// <summary>
        /// Internal variable to hold headers
        /// </summary>
	    private Dictionary<string, string> headers;

	    /// <summary>
	    /// Private Constructor
	    /// </summary>
	    /// <param name="rawPayLoad"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
	    private NVPAPICallPreHandler(string rawPayLoad, string serviceName,	string method) : base()
        {
            this.rawPayLoad = rawPayLoad;
		    this.serviceName = serviceName;
		    this.method = method;
	    }

        //throws InvalidCredentialException, MissingCredentialException
	    /**
	     * NVPAPICallPreHandler
	     * 
	     * @param serviceName
	     *            Service Name
	     * @param rawPayLoad
	     *            Payload
	     * @param method
	     *            API method
	     * @param apiUserName
	     *            API Username
	     * @param accessToken
	     *            Access Token
	     * @param tokenSecret
	     *            Token Secret
	     * @throws MissingCredentialException
	     * @throws InvalidCredentialException
	     */
	    public NVPAPICallPreHandler(string rawPayLoad, string serviceName, string method, string apiUserName, string accessToken, string tokenSecret)  : this(rawPayLoad, serviceName, method)
        {
            this.apiUserName = apiUserName;
		    this.accessToken = accessToken;
		    this.tokenSecret = tokenSecret;
		    initCredential();
	    }

	    /// <summary>
        /// NVPAPICallPreHandler
	    /// </summary>
	    /// <param name="rawPayLoad"></param>
	    /// <param name="serviceName"></param>
	    /// <param name="method"></param>
	    /// <param name="credential"></param>
	    public NVPAPICallPreHandler(string rawPayLoad, string serviceName,string method, ICredential credential) : this(rawPayLoad, serviceName, method)
        {  		
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in NVPAPICallPreHandler");
		    }
		    this.credential = credential;
	    }

	    /**
	     * @return the sdkName
	     */
	    public string getSdkName() 
        {
		    return sdkName;
	    }

	    /**
	     * @param sdkName
	     *            the sdkName to set
	     */
	    public void setSdkName(string sdkName) 
        {
		    this.sdkName = sdkName;
	    }

	    /**
	     * @return the sdkVersion
	     */
	    public string getSdkVersion()
        {
		    return sdkVersion;
	    }

	    /**
	     * @param sdkVersion
	     *            the sdkVersion to set
	     */
	    public void setSdkVersion(string sdkVersion) 
        {
		    this.sdkVersion = sdkVersion;
	    }

        public Dictionary<string, string> GetHeaderMap()
        {
            try
            {
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                    if (credential is SignatureCredential)
                    {
                        SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(GetEndPoint());
                        headers = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy((SignatureCredential)credential);
                    }
                    else if (credential is CertificateCredential)
                    {
                        CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(GetEndPoint());
                        headers = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy((CertificateCredential)credential);
                    }
                    foreach (KeyValuePair<string, string> pair in getDefaultHttpHeadersNVP())
                    {
                        headers.Add(pair.Key, pair.Value);
                    }
                }
            }
            catch (OAuthException)
            {
                throw;
            }
            return headers;
        }

	    public string GetPayLoad() 
        {
		    // No processing necessary for NVP return the raw payload
		    return rawPayLoad;
	    }

	    public string GetEndPoint() 
        {
		    return ConfigManager.Instance.GetProperty(BaseConstants.END_POINT) + serviceName + "/" + method;
	    }

	    public ICredential GetCredential() 
        {
		    return credential;
	    }

        //throws InvalidCredentialException, MissingCredentialException
	    private ICredential GetCredentials()  
        {
		    ICredential returnCredential = null;
		    CredentialManager credentialManager = CredentialManager.Instance;
		    returnCredential = credentialManager.GetCredentials(apiUserName);

		    if (!string.IsNullOrEmpty(accessToken)) 
            {
			    IThirdPartyAuthorization tokenAuth = new TokenAuthorization(accessToken, tokenSecret);
    			
                if (returnCredential is SignatureCredential) 
                {
				    SignatureCredential sigCred = (SignatureCredential) returnCredential;
				    sigCred.ThirdPartyAuthorization = tokenAuth;
			    } 
                else if (returnCredential is CertificateCredential) 
                {
				    CertificateCredential certCred = (CertificateCredential) returnCredential;
				    certCred.ThirdPartyAuthorization = tokenAuth;
			    }
		    }
		    return returnCredential;
	    }

	    private Dictionary<string, string> getDefaultHttpHeadersNVP() 
        {
		    Dictionary<string, string> returnMap = new Dictionary<string, string>();
            returnMap.Add(BaseConstants.PAYPAL_APPLICATION_ID, getApplicationId());
		    returnMap.Add(BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER, BaseConstants.NVP);
            returnMap.Add(BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER, BaseConstants.NVP);
            returnMap.Add(BaseConstants.PAYPAL_REQUEST_SOURCE_HEADER, sdkName + "-" + sdkVersion);
		    return returnMap;
	    }

	    private string getApplicationId() 
        {
		    string applicationId = null;
		    if (credential is CertificateCredential) 
            {
			    applicationId = ((CertificateCredential) credential).ApplicationId;
		    } 
            else if (credential is SignatureCredential) 
            {
			    applicationId = ((SignatureCredential) credential).ApplicationId;
		    }
		    return applicationId;
	    }

        //throws InvalidCredentialException, MissingCredentialException 
	    private void initCredential() 
        {
		    if (credential == null) 
            {
			    credential = GetCredentials();
		    }
	    }
    }
}
