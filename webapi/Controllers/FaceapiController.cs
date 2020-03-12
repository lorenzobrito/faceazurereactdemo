using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaceapiController : ControllerBase
    {
        private static readonly List<User> Picture = new List<User>();

        private readonly ILogger<WeatherForecastController> _logger;

        public FaceapiController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
      
            return Ok(Picture);
        }
         [HttpPost]
        public  IActionResult PostAsync([FromBody]User user)
        {
           
            Picture.Add(user);
            return Ok();
        }
    }
}
