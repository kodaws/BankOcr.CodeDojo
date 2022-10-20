using BankOcr.Parser;
using BankOcr.Parser.ChecksumCorrection;
using BankOcr.Parser.Initialization;
using BankOcr.Parser.OutputFormatting;
using BankOcr.Parser.Recognition;
using BankOcr.Parser.TextParsing;
using BankOcr.Parser.Validation;
using CommandLine;

namespace BankOcr.Cli;

public static class Program
{
    private class Options
    {
        [Option('f', "file", HelpText = "Input file path", SetName = "parse")]
        public string FilePath { get; set; }
        
        [Option('g', "generate", HelpText = "Generates single random account number (checksum not verified)", SetName = "generate")]
        public int? GenerateAccounts { get; set; }
        
    }
    public static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                if (o.GenerateAccounts.HasValue)
                    GenerateRandomNumbers(o.GenerateAccounts.Value);
                else
                    ParseFile(o.FilePath).ToList().ForEach(Console.WriteLine);
            });
    }

    private static void GenerateRandomNumbers(int numAccounts)
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

    public static IEnumerable<string> ParseFile(string filePath)
    {
        var glyphEnumerator = new GlyphEnumerator();
        var prototypes = new DigitPrototypeFactory(glyphEnumerator).BuildPrototypes();
        
        var workflow = new RecognitionWorkflow(
            new GlyphEnumerator(), 
            new DigitRecognizer(prototypes),
            new AccountNumberValidator(),
            new AccountNumberCorrector(prototypes),
            new AccountNumberFormatter());

        return File.ReadAllLines(filePath)
            .Chunk(4)
            .Select(l => string.Join(Environment.NewLine, l))
            .Select(workflow.Run);
    }
}