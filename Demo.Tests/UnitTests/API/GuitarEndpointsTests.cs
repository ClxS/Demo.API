using Demo.Core.Services;
using System.Threading.Tasks;
using Moq;
using Demo.Core.DTOs;
using Demo.Domain;
using Demo.API.Endpoints;
using Demo.API.Validators;
using Xunit;
using System.Collections.Generic;
using Demo.Core.Repositories;

namespace Demo.Tests.UnitTests.API
{
    /// <summary>
    /// *NOTE* Microsoft.AspNetCore.Http.Result is internal, so it is difficult to assert the result type we get back from an endpoint call.
    /// So, for the time being until a better solution is found, we are asserting on the name of the result type.
    /// </summary>
    public class GuitarEndpointsTests
    {
        [Fact]
        public async Task CreateSuccessfulAsync()
        {
            // Arrange
            var guitarDto = new GuitarDto
            {
                Id = 1,
                GuitarType = GuitarType.AcousticElectric,
                MaxNumberOfStrings = 6,
                Make = "Taylor",
                Model = "314-CE"
            };

            var guitarService = new Mock<IGuitarService>();
            guitarService.Setup(x => x.CreateAsync(It.IsAny<GuitarDto>())).ReturnsAsync(guitarDto);

            // Act
            var result = await GuitarEndpoints.CreateAsync(guitarService.Object, new GuitarValidator(), guitarDto);

            // Assert
            Assert.True(result.GetType().Name == "CreatedResult");
        }

        [Fact]
        public async Task CreateWithBadRequestResultAsync()
        {
            // Arrange
            var guitarService = new Mock<IGuitarService>();

            // Act
            var result = await GuitarEndpoints.CreateAsync(guitarService.Object, new GuitarValidator(), new GuitarDto());

            // Assert
            Assert.True(result.GetType().Name == "BadRequestObjectResult");
        }

        [Fact]
        public async Task ReadSuccessfulAsync()
        {
            // Arrange
            var guitarService = new Mock<IGuitarService>();
            guitarService.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(new GuitarWithStringsDto());

            // Act
            var result = await GuitarEndpoints.ReadAsync(guitarService.Object, 1);

            // Assert
            Assert.True(result.GetType().Name == "OkObjectResult");
        }

        [Fact]
        public async Task ReadWithNotFoundResultAsync()
        {
            // Arrange
            var guitarService = new Mock<IGuitarService>();
            guitarService.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync((GuitarWithStringsDto)null);

            // Act
            var result = await GuitarEndpoints.ReadAsync(guitarService.Object, 1);

            // Assert
            Assert.True(result.GetType().Name == "NotFoundObjectResult");
        }

        [Fact]
        public async Task ReadAllAsync()
        {
            // Arrange
            var guitarService = new Mock<IGuitarService>();
            guitarService.Setup(x => x.ReadAllAsync()).ReturnsAsync(new List<GuitarWithStringsDto>());

            // Act
            var result = await GuitarEndpoints.ReallAllAsync(guitarService.Object);

            // Assert
            Assert.True(result.GetType().Name == "OkObjectResult");
        }

        [Fact]
        public async Task StringSuccessfulAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };
            var guitarTuningDto = new GuitarTuningDto
            {
                GuitarId = 1,
                GuitarStrings = new List<GuitarStringDto> { new GuitarStringDto { Number = 1, Gauge = "010", Tuning = "E" } }
            };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(guitar);

            // Act
            var result = await GuitarEndpoints.StringAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "NoContentResult");
            repository.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task StringWithBadRequestResultAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };
            var guitarTuningDto = new GuitarTuningDto { GuitarId = 1 };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(guitar);

            // Act
            var result = await GuitarEndpoints.StringAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "BadRequestObjectResult");
        }

        [Fact]
        public async Task StringWithNotFoundResultAsync()
        {
            // Arrange
            var guitarTuningDto = new GuitarTuningDto { GuitarId = 1 };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync((Guitar)null);

            // Act
            var result = await GuitarEndpoints.StringAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "NotFoundObjectResult");
        }

        [Fact]
        public async Task TuneSuccessfulAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };
            var guitarTuningDto = new GuitarTuningDto
            {
                GuitarId = 1,
                GuitarStrings = new List<GuitarStringDto> { new GuitarStringDto { Number = 1, Gauge = "010", Tuning = "E" } }
            };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(guitar);

            // Act
            var result = await GuitarEndpoints.TuneAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "NoContentResult");
            repository.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task TuneWithBadRequestResultAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };
            var guitarTuningDto = new GuitarTuningDto { GuitarId = 1 };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(guitar);

            // Act
            var result = await GuitarEndpoints.TuneAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "BadRequestObjectResult");
        }

        [Fact]
        public async Task TuneWithNotFoundResultAsync()
        {
            // Arrange
            var guitarTuningDto = new GuitarTuningDto { GuitarId = 1 };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync((Guitar)null);

            // Act
            var result = await GuitarEndpoints.TuneAsync(repository.Object, new GuitarStringTuningValidator(), guitarTuningDto);

            // Assert
            Assert.True(result.GetType().Name == "NotFoundObjectResult");
        }

        [Fact]
        public async Task UpdateSuccessfulAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };
            var guitarDto = new GuitarDto { Id = 1, Make = "Taylor2", Model = "314-CE2" };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(guitar);

            var guitarService = new Mock<IGuitarService>();
            guitarService.Setup(x => x.UpdateAsync(It.IsAny<Guitar>(), It.IsAny<GuitarDto>()));

            // Act
            var result = await GuitarEndpoints.UpdateAsync(guitarService.Object, repository.Object, guitarDto);

            // Assert
            Assert.True(result.GetType().Name == "NoContentResult");
        }

        [Fact]
        public async Task UpdateWithNotFoundResultAsync()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();            
            repository.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync((Guitar)null);

            var guitarService = new Mock<IGuitarService>();

            // Act
            var result = await GuitarEndpoints.UpdateAsync(guitarService.Object, repository.Object, new GuitarDto());

            // Assert
            Assert.True(result.GetType().Name == "NotFoundObjectResult");
        }

        [Fact]
        public async Task DeleteSuccessfulAsync()
        {
            // Arrange
            var guitar = new Guitar(GuitarType.AcousticElectric, 6, "Taylor", "314-CE") { Id = 1 };

            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync(guitar);
            repository.Setup(x => x.Delete(It.IsAny<Guitar>()));

            // Act
            var result = await GuitarEndpoints.DeleteAsync(repository.Object, 1);

            // Assert
            Assert.True(result.GetType().Name == "NoContentResult");
        }

        [Fact]
        public async Task DeleteWithNotFoundResult()
        {
            // Arrange
            var repository = new Mock<IGuitarRepository>();
            repository.Setup(x => x.ReadAsync(It.IsAny<int>())).ReturnsAsync((Guitar)null);

            // Act
            var result = await GuitarEndpoints.DeleteAsync(repository.Object, 1);

            // Assert            
            Assert.True(result.GetType().Name == "NotFoundObjectResult");
        }
    }
}