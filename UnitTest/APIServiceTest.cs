using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.NVP;
using PayPal.SOAP;

namespace PayPal.UnitTest
{
    class APIServiceTest
    {
        APIService service;
        HttpWebRequest connection;
        Dictionary<string, string> map = new Dictionary<string, string>();
 
        public APIServiceTest()
        {
            service = new APIService();
            string uri = ConfigManager.Instance.GetProperty(BaseConstants.END_POINT);
            ConfigManager configMgr = ConfigManager.Instance;
            ConnectionManager connMgr = ConnectionManager.Instance;
            connection = connMgr.GetConnection(uri);
        }

        [Test]
        public void getEndPointTest(ConfigManager conf)
        {
            Assert.AreEqual(UnitTestConstants.API_ENDPOINT, ConfigManager.Instance.GetProperty(BaseConstants.END_POINT));

        }

        [Test]
        public void makeRequestUsingForNVPSignatureCredentialTest(ConfigManager conf)
        {
            string payload = "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            IAPICallPreHandler handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    UnitTestConstants.API_USER_NAME, null, null);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        [Test]
        public void makeRequestUsingForSOAPSignatureCredentialTest(ConfigManager conf)
        {
            service = new APIService();
            string payload = "<ns:GetBalanceReq><ns:GetBalanceRequest><ebl:Version>94.0</ebl:Version></ns:GetBalanceRequest></ns:GetBalanceReq>";
            DefaultSOAPAPICallHandler apiCallHandler = new DefaultSOAPAPICallHandler(
                    payload, null, null);
            IAPICallPreHandler handler = new MerchantAPICallPreHandler(
                    apiCallHandler, UnitTestConstants.API_USER_NAME, null, null);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
        
        [Test]
        public void makeRequestUsingForNVPCertificateCredentialTest(ConfigManager conf)
        {
            string payload = "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            IAPICallPreHandler handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    "certuser_biz_api1.paypal.com", null, null);
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        [Test]
        public void proxyTest(ConfigManager conf)
        {
            Assert.AreEqual(conf.GetProperty(BaseConstants.HTTP_PROXY_ADDRESS), "https://svcs.sandbox.paypal.com/proxy");
        }        
    }
}
