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
                    StartMin = DateTime.ParseExact("06:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    StartMax = DateTime.ParseExact("09:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMin = DateTime.ParseExact("03:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMax = DateTime.ParseExact("11:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture)
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Night Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekday,
                    StartMin = DateTime.ParseExact("06:00:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    StartMax = DateTime.ParseExact("11:59:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMin = DateTime.ParseExact("03:30:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1),
                    EndMax = DateTime.ParseExact("11:30:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture).AddDays(1)
                });

                context.Rates.Add(new RateEntity
                {
                    Name = "Weekend Rate",
                    Type = RateType.Flat,
                    RateDay = RateDay.Weekend,
                    StartMin = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMax = DateTime.ParseExact("12:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                });

                await context.SaveChangesAsync();
            }

            //Early Bird
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var start = DateTime.ParseExact("20/08/2020 08:00:00 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact("20/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRate(start, end);

                //Assert
                Assert.True(rate.Name == "Early Bird");
            }

            //Night Rate
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var start = DateTime.ParseExact("20/08/2020 07:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact("21/08/2020 04:30:00 AM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRate(start, end);

                //Assert
                Assert.True(rate.Name == "Night Rate");
            }

            //Weekend Rate
            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var start = DateTime.ParseExact("22/08/2020 07:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact("23/08/2020 04:30:00 PM", "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                //Act
                var rate = await repository.GetApplicableRate(start, end);

                //Assert
                Assert.True(rate.Name == "Weekend Rate");

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
