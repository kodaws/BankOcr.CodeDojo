using BankOcr.Parser.Recognition;
using OneOf;

namespace BankOcr.Parser.Validation;

public record UnrecognizedDigits(RecognitionResult[] RecognitionResults);

public record InvalidChecksum(RecognitionResult[] RecognitionResults);

public record InvalidAccountNumberLength(RecognitionResult[] RecognitionResults);

public record ValidAccountNumber(RecognitionResult[] RecognitionResults);

public record AmbiguousAccountNumber(InvalidAccountNumber OriginalNumber, ValidAccountNumber[] ValidCandidates);

[GenerateOneOf]
public partial class InvalidAccountNumber : OneOfBase<InvalidAccountNumberLength, InvalidChecksum, UnrecognizedDigits> {}

[GenerateOneOf]
public partial class AccountNumber : OneOfBase<ValidAccountNumber, InvalidAccountNumber, AmbiguousAccountNumber> { }