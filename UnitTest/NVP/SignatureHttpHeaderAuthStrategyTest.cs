using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.NVP;
using PayPal.Authentication;

namespace PayPal.UnitTest.NVP
{
    [TestFixture]
    class SignatureHttpHeaderAuthStrategyTest
    {
        [Test]
        public void ProcessTokenAuthorizationTest()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://svcs.sandbox.paypal.com/");
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");           
            Dictionary<string, string> header = signatureHttpHeaderAuthStrategy.ProcessTokenAuthorization(signatureCredential, tokenAuthorization);
            string authHeader = (string)header["X-PAYPAL-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=accessToken", headers[0]);
        }  

        [Test]
        public void GenerateHeaderStrategyWithoutTokenTest()
        {
            SignatureHttpHeaderAuthStrategy signatureHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://svcs.sandbox.paypal.com/");
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
