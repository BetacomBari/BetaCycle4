using Microsoft.AspNetCore.Mvc;

namespace BetaCycle4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
