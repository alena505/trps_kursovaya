using Microsoft.AspNetCore.Mvc;

namespace BooleanCompletenessBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { message = "Привет из C# backend!" });
    }
}
