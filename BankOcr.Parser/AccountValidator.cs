using OneOf;

namespace BankOcr.Parser;

public static class AccountNumberValidator
{
    public record ValidAccountNumber(RecognitionResult[] RecognizedDigits);

    public record InvalidChecksum(RecognitionResult[] RecognizedDigits);

    public record UnrecognizedCharacters(RecognitionResult[] RecognizedDigits);

    public record InvalidAccountNumberLength(RecognitionResult[] RecognizedDigits);

    public class AccountValidationResult : OneOfBase<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum, UnrecognizedCharacters, AmbiguousAccountNumber>
    {
        private AccountValidationResult(OneOf<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum, UnrecognizedCharacters, AmbiguousAccountNumber> _) : base(_) {}

        public static implicit operator AccountValidationResult(ValidAccountNumber van) => new(van);
        public static implicit operator AccountValidationResult(InvalidAccountNumberLength ialn) => new(ialn);
        public static implicit operator AccountValidationResult(InvalidChecksum ichk) => new(ichk);
        public static implicit operator AccountValidationResult(UnrecognizedCharacters uchars) => new(uchars);
        public static implicit operator AccountValidationResult(AmbiguousAccountNumber aan) => new(aan);
    }

    public record AmbiguousAccountNumber;
 
    const int AccountNumberLength = 9;
    
    public static AccountValidationResult Validate(RecognitionResult[] accountDigits)
    {
        if (accountDigits.Length != AccountNumberLength)
            return new InvalidAccountNumberLength(accountDigits);

        if (accountDigits.Any(g => g.IsT1))
            return new UnrecognizedCharacters(accountDigits);
        
        var checksum =
            accountDigits
                .Select((g, i) =>
                    g.Match(
                        rg => rg.DigitPrototype.Digit * (9 - i),
                        ug => 0)).Sum();

        if (checksum % 11 != 0)
            return new InvalidChecksum(accountDigits);

        return new ValidAccountNumber(accountDigits);
    }
}

