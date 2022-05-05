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

        public string VocabularyInformation => $"A:{_adjectives.Count} N:{_nouns.Count} O:{_others.Count} V:{_verbs.Count}";

        public string FlashcardInformation => $"F:{_flashcards.Count} A:{Available} A24:{AvailableAtDaysEnd}";

        public FlashcardSet()
        {
            _verbEndingsCache = new VerbEndingsCache(Config.Instance.VerbEndingsCacheFilePath);

            _adjectives = Adjective.ReadAdjectivesFromCsvFile(Config.Instance.AdjectivesCsvFilePath);
            _nouns = Noun.ReadNounsFromCsvFile(Config.Instance.NounsCsvFilePath);
            _others = Other.ReadOthersFromCsvFile(Config.Instance.OthersCsvFilePath);
            _verbs = Verb.ReadVerbsFromCsvFile(Config.Instance.VerbsCsvFilePath, _verbEndingsCache);

            _singleFlashcardSet = new SingleFlashcardSet(Config.Instance.SingleFlashcardsFilePath);
            _singleFlashcardSet.AddFlashcardsFromAdjectives(_adjectives);
            _singleFlashcardSet.AddFlashcardsFromNouns(_nouns);
            _singleFlashcardSet.AddFlashcardsFromOthers(_others);
            _singleFlashcardSet.AddFlashcardsFromVerbs(_verbs);

            _multiFlashcardSet = new MultiFlashcardSet(Config.Instance.MultiFlashcardsFilePath);
            _multiFlashcardSet.AddFlashcardsFromVerbs(_verbs);

            _flashcards = new List<Flashcard>();

            foreach (var f in _singleFlashcardSet.Flashcards)
                _flashcards.Add(f);

            foreach (var f in _multiFlashcardSet.Flashcards)
                _flashcards.Add(f);

            SaveToFileAsJson();
        }

        public void SaveToFileAsJson()
        {
            _singleFlashcardSet.SaveToFileAsJson();
            _multiFlashcardSet.SaveToFileAsJson();
        }

        public (Flashcard?, string) GetRandomFlashcard()
        {
            var availableFlashcards = _flashcards.FindAll(f => f.IsAvailable());

            if (availableFlashcards.Count == 0)
                return (null, "");

            var flashcard = availableFlashcards[_random.Next(availableFlashcards.Count)];
            var exampleSentence = GetExampleSentence(flashcard);

            return (flashcard, exampleSentence);
        }

        private string GetExampleSentence(Flashcard flashcard)
        {
            var parentId = flashcard.ParentId;

            if (Config.Instance.MinimalAdjectiveId <= parentId && parentId < Config.Instance.MinimalNounId)
            {
                var adjective = _adjectives.Find(a => a.Id == parentId);

                if (adjective == null)
                    throw new Exception($"Unable to find a matching adjective to a flashcard with {parentId} id.");

                return adjective.ExampleSentence;
            }
            else if (Config.Instance.MinimalNounId <= parentId && parentId < Config.Instance.MinimalOtherId)
            {
                var noun = _nouns.Find(a => a.Id == parentId);

                if (noun == null)
                    throw new Exception($"Unable to find a matching noun to a flashcard with {parentId} id.");

                return noun.ExampleSentence;
            }
            else if (Config.Instance.MinimalOtherId <= parentId && parentId < Config.Instance.MinimalVerbId)
            {
                var other = _others.Find(a => a.Id == parentId);

                if (other == null)
                    throw new Exception($"Unable to find a matching other to a flashcard with {parentId} id.");

                return other.ExampleSentence;
            }
            else
            {
                var verb = _verbs.Find(a => a.Id == parentId);

                if (verb == null)
                    throw new Exception($"Unable to find a matching verb to a flashcard with {parentId} id.");

                return verb.ExampleSentence;
            }
        }
    }
}
