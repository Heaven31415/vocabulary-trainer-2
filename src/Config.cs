namespace VocabularyTrainer2
{
    internal class Config
    {
        public static string ProgramName = "Vocabulary Trainer 2";

        public static string VerbsPath = "data/Verben.csv";
        public static string NounsPath = "data/Nomen.csv";
        public static string AdjectivesPath = "data/Adjektive.csv";

        public static string SingleFlashcardsPath = "data/SingleFlashcards.json";
        public static string MultiFlashcardsPath = "data/MultiFlashcards.json";
        public static string VerbCachePath = "cache/Verbs.json";
        public static string StatisticsPath = "data/Statistics.json";

        public static string CredentialsPath = "credentials.json";
        public static string UserCredentialPath = "user";
        public static string SpreadsheetKey = "1waGGcG56KU3hijXlUlZ2no4-mV75W6FCWYDwZ2qofeE";
        public static string[] SpreadsheetNames = new string[] { "Verben", "Nomen", "Adjektive" };
    }
}
