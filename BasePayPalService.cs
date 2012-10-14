using System;
using System.Collections.Generic;
using System.Text;
using PayPal.Authentication;

namespace PayPal
{
    public class BasePayPalService
    {       
       private string AccessToken;
       private string AccessTokenSecret;
       private string LastRequest;
       private string LastResponse;

        public BasePayPalService()
        {
           
        }

        public void setAccessToken(string accessToken)
        {
            this.AccessToken = accessToken;
        }

        public void setAccessTokenSecret(string accessTokenSecret)
        {
            this.AccessTokenSecret = accessTokenSecret;
        }

        public string getAccessToken()
        {
            return this.AccessToken;
        }

        public string getAccessTokenSecret()
        {
            return this.AccessTokenSecret;
        }

        public string getLastRequest()
        {
            return this.LastRequest;
        }

        public string getLastResponse()
        {
            return this.LastResponse;
        }

        /// <summary>
        /// Call method exposed to user
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string Call(IAPICallPreHandler apiCallHandler)
        {
            APIService apiService = new APIService();
            this.LastRequest = apiCallHandler.GetPayLoad();
            this.LastResponse = apiService.MakeRequestUsing(apiCallHandler);
            return this.LastResponse;
        }
    }
}
