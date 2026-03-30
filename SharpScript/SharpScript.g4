grammar SharpScript;

// --------------------------------------------------------------------------------
// parser rules
// --------------------------------------------------------------------------------

program : statement* EOF ;

statement
    : assignmentStat SEMI
    | functionCallStat SEMI
    | functionDeclarationStat
    | ifStat
    | whileStat
    | foreachStat
    | blockStat
    | returnStat SEMI
    ;

blockStat : '{' statement* '}' ;

assignmentStat : ID (COMMA ID)* ASSIGN expr ;

ifStat : IF '(' expr ')' blockStat (ELSE IF '(' expr ')' blockStat)* (ELSE blockStat)? ;

whileStat : WHILE '(' expr ')' blockStat ;
foreachStat : FOREACH '(' ID IN expr ')' blockStat ;

functionCallStat : ID '(' (expr (COMMA expr)*)? ')' ;
functionDeclarationStat : FUNC ID '(' (ID (COMMA ID)*)? ')' blockStat ; // return?
returnStat : RETURN (expr (COMMA expr)*)? ;

expr
    : '(' expr ')'                #ParenthesesExpr
    | expr (MUL|DIV) expr         #MulDivExpr
    | expr (PLUS|MINUS) expr      #PlusMinusExpr
    | expr (EQ|GE|LE|GT|LT) expr  #ComparisonExpr
    | functionCallStat            #FunctionCallExpr
    | ID                          #IdExpr
    | INT                         #IntExpr
    | FLOAT                       #FloatExpr
    | STRING                      #StringExpr
    | TRUE                        #TrueExpr
    | FALSE                       #FalseExpr
    ;

// --------------------------------------------------------------------------------
// lexer rules
// --------------------------------------------------------------------------------

COMMA  : ',' ;
SEMI   : ';' ;

INT    : [0-9]+;
FLOAT  : [0-9]* '.' [0-9]+ ;
STRING : '"' ~["]* '"' ; // anything that isnt a quote

PLUS  : '+' ;
MINUS : '-' ;
MUL   : '*' ;
DIV   : '/';

EQ : '==' ;
GE : '>=' ;
LE : '<=' ;
GT : '>' ;
LT : '<' ;

ASSIGN : '=' ;

TRUE    : 'true' ;
FALSE   : 'false' ;
IF      : 'if' ;
ELSE    : 'else' ;
WHILE   : 'while' ;
FOREACH : 'foreach' ;
IN      : 'in';
FUNC    : 'func' ;
RETURN  : 'return' ;

WS     : [ \t\r\n]+ -> skip;
ID     : [a-zA-Z_][a-zA-Z0-9_]*;