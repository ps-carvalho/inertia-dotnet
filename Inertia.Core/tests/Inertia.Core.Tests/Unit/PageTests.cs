using System.Text.Json;
using Xunit;

namespace Inertia.Core.Tests.Unit;

public class PageTests
{
    [Fact]
    public void Page_Serializes_With_Correct_Property_Names()
    {
        var page = new Page
        {
            Component = "Home/Index",
            Props = new Dictionary<string, object?> { ["message"] = "Hello" },
            Url = "/home",
            Version = "1.0.0"
        };

        var json = JsonSerializer.Serialize(page, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Assert.Contains("\"component\":\"Home/Index\"", json);
        Assert.Contains("\"props\":", json);
        Assert.Contains("\"url\":\"/home\"", json);
        Assert.Contains("\"version\":\"1.0.0\"", json);
    }

    [Fact]
    public void Page_Deserializes_Correctly()
    {
        var json = @"{""component"":""Home/Index"",""props"":{""message"":""Hello""},""url"":""/home"",""version"":""1.0.0""}";

        var page = JsonSerializer.Deserialize<Page>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        Assert.NotNull(page);
        Assert.Equal("Home/Index", page.Component);
        Assert.Equal("Hello", page.Props["message"].ToString());
        Assert.Equal("/home", page.Url);
        Assert.Equal("1.0.0", page.Version);
    }
}
