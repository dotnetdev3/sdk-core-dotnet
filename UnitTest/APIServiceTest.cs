using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using PayPal.Manager;
using PayPal.NVP;
using PayPal.SOAP;

namespace PayPal.UnitTest
{
    [TestFixture]
    class APIServiceTest
    {
        APIService service;
        IAPICallPreHandler handler;
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        [Test]
        public void MakeRequestUsingNVPCertificateCredential()
        {
            string payload = @"requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    "certuser_biz_api1.paypal.com", null, null);
            Thread.Sleep(5000);
            APIService service = new APIService();
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }     

        [Test]
        public void MakeRequestUsingNVPSignatureCredential()
        {
            string payload = @"requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    UnitTestConstants.API_USER_NAME, null, null);
            Thread.Sleep(5000);
            service = new APIService();
            string response = service.MakeRequestUsing(handler);           
            Assert.IsNotNull(response);            
            Assert.IsTrue(response.Contains("responseEnvelope.ack=Success"));
        }
        
        [Ignore] //[Test] To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
        public void MakeRequestUsingSOAPSignatureCredential()
        {
            string payload = @"<ns:GetBalanceReq><ns:GetBalanceRequest><ebl:Version>94.0</ebl:Version></ns:GetBalanceRequest></ns:GetBalanceReq>";
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(payload, null, null);
            handler = new MerchantAPICallPreHandler(defaultSOAPHandler, UnitTestConstants.API_USER_NAME, null, null);
            service = new APIService();
            string response = service.MakeRequestUsing(handler);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Contains("<Ack xmlns=\"urn:ebay:apis:eBLBaseComponents\">Success</Ack>"));
        }
    }
}
