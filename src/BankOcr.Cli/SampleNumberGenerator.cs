using BankOcr.Parser.Initialization;
using BankOcr.Parser.TextParsing;

namespace BankOcr.Cli;

/// <summary>
/// Quick and dirty test case generator
/// </summary>
public static class SampleNumberGenerator
{
    public static void GenerateRandomNumbers(int numAccounts)
    {
        for(var i = 0; i < numAccounts; i++)
            GenerateSingleNumber();
    }

    private static void GenerateSingleNumber()
    {
        const int numDigitsPerAccount = 9;
        const int numRowsPerDigit = 3;

        var prototypes = new DigitPrototypeFactory(new GlyphEnumerator()).BuildPrototypes();
        var glyphs =
            Enumerable
                .Range(0, numDigitsPerAccount)
                .Select(_ => new Random().Next(0, 9))
                .Select(n => prototypes[n].Glyph)
                .SelectMany(a => a)
                .ToArray();

        for (var row = 0; row < numRowsPerDigit; row++)
        {
            for (var digNum = 0; digNum < numDigitsPerAccount; digNum++)
            {
                var pos = digNum * numDigitsPerAccount + row * numRowsPerDigit;
                Console.Write(glyphs[pos]);
                Console.Write(glyphs[pos + 1]);
                Console.Write(glyphs[pos + 2]);
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }
}