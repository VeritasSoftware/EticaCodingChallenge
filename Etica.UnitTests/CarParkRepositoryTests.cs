using Etica.Repository;
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
                //Arrange
                context.Seed().ConfigureAwait(true);
            }
        }

        [Fact]
        public async Task GetApplicableRateAsync_EarlyBird()
        {            
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
        }

        [Fact]
        public async Task GetApplicableRateAsync_NightRate()
        {
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
        }

        [Fact]
        public async Task GetApplicableRateAsync_WeekendRate()
        {
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
        }

        [Fact]
        public async Task GetApplicableRateAsync_StandardRate_Hourly()
        {
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
        }

        [Fact]
        public async Task GetApplicableRateAsync_StandardRate_Daily()
        {
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
