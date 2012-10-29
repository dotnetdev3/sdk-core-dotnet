using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.NVP;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class PlatformAPICallPreHandlerTest
    {
        [Test]
        public void GetHeaderMapWithSignatureWithTokenTest()
        {
            PlatformAPICallPreHandler platformApiCaller = new PlatformAPICallPreHandler("payload", "servicename", "method", "jb-us-seller_api1.paypal.com", "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformApiCaller.GetHeaderMap();
            string authHeader = header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapSignatureWithoutTokenTest()
        {
            CredentialManager credentialmgr = CredentialManager.Instance;
            ICredential signatureCredential = credentialmgr.GetCredentials("jb-us-seller_api1.paypal.com");
            PlatformAPICallPreHandler platformApiCaller = new PlatformAPICallPreHandler("payload", "servicename", "method", signatureCredential);            
            Dictionary<string, string> header = platformApiCaller.GetHeaderMap();
            Assert.AreEqual("jb-us-seller_api1.paypal.com", header["X-PAYPAL-SECURITY-USERID"]);
            Assert.AreEqual("WX4WTU3S8MY44S7F", header["X-PAYPAL-SECURITY-PASSWORD"]);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", header["X-PAYPAL-SECURITY-SIGNATURE"]);
            Assert.AreEqual("APP-80W284485P519543T", header["X-PAYPAL-APPLICATION-ID"]);
            Assert.AreEqual("NV", header["X-PAYPAL-REQUEST-DATA-FORMAT"]);
            Assert.AreEqual("NV", header["X-PAYPAL-RESPONSE-DATA-FORMAT"]);
        }

        [Test]
        public void GetHeaderMapWithCertificateWithTokenTest()
        {
            PlatformAPICallPreHandler platformApiCaller = new PlatformAPICallPreHandler("payload", "servicename", "method", "certuser_biz_api1.paypal.com", "accessToken", "tokenSecret");
            Dictionary<string, string> header = platformApiCaller.GetHeaderMap();            
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GetHeaderMapCertificateWithoutTokenTest()
        {
            CredentialManager credentialmgr = CredentialManager.Instance;
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            PlatformAPICallPreHandler platformApiCaller = new PlatformAPICallPreHandler("payload", "servicename", "method", certificateCredential);
            Dictionary<string, string> header = platformApiCaller.GetHeaderMap();
            Assert.AreEqual("certuser_biz_api1.paypal.com", header["X-PAYPAL-SECURITY-USERID"]);
            Assert.AreEqual("D6JNKKULHN3G5B8A", header["X-PAYPAL-SECURITY-PASSWORD"]);
            Assert.AreEqual("APP-80W284485P519543T", header["X-PAYPAL-APPLICATION-ID"]);
            Assert.AreEqual(BaseConstants.NVP, header["X-PAYPAL-REQUEST-DATA-FORMAT"]);
            Assert.AreEqual(BaseConstants.NVP, header["X-PAYPAL-RESPONSE-DATA-FORMAT"]);
        }  
        
        
        [Test]
        public void GetPayloadEndpointWithoutTokenTest()
        {
            PlatformAPICallPreHandler platformApiCaller = new PlatformAPICallPreHandler("payload", "servicename", "method", "jb-us-seller_api1.paypal.com", "accessToken", "tokenSecret");
            Assert.AreEqual("https://svcs.sandbox.paypal.com/servicename/method", platformApiCaller.GetEndPoint());
            Assert.AreEqual("payload", platformApiCaller.GetPayLoad());
            SignatureCredential signatureCredential = (SignatureCredential)platformApiCaller.GetCredential();
            TokenAuthorization thirdAuth = (TokenAuthorization)signatureCredential.ThirdPartyAuthorization;
            Assert.AreEqual("accessToken", thirdAuth.AccessToken);
            Assert.AreEqual("tokenSecret", thirdAuth.TokenSecret);
        }
    }
}
