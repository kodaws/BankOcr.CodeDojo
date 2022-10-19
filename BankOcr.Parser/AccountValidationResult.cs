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

    public static implicit operator AccountValidationResult(ValidAccountNumber _) => new(_);
    public static implicit operator AccountValidationResult(InvalidAccountNumberLength _) => new(_);
    public static implicit operator AccountValidationResult(InvalidChecksum _) => new(_);
    public static implicit operator AccountValidationResult(UnrecognizedCharacters _) => new(_);
    public static implicit operator AccountValidationResult(AmbiguousAccountNumber _) => new(_);
}

