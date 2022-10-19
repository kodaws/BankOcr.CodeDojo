using NUnit.Framework;

namespace BankOcr.Parser.Tests;

public abstract class WhenUsingRecognizer
{
    protected DigitRecognizer _recognizer;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        var prototypes = DigitPrototypeFactory.BuildPrototypes();
        _recognizer = new DigitRecognizer(prototypes);
    }    
}