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
            verbs = CSV.ReadVerbsFromFile(Config.VerbsPath);
            nouns = CSV.ReadNounsFromFile(Config.NounsPath);
            adjectives = CSV.ReadAdjectivesFromFile(Config.AdjectivesPath);

            singleFlashcards = SingleFlashcardSet.Load(Config.SingleFlashcardsPath);
            SingleFlashcardSet.Update(verbs, singleFlashcards);
            SingleFlashcardSet.Update(nouns, singleFlashcards);
            SingleFlashcardSet.Update(adjectives, singleFlashcards);

            multiFlashcards = MultiFlashcardSet.Load(Config.MultiFlashcardsPath);
            MultiFlashcardSet.Update(verbs, multiFlashcards);

            foreach (var f in singleFlashcards)
                flashcards.Add(f);

            foreach (var f in multiFlashcards)
                flashcards.Add(f);

            Save();
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
