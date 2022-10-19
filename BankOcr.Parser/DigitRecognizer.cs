namespace BankOcr.Parser;
using OneOf;

public class DigitRecognizer
{
    private readonly IDictionary<string, DigitPrototype> _prototypes;

    public DigitRecognizer(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypes = prototypes.ToDictionary(k => k.Glyph, v => v);
    }

    public OneOf<int, UnrecognizedGlyph> Recognize(string glyph)
    {
        return _prototypes.ContainsKey(glyph) ? 
            _prototypes[glyph].Digit : 
            new UnrecognizedGlyph(glyph);
    }
}