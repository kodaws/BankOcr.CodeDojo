using OneOf;

namespace BankOcr.Parser;

public record UnrecognizedCharacters(RecognitionResult[] RecognizedDigits);

public record InvalidChecksum(RecognitionResult[] RecognizedDigits);

public record InvalidAccountNumberLength(RecognitionResult[] RecognizedDigits);

public record ValidAccountNumber(RecognitionResult[] RecognizedDigits);

[GenerateOneOf]
public partial class AccountValidationResult : OneOfBase<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum,
    UnrecognizedCharacters> { }
