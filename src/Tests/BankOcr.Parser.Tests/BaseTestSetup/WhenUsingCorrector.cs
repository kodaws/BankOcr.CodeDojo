using BankOcr.Parser.ChecksumCorrection;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingCorrector : WhenUsingValidator
{
#pragma warning disable CS8618
    protected IAccountNumberCorrector AccountNumberCorrector;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void ConfigureFormatter()
    {
        AccountNumberCorrector = new AccountNumberCorrector(Prototypes);
    }    
}