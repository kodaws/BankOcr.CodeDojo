using NUnit.Framework;

namespace BankOcr.Parser.Tests;

public abstract class WhenUsingRecognizer
{
#pragma warning disable CS8618
    //initialized by NUnit at runtime, lacks proper support for static code inspection
    protected DigitRecognizer Recognizer;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void SetUp()
    {
        var prototypes = DigitPrototypeFactory.BuildPrototypes();
        Recognizer = new DigitRecognizer(prototypes);
    }    
}