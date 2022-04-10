using VocabularyTrainer2.Source.Common;
using VocabularyTrainer2.Source.Word;

namespace VocabularyTrainer2.Source.Flashcard
{
    public class FlashcardSet
    {
        private readonly VerbEndingsCache _verbEndingsCache;
        private readonly List<Adjective> _adjectives;
        private readonly List<Noun> _nouns;
        private readonly List<Other> _others;
        private readonly List<Verb> _verbs;
        private readonly SingleFlashcardSet _singleFlashcardSet;
        private readonly MultiFlashcardSet _multiFlashcardSet;
        private readonly List<Flashcard> _flashcards = new();
        private readonly Random _random = new();

        public int Available => _flashcards.FindAll(f => f.IsAvailable()).Count;

        public int AvailableAtDaysEnd => _flashcards.FindAll(f => f.IsAvailableAtDaysEnd()).Count;

        public string VocabularyInformation => $"V<{_verbs.Count}> N<{_nouns.Count}> A<{_adjectives.Count}>";

        public string FlashcardInformation => $"F<{_flashcards.Count}> A<{Available}> A24<{AvailableAtDaysEnd}>";

        public FlashcardSet()
        {
            _verbEndingsCache = new VerbEndingsCache(Config.VerbEndingsCacheFilePath);

            _adjectives = Adjective.ReadAdjectivesFromCsvFile(Config.AdjectivesCsvFilePath);
            _nouns = Noun.ReadNounsFromCsvFile(Config.NounsCsvFilePath);
            _others = Other.ReadOthersFromCsvFile(Config.OthersCsvFilePath);
            _verbs = Verb.ReadVerbsFromCsvFile(Config.VerbsCsvFilePath, _verbEndingsCache);

            _singleFlashcardSet = new SingleFlashcardSet(Config.SingleFlashcardsFilePath);
            _singleFlashcardSet.AddFlashcardsFromAdjectives(_adjectives);
            _singleFlashcardSet.AddFlashcardsFromNouns(_nouns);
            _singleFlashcardSet.AddFlashcardsFromOthers(_others);
            _singleFlashcardSet.AddFlashcardsFromVerbs(_verbs);

            _multiFlashcardSet = new MultiFlashcardSet(Config.MultiFlashcardsFilePath);
            _multiFlashcardSet.AddFlashcardsFromVerbs(_verbs);

            _flashcards = new List<Flashcard>();

            foreach (var f in _singleFlashcardSet.Flashcards)
                _flashcards.Add(f);

            foreach (var f in _multiFlashcardSet.Flashcards)
                _flashcards.Add(f);

            SaveToFileAsJson();
        }

        public Flashcard? GetRandomFlashcard()
        {
            var availableFlashcards = _flashcards.FindAll(f => f.IsAvailable());

            if (availableFlashcards.Count == 0)
                return null;

            return availableFlashcards[_random.Next(availableFlashcards.Count)];
        }

        public void SaveToFileAsJson()
        {
            _singleFlashcardSet.SaveToFileAsJson();
            _multiFlashcardSet.SaveToFileAsJson();
        }
    }
}
