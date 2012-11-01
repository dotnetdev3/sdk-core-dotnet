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
        SignatureHttpHeaderAuthStrategy signHttpHeaderAuthStrategy;
        SignatureCredential signCredential;

        [Test]
        public void GenerateHeaderStrategyWithToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            TokenAuthorization toknAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature", toknAuthorization);
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);
            string authHeader = header["X-PP-AUTHORIZATION"];
            string[] headers = authHeader.Split(',');
            Assert.AreEqual("token=" + UnitTestConstants.ACCESS_TOKEN, headers[0]);
        }

        [Test]
        public void GenerateHeaderStrategyWithoutToken()
        {
            signHttpHeaderAuthStrategy = new SignatureHttpHeaderAuthStrategy("https://api-3t.sandbox.paypal.com/2.0");
            signCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            Dictionary<string, string> header = signHttpHeaderAuthStrategy.GenerateHeaderStrategy(signCredential);            
            string username = header["X-PAYPAL-SECURITY-USERID"];
            string psw = header["X-PAYPAL-SECURITY-PASSWORD"];
            string sign = header["X-PAYPAL-SECURITY-SIGNATURE"];
            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", psw);
            Assert.AreEqual("testsignature", sign);
        }
    }
}
