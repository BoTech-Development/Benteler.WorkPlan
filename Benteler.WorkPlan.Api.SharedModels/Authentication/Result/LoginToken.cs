namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Result
{
    public class LoginToken
    {
        public string TokenType { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; } = 0;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
