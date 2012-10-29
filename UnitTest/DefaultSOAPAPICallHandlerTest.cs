using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PayPal.UnitTest
{
    [TestFixture]
    class DefaultSOAPAPICallHandlerTest
    {
        [Test]
	    public void EndPoint() 
        {
             DefaultSOAPAPICallHandler defaultHandler = new DefaultSOAPAPICallHandler(
                    "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP",
                    string.Empty, string.Empty);
		    Assert.AreEqual("https://api-3t.sandbox.paypal.com/2.0", defaultHandler.GetEndPoint());
        }
    
        [Test]
        public void HeaderElement()
        {
             DefaultSOAPAPICallHandler defaultHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            defaultHandler.HeaderElement = "HeaderElement";
            Assert.AreEqual("HeaderElement", defaultHandler.HeaderElement);
        }

        [Test]
        public void NamespaceAttributes()
        {
             DefaultSOAPAPICallHandler defaultHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            defaultHandler.NamespaceAttributes = "NamespaceAttributes";
            Assert.AreEqual("NamespaceAttributes", defaultHandler.NamespaceAttributes);
        }

        [Test]
        public void GetPayloadForEmptyRawPayload()
        {
             DefaultSOAPAPICallHandler defaultHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultHandler.GetPayLoad());
        }
    }

}
