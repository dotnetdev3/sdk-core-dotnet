using System;
using NUnit.Framework;
using PayPal.Exception;
using PayPal.Manager;
using PayPal.Authentication;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class CredentialManagerTest
    {
        CredentialManager credentialMgr;
        ICredential credential;

        [Test]
        public void LoadSignatureCredential()
        {
            string apiUsername = "jb-us-seller_api1.paypal.com";
            credentialMgr = CredentialManager.Instance;
            credential = credentialMgr.GetCredentials(apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(SignatureCredential), credential);
            SignatureCredential sig = (SignatureCredential) credential;
            Assert.AreEqual(apiUsername, sig.UserName);
            Assert.AreEqual("WX4WTU3S8MY44S7F", sig.Password);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", sig.Signature);
            Assert.AreEqual("APP-80W284485P519543T", sig.ApplicationID);            
        }

        [Test]
        public void LoadCertificateCredential()
        {
            string apiUsername = "certuser_biz_api1.paypal.com";
            credentialMgr = CredentialManager.Instance;
            credential = credentialMgr.GetCredentials(apiUsername);
            Assert.NotNull(credential);
            Assert.IsInstanceOf(typeof(CertificateCredential), credential);
            CertificateCredential cert = (CertificateCredential)credential;
            Assert.AreEqual(apiUsername, cert.UserName);
            Assert.AreEqual("D6JNKKULHN3G5B8A", cert.Password);
            Assert.AreEqual(UnitTestConstants.CERT_PATH, cert.CertificateFile);
            Assert.AreEqual(UnitTestConstants.CERT_PASSWORD, cert.PrivateKeyPassword);
            Assert.AreEqual("APP-80W284485P519543T", cert.ApplicationID);
        }

        [Test, ExpectedException( typeof(MissingCredentialException) )]
        public void LoadCredentialForNonExistentAccount()
        {
            credentialMgr = CredentialManager.Instance;
            credential = credentialMgr.GetCredentials("i-do-not-exist_api1.paypal.com");
        }
    }
}
