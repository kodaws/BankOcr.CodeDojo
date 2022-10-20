using BankOcr.Parser.ChecksumCorrection;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingCorrector : WhenUsingValidator
{
    protected IAccountNumberCorrector AccountNumberCorrector;
    
    [OneTimeSetUp]
    public void ConfigureFormatter()
    {
        AccountNumberCorrector = new AccountNumberCorrector(Prototypes);
    }    
}