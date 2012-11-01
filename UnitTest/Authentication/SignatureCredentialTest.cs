using System;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal.UnitTest.Authentication
{
    [TestFixture]
    class SignatureCredentialTest
    {
        SignatureCredential signCredential;

        public SignatureCredentialTest()
        {
            signCredential = new SignatureCredential("platfo_1255077030_biz_api1.gmail.com",
                    "1255077037",
                    "Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf");
        }

        [Test]
        public void Signature()
        {
            Assert.AreEqual("Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf",
                    ((SignatureCredential)signCredential).Signature);
        }

        [Test]
        public void Password()
        {
            Assert.AreEqual("1255077037", signCredential.Password);
        }

        [Test]
        public void UserName()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com",
                    signCredential.UserName);
        }

        [Test]
        public void ApplicationID()
        {
            signCredential.ApplicationID = "APP-80W284485P519543T";
            Assert.AreEqual("APP-80W284485P519543T", signCredential.ApplicationID);
        }

        [Test]
        public void ThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            signCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject, "Subject");
        }

        [Test]
        public void ThirdPartyAuthorizationForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            signCredential.ThirdPartyAuthorization = thirdPartyAuthorization;            
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken, UnitTestConstants.ACCESS_TOKEN);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).TokenSecret, UnitTestConstants.TOKEN_SECRET);

        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void SignatureCredentialArgumentException()
        {
            signCredential = new SignatureCredential(null, null, null);
        }
    }
}
