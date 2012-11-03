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
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
        }

        [Test]
        public void RetrieveAccountConfigByUsername()
        {
            configMgr = ConfigManager.Instance;
            Account acc = configMgr.GetAccount(UnitTestConstants.APIUserName);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
            Assert.AreEqual(UnitTestConstants.APIPassword, acc.APIPassword);
            Assert.AreEqual(UnitTestConstants.APISignature, acc.APISignature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, acc.ApplicationId);
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
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
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
