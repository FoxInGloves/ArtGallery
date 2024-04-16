using ArtGallery.Models.Structs.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Tests.Controllers.ArtController;

public class ArtistTests
{
    [Fact]
    public async Task ReturnsViewIsNotNull()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Artist("C66B0FFE-1C72-4C89-973D-32CADCB48672");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ReturnsViewIsNotNullWithInvalidParameter()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Artist("");

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task IsReturnsValidView()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Artist("C66B0FFE-1C72-4C89-973D-32CADCB48672");

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }
    
    [Fact]
    public async Task ReturnsViewIsValidWithInvalidParameter()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Artist("");

        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("/Arts/Error/", redirectResult.Url);
    }
    
    [Fact]
    public async Task IsReturnsValidModel()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Artist("C66B0FFE-1C72-4C89-973D-32CADCB48672");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<ArtistDto>(viewResult.Model);
        Assert.NotNull(model);
    }
}