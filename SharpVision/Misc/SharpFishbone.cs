using System.Globalization;
using System.Reflection;
using Fishbone.Engine;

namespace SharpVision;

public record SharpFunctionDescriptor(string Name, string Signature, string Group, string Description);

public static class SharpFishbone
{
    public static FishboneConfiguration CreateConfiguration()
    {
        var configuration = new FishboneConfiguration();

        foreach (var method in GetSharpFunctionMethods())
            configuration.RegisterFunction(GetFunctionName(method), CreateDelegate(method));

        return configuration;
    }

    public static IReadOnlyList<SharpFunctionDescriptor> GetFunctionDescriptors()
    {
        return GetSharpFunctionMethods()
            .Select(method =>
            {
                var attribute = method.GetCustomAttribute<SharpFunctionAttribute>()!;
                return new SharpFunctionDescriptor(GetFunctionName(method),
                                                   GetFunctionSignature(method),
                                                   attribute.Group,
                                                   attribute.Description);
            })
            .OrderBy(descriptor => descriptor.Group)
            .ThenBy(descriptor => descriptor.Name)
            .ToArray();
    }

    private static IEnumerable<MethodInfo> GetSharpFunctionMethods()
    {
        return typeof(Sharp)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => !method.IsGenericMethodDefinition)
            .Where(method => method.GetCustomAttribute<SharpFunctionAttribute>() is not null);
    }

    private static string GetFunctionName(MethodInfo method)
    {
        var attribute = method.GetCustomAttribute<SharpFunctionAttribute>()!;
        return attribute.Name ?? method.Name;
    }

    private static string GetFunctionSignature(MethodInfo method)
    {
        var parameters = method.GetParameters()
            .Select(parameter =>
            {
                string prefix = parameter.GetCustomAttribute<SharpOutputAttribute>() is null ? "" : "out ";
                return $"{prefix}{parameter.Name}";
            });

        return $"{GetFunctionName(method)}({string.Join(", ", parameters)})";
    }

    private static Delegate CreateDelegate(MethodInfo method)
    {
        return method.GetParameters().Length switch
        {
            0 => new Func<List<object>>(() => Invoke(method)),
            1 => new Func<object, List<object>>(a => Invoke(method, a)),
            2 => new Func<object, object, List<object>>((a, b) => Invoke(method, a, b)),
            3 => new Func<object, object, object, List<object>>((a, b, c) => Invoke(method, a, b, c)),
            4 => new Func<object, object, object, object, List<object>>((a, b, c, d) => Invoke(method, a, b, c, d)),
            5 => new Func<object, object, object, object, object, List<object>>((a, b, c, d, e) => Invoke(method, a, b, c, d, e)),
            6 => new Func<object, object, object, object, object, object, List<object>>((a, b, c, d, e, f) => Invoke(method, a, b, c, d, e, f)),
            _ => throw new NotSupportedException($"Sharp function \"{method.Name}\" has too many script parameters.")
        };
    }

    private static List<object> Invoke(MethodInfo method, params object?[] scriptArguments)
    {
        var parameters = method.GetParameters();
        var invocationArguments = new object?[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
            invocationArguments[i] = CoerceArgument(scriptArguments[i], parameters[i].ParameterType);

        object? returnedValue;
        try
        {
            returnedValue = method.Invoke(null, invocationArguments);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex;
        }

        var results = new List<object>();
        if (method.ReturnType != typeof(void) && returnedValue is not null)
            results.Add(returnedValue);
        return results;
    }

    private static object? CoerceArgument(object? argument, Type targetType)
    {
        argument = UnwrapSingleResult(argument);

        if (argument is null)
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

        if (targetType.IsAssignableFrom(argument.GetType()))
            return argument;

        if (targetType.IsEnum && argument is string enumText)
            return Enum.Parse(targetType, enumText, ignoreCase: true);

        return Convert.ChangeType(argument, targetType, CultureInfo.InvariantCulture);
    }

    private static object? UnwrapSingleResult(object? value)
    {
        return value is List<object> { Count: 1 } values ? values[0] : value;
    }
}
