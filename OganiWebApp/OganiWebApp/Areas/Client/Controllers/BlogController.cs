using Microsoft.AspNetCore.Mvc;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("blog")]
    public class BlogController : Controller
    {
        [HttpGet("index", Name = "client-blog-index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
