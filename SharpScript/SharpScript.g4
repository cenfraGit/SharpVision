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

ifStat : IF '(' expr ')' blockStat (ELSEIF '(' expr ')' blockStat)* (ELSE blockStat)? ;

whileStat : WHILE '(' expr ')' blockStat ;
foreachStat : FOREACH '(' ID IN expr ')' blockStat ;

functionCallStat : ID '(' (expr (COMMA expr)*)? ')' ;
functionDeclarationStat : FUNC ID '(' (ID (COMMA ID)*)? ')' blockStat ; // return?
returnStat : RETURN (expr (COMMA expr)*)? ;

expr
    : '(' expr ')'                    #ParenthesesExpr
    | expr (MUL|DIV) expr             #MulDivExpr
    | expr (PLUS|MINUS) expr          #PlusMinusExpr
    | expr (EQ|NEQ|GE|LE|GT|LT) expr  #ComparisonExpr
    | expr (AND|OR|XOR) expr          #BoolOperatorExpr
    | NOT expr                        #NotExpr
    | functionCallStat                #FunctionCallExpr
    | ID                              #IdExpr
    | INT                             #IntExpr
    | FLOAT                           #FloatExpr
    | STRING                          #StringExpr
    | (TRUE|FALSE)                    #BoolExpr
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

EQ  : '==' ;
NEQ : '!=' ;
GE  : '>=' ;
LE  : '<=' ;
GT  : '>' ;
LT  : '<' ;

AND : 'and' ;
OR  : 'or' ;
XOR : 'xor' ;
NOT : 'not' ;

ASSIGN : '=' ;

TRUE    : 'true' ;
FALSE   : 'false' ;
IF      : 'if' ;
ELSEIF  : 'else if' ;
ELSE    : 'else' ;

WHILE   : 'while' ;
FOREACH : 'foreach' ;
IN      : 'in';
FUNC    : 'func' ;
RETURN  : 'return' ;

WS     : [ \t\r\n]+ -> skip;
ID     : [a-zA-Z_][a-zA-Z0-9_]*;