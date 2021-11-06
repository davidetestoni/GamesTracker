using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// The base api controller. Has api/[controller] as route.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
