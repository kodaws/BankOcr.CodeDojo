namespace BankOcr.Parser;

public class DigitRecognizer
{
    private readonly IDictionary<string, DigitPrototype> _prototypes;

    public DigitRecognizer(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypes = prototypes.ToDictionary(k => k.Glyph, v => v);
    }

    public int Recognize(string glyph)
    {
        //TODO: handle missing values
        return _prototypes[glyph].Digit;
    }
}