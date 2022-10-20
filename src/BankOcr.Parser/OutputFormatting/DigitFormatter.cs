using BankOcr.Parser.Recognition;

namespace BankOcr.Parser.OutputFormatting;

public static class DigitFormatter
{
    public static string FormatAccountDigits(this IEnumerable<RecognitionResult> digits)
    {
        return string.Join("",
            digits.Select(g =>
                g.Match(
                    rg => rg.DigitPrototype.Digit.ToString(),
                    _ => "?")));
    }
}