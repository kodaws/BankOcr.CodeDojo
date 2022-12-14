using BankOcr.Parser.Models;

namespace BankOcr.Parser.Initialization;

public interface IDigitPrototypeFactory
{
    // ReSharper disable once UnusedMemberInSuper.Global
    DigitPrototype[] BuildPrototypes();
}