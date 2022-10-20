namespace BankOcr.Parser.TextParsing;

public interface IGlyphEnumerator
{
    IEnumerable<string> EnumerateGlyphs(string input);
}