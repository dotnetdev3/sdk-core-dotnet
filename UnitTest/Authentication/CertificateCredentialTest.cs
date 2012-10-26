using System;
using NUnit.Framework;
using PayPal.Authentication;

namespace PayPal.UnitTest.Authentication
{
    [TestFixture]
    class CertificateCredentialTest
    {
        CertificateCredential cred;

        public CertificateCredentialTest()
        {
            cred = new CertificateCredential(
                    "platfo_1255077030_biz_api1.gmail.com", "1255077037",
                    "sdk-cert.p12", "KJAERUGBLVF6Y");          
        }

        [Test]
        public void UserNameTest()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com", cred.UserName);
        }


        [Test]
        public void PasswordTest()
        {
            Assert.AreEqual("1255077037", cred.Password);
        }

        [Test]
        public void CertificateFileTest()
        {
            Assert.AreEqual("sdk-cert.p12", cred.CertificateFile);
        }

        [Test]
        public void PrivateKeyPasswordTest()
        {
            Assert.AreEqual("KJAERUGBLVF6Y", cred.PrivateKeyPassword);
        }
        
        [Test]
        public void ApplicationIDTest()
        {
            cred.ApplicationID ="APP-80W284485P519543T" ;
            Assert.AreEqual("APP-80W284485P519543T", cred.ApplicationID);
        }               

        [Test]
        public void setAndGetThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            cred.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject,"Subject");

        }

        [Test]
        public void ThirdPartyAuthorizationTestForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            cred.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken,
                    UnitTestConstants.ACCESS_TOKEN);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).TokenSecret,
                    UnitTestConstants.TOKEN_SECRET);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void CertificateCredentialArgumentExceptionTest()
        {
            cred = new CertificateCredential(null, null, null, null);
        }
    }
}
