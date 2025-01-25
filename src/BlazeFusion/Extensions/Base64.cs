using System.Text.Json;
using System.Text;

namespace BlazeFusion;
internal static class Base64
{
    public static string Serialize(object input)
    {
        if (input == null)
        {
            return null;
        }

        var json = JsonSerializer.Serialize(input, JsonSettings.SerializerSettings);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static object Deserialize(string input, Type outputType)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var bytes = Convert.FromBase64String(input);
        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize(json, outputType);
    }
}
