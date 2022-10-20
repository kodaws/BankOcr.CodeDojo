using BankOcr.Parser.Validation;

namespace BankOcr.Parser.OutputFormatting;

public interface IAccountNumberFormatter
{
    string Format(AccountNumber account);
}