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

        public async Task<RateResponseModel> GetApplicableRate(string start, string end)
        {            
            var dtStart = DateTime.ParseExact(start, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            var dtEnd = DateTime.ParseExact(end, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

            var rate = await _repository.GetApplicableRate(dtStart, dtEnd);

            if (rate == null)
                return null;

            var rateResponse = _mapper.MapRate(rate);

            return rateResponse;
        }
    }
}
