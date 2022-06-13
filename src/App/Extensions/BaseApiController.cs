using Microsoft.AspNetCore.Mvc;

namespace App.Extensions
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : Controller { }
}