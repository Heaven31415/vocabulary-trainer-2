using System.Security.Cryptography;
using System.Text;

namespace VocabularyTrainer2.Source.Common
{
    public static class Extensions
    {
        public static int Digit(this int i, int n) => i / (int)Math.Pow(10, n - 1) % 10;
        public static string Capitalize(this string str) => string.Concat(str[..1].ToUpper(), str.AsSpan(1));
        public static bool IsCapitalized(this string str) => str[0] == str.Capitalize()[0];
        public static bool IsLower(this string str) => str == str.ToLower();
        public static string Repeat(this string str, int n) => string.Concat(Enumerable.Repeat(str, n));
        public static string Hash(this string str)
        {
            using SHA256 hashAlgorithm = SHA256.Create();

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
