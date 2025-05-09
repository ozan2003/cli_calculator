using Calculator;

namespace Tests;

[TestClass]
public class UnitTests
{
    private readonly Calculator.Calculator _calculator = new();

    [TestMethod]
    public void TestCalculate()
    {
        Assert.AreEqual(3.0m, _calculator.Calculate("1-(     -2)"));
        Assert.AreEqual(-4.0m, _calculator.Calculate("1+2*-4--3"));
        Assert.AreEqual(-16.0m, _calculator.Calculate("3+5-(2+6)*3"));
        Assert.AreEqual(-22.5m, _calculator.Calculate("24 - 32 + 29 * 3 / (3 - 9)"));
        Assert.AreEqual(8m, _calculator.Calculate("2^3"));
        Assert.AreEqual(18m, _calculator.Calculate("2^3 + 10"));
        Assert.AreEqual(15.5m, _calculator.Calculate("2 ^ 3 + (5 * 3) / 2"));
        Assert.AreEqual(1_679_625.0m, _calculator.Calculate("(1+(4+5+2)-3)+(6 ^ 8)"));
        Assert.AreEqual(-12.0m, _calculator.Calculate("- (3 + (4 + 5))"));
        Assert.AreEqual(2.0m, _calculator.Calculate("1 + 1"));
        Assert.AreEqual(3.0m, _calculator.Calculate(" 2-1 + 2 "));
        Assert.AreEqual(-12.0m, _calculator.Calculate("- (3 - (- (4 + 5) ) )"));
        Assert.AreEqual(
            14m - (4m * 8m - 60m) / 9.0m,
            _calculator.Calculate("14 - (4 * 8 - 60) / 9")
        );
        Assert.AreEqual(-9.0m, _calculator.Calculate("1 + 5 * (-6/3)"));
    }
}
