using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace VocabularyTrainer2.Source.Common
{
    public class Utility
    {
        public static void WriteRed(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteRedLine(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteGreen(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteGreenLine(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static string ReadLine()
        {
            var line = Console.ReadLine();

            if (line == null)
                throw new IOException("Unable to read line from standard input.");

            return line;
        }

        public static void SaveToFileAsJson<T>(string fileName, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            });

            File.WriteAllText(fileName, json);
        }
    }
}
