namespace Benteler.WorkPlan.Web.Api
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
