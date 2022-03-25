using System.Text.Json.Serialization;

namespace VocabularyTrainer2
{
    internal enum Type
    {
        NounSingularForm,
        NounPluralForm,
        AdjectivePositiveDegree,
        AdjectiveComparativeDegree,
        AdjectiveSuperlativeDegree
    }

    internal abstract class FlashcardBase
    {
        public int ParentId { get; set; }
        public Type Type { get; set; }
        public DateTime LastTrainingTime { get; set; }
        public TimeSpan Cooldown { get; set; }

        protected FlashcardBase(int parentId, Type type)
        {
            ParentId = parentId;
            Type = type;
            LastTrainingTime = DateTime.Now.AddDays(-1);
            Cooldown = TimeSpan.FromDays(1);
        }

        [JsonIgnore]
        public bool IsAvailable => DateTime.Now > LastTrainingTime.Add(Cooldown);
        public abstract string AskQuestion();
        public abstract (bool, string) AnswerQuestion(string answer);
        public abstract string ComputeHash();
    }
}
