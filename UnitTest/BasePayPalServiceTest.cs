using System;
using NUnit.Framework;
using PayPal.NVP;

namespace PayPal.UnitTest
{
    [TestFixture]
    class BasePayPalServiceTest
    {
        [Test]
        public void CallServiceTest()
        {
            BasePayPalService service = new BasePayPalService();
            string payload = "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP";
            IAPICallPreHandler handler = new PlatformAPICallPreHandler(payload,
                    "AdaptivePayments", "ConvertCurrency",
                    UnitTestConstants.API_USER_NAME, null, null);
            string response = service.Call(handler);
            Assert.IsNotNull(response);
            StringAssert.Contains("responseEnvelope.ack", response);
        }
    }
}
