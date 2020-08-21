using Etica.Models;
using Etica.Repository;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Etica.Business
{
    public class CarParkManager : ICarParkManager
    {
        private readonly ICarParkRepository _repository;
        private readonly IMappingService _mapper;

        public CarParkManager(ICarParkRepository repository, IMappingService mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get applicable rate
        /// </summary>
        /// <param name="entry">The entry date-time</param>
        /// <param name="exit">The exit date-time</param>
        /// <returns><see cref="RateResponseModel"/></returns>
        public async Task<RateResponseModel> GetApplicableRateAsync(string entry, string exit)
        {       
            //Parse dates
            var dtEntry = DateTime.ParseExact(entry, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            var dtExit = DateTime.ParseExact(exit, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

            //if Exit date-time before Entry, throw Exception
            if (dtExit < dtEntry)
            {
                throw new InvalidProgramException("Exit date-time should be on or after Entry");
            }

            var rate = await _repository.GetApplicableRateAsync(dtEntry, dtExit);

            if (rate == null)
                return null;

            //Map data entity to response entity
            var rateResponse = _mapper.MapRate(rate);

            return rateResponse;
        }
    }
}
