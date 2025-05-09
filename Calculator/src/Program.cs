namespace Calculator;

/// <summary>
/// Entry point for the Calculator application
/// </summary>
public class Program
{
    private const string prompt = "> ";

    /// <summary>
    /// Main entry point
    /// </summary>
    public static void Main()
    {
        Calculator calculator = new();

        Console.WriteLine("RPN Calculator");
        Console.WriteLine("Type help for more information.");
        Console.WriteLine();

        while (true)
        {
            Console.Write(prompt);

            string input = Console.ReadLine() ?? string.Empty;

            switch (input.Trim().ToLower())
            {
                case "":
                    continue;
                case "exit":
                case "quit":
                    return;
                default:
                    try
                    {
                        Console.WriteLine(calculator.Calculate(input));
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
