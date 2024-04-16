using ArtGallery.Models.Structs.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Tests.Controllers.ArtController;

public class PictureTests
{
    [Fact]
    public async Task ReturnsViewIsNotNull()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Picture("41B8A023-8624-4EC6-A324-28ACDBCCB23D");

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task ReturnsViewIsNotNullWithInvalidParameter()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Picture("");

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task ReturnsViewIsValid()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Picture("41B8A023-8624-4EC6-A324-28ACDBCCB23D");

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }
    
    [Fact]
    public async Task ReturnsViewIsValidWithInvalidParameter()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Picture("");

        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("/Arts/Error/", redirectResult.Url);
    }
    
    [Fact]
    public async Task IsReturnsValidModel()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Picture("41B8A023-8624-4EC6-A324-28ACDBCCB23D");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<ArtDto>(viewResult.Model);
        Assert.NotNull(model);
    }
}