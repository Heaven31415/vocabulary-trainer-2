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
                            throw new IOException($"File '{FileName}' contains invalid content. Unable to deserialize it.");

                        _instance = config;
                    }
                }

                return _instance;
            }
        }

        // Developer Configurable Properties
        public readonly string ProgramName = "Vocabulary Trainer 2";
        public readonly string AdjectivesCsvFilePath = "Data/Adjectives.csv";
        public readonly string NounsCsvFilePath = "Data/Nouns.csv";
        public readonly string OthersCsvFilePath = "Data/Others.csv";
        public readonly string VerbsCsvFilePath = "Data/Verbs.csv";
        public readonly string VerbEndingsCacheFilePath = "Cache/VerbEndings.json";
        public readonly string FlashcardsFilePath = "Data/Flashcards.json";
        public readonly string CredentialsFilePath = "Credentials.json";
        public readonly string UserCredentialDirectoryPath = "User";

        // User Configurable Properties
        public bool OnlineMode { get; set; } = true;
        public string VerbEndingsUrl { get; set; } = "https://conjugator.reverso.net/conjugation-german-verb";
        public int InitialFlashcardCooldownInHours { get; set; } = 12;
        public int MinimalFlashcardCooldownInDays { get; set; } = 1;
        public int MaximalFlashcardCooldownInDays { get; set; } = 32;
        public string SpreadsheetKey { get; set; } = "1eP9-mlyuen3XezQn79EuPTQMMsponFWxlCxzK5qllMY";
        public string[] SpreadsheetNames { get; set; } = new string[] { "Adjectives", "Nouns", "Others", "Verbs" };
    }
}
