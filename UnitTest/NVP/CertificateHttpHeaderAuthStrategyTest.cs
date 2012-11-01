using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.NVP;
using PayPal.Authentication;
using PayPal.Manager;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class CertificateHttpHeaderAuthStrategyTest
    {
        CertificateHttpHeaderAuthStrategy certHttpHeaderAuthStrategy;
        CertificateCredential certCredential;

        [Test]
        public void GenerateHeaderStrategyWithTokenTest()
        {
            certHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y", toknAuthorization);
            Dictionary<string, string> header = certHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);            
            string authHeader = header[BaseConstants.PAYPAL_AUTHORIZATION_PLATFORM];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.ACCESS_TOKEN, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutTokenTest()
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
