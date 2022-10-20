using BankOcr.Parser.OutputFormatting;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingFullWorkflow : WhenUsingCorrector
{
    protected IAccountNumberFormatter AccountNumberFormatter;
    protected RecognitionWorkflow Workflow;
    
    [OneTimeSetUp]
    public void ConfigureFormatter()
    {
        AccountNumberFormatter = new AccountNumberFormatter();
        Workflow = new RecognitionWorkflow(GlyphEnumerator, Recognizer, AccountNumberValidator,
            AccountNumberCorrector, AccountNumberFormatter);
    }    
}