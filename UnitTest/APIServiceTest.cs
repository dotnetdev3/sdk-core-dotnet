using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PayPal.Manager;
using PayPal.NVP;
using PayPal.SOAP;

namespace PayPal.UnitTest
{
    class APIServiceTest
    {
        [Test]
        public void MakeRequestUsingNVPSignatureCredentialTest()
        {
            string payload = "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            IAPICallPreHandler handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    UnitTestConstants.API_USER_NAME, null, null);
            APIService  service = new APIService();
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }

        [Test]
        public void MakeRequestUsingSOAPSignatureCredentialTest()
        {
            string payload = "<ns:GetBalanceReq><ns:GetBalanceRequest><ebl:Version>94.0</ebl:Version></ns:GetBalanceRequest></ns:GetBalanceReq>";
            DefaultSOAPAPICallHandler apiCallHandler = new DefaultSOAPAPICallHandler(
                    payload, null, null);
            IAPICallPreHandler handler = new MerchantAPICallPreHandler(
                    apiCallHandler, UnitTestConstants.API_USER_NAME, null, null);
            APIService service = new APIService();
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }

        [Test]
        public void MakeRequestUsingNVPCertificateCredentialTest()
        {
            string payload = "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            IAPICallPreHandler handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    "certuser_biz_api1.paypal.com", null, null);
            APIService service = new APIService();
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }        
    }
}
