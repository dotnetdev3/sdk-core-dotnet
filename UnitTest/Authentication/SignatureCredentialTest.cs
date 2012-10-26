using System;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.Exception;

namespace PayPal.UnitTest.Authentication
{
    [TestFixture]
    class SignatureCredentialTest
    {
        SignatureCredential cred;

        public SignatureCredentialTest()
        {
            cred = new SignatureCredential("platfo_1255077030_biz_api1.gmail.com",
                    "1255077037",
                    "Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf");
        }

        [Test]
        public void SignatureTest()
        {
            Assert.AreEqual("Abg0gYcQyxQvnf2HDJkKtA-p6pqhA1k-KTYE0Gcy1diujFio4io5Vqjf",
                    ((SignatureCredential)cred).Signature);
        }

        [Test]
        public void PasswordTest()
        {
            Assert.AreEqual("1255077037", cred.Password);
        }

        [Test]
        public void UserNameTest()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com",
                    cred.UserName);
        }

        [Test]
        public void ApplicationIDTest()
        {
            cred.ApplicationID = "APP-80W284485P519543T";
            Assert.AreEqual("APP-80W284485P519543T", cred.ApplicationID);
        }

        [Test]
        public void ThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            cred.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject, "Subject");
        }

        [Test]
        public void ThirdPartyAuthorizationForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            cred.ThirdPartyAuthorization = thirdPartyAuthorization;            
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken, UnitTestConstants.ACCESS_TOKEN);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).TokenSecret, UnitTestConstants.TOKEN_SECRET);

        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void SignatureCredentialArgumentExceptionTest()
        {
            cred = new SignatureCredential(null, null, null);
        }
    }
}
