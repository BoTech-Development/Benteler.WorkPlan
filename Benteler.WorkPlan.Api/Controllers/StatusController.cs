using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Benteler.WorkPlan.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("TestConnection")]
        public ActionResult TestConnection()
        {
            return Ok("Api is running");
        }
    }
}
