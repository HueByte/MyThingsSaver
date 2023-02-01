using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Core.Entities;

namespace App.Controllers
{
    public class DebugController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        public DebugController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("HandledException")]
        public IActionResult HandledException()
        {
            if (!_env.IsDevelopment()) return NotFound();
            throw new HandledException("HandledException");
        }

        [HttpGet("UnHandledException")]
        public IActionResult UnHandledException()
        {
            if (!_env.IsDevelopment()) return NotFound();
            throw new Exception("UnHandledException");
        }
    }
}