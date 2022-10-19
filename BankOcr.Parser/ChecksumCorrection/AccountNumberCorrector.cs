using BankOcr.Parser.Models;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.Validation;

namespace BankOcr.Parser.ChecksumCorrection;

public class AccountNumberCorrector
{
    private ILookup<int, DigitPrototype> _prototypesByNumElems;

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
            _ => invalidAccountNumber);
    }

    private AccountNumber TryCorrectInvalidChecksum(InvalidChecksum invalidChecksum)
    {
        return null;
    }

    private AccountNumber TryCorrectUnrecognizedChars(UnrecognizedCharacters unrecognizedCharacters)
    {
        var characterWithPosition =
            unrecognizedCharacters.RecognitionResults.Select((c, i) => new {Position = i, Digit = c})
                .ToArray();

        var charactersToFix = characterWithPosition.Where(c => c.Digit.IsT1).ToArray();
            
        if (charactersToFix.Length > 1)
            return (InvalidAccountNumber)unrecognizedCharacters; //can't fix more than one unrecognized digit

        var fixableChar = charactersToFix.First();

        var checksumFromRecognizedChars =
            characterWithPosition
                .Except(charactersToFix)
                .Select(c => c.Digit.AsT0.DigitPrototype.Digit * (9 - c.Position))
                .Sum();

        var fixableCharNumElems = fixableChar.Digit.AsT1.InputGlyph.Count(c => !char.IsWhiteSpace(c));
        var replacementCandidates =
            _prototypesByNumElems[fixableCharNumElems - 1]
                .Union(_prototypesByNumElems[fixableCharNumElems + 1]);

        var head = unrecognizedCharacters.RecognitionResults.Take(fixableChar.Position).ToArray();
        var tail = unrecognizedCharacters.RecognitionResults.Skip(fixableChar.Position + 1).ToArray();
        var candidateAccounts =
            replacementCandidates
                .Select(cand => new
                {
                    Digit = cand.Digit,
                    VersionChecksum = checksumFromRecognizedChars + (9 - fixableChar.Position) * cand.Digit
                })
                .Where(ver => ver.VersionChecksum % 11 == 0)
                .Select(ver =>
                    head
                        .Append(new RecognizedGlyph(new DigitPrototype(ver.Digit, "", 0)))
                        .Concat(tail) //tail
                ).ToArray();
        
        return new InvalidAccountNumber(new AmbiguousAccountNumber(candidateAccounts.Select(ca => new ValidAccountNumber(ca.ToArray())).ToArray()));
    }
}