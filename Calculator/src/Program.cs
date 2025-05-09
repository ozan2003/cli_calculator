namespace Calculator;

/// <summary>
/// Entry point for the Calculator application
/// </summary>
public class Program
{
    private const string prompt = "> ";

    /// <summary>
    /// Displays the help message
    /// </summary>
    private static void DisplayHelp()
    {
        Console.WriteLine("Calculator Help");
        Console.WriteLine("==============");
        Console.WriteLine("Enter a mathematical expression to calculate the result.");
        Console.WriteLine();
        Console.WriteLine("Supported operations:");
        Console.WriteLine("  +    Addition");
        Console.WriteLine("  -    Subtraction");
        Console.WriteLine("  *    Multiplication");
        Console.WriteLine("  /    Division");
        Console.WriteLine("  ^    Exponentiation (power)");
        Console.WriteLine("  ()   Parentheses for grouping");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  2 + 3 * 4       = 14");
        Console.WriteLine("  (2 + 3) * 4     = 20");
        Console.WriteLine("  2^3 + 10        = 18");
        Console.WriteLine("  5 / 2           = 2.5");
        Console.WriteLine("  -5 + 3          = -2");
        Console.WriteLine("  3.14 * 2        = 6.28");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  help        Display this help message");
        Console.WriteLine("  exit/quit   Exit the calculator");
        Console.WriteLine();
    }

    /// <summary>
    /// Main entry point
    /// </summary>
    public static void Main()
    {
        ICalculator calculator = new Calculator();

        Console.WriteLine("Calculator - Type 'help' for usage information");
        Console.WriteLine("Type an expression to calculate or 'exit' to quit.");
        Console.WriteLine();

        while (true)
        {
            Console.Write(prompt);

            string input = Console.ReadLine() ?? string.Empty;
            string command = input.Trim().ToLower();

            switch (command)
            {
                case "":
                    continue;
                case "exit":
                case "quit":
                    return;
                case "help":
                    DisplayHelp();
                    break;
                default:
                    try
                    {
                        decimal result = calculator.Calculate(input);
                        // Format the result to avoid unnecessary decimal places.
                        string formattedResult =
                            result % 1 == 0 ? result.ToString("0") : result.ToString();
                        Console.WriteLine(formattedResult);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"Error: {exc.Message}");
                    }
                    break;
            }
        }
    }
}
