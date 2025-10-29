using System.Threading.Tasks;

namespace Benteler.WorkPlan.Web.Client.Api.Clients
{
    public class VerificationClient : ApiClientBase
    {
        public VerificationClient(HttpRequestHelper requestHelper) : base(requestHelper)
        {
        }
        public async Task<bool> IsEmailVerified(string email)
        {
            RequestResult<bool> result = await _httpRequestHelper.HttpGetJsonObject<bool>($"/auth/Verification/IsEmailVerified?email={email}");
            return result.IsSuccess() && result.ParsedData;
        }
    }
}
