namespace UnitTest3;
using Code3;

using NUnit;

[TestFixture]
public class JsonWithDuplicateHandlerTests
{
    private JsonWithDuplicateHandler handler;
    private StringWriter consoleOutput;

    [SetUp]
    public void Setup()
    {
        handler = new JsonWithDuplicateHandler();
        consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(Console.Out); // Restore standard output
    }


    [Test]
    public void Test_NoDuplicates()
    {
        string json = "{ \"name\": \"John\", \"age\": 30 }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""name"": ""John"",
  ""age"": 30
}".Replace("\r\n", "\n"); //Normalize for different environments
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_SimpleDuplicates()
    {
        string json = "{ \"name\": \"John\", \"name\": \"Doe\" }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""name"": [
    ""John"",
    ""Doe""
  ]
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_MixedDuplicates()
    {
        string json = "{ \"name\": \"John\", \"age\": 30, \"name\": \"Doe\" }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""name"": [
    ""John"",
    ""Doe""
  ],
  ""age"": 30
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_NestedJsonWithDuplicates()
    {
        string json = "{ \"name\": \"John\", \"address\": { \"street\": \"Main St\", \"street\": \"Oak St\" } }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""name"": ""John"",
  ""address"": {
    ""street"": [
      ""Main St"",
      ""Oak St""
    ]
  }
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_ArrayDuplicates()
    {
        string json = "{ \"items\": [1, 2], \"items\": [3, 4] }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""items"": [
    1,
    2,
    3,
    4
  ]
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }


    [Test]
    public void Test_InvalidJson()
    {
        string json = "This is not JSON";
        handler.printJsonWithDuplicates(json);
        Assert.IsTrue(consoleOutput.ToString().StartsWith("Error: Invalid JSON format."));
    }

    [Test]
    public void Test_EmptyJson()
    {
        string json = "{}";
        handler.printJsonWithDuplicates(json);
        Assert.AreEqual("{}".Replace("\r\n", "\n"), consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_NullValue()
    {
        string json = "{ \"name\": null }";
        handler.printJsonWithDuplicates(json);
        string expected = @"{
  ""name"": null
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

    [Test]
    public void Test_DuplicateNullValues()
    {
        string json = "{ \"name\": null, \"name\": null }";
        handler.printJsonWithDuplicates(json);
                string expected = @"{
  ""name"": [
    null,
    null
  ]
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

     [Test]
    public void Test_ComplexNestedDuplicates()
    {
        string json = @"{
            ""name"": ""John"",
            ""address"": {
                ""street"": ""Main St"",
                ""city"": ""Anytown"",
                ""street"": ""Second St""
            },
            ""age"": 30,
            ""address"": {
                ""zip"": ""12345""
            },
             ""name"": ""Jane""
        }";

        handler.printJsonWithDuplicates(json);

        string expected = @"{
  ""name"": [
    ""John"",
    ""Jane""
  ],
  ""address"": [
    {
      ""street"": [
        ""Main St"",
        ""Second St""
      ],
      ""city"": ""Anytown""
    },
    {
      ""zip"": ""12345""
    }
  ],
  ""age"": 30
}".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }


    [Test]
    public void Test_ArrayOfObjectsWithDuplicates()
    {
        string json = @"[
            { ""id"": 1, ""name"": ""Apple"", ""id"": 2 },
            { ""id"": 3, ""name"": ""Banana"" }
        ]";

        handler.printJsonWithDuplicates(json);

        string expected = @"[
  {
    ""id"": [
      1,
      2
    ],
    ""name"": ""Apple""
  },
  {
    ""id"": 3,
    ""name"": ""Banana""
  }
]".Replace("\r\n", "\n");
        Assert.AreEqual(expected, consoleOutput.ToString().Replace("\r\n", "\n"));
    }

}