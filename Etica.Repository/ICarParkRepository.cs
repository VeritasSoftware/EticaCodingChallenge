using Etica.Repository.Entitites;
using System;
using System.Threading.Tasks;

namespace Etica.Repository
{
    public interface ICarParkRepository
    {
        Task<BaseRateEntity> GetApplicableRateAsync(DateTime start, DateTime end);
    }
}