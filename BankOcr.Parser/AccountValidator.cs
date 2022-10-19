using OneOf;

namespace BankOcr.Parser;

public static class AccountNumberValidator
{
    public record AccountNumber(string Number);

    public record InvalidAccountNumber(string Reason, string PartialNumber);

    public record AmbiguousAccountNumber;

    public class InputGlyph : OneOfBase<RecognizedGlyph, UnrecognizedGlyph>
    {
        InputGlyph(OneOf<RecognizedGlyph, UnrecognizedGlyph> _) : base(_) {}
        public static implicit operator InputGlyph(RecognizedGlyph _) => new(_);
        public static implicit operator InputGlyph(UnrecognizedGlyph _) => new(_);
    }
    const int AccountNumberLength = 9;
    
    public static OneOf<AccountNumber, InvalidAccountNumber> Validate(InputGlyph[] accountNumbers)
    {
        if(accountNumbers.Length != AccountNumberLength)
            return new InvalidAccountNumber(
                $"Invalid account number length, expected {AccountNumberLength}, got {accountNumbers.Length}.",
                string.Join("", 
                    accountNumbers.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                                ug => "?"))));

        if(accountNumbers.Any(g => g.IsT1))
            return new InvalidAccountNumber(
                $"Unrecognized characters in account number",
                string.Join("", 
                    accountNumbers.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                            ug => "?"))));
        
        var checksum =
            accountNumbers
                .Select((g, i) =>
                    g.Match(
                        rg => rg.DigitPrototype.Digit * (9 - i),
                        ug => 0)).Sum();
        
        if(checksum % 11 != 0)
            return new InvalidAccountNumber(
                $"Invalid checksum ({checksum} should be divisible by 11)",
                string.Join("", 
                    accountNumbers.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                            ug => "?"))));

        return new AccountNumber(string.Join("",
            accountNumbers.Select(g =>
                g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                    ug => "?"))));
    }
}

