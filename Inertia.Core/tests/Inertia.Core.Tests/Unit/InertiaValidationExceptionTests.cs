using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Inertia.Core.Tests.Unit;

public class InertiaValidationExceptionTests
{
    [Fact]
    public void Constructor_From_ModelState_Extracts_Errors()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "Name is required");
        modelState.AddModelError("Email", "Email is invalid");

        var ex = new InertiaValidationException(modelState);

        Assert.Equal("Name is required", ex.Errors["Name"]);
        Assert.Equal("Email is invalid", ex.Errors["Email"]);
        Assert.Null(ex.ErrorBag);
    }

    [Fact]
    public void Constructor_From_Dictionary_Stores_Errors()
    {
        var errors = new Dictionary<string, string>
        {
            ["field1"] = "Error 1",
            ["field2"] = "Error 2"
        };

        var ex = new InertiaValidationException(errors, "createForm");

        Assert.Equal("Error 1", ex.Errors["field1"]);
        Assert.Equal("Error 2", ex.Errors["field2"]);
        Assert.Equal("createForm", ex.ErrorBag);
    }

    [Fact]
    public void Constructor_Picks_First_Error_Per_Field()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Name", "First error");
        modelState.AddModelError("Name", "Second error");

        var ex = new InertiaValidationException(modelState);

        Assert.Equal("First error", ex.Errors["Name"]);
        Assert.Single(ex.Errors);
    }
}
