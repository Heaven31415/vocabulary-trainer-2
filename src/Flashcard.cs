using System.Text.Json.Serialization;

namespace VocabularyTrainer2
{
    internal enum Type
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

    internal abstract class Flashcard
    {
        public int ParentId { get; set; }
        public Type Type { get; set; }
        public DateTime LastTrainingTime { get; set; }
        public TimeSpan Cooldown { get; set; }

        protected Flashcard(int parentId, Type type)
        {
            ParentId = parentId;
            Type = type;
            LastTrainingTime = DateTime.Now.AddDays(-1);
            Cooldown = TimeSpan.FromDays(1);
        }

        [JsonIgnore]
        public bool IsAvailable => DateTime.Now > LastTrainingTime.Add(Cooldown);
        [JsonIgnore]
        public bool IsAvailableAtTheEndOfDay => DateTime.Today.AddDays(1) > LastTrainingTime.Add(Cooldown);
        public abstract string AskQuestion();
        public abstract (bool, string) AnswerQuestion(string answer);
        public abstract string ComputeHash();
    }
}
