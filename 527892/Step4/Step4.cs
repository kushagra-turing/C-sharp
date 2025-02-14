namespace Step4;

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public class JsonHandlerWithDuplicates
{
    private Dictionary<string, List<JToken>> DeserializeAndMerge(string json)
    {
        var mergedData = new Dictionary<string, List<JToken>>();
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var key = (string)reader.Value;
                    reader.Read();
                    var value = JToken.ReadFrom(reader);
                    if (!mergedData.ContainsKey(key))
                    {
                        mergedData[key] = new List<JToken>();
                    }
                    mergedData[key].Add(value);
                }
            }
        }

        return mergedData;
    }

    public void PrintJsonWithDuplicates(string jsonString)
    {

        var mergedData = DeserializeAndMerge(jsonString);
        foreach (var kvp in mergedData)
        {
            Console.WriteLine($"{kvp.Key}: [{string.Join(", ", kvp.Value)}]");
        }
    }

    public static void Main(String[] args)
    {
            string json = @"{
                'name': 'John',
                'age': 30,
                'name': 'Jane',
                'age': 25,
                'address': '123 Main St',
                'address': '456 Oak Ave'
            }";
            JsonHandlerWithDuplicates handler = new JsonHandlerWithDuplicates();
            handler.PrintJsonWithDuplicates(json);
    }
}
