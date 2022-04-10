namespace VocabularyTrainer2.Source.Flashcard
{
    public abstract class Flashcard
    {
        public enum FlashcardType
        {
            VerbPresentFirstSingular,
            VerbPresentSecondSingular,
            VerbPresentThirdSingular,
            VerbPresentFirstOrThirdPlural,
            VerbPresentSecondPlural,
            VerbSimplePastFirstSingular,
            VerbSimplePastSecondSingular,
            VerbSimplePastThirdSingular,
            VerbSimplePastFirstOrThirdPlural,
            VerbSimplePastSecondPlural,
            VerbPerfekt,
            NounSingularForm,
            NounPluralForm,
            AdjectivePositiveDegree,
            AdjectiveComparativeDegree,
            AdjectiveSuperlativeDegree
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
        public DateTime LastTrainingTime { get; set; } = DateTime.Now.AddDays(-1);
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
