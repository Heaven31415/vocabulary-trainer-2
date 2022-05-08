using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System;

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

        public static void WriteGreen(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteRedLine(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
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

        public static string ComputeHash(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("input cannot be null, empty or whitespace.", nameof(input));

            using SHA256 hashAlgorithm = SHA256.Create();

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));

            return stringBuilder.ToString();
        }

        public static void SaveToFileAsJson<T>(string fileName, T data)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null, empty or whitespace.", nameof(fileName));

            var extension = Path.GetExtension(fileName);

            if (extension == null || extension != ".json")
                throw new IOException($"fileName needs to have a .json extension.");

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
