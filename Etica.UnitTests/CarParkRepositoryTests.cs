using Etica.Repository;
using Etica.Repository.Entitites;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Etica.UnitTests
{
    public class CarParkRepositoryTests : IDisposable
    {
        public CarParkRepositoryTests()
        {
            using (var context = new CarParkContext())
            {
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task GetApplicableRate()
        {
            //Arrange
            using (var context = new CarParkContext())
            {
                context.Rates.Add(new RateEntity
                {
                    Name = "Early Bird",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekday,
                    EntryMin = DateTime.ParseExact("06:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EntryMax = DateTime.ParseExact("09:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    ExitMin = DateTime.ParseExact("03:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    ExitMax = DateTime.ParseExact("11:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    Price = 13
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Night Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekday,
                    EntryMin = DateTime.ParseExact("06:00:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EntryMax = DateTime.ParseExact("11:59:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    ExitMin = DateTime.ParseExact("03:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1),
                    ExitMax = DateTime.ParseExact("11:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1),
                    Price = 6.5
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Weekend Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekend,
                    EntryMin = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    ExitMax = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
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

            //Early Bird
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("20/08/2020 08:00:00 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("20/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Early Bird");
                Assert.True(rate.Price == 13);
            }

            //Night Rate
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("20/08/2020 07:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("21/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Night Rate");
                Assert.True(rate.Price == 6.5);
            }

            //Weekend Rate
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("22/08/2020 07:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("23/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Weekend Rate");
                Assert.True(rate.Price == 10);
            }

            //Night Rate instead of Weekend rate
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("21/08/2020 11:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("22/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Night Rate");
                Assert.True(rate.Price == 6.5);
            }

            //Standard Rate - Hourly
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("20/08/2020 09:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("20/08/2020 11:35:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Standard Rate");
                Assert.True(rate.Price == 15);
            }

            //Standard Rate - Daily
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var entry = DateTime.ParseExact("20/08/2020 09:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var exit = DateTime.ParseExact("22/08/2020 11:35:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRateAsync(entry, exit);

                //Assert
                Assert.True(rate.Name == "Standard Rate");
                Assert.True(rate.Price == 60);
            }
        }

        public void Dispose()
        {
            using (var context = new CarParkContext())
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
