using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Benteler.WorkPlan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("GetValue"), Authorize]
        public ActionResult<string> GetValue()
        {
            return "You are authorized!";
        }
    }
}
