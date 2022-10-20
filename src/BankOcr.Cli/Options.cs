using CommandLine;

namespace BankOcr.Cli;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

/// <summary>
/// Class is instantiated via CommandLineParser, disabling redundant warnings
/// </summary>
internal class Options
{
    [Option('f', "file", HelpText = "Input file path", SetName = "parse")]

    public string FilePath { get; set; }
        
    [Option('g', "generate", HelpText = "Generates single random account number (checksum not verified)", SetName = "generate")]
    public int? GenerateAccounts { get; set; }
}
// ReSharper enable UnusedAutoPropertyAccessor.Global
#pragma warning restore CS8618