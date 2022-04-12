namespace VocabularyTrainer2.Source.Flashcard
{
    public abstract class Flashcard
    {
        public enum FlashcardType
        {
            VerbPresentFirstSingular = 0,
            VerbPresentSecondSingular = 1,
            VerbPresentThirdSingular = 2,
            VerbPresentFirstOrThirdPlural = 3,
            VerbPresentSecondPlural = 4,
            VerbSimplePastFirstSingular = 5,
            VerbSimplePastSecondSingular = 6,
            VerbSimplePastThirdSingular = 7,
            VerbSimplePastFirstOrThirdPlural = 8,
            VerbSimplePastSecondPlural = 9,
            VerbPerfekt = 10,
            NounSingularForm = 11,
            NounPluralForm = 12,
            AdjectivePositiveDegree = 13,
            AdjectiveComparativeDegree = 14,
            AdjectiveSuperlativeDegree = 15,
            Other = 16,
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public DateTime Time { get; set; }

            public Result(bool isSuccessful, DateTime time)
            {
                IsSuccessful = isSuccessful;
                Time = time;
            }
        }

        public int ParentId { get; set; }
        public FlashcardType Type { get; set; }
        public DateTime LastTrainingTime { get; set; } = DateTime.Now;
        public TimeSpan Cooldown { get; set; } = TimeSpan.FromDays(1);
        public List<Result> Results { get; set; } = new List<Result>();

        protected Flashcard(int parentId, FlashcardType type)
        {
            ParentId = parentId;
            Type = type;
        }

        public bool IsAvailable() => DateTime.Now > LastTrainingTime.Add(Cooldown);
        public bool IsAvailableAtDaysEnd() => DateTime.Today.AddDays(1) > LastTrainingTime.Add(Cooldown);

        public abstract string AskQuestion();
        public abstract (bool, string) AnswerQuestion(string answer);
        public abstract string ComputeHash();
    }
}
