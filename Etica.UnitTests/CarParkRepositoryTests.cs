using Etica.Repository;
using Etica.Repository.Entitites;
using System;
using System.Globalization;
using System.Linq;
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
        public async Task GetApplicableRate_EarlyBird()
        {
            using (var context = new CarParkContext())
            {
                context.Rates.Add(new RateEntity
                {
                    Name = "Early Bird",
                    Type = RateType.Flat,
                    StartMin = DateTime.ParseExact("06:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    StartMax = DateTime.ParseExact("09:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMin = DateTime.ParseExact("3:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndMax = DateTime.ParseExact("11:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture)
                });

                await context.SaveChangesAsync();                
            }


            using (var context = new CarParkContext())
            {
                var repository = new CarParkRepository(context);

                var start = DateTime.ParseExact("08:00:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact("04:30:00 PM", "h:mm:ss tt", CultureInfo.InvariantCulture);

                var rates = await repository.GetApplicableRate(start, end);

                var rate = rates.First();
                Assert.True(rate.Name == "Early Bird");
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
