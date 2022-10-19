using OneOf;

namespace BankOcr.Parser.Validation;

public record UnrecognizedCharacters(Recognition.RecognitionResult[] RecognizedDigits);

public record InvalidChecksum(Recognition.RecognitionResult[] RecognizedDigits);

public record InvalidAccountNumberLength(Recognition.RecognitionResult[] RecognizedDigits);

public record ValidAccountNumber(Recognition.RecognitionResult[] RecognizedDigits);

[GenerateOneOf]
public partial class AccountValidationResult : OneOfBase<ValidAccountNumber, InvalidAccountNumberLength, InvalidChecksum,
    UnrecognizedCharacters> { }
