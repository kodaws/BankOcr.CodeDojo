using System.Linq;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenValidatingAccountNumber
{
    [TestCase("711111111", true)]
    [TestCase("123456789", true)]
    [TestCase("490867715", true)]
    [TestCase("888888888", false)]
    [TestCase("490067715", false)]
    [TestCase("012345678", false)]
    public void ShouldDetectInvalidNumbers(string accountNumber, bool isValid)
    {
        var accNumber = accountNumber.Select(d => int.Parse(d.ToString()))
            .Select(d => (RecognitionResult)new RecognizedGlyph(new DigitPrototype(d, "", 0))) //TODO: refactor
            .ToArray();
        var validationResult = AccountNumberValidator.Validate(accNumber);
        Assert.AreEqual(isValid, validationResult.IsT0);
    }
}