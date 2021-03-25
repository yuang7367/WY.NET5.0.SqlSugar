using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WY.WebApi.Filters;

namespace WY.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var m = "";
                if (string.IsNullOrEmpty(m))
                    throw new ArgumentNullException("");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Json("ok");
        }
    }
}
