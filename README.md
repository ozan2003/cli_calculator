# Calculator

A simple mathematical expression calculator implemented in C#. This calculator evaluates infix expressions using the Shunting Yard algorithm to convert to postfix (Reverse Polish Notation) before evaluation.

## Features

- Evaluates basic arithmetic expressions: addition, subtraction, multiplication, division
- Supports exponentiation (^)
- Handles parentheses for expression grouping
- Properly processes negative numbers
- Handles multi-digit numbers
- Supports decimal numbers (e.g., 3.14)
- Built-in help command

## Usage

Run the calculator and enter mathematical expressions at the prompt. Type `help` for usage information, or `exit`/`quit` to exit the application.

```shell
$ dotnet run --project Calculator

Calculator - Type 'help' for usage information
Type an expression to calculate or 'exit' to quit.

> 2 + 3 * 4
14
> (2 + 3) * 4
20
> 2^3 + 10
18
> 5 / 2
2.5
> 3.14 * 2
6.28
```

## Supported Operations

- `+` Addition
- `-` Subtraction
- `*` Multiplication
- `/` Division
- `^` Exponentiation
- `(` `)` Parentheses for grouping
