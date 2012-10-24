using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.SOAP;
using PayPal.Authentication;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class SignatureHttpHeaderAuthStrategyTest
    {
        [Test]
        public void processTokenAuthorizationTest()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.ProcessTokenAuthorization(signatureCredential, tokenAuthorization);
            string authHeader = (string)header["X-PP-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');

            Assert.AreEqual("token=accessToken", headers[0]);
        }

        [Test]
        public void generateHeaderStrategyWithoutTokenTest()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);
            string username = header["X-PAYPAL-SECURITY-USERID"];
            string psw = header["X-PAYPAL-SECURITY-PASSWORD"];
            string sign = header["X-PAYPAL-SECURITY-SIGNATURE"];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", psw);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
