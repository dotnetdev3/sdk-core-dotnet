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
            IAPICallPreHandler apiCallHandler = new PlatformAPICallPreHandler("payload", "servicename", "method",UnitTestConstants.API_USER_NAME, UnitTestConstants.ACCESS_TOKEN, UnitTestConstants.TOKEN_SECRET);
            string response = service.Call(apiCallHandler);
            Assert.IsNotNull(response);
            StringAssert.Contains("responseEnvelope.ack", response);
        }
    }
}
