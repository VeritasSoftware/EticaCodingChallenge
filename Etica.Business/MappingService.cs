using AutoMapper;
using Etica.Models;
using Etica.Repository.Entitites;

namespace Etica.Business
{
    public interface IMappingService
    {
        RateResponseModel MapRate(BaseRateEntity rate);
    }

    public class MappingService : IMappingService
    {
        private readonly IMapper _mapper;

        public MappingService()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        public RateResponseModel MapRate(BaseRateEntity rate)
        {
            return _mapper.Map<RateResponseModel>(rate);
        }
    }
}
