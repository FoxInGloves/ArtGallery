using ArtGallery.Models.Structs.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Tests.Controllers.ArtController;

public class CatalogTests
{
    [Fact]
    public async Task ReturnsViewIsNotNull()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Catalog();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task IsReturnsValidView()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Catalog();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task IsReturnsValidModel()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Catalog();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ArtDto>>(viewResult.Model);
        Assert.NotEmpty(model);
    }
}