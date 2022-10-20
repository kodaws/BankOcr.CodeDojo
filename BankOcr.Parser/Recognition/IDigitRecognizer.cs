namespace BankOcr.Parser.Recognition;

public interface IDigitRecognizer
{
    RecognitionResult Recognize(string glyph);
}