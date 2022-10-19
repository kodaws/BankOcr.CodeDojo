namespace BankOcr.Parser.Validation;

public static class AccountNumberValidator
{
    private const int AccountNumberLength = 9;
    
    public static AccountValidationResult Validate(Recognition.RecognitionResult[] accountDigits)
    {
        if (accountDigits.Length != AccountNumberLength)
            return new InvalidAccountNumberLength(accountDigits);

        if (accountDigits.Any(g => g.IsT1))
            return new UnrecognizedCharacters(accountDigits);
        
        var checksum =
            accountDigits
                .Select((d, i) =>
                    d.Match(
                        rg => rg.DigitPrototype.Digit * (9 - i),
                        _ => 0)).Sum();

        if (checksum % 11 != 0)
            return new InvalidChecksum(accountDigits);

        return new ValidAccountNumber(accountDigits);
    }
}