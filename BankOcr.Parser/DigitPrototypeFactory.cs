namespace BankOcr.Parser;

public static class DigitPrototypeFactory
{
    public static IEnumerable<DigitPrototype> BuildPrototypes()
    {
        return
            @"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|"
                .EnumerateGlyphs()
                .Select((g, i) =>
                    new DigitPrototype(
                        Digit: i,
                        Glyph: g,
                        NumElems: g.Count(c => !char.IsWhiteSpace(c))));
    }
}