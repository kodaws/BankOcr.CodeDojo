using OneOf;

namespace BankOcr.Parser;

public static class AccountNumberValidator
{
    public record AccountNumber(string Number);

    public record InvalidAccountNumber(string Reason, string PartialNumber);

    public record AmbiguousAccountNumber;
 
    const int AccountNumberLength = 9;
    
    public static OneOf<AccountNumber, InvalidAccountNumber> Validate(RecognitionResult[] accountDigits)
    {
        //TODO
        // 1. Refactor result type
        // 2. Refactor string formatting
        if(accountDigits.Length != AccountNumberLength)
            return new InvalidAccountNumber(
                $"Invalid account number length, expected {AccountNumberLength}, got {accountDigits.Length}.",
                string.Join("", 
                    accountDigits.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                                ug => "?"))));

        if(accountDigits.Any(g => g.IsT1))
            return new InvalidAccountNumber(
                $"Unrecognized characters in account number",
                string.Join("", 
                    accountDigits.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                            ug => "?"))));
        
        var checksum =
            accountDigits
                .Select((g, i) =>
                    g.Match(
                        rg => rg.DigitPrototype.Digit * (9 - i),
                        ug => 0)).Sum();
        
        if(checksum % 11 != 0)
            return new InvalidAccountNumber(
                $"Invalid checksum ({checksum} should be divisible by 11)",
                string.Join("", 
                    accountDigits.Select(g => 
                        g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                            ug => "?"))));

        return new AccountNumber(string.Join("",
            accountDigits.Select(g =>
                g.Match(rg => rg.DigitPrototype.Digit.ToString(),
                    ug => "?"))));
    }
}

