using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace PayPal.Util
{
    public class NVPUtil
    {
        /// <summary>
        /// Split a Name Value formatted string into a dictionary
        /// </summary>
        /// <param name="nvpStr"></param>
        /// <returns></returns>
        public Dictionary<string, string> ParseNVPString(string nvpStr)
        {
            Dictionary<string, string> nvpMap = new Dictionary<string, string>();
            string[] keyValuePairs = nvpStr.Split('&');
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    nvpMap.Add(keyValue[0], HttpUtility.UrlDecode(keyValue[1], BaseConstants.ENCODING_FORMAT) );
                }
            }
            return nvpMap;
        }
    }
}
