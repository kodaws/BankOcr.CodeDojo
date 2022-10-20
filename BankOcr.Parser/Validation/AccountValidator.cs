using BankOcr.Parser.Recognition;

namespace BankOcr.Parser.Validation;

public class AccountNumberValidator : IAccountNumberValidator
{
    private const int AccountNumberLength = 9;
    
    public AccountNumber Validate(RecognitionResult[] accountDigits)
    {
        if (accountDigits.Length != AccountNumberLength)
            return (InvalidAccountNumber)new InvalidAccountNumberLength(accountDigits);

        if (accountDigits.Any(g => g.IsT1))
            return (InvalidAccountNumber)new UnrecognizedDigits(accountDigits);
        
        var checksum =
            accountDigits
                .Select((d, i) =>
                    d.Match(
                        rg => rg.DigitPrototype.Digit * (9 - i),
                        _ => 0)).Sum();

        if (checksum % 11 != 0)
            return (InvalidAccountNumber)new InvalidChecksum(accountDigits);

        return new ValidAccountNumber(accountDigits);
    }
}