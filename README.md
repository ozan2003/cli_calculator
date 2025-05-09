# Calculator

A simple mathematical expression calculator implemented in C#. This calculator evaluates infix expressions using the Shunting Yard algorithm to convert to postfix (Reverse Polish Notation) before evaluation.

## Features

- Evaluates basic arithmetic expressions: addition, subtraction, multiplication, division
- Supports exponentiation (^)
- Handles parentheses for expression grouping
- Properly processes negative numbers
- Handles multi-digit numbers

## Usage

Run the calculator and enter mathematical expressions at the prompt. Type `exit` or `quit` to exit the application.

```shell
$ dotnet run --project Calculator

RPN Calculator
Type help for more information.

> 2 + 3 * 4
14
> (2 + 3) * 4
20
> 2^3 + 10
18
> exit
```

## Supported Operations

- `+` Addition
- `-` Subtraction
- `*` Multiplication
- `/` Division
- `^` Exponentiation
- `(` `)` Parentheses for grouping
