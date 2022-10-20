using BankOcr.Parser.Validation;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingValidator : WhenUsingRecognizer
{
#pragma warning disable CS8618
    protected IAccountNumberValidator AccountNumberValidator;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void ConfigureValidator()
    {
        AccountNumberValidator = new AccountNumberValidator();
    }    
}