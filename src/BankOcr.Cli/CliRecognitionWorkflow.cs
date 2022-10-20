using BankOcr.Parser;
using BankOcr.Parser.ChecksumCorrection;
using BankOcr.Parser.Initialization;
using BankOcr.Parser.OutputFormatting;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.TextParsing;
using BankOcr.Parser.Validation;

namespace BankOcr.Cli;

public static class CliRecognitionWorkflow
{
    public static IEnumerable<string> ParseFile(string filePath)
    {
        var glyphEnumerator = new GlyphEnumerator();
        var prototypes = new DigitPrototypeFactory(glyphEnumerator).BuildPrototypes();
        
        var workflow = new RecognitionWorkflow(
            new GlyphEnumerator(), 
            new DigitRecognizer(prototypes),
            new AccountNumberValidator(),
            new AccountNumberCorrector(prototypes),
            new AccountNumberFormatter());

        return File.ReadAllLines(filePath)
            .Chunk(4)
            .Select(l => string.Join(Environment.NewLine, l))
            .Select(workflow.Run);
    }
}