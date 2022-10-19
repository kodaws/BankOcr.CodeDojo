using OneOf;

namespace BankOcr.Parser.Recognition;

public record RecognizedGlyph(DigitPrototype DigitPrototype);
public record UnrecognizedGlyph(string InputGlyph);

[GenerateOneOf]
public partial class RecognitionResult : OneOfBase<RecognizedGlyph, UnrecognizedGlyph> {}