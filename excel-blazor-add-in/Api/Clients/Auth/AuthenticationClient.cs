using Benteler.WorkPlan.Web.Models.Auth;
using Newtonsoft.Json;
using System.Text;

namespace Benteler.WorkPlan.Web.Api.Clients.Auth
{
    public class AuthenticationClient : ApiClientBase
    {
        public AuthenticationClient(HttpRequestHelper requestHelper) : base(requestHelper)
        {
        }
        public async Task< LoginToken> Login(string email, string password)
        {
            RequestResult<LoginToken> result = await _httpRequestHelper.HttpGetJsonObject<LoginToken>($"/login", new StringContent(
                $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}",
                System.Text.Encoding.UTF8, "application/json"));
            if (!result.IsSuccess())
                throw new Exception($"Could not login. {result.Error?.Message}");
            return result.ParsedData;
            
        }
        /// <summary>
        /// Registers a new user with the given email, password and phonenumber.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="phoneNumber"></param>
        /// <exception cref="Exception"></exception>
        public async Task Register(string email, string password, string phoneNumber)
        {
            RequestResult<dynamic> registerResult = await RegisterNewUser(email, password);
            if (registerResult.IsSuccess())
            {
                RequestResult<dynamic> addPhoneResult = await AddPhoneNumberToAccount(email, phoneNumber);
                if(!addPhoneResult.IsSuccess())
                    throw new Exception($"Could not add phonenumber to account. {addPhoneResult.ResponseMessage.ReasonPhrase} \n {addPhoneResult.Error}");
                return;
            }
            throw new Exception($"Could not register new user. \n  {registerResult.ResponseMessage.ReasonPhrase} \n {registerResult.Error} ");
        }
        /// <summary>
        /// Resends the email verification message to the specified email address.
        /// </summary>
        /// <param name="email">The email address to which the verification message will be sent. Cannot be null or empty.</param>
        /// <exception cref="Exception">Thrown if the email verification message could not be sent.</exception>
        public async Task ResendEmailVerification(string email)
        {
            RequestResult<dynamic> result = await _httpRequestHelper.HttpPost($"/confirmEmail", new StringContent(
                $"{{\"email\":\"{email}\"}}",
                System.Text.Encoding.UTF8, "application/json"));
            if (!result.IsSuccess())
                throw new Exception($"Could not send email verification. {result.Error?.Message}");
        }
        /// <summary>
        /// Only Registers a new user with the given password and email. <br/>
        /// <code>The Phonenumber is not included in this process.</code>
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private async Task<RequestResult<dynamic>> RegisterNewUser(string email, string password)
        {
            return await _httpRequestHelper.HttpPostJson("/register", 
                new RegisterModel(email, password));
        }
        private async Task<RequestResult<dynamic>> AddPhoneNumberToAccount(string email, string phoneNumber)
        {
            return await _httpRequestHelper.HttpPatch($"/Sms/AddPhoneNumberToUser?email={email}&phoneNumber={phoneNumber}", null);
        }
        private class RegisterModel
        {
            public string email { get; set; }
            public string password { get; set; }
            public RegisterModel(string email, string password)
            {
                this.email = email;
                this.password = password;
            }
        }
    }
}
