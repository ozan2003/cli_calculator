namespace Program;

public class Program
{
    public static void Test(Dictionary<string, double> tests)
    {
        foreach (var (test, result) in tests)
        {
            var actual_result = Calculator.Calculator.Calculate(test);
            if (actual_result != result)
            {
                Console.WriteLine(
                    "Test failed for '{0}'.\nActual result: {1}\nExpected: {2}",
                    test,
                    result,
                    actual_result
                );
            }
            else
            {
                Console.WriteLine("Passed!");
            }
        }
    }

    public static void Main()
    {
        Dictionary<string, double> testcases =
            new()
            {
                { "3+5-(2+6)*3", -16.0 },
                { "24 - 32 + 29 * 3 / (3 - 9)", -22.5 },
                { "2^3", 8 },
                { "2^3 + 10", 18 },
                { "2 ^ 3 + (5 * 3) / 2", 15.5 },
                { "(1+(4+5+2)-3)+(6 ^ 8)", 1_679_625.0 },
                { "- (3 + (4 + 5))", -12.0 },
                { "1-(     -2)", 3.0 },
                { "1 + 1", 2.0 },
                { " 2-1 + 2 ", 3.0 },
                { "- (3 - (- (4 + 5) ) )", -12.0 },
                { "14 - (4 * 8 - 60) / 9", 14 - (4 * 8 - 60) / 9.0 },
            };

        Test(testcases);
    }
}
