namespace UnitTest1;
using Code1;

using NUnit.Framework;
using System;
using System.IO;

[TestFixture]
public class JsonDuplicateHandlerTests
{
    private JsonDuplicateHandler handler;
    private StringWriter consoleOutput;

    [SetUp]
    public void Setup()
    {
        handler = new JsonDuplicateHandler();
        consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput); // Redirect console output to capture it
    }

    [TearDown]
    public void TearDown()
    {
        consoleOutput.Dispose();
        Console.SetOut(Console.Out); // Restore standard output
    }

    [Test]
    public void PrintJsonWithDuplicates_EmptyJson()
    {
        handler.PrintJsonWithDuplicates("");
        Assert.AreEqual("JSON string is empty.\r\n", consoleOutput.ToString());
    }

    [Test]
    public void PrintJsonWithDuplicates_NoDuplicates()
    {
        string json = "{ \"name\": \"John\", \"age\": 30 }";
        handler.PrintJsonWithDuplicates(json);
        Assert.IsTrue(consoleOutput.ToString().Contains("name: John"));
        Assert.IsTrue(consoleOutput.ToString().Contains("age: 30"));
    }

    [Test]
    public void PrintJsonWithDuplicates_SimpleDuplicates()
    {
        string json = "{ \"name\": \"John\", \"name\": \"Jane\" }";
        handler.PrintJsonWithDuplicates(json);
        Assert.IsTrue(consoleOutput.ToString().Contains("name: John, Jane"));
    }

    [Test]
    public void PrintJsonWithDuplicates_MixedDataTypesAndDuplicates()
    {
        string json = "{ \"id\": 1, \"name\": \"Alice\", \"id\": \"2\", \"values\": [10, 20], \"values\": [30] }";
        handler.PrintJsonWithDuplicates(json);

        Assert.IsTrue(consoleOutput.ToString().Contains("id: 1, 2"));
        Assert.IsTrue(consoleOutput.ToString().Contains("name: Alice"));
        Assert.IsTrue(consoleOutput.ToString().Contains("values: 10, 20, 30"));
    }

    [Test]
    public void PrintJsonWithDuplicates_InvalidJson()
    {
        string json = "{ \"name\": \"John\", \"age\": 30, }"; // Trailing comma
        handler.PrintJsonWithDuplicates(json);
        Assert.IsTrue(consoleOutput.ToString().Contains("Error parsing JSON:"));
    }
}