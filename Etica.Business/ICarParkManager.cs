using Etica.Models;
using System.Threading.Tasks;

namespace Etica.Business
{
    public interface ICarParkManager
    {
        Task<RateResponseModel> GetApplicableRateAsync(string entry, string exit);
    }
}