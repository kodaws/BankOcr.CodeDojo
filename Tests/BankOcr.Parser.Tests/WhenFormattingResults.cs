using System.Linq;
using BankOcr.Parser.OutputFormatting;
using BankOcr.Parser.Tests.BaseTestSetup;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenFormattingResults : WhenUsingValidator
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
    
    [TestCase( @"
 _     _  _  _  _  _  _  _ 
|_ |_||    |  ||_  _|| | _ 
 _|  ||_|  |  ||_| _||_| _|", "54?77630? ILL")]
    public void ShouldWritePartialMatchAndResult(string input, string expectedResult)
    {
        var accountNumber = 
                GlyphEnumerator
                    .EnumerateGlyphs(input)
                    .Select(Recognizer.Recognize)
                    .ToArray();
        
        var validationResult = AccountNumberValidator.Validate(accountNumber);
        var formatted = new AccountNumberFormatter().Format(validationResult);
        Assert.AreEqual(expectedResult, formatted);
    }
}