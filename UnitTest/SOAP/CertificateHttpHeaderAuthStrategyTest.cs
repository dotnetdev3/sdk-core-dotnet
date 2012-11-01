using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.SOAP;
using PayPal.Authentication;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class CertificateHttpHeaderAuthStrategyTest
    {
        CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        CertificateCredential certCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");                                 
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string authHeader = header["X-PP-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.ACCESS_TOKEN, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy("https://svcs.sandbox.paypal.com/");
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);            
            string username = header["X-PAYPAL-SECURITY-USERID"];
            string psw = header["X-PAYPAL-SECURITY-PASSWORD"];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", psw);
        }
    }
}
