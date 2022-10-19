using System.Linq;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

[TestFixture]
public class WhenParsingSingleDigits : WhenUsingRecognizer
{
    [TestCase(0, @"
 _ 
| |
|_|")]
    [TestCase(1, @"
   
  |
  |")]
    [TestCase(2, @"
 _ 
 _|
|_ ")]
    [TestCase(3, @"
 _ 
 _|
 _|")]
    [TestCase(4, @"
   
|_|
  |")]
    [TestCase(5, @"
 _ 
|_ 
 _|")]
    [TestCase(6, @"
 _ 
|_ 
|_|")]
    [TestCase(7, @"
 _ 
  |
  |")]
    [TestCase(8, @"
 _ 
|_|
|_|")]
    [TestCase(9, @"
 _ 
|_|
 _|")]
    public void ShouldRecognizeDigit(int expectedDigit, string input)
    {
        var prototypes = DigitPrototypeFactory.BuildPrototypes().ToArray();
        var matchingDigit = _recognizer.Recognize(input.ReplaceLineEndings(""));
        Assert.AreEqual(expectedDigit, matchingDigit);
    }
}