using Etica.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace Etica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarParkController : ControllerBase
    {
        private readonly ICarParkManager _manager;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="manager"><see cref="ICarParkManager"/></param>
        public CarParkController(ICarParkManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Calculate Rate endpoint
        /// </summary>
        /// <param name="entry">The entry time</param>
        /// <param name="exit">The exit time</param>
        /// <returns></returns>
        [HttpGet("{entry}/{exit}")]
        public async Task<IActionResult> CalculateRateAsync(string entry, string exit)
        {
            entry = HttpUtility.UrlDecode(entry);
            exit = HttpUtility.UrlDecode(exit);
            return Ok(await _manager.GetApplicableRateAsync(entry, exit));
        }
    }
}
