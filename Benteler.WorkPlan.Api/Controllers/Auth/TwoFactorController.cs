using Benteler.WorkPlan.Api.SharedModels.Authentication;
using Benteler.WorkPlan.Api.SharedModels.Authentication.Dto;
using Benteler.WorkPlan.Api.SharedModels.Authentication.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Benteler.WorkPlan.Api.Controllers.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class TwoFactorController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public TwoFactorController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Checks if the user has already enabled 2fa  once before.
        /// 
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>When true the client should prompt for 2FA code else the client should enable 2FA.</returns>
        [HttpGet("IsTwoFactorAuthEnabled")]
        public async Task<ActionResult<bool>> IsTwoFactorAuthEnabled(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);
            if(user == null) return NotFound("User not found.");
            bool is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            return Ok(is2FaEnabled);
        }
        /// <summary>
        /// When 2FA is not enabled, this endpoint will generate a new key and URI for the user to set up their authenticator app.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Generate")]
        public async Task<IActionResult> Generate([FromQuery] string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);
            if(user == null) return NotFound("User not found.");

            string key = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            string issuer = "Benteler.WorkPlan";
            string emailEscapeDataString = Uri.EscapeDataString(user.Email);
            string uri = $"otpauth://totp/{issuer}:{emailEscapeDataString}?secret={key}&issuer={issuer}";

            return Ok(new Generated2FaInfo()
            {
                Uri = uri,
                Email = email,
                Issuer = issuer,
                Key = key
            });
        }
        /// <summary>
        /// Enables two-factor authentication for the specified user if the provided authentication code is valid.
        /// </summary>
        /// <param name="dto">An object containing the user's email address and the two-factor authentication code to verify. The email
        /// must correspond to an existing user; the code must be valid for the user.</param>
        /// <returns> Returns <see cref="OkResult"/> with a
        /// confirmation if successful; <see cref="NotFoundResult"/> if the user does not exist; or <see
        /// cref="BadRequestResult"/> if the code is invalid.</returns>
        [Authorize]
        [HttpPost("Enable")]
        public async Task<ActionResult> Enable([FromBody] Enable2Fa dto)
        {
            User? user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null) return NotFound("User not found.");
            bool valid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultAuthenticatorProvider,
                dto.Code);

            if (!valid) return BadRequest(new { message = "Invalid code." });

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return Ok(new { enabled = true });
        }

    }
}
