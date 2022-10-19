using BankOcr.Parser.Recognition;
using BankOcr.Parser.TextParsing;

namespace BankOcr.Parser.Initialization;

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
                    new DigitPrototype(i, g, g.Count(c => !char.IsWhiteSpace(c))));
    }
}