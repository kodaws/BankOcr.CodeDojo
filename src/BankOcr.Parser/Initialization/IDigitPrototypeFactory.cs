using BankOcr.Parser.Models;

namespace BankOcr.Parser.Initialization;

public interface IDigitPrototypeFactory
{
    DigitPrototype[] BuildPrototypes();
}