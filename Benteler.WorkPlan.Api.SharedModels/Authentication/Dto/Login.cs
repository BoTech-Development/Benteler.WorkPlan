namespace Benteler.WorkPlan.Api.SharedModels.Authentication.Dto
{
    public class Login (string email, string password, string code, string recoveryCode)
    {
        public string email { get; set; } = email;
        public string password { get; set; } = password;
		public string twoFactorCode { get; set; } = code;
        public string twoFactorRecoveryCode { get; set; } = recoveryCode;

    }
}
