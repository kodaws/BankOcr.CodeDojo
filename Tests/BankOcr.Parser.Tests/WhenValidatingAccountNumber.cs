using System.Linq;
using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.Tests.BaseTestSetup;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenValidatingAccountNumber : WhenUsingValidator
{
    [TestCase("711111111", true)]
    [TestCase("123456789", true)]
    [TestCase("490867715", true)]
    [TestCase("888888888", false)]
    [TestCase("490067715", false)]
    [TestCase("012345678", false)]
    public void ShouldDetectInvalidNumbers(string accountDigits, bool isValid)
    {
        var validationResult = AccountNumberValidator.Validate(ConvertFromLiteral(accountDigits));
        Assert.AreEqual(isValid, validationResult.IsT0);
    }

    private RecognitionResult[] ConvertFromLiteral(string accountDigits)
    {
        return accountDigits.Select(d => int.Parse(d.ToString()))
            .Select(d => (RecognitionResult)new RecognizedGlyph(new DigitPrototype(d, "ignored", 0)))
            .ToArray();
    }
}