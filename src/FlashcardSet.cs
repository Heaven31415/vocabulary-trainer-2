namespace VocabularyTrainer2
{
    internal class FlashcardSet
    {
        private readonly List<Verb> verbs;
        private readonly List<Noun> nouns;
        private readonly List<Adjective> adjectives;

        private readonly List<SingleFlashcard> singleFlashcards;
        private readonly List<MultiFlashcard> multiFlashcards;

        private readonly List<Flashcard> flashcards = new();
        private readonly Random random = new();

        public FlashcardSet()
        {
            CSV.DownloadSheetsFromGoogleDriveFolder();

            Utility.WriteLine("Reading verbs from CSV file...");
            verbs = CSV.ReadVerbsFromFile(Config.VerbsPath);

            Utility.WriteLine("Reading nouns from CSV file...");
            nouns = CSV.ReadNounsFromFile(Config.NounsPath);

            Utility.WriteLine("Reading adjectives from CSV file...");
            adjectives = CSV.ReadAdjectivesFromFile(Config.AdjectivesPath);

            Utility.WriteLine("Loading single flashcards from file...");
            singleFlashcards = SingleFlashcardSet.Load(Config.SingleFlashcardsPath);

            Utility.WriteLine("Updating single flashcards...");
            SingleFlashcardSet.Update(verbs, singleFlashcards);
            SingleFlashcardSet.Update(nouns, singleFlashcards);
            SingleFlashcardSet.Update(adjectives, singleFlashcards);

            Utility.WriteLine("Loading multi flashcards from file...");
            multiFlashcards = MultiFlashcardSet.Load(Config.MultiFlashcardsPath);

            Utility.WriteLine("Updating multi flashcards...");
            MultiFlashcardSet.Update(verbs, multiFlashcards);

            foreach (var f in singleFlashcards)
                flashcards.Add(f);

            foreach (var f in multiFlashcards)
                flashcards.Add(f);

            Utility.WriteLine("Saving flashcards to files...");
            Save();

            Utility.WriteLine("Success!", ConsoleColor.Green);

            Utility.Write("Press enter to continue... ");
            Utility.ReadLine();
            Console.Clear();
        }

        public int Available => flashcards.FindAll(f => f.IsAvailable).Count;

        public int AvailableAtTheEndOfDay => flashcards.FindAll(f => f.IsAvailableAtTheEndOfDay).Count;

        public string GetVocabularyInformation()
        {
            return $"V<{verbs.Count}> N<{nouns.Count}> A<{adjectives.Count}>";
        }

        public string GetFlashcardInformation()
        {
            return $"F<{flashcards.Count}> A<{Available}> A24<{AvailableAtTheEndOfDay}>";
        }

        public Flashcard? GetRandomFlashcard()
        {
            var availableFlashcards = flashcards.FindAll(f => f.IsAvailable);

            if (availableFlashcards.Count == 0)
                return null;

            return availableFlashcards[random.Next(availableFlashcards.Count)];
        }

        public void Save()
        {
            SingleFlashcardSet.Save(Config.SingleFlashcardsPath, singleFlashcards);
            MultiFlashcardSet.Save(Config.MultiFlashcardsPath, multiFlashcards);
        }
    }
}
