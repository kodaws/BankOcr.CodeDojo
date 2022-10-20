using BankOcr.Cli;
using NUnit.Framework;

namespace Bank.Ocr.Integration.Tests;

public class WhenParsingInputFiles
{
    [TestCase("TestData\\RandomNumbers.Input.txt", "TestData\\RandomNumbers.Answers.txt")]
    public void ShouldProcessLargeFiles(string inputFilePath, string answersFilePath)
    {
        var actualAnswers = Program.ParseFile(inputFilePath); 
        var expectedAnswers = System.IO.File.ReadAllLines(answersFilePath);
        CollectionAssert.AreEqual(expectedAnswers, actualAnswers);
    }
}