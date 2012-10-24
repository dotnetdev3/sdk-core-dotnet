using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class SignatureSOAPHeaderAuthStrategyTest
    {
        [Test]
        public void generateHeaderStrategyForTokenTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            TokenAuthorization tokenAuthorization = new TokenAuthorization("accessToken", "tokenSecret");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization =tokenAuthorization;
            signatureCredential.ThirdPartyAuthorization =tokenAuthorization;
            String payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);
            Assert.AreEqual("<ns:RequesterCredentials/>", payload);
        }

        [Test]
        public void generateHeaderStrategyForSubjectTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            signatureCredential.ThirdPartyAuthorization = subjectAuthorization;
            String payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);


            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList credential = docEle.getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");
            NodeList sign = ((Element)credential.item(0)).getElementsByTagName("ebl:Signature");
            NodeList subject = ((Element)credential.item(0)).getElementsByTagName("ebl:Subject");

            String username = user.item(0).getTextContent();
            String password = psw.item(0).getTextContent();
            String signature = sign.item(0).getTextContent();
            String sub = subject.item(0).getTextContent();

            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", signature);
            Assert.AreEqual("testsubject", sub);
        }

        [Test]
        public void generateHeaderStrategyForNonThirdPartyTest()
        {
            SignatureCredential signatureCredential = new SignatureCredential("testusername", "testpassword", "testsignature");
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            String payload = signatureSOAPHeaderAuthStrategy.GenerateHeaderStrategy(signatureCredential);

            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList credential = docEle.getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");
            NodeList sign = ((Element)credential.item(0)).getElementsByTagName("ebl:Signature");
            NodeList subject = ((Element)credential.item(0)).getElementsByTagName("ebl:Subject");

            String username = user.item(0).getTextContent();
            String password = psw.item(0).getTextContent();
            String signature = sign.item(0).getTextContent();
            Object sub = subject.item(0);

            Assert.AreEqual("testusername", username);
            Assert.AreEqual("testpassword", password);
            Assert.AreEqual("testsignature", signature);
            Assert.IsNull(sub);
        }

        [Test]
        public void setGetThirdPartyAuthorization()
        {
            SignatureSOAPHeaderAuthStrategy signatureSOAPHeaderAuthStrategy = new SignatureSOAPHeaderAuthStrategy();
            SubjectAuthorization subjectAuthorization = new SubjectAuthorization("testsubject");
            signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization = subjectAuthorization;
            Assert.IsNotNull(signatureSOAPHeaderAuthStrategy.ThirdPartyAuthorization);
        }

        //TODO:
        // private XmlDocument loadXMLFromString(String xml)
        private Document loadXMLFromString(String xml)
        {
            ByteArrayInputStream stream = new ByteArrayInputStream(xml.getBytes());
            DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
            return builder.parse(stream);
        }
    }
}
