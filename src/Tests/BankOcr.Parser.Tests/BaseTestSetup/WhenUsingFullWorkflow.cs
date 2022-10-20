using BankOcr.Parser.OutputFormatting;
using NUnit.Framework;

namespace BankOcr.Parser.Tests.BaseTestSetup;

public abstract class WhenUsingFullWorkflow : WhenUsingCorrector
{
#pragma warning disable CS8618
    protected RecognitionWorkflow Workflow;
#pragma warning restore CS8618
    
    [OneTimeSetUp]
    public void ConfigureFullWorkflow()
    {
        var accountNumberFormatter = new AccountNumberFormatter();
        Workflow = new RecognitionWorkflow(GlyphEnumerator, Recognizer, AccountNumberValidator,
            AccountNumberCorrector, accountNumberFormatter);
    }    
}