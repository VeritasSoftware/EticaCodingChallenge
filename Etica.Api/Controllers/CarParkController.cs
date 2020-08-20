using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Etica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParkController : ControllerBase
    {
        [HttpGet("{start}/{end}")]
        public void CalculateRate(DateTimeOffset start, DateTimeOffset end)
        {

        }
    }
}
