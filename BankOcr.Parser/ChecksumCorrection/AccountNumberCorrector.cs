using System.Text;
using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.Validation;

namespace BankOcr.Parser.ChecksumCorrection;

public class AccountNumberCorrector
{
    private readonly ILookup<int, DigitPrototype> _prototypesByNumElems;

    public AccountNumberCorrector(IEnumerable<DigitPrototype> prototypes)
    {
        _prototypesByNumElems = prototypes.ToLookup(k => k.NumElems, v => v);
    }

    public AccountNumber TryCorrect(InvalidAccountNumber invalidAccountNumber)
    {
        return invalidAccountNumber.Match(
            _ => invalidAccountNumber, //can't fix incorrect number of chars, just pass it back
            TryCorrectInvalidChecksum,
            TryCorrectUnrecognizedChars,
            _ => invalidAccountNumber);//TODO: fix odd entry
    }

    private AccountNumber TryCorrectInvalidChecksum(InvalidChecksum invalidChecksum)
    {
        var characterWithPosition =
            invalidChecksum.RecognitionResults.Select((c, i) => new CharacterWithPosition(i, c))//TODO unify character vs digit naming
                .ToArray();

        var validCandidates = Enumerable.Range(0, 9)
            .Select(p => TryCorrectSingleDigit(characterWithPosition, p))
            .SelectMany(c => c)
            .OrderBy(ord =>
            {
                var result = new StringBuilder();
                foreach (var item in ord.RecognitionResults)
                {
                    result.Append(item.AsT0.DigitPrototype.Digit);
                }

                return result.ToString();
            })
            .ToArray();

        return new InvalidAccountNumber(new AmbiguousAccountNumber(invalidChecksum, validCandidates));
    }

    private record CharacterWithPosition(int Position, RecognitionResult Digit);

    private AccountNumber TryCorrectUnrecognizedChars(UnrecognizedCharacters unrecognizedCharacters)
    {
        var characterWithPosition =
            unrecognizedCharacters.RecognitionResults.Select((c, i) => new CharacterWithPosition(i, c))//TODO unify character vs digit naming
                .ToArray();

        var charactersToFix = characterWithPosition.Where(c => c.Digit.IsT1).ToArray();
            
        if (charactersToFix.Length > 1)
            return (InvalidAccountNumber)unrecognizedCharacters; //can't fix more than one unrecognized digit

        var fixableChar = charactersToFix.First();

        return new InvalidAccountNumber(new AmbiguousAccountNumber(unrecognizedCharacters, TryCorrectSingleDigit(characterWithPosition, fixableChar.Position)));
    }

    private ValidAccountNumber[] TryCorrectSingleDigit(CharacterWithPosition[] digits, int position)
    {
        var checksumFromRecognizedChars =
            digits
                .Where(cwp => cwp.Position != position)
                .Select(c => c.Digit.AsT0.DigitPrototype.Digit * (9 - c.Position))
                .Sum();

        var fixableCharNumElems = digits[position].Digit.Match(rg => rg.DigitPrototype.NumElems, ug => ug.InputGlyph.Count(c => !char.IsWhiteSpace(c)));
        var fixableCharGlyph = digits[position].Digit.Match(rg => rg.DigitPrototype.Glyph, ug => ug.InputGlyph);

        var replacementCandidates =
            _prototypesByNumElems[fixableCharNumElems - 1]
                .Union(_prototypesByNumElems[fixableCharNumElems + 1])
                .Where(cand => cand.Glyph.Zip(fixableCharGlyph).Count(match => match.First != match.Second) == 1);

        var head = digits.Take(position).ToArray();
        var tail = digits.Skip(position + 1).ToArray();
        var candidateAccounts =
            replacementCandidates
                .Select(cand => new
                {
                    Digit = cand.Digit,
                    VersionChecksum = checksumFromRecognizedChars + (9 - position) * cand.Digit
                })
                .Where(ver => ver.VersionChecksum % 11 == 0)
                .Select(ver =>
                        head
                            .Append(new CharacterWithPosition(position, new RecognizedGlyph(new DigitPrototype(ver.Digit, "", 0))))
                            .Concat(tail) //tail
                )
                .ToArray();

        return candidateAccounts.Select(ca => new ValidAccountNumber(ca.Select(c => c.Digit).ToArray())).ToArray();
    }
}