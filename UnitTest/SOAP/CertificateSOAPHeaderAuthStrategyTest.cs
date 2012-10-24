using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.SOAP;
using System.Xml;
using System.IO;
using System.Text;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class CertificateSOAPHeaderAuthStrategyTest
    {
        [Test]
        public void GenerateHeaderStrategyForTokenTest()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "certkey", "certpath");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = tokenAuthorization;
            certCredential.ThirdPartyAuthorization = tokenAuthorization;
            String payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }

        [Test]
        public void GenerateHeaderStrategyForSubjectTest()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "certkey", "certpath");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            certCredential.ThirdPartyAuthorization = subjectAuthorization;
            String payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);
            
            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList credential = docEle.getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");
            NodeList sign = ((Element)credential.item(0)).getElementsByTagName("ebl:Signature");
            NodeList subject = ((Element)credential.item(0)).getElementsByTagName("ebl:Subject");

            String username = user.item(0).getTextContent();
            String password = psw.item(0).getTextContent();
            Object signature = sign.item(0);
            String sub = subject.item(0).getTextContent();

            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.IsNull(signature);
            Assert.AreEqual("testsubject", sub);
        }

        [Test]
        public void GenerateHeaderStrategyForNonThirdPartyTest()
        {
            CertificateCredential certCredential = new CertificateCredential("testusername", "testpassword", "certkey", "certpath");
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            String payload = certificateSOAPHeaderAuthStrategy.GenerateHeaderStrategy(certCredential);

            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList credential = docEle.getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");
            NodeList sign = ((Element)credential.item(0)).getElementsByTagName("ebl:Signature");
            NodeList subject = ((Element)credential.item(0)).getElementsByTagName("ebl:Subject");

            String username = user.item(0).getTextContent();
            String password = psw.item(0).getTextContent();
            Object signature = sign.item(0);
            Object sub = subject.item(0);

            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.IsNull(signature);
            Assert.IsNull(sub);
        }
        
        [Test]
        public void setGetThirdPartyAuthorization()
        {
            CertificateSOAPHeaderAuthStrategy certificateSOAPHeaderAuthStrategy = new CertificateSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            Assert.IsNotNull(certificateSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
        }
                
        //TODO:
        // private XmlDocument loadXMLFromString(String xml)
        private Document loadXMLFromString(String xml)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = Encoding.GetBytes(xml);

            ByteArrayInputStream stream = new ByteArrayInputStream(xml.getBytes());
            DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
            return builder.parse(stream);
        }
    }
}
