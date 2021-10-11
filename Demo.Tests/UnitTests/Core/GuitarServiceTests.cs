using Demo.Core.DTOs;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Demo.Domain;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Demo.Tests.UnitTests.Core
{
    public class GuitarServiceTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var guitarDto = new GuitarDto
            {
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.CreateAsync(It.IsAny<Guitar>()));
            var guitarService = new GuitarService(repository.Object);

            // Act
            await guitarService.CreateAsync(guitarDto);

            // Assert
            repository.Verify(x => x.CreateAsync(It.IsAny<Guitar>()), Times.Once());
            repository.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task ReadWhenGuitarExists()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE"));
            var guitarService = new GuitarService(repository.Object);

            // Act
            var guitar = await guitarService.ReadAsync(1);

            // Assert
            Assert.NotNull(guitar);
        }

        [Fact]
        public async Task ReadWhenGuitarDoesNotExist()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync((Guitar)null);
            var guitarService = new GuitarService(repository.Object);

            // Act
            var guitar = await guitarService.ReadAsync(1);

            // Assert
            Assert.Null(guitar);
        }

        [Fact]
        public async Task ReadAllWhenGuitarsExist()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAllAsync()).ReturnsAsync(
                new List<Guitar>
                {
                    new Guitar(GuitarType.Acoustic, 6, "Gibson", "J-45") { Id = 1 },
                    new Guitar(GuitarType.Electric, 6, "Fender", "Stratocaster") { Id = 2 },
                    new Guitar(GuitarType.Electric, 6, "Gibson", "Les Paul") { Id = 3 }
                });
            var guitarService = new GuitarService(repository.Object);

            // Act
            var guitarWithStringsDtos = await guitarService.ReadAllAsync();

            // Assert
            Assert.NotEmpty(guitarWithStringsDtos);
            Assert.Equal(3, guitarWithStringsDtos.Count());
        }

        [Fact]
        public async Task ReadAllWhenNoGuitarsExist()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAllAsync()).ReturnsAsync(new List<Guitar>());
            var guitarService = new GuitarService(repository.Object);

            // Act
            var guitarWithStringsDtos = await guitarService.ReadAllAsync();

            // Assert
            Assert.Empty(guitarWithStringsDtos);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.Update(It.IsAny<Guitar>()));
            var guitarService = new GuitarService(repository.Object);

            // Act
            var guitar = new Guitar(GuitarType.Acoustic, 6, "Gibson", "J-45") { Id = 1 };
            var guitarDto = new GuitarDto { Id = 1, Make = "Gibson2", Model = "J-45a" };
            await guitarService.UpdateAsync(guitar, guitarDto);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<Guitar>()), Times.Once());
            repository.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.Equal("Gibson2", guitar.Make);
            Assert.Equal("J-45a", guitar.Model);
        }
    }
}