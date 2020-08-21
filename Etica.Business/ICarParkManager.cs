using Etica.Models;
using System;
using System.Threading.Tasks;

namespace Etica.Business
{
    public interface ICarParkManager
    {
        Task<RateResponseModel> GetApplicableRate(DateTime start, DateTime end);
    }
}