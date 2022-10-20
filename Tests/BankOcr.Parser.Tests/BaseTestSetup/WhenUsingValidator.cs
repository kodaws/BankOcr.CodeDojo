using BankOcr.Parser.Validation;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingValidator : WhenUsingRecognizer
{
    protected IAccountNumberValidator AccountNumberValidator;
    
    [OneTimeSetUp]
    public void ConfigureValidator()
    {
        AccountNumberValidator = new AccountNumberValidator();
    }    
}