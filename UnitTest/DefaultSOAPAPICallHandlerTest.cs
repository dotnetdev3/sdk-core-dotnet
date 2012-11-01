using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PayPal.UnitTest
{
    [TestFixture]
    class DefaultSOAPAPICallHandlerTest
    {
        DefaultSOAPAPICallHandler defaultSOAPHandler;

        [Ignore] //[Test] To Run this Test Case configure App.config <add name="endpoint" value="https://api-3t.sandbox.paypal.com/2.0"/>
	    public void EndPoint() 
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(
                    "requestEnvelope.errorLanguage=en_US&baseAmountList.currency(0).code=USD&baseAmountList.currency(0).amount=2.0&convertToCurrencyList.currencyCode(0)=GBP",
                    string.Empty, string.Empty);
		    Assert.AreEqual("https://api-3t.sandbox.paypal.com/2.0", defaultSOAPHandler.GetEndPoint());
        }
    
        [Test]
        public void HeaderElement()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            defaultSOAPHandler.HeaderElement = "HeaderElement";
            Assert.AreEqual("HeaderElement", defaultSOAPHandler.HeaderElement);
        }

        [Test]
        public void NamespaceAttributes()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            defaultSOAPHandler.NamespaceAttributes = "NamespaceAttributes";
            Assert.AreEqual("NamespaceAttributes", defaultSOAPHandler.NamespaceAttributes);
        }

        [Test]
        public void GetPayloadForEmptyRawPayload()
        {
            defaultSOAPHandler = new DefaultSOAPAPICallHandler(string.Empty, string.Empty, string.Empty);
            Assert.AreEqual("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" ><soapenv:Header></soapenv:Header><soapenv:Body></soapenv:Body></soapenv:Envelope>", defaultSOAPHandler.GetPayLoad());
        }
    }
}
