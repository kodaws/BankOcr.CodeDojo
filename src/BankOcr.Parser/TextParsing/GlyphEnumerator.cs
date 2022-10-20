namespace BankOcr.Parser.TextParsing;

public class GlyphEnumerator : IGlyphEnumerator
{
    private const int NumCharsPerGlyph = 9;
    private const int NumGlyphCharsPerLine = 3;
    
    public IEnumerable<string> EnumerateGlyphs(string input)
    {
        var noBreaks = input.ReplaceLineEndings("");
        
        if(noBreaks.Length % NumCharsPerGlyph != 0)
            throw new Exception($"Invalid input length - {noBreaks.Length} is not divisible by {NumCharsPerGlyph}");
       
        var numGlyphs = noBreaks.Length / NumCharsPerGlyph;
        return
            noBreaks
                .Select((c, i) => new {Character = c, CharPos = i})
                .GroupBy(g => g.CharPos / NumGlyphCharsPerLine) //reproduce lines, TODO: refactor unnecessary steps
                .Select((line, i) =>
                    new
                    {
                        Line = line.Select(l => l.Character),
                        DigitPos = i % numGlyphs
                    }) //assign digit position
                .GroupBy(lineWithPos => lineWithPos.DigitPos) //group by digit number
                .Select(g =>
                    string.Concat(g.SelectMany(v => v.Line))); //compact grouped lines into glyphs
    }
}