using BankOcr.Parser.Validation;

namespace BankOcr.Parser.ChecksumCorrection;

public interface IAccountNumberCorrector
{
    AccountNumber TryCorrect(InvalidAccountNumber invalidAccountNumber);
}