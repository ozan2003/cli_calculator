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
    private static readonly Dictionary<char, (int priority, Associativity assoc)> _operators = new()
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
    private static bool IsOperator(in char ch) => _operators.ContainsKey(ch);

    /// <summary>
    /// Returns a postfix (RPN) version of the infix expression.
    /// </summary>
    private static string ToPostfix(in string infix)
    {
        Stack<char> operatorStack = new();
        StringBuilder postfix = new(infix.Length);
        // Previous character in the infix expression. Useful for determining if `-` is binary or unary.
        char previousChar = '\0';

        for (int i = 0; i < infix.Length; ++i)
        {
            // Handle numbers (multi-digit).
            if (char.IsDigit(infix[i]))
            {
                StringBuilder number = new();

                // Capture the entire number (multi-digit support)
                while (i < infix.Length && char.IsDigit(infix[i]))
                {
                    number.Append(infix[i]);
                    ++i;
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
                    && _operators[infix[i]].priority <= _operators[operatorStack.Peek()].priority
                    && _operators[infix[i]].assoc == Associativity.Left
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
                // Pop operators off the stack and append them to the output,
                // until the operator at the top of the stack is a opening bracket.
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

        // Pop any remaining operators
        while (operatorStack.Count != 0)
        {
            postfix.Append(operatorStack.Pop());
            postfix.Append(' ');
        }

        return postfix.ToString().TrimEnd();
    }

    /// <summary>
    /// Evaluates a mathematical expression and returns the result
    /// </summary>
    /// <param name="expression">The expression to calculate</param>
    /// <returns>The result of the calculation</returns>
    public double Calculate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException("Expression cannot be empty");
        }

        Stack<double> nums = new();
        string postfix = ToPostfix(expression);

        foreach (string token in postfix.Split(' '))
        {
            char front = token[0];

            if (char.IsDigit(front) || (front == '-' && token.Length > 1))
            {
                nums.Push(Convert.ToDouble(token));
            }
            // Do the operation.
            else if (IsOperator(front) && nums.Count >= 2)
            {
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
                        nums.Push(first / second);
                        break;
                    case '^':
                        nums.Push(Math.Pow(first, second));
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

        return nums.Count > 0 ? nums.Pop() : 0;
    }
}
