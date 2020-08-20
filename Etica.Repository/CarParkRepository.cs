using Etica.Repository.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etica.Repository
{
    public class CarParkRepository : ICarParkRepository
    {
        private readonly CarParkContext _carParkContext;

        private readonly IEnumerable<DayOfWeek> weekDays = new List<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        };

        private readonly IEnumerable<DayOfWeek> weekEnd = new List<DayOfWeek>
        {
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };


        public CarParkRepository(CarParkContext carParkContext)
        {
            _carParkContext = carParkContext;
        }

        public async Task<BaseRateEntity> GetApplicableRate(DateTime start, DateTime end)
        {
            var dailyRate = await _carParkContext.Rates.SingleOrDefaultAsync(r => (weekDays.Contains(start.DayOfWeek) && r.RateDay == RateDay.Weekday && (TimeSpan.Compare(r.StartMin.TimeOfDay, start.TimeOfDay) <= 0 && TimeSpan.Compare(r.StartMax.TimeOfDay, start.TimeOfDay) >= 0) && (TimeSpan.Compare(r.EndMin.TimeOfDay, end.TimeOfDay) <= 0 && TimeSpan.Compare(r.EndMax.TimeOfDay, end.TimeOfDay) >= 0)));

            if (dailyRate != null)
                return dailyRate;
            
            var weekendRate = await _carParkContext.Rates.SingleOrDefaultAsync(r => weekEnd.Contains(start.DayOfWeek) && r.RateDay == RateDay.Weekend && (TimeSpan.Compare(r.StartMin.TimeOfDay, start.TimeOfDay) < 0) && (TimeSpan.Compare(r.EndMax.TimeOfDay, end.TimeOfDay) < 0));

            if (weekendRate == null)
            {
                var duration = (end - start).TotalHours;

                var rate = await _carParkContext.HourlyRates.SingleOrDefaultAsync(r => duration >= r.DurationMin && duration < r.DurationMax);

                if (rate.IsDaily)
                {
                    var durationInDays = Math.Ceiling((end - start).TotalDays);
                    var amount = rate.Price * durationInDays;
                    rate.Price = amount;

                    return rate;
                }

                return rate;
            }

            return weekendRate;
        }
    }
}
