using System.Reflection;

namespace SharpScript;

public class SharpScriptVisitor : SharpScriptBaseVisitor<object?>
{
    private readonly Dictionary<string, object?> _variables;
    private readonly Dictionary<string, MethodInfo> _registryNative;
    private readonly Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> _registryUser;

    // --------------------------------------------------------------------------------
    // constructor
    // --------------------------------------------------------------------------------

    public SharpScriptVisitor(Dictionary<string, object?> variables,
                              Dictionary<string, MethodInfo> registryNative,
                              Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> registryUser)
    {
        // use environment tables
        _variables = variables;
        _registryNative = registryNative;
        _registryUser = registryUser;
    }

    public override object? VisitProgram(SharpScriptParser.ProgramContext context)
    {
        foreach (var statement in context.statement())
            Visit(statement);
        return null;
    }

    // --------------------------------------------------------------------------------
    // statements
    // --------------------------------------------------------------------------------

    public override object? VisitAssignmentStat(SharpScriptParser.AssignmentStatContext context)
    {
        var rightSideValue = Visit(context.expr());
        var variableNames = context.ID().Select(id => id.GetText()).ToArray();

        if (variableNames.Length > 1)
        {
            if (rightSideValue is object[] resultsArray)
            {
                if (variableNames.Length != resultsArray.Length)
                    throw new Exception($"Cannot unpack {resultsArray.Length} values into {variableNames.Length} variables.");

                for (int i = 0; i < variableNames.Length; i++)
                {
                    _variables[variableNames[i]] = resultsArray[i];
                }
            }
            else
            {
                throw new Exception("Right side of assignment does not return multiple values.");
            }
        }
        else
        {
            if (rightSideValue is object[] { Length: 1 } singleResult)
            {
                _variables[variableNames[0]] = singleResult[0];
            }
            else
            {
                _variables[variableNames[0]] = rightSideValue;
            }
        }

        return null;
    }

    // --------------------------------------------------------------------------------
    // atom
    // --------------------------------------------------------------------------------

    public override object? VisitIntExpr(SharpScriptParser.IntExprContext context)
    {
        return int.Parse(context.INT().GetText());
    }

    public override object? VisitFloatExpr(SharpScriptParser.FloatExprContext context)
    {
        return float.Parse(context.FLOAT().GetText());
    }

    public override object? VisitStringExpr(SharpScriptParser.StringExprContext context)
    {
        var rawString = context.STRING().GetText();
        return rawString.Substring(1, rawString.Length - 2);
    }

    public override object? VisitIdExpr(SharpScriptParser.IdExprContext context)
    {
        string id = context.ID().GetText();
        return _variables[id];
    }

    // --------------------------------------------------------------------------------
    // arithmetic
    // --------------------------------------------------------------------------------

    public override object? VisitPlusMinusExpr(SharpScriptParser.PlusMinusExprContext context)
    {
        dynamic? left = Visit(context.expr(0));
        dynamic? right = Visit(context.expr(1));
        return context.PLUS() != null ? left + right : left - right;
    }

    public override object? VisitMulDivExpr(SharpScriptParser.MulDivExprContext context)
    {
        dynamic? left = Visit(context.expr(0));
        dynamic? right = Visit(context.expr(1));
        return context.MUL() != null ? left * right : left / right;
    }

    // --------------------------------------------------------------------------------
    // functions
    // --------------------------------------------------------------------------------

    public override object? VisitFunctionCallStat(SharpScriptParser.FunctionCallStatContext context)
    {
        string functionName = context.ID().GetText();

        var argsContext = context.expr();
        object?[] providedArgs = argsContext != null ? [.. argsContext.Select(e => Visit(e))] : [];

        if (_registryNative.TryGetValue(functionName, out MethodInfo? nativeMethod))
        {
            var expectedParameters = nativeMethod.GetParameters();

            if (expectedParameters.Length != providedArgs.Length)
            {
                throw new Exception($"Function \"{functionName}\" expects {expectedParameters.Length} arguments, but got {providedArgs.Length}.");
            }

            object?[] coercedArgs = new object?[providedArgs.Length];

            for (int i = 0; i < expectedParameters.Length; i++)
            {
                Type expectedType = expectedParameters[i].ParameterType;
                object? providedValue = providedArgs[i];

                if (providedValue == null)
                {
                    coercedArgs[i] = null;
                    continue;
                }

                if (expectedType.IsAssignableFrom(providedValue.GetType()))
                {
                    coercedArgs[i] = providedValue;
                }
                else
                {
                    try
                    {
                        coercedArgs[i] = Convert.ChangeType(providedValue, expectedType);
                    }
                    catch (InvalidCastException)
                    {
                        throw new Exception($"Cannot convert argument {i + 1} (\"{providedValue}\") from {providedValue.GetType().Name} to {expectedType.Name} in function \"{functionName}\".");
                    }
                }
            }

            try
            {
                return nativeMethod.Invoke(null, coercedArgs);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        if (_registryUser.TryGetValue(functionName, out var userFuncContext))
        {
            throw new NotImplementedException("User-defined function");
        }

        throw new Exception($"Unknown function: \"{functionName}\"");
    }
}