using System.Diagnostics;

namespace BankOcr.Parser.TextParsing;

public class GlyphEnumerator : IGlyphEnumerator
{
    private const int NumCharsPerGlyph = 9;
    private const int NumGlyphCharsPerLine = 3;
    
    public IEnumerable<string> EnumerateGlyphs(string input)
    {
        var noBreaks = input.ReplaceLineEndings("");
        
        Debug.Assert(noBreaks.Length % NumCharsPerGlyph == 0); //TODO: improve with result?
        
        //TODO: refactor it all
        var numGlyphs = noBreaks.Length / NumCharsPerGlyph;
        //TODO: improve with explicit group key selector
        return noBreaks
            .Select((c, i) => new { Character = c, CharPos = i }) //TODO: maybe something exists already
            .GroupBy(g => g.CharPos / NumGlyphCharsPerLine)
            .Select((line, i) => new { Line = line.Select(l => l.Character), GlyphPos = i % numGlyphs })
            .GroupBy(lineWithPos => lineWithPos.GlyphPos)
            .Select(g => new string(g.SelectMany(v => v.Line).ToArray()));
    }
}