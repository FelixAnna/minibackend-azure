using Microsoft.AspNetCore.Mvc;

namespace BOTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "ok";
        }
    }
}
