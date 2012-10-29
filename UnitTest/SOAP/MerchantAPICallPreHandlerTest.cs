using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.Authentication;
using PayPal.SOAP;

namespace PayPal.UnitTest.SOAP
{
    [TestFixture]
    class MerchantAPICallPreHandlerTest
    {
        private DefaultSOAPAPICallHandler defaultSoaphandler;
        private CredentialManager credentialmgr;

        public MerchantAPICallPreHandlerTest()
        {
            defaultSoaphandler = new DefaultSOAPAPICallHandler(
                    "<Request>test</Request>", null, null);
            credentialmgr = CredentialManager.Instance;
        }

        [Test]
        public void GetHeaderMapSignature()
        {
            ICredential signatureCredential = credentialmgr.GetCredentials("jb-us-seller_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, signatureCredential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual("jb-us-seller_api1.paypal.com", headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual("WX4WTU3S8MY44S7F", headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", headers[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }

        [Test]
        public void GetHeaderMapCertificate()
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual("certuser_biz_api1.paypal.com", headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual("D6JNKKULHN3G5B8A", headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
            Assert.AreEqual(soapHandler.SDKName + "-" + soapHandler.SDKVersion, headers[BaseConstants.PAYPAL_REQUEST_SOURCE_HEADER]);
        }

        [Test]
        public void GetPayLoadSignature()
        {
            ICredential signatureCredential = credentialmgr.GetCredentials("jb-us-seller_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, signatureCredential);
            string payload = soapHandler.GetPayLoad();

            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("jb-us-seller_api1.paypal.com", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("WX4WTU3S8MY44S7F", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListSignature = xmlDoc.GetElementsByTagName("Signature");
            Assert.IsTrue(xmlNodeListSignature.Count > 0);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", xmlNodeListSignature[0].InnerXml);
            XmlNodeList xmlNodeListRequest = xmlDoc.GetElementsByTagName("Request");
            Assert.IsTrue(xmlNodeListRequest.Count > 0);
            Assert.AreEqual("test", xmlNodeListRequest[0].InnerXml);          
        }

        [Test]
        public void GetPayLoadForCertificate()
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            string payload = soapHandler.GetPayLoad();

            XmlDocument xmlDoc = GetXmlDocument(payload);
            XmlNodeList xmlNodeListUsername = xmlDoc.GetElementsByTagName("Username");
            Assert.IsTrue(xmlNodeListUsername.Count > 0);
            Assert.AreEqual("certuser_biz_api1.paypal.com", xmlNodeListUsername[0].InnerXml);
            XmlNodeList xmlNodeListPassword = xmlDoc.GetElementsByTagName("Password");
            Assert.IsTrue(xmlNodeListPassword.Count > 0);
            Assert.AreEqual("D6JNKKULHN3G5B8A", xmlNodeListPassword[0].InnerXml);
            XmlNodeList xmlNodeListRequest = xmlDoc.GetElementsByTagName("Request");
            Assert.IsTrue(xmlNodeListRequest.Count > 0);
            Assert.AreEqual("test", xmlNodeListRequest[0].InnerXml);
        }
        
        [Test]
        public void SDKName()
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            soapHandler.SDKName = "testsdk";
            Assert.AreEqual("testsdk", soapHandler.SDKName);
        }

        [Test]
        public void SDKVersion()
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            soapHandler.SDKVersion = "1.0.0";
            Assert.AreEqual("1.0.0", soapHandler.SDKVersion);
        }

        [Test]
        public void GetEndPoint()
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            string endpoint = soapHandler.GetEndPoint();
            Assert.AreEqual(UnitTestConstants.API_ENDPOINT, endpoint);
        }      

        private XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlString = xmlString.Replace("soapenv:", string.Empty);
            xmlString = xmlString.Replace("ebl:", string.Empty);
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }
    }
}
