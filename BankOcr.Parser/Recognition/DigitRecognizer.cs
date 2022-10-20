using BankOcr.Parser.Models;

namespace BankOcr.Parser.Recognition;

public class DigitRecognizer : IDigitRecognizer
{
    private readonly IDictionary<string, DigitPrototype> _prototypes;

    public DigitRecognizer(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypes = prototypes.ToDictionary(k => k.Glyph, v => v);
    }

    public RecognitionResult Recognize(string glyph)
    {
        return _prototypes.ContainsKey(glyph) ? 
            new RecognizedGlyph(_prototypes[glyph]): 
            new UnrecognizedGlyph(glyph);
    }
}