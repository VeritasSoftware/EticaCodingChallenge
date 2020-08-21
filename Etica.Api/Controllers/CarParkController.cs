using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Etica.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Etica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParkController : ControllerBase
    {
        private readonly ICarParkManager _manager;

        public CarParkController(ICarParkManager manager)
        {
            _manager = manager;
        }

        [HttpGet("{start}/{end}")]
        public async Task<IActionResult> CalculateRate(string start, string end)
        {
            start = HttpUtility.UrlDecode(start);
            end = HttpUtility.UrlDecode(end);
            return Ok(await _manager.GetApplicableRate(start, end));
        }
    }
}
