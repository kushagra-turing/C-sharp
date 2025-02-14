namespace Code527892;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class JsonDuplicateHandler
{
    public void PrintJsonWithDuplicates(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            Console.WriteLine("JSON string is empty.");
            return;
        }

        try
        {
            JObject jsonObject = JObject.Parse(jsonString);
            Dictionary<string, List<object>> combinedData = new Dictionary<string, List<object>>();

            foreach (var property in jsonObject.Properties())
            {
                string key = property.Name;
                object value = property.Value;

                if (!combinedData.ContainsKey(key))
                {
                    combinedData[key] = new List<object>();
                }

                //If the value is a JArray, add all elements, otherwise add the single value
                if (value is JArray array)
                {
                    foreach (var item in array)
                    {
                        combinedData[key].Add(item.ToString());
                    }
                }
                else
                {
                    combinedData[key].Add(value.ToString());
                }
            }

            // Display the combined data
            foreach (var kvp in combinedData)
            {
                Console.WriteLine($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
            }
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

    public static void Main(String[] args)
    {
        JsonDuplicateHandler handler = new JsonDuplicateHandler();
        string json = "{ \"id\": 1, \"name\": \"Alice\", \"id\": \"2\", \"values\": [10, 20], \"values\": [30] }"; 
        handler.PrintJsonWithDuplicates(json);
    }
}
