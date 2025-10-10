using System.Threading.Tasks;

namespace Benteler.WorkPlan.Web.Api.Clients
{
    public class VerificationClient : ApiClientBase
    {
        public VerificationClient(HttpRequestHelper requestHelper) : base(requestHelper)
        {
        }
        public async Task<bool> IsEmailVerified(string email)
        {
            RequestResult<bool> result = await _httpRequestHelper.HttpGetJsonObject<bool>($"/Verification/IsEmailVerified?email={email}");
            return result.IsSuccess() && result.ParsedData;
        }
    }
}
