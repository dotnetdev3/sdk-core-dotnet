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
        [Test]
        public void GenerateHeaderStrategyForTokenTest()
        {
            CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "certkey", "certpath");
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            certCredential.ThirdPartyAuthorization = tokenAuthorization;
            Dictionary<string, string> header = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

            string authHeader = header["X-PP-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutTokenTest()
        {
            CertificateHttpHeaderAuthStrategy certificateHttpHeaderAuthStrategy = new CertificateHttpHeaderAuthStrategy("https://svcs.sandbox.paypal.com/");
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "certkey", "certpath");
            Dictionary<string, string> header = certificateHttpHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            string username = header["X-PAYPAL-SECURITY-USERID"];
            string psw = header["X-PAYPAL-SECURITY-PASSWORD"];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", psw);
        }
    }
}
