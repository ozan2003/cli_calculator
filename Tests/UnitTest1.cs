using System;

namespace Tests;

[TestClass]
public class UnitTests
{
    private readonly Calculator.Calculator m_calculator = new();

    [TestMethod]
    public void TestCalculate()
    {
        Assert.AreEqual(3.0m, m_calculator.Calculate("1-(     -2)"));
        Assert.AreEqual(-4.0m, m_calculator.Calculate("1+2*-4--3"));
        Assert.AreEqual(-16.0m, m_calculator.Calculate("3+5-(2+6)*3"));
        Assert.AreEqual(
            -22.5m,
            m_calculator.Calculate("24 - 32 + 29 * 3 / (3 - 9)")
        );
        Assert.AreEqual(8m, m_calculator.Calculate("2^3"));
        Assert.AreEqual(18m, m_calculator.Calculate("2^3 + 10"));
        Assert.AreEqual(15.5m, m_calculator.Calculate("2 ^ 3 + (5 * 3) / 2"));
        Assert.AreEqual(
            1_679_625.0m,
            m_calculator.Calculate("(1+(4+5+2)-3)+(6 ^ 8)")
        );
        Assert.AreEqual(-12.0m, m_calculator.Calculate("- (3 + (4 + 5))"));
        Assert.AreEqual(2.0m, m_calculator.Calculate("1 + 1"));
        Assert.AreEqual(3.0m, m_calculator.Calculate(" 2-1 + 2 "));
        Assert.AreEqual(
            -12.0m,
            m_calculator.Calculate("- (3 - (- (4 + 5) ) )")
        );
        Assert.AreEqual(
            14m - (4m * 8m - 60m) / 9.0m,
            m_calculator.Calculate("14 - (4 * 8 - 60) / 9")
        );
        Assert.AreEqual(-9.0m, m_calculator.Calculate("1 + 5 * (-6/3)"));
    }

    [TestMethod]
    public void TestDecimalCalculations()
    {
        // Basic decimal operations
        Assert.AreEqual(3.5m, m_calculator.Calculate("1.5 + 2"));
        Assert.AreEqual(1.5m, m_calculator.Calculate("3.5 - 2"));
        Assert.AreEqual(7.5m, m_calculator.Calculate("3 * 2.5"));
        Assert.AreEqual(1.5m, m_calculator.Calculate("3 / 2"));

        // Leading decimal point (with implicit 0)
        Assert.AreEqual(0.5m, m_calculator.Calculate(".5"));
        Assert.AreEqual(1.75m, m_calculator.Calculate("1 + .75"));

        // Expressions with multiple decimal points
        Assert.AreEqual(4.95m, m_calculator.Calculate("2.1 + 2.85"));
        Assert.AreEqual(0.25m, m_calculator.Calculate("1.5 - 1.25"));

        // Complex expressions with decimals
        Assert.AreEqual(10.75m, m_calculator.Calculate("2.5 * 4 + 0.75"));
        Assert.AreEqual(-3.375m, m_calculator.Calculate("2.25 - 5.625"));

        // Decimal exponentiation
        Assert.AreEqual(8.0m, m_calculator.Calculate("2.0 ^ 3"));
        Assert.AreEqual(2.25m, m_calculator.Calculate("1.5 ^ 2"));

        // Precision tests with many decimal places
        Assert.AreEqual(
            0.3333333333333333333333333333m,
            m_calculator.Calculate("1 / 3")
        );
        Assert.AreEqual(
            0.1428571428571428571428571429m,
            m_calculator.Calculate("1 / 7")
        );

        // Calculations with negative decimals
        Assert.AreEqual(-1.5m, m_calculator.Calculate("0.5 * -3"));
        Assert.AreEqual(-0.5m, m_calculator.Calculate("-1.5 + 1"));

        // Operations with decimal numbers in parentheses
        Assert.AreEqual(
            5.5m,
            m_calculator.Calculate("(2.5 + 3) * (4.5 - 3.5)")
        );
        Assert.AreEqual(0.8m, m_calculator.Calculate("(1.2 * 2) / 3"));
    }

    [TestMethod]
    public void Calculate_SimpleAddition_ReturnsCorrectResult()
    {
        decimal result = m_calculator.Calculate("1 + 2");
        Assert.AreEqual(3, result);
    }

    [TestMethod]
    public void Calculate_DivisionByZero_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("1 / 0")
        );
    }

    [TestMethod]
    public void Calculate_EmptyExpression_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("")
        );
    }

    [TestMethod]
    public void Calculate_WhitespaceExpression_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("   ")
        );
    }

    [TestMethod]
    public void Calculate_InvalidCharacter_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("1 + a")
        );
    }

    [TestMethod]
    public void Calculate_UnmatchedOpeningParenthesis_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("(1 + 2")
        );
    }

    [TestMethod]
    public void Calculate_UnmatchedClosingParenthesis_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("1 + 2)")
        );
    }

    [TestMethod]
    public void Calculate_MultipleDecimalPoints_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("1.2.3")
        );
    }

    [TestMethod]
    public void Calculate_InsufficientOperands_ThrowsException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            m_calculator.Calculate("1 +")
        );
    }
}
