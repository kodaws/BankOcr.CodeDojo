namespace BankOcr.Parser.OutputFormatting;

public static class AccountNumberFormatter
{
    public static string Format(Validation.AccountNumber validatedAccount)
    {
        return validatedAccount.Match(
            van => van.RecognitionResults.FormatAccountDigits(),
            inv => inv.Match(
                invLen => invLen.RecognitionResults.FormatAccountDigits() + " LEN",
                invChk => invChk.RecognitionResults.FormatAccountDigits() + " ERR",
                unkChars => unkChars.RecognitionResults.FormatAccountDigits() + " ILL",
                ambNumbers => ambNumbers.ValidCandidates.First().RecognitionResults.FormatAccountDigits() 
            ));
    }

    private static string FormatAccountDigits(this IEnumerable<Recognition.RecognitionResult> digits)
    {
        return string.Join("",
            digits.Select(g =>
                g.Match(
                    rg => rg.DigitPrototype.Digit.ToString(),
                    _ => "?")));
    }
}