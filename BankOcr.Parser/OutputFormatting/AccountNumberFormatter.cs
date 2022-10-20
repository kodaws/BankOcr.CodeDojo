using BankOcr.Parser.Validation;

namespace BankOcr.Parser.OutputFormatting;

public class AccountNumberFormatter : IAccountNumberFormatter
{
    public string Format(AccountNumber account)
    {
        return account.Match(
            van => van.RecognitionResults.FormatAccountDigits(),
            inv => inv.Match(
                invLen => invLen.RecognitionResults.FormatAccountDigits() + " LEN",
                invChk => invChk.RecognitionResults.FormatAccountDigits() + " ERR",
                unkChars => unkChars.RecognitionResults.FormatAccountDigits() + " ILL"
                ),
            ambNumber =>
                ambNumber.ValidCandidates.Length == 1 ?
                    ambNumber.ValidCandidates.First().RecognitionResults.FormatAccountDigits() :
                    ambNumber.OriginalNumber.Match(
                        invLen => invLen.RecognitionResults,
                        invChecksum => invChecksum.RecognitionResults, 
                        unkChars => unkChars.RecognitionResults).FormatAccountDigits() + 
                    $" AMB [{string.Join(", ", ambNumber.ValidCandidates.Select(c => $"'{c.RecognitionResults.FormatAccountDigits()}'"))}]"); 
    }
}