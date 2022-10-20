using BankOcr.Parser.Models;
using BankOcr.Parser.TextParsing;
namespace BankOcr.Parser.Initialization;

public class DigitPrototypeFactory : IDigitPrototypeFactory
{
    private readonly IGlyphEnumerator _glyphEnumerator;

    public DigitPrototypeFactory(IGlyphEnumerator glyphEnumerator)
    {
        _glyphEnumerator = glyphEnumerator;
    }
    
    public DigitPrototype[] BuildPrototypes()
    {
        return
            _glyphEnumerator.EnumerateGlyphs(@"
 _     _  _     _  _  _  _  _ 
| |  | _| _||_||_ |_   ||_||_|
|_|  ||_  _|  | _||_|  ||_| _|")
                .Select((glyph, i) =>
                    new DigitPrototype(i, glyph, glyph.Count(c => !char.IsWhiteSpace(c))))
                .ToArray();
    }
}