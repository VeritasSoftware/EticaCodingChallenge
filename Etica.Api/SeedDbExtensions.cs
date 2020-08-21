using Etica.Repository;
using Etica.Repository.Entitites;
using Microsoft.AspNetCore.Builder;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Etica.Api
{
    public static class SeedDbExtensions
    {        
        public async static Task Seed(this IApplicationBuilder builder)
        {
            using (var context = new CarParkContext())
            {
                await context.Database.EnsureDeletedAsync();

                await context.Database.EnsureCreatedAsync();

                context.Rates.Add(new RateEntity
                {
                    Name = "Early Bird",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekday,
                    StartMin = DateTime.ParseExact("06:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    StartMax = DateTime.ParseExact("09:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMin = DateTime.ParseExact("03:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMax = DateTime.ParseExact("11:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    Price = 13
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Night Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekday,
                    StartMin = DateTime.ParseExact("06:00:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    StartMax = DateTime.ParseExact("11:59:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMin = DateTime.ParseExact("03:30:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1),
                    EndMax = DateTime.ParseExact("11:30:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1),
                    Price = 6.5
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Weekend Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekend,
                    StartMin = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMax = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    Price = 10
                });

                context.HourlyRates.Add(new HourlyRateEntity
                {
                    Name = "Standard Rate",
                    DurationMin = 0,
                    DurationMax = 1,
                    IsHourly = true,
                    Price = 5
                });

                context.HourlyRates.Add(new HourlyRateEntity
                {
                    Name = "Standard Rate",
                    DurationMin = 1,
                    DurationMax = 2,
                    IsHourly = true,
                    Price = 10
                });

                context.HourlyRates.Add(new HourlyRateEntity
                {
                    Name = "Standard Rate",
                    DurationMin = 2,
                    DurationMax = 3,
                    IsHourly = true,
                    Price = 15
                });

                context.HourlyRates.Add(new HourlyRateEntity
                {
                    Name = "Standard Rate",
                    DurationMin = 3,
                    DurationMax = int.MaxValue,
                    IsDaily = true,
                    Price = 20
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
