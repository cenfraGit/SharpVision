using System;
namespace SharpVision;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class SharpFunctionAttribute : Attribute
{
    public string Group { get; }
    public string Description { get; }
    public string? Name { get; set; }

    public SharpFunctionAttribute(string group, string description = "")
    {
        this.Group = group;
        this.Description = description;
    }
}

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class SharpOutputAttribute : Attribute
{
}
