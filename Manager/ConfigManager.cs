using System.Configuration;
using PayPal.Exception;

namespace PayPal.Manager
{    
    /// <summary>
    /// ConfigManager loads the configuration file and hands out
    /// appropriate parameters to application
    /// </summary>
    public sealed class ConfigManager
    {
        private SDKConfigHandler config;

        /// <summary>
        /// Singleton instance of the ConfigManager
        /// </summary>
        private static readonly ConfigManager singletonInstance = new ConfigManager();

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static ConfigManager() { }

        /// <summary>
        /// Private constructor
        /// </summary>
        private ConfigManager()
        {
            config = (SDKConfigHandler)ConfigurationManager.GetSection("paypal");
            if (config == null)
            {
                throw new ConfigException("Cannot read config file");
            }
        }

        /// <summary>
        /// Gets the Singleton instance of the ConfigManager
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                return singletonInstance;
            }
        }

        /// <summary>
        /// Returns the key from the configuration file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetProperty(string key)
        {
            return config.Setting(key);
        }

        /// <summary>
        /// Returns the API Username from the configuration file
        /// </summary>
        /// <param name="apiUserName"></param>
        /// <returns></returns>
        public Account GetAccount(string apiUserName)
        {   
            return config.Accounts[apiUserName];
        }

        /// <summary>
        /// Returns the index from the configuration file
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Account GetAccount(int index)
        {
            return config.Accounts[index];
        }        
    }
}
