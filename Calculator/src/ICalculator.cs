namespace Calculator;

/// <summary>
/// Interface for calculator operations
/// </summary>
public interface ICalculator
{
    /// <summary>
    /// Calculates the result of a mathematical expression
    /// </summary>
    /// <param name="expression">Expression to calculate</param>
    /// <returns>The result of the calculation</returns>
    double Calculate(string expression);
}
