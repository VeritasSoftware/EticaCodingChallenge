using AutoMapper;
using Etica.Models;
using Etica.Repository.Entitites;

namespace Etica.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseRateEntity, RateResponseModel>();
        }
    }
}
