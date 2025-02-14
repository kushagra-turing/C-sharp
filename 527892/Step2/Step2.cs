namespace Code2;

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class JsonDuplicateKeyHandler
{
    public static void PrintJsonWithDuplicates(string jsonString)
    {
        try
        {
            var parsedObject = ParseJson(jsonString);
            PrintParsedObject(parsedObject);
        }
        catch (JsonReaderException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static Dictionary<string, List<object>> ParseJson(string jsonString)
    {
        var result = new Dictionary<string, List<object>>();
        JToken token = JToken.Parse(jsonString);

        PopulateDictionary(token, result);

        return result;
    }

    private static void PopulateDictionary(JToken token, Dictionary<string, List<object>> result, string prefix = "")
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (JProperty property in (JObject)token)
                {
                    PopulateDictionary(property.Value, result, property.Name);
                }
                break;
            case JTokenType.Array:
                int index = 0;
                foreach (JToken item in (JArray)token)
                {
                    PopulateDictionary(item, result, $"{prefix}[{index}]");
                    index++;
                }
                break;
            case JTokenType.Property:
                JProperty prop = (JProperty)token;
                PopulateDictionary(prop.Value, result, prop.Name);
                break;
            default:
                // Handle primitive values (string, number, boolean, null)
                string key = prefix;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!result.ContainsKey(key))
                    {
                        result[key] = new List<object>();
                    }

                    object value = ((JValue)token).Value;
                    result[key].Add(value);
                }
                break;
        }
    }

    private static void PrintParsedObject(Dictionary<string, List<object>> data)
    {
        foreach (var kvp in data)
        {
            Console.WriteLine($"Key: {kvp.Key}");
            foreach (var value in kvp.Value)
            {
                Console.WriteLine($"  Value: {value}");
            }
        }
    }

    public static void Main(String[] args)
    {
        string jsonString = "{ \"name\": \"John\", \"age\": 30, \"name\": \"Jane\" }";
        JsonDuplicateKeyHandler.PrintJsonWithDuplicates(jsonString);
    }
}