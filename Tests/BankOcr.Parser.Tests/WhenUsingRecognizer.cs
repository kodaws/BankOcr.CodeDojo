using System.Linq;
using BankOcr.Parser.Initialization;
using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using NUnit.Framework;

namespace BankOcr.Parser.Tests;

public abstract class WhenUsingRecognizer
{
#pragma warning disable CS8618
    //initialized by NUnit at runtime, lacks proper support for static code inspection
    protected DigitRecognizer Recognizer;
    protected DigitPrototype[] Prototypes;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void SetUp()
    {
        Prototypes = DigitPrototypeFactory.BuildPrototypes().ToArray();
        Recognizer = new DigitRecognizer(Prototypes);
    }    
}