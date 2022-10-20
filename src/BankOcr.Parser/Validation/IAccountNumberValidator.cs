using BankOcr.Parser.Recognition;

namespace BankOcr.Parser.Validation;

public interface IAccountNumberValidator
{
    AccountNumber Validate(RecognitionResult[] accountDigits);
}