using Benteler.WorkPlan.Api.Data;
using Benteler.WorkPlan.Api.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Benteler.WorkPlan.Api.Controllers.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public VerificationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet("IsEmailVerified")]
        public ActionResult<bool> IsEmailVerified([FromQuery]string email)
        {
            if (_dataContext.Users.Count() == 0)
                return NotFound("No users in db.");
            User? user = _dataContext.Users.First(u => u.Email == email);
            if (user != null)
                return Ok(user.EmailConfirmed);
            return NotFound($"User with Email: {email} does not exsist in the db.");
        }
    }
}