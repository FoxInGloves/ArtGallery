using Microsoft.AspNetCore.Mvc;

namespace Tests.Tests.Controllers.ArtController;

public class PrivacyTests
{
    [Fact]
    public void ReturnsViewIsNotNull()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = controller.Privacy();

        Assert.NotNull(result);
    }

    [Fact]
    public void ReturnsViewIsValid()
    {
        var controller = Mocks.FakeControllers.GetArtsController();

        var result = controller.Privacy();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);
    }
}