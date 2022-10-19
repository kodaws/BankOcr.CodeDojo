using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenParsingFullAccountNumbers : WhenUsingRecognizer
{
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
| || || || || || || || || |
|_||_||_||_||_||_||_||_||_|", "000000000")]
    [TestCase(@"
                           
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |", "111111111")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
|_ |_ |_ |_ |_ |_ |_ |_ |_ ", "222222222")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
 _| _| _| _| _| _| _| _| _|
 _| _| _| _| _| _| _| _| _|", "333333333")]
    [TestCase(@"
                           
|_||_||_||_||_||_||_||_||_|
  |  |  |  |  |  |  |  |  |", "444444444")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
 _| _| _| _| _| _| _| _| _|", "555555555")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
|_ |_ |_ |_ |_ |_ |_ |_ |_ 
|_||_||_||_||_||_||_||_||_|", "666666666")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |", "777777777")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
|_||_||_||_||_||_||_||_||_|", "888888888")]
    [TestCase(@"
 _  _  _  _  _  _  _  _  _ 
|_||_||_||_||_||_||_||_||_|
 _| _| _| _| _| _| _| _| _|", "999999999")]
    [TestCase(@"
    _  _     _  _  _  _  _ 
  | _| _||_||_ |_   ||_||_|
  ||_  _|  | _||_|  ||_| _|", "123456789")]
    public void ShouldParseTheNumber(string input, string expectedResult)
    {
        var digits = input.EnumerateGlyphs();
        var accountNumber = digits
            .Select(_recognizer.Recognize)
            .Select(d => d.AsT0.DigitPrototype.Digit);
        
        var accountNumText = accountNumber.Aggregate(new StringBuilder(), (state, d) =>
        {
            state.Append(d);
            return state;
        });
        Assert.AreEqual(expectedResult, accountNumText.ToString());
    }
}