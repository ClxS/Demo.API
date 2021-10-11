using Demo.Domain;
using Demo.Infrastructure;
using Demo.Tests.Utility;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Demo.Tests.UnitTests.Infrastructure
{
    public class EFGuitarRepositoryTests
    {
        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var guitarDbSet = GetGuitars().AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            await repository.CreateAsync(new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE"));
            
            // Assert
            guitarDbSet.Verify(x => x.AddAsync(It.IsAny<Guitar>(), default).Result, Times.Once);
        }

        [Fact]
        public async Task ReadWhenGuitarExistsAsync()
        {
            // Arrange
            var guitarDbSet = GetGuitars().AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = await repository.ReadAsync(1);

            // Assert
            Assert.NotNull(guitar);
            Assert.Equal(1, guitar.Id);
        }

        [Fact]
        public async Task ReadWhenGuitarDoesNotExistAsync()
        {
            // Arrange
            var guitarDbSet = GetGuitars().AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = await repository.ReadAsync(5);

            // Assert
            Assert.Null(guitar);
        }

        [Fact]
        public async Task ReadAllWhenGuitarsExist()
        {
            // Arrange
            var guitarDbSet = GetGuitars().AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitars = await repository.ReadAllAsync();

            // Assert
            Assert.NotEmpty(guitars);
            Assert.Equal(3, guitars.Count());
        }

        [Fact]
        public async Task ReadAllWhenNoGuitarsExist()
        {
            // Arrange
            var guitarDbSet = new List<Guitar>().AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitars = await repository.ReadAllAsync();

            // Assert
            Assert.Empty(guitars);
        }

        [Fact]
        public async Task FindWhenGuitarExistsAsync()
        {
            // Arrange
            var guitarList = GetGuitars();
            var guitarDbSet = guitarList.AsQueryable().BuildMockDbSet();
            guitarDbSet.SetupFindAsync(guitarList);
            var dbContext = GetMockedDbContext(guitarDbSet);            
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = await repository.FindAsync(1);

            // Assert
            Assert.NotNull(guitar);
            Assert.Equal(1, guitar.Id);
        }

        [Fact]
        public async Task FindWhenGuitarDoesNotExistAsync()
        {
            // Arrange
            var guitarList = GetGuitars();
            var guitarDbSet = guitarList.AsQueryable().BuildMockDbSet();
            guitarDbSet.SetupFindAsync(guitarList);
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = await repository.FindAsync(5);

            // Assert
            Assert.Null(guitar);
        }

        [Fact]
        public void Update()
        {
            // Arrange
            var guitarList = GetGuitars();
            var guitarDbSet = guitarList.AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = guitarList.First();
            guitar.Make = "Gibson2";
            guitar.Model = "J-46";
            repository.Update(guitar);

            // Assert
            guitarDbSet.Verify(x => x.Update(It.IsAny<Guitar>()), Times.Once);
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var guitarList = GetGuitars();
            var guitarDbSet = guitarList.AsQueryable().BuildMockDbSet();
            var dbContext = GetMockedDbContext(guitarDbSet);
            var repository = new EFGuitarRepository(dbContext.Object);

            // Act
            var guitar = guitarList.First();
            repository.Delete(guitar);

            // Assert
            guitarDbSet.Verify(x => x.Remove(It.IsAny<Guitar>()), Times.Once);
        }

        private static List<Guitar> GetGuitars()
        {
            return new List<Guitar>
            {
                new Guitar(GuitarType.Acoustic, 6, "Gibson", "J-45") { Id = 1 },
                new Guitar(GuitarType.Electric, 6, "Fender", "Stratocaster") { Id = 2 },
                new Guitar(GuitarType.Electric, 6, "Gibson", "Les Paul") { Id = 3 }
            };
        }

        private static Mock<DemoContext> GetMockedDbContext(Mock<DbSet<Guitar>> guitarDbSet)
        {           
            var dbContext = new Mock<DemoContext>();
            dbContext.Setup(x => x.Guitar).Returns(guitarDbSet.Object);

            return dbContext;
        }
    }
}