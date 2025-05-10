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
        Console.WriteLine("Enter a mathematical expression to calculate the result.");
        Console.WriteLine("Decimal and whole numbers are supported.");
        Console.WriteLine();
        Console.WriteLine("Supported operators:");
        Console.WriteLine(string.Join(" ", Calculator.operators.Keys));
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  help        Display this help message");
        Console.WriteLine("  cls         Clear the console");
        Console.WriteLine("  exit/quit   Exit the calculator");
        Console.WriteLine();
    }

    /// <summary>
    /// Main entry point
    /// </summary>
    public static void Main()
    {
        Calculator calculator = new();

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
                case "cls":
                    try
                    {
                        Console.Clear();
                    }
                    catch (IOException ioexc)
                    {
                        Console.WriteLine($"Error: {ioexc.Message}");
                    }
                    break;
                default:
                    try
                    {
                        decimal result = calculator.Calculate(input);
                        Console.WriteLine($"{result:G}");
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
