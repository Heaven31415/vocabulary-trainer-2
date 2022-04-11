namespace VocabularyTrainer2.Source.Common
{
    public static class Config
    {
        public const string ProgramName = "Vocabulary Trainer 2";
        public const int MinimalAdjectiveId = 0;
        public const int MinimalNounId = 100_000;
        public const int MinimalOtherId = 200_000;
        public const int MinimalVerbId = 300_000;
        public const string AdjectivesCsvFilePath = "Data/Adjectives.csv";
        public const string NounsCsvFilePath = "Data/Nouns.csv";
        public const string OthersCsvFilePath = "Data/Others.csv";
        public const string VerbsCsvFilePath = "Data/Verbs.csv";
        public const string VerbEndingsCacheFilePath = "Cache/VerbEndings.json";
        public const string VerbEndingsUrl = "https://conjugator.reverso.net/conjugation-german-verb";
        public const string SingleFlashcardsFilePath = "Data/SingleFlashcards.json";
        public const string MultiFlashcardsFilePath = "Data/MultiFlashcards.json";
        public const int MinimalFlashcardCooldownInDays = 1;
        public const int MaximalFlashcardCooldownInDays = 32;
        public const string CredentialsFilePath = "Credentials.json";
        public const string UserCredentialDirectoryPath = "User";
        public const string SpreadsheetKey = "1eP9-mlyuen3XezQn79EuPTQMMsponFWxlCxzK5qllMY";
        public static readonly string[] SpreadsheetNames = new string[] { "Adjectives", "Nouns", "Others", "Verbs" };
    }
}
