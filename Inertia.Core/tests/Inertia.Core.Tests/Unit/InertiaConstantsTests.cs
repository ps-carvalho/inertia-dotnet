using Xunit;

namespace Inertia.Core.Tests.Unit;

public class InertiaConstantsTests
{
    [Fact]
    public void Header_Names_Are_Correct()
    {
        Assert.Equal("X-Inertia", InertiaConstants.HeaderInertia);
        Assert.Equal("X-Inertia-Version", InertiaConstants.HeaderInertiaVersion);
        Assert.Equal("X-Inertia-Location", InertiaConstants.HeaderInertiaLocation);
        Assert.Equal("X-Inertia-Partial-Data", InertiaConstants.HeaderInertiaPartialData);
        Assert.Equal("X-Inertia-Partial-Except", InertiaConstants.HeaderInertiaPartialExcept);
        Assert.Equal("X-Inertia-Error-Bag", InertiaConstants.HeaderInertiaErrorBag);
    }
}
