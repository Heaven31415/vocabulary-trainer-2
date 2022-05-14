namespace VocabularyTrainer2.Source.Common
{
    public static class Extensions
    {
        public static int Digit(this int i, int n) => i / (int)Math.Pow(10, n - 1) % 10;
        public static string Capitalize(this string str) => string.Concat(str[..1].ToUpper(), str.AsSpan(1));
        public static bool IsCapitalized(this string str) => str == str.ToLower().Capitalize();
        public static bool IsLower(this string str) => str == str.ToLower();
    }
}
