using BankOcr.Parser.OutputFormatting;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingFullWorkflow : WhenUsingCorrector
{
#pragma warning disable CS8618
    protected IAccountNumberFormatter AccountNumberFormatter;
    protected RecognitionWorkflow Workflow;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void ConfigureFullWorkflow()
    {
        AccountNumberFormatter = new AccountNumberFormatter();
        Workflow = new RecognitionWorkflow(GlyphEnumerator, Recognizer, AccountNumberValidator,
            AccountNumberCorrector, AccountNumberFormatter);
    }    
}