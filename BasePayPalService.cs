namespace PayPal
{
    public abstract class BasePayPalService
    {       
       private string accessToken;
       private string accessTokenSecret;
       private string lastRequest;
       private string lastResponse;

        public BasePayPalService() { }

        public void setAccessToken(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public void setAccessTokenSecret(string accessTokenSecret)
        {
            this.accessTokenSecret = accessTokenSecret;
        }

        public string getAccessToken()
        {
            return this.accessToken;
        }

        public string getAccessTokenSecret()
        {
            return this.accessTokenSecret;
        }

        public string getLastRequest()
        {
            return this.lastRequest;
        }

        public string getLastResponse()
        {
            return this.lastResponse;
        }

        /// <summary>
        /// Call method exposed to user
        /// </summary>
        /// <param name="apiCallHandler"></param>
        /// <returns></returns>
        public string Call(IAPICallPreHandler apiCallHandler)
        {
            APIService apiServ = new APIService();
            this.lastRequest = apiCallHandler.GetPayLoad();
            this.lastResponse = apiServ.MakeRequestUsing(apiCallHandler);
            return this.lastResponse;
        }
    }
}
