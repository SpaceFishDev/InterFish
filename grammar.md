# Lexer Grammar:
Using regex in the grammar but I am not actually using regex in the lexer.
## IF
- if statements (just the word if)<br>
`if`
## KEYWORD
- Used for names of functions variables etc<br>
`^[A-Za-z][A-Za-z0-9_]*$`
## NUM
- Used for decimal numbers <br>
`[0-9]*.[0-9]*`
## STRING
- Used for well, strings <br>
`"[!-~]*"|'[!-~]*'`
## PARENTHESIS (Outside of strings)
`(|)`
## BRACKETS (Outside of strings)
`[|]`
## BRACES (Outside of strings)
`{|}`
## FUNCTION DECLARATION
`fn`
## VARIABLE
`var`
## Equals
`=`
## BoolEq
`==`
## BoolNot
`!`
## BoolNotEq
`!=`
## BoolOr
`\|\|`
## Operations
`+|-|*|/`