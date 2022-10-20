using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.Validation;

namespace BankOcr.Parser.ChecksumCorrection;

public class AccountNumberCorrector : IAccountNumberCorrector
{
    private readonly ILookup<int, DigitPrototype> _prototypesByNumStrokes;
    private record DigitWithPosition(int Position, RecognitionResult Digit);

    public AccountNumberCorrector(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypesByNumStrokes = prototypes.ToLookup(k => k.NumStrokes, v => v);
    }

    public AccountNumber TryCorrect(InvalidAccountNumber invalidAccountNumber)
    {
        return invalidAccountNumber.Match(
            _ => invalidAccountNumber, //can't fix incorrect number of chars, just pass it back
            TryCorrectInvalidChecksum,
            TryCorrectUnrecognizedDigits);
    }

    private AccountNumber TryCorrectInvalidChecksum(InvalidChecksum invalidChecksum)
    {
        var digitWithPosition =
            invalidChecksum
                .RecognitionResults
                .Select((d, i) => new DigitWithPosition(i, d))
                .ToArray();

        var validAlternatives = 
            Enumerable.Range(0, 9)
                .Select(pos => TryCorrectSingleDigit(digitWithPosition, pos))
                .SelectMany(an => an)
                .OrderBy(ord =>
                    ord.RecognitionResults.Select((d, i) => d.AsT0.DigitPrototype.Digit * Math.Pow(10, 9-i)).Sum()
                )
                .ToArray();

        return new AmbiguousAccountNumber(invalidChecksum, validAlternatives);
    }

    private AccountNumber TryCorrectUnrecognizedDigits(UnrecognizedDigits unrecognizedDigits)
    {
        var digitsWithPositions =
            unrecognizedDigits
                .RecognitionResults
                .Select((recResult, i) => new DigitWithPosition(i, recResult))
                .ToArray();

        var digitsToFix = digitsWithPositions.Where(c => c.Digit.IsT1).ToArray();
            
        if (digitsToFix.Length != 1)
            return (InvalidAccountNumber)unrecognizedDigits; //can't fix number with other than one unrecognized digit, just pass on

        var fixableDigit = digitsToFix.First();
        return new AmbiguousAccountNumber(unrecognizedDigits, TryCorrectSingleDigit(digitsWithPositions, fixableDigit.Position));
    }

    private ValidAccountNumber[] TryCorrectSingleDigit(IReadOnlyList<DigitWithPosition> origDigits, int positionToFix)
    {
        var partialChecksum =
            origDigits
                .Where(dwp => dwp.Position != positionToFix)
                .Select(c => c.Digit.AsT0.DigitPrototype.Digit * (9 - c.Position))
                .Sum();

        var numStrokes = 
            origDigits[positionToFix].Digit.Match(
                rg => rg.DigitPrototype.NumStrokes, 
                ug => ug.InputGlyph.Count(c => !char.IsWhiteSpace(c)));
        
        var glyph = 
            origDigits[positionToFix].Digit.Match(
                rg => rg.DigitPrototype.Glyph, 
                ug => ug.InputGlyph);

        var candidates = FindGlyphReplacementCandidates(numStrokes, glyph);
        var matchingCandidates = FilterCandidatesWithMatchingChecksum(positionToFix, candidates, partialChecksum);

        var head = origDigits.Take(positionToFix).ToArray();
        var tail = origDigits.Skip(positionToFix + 1).ToArray();
        
        var verifiedAlternatives =
            matchingCandidates
                .Select(match =>
                    head
                    .Append(new DigitWithPosition(positionToFix, new RecognizedGlyph(new DigitPrototype(match, "", 0))))
                    .Concat(tail)
                )
                .ToArray();

        return verifiedAlternatives.Select(ca => 
            new ValidAccountNumber(ca.Select(c => c.Digit).ToArray()))
            .ToArray();
    }

    private static IEnumerable<int> FilterCandidatesWithMatchingChecksum(int positionToFix, IEnumerable<DigitPrototype> candidates,
        int partialChecksum)
    {
        return 
            candidates
                .Select(candidate => new
                {
                    candidate.Digit,
                    AlternativeChecksum = partialChecksum + (9 - positionToFix) * candidate.Digit
                })
                .Where(ver => ver.AlternativeChecksum % 11 == 0)
                .Select(ver => ver.Digit);
    }

    /// <summary>
    /// Find all glyph prototypes that differ by only one dash or pipe character.
    /// Optimized by using precomputed lookup that groups prototypes by number of "strokes" (either dash or pipe) 
    /// </summary>
    private IEnumerable<DigitPrototype> FindGlyphReplacementCandidates(int numStrokes, string glyph)
    {
        return _prototypesByNumStrokes[numStrokes - 1]
            .Union(_prototypesByNumStrokes[numStrokes + 1])
            .Where(candidate => 
                candidate
                    .Glyph
                    .Zip(glyph)
                    .Count(match => match.First != match.Second) == 1);
    }
}