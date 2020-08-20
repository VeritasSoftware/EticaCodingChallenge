using Etica.Repository.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etica.Repository
{
    public class CarParkRepository
    {
        private readonly CarParkContext _carParkContext;

        public CarParkRepository(CarParkContext carParkContext)
        {
            _carParkContext = carParkContext;
        }

        public async Task<ICollection<RateEntity>> GetApplicableRate(DateTime start, DateTime end)
        {
            return await _carParkContext.Rates
                            .Where(r => (r.StartMin <= start && r.StartMax >= start) && (r.EndMin <= end && r.EndMax >= end))
                        .AsQueryable().ToListAsync();
        }
    }
}
