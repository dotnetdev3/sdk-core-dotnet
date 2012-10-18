using System.Text;

namespace PayPal
{
    public static class BaseConstants
    {
        // Request Method in HTTP Connection
        public const string REQUESTMETHOD = "POST";

        // Log file
        public const string PAYPALLOGFILE = "PAYPALLOGFILE";

        // Default connection timeout in milliseconds
        public const int DEFAULT_TIMEOUT = 3600000;

        // Encoding Format
        public static readonly Encoding ENCODING_FORMAT = Encoding.UTF8;
        
        // Account Prefix
        public const string ACCCOUT_PREFIX = "acct";

        // Sandbox Default Email Address
        public const string PayPalSandboxEmailAddressDefault = "Platform.sdk.seller@gmail.com";
        
        // SOAP Format
        public const string SOAP = "SOAP";
        
        // NVP Format
        public const string NVP = "NV";
        
        // HTTP Header Constants
        // PayPal Security UserId Header
        public const string PAYPAL_SECURITY_USERID_HEADER = "X-PAYPAL-SECURITY-USERID";

        // PayPal Security Password Header
        public const string PAYPAL_SECURITY_PASSWORD_HEADER = "X-PAYPAL-SECURITY-PASSWORD";

        // PayPal Security Signature Header
        public const string PAYPAL_SECURITY_SIGNATURE_HEADER = "X-PAYPAL-SECURITY-SIGNATURE";

        // PayPal Platform Authorization Header
        public const string PAYPAL_AUTHORIZATION_PLATFORM = "X-PAYPAL-AUTHORIZATION";

        // PayPal Merchant Authorization Header
        public const string PAYPAL_AUTHORIZATION_MERCHANT = "X-PP-AUTHORIZATION";

        // PayPal Application ID Header
        public const string PAYPAL_APPLICATION_ID = "X-PAYPAL-APPLICATION-ID";

        // PayPal Request Data Header
        public const string PAYPAL_REQUEST_DATA_FORMAT_HEADER = "X-PAYPAL-REQUEST-DATA-FORMAT";

        // PayPal Request Data Header
        public const string PAYPAL_RESPONSE_DATA_FORMAT_HEADER = "X-PAYPAL-RESPONSE-DATA-FORMAT";

        // PayPal Request Source Header
        public const string PAYPAL_REQUEST_SOURCE_HEADER = "X-PAYPAL-REQUEST-SOURCE";
        
        // PayPal Sandbox Email Address Header
        public const string PAYPAL_SANDBOX_DEVICE_IPADDRESS = "X-PAYPAL-DEVICE-IPADDRESS";

        // PayPal Sandbox Email Address Header
        public const string PAYPAL_SANDBOX_EMAIL_ADDRESS_HEADER = "X-PAYPAL-SANDBOX-EMAIL-ADDRESS";

        // Constants key defined for configuration options in application properties
        // End point
        public const string END_POINT = "endpoint";

        // Constants key defined for configuration options in application properties
        // IPAddress
        public const string PayPalIPAddress = "IPAddress";
       
        // Constants key defined for configuration options in application properties
        // Email Address
        public const string PayPalSandboxEmailAddress = "sandboxEmailAddress";

        // HTTP Proxy Address
        public const string HTTP_PROXY_ADDRESS = "proxyAddress";

        // HTTP Proxy Credential
        public const string HTTP_PROXY_CREDENTIAL = "proxyCredentials";

        // HTTP Connection Timeout
        public const string HTTP_CONNECTION_TIMEOUT = "connectionTimeout";

        // HTTP Connection Retry
        public const string HTTP_CONNECTION_RETRY = "requestRetries";

        // Credential Username suffix
        public const string CREDENTIAL_USERNAME = "apiUsername";

        // Credential Password suffix
        public const string CREDENTIAL_PASSWORD = "apiPassword";

        // Credential Application ID
        public const string CREDENTIAL_APPLICACTIONID = "applicationId";

        // Credential Subject
        public const string CREDENTIAL_SUBJECT = ".Subject";

        // Credential Signature
        public const string CREDENTIAL_SIGNATURE = "apiSignature";

        // Credential Certificate Path
        public const string CREDENTIAL_CERTPATH_SUFFIX = "apiCertificate";

        // Credential Certificate Key
        public const string CREDENTIAL_CERTKEY_SUFFIX = "privateKeyPassword";

        public static class ErrorMessages
        {
            public const string PROFILE_NULL = "APIProfile cannot be null.";
            public const string PAYLOAD_NULL = "PayLoad cannot be null or empty.";
            public const string err_endpoint = "Endpoint cannot be empty.";
            public const string err_username = "API username cannot be empty";
            public const string err_passeword = "API password cannot be empty.";
            public const string err_signature = "API signature cannot be empty";
            public const string err_appid = "Application Id cannot be empty";
            public const string err_certificate = "Certificate cannot be empty";
            public const string err_privatekeypassword = "Private Key password cannot be null or empty.";
        }
    }
}
