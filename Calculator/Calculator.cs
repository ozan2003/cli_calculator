using System.Text;

namespace Calculator;

public class Calculator
{
    private enum Associativity
    {
        Left,
        Right,
    }

    /// <summary>
    /// A dictionary that contains the priority and associativity of each operator.
    /// </summary>
    private static readonly Dictionary<char, (int priority, Associativity assoc)> @operator =
        new()
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
    ///
    /// <param name="ch">
    /// The character to check.
    /// </param>
    ///
    /// <returns>
    /// True if the character is an operator, false otherwise.
    /// </returns>
    ///
    /// <see cref="@operator"/>
    private static bool IsOperator(in char ch) => @operator.ContainsKey(ch);

    /// <summary>
    /// Returns a postfix (RPN) version of the infix expression.
    /// </summary>
    ///
    /// <param name="input">
    /// The infix expression to convert.
    /// </param>
    ///
    /// <returns>
    /// Postfix version of the input expression.
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// If there are invalid characters in the infix expression.
    /// i.e. characters other than digits, arithmetic operators, and parentheses.
    /// </exception>
    private static string ToPostfix(in string infix)
    {
#if false
        // Remove all whitespaces and convert to a list of characters.
        StringBuilder infix = new(input.Length + 1); // +1 for the possible extra '0' at the beginning.

        if (input.StartsWith('-'))
        {
            infix.Append('0');
        }
        // Handle negative numbers at the beginning of the expression.
        infix.Append(string.Concat(input.Where(ch => !char.IsWhiteSpace(ch))));
        //---------------------------------------------------------------------------
#endif
        Stack<char> operator_stack = new();
        StringBuilder postfix = new(infix.Length);
        // Previous character in the infix expression. Useful for determining if `-` is binary or unary.
        char previous_char = '\0'; 

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
            // Handle numbers (multi-digit, negative).
            // Whenever '-' comes in string, check if there's a number before it.
            // If not push '0' then push '-'.
            //else if (infix[i] == '-' && (i == 0 || infix[i - 1] == '(' || IsOperator(infix[i - 1]) || previous_char == '('))
            else if (infix[i] == '-' && (i == 0 || previous_char == '(' || IsOperator(previous_char)))
            {
                postfix.Append("0 ");
                operator_stack.Push(infix[i]);
            }
            // If it's an operator.
            else if (IsOperator(infix[i]))
            {
                // While there is an operator of higher or equal precedence than top of the stack,
                // pop it off the stack and append it to the output.
                // Changing the their order will fuck things up, idk why.
                while (
                    operator_stack.Count != 0
                    && operator_stack.Peek() != '('
                    && @operator[infix[i]].priority <= @operator[operator_stack.Peek()].priority
                    && @operator[infix[i]].assoc == Associativity.Left
                )
                {
                    postfix.Append(operator_stack.Pop());
                    postfix.Append(' ');
                }
                operator_stack.Push(infix[i]);
            }
            // Opening parenthesis.
            else if (infix[i] == '(')
            {
                operator_stack.Push(infix[i]);
            }
            // Closing parenthesis.
            else if (infix[i] == ')')
            {
                // Pop operators off the stack and append them to the output,
                // until the operator at the top of the stack is a opening bracket.
                while (operator_stack.Count != 0 && operator_stack.Peek() != '(')
                {
                    postfix.Append(operator_stack.Pop());
                    postfix.Append(' ');
                }
                operator_stack.Pop(); // Remove '(' from stack
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

            previous_char = infix[i];
        }

        // Pop any remaining operators
        while (operator_stack.Count != 0)
        {
            postfix.Append(operator_stack.Pop());
            postfix.Append(' ');
        }

        return postfix.ToString().TrimEnd();
    }

    /// <summary>
    /// Evaluates RPN (Reverse Polish Notation) expression.
    /// </summary>
    ///
    /// <param name="infix">
    /// The infix expression to evaluate.
    /// </param>
    ///
    /// <returns>
    /// The result of the expression.
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// If there are invalid characters in the postfix expression.
    /// i.e. characters other than digits, arithmetic operators, and spaces.
    /// </exception>
    public static double Calculate(in string infix)
    {
        Stack<double> nums = new();
        string postfix = ToPostfix(infix);

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
                throw new ArgumentException(
                    $"Invalid character '{front}' in the postfix expression."
                );
            }
        }

        return nums.Peek();
    }
}
