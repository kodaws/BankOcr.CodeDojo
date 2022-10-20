using BankOcr.Parser.Models;

namespace BankOcr.Parser.Initialization;

public interface IDigitPrototypeFactory
{
    IEnumerable<DigitPrototype> BuildPrototypes();
}