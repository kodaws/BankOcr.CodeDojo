namespace BankOcr.Parser;

public abstract record InputGlyph(string Glyph);
public record RecognizedGlyph(DigitPrototype DigitPrototype, string Glyph) : InputGlyph(Glyph);
public record UnrecognizedGlyph(string Glyph) : InputGlyph(Glyph);