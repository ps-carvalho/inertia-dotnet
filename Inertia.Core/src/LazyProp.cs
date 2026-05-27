using System.Text.Json.Serialization;

namespace Inertia.Core;

public class LazyProp
{
    public Func<object?> Value { get; }

    public LazyProp(Func<object?> value)
    {
        Value = value;
    }
}

public class DeferredProp
{
    public string Group { get; }
    public Func<object?> Value { get; }

    public DeferredProp(string group, Func<object?> value)
    {
        Group = group;
        Value = value;
    }
}
