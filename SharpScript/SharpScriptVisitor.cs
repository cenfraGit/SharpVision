using System.Reflection;
using Antlr4.Runtime;

namespace SharpScript;

public class SharpScriptVisitor : SharpScriptBaseVisitor<object?>
{
    // --------------------------------------------------------------------------------
    // fields and properties
    // --------------------------------------------------------------------------------

    private Script _script;
    private HashSet<string> _loadedFiles = new (StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, object?> _variables;
    private readonly Dictionary<string, MethodInfo> _registryNative;
    private readonly Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> _registryUser;

    // --------------------------------------------------------------------------------
    // constructor
    // --------------------------------------------------------------------------------

    public SharpScriptVisitor(Script script,
                              Dictionary<string, object?> variables,
                              Dictionary<string, MethodInfo> registryNative,
                              Dictionary<string, SharpScriptParser.FunctionDeclarationStatContext> registryUser)
    {
        // use data from environment
        this._script = script;
        this._variables = variables;
        this._registryNative = registryNative;
        this._registryUser = registryUser;

        if (this._script.Path is not null)
            this._loadedFiles.Add(this._script.Path);
    }

    public override object? VisitProgram(SharpScriptParser.ProgramContext context)
    {
        foreach (var import in context.importStat())
            Visit(import);
        foreach (var statement in context.statement())
            Visit(statement);
        return null;
    }

    // --------------------------------------------------------------------------------
    // imports
    // --------------------------------------------------------------------------------

    public override object? VisitImportStat(SharpScriptParser.ImportStatContext context)
    {
        string pathFile = context.STRING().GetText().Trim('"');

        // works for both absolute and relative paths
        if (!File.Exists(pathFile))
            throw new SharpScriptException($"Import: file not found ({pathFile})", context);

        if (Path.GetExtension(pathFile) != ".svs")
            throw new SharpScriptException($"Import: file not a .svs file ({pathFile})", context);

        if (_loadedFiles.Add(pathFile))
        {
            string code = File.ReadAllText(pathFile);

            var inputStream = new AntlrInputStream(code);
            var lexer = new SharpScriptLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SharpScriptParser(tokens);
            var tree = parser.program();
            this.Visit(tree);
        }
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
                    throw new SharpScriptException(
                        $"Cannot unpack {resultsArray.Length} values into {variableNames.Length} variables.", context);

                for (int i = 0; i < variableNames.Length; i++)
                {
                    _variables[variableNames[i]] = resultsArray[i];
                }
            }
            else
            {
                throw new SharpScriptException("Right side of assignment does not return multiple values.", context);
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
    // expr
    // --------------------------------------------------------------------------------

    public override object? VisitParenthesesExpr(SharpScriptParser.ParenthesesExprContext context)
    {
        return Visit(context.expr());
    }

    public override object? VisitMulDivExpr(SharpScriptParser.MulDivExprContext context)
    {
        dynamic left = Visit(context.expr(0))!;
        dynamic right = Visit(context.expr(1))!;

        if (context.MUL() is not null)
        {
            try { return left * right; }
            catch { throw new SharpScriptException($"Can't multiply {left.GetType()} with {right.GetType()}", context); }
        }
        else
        {
            try { return left / right; }
            catch { throw new SharpScriptException($"Can't divide {left.GetType()} by {right.GetType()}", context); }
        }
    }

    public override object? VisitPlusMinusExpr(SharpScriptParser.PlusMinusExprContext context)
    {
        dynamic left = Visit(context.expr(0))!;
        dynamic right = Visit(context.expr(1))!;

        if (context.PLUS() is not null)
        {
            try { return left + right; }
            catch { throw new SharpScriptException($"Can't add {left.GetType()} with {right.GetType()}", context); }
        }
        else
        {
            try { return left - right; }
            catch { throw new SharpScriptException($"Can't subtract {right.GetType()} from {left.GetType()}", context); }
        }
    }

    public override object? VisitComparisonExpr(SharpScriptParser.ComparisonExprContext context)
    {
        dynamic? left = Visit(context.expr(0));
        dynamic? right = Visit(context.expr(1));

        try
        {
            if (context.EQ() is not null)
                return left == right;
            else if (context.NEQ() is not null)
                return left != right;
            else if (context.GE() is not null)
                return left >= right;
            else if (context.LE() is not null)
                return left <= right;
            else if (context.GT() is not null)
                return left > right;
            else if (context.LT() is not null)
                return left < right;
            else
                throw new SharpScriptException("Comparison symbol not defined.", context);
        }
        catch (Exception ex)
        {
            if (ex is SharpScriptException shEx) throw;
            throw new SharpScriptException("Can't use comparison operator on types {left.GetType()} and {right.GetType()}",
                                           context);
        }
    }

    public override object? VisitBoolOperatorExpr(SharpScriptParser.BoolOperatorExprContext context)
    {
        dynamic? left = Visit(context.expr(0));
        dynamic? right = Visit(context.expr(1));
        if (left is not bool || right is not bool)
            throw new SharpScriptException("Can't use boolean operator on types " +
                                           "{left.GetType} and {right.GetType()}.", context);

        if (context.AND() is not null)
            return left && right;
        else if (context.OR() is not null)
            return left || right;
        else if (context.XOR() is not null)
            return left ^ right;
        else
            throw new SharpScriptException("Bool operator logic not implemented.", context);
    }

    public override object? VisitNotExpr(SharpScriptParser.NotExprContext context)
    {
        dynamic? expression = Visit(context.expr());
        if (expression is bool)
            return !expression;
        else
            throw new SharpScriptException($"Can't apply NOT to a non-boolean expression ({expression}).", context);
    }

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

    public override object? VisitBoolExpr(SharpScriptParser.BoolExprContext context)
    {
        return (context.TRUE() is not null) ? true : false;
    }

    public override object? VisitIdExpr(SharpScriptParser.IdExprContext context)
    {
        string id = context.ID().GetText();
        if (!_variables.ContainsKey(id))
            throw new SharpScriptException($"\"{id}\" is not defined.", context);
        return _variables[id];
    }

    // --------------------------------------------------------------------------------
    // block
    // --------------------------------------------------------------------------------

    public override object? VisitBlockStat(SharpScriptParser.BlockStatContext context)
    {
        foreach (var statement in context.statement())
            Visit(statement);
        return null;
    }

    // --------------------------------------------------------------------------------
    // conditionals
    // --------------------------------------------------------------------------------

    public override object? VisitIfStat(SharpScriptParser.IfStatContext context)
    {
        for (int i = 0; i < context.expr().Length; i++)
        {
            if (Visit(context.expr(i)) is not bool condition)
                throw new SharpScriptException("If: {i}-th condition is not a boolean expression.", context);
            if (condition)
            {
                Visit(context.blockStat(i));
                return null;
            }
        }

        if (context.ELSE() is not null)
            Visit(context.blockStat(context.blockStat().Length - 1)); // execute last

        return null;
    }

    // --------------------------------------------------------------------------------
    // loops
    // --------------------------------------------------------------------------------

    public override object? VisitWhileStat(SharpScriptParser.WhileStatContext context)
    {
        if (Visit(context.expr()) is not bool)
            throw new SharpScriptException("While: condition is not boolean expression.", context);

        while (Visit(context.expr()) is bool condition && condition)
        {
            Visit(context.blockStat());
        }
        return null;
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
                throw new SharpScriptException(
                    $"Function \"{functionName}\" expects {expectedParameters.Length} " +
                    "arguments, but got {providedArgs.Length}.", context);
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
                        throw new SharpScriptException(
                            $"Cannot convert argument {i + 1} (\"{providedValue}\") " +
                            "from {providedValue.GetType().Name} to {expectedType.Name} " +
                            "in function \"{functionName}\".", context);
                    }
                }
            }

            try
            {
                return nativeMethod.Invoke(null, coercedArgs);
            }
            catch (TargetInvocationException ex)
            {
                throw new SharpScriptException((ex.InnerException is not null) ?
                                               ex.InnerException.Message : ex.Message, context);
            }
        }

        if (_registryUser.TryGetValue(functionName, out var userFuncContext))
        {
            throw new NotImplementedException("User-defined function");
        }

        throw new SharpScriptException($"Unknown function: \"{functionName}\"", context);
    }
}
