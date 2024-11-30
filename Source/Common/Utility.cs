using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using VocabularyTrainer2.Source.Common.Statistics;

namespace VocabularyTrainer2.Source.Common
{
    public class Utility
    {
        public static int CommonPadding<T>(IEnumerable<T> values)
        {
            var padding = 0;

            foreach (var value in values)
            {
                if (value != null)
                {
                    var stringRepresentation = value.ToString();

                    if (stringRepresentation != null)
                    {
                        if (stringRepresentation.Length > padding)
                        {
                            padding = stringRepresentation.Length;
                        }
                    }
                }
            }

            return padding;
        }

        public static int CommonPadding(FlashcardsStatistics statistics)
        {
            return new[] {
                $"{statistics.Practiced}",
                $"{statistics.PracticedSuccessfully}",
                $"{statistics.PracticedUnsuccessfully}"
            }.Max(v => v.Length);
        }

        public static void Space(int count)
        {
            Console.Write(" ".Repeat(count));
        }

        public static void Write(ConsoleColor color, string? value = null)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string? value = null)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

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

        public static void WriteBlue(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteBlueLine(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteYellow(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteYellowLine(string? value = null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ResetColor();
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
