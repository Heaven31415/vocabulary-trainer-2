namespace VocabularyTrainer2.Source.Common
{
    public static class Config
    {
        public static string ProgramName { get; } = "Vocabulary Trainer 2";
        public static string AdjectivesCsvFilePath { get; } = "Data/Adjectives.csv";
        public static string NounsCsvFilePath { get; } = "Data/Nouns.csv";
        public static string OthersCsvFilePath { get; } = "Data/Others.csv";
        public static string VerbsCsvFilePath { get; } = "Data/Verbs.csv";
        public static string SingleFlashcardsFilePath { get; } = "Data/SingleFlashcards.json";
        public static string MultiFlashcardsFilePath { get; } = "Data/MultiFlashcards.json";
        public static string VerbEndingsCacheFilePath { get; } = "Cache/VerbEndings.json";
        public static string CredentialsFilePath { get; } = "Credentials.json";
        public static string UserCredentialDirectoryPath { get; } = "User";
        public static string SpreadsheetKey { get; } = "1eP9-mlyuen3XezQn79EuPTQMMsponFWxlCxzK5qllMY";
        public static string[] SpreadsheetNames { get; } = new string[] { "Adjectives", "Nouns", "Others", "Verbs" };
        public static int MinimalFlashcardCooldownInDays { get; } = 1;
        public static int MaximalFlashcardCooldownInDays { get; } = 32;
    }
}
