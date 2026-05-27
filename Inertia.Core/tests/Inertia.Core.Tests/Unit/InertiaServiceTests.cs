using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Inertia.Core.Tests.Unit;

public class InertiaServiceTests
{
    private readonly InertiaOptions _options;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

    public InertiaServiceTests()
    {
        _options = new InertiaOptions();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public void GetVersion_Returns_Static_Version()
    {
        _options.Version = "1.0.0";
        var service = new InertiaService(_options, _httpContextAccessor.Object);

        Assert.Equal("1.0.0", service.GetVersion());
    }

    [Fact]
    public void GetVersion_Returns_Resolved_Version()
    {
        _options.VersionResolver = _ => "resolved";
        var service = new InertiaService(_options, _httpContextAccessor.Object);

        Assert.Equal("resolved", service.GetVersion());
    }

    [Fact]
    public void Share_Adds_Single_Value()
    {
        var service = new InertiaService(_options, _httpContextAccessor.Object);
        service.Share("key", "value");

        var data = service.GetSharedData();
        Assert.Equal("value", data["key"]);
    }

    [Fact]
    public void Share_Adds_Multiple_Values()
    {
        var service = new InertiaService(_options, _httpContextAccessor.Object);
        service.Share(new Dictionary<string, object?>
        {
            ["key1"] = "value1",
            ["key2"] = "value2"
        });

        var data = service.GetSharedData();
        Assert.Equal("value1", data["key1"]);
        Assert.Equal("value2", data["key2"]);
    }

    [Fact]
    public void GetSharedData_Returns_Copy()
    {
        var service = new InertiaService(_options, _httpContextAccessor.Object);
        service.Share("key", "value");

        var data1 = service.GetSharedData();
        var data2 = service.GetSharedData();

        Assert.NotSame(data1, data2);
        Assert.Equal(data1["key"], data2["key"]);
    }

    [Fact]
    public void Render_Returns_InertiaResult()
    {
        var service = new InertiaService(_options, _httpContextAccessor.Object);
        var result = service.Render("Component", new Dictionary<string, object?> { ["key"] = "value" });

        Assert.NotNull(result);
        Assert.IsType<InertiaResult>(result);
    }

    [Fact]
    public void SetErrorBag_Stores_ErrorBag()
    {
        var service = new InertiaService(_options, _httpContextAccessor.Object);
        service.SetErrorBag("createForm");

        Assert.Equal("createForm", service.GetErrorBag());
    }
}
