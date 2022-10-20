using BankOcr.Parser.ChecksumCorrection;
using BankOcr.Parser.OutputFormatting;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.TextParsing;
using BankOcr.Parser.Validation;

namespace BankOcr.Parser;

public class RecognitionWorkflow
{
    private readonly IGlyphEnumerator _glyphEnumerator;
    private readonly IDigitRecognizer _digitRecognizer;
    private readonly IAccountNumberValidator _validator;
    private readonly IAccountNumberCorrector _corrector;
    private readonly IAccountNumberFormatter _formatter;

    public RecognitionWorkflow(
        IGlyphEnumerator glyphEnumerator,
        IDigitRecognizer digitRecognizer,
        IAccountNumberValidator validator,
        IAccountNumberCorrector corrector,
        IAccountNumberFormatter formatter
        )
    {
        _glyphEnumerator = glyphEnumerator;
        _digitRecognizer = digitRecognizer;
        _validator = validator;
        _corrector = corrector;
        _formatter = formatter;
    }

    public string Run(string input)
    {
        var recognitionResult = _glyphEnumerator
            .EnumerateGlyphs(input)
            .Select(_digitRecognizer.Recognize)
            .ToArray();
            
         return 
             _validator.Validate(recognitionResult).Match(
                 ok => _formatter.Format(ok),
                 invalid => _formatter.Format(_corrector.TryCorrect(invalid)),
                 amb => _formatter.Format(amb));
    }
}