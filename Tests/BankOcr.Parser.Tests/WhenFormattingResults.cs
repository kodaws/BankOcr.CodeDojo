using System.Linq;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenFormattingResults : WhenUsingRecognizer
{
    [TestCase(@"
 _  _  _  _  _  _  _  _    
| || || || || || || ||_   |
|_||_||_||_||_||_||_| _|  |", "000000051")]
    [TestCase(@"
    _  _  _  _  _  _     _ 
|_||_|| || ||_   |  |  | _ 
  | _||_||_||_|  |  |  | _|", "49006771? ILL")]
    [TestCase(@"
    _  _     _  _  _  _  _ 
  | _| _||_| _ |_   ||_||_|
  ||_  _|  | _||_|  ||_| _ ", "1234?678? ILL")]
    public void ShouldWritePartialMatchAndResult(string input, string expectedResult)
    {
        var digits = input.EnumerateGlyphs();
        var accountNumber = digits
            .Select(Recognizer.Recognize)
            .ToArray();
        
        var validationResult = AccountNumberValidator.Validate(accountNumber);
        var formatted = AccountNumberFormatter.Format(validationResult);
        Assert.AreEqual(expectedResult, formatted);
    }
}