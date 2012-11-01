using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NUnit.Framework;
using PayPal.Manager;

namespace PayPal.UnitTest.Manager
{
    [TestFixture]
    class ConfigManagerTest
    {
        ConfigManager configMgr;

        [Test]
        public void RetrieveAccountConfigByIndex()
        {
            configMgr = ConfigManager.Instance;
            Account acc = configMgr.GetAccount(0);
            Assert.IsNotNull(acc);
            Assert.AreEqual("jb-us-seller_api1.paypal.com", acc.APIUsername);
        }

        [Test]
        public void RetrieveAccountConfigByUsername()
        {
            configMgr = ConfigManager.Instance;
            Account acc = configMgr.GetAccount("jb-us-seller_api1.paypal.com");
            Assert.IsNotNull(acc);
            Assert.AreEqual("jb-us-seller_api1.paypal.com", acc.APIUsername);
            Assert.AreEqual("WX4WTU3S8MY44S7F", acc.APIPassword);
            Assert.AreEqual("AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy", acc.APISignature);
            Assert.AreEqual("APP-80W284485P519543T", acc.ApplicationId);
        }

        [Test]
        public void RetrieveNonExistentAccount()
        {
            configMgr = ConfigManager.Instance;
            Account acc = configMgr.GetAccount("i-do-not-exist_api1.paypal.com");
            Assert.IsNull(acc, "Invalid account name returns null account config");
        }

        [Test]
        public void RetrieveValidProperty()
        {
            configMgr = ConfigManager.Instance;
            string endpoint = configMgr.GetProperty("endpoint");
            Assert.IsNotNull(endpoint);
            Assert.AreEqual("https://svcs.sandbox.paypal.com/", endpoint);
            string connectionTimeout = configMgr.GetProperty("connectionTimeout");
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }

        [Test]
        public void RetrieveNonExistentProperty()
        {
            configMgr = ConfigManager.Instance;
            string endpoint = configMgr.GetProperty("endpointMisspelt");
            Assert.IsNull(endpoint);
        }

    }
}
