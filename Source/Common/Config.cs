using System.Text.Json;

namespace VocabularyTrainer2.Source.Common
{
    public class Config
    {
        private const string FileName = "Config.json";
        private static Config? _instance;

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!File.Exists(FileName))
                    {
                        _instance = new Config();
                        Utility.SaveToFileAsJson(FileName, _instance);
                    }
                    else
                    {
                        var json = File.ReadAllText(FileName);
                        var config = JsonSerializer.Deserialize<Config>(json);

                        if (config == null)
                            throw new IOException($"{FileName} contains invalid content. Unable to deserialize it.");

                        _instance = config;
                    }
                }

                return _instance;
            }
        }

        public string ProgramName { get; set; } = "Vocabulary Trainer 2";
        public int MinimalAdjectiveId { get; set; } = 0;
        public int MinimalNounId { get; set; } = 100_000;
        public int MinimalOtherId { get; set; } = 200_000;
        public int MinimalVerbId { get; set; } = 300_000;
        public string AdjectivesCsvFilePath { get; set; } = "Data/Adjectives.csv";
        public string NounsCsvFilePath { get; set; } = "Data/Nouns.csv";
        public string OthersCsvFilePath { get; set; } = "Data/Others.csv";
        public string VerbsCsvFilePath { get; set; } = "Data/Verbs.csv";
        public string VerbEndingsCacheFilePath { get; set; } = "Cache/VerbEndings.json";
        public string VerbEndingsUrl { get; set; } = "https://conjugator.reverso.net/conjugation-german-verb";
        public string FlashcardsFilePath { get; set; } = "Data/Flashcards.json";
        public int MinimalFlashcardCooldownInDays { get; set; } = 1;
        public int MaximalFlashcardCooldownInDays { get; set; } = 32;
        public string CredentialsFilePath { get; set; } = "Credentials.json";
        public string UserCredentialDirectoryPath { get; set; } = "User";
        public string SpreadsheetKey { get; set; } = "1eP9-mlyuen3XezQn79EuPTQMMsponFWxlCxzK5qllMY";
        public string[] SpreadsheetNames { get; set; } = new string[] { "Adjectives", "Nouns", "Others", "Verbs" };
    }
}
