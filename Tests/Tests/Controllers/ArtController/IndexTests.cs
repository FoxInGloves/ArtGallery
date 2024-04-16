using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Tests.Controllers.ArtController;

public class IndexTests
{
    [Fact]
    public async Task ReturnsViewIsNotNull()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Index();

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task IsReturnsValidView()
    {
        var controller = Mocks.FakeControllers.GetArtsController();
        
        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }

    [Fact]
    public async Task IsReturnsValidModel()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<IndexViewModel>(viewResult.Model);
    }

    [Fact]
    public async Task IsReturnsModelValidGenres()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<IndexViewModel>(viewResult.Model);
        Assert.NotNull(model.Genres);
        Assert.Equal(5, model.Genres.Count());
    }
    
    [Fact]
    public async Task IsReturnsModelValidArts()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = await controller.Index();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<IndexViewModel>(viewResult.Model);
        Assert.NotNull(model.Arts);
        Assert.NotEmpty(model.Arts);
    }
}