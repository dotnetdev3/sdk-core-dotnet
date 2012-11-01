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
        ConnectionManager connectionMgr;
        HttpWebRequest httpRequest;

        [Test]
        public void CreateNewConnection()
        {
            connectionMgr = ConnectionManager.Instance;
            ConfigManager configMgr = ConfigManager.Instance;
            httpRequest = connectionMgr.GetConnection("http://paypal.com/");
            Assert.IsNotNull(httpRequest);
            Assert.AreEqual("http://paypal.com/", httpRequest.RequestUri.AbsoluteUri);
            Assert.AreEqual(configMgr.GetProperty("connectionTimeout"), httpRequest.Timeout.ToString());
        }

        [Test, ExpectedException(typeof(ConfigException))]
        public void CreateNewConnectionWithInvalidURL()
        {
            connectionMgr = ConnectionManager.Instance;
            httpRequest = connectionMgr.GetConnection("Not a url");
        }
    }
}
