using OneOf;

namespace BankOcr.Parser;

public record RecognizedGlyph(DigitPrototype DigitPrototype);
public record UnrecognizedGlyph(string InputGlyph);

[GenerateOneOf]
public partial class RecognitionResult : OneOfBase<RecognizedGlyph, UnrecognizedGlyph> {}