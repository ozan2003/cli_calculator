namespace Calculator;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Type an expression to calculate or 'exit' to quit.");
        while (true)
        {
            Console.Write("> ");

            string input = Console.ReadLine() ?? string.Empty;

            switch (input)
            {
                case "":
                    continue;
                case "exit":
                    return;
                default:
                    try
                    {
                        Console.WriteLine(Calculator.Calculate(input));
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                    break;
            }
        }
    }
}
