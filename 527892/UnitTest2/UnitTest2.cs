namespace UnitTest2;
using Code2;

[TestFixture]
public class JsonDuplicateKeyHandlerTests
{
    [Test]
    public void TestEmptyJson()
    {
        string jsonString = "{}";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestSimpleJson()
    {
        string jsonString = "{ \"name\": \"John\", \"age\": 30 }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestJsonWithDuplicateKeys()
    {
        string jsonString = "{ \"name\": \"John\", \"age\": 30, \"name\": \"Jane\" }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestNestedJson()
    {
        string jsonString = "{ \"person\": { \"name\": \"John\", \"age\": 30 }, \"city\": \"New York\" }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestJsonWithArray()
    {
        string jsonString = "{ \"names\": [\"John\", \"Jane\", \"Peter\"] }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestJsonWithDuplicateKeysAndArray()
    {
        string jsonString = "{ \"name\": \"John\", \"name\": [\"Jane\", \"Peter\"] }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestComplexJsonWithDuplicateKeys()
    {
       string jsonString = @"
        {
            ""name"": ""John"",
            ""age"": 30,
            ""name"": ""Jane"",
            ""address"": {
                ""street"": ""123 Main St"",
                ""city"": ""Anytown"",
                ""street"": ""456 Oak Ave""
            },
            ""hobbies"": [""reading"", ""sports"", ""reading""],
            ""name"": 123
        }";

        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestJsonWithNullValue()
    {
        string jsonString = "{ \"name\": null }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

    [Test]
    public void TestJsonWithNumberAndBoolean()
    {
        string jsonString = "{ \"age\": 30, \"isStudent\": true }";
        Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }

     [Test]
    public void TestArrayWithNestedObjectsAndDuplicates() {
        string jsonString = @"
        {
            ""items"": [
                { ""id"": 1, ""name"": ""Product A"", ""id"": 2 },
                { ""id"": 3, ""name"": ""Product B"" }
            ],
            ""items"": [
                { ""id"": 4, ""name"": ""Product C"" }
            ]
        }";
         Assert.DoesNotThrow(() => JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString));
    }
}