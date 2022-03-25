using System.Security.Cryptography;
using System.Text;

namespace VocabularyTrainer2
{
    internal class Utility
    {
        public static void Write(string? value = null, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteLine(string? value = null, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static string ReadLine()
        {
            var line = Console.ReadLine();

            if (line == null)
                throw new Exception("Unable to read line from standard input.");

            return line;
        }

        public static string ComputeHash(string input)
        {
            using SHA256 hashAlgorithm = SHA256.Create();

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
