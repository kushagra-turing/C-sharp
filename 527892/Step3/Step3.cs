namespace Code527892;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class JsonWithDuplicateHandler
{
    public void printJsonWithDuplicates(string jsonString)
    {
        try
        {
            using (JsonTextReader reader = new JsonTextReader(new StringReader(jsonString)))
            {
                reader.DateParseHandling = DateParseHandling.None;
                JToken result = ReadAndCombine(reader);
                Console.WriteLine(result.ToString(Formatting.Indented));
            }
        }
        catch (JsonReaderException ex)
        {
            Console.WriteLine($"Error: Invalid JSON format. {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private JToken ReadAndCombine(JsonTextReader reader)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            reader.Read();
            return null;
        }

        if (reader.TokenType == JsonToken.StartObject)
        {
            return ReadObject(reader);
        }
        else if (reader.TokenType == JsonToken.StartArray)
        {
            return ReadArray(reader);
        }
        else
        {
            // Handle primitive types (string, number, boolean, null)
            JValue value = new JValue(reader.Value);
            reader.Read();
            return value;
        }
    }

    private JObject ReadObject(JsonTextReader reader)
    {
        JObject obj = new JObject();
        reader.Read(); // Read the StartObject token

        while (reader.TokenType != JsonToken.EndObject)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();
                reader.Read(); // Read the property name

                JToken value = ReadAndCombine(reader);

                if (obj[propertyName] == null)
                {
                    obj[propertyName] = value;
                }
                else
                {
                    // Handle duplicate keys.  If both are arrays, combine them.  Otherwise, make an array.
                    JToken existingValue = obj[propertyName];

                    if (existingValue is JArray && value is JArray)
                    {
                        // Combine two arrays
                        foreach (var item in (JArray)value)
                        {
                            ((JArray)existingValue).Add(item);
                        }
                    }
                    else if (existingValue is JArray)
                    {
                        // Add the new value to the existing array
                        ((JArray)existingValue).Add(value);
                    }
                    else
                    {
                        // Create a new array and add both values
                        JArray newArray = new JArray();
                        newArray.Add(existingValue);
                        newArray.Add(value);
                        obj[propertyName] = newArray;
                    }
                }
            }
            else
            {
                //Handle unexpected token.  Could throw an exception, but skipping for robustness
                reader.Read();
            }
        }

        reader.Read(); // Read the EndObject token
        return obj;
    }

    private JArray ReadArray(JsonTextReader reader)
    {
        JArray array = new JArray();
        reader.Read(); // Read the StartArray token

        while (reader.TokenType != JsonToken.EndArray)
        {
            JToken value = ReadAndCombine(reader);
            array.Add(value);
        }

        reader.Read(); // Read the EndArray token
        return array;
    }

    public static void Main(String[] args)
    {
        string jsonString = "{ \"name\": \"John\", \"age\": 3}";
        JsonWithDuplicateHandler handler = new JsonWithDuplicateHandler();
        handler.printJsonWithDuplicates(jsonString);
    }
}