using Benteler.WorkPlan.Api.Data;
using Benteler.WorkPlan.Api.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Benteler.WorkPlan.Api.Controllers.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public SmsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPatch("AddPhoneNumberToUser")]
        public ActionResult AddPhoneNumberToUser([FromQuery] string email, [FromQuery] string phoneNumber)
        {
            User? user = _dataContext.Users.First(u => u.Email == email);
            if (user == null)
                return NotFound($"User with Email: {email} does not exsist in the db.");
            user.PhoneNumber = phoneNumber;
            _dataContext.SaveChanges();
            return Ok($"Added following PhoneNumber: {phoneNumber} to ");
        }
        [HttpPatch("SendVerificationSms")]
        public ActionResult SendVerificationSms(string email)
        {
            return Ok();
        }
    }
}
