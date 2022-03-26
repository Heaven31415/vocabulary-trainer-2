namespace VocabularyTrainer2
{
    internal class RandomFlashcardHelper
    {
        public int ParentId { get; set; }
        public Type Type { get; set; }
        public DateTime LastTrainingTime { get; set; }
        public TimeSpan Cooldown { get; set; }
        public List<string> Questions { get; set; } = new List<string>();
        public List<string> Answers { get; set; } = new List<string>();
    }
}
