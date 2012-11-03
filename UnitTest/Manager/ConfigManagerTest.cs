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
        ConfigManager configMngr;

        [Test]
        public void RetrieveAccountConfigByIndex()
        {
            configMngr = ConfigManager.Instance;
            Account acc = configMngr.GetAccount(0);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
        }

        [Test]
        public void RetrieveAccountConfigByUsername()
        {
            configMngr = ConfigManager.Instance;
            Account acc = configMngr.GetAccount(UnitTestConstants.APIUserName);
            Assert.IsNotNull(acc);
            Assert.AreEqual(UnitTestConstants.APIUserName, acc.APIUsername);
            Assert.AreEqual(UnitTestConstants.APIPassword, acc.APIPassword);
            Assert.AreEqual(UnitTestConstants.APISignature, acc.APISignature);
            Assert.AreEqual(UnitTestConstants.ApplicationID, acc.ApplicationId);
        }

        [Test]
        public void RetrieveNonExistentAccount()
        {
            configMngr = ConfigManager.Instance;
            Account acc = configMngr.GetAccount("i-do-not-exist_api1.paypal.com");
            Assert.IsNull(acc, "Invalid account name returns null account config");
        }

        [Test]
        public void RetrieveValidProperty()
        {
            configMngr = ConfigManager.Instance;
            string endpoint = configMngr.GetProperty("endpoint");
            Assert.IsNotNull(endpoint);
            Assert.AreEqual(UnitTestConstants.APIEndpointNVP, endpoint);
            string connectionTimeout = configMngr.GetProperty("connectionTimeout");
            Assert.IsNotNull(connectionTimeout);
            Assert.AreEqual("360000", connectionTimeout);
        }

        [Test]
        public void RetrieveNonExistentProperty()
        {
            configMngr = ConfigManager.Instance;
            string endpoint = configMngr.GetProperty("endpointMisspelt");
            Assert.IsNull(endpoint);
        }

    }
}
