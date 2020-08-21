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

        /// <summary>
        /// Get applicable rate
        /// </summary>
        /// <param name="entry">The entry date-time</param>
        /// <param name="exit">The exit date-time</param>
        /// <returns><see cref="BaseRateEntity"/></returns>
        public async Task<BaseRateEntity> GetApplicableRateAsync(DateTime entry, DateTime exit)
        {
            //Check if weekdays flat rate applies
            var dailyRate = await _carParkContext.Rates.SingleOrDefaultAsync(r => (weekDays.Contains(entry.DayOfWeek) && r.RateDay == RateDay.Weekday && (TimeSpan.Compare(r.EntryMin.TimeOfDay, entry.TimeOfDay) <= 0 && TimeSpan.Compare(r.EntryMax.TimeOfDay, entry.TimeOfDay) >= 0) && (TimeSpan.Compare(r.ExitMin.TimeOfDay, exit.TimeOfDay) <= 0 && TimeSpan.Compare(r.ExitMax.TimeOfDay, exit.TimeOfDay) >= 0)));

            if (dailyRate != null)
                return dailyRate;
            
            //Check if weekend flat rate applies
            var weekendRate = await _carParkContext.Rates.SingleOrDefaultAsync(r => weekEnd.Contains(entry.DayOfWeek) && r.RateDay == RateDay.Weekend && (TimeSpan.Compare(r.EntryMin.TimeOfDay, entry.TimeOfDay) < 0) && (TimeSpan.Compare(r.ExitMax.TimeOfDay, exit.TimeOfDay) < 0));

            if (weekendRate == null)
            {
                //Calculate hourly/daily standard rate
                var duration = (exit - entry).TotalHours;

                var rate = await _carParkContext.HourlyRates.SingleOrDefaultAsync(r => duration >= r.DurationMin && duration < r.DurationMax);

                if (rate.IsDaily)
                {
                    var durationInDays = Math.Ceiling((exit - entry).TotalDays);
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
