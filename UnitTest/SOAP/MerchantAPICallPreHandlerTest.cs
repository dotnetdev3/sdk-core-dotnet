using System;
using System.Collections.Generic;
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
        public void getHeaderMapSignatureTest(ConfigManager configMgr)
        {

            ICredential signatureCredential = credentialmgr.GetCredentials("jb-us-seller_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, signatureCredential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual("jb-us-seller_api1.paypal.com", headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual("WX4WTU3S8MY44S7F", headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", headers[BaseConstants.PAYPAL_SECURITY_SIGNATURE_HEADER]);
            Assert.AreEqual(BaseConstants.PAYLOAD_FORMAT_SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.PAYLOAD_FORMAT_SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
        }

        [Test]
        public void getHeaderMapCertificateTest(ConfigManager conf)
        {

            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            Dictionary<String, String> headers = soapHandler.GetHeaderMap();
            Assert.IsNotNull(headers);
            Assert.IsTrue(headers.Count > 0);
            Assert.AreEqual("certuser_biz_api1.paypal.com", headers[BaseConstants.PAYPAL_SECURITY_USERID_HEADER]);
            Assert.AreEqual("D6JNKKULHN3G5B8A", headers[BaseConstants.PAYPAL_SECURITY_PASSWORD_HEADER]);
            Assert.AreEqual(BaseConstants.PAYLOAD_FORMAT_SOAP, headers[BaseConstants.PAYPAL_REQUEST_DATA_FORMAT_HEADER]);
            Assert.AreEqual(BaseConstants.PAYLOAD_FORMAT_SOAP, headers[BaseConstants.PAYPAL_RESPONSE_DATA_FORMAT_HEADER]);
            Assert.AreEqual(soapHandler.SDKName + "-" + soapHandler.SDKVersion, headers[BaseConstants.PAYPAL_REQUEST_SOURCE_HEADER]);
        }

        [Test]
        public void getPayLoadForSignature(ConfigManager conf)
        {
            ICredential signatureCredential = credentialmgr.GetCredentials("jb-us-seller_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, signatureCredential);
            string payload = soapHandler.GetPayLoad();

            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList header = docEle.getElementsByTagName("soapenv:Header");
            NodeList requestCredential = ((Element)header.item(0)).getElementsByTagName("ns:RequesterCredentials");
            NodeList credential = ((Element)requestCredential.item(0)).getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");
            NodeList sign = ((Element)credential.item(0)).getElementsByTagName("ebl:Signature");

            string username = user.item(0).getTextContent();
            string password = psw.item(0).getTextContent();
            string signature = sign.item(0).getTextContent();

            Assert.AreEqual("jb-us-seller_api1.paypal.com", username);
            Assert.AreEqual("WX4WTU3S8MY44S7F", password);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", signature);

            NodeList requestBody = docEle.getElementsByTagName("Request");
            Node bodyContent = requestBody.item(0);
            string bodyText = bodyContent.getTextContent();
            Assert.AreEqual("test", bodyText);

        }

        [Test]
        public void getPayLoadForCertificate(ConfigManager conf)
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            string payload = soapHandler.GetPayLoad();

            Document dom = loadXMLFromString(payload);
            Element docEle = dom.getDocumentElement();
            NodeList header = docEle.getElementsByTagName("soapenv:Header");
            NodeList requestCredential = ((Element)header.item(0)).getElementsByTagName("ns:RequesterCredentials");
            NodeList credential = ((Element)requestCredential.item(0)).getElementsByTagName("ebl:Credentials");
            NodeList user = ((Element)credential.item(0)).getElementsByTagName("ebl:Username");
            NodeList psw = ((Element)credential.item(0)).getElementsByTagName("ebl:Password");

            string username = user.item(0).getTextContent();
            string password = psw.item(0).getTextContent();

            Assert.AreEqual("certuser_biz_api1.paypal.com", username);
            Assert.AreEqual("D6JNKKULHN3G5B8A", password);

            NodeList requestBody = docEle.getElementsByTagName("Request");
            Node bodyContent = requestBody.item(0);
            string bodyText = bodyContent.getTextContent();
            Assert.AreEqual("test", bodyText);
        }

        [Test]
        public void setGetSDKNameTest(ConfigManager conf)
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            soapHandler.SDKName = "testsdk";
            Assert.AreEqual("testsdk", soapHandler.SDKName);
        }

        [Test]
        public void setGetSDKVersionTest(ConfigManager conf)
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            soapHandler.SDKVersion = "1.0.0";
            Assert.AreEqual("1.0.0", soapHandler.SDKVersion);
        }

        [Test]
        public void getEndPointTest(ConfigManager conf)
        {
            ICredential certificateCredential = credentialmgr.GetCredentials("certuser_biz_api1.paypal.com");
            MerchantAPICallPreHandler soapHandler = new MerchantAPICallPreHandler(defaultSoaphandler, certificateCredential);
            string endpoint = soapHandler.GetEndPoint();
            Assert.AreEqual("https://api-3t.sandbox.paypal.com/2.0", endpoint);
        }

        [Test]
        public void MerchantAPICallPreHandlerConstructorTest()
        {
            new MerchantAPICallPreHandler(defaultSoaphandler, null);
        }

        //TODO:
        // private XmlDocument loadXMLFromString(String xml)
	    private  Document loadXMLFromString(String xml) 
        {
		    ByteArrayInputStream stream = new ByteArrayInputStream(xml.getBytes());
		    DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
		    return builder.parse(stream);
        }	         
    }
}
