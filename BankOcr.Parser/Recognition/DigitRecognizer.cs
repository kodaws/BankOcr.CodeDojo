namespace BankOcr.Parser.Recognition;

public class DigitRecognizer
{
    private readonly IDictionary<string, DigitPrototype> _prototypes;

    public DigitRecognizer(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypes = prototypes.ToDictionary(k => k.Glyph, v => v); //TODO: compare performance with ToImmutableDictionary
    }

    public RecognitionResult Recognize(string glyph)
    {
        return _prototypes.ContainsKey(glyph) ? 
            new RecognizedGlyph(_prototypes[glyph]): 
            new UnrecognizedGlyph(glyph);
    }
}