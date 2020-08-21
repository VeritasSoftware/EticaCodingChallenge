using Etica.Models;
using Etica.Repository;
using System;
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

        public async Task<RateResponseModel> GetApplicableRate(DateTime start, DateTime end)
        {
            var rate = await _repository.GetApplicableRate(start, end);

            if (rate == null)
                return null;

            var rateResponse = _mapper.MapRate(rate);

            return rateResponse;
        }
    }
}
