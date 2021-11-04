using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace API.Controllers
{
    // Controller is the base class for MVC. Angular is our view, so this controller
    // will handle all the routes that the web api does not understand by sending them to the
    // angular app.
    public class FallbackController : Controller
    {
        public ActionResult Index()
            => PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
    }
}
