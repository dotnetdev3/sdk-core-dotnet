using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using PayPal.Exception;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    public class ConnectionManagerTest
    {
        [Test]
        public void CreateNewConnection()
        {
            ConnectionManager connMgr = ConnectionManager.Instance;
            ConfigManager configMgr = ConfigManager.Instance;

            HttpWebRequest httpRequest = connMgr.GetConnection("http://paypal.com/");
            Assert.IsNotNull(httpRequest);
            Assert.AreEqual("http://paypal.com/", httpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(configMgr.GetProperty("connectionTimeout"), httpRequest.Timeout.ToString());            
        }

        [Test, ExpectedException( typeof(ConfigException) )]
        public void CreateNewConnectionWithInvalidURL()
        {
            ConnectionManager connMgr = ConnectionManager.Instance;
            HttpWebRequest httpRequest = connMgr.GetConnection("Not a url");
        }
    }
}
