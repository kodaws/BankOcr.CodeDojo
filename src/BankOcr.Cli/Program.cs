using CommandLine;

namespace BankOcr.Cli;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.GenerateAccounts.HasValue)
                        SampleNumberGenerator.GenerateRandomNumbers(o.GenerateAccounts.Value);
                    else
                        CliRecognitionWorkflow.ParseFile(o.FilePath).ToList().ForEach(Console.WriteLine);
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}