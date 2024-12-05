namespace CT.Models;

/// <summary>
/// Programos kodo parametrų konfigūracinė klasė
/// </summary>
public class CodeParametersDto
{
    public double ErrorRate { get; set; }          // p
    public int CodeDimension { get; set; }         // k
    public int CodeLength { get; set; }            // n
    public byte[][]? GeneratorMatrix { get; set; } // G
}