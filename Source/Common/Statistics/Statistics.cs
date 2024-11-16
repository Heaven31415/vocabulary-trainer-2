using VocabularyTrainer2.Source.Flashcard;
using VocabularyTrainer2.Source.Word;

namespace VocabularyTrainer2.Source.Common.Statistics
{
    public class Statistics
    {
        public int AdjectivesCount { get; }
        public int NounsCount { get; }
        public int OthersCount { get; }
        public int VerbsCount { get; }
        public int VocabularyCount { get; }
        public int FlashcardsCount { get; }
        public int FlashcardsAvailableNow { get; }
        public int FlashcardsAvailableNext24Hours { get; }
        public int FlashcardsAvailableNext7Days { get; }
        public FlashcardsStatistics FlashcardsToday { get; }
        public FlashcardsStatistics FlashcardsLast7Days { get; }
        public FlashcardsStatistics FlashcardsLast30Days { get; }
        public FlashcardsStatistics FlashcardsLifetime { get; }
        public Dictionary<TimeSpan, int> FlashcardsCooldowns { get; }
        public Dictionary<TimeSpan, float> FlashcardsCooldownsPercentages { get; }

        public Statistics(List<Adjective> adjectives, List<Noun> nouns, List<Other> others, List<Verb> verbs, FlashcardSet flashcardSet)
        {
            AdjectivesCount = adjectives.Count;
            NounsCount = nouns.Count;
            OthersCount = others.Count;
            VerbsCount = verbs.Count;
            VocabularyCount = adjectives.Count + nouns.Count + others.Count + verbs.Count;

            FlashcardsCount = flashcardSet.Flashcards.Count;
            FlashcardsAvailableNow = flashcardSet.Flashcards.FindAll(f => f.IsAvailable()).Count;
            FlashcardsAvailableNext24Hours = flashcardSet.Flashcards.FindAll(f => f.IsAvailable(DateTime.Now.AddHours(24))).Count;
            FlashcardsAvailableNext7Days = flashcardSet.Flashcards.FindAll(f => f.IsAvailable(DateTime.Now.AddDays(7))).Count;

            FlashcardsToday = new FlashcardsStatistics(flashcardSet.Flashcards, r => DateTime.Today <= r.Time && r.Time < DateTime.Today.AddDays(1));
            FlashcardsLast7Days = new FlashcardsStatistics(flashcardSet.Flashcards, r => DateTime.Today.AddDays(-7) <= r.Time && r.Time < DateTime.Today.AddDays(1));
            FlashcardsLast30Days = new FlashcardsStatistics(flashcardSet.Flashcards, r => DateTime.Today.AddDays(-30) <= r.Time && r.Time < DateTime.Today.AddDays(1));
            FlashcardsLifetime = new FlashcardsStatistics(flashcardSet.Flashcards, r => true);

            FlashcardsCooldowns = new Dictionary<TimeSpan, int>();

            foreach (var flashcard in flashcardSet.Flashcards)
            {
                if (FlashcardsCooldowns.ContainsKey(flashcard.Cooldown))
                    FlashcardsCooldowns[flashcard.Cooldown]++;
                else
                    FlashcardsCooldowns[flashcard.Cooldown] = 1;
            }

            FlashcardsCooldownsPercentages = new Dictionary<TimeSpan, float>();

            foreach (var (cooldown, count) in FlashcardsCooldowns)
            {
                FlashcardsCooldownsPercentages[cooldown] = 100f * count / flashcardSet.Flashcards.Count;
            }
        }
    }
}
