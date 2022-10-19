using OneOf;

namespace BankOcr.Parser;

public record AmbiguousAccountNumber;

public record UnrecognizedCharacters(RecognitionResult[] RecognizedDigits);

public record InvalidChecksum(RecognitionResult[] RecognizedDigits);

public record InvalidAccountNumberLength(RecognitionResult[] RecognizedDigits);

public record ValidAccountNumber(RecognitionResult[] RecognizedDigits);

public class AccountValidationResult : OneOfBase<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum, UnrecognizedCharacters, AmbiguousAccountNumber>
{
    private AccountValidationResult(OneOf<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum, UnrecognizedCharacters, AmbiguousAccountNumber> _) : base(_) {}

    public static implicit operator AccountValidationResult(ValidAccountNumber van) => new(van);
    public static implicit operator AccountValidationResult(InvalidAccountNumberLength ialn) => new(ialn);
    public static implicit operator AccountValidationResult(InvalidChecksum ichk) => new(ichk);
    public static implicit operator AccountValidationResult(UnrecognizedCharacters uchars) => new(uchars);
    public static implicit operator AccountValidationResult(AmbiguousAccountNumber aan) => new(aan);
}

