using Benteler.WorkPlan.Api.SharedModels.Authentication.Dto;
using Benteler.WorkPlan.Api.SharedModels.Authentication.Result;
using BitzArt.Blazor.Cookies;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Benteler.WorkPlan.Web.Client.Api.Clients.Auth
{
    public class AuthenticationClient : ApiClientBase
    {
        public AuthenticationClient(HttpRequestHelper requestHelper) : base(requestHelper)
        {
        }
        /// <summary>
        /// Assigns the specified login token to the HTTP request helper for use in subsequent HTTP requests headers.
        /// </summary>
        /// <param name="token">The login token to associate with the HTTP request helper. Cannot be null.</param>
        public void SaveLoginTokenToHttpRequestHelper(LoginToken token)
        {
            _httpRequestHelper.Token = token;
        }
        /// <summary>
        /// Saves the specified login token to browser cookies using the provided cookie service.
        /// </summary>
        /// <param name="cookieService">The service used to manage browser cookies for storing the login token. Cannot be null.</param>
        /// <param name="token">The login token to be saved to cookies. Cannot be null.</param>
        public async Task SaveLoginTokenToCookies(ICookieService cookieService, LoginToken token)
        {
            //_httpRequestHelper.Token = token;
            //Console.WriteLine("Set Token to: "  + _httpRequestHelper.Token.AccessToken);
            await _httpRequestHelper.SaveTokenCookies(cookieService, token);
        }
        /// <summary>
        /// Retrieves the login token from cookies using the specified cookie service.
        /// </summary>
        /// <param name="cookieService">The service used to access and manage cookies for retrieving the login token. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the login token if found;
        /// otherwise, null.</returns>
        public async Task<LoginToken?> GetLoginTokenFromCookies(ICookieService cookieService)
        {
            return await _httpRequestHelper.LoadTokenCookies(cookieService);
        }
        /// <summary>
        /// Requests a new access token using the provided refresh token.
        /// </summary>
        /// <remarks>This method performs an asynchronous HTTP request to refresh the authentication
        /// token. The returned token should be used for subsequent authenticated requests. Ensure that the provided
        /// token contains a valid refresh token.</remarks>
        /// <param name="token">The current <see cref="LoginToken"/> containing the refresh token to be used for obtaining a new access
        /// token. Cannot be null.</param>
        /// <returns>A <see cref="LoginToken"/> instance containing the refreshed access and refresh tokens.</returns>
        /// <exception cref="Exception">Thrown if the token refresh operation fails or the server returns an error.</exception>
        public async Task<LoginToken> RefreshToken(LoginToken token)
        {
            RequestResult<LoginToken> result = await _httpRequestHelper.HttpPostJsonAndGetJson<LoginToken>("/refresh", new RefreshToken()
            {
                refreshToken = token.RefreshToken
            });
            if (!result.IsSuccess() && result.ParsedData != null)
                throw new Exception($"Could not refresh token. {result.Error?.Message}");
            return result.ParsedData;
        }
        /// <summary>
        /// Checks if two-factor authentication (2FA) is enabled for the user associated with the provided email address.
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <returns>true or false</returns>
        /// <exception cref="Exception">When the api request failed.</exception>
        public async Task<bool> IsTwoFactorEnabled(string email)
        {
            RequestResult<bool> result = await _httpRequestHelper.HttpGetJsonObject<bool>(
                $"/auth/TwoFactor/IsTwoFactorAuthEnabled?email={email}", null);
            if (!result.IsSuccess())
                throw new Exception($"Could not check if 2fa is enabled. \n  {result.ResponseMessage.ReasonPhrase} \n {result.Error} ");
            return result.ParsedData;
        }
        /// <summary>
        /// Generates two-factor authentication (2FA) information for the specified email address by performing an
        /// asynchronous HTTP request.
        /// </summary>
        /// <param name="email">The email address for which to generate 2FA information. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Generated2FaInfo"/>
        /// object with the generated 2FA details for the specified email address.</returns>
        /// <exception cref="Exception">Thrown if the 2FA information could not be generated due to a failed HTTP request or an error response from
        /// the server.</exception>
        public async Task<Generated2FaInfo> Generated2FaInfo(string email)
        {
			RequestResult<Generated2FaInfo> result = await _httpRequestHelper.HttpGetJsonObject<Generated2FaInfo>(
			   $"/auth/TwoFactor/Generate?email={email}", null);
			if (!result.IsSuccess())
				throw new Exception($"Could generate 2fa info. \n  {result.ResponseMessage.ReasonPhrase} \n {result.Error} ");
			return result.ParsedData;
		}
        /// <summary>
        /// Enables two-factor authentication (2FA) for the user by submitting the correct code and correct email.
        /// </summary>
        /// <param name="dto">An object containing the information required to enable two-factor authentication. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the request
        /// to enable 2FA was successful; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> Enable2Fa(Enable2Fa dto)
        {
			RequestResult<dynamic> result = await _httpRequestHelper.HttpPostJson($"/auth/TwoFactor/Enable", dto);
            return result.IsSuccess();
		}
        /// <summary>
        /// Authenticates a user using the specified email, password, and two-factor authentication code, and returns a
        /// login token if successful.
        /// </summary>
        /// <param name="email">The email address associated with the user account to authenticate. Cannot be null or empty.</param>
        /// <param name="password">The password for the user account. Cannot be null or empty.</param>
        /// <param name="twoFaCode">The two-factor authentication code required to complete the login process. May be null or empty if
        /// two-factor authentication is not enabled for the account.</param>
        /// <returns>A <see cref="LoginToken"/> representing the authenticated session for the user.</returns>
        /// <exception cref="Exception">Thrown if the login attempt fails due to invalid credentials, missing two-factor authentication code, or
        /// other authentication errors.</exception>
        public async Task<LoginToken?> Login(string email, string password, string twoFaCode, string recoveryCode)
        {
            RequestResult<LoginToken> result = await _httpRequestHelper.HttpPostJsonAndGetJson<LoginToken>(
                $"/login",
                new Login(email, password, twoFaCode, recoveryCode));
            // Check if the request was successful and if 2fa is required
            if (result.IsSuccess())
            {
                if(result.ResponseMessage != null)
                    if (result.ResponseMessage.Content.ReadAsStringAsync().Result.Contains("RequiresTwoFactor"))
                        throw new ArgumentException("Two factor authentication is enabled. Please provide the 2fa code.");
                return result.ParsedData; // Else return the login token
            }
            // If the request failed, throw an exception with the error message
            throw new Exception($"Could not login. {result.Error?.Message}");
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
            RequestResult<dynamic> result = await _httpRequestHelper.HttpPostJson($"/resendConfirmationEmail", new ResendConfirmationEmailModel(email));
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
                new Register(email, password));
        }
        /// <summary>
        /// Associates the specified phone number with the user account identified by the provided email address.
        /// </summary>
        /// <param name="email">The email address of the user account to which the phone number will be added. Cannot be null or empty.</param>
        /// <param name="phoneNumber">The phone number to associate with the user account. Must be a valid phone number format and cannot be null or empty.</param>
        /// <returns> The task result contains a <see cref="RequestResult{dynamic}"/> indicating the outcome of the request.</returns>
        private async Task<RequestResult<dynamic>> AddPhoneNumberToAccount(string email, string phoneNumber)
        {
            return await _httpRequestHelper.HttpPatch($"/auth/Sms/AddPhoneNumberToUser?email={email}&phoneNumber={phoneNumber}", null);
        }
    }
	/*   public async Task<LoginToken> Login2Fa(string userId, string code)
	  {
		  RequestResult<LoginToken> result = await _httpRequestHelper.HttpGetJsonObject<LoginToken>(
			  $"/auth/Account/Login2fa",
			  new StringContent(JsonConvert.SerializeObject(new Login(userId, code)),
				  System.Text.Encoding.UTF8, "application/json"));
		  if (!result.IsSuccess())
			  throw new Exception($"Could not login. {result.Error?.Message}");
		  return result.ParsedData;

	  }*/
	/// <summary>
	/// Attempts to log in with the specified email address or retrieves the associated user identifier if 2fa is required.
	/// </summary>
	/// <param name="email">The email address to use for login or user identification. Cannot be null or empty.</param>
	/// <returns>"ok" if the login was successfull without the 2fa code or the id of the user</returns>
	/*  private async Task<string> TryToLoginOrGetUserId(string email, string password)
	  {
		  RequestResult<string> result = await _httpRequestHelper.HttpPostJson(
			  $"/auth/Account/Login",
			  new Login(email, password));
	  }*/
}
