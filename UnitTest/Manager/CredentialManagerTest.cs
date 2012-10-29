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
        [Test]
        public void LoadSignatureCredential()
        {
            string apiUsername = "jb-us-seller_api1.paypal.com";
            CredentialManager mgr = CredentialManager.Instance;
            ICredential cred = mgr.GetCredentials(apiUsername);
            Assert.NotNull(cred);
            Assert.IsInstanceOf(typeof(SignatureCredential), cred);

            SignatureCredential sig = (SignatureCredential) cred;
            Assert.AreEqual(apiUsername, sig.UserName);
            Assert.AreEqual("WX4WTU3S8MY44S7F", sig.Password);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", sig.Signature);
            Assert.AreEqual("APP-80W284485P519543T", sig.ApplicationID);            
        }

        [Test]
        public void LoadCertificateCredential()
        {
            string apiUsername = "certuser_biz_api1.paypal.com";
            CredentialManager mgr = CredentialManager.Instance;
            ICredential cred = mgr.GetCredentials(apiUsername);
            Assert.NotNull(cred);
            Assert.IsInstanceOf(typeof(CertificateCredential), cred);

            CertificateCredential cert = (CertificateCredential)cred;
            Assert.AreEqual(apiUsername, cert.UserName);
            Assert.AreEqual("D6JNKKULHN3G5B8A", cert.Password);
            Assert.AreEqual(UnitTestConstants.CERT_PATH, cert.CertificateFile);
            Assert.AreEqual(UnitTestConstants.CERT_PASSWORD, cert.PrivateKeyPassword);
            Assert.AreEqual("APP-80W284485P519543T", cert.ApplicationID);
        }

        [Test, ExpectedException( typeof(MissingCredentialException) )]
        public void LoadCredentialForNonExistentAccount()
        {
            CredentialManager mgr = CredentialManager.Instance;
            ICredential cred = mgr.GetCredentials("i-do-not-exist_api1.paypal.com");
        }
    }
}
