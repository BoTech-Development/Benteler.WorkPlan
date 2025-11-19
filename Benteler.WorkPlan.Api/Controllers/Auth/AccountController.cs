using Benteler.WorkPlan.Api.Data;
using Benteler.WorkPlan.Api.Models.Auth;
using Benteler.WorkPlan.Api.SharedModels.Authentication.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Benteler.WorkPlan.Api.Controllers.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
		private readonly DataContext _dataContext;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
			_dataContext = dataContext;
        }
		/// <summary>
		/// Deletes the user account associated with the specified email address.
		/// </summary>
		/// <param name="email">The email address of the user account to delete. Cannot be null or empty.</param>
		/// <returns>An <see cref="IActionResult"/> indicating the result of the delete operation. Returns <see cref="OkResult"/> if
		/// the user was deleted successfully, <see cref="BadRequestObjectResult"/> if the deletion failed, or <see
		/// cref="NotFoundObjectResult"/> if no user with the specified email exists.</returns>
		[HttpDelete("DeleteAccount")]
		public async Task<IActionResult> DeleteAccount([FromQuery] string email)
		{
			User? user = await _userManager.FindByEmailAsync(email);
			if(user != null) 
			{ 
				var result = await _userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return Ok("User deleted successfully");
				}
				return BadRequest(result.Errors);
            }
            return NotFound("User with email: " + email + " not found");
        }
		/// <summary>
		/// Retrieves authorization information for a user identified by their email address, including user ID, email, and
		/// assigned roles.
		/// </summary>
		/// <param name="email">The email address of the user whose authorization information is to be retrieved. Cannot be null or empty.</param>
		/// <returns>An HTTP 200 response containing the user's authorization information if the user is found; otherwise, an HTTP 404
		/// response if no user with the specified email exists.</returns>
		[Authorize]
		[HttpGet("GetAuthorizationInfo")]
		public async Task<IActionResult> GetAuthorizationInfo() //[FromQuery]string email)
		{
			
			if(_signInManager.Context.User.Identity != null && _signInManager.Context.User.Identity.Name != null)
			{
				User? user = await _userManager.FindByNameAsync(_signInManager.Context.User.Identity.Name);
				if (user != null)
				{
					IList<string> roles = await _userManager.GetRolesAsync(user);
					return Ok(new AuthorizationInfo 
					{ 
						UserId = user.Id, 
						Email = user.Email,
						Roles = roles 
					});
				}
				return NotFound("User with name: " + _signInManager.Context.User.Identity.Name + " not found");
			}
			return Unauthorized("Can not get user identity from token.");
        }
		/*
		[HttpPost("Logout")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			if(_signInManager.Context.User != null && _signInManager.Context.User.Identity != null && _signInManager.Context.User.Identity.IsAuthenticated != true)
				return Ok($"You ({_signInManager.Context.User.Identity.Name}) are know logged out!"); 
			return Ok("You are know logged out!");
		}*/
		/* [HttpPost("Register")]
		 public async Task<ActionResult> Register([FromBody] Register dto)
		 {
			 IdentityResult result = await _userManager.CreateAsync(new User()
			 {
				 Email = dto.Email,
				 UserName = dto.Username,
				 PhoneNumber = dto.Phone
			 }, dto.Password);
			 if (!result.Succeeded)
				 return StatusCode(StatusCodes.Status500InternalServerError, ConvertErrorListIntoString(result.Errors));
			 return Ok("Please confirm your mail.");
		 }


		 private string ConvertErrorListIntoString(IEnumerable<IdentityError> errors)
		 {
			 string result= string.Empty;
			 foreach (IdentityError error in errors)
			 {
				 result += error.Code + " | " + error.Description + "\n";
			 }
			 return result;
		 }*/
		/*  [HttpPost("Login")]
		  public async Task<ActionResult> Login([FromBody] Login dto)
		  {
			  User user = await _userManager.FindByEmailAsync(dto.Email);
			  if (user is null) return NotFound("User might not exists.");

			  SignInResult result = await _signInManager.PasswordSignInAsync(
				  user, dto.Password, isPersistent: false, lockoutOnFailure: true);

			  if (result.RequiresTwoFactor)
				  return Ok(new Require2FaLogin(user.Id));

			  if (!result.Succeeded)
				  return Unauthorized();
			  var token = GenerateJwtToken(user);
			  return Ok(new { token });
		  }

		  private string GenerateJwtToken(User user)
		  {
			  var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			  var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			  var claims = new[]
			  {
				  new Claim(JwtRegisteredClaimNames.Sub,   user.Id),
				  new Claim(JwtRegisteredClaimNames.Email, user.Email),
				  new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
			  };

			  var jwt = new JwtSecurityToken(
				  issuer: _config["Jwt:Issuer"],
				  audience: _config["Jwt:Audience"],
				  claims: claims,
				  expires: DateTime.UtcNow.AddHours(1),
				  signingCredentials: creds);

			  return new JwtSecurityTokenHandler().WriteToken(jwt);
		  }

		  [HttpPost("Login2fa")]
		  public async Task<IActionResult> Login2Fa([FromBody] Login2Fa dto)
		  {
			  SignInResult result = await _signInManager.TwoFactorAuthenticatorSignInAsync(dto.Code,false,false);
			  if (!result.Succeeded)
				  return Unauthorized();

			  User user = await _userManager.FindByIdAsync(dto.UserId);
			  string token = GenerateJwtToken(user);
			  return Ok(new { token });
		  }

		  */
	}
}
