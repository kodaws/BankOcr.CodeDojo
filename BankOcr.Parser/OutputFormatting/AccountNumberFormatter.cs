namespace BankOcr.Parser;

public static class AccountNumberFormatter
{
    public static string Format(Validation.AccountValidationResult validatedAccount)
    {
        return validatedAccount.Match(
            van => van.RecognizedDigits.FormatAccountDigits(),
            invLen => invLen.RecognizedDigits.FormatAccountDigits() + " LEN",
            invChk => invChk.RecognizedDigits.FormatAccountDigits() + " ERR",
            unkChars => unkChars.RecognizedDigits.FormatAccountDigits() + " ILL");
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