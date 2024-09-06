namespace Calculator;

internal enum Associavity
{
    Left,
    Right
}

internal record Operator(char Opr, int Priority, Associavity Assoc);