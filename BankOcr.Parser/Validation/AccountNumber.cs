using BankOcr.Parser.Recognition;
using OneOf;

namespace BankOcr.Parser.Validation;

public record UnrecognizedCharacters(RecognitionResult[] RecognitionResults);

public record InvalidChecksum(RecognitionResult[] RecognitionResults);

public record InvalidAccountNumberLength(RecognitionResult[] RecognitionResults);

public record ValidAccountNumber(RecognitionResult[] RecognitionResults);

public record AmbiguousAccountNumber(ValidAccountNumber[] ValidCandidates);

[GenerateOneOf]
public partial class InvalidAccountNumber : OneOfBase<InvalidAccountNumberLength, InvalidChecksum,
    UnrecognizedCharacters, AmbiguousAccountNumber> {}

[GenerateOneOf]
public partial class AccountNumber : OneOfBase<ValidAccountNumber, InvalidAccountNumber> { }