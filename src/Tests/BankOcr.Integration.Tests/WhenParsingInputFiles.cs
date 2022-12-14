using BankOcr.Cli;
using NUnit.Framework;

namespace BankOcr.Integration.Tests;

public class WhenParsingInputFiles
{
    [TestCase("TestData\\RandomNumbers.Input.txt", "TestData\\RandomNumbers.Answers.txt")]
    public void ShouldProcessLargeFiles(string inputFilePath, string answersFilePath)
    {
        var actualAnswers = CliRecognitionWorkflow.ParseFile(inputFilePath); 
        var expectedAnswers = System.IO.File.ReadAllLines(answersFilePath);
        CollectionAssert.AreEqual(expectedAnswers, actualAnswers);
    }
}