using System;
using NUnit.Framework;
using PayPal.Authentication;

namespace PayPal.UnitTest.Authentication
{
    [TestFixture]
    class CertificateCredentialTest
    {
        CertificateCredential certCredential;

        public CertificateCredentialTest()
        {
            certCredential = new CertificateCredential(
                    "platfo_1255077030_biz_api1.gmail.com", "1255077037",
                    "sdk-cert.p12", "KJAERUGBLVF6Y");          
        }

        [Test]
        public void UserName()
        {
            Assert.AreEqual("platfo_1255077030_biz_api1.gmail.com", certCredential.UserName);
        }


        [Test]
        public void Password()
        {
            Assert.AreEqual("1255077037", certCredential.Password);
        }

        [Test]
        public void CertificateFile()
        {
            Assert.AreEqual("sdk-cert.p12", certCredential.CertificateFile);
        }

        [Test]
        public void PrivateKeyPassword()
        {
            Assert.AreEqual("KJAERUGBLVF6Y", certCredential.PrivateKeyPassword);
        }
        
        [Test]
        public void ApplicationID()
        {
            certCredential.ApplicationID ="APP-80W284485P519543T" ;
            Assert.AreEqual("APP-80W284485P519543T", certCredential.ApplicationID);
        }               

        [Test]
        public void setAndGetThirdPartyAuthorizationForSubject()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new SubjectAuthorization("Subject");
            certCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((SubjectAuthorization)thirdPartyAuthorization).Subject,"Subject");

        }

        [Test]
        public void ThirdPartyAuthorizationTestForToken()
        {
            IThirdPartyAuthorization thirdPartyAuthorization = new TokenAuthorization(UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            certCredential.ThirdPartyAuthorization = thirdPartyAuthorization;
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).AccessToken,
                    UnitTestConstants.ACCESS_TOKEN);
            Assert.AreEqual(((TokenAuthorization)thirdPartyAuthorization).TokenSecret,
                    UnitTestConstants.TOKEN_SECRET);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void CertificateCredentialArgumentException()
        {
            certCredential = new CertificateCredential(null, null, null, null);
        }
    }
}
