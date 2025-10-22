using System.Net;
using Benteler.WorkPlan.Web.Api;
using Benteler.WorkPlan.Web.Api.Clients;
using Benteler.WorkPlan.Web.Api.Clients.Auth;

namespace Benteler.WorkPlan.Web.Api
{
    public class ApiClient
    {
        public static HttpResponseMessage? Error { get; private set; }
        private static ApiClient? _instance;

        public VerificationClient VerificationClient { get; }
        public AuthenticationClient AuthenticationClient { get; }
        private HttpRequestHelper _httpRequestHelper;

        private ApiClient(HttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
            AuthenticationClient = new AuthenticationClient(httpRequestHelper);
            VerificationClient = new VerificationClient(httpRequestHelper);
        }
        public static async Task<ApiClient?> GetInstance()
        {
            if (_instance == null) Error = await ApiClient.CreateApiClientFor("https://localhost:7236");
            return _instance;
        }
        /// <summary>
        /// Creates an ApiClient, which can be used to access a specific api, which is hosted on a specific IpAddress or domain
        /// </summary>
        /// <param name="baseUrl">the url of the api</param>
        /// <returns>The error or success message when a connection can be established.</returns>
        public static async Task<HttpResponseMessage?>  CreateApiClientFor(string baseUrl)
        {
            HttpRequestHelper requestHelper = new HttpRequestHelper(baseUrl);
            requestHelper.Token = new WorkPlan.Api.SharedModels.Authentication.Result.LoginToken();
            RequestResult<bool> result = await TryToConnectToNewApi(requestHelper);
            if(result.IsSuccess())
                _instance = new ApiClient(requestHelper);
            return result.ResponseMessage;
        }
        /// <summary>
        /// Check if the given Api is still reachable in this network.
        /// </summary>
        /// <param name="requestHelper"></param>
        /// <returns></returns>
        private static async Task<RequestResult<bool>> TryToConnectToNewApi(
            HttpRequestHelper requestHelper)
        {
            RequestResult<string> response = await requestHelper.HttpGetString("/Status/TestConnection");
            if (response.IsSuccess())
                return new RequestResult<bool>(true, response.ResponseMessage, true, null);
            return new RequestResult<bool>(false, response.ResponseMessage, false, response.Error);
        }
        /// <summary>
        /// Check if the Api is still reachable in this network.
        /// </summary>
        /// <returns></returns>
        public async Task<RequestResult<bool>> TestConnection()
        {
            return await ApiClient.TryToConnectToNewApi(this._httpRequestHelper);
        }
    }
}
