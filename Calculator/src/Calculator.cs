using System.Text;

namespace Calculator;

/// <summary>
/// Represents the associativity of operators
/// </summary>
public enum Associativity
{
    Left,
    Right,
}

/// <summary>
/// Main calculator implementation
/// </summary>
public class Calculator : ICalculator
{
    /// <summary>
    /// A dictionary that contains the priority and associativity of each operator.
    /// </summary>
    private static readonly Dictionary<char, (int priority, Associativity assoc)> operators = new()
    {
        { '+', (1, Associativity.Left) },
        { '-', (1, Associativity.Left) },
        { '*', (2, Associativity.Left) },
        { '/', (2, Associativity.Left) },
        { '^', (3, Associativity.Right) }, // Not to be confused with xor operator.
    };

    /// <summary>
    /// Check the character is an operator or not.
    /// </summary>
    private static bool IsOperator(char ch) => operators.ContainsKey(ch);

    /// <summary>
    /// Returns a postfix (RPN) version of the infix expression.
    /// </summary>
    private static string ToPostfix(ReadOnlySpan<char> infix)
    {
        Stack<char> operatorStack = new();
        StringBuilder postfix = new(infix.Length);
        // Previous character in the infix expression. Useful for determining if `-` is binary or unary.
        char previousChar = '\0';

        for (int i = 0; i < infix.Length; ++i)
        {
            // Handle numbers (multi-digit with decimal points).
            if (char.IsDigit(infix[i]) || infix[i] == '.')
            {
                StringBuilder number = new();
                bool hasDecimalPoint = false;

                // Capture the entire number (multi-digit with decimal support)
                while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.'))
                {
                    if (infix[i] == '.')
                    {
                        if (hasDecimalPoint)
                        {
                            throw new ArgumentException(
                                $"Multiple decimal points in a number at position {i}"
                            );
                        }
                        hasDecimalPoint = true;
                    }
                    number.Append(infix[i]);
                    ++i;
                }

                // Make sure the number has at least one digit
                if (number.ToString() == ".")
                {
                    throw new ArgumentException(
                        "A decimal point must be followed by at least one digit"
                    );
                }

                // Add a leading zero if the number starts with a decimal point
                if (number.Length > 0 && number[0] == '.')
                {
                    number.Insert(0, '0');
                }

                postfix.Append(number);
                postfix.Append(' ');
                --i; // Adjust to correct the loop increment.
            }
            // Handle negative numbers
            else if (infix[i] == '-' && (i == 0 || previousChar == '(' || IsOperator(previousChar)))
            {
                postfix.Append("0 ");
                operatorStack.Push(infix[i]);
            }
            // If it's an operator.
            else if (IsOperator(infix[i]))
            {
                // While there is an operator of higher or equal precedence than top of the stack,
                // pop it off the stack and append it to the output.
                while (
                    operatorStack.Count != 0
                    && operatorStack.Peek() != '('
                    && operators[infix[i]].priority <= operators[operatorStack.Peek()].priority
                    && operators[infix[i]].assoc == Associativity.Left
                )
                {
                    postfix.Append(operatorStack.Pop());
                    postfix.Append(' ');
                }
                operatorStack.Push(infix[i]);
            }
            // Opening parenthesis.
            else if (infix[i] == '(')
            {
                operatorStack.Push(infix[i]);
            }
            // Closing parenthesis.
            else if (infix[i] == ')')
            {
                // Check if there's a matching opening parenthesis
                if (operatorStack.Count == 0 || !operatorStack.Contains('('))
                {
                    throw new ArgumentException("Unmatched closing parenthesis");
                }

                // Pop operators off the stack and append them to the output,
                // until the operator at the top of the stack is an opening bracket.
                while (operatorStack.Count != 0 && operatorStack.Peek() != '(')
                {
                    postfix.Append(operatorStack.Pop());
                    postfix.Append(' ');
                }
                operatorStack.Pop(); // Remove '(' from stack
            }
            else if (char.IsWhiteSpace(infix[i]))
            {
                continue;
            }
            else
            {
                throw new ArgumentException(
                    $"Invalid character '{infix[i]}' in the infix expression."
                );
            }

            previousChar = infix[i];
        }

        // Check for unmatched opening parentheses
        if (operatorStack.Contains('('))
        {
            throw new ArgumentException("Unmatched opening parenthesis");
        }

        // Pop any remaining operators
        while (operatorStack.Count != 0)
        {
            postfix.Append(operatorStack.Pop());
            postfix.Append(' ');
        }

        return postfix.ToString().TrimEnd();
    }

    /// <summary>
    /// Evaluates a postfix expression and returns the result
    /// </summary>
    /// <param name="postfix">The postfix expression to evaluate</param>
    /// <returns>The result of the evaluation</returns>
    private static decimal PostfixEval(string postfix)
    {
        Stack<decimal> nums = new();

        foreach (string token in postfix.Split(' '))
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            char front = token[0];

            if (char.IsDigit(front) || front == '.' || (front == '-' && token.Length > 1))
            {
                if (!decimal.TryParse(token, out decimal value))
                {
                    throw new ArgumentException($"Invalid number format: {token}");
                }
                nums.Push(value);
            }
            // Do the operation.
            else if (IsOperator(front))
            {
                if (nums.Count < 2)
                {
                    throw new ArgumentException($"Not enough operands for operator '{front}'");
                }

                var second = nums.Pop();
                var first = nums.Pop();

                switch (front)
                {
                    case '+':
                        nums.Push(first + second);
                        break;
                    case '-':
                        nums.Push(first - second);
                        break;
                    case '*':
                        nums.Push(first * second);
                        break;
                    case '/':
                        if (second == 0)
                        {
                            throw new ArgumentException("Division by zero");
                        }
                        nums.Push(first / second);
                        break;
                    case '^':
                        // Since Math.Pow works with doubles, we need to convert
                        var result = (decimal)Math.Pow((double)first, (double)second);
                        nums.Push(result);
                        break;
                    default:
                        throw new ArgumentException($"Unknown operator '{front}'.");
                }
            }
            else
            {
                throw new ArgumentException($"Invalid token '{token}' in the postfix expression.");
            }
        }

        if (nums.Count == 0)
        {
            throw new ArgumentException("The expression did not produce a result");
        }

        if (nums.Count > 1)
        {
            throw new ArgumentException("The expression has too many operands");
        }

        return nums.Pop();
    }

    /// <summary>
    /// Evaluates a mathematical expression and returns the result
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <returns>The result of the calculation</returns>
    public decimal Calculate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException("Expression cannot be empty");
        }

        string postfix = ToPostfix(expression);
        return PostfixEval(postfix);
    }
}
