using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using PayPal.Authentication;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal
{
    public class SOAPAPICallPreHandler : IAPICallPreHandler
    {
	    /**
	     * Pattern for Message Formatting
	     */
	    //private const Pattern REGEX_PATTERN = Pattern.compile("(['])");

        //private const string REGEX_PATTERN = @"(['])";

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
	    /// {@link APICallPreHandler} instance
	    /// </summary>
	    private IAPICallPreHandler apiCallHandler;
        
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
	    /// Internal variable to hold payload
	    /// </summary>
	    private string payLoad;
        
        /// <summary>
        /// Private Constructor
        /// </summary>
        /// <param name="apiCallHandler"></param>
	    private SOAPAPICallPreHandler(IAPICallPreHandler apiCallHandler) : base()
        {
		    
		    this.apiCallHandler = apiCallHandler;
	    }

	    /**
	     * SOAPAPICallPreHandler decorating basic {@link APICallPreHandler} using
	     * API Username
	     * 
	     * @param apiCallHandler
	     *            Instance of {@link APICallPreHandler}
	     * @param apiUserName
	     *            API Username
	     * @param accessToken
	     *            Access Token
	     * @param tokenSecret
	     *            Token Secret
	     * @throws InvalidCredentialException
	     * @throws MissingCredentialException
	     */
        //throws InvalidCredentialException, MissingCredentialException 
	    public SOAPAPICallPreHandler(IAPICallPreHandler apiCallHandler, string apiUserName, string accessToken, string tokenSecret) : this(apiCallHandler)
		{		    
		    this.apiUserName = apiUserName;
		    this.accessToken = accessToken;
		    this.tokenSecret = tokenSecret;
		    initCredential();
	    }

	    /**
	     * SOAPAPICallPreHandler decorating basic {@link APICallPreHandler} using
	     * {@link ICredential}
	     * 
	     * @param apiCallHandler
	     *            Instance of {@link APICallPreHandler}
	     * @param credential
	     *            Instance of {@link ICredential}
	     */
	    public SOAPAPICallPreHandler(IAPICallPreHandler apiCallHandler, ICredential credential) : this(apiCallHandler)
        {	    
		    if (credential == null) 
            {
			    throw new ArgumentException("Credential is null in SOAPAPICallPreHandler");
		    }
		    this.credential = credential;
	    }
    	
	    
	    public string SdkName
        {
            get
            {
		        return sdkName;
	        }
	        set
            {
		        this.sdkName = value;
	        }
        }
        
        

	 
	    public string getSdkVersion
        {
            get

            {
		    return sdkVersion;
	        }

	       set
           {
		        this.sdkVersion = value;
	        }
        }

        //throws OAuthException
	    public Dictionary<string, string> GetHeaderMap()  
        {
		    if (headers == null) 
            {
			    headers = apiCallHandler.GetHeaderMap();
			    if (credential is SignatureCredential) 
                {
				    SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy(GetEndPoint());
				    headers = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy((SignatureCredential) credential);
			    } 
                else if (credential is CertificateCredential) 
                {
				    CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy(GetEndPoint());
				    headers = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy((CertificateCredential) credential);
			    }
			    //headers.putAll(getDefaultHttpHeadersSOAP());

                foreach(KeyValuePair<string, string> pair in getDefaultHttpHeadersSOAP())
                {
                    headers.Add(pair.Key, pair.Value);
                }
		    }
		    return headers;
	    }

	    public string GetPayLoad() 
        {
		    // This method appends SOAP Headers to payload
		    // if the credentials mandate soap headers
		    if (payLoad == null) 
            {
			    payLoad = apiCallHandler.GetPayLoad();
			    string header = null;
			    if (credential is SignatureCredential)
                {
				    SignatureCredential sigCredential = (SignatureCredential) credential;
				    SignatureSOAPHeaderAuthStrategy signatureSoapHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
				    signatureSoapHeaderAuthStrategy.ThirdPartyAuthorize = sigCredential.ThirdPartyAuthorization;
						    
				    header = signatureSoapHeaderAuthStrategy.GenerateHeaderStrategy(sigCredential);
			    } else if (credential is CertificateCredential) {
				    CertificateCredential certCredential = (CertificateCredential) credential;
				    CertificateSOAPHeaderAuthStrategy certificateSoapHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
				    certificateSoapHeaderAuthStrategy.ThirdPartyAuthorize = certCredential.ThirdPartyAuthorization;					
				    header = certificateSoapHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

			    }
			    payLoad = getPayLoadUsingSOAPHeader(payLoad, getNamespaces(),header);
		    }
		    return payLoad;
	    }

	    public string GetEndPoint() 
        {
		    return apiCallHandler.GetEndPoint();
	    }

	    public ICredential GetCredential() 
        {
		    return credential;
	    }

	    /*
	     * Returns a credential as configured in the application configuration
	     */
        // throws InvalidCredentialException, MissingCredentialException
	    private ICredential getCredentials() 
        {
		    ICredential returnCredential = null;
		    CredentialManager credentialManager = CredentialManager.Instance;
		    returnCredential = credentialManager.GetCredentials(apiUserName);

		    if (!string.IsNullOrEmpty(accessToken)) 
            {

			    // Set third party authorization to token
			    // if token is sent as part of request call
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

	    /*
	     * Returns default HTTP headers used in SOAP call
	     */
	    private Dictionary<string, string> getDefaultHttpHeadersSOAP() 
        {
		    Dictionary<string, string> returnMap = new Dictionary<string, string>();
		    returnMap.Add(BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER, "SOAP");
		    returnMap.Add(BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER, "SOAP");
		    returnMap.Add("X-PAYPAL-REQUEST-SOURCE", sdkName + "-" + sdkVersion);
		    return returnMap;
	    }

	    /*
	     * Initialize {@link ICredential}
	     */
        //throws InvalidCredentialException, MissingCredentialException
	    private void initCredential()  
        {
		    if (credential == null) 
            {
			    credential = getCredentials();
		    }
	    }

	    /*
	     * Gets Namespace specific to PayPal APIs
	     */
	    private string getNamespaces() 
        {
		    string namespaces = "xmlns:ns=\"urn:ebay:api:PayPalAPI\" xmlns:ebl=\"urn:ebay:apis:eBLBaseComponents\" xmlns:cc=\"urn:ebay:apis:CoreComponentTypes\" xmlns:ed=\"urn:ebay:apis:EnhancedDataTypes\"";
		    return namespaces;
	    }

	    /*
	     * Returns Payload after decoration
	     */
	    private string getPayLoadUsingSOAPHeader(string payLoad, string namespaces, string header) 
        {
		    string returnPayLoad = null;
		    string formattedPayLoad = payLoad;
		    returnPayLoad = string.Format(formattedPayLoad, new object[] {namespaces, header});
		    return returnPayLoad;
	    }

    }
}
