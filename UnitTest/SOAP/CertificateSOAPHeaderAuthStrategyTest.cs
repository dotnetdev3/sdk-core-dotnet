using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class CertificateSOAPHeaderAuthStrategyTest
    {
        [Test]
        public void GenerateHeaderStrategy()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            string payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
        }

        [Test]
        public void GenerateHeaderStrategyToken()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = tokenAuthorization;
            string payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }
        
        [Test]
        public void GenerateHeaderStrategyThirdPartyAuthorization()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "sdk-cert.p12", "KJAERUGBLVF6Y");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            certCredential.ThirdPartyAuthorization = subjectAuthorization;
            string payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSubject = xmlDoc.GetElementsByTagName("Subject");
            Assert.IsTrue(xmlNodeListSubject.Count > 0);
            Assert.AreEqual("testsubject", xmlNodeListSubject[0].InnerXml);
        }      
        
        [Test]
        public void ThirdPartyAuthorization()
        {
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            Assert.IsNotNull(certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
        }
      
        private XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlString = xmlString.Replace("ns:", string.Empty);
            xmlString = xmlString.Replace("ebl:", string.Empty);
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }
    }
}
