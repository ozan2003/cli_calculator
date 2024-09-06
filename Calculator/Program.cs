using System.Collections.Generic;
using System.Text;
using Calculator;

namespace Calculator;

internal class Calculator
{
    private static readonly Dictionary<char, Operator> @operator = new()
    {
        {'(', new('(', 4, Associavity.Left)},
        {')', new(')', 4, Associavity.Left)},
        {'+', new('+', 1, Associavity.Left)},
        {'-', new('-', 1, Associavity.Left)},
        {'*', new('*', 2, Associavity.Left)},
        {'/', new('/', 2, Associavity.Left)},
        {'^', new('^', 3, Associavity.Right)},
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
    /// The corresponding operator if the character is an operator, null otherwise.
    /// </returns>
    private static Operator? IsOperator(in char ch) =>
        @operator.TryGetValue(ch, out Operator? op) ? op : null;
   

    /// <summary>
    /// Converts the infix expression to postfix notation.
    /// </summary>
    /// 
    /// <param name="input">
    /// The infix expression to convert.
    /// </param>
    /// 
    /// <returns>
    /// The postfix notation of the input expression.
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// If there are invalid characters in the infix expression.
    /// i.e. characters other than digits, arithmetic operators, and parentheses.
    /// </exception>
    private static string ToPostfix(in string input)
    {
        // Remove all whitespaces and convert to a list of characters.
        StringBuilder infix = new(input.Length + 1); // +1 for the possible extra '0' at the beginning.

        if (input.StartsWith('-'))
        {
            infix.Append('0');
        }
        // Handle negative numbers at the beginning of the expression.
        infix.Append(string.Concat(input.Where(ch => !char.IsWhiteSpace(ch))));

        //---------------------------------------------------------------------------
        Stack<Operator> operator_stack = new();
        StringBuilder postfix = new(infix.Length);

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
            // Handle numbers (negative).
            // Whenever '-' comes in string, check if there's a number before it.
            // If not push '0' then push '-'.
            else if (infix[i] == '-' && (i == 0 || infix[i - 1] == '('))
            {
                postfix.Append("0 ");
                //operator_stack.Push(infix[i]);
                operator_stack.Push(@operator['-']);
            }
            // If it's an operator.
            else if (IsOperator(infix[i]) is var opr && opr is not null)
            {
                while (operator_stack.Count != 0 && operator_stack.Peek().Priority >= opr.Priority)
                {
                    postfix.Append(operator_stack.Pop().Opr);
                    postfix.Append(' ');
                }
                operator_stack.Push(opr);
            }
#if false
            else if (IsOperator(infix[i]))
            {
                while (
                    operators.Count != 0
                    && GetPriority(operators.Peek()) >= GetPriority(infix[i])
                )
                {
                    postfix.Append(operators.Pop());
                    postfix.Append(' ');
                }
                operators.Push(infix[i]);
            }
#endif
            // Opening parenthesis
            else if (infix[i] == '(')
            {
                //operator_stack.Push(infix[i]);
                operator_stack.Push(@operator['(']);
            }
            // Closing parenthesis
            else if (infix[i] == ')')
            {
                while (operator_stack.Count != 0 && operator_stack.Peek().Opr != '(')
                {
                    postfix.Append(operator_stack.Pop().Opr);
                    postfix.Append(' ');
                }
                operator_stack.Pop(); // Remove '(' from stack
            }
#if false
            else if (infix[i] == ')')
            {
                while (operator_stack.Count != 0 && operator_stack.Peek() != '(')
                {
                    postfix.Append(operator_stack.Pop());
                    postfix.Append(' ');
                }
                operator_stack.Pop(); // Remove '(' from stack
            }
#endif
            // Whitespace
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
                    default:
                        break;
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