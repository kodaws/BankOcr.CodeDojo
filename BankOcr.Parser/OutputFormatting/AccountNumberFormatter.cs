using BankOcr.Parser.Recognition;

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
                ambNumbers =>
                    (ambNumbers.ValidCandidates.Length == 1) ?
                        (ambNumbers.ValidCandidates.First().RecognitionResults.FormatAccountDigits()) :
                        (ambNumbers.OriginalNumber.Match(
                                invLen => invLen.RecognitionResults,
                                invChecksum => invChecksum.RecognitionResults, 
                                unrecChars => unrecChars.RecognitionResults, 
                                amb => Array.Empty<RecognitionResult>()).FormatAccountDigits() + 
                                $" AMB [{string.Join(", ", ambNumbers.ValidCandidates.Select(c => $"'{c.RecognitionResults.FormatAccountDigits()}'"))}]"))); 
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