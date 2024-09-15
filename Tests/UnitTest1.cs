using Calculator;

namespace Tests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void TestCalculate()
    {
        Assert.AreEqual(3.0, Calculator.Calculator.Calculate("1-(     -2)"));
        Assert.AreEqual(-4.0, Calculator.Calculator.Calculate("1+2*-4--3"));
        Assert.AreEqual(-16.0, Calculator.Calculator.Calculate("3+5-(2+6)*3"));
        Assert.AreEqual(-22.5, Calculator.Calculator.Calculate("24 - 32 + 29 * 3 / (3 - 9)"));
        Assert.AreEqual(8, Calculator.Calculator.Calculate("2^3"));
        Assert.AreEqual(18, Calculator.Calculator.Calculate("2^3 + 10"));
        Assert.AreEqual(15.5, Calculator.Calculator.Calculate("2 ^ 3 + (5 * 3) / 2"));
        Assert.AreEqual(1_679_625.0, Calculator.Calculator.Calculate("(1+(4+5+2)-3)+(6 ^ 8)"));
        Assert.AreEqual(-12.0, Calculator.Calculator.Calculate("- (3 + (4 + 5))"));
        Assert.AreEqual(2.0, Calculator.Calculator.Calculate("1 + 1"));
        Assert.AreEqual(3.0, Calculator.Calculator.Calculate(" 2-1 + 2 "));
        Assert.AreEqual(-12.0, Calculator.Calculator.Calculate("- (3 - (- (4 + 5) ) )"));
        Assert.AreEqual(
            14 - (4 * 8 - 60) / 9.0,
            Calculator.Calculator.Calculate("14 - (4 * 8 - 60) / 9")
        );
        Assert.AreEqual(-9.0, Calculator.Calculator.Calculate("1 + 5 * (-6/3)"));
    }
}
