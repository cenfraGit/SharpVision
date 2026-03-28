grammar SharpScript;

program: declaration* EOF;

declaration
    : varDecl
    | statement
    ;

varDecl: 'int' ID ('=' expr)? ';';
assignment: ID '=' expr;

statement
    : assignment ';'                           # AssignStmt
    | 'if' '(' expr ')' '{' declaration* '}'   # IfStmt
    | 'print' expr ';'                         # PrintStmt
    ;

expr
    : INT                     # IntExpr
    | ID                      # IdExpr
    | expr op=('*'|'/') expr  # MulDivExpr
    | expr op=('+'|'-') expr  # AddSubExpr
    | '(' expr ')'            # ParensExpr
    ;

ID  : [a-zA-Z_][a-zA-Z0-9_]*;
INT : [0-9]+;
WS  : [ \t\r\n]+ -> skip;