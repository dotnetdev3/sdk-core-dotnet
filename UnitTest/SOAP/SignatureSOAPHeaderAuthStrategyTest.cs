using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class SignatureSOAPHeaderAuthStrategyTest
    {
        [Test]
        public void GenerateHeaderStrategyTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            string payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);

            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("testusername", xmlNodeListUsername[0].InnerXml);

            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);

            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual("testsignature", xmlNodeListSignature[0].InnerXml);
        }

        [Test]
        public void GenerateHeaderStrategyTokenTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization = tokenAuthorization;
            signatureCredential.ThirdPartyAuthorization =tokenAuthorization;
            string payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }               

        [Test]
        public void GenerateHeaderStrategyThirdPartyTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            signatureCredential.ThirdPartyAuthorization = subjectAuthorization;
            string payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);
            
            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList NodeListUsername = xmlDoc.GetElementsByTagName("Username");            
            Assert.IsTrue(NodeListUsername.Count > 0);
            Assert.AreEqual("testusername", NodeListUsername[0].InnerXml);

            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("testpassword", xmlNodeListPassword[0].InnerXml);

            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual("testsignature", xmlNodeListSignature[0].InnerXml);

            XmlNodeList xmlNodeListSubject = xmlDoc.GetElementsByTagName("Subject");
            Assert.IsTrue(xmlNodeListSubject.Count > 0);
            Assert.AreEqual("testsubject", xmlNodeListSubject[0].InnerXml);
        }       

        [Test]
        public void ThirdPartyAuthorizationTest()
        {
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            Assert.IsNotNull(signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
            Assert.AreEqual("testsubject", ((PayPal.Authentication.SubjectAuthorization)(signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization)).Subject);
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
