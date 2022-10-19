using System.Diagnostics;

namespace BankOcr.Parser;

public static class GlyphEnumerator
{
    private const int NumCharsPerGlyph = 9;
    private const int NumCharsPerGlyphPerLine = 3;
    public static IEnumerable<string> EnumerateGlyphs(this string input)
    {
        var inputSanitized = input.ReplaceLineEndings("");
        var length = inputSanitized.Length;
        
        Debug.Assert(length % NumCharsPerGlyph == 0); //TODO: improve with result?
        var numGlyphs = length / NumCharsPerGlyph;

        //TODO: improve with explicit key selector
        return inputSanitized
            .Select((c, i) => new { Character = c, Position = i })
            .GroupBy(g => g.Position / NumCharsPerGlyphPerLine)
            .Select((line, i) => new {Line = line.Select(l => l.Character), Digit = i % numGlyphs})
            .GroupBy(elem => elem.Digit)
            .Select(g => new string(g.SelectMany(v => v.Line).ToArray()));
    }
}