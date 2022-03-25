namespace VocabularyTrainer2
{
    internal enum Type
    {
        NounSingularForm,
        NounPluralForm
    }

    internal abstract class FlashcardBase
    {
        public int ParentId { get; set; }
        public Type Type { get; set; }

        protected DateTime lastTrainingTime;
        protected TimeSpan cooldown;

        protected FlashcardBase(int parentId, Type type)
        {
            ParentId = parentId;
            Type = type;
            lastTrainingTime = DateTime.Now.AddDays(-1);
            cooldown = TimeSpan.FromDays(1);
        }

        public bool IsAvailable => DateTime.Now > lastTrainingTime.Add(cooldown);
        public abstract string Ask();
        public abstract (bool, string) Answer(string answer);
        public abstract string ComputeHash();
    }
}
