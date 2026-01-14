using Microsoft.AspNetCore.Mvc;

namespace SportMafiaSpiel.Controllers
{
    [ApiController]
    [Route("/")] // Корневой путь
    public class HomeController : ControllerBase
    {
        // GET /
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("SportMafiaSpiel Backend is running!");
        }
    }
}
