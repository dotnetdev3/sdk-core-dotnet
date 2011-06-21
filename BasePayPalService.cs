using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Authentication;

namespace PayPal
{
    public class BasePayPalService
    {       
        private string serviceName;

        public BasePayPalService(string serviceName)
        {
            this.serviceName = serviceName;
        }

        /// <summary>
        /// Call method exposed to user
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestPayload"></param>
        /// <returns></returns>
        public string call(String method, string requestPayload, string apiUsername)
        {            
            APIService apiService = new APIService(serviceName);
            return apiService.makeRequest(method, requestPayload, apiUsername);            
        }
    }
}
