using System.Linq;
using BankOcr.Parser.Initialization;
using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.TextParsing;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup
{
    public abstract class WhenUsingRecognizer
    {
#pragma warning disable CS8618
        //initialized by NUnit at runtime, lacks proper support for static code inspection
        protected IDigitRecognizer Recognizer;
        protected DigitPrototype[] Prototypes;
        protected IGlyphEnumerator GlyphEnumerator;
#pragma warning restore CS8618
    
        [OneTimeSetUp]
        public void ConfigureRecognizer()
        {
            GlyphEnumerator = new GlyphEnumerator();
            Prototypes = new DigitPrototypeFactory(GlyphEnumerator).BuildPrototypes().ToArray();
            Recognizer = new DigitRecognizer(Prototypes);
        }    
    }
}