namespace VocabularyTrainer2.Source.Common
{
    public static class StringExtension
    {
        public static string Capitalize(this string str) => string.Concat(str[..1].ToUpper(), str.AsSpan(1));
    }
}
