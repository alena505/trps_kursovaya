using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooleanCompletenessBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { message = "Привет из C# backend!" });
    }
}
