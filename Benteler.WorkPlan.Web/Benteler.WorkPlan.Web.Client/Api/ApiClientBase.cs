using Benteler.WorkPlan.Api.SharedModels.Authentication.Result;
using BitzArt.Blazor.Cookies;

namespace Benteler.WorkPlan.Web.Client.Api
{
    public class ApiClientBase
    {
        protected readonly HttpRequestHelper _httpRequestHelper;
        public ApiClientBase(HttpRequestHelper requestHelper)
        {
            _httpRequestHelper = requestHelper;
        }
       
    }
}
