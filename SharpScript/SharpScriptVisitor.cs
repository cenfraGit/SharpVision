namespace SharpScript;

public class SharpScriptVisitor : SharpScriptBaseVisitor<int>
{
    private readonly Dictionary<string, object?> _variables = new();
    public Dictionary<string, object?> Variables => _variables;

    public override int VisitProgram(SharpScriptParser.ProgramContext context)
    {
        foreach (var decl in context.declaration())
        {
            Visit(decl);
        }
        return 0;
    }

    public override int VisitVarDecl(SharpScriptParser.VarDeclContext context)
    {
        string varName = context.ID().GetText();
        int value = 0;
        if (context.expr() != null)
        {
            value = Visit(context.expr());
        }
        _variables[varName] = value;
        return value;
    }

    public override int VisitAssignStmt(SharpScriptParser.AssignStmtContext context)
    {
        string varName = context.assignment().ID().GetText();
        int value = Visit(context.assignment().expr());

        if (!_variables.ContainsKey(varName))
            throw new Exception($"Variable {varName} not defined.");

        _variables[varName] = value;
        return value;
    }

    public override int VisitIfStmt(SharpScriptParser.IfStmtContext context)
    {
        int condition = Visit(context.expr());

        if (condition != 0)
        {
            foreach (var decl in context.declaration())
            {
                Visit(decl);
            }
        }
        return 0;
    }

    public override int VisitPrintStmt(SharpScriptParser.PrintStmtContext context)
    {
        int value = Visit(context.expr());
        Console.WriteLine($"> {value}");
        return value;
    }

    public override int VisitIntExpr(SharpScriptParser.IntExprContext context)
        => int.Parse(context.INT().GetText());

    public override int VisitIdExpr(SharpScriptParser.IdExprContext context)
    {
        string varName = context.ID().GetText();
        return (int)_variables.GetValueOrDefault(varName, 0);
    }

    public override int VisitAddSubExpr(SharpScriptParser.AddSubExprContext context)
    {
        int left = Visit(context.expr(0));
        int right = Visit(context.expr(1));
        return context.op.Text == "+" ? left + right : left - right;
    }

    public override int VisitMulDivExpr(SharpScriptParser.MulDivExprContext context)
    {
        int left = Visit(context.expr(0));
        int right = Visit(context.expr(1));
        return context.op.Text == "*" ? left * right : left / right;
    }
}
