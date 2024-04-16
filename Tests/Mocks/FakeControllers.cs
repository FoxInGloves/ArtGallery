using ArtGallery.Controllers;
using ArtGallery.Data.Abstractions;
using ArtGallery.Models.Structs.Entity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Mocks;

internal static class FakeControllers
{
    internal static ArtsController GetArtsController()
    {
        var mockLogger = new Mock<ILogger<ArtsController>>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(x => x.ArtRepository.GetAsync(null, null))
            .ReturnsAsync(new[] { new Art() });
        mockUnitOfWork.Setup(x => x.ArtRepository.GetByIdAsync("41B8A023-8624-4EC6-A324-28ACDBCCB23D"))
            .ReturnsAsync(new Art()
            {
                Id = "41B8A023-8624-4EC6-A324-28ACDBCCB23D"
            });
        mockUnitOfWork.Setup(x => x.GenresRepository.GetAsync(null, null))
            .ReturnsAsync(new[] { new Genre() });
        mockUnitOfWork.Setup(x => x.ArtistRepository.GetByIdAsync("C66B0FFE-1C72-4C89-973D-32CADCB48672"))
            .ReturnsAsync(new Artist()
            {
                Country = "TestCountry",
                Description = "NotFound",
                IconPath = "",
                Id = "C66B0FFE-1C72-4C89-973D-32CADCB48672",
                Name = "TestArtist"
            });
        var controller = new ArtsController(mockLogger.Object, mockUnitOfWork.Object);

        return controller;
    }
}