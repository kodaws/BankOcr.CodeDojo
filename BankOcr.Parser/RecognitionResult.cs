using OneOf;

namespace BankOcr.Parser;

public record RecognizedGlyph(DigitPrototype DigitPrototype);
public record UnrecognizedGlyph(string InputGlyph);

public class RecognitionResult : OneOfBase<RecognizedGlyph, UnrecognizedGlyph>
{
    private RecognitionResult(OneOf<RecognizedGlyph, UnrecognizedGlyph> _) : base(_) {}
    public static implicit operator RecognitionResult(RecognizedGlyph _) => new(_);
    public static implicit operator RecognitionResult(UnrecognizedGlyph _) => new(_);
}